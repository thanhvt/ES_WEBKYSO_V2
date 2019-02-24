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
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using Telerik.Reporting.Processing;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class NhanVienKyBangKeController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/NhanVienKyBangKe/
        #region Trang chủ
        public ActionResult Index(FindModelGcs model)
        {
            ViewBag.Title = "Nhân viên ký bảng kê";
            var maDonviQuanly = new CommonUserProfile().MA_DVIQLY;
            ViewBag.MaDviQly = maDonviQuanly;

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS.ToString(),
                Text = x.TEN_DOI
            }).ToList();

            //int idLichGcs = string.IsNullOrEmpty(Request["idLichgcs"]) ? 0 : Convert.ToInt32(Request["idLichgcs"]);
            //int idLichGcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(x => x.STATUS_NVK == model.TrangThai).ID_LICHGCS;
            //var gcsLichgcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLichGcs);
            //ReportProcessor reportProcessor = new ReportProcessor();
            //InstanceReportSource instanceReportSource = new InstanceReportSource();
            ////rptBangKeChiSo report1 = new rptBangKeChiSo();
            //var report = new rptBangKeChiSo();
            //instanceReportSource.ReportDocument = report;

            //RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);
            //Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            //string pathQuery = myuri.PathAndQuery;
            //string hostName = myuri.ToString().Replace(pathQuery, "");

            ////if (gcsLichgcs != null)
            ////{
            //var maso = gcsLichgcs.MA_SOGCS;
            //var nam = gcsLichgcs.NAM;
            //var thang = gcsLichgcs.THANG;
            //var ky = gcsLichgcs.KY;
            //var maDvqLy = gcsLichgcs.MA_DVIQLY;
            //string fileName = "BangKeChiSo" + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
            //                             result.Extension;
            //string directoryPath = CommonHelper.UploadsPath + maDvqLy;
            //string filePath = directoryPath + "/" + fileName;
            //string str = @"Uploads/FilesHoSo/";
            //int index = filePath.LastIndexOf(str);
            //string fileCut = filePath.Substring(2, index + str.Length - 2);
            //if (Directory.Exists(Server.MapPath(directoryPath)))
            //{
            //    var a = filePath;
            //    @ViewBag.Link = hostName + "/" + fileCut + fileName;
            //}
            return View();
        }
        #endregion
        #region Load DataTable
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
            var roleId = Uow.RepoBase<WebpagesUsersInRoles>().GetOne(x => x.UserId == userId).RoleId;
            var getThuTuKy = Uow.RepoBase<CFG_BOPHAN_KY>().GetOne(x => x.RoleId == roleId).THU_TUKY;
            ////var tblGCS_LICHGCS = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll().Where(i=> i.MA_DVIQLY == findModel.MaDonVi || (i.NGAY_GHI == findModel.NgayGhi || i.MA_SOGCS == findModel.MaSo) || i.KY == findModel.Ky && i.THANG == findModel.Thang && i.NAM == findModel.Nam).ToList();
            var tblGCS_LICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            var tblLOAI_BANGKE_DONVI = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll();
            var tblGCS_BANGKE_LICH = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll();
            var tblFL_FILE = Uow.RepoBase<FL_FILE>().GetAll();
            var tblGCS_CHITIET_KY = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().GetAll();

            var result = (from gcsLichGcs in tblGCS_LICHGCS
                          join loaiBangkeDonvi in tblLOAI_BANGKE_DONVI
                          on gcsLichGcs.MA_DVIQLY equals loaiBangkeDonvi.MA_DVIQLY
                          into lstBangKeLich
                          from bangKeLich in lstBangKeLich.DefaultIfEmpty()
                          select new BANGKE_LICH()
                          {
                              ID_LICHGCS = gcsLichGcs.ID_LICHGCS,
                              MA_DVIQLY = gcsLichGcs.MA_DVIQLY,
                              MA_SOGCS = gcsLichGcs.MA_SOGCS,
                              TEN_SOGCS = gcsLichGcs.TEN_SOGCS,
                              HINH_THUC = gcsLichGcs.HINH_THUC,
                              NGAY_GHI = gcsLichGcs.NGAY_GHI,
                              KY = gcsLichGcs.KY,
                              THANG = gcsLichGcs.THANG,
                              NAM = gcsLichGcs.NAM,
                              MA_DOIGCS = gcsLichGcs.MA_DOIGCS,
                              USERID = gcsLichGcs.USERID,
                              FullName = gcsLichGcs.USERID == null ? "" : gcsLichGcs.FullName,
                              STATUS_NVK = gcsLichGcs.STATUS_NVK,
                              MA_BANGKELICH = (from a in tblGCS_BANGKE_LICH
                                               where a.MA_LOAIBANGKE == bangKeLich.MA_LOAIBANGKE
                  && a.ID_LICHGCS == gcsLichGcs.ID_LICHGCS
                                               select a.MA_BANGKELICH).FirstOrDefault(),
                              MA_LOAIBANGKE = bangKeLich.MA_LOAIBANGKE
                          }).ToList();

            //ktra thứ tự ký

            //nếu thứ tự ký (ThuTuKy) > 1 thì lọc theo tình trạng ký của thứ tự ThuTuKy-1

            foreach (BANGKE_LICH bkelich in result)
            {
                bkelich.TrangThaiKy = tblGCS_CHITIET_KY.Any(i => i.UserId == userId && i.MA_BANGKELICH == bkelich.MA_BANGKELICH.Value);
            }

            if (findModel.TrangThaiKy.HasValue)
                result = result.Where(i => i.TrangThaiKy == findModel.TrangThaiKy).ToList();




            paging.data = result;
            return Json(paging,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Vai trò GCS ký bảng kê
        /// <summary>
        /// Bước thực hiện ký
        ///1.getXML
        ///2.gán XML => Source
        ///3.Xuất file PDF
        ///4.Save file vào thư mục....
        ///5.Lưu thông tin vào bảng FL_File
        ///6.Tạo keySign 
        ///7.redirect lại trang để callApp ký
        /// </summary>
        /// <param name="ids">Mã lịch GCS</param>
        /// <param name="maLoaiBangKe">Mã loại bảng kê</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignPerform(List<int> ids, string maLoaiBangKe)
        {
            var model = new CommonJsonResult();
            var statusNvKy = "NVDK";
            var statusDtKy = "DTCK";
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
                    //var boPhanKy = UnitOfWork.RepoBase<>()
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
                            fileInfo = new FL_FILE();
                            fileInfo.FilePath = filePath;
                            fileInfo.FileHash = fileHash;
                            fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                            Uow.RepoBase<FL_FILE>().Create(fileInfo);
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
                                fileInfo.FilePath = filePath;
                                fileInfo.FileHash = fileHash;
                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                Uow.RepoBase<FL_FILE>().Update(fileInfo);
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

                    if (model.Data != null)
                    {


                        gcsLichgcs.STATUS_NVK = statusNvKy;
                        gcsLichgcs.STATUS_DTK = statusDtKy;
                        Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
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
        #endregion
        [HttpPost]
        public JsonResult GetDOI_GCS()
        {
            string username = User.Identity.Name;
            var listDOIGCS = new List<D_DOIGCS>();
            string MaDonVi = new CommonUserProfile().MA_DVIQLY;
            try
            {
                if (username == "administrator")
                {
                    listDOIGCS = Uow.RepoBase<D_DOIGCS>().GetAll().ToList();
                }
                else
                {
                    listDOIGCS = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MaDonVi).ToList();
                }
                List<DANHMUC> listDOI = new List<DANHMUC>();
                foreach (var item in listDOIGCS)
                {
                    DANHMUC dm = new DANHMUC();
                    dm.MAChar = item.MA_DOIGCS;
                    dm.TEN = item.TEN_DOI;
                    listDOI.Add(dm);
                }
                return Json(listDOI, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [HttpPost]
        public ActionResult GetLoaiBangKe()
        {
            try
            {
                var maDonviQuanly = new CommonUserProfile().MA_DVIQLY;
                ViewBag.MaDviQly = maDonviQuanly;

                var lstMaLoaiBKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll(x => x.MA_DVIQLY == maDonviQuanly).ToList();
                List<DANHMUC> listDM = new List<DANHMUC>();
                foreach (var VARIABLE in lstMaLoaiBKe)
                {
                    var dm = new DANHMUC();
                    dm.MAChar = VARIABLE.MA_LOAIBANGKE;
                    dm.TEN = Uow.RepoBase<D_LOAI_BANGKE>().GetOne(o => o.MA_LOAIBANGKE == dm.MAChar).TEN_LOAIBANGKE;
                    listDM.Add(dm);
                }
                return Json(listDM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetReport(int IdLich,string MaBangKe)
        {
            ViewBag.IdLich = IdLich;
            ViewBag.MaBangKe = MaBangKe;
            return View();
        }
    }
}
