using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Helpers;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using esDigitalSignature;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Common.DTO;
using ES_WEBKYSO.Reports;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class DoiTruongKyBangKeController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/DoiTruongKyBangKe/

        public ActionResult Index()
        {
            ViewBag.Title = "Đội trưởng ký bảng kê";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS.ToString(),
                Text = x.TEN_DOI
            }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewBangKe(int ID_LICHGCS)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignPerformDoiTruongKy(List<int> ids, string maLoaiBangKe)
        {
            var model = new CommonJsonResult();
            //var status = "DK";
            var statusDtk = "DTDK";
            var statusDhk = "DHCK";
            var cad = new CA_DataSign();
            try
            {
                foreach (var idLichgcs in ids)
                {
                    //Create GCS_BANGKE_LICH
                    var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLichgcs);
                    var gcsSoGcs = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_SOGCS == gcsLichgcs.MA_SOGCS);

                    var gcsBangkeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetOne(i => i.ID_LICHGCS == idLichgcs
                                                                                    && i.MA_LOAIBANGKE == maLoaiBangKe);
                    if (gcsBangkeLich == null)
                    {
                        gcsBangkeLich = new GCS_BANGKE_LICH();
                        gcsBangkeLich.ID_LICHGCS = idLichgcs;
                        gcsBangkeLich.MA_LOAIBANGKE = maLoaiBangKe;
                        Uow.RepoBase<GCS_BANGKE_LICH>().Create(gcsBangkeLich);
                    }
                    FL_FILE fileInfo = Uow.RepoBase<FL_FILE>().GetOne(i => i.MA_BANGKELICH == gcsBangkeLich.MA_BANGKELICH);

                    //1.getXML
                    //2.gán XML => Source
                    //3.Xuất file PDF
                    //4.Save file vào thư mục....
                    //5.Lưu thông tin vào bảng FL_File
                    //6.Tạo keySign 
                    //7.redirect lại trang để callApp ký
                    if (fileInfo == null)
                    {
                        var maso = gcsLichgcs.MA_SOGCS;
                        var nam = gcsLichgcs.NAM;
                        var thang = gcsLichgcs.THANG;
                        var ky = gcsLichgcs.KY;
                        var maDvqLy = gcsLichgcs.MA_DVIQLY;

                        //2.gán XML => Source 
                        object dsSo = _bangKeHelper.GetBangKeChiSo(idLichgcs);

                        //3 Xuất file PDF
                        ReportProcessor reportProcessor = new ReportProcessor();
                        InstanceReportSource instanceReportSource;
                        instanceReportSource = new InstanceReportSource();
                        var report = new rptBangKeChiSo();
                        report.SetSourceTable(dsSo);
                        instanceReportSource.ReportDocument = report;
                        RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                        //4.Save file vào thư mục....
                        string fileName = "BangKeChiSo" + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                          result.Extension;
                        string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                        string filePath = directoryPath + "/" + fileName;
                        if (!Directory.Exists(Server.MapPath(directoryPath)))
                            Directory.CreateDirectory(Server.MapPath(directoryPath));

                        using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                        {
                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                        }

                        //5.Lưu thông tin vào bảng FL_File
                        //var fileInfo = new FL_File();
                        //ToDo: tannv sửa lại cách lấy FileInfo
                        using (
                            ESDigitalSignatureManager dsm =
                                new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePath)),
                                    Path.GetExtension(filePath)))
                        {
                            var fileHash = dsm.GetHashValue();
                            if (fileInfo == null)
                            {
                                fileInfo = new FL_FILE();
                                fileInfo.FilePath = filePath;
                                fileInfo.FileHash = fileHash;
                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                Uow.RepoBase<FL_FILE>().Create(fileInfo);
                            }
                            else
                            {
                                fileInfo.FilePath = filePath;
                                fileInfo.FileHash = fileHash;
                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                Uow.RepoBase<FL_FILE>().Update(fileInfo);
                            }
                        }
                    }
                    else
                    {
                        var mapPath = Server.MapPath(fileInfo.FilePath);
                        if (mapPath == null)
                        {
                            //1 get XML
                            var maso = gcsLichgcs.MA_SOGCS;
                            var nam = gcsLichgcs.NAM;
                            var thang = gcsLichgcs.THANG;
                            var ky = gcsLichgcs.KY;
                            var maDvqLy = gcsLichgcs.MA_DVIQLY;

                            //2.gán XML => Source 
                            object dsSo = _bangKeHelper.GetBangKeChiSo(idLichgcs);

                            //3 Xuất file PDF
                            ReportProcessor reportProcessor = new ReportProcessor();
                            InstanceReportSource instanceReportSource;
                            instanceReportSource = new InstanceReportSource();
                            var report = new rptBangKeChiSo();
                            report.SetSourceTable(dsSo);
                            instanceReportSource.ReportDocument = report;
                            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                            //4.Save file vào thư mục....
                            string fileName = "BangKeChiSo" + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                              result.Extension;
                            string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                            string filePath = directoryPath + "/" + fileName;
                            if (!Directory.Exists(Server.MapPath(directoryPath)))
                                Directory.CreateDirectory(Server.MapPath(directoryPath));
                            using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                            {
                                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                            }

                            //5.Lưu thông tin vào bảng FL_File
                            //var fileInfo = new FL_File();
                            //ToDo: tannv sửa lại cách lấy FileInfo
                            using (
                                ESDigitalSignatureManager dsm =
                                    new ESDigitalSignatureManager(
                                        System.IO.File.ReadAllBytes(Server.MapPath(filePath)),
                                        Path.GetExtension(filePath)))
                            {
                                var fileHash = dsm.GetHashValue();
                                if (fileInfo == null)
                                {
                                    fileInfo = new FL_FILE();
                                    fileInfo.FilePath = filePath;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                }
                                else
                                {
                                    fileInfo.FilePath = filePath;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Update(fileInfo);
                                }
                            }

                        }
                    }



                    //6.Tạo keySign
                    cad.KeySign = Guid.NewGuid().ToString();
                    cad.FileID = fileInfo.FileID;
                    cad.FilePath = fileInfo.FilePath;
                    cad.UserSign = User.Identity.Name;
                    cad.SignTime = DateTime.Now;
                    cad.MethodName = "NhanVienKyBangKe_Oncompleted";// = "ChuyenHoSo_DoiTruong";
                    Uow.RepoBase<CA_DataSign>().Create(cad);

                    //7.trả kết quả để ajax gọi application ký

                    model.Data = new CadDTO
                    {
                        KeySign = cad.KeySign,
                        CallApp = true,
                        CAVersion = ConfigurationManager.AppSettings["CAVersion"]
                    };

                    if (model != null)
                    {
                        gcsLichgcs.STATUS_DTK = statusDtk;
                        gcsLichgcs.STATUS_DHK = statusDhk;
                        //gcsSoGcs.STATUS_DTK = statusDtk;
                        //gcsSoGcs.STATUS_DHK = statusDhk;
                        Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                        //UnitOfWork.RepoBase<D_SOGCS>().Update(gcsSoGcs);
                        model.Message = "Kiểm tra chương trình chữ ký số!";
                        model.Result = true;
                    }
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                model.Message = ex.Message + "\rn" + ex.StackTrace;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
