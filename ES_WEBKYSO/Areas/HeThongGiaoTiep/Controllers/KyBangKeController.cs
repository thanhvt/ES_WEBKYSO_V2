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
    public class KyBangKeController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/KyBangKe/
        /// <summary>
        /// Giao diện form phòng kinh doanh ký bảng kê
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Phòng kinh doanh ký bảng kê";
            var dmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
            ViewBag.MaDviQly = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == dmId).DepartmentCode;

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS.ToString(),
                Text = x.TEN_DOI
            }).ToList();
            return View();
        }

        public ActionResult NhanVienKyBangKe()
        {
            ViewBag.Title = "Nhân viên ký bảng kê";
            var usdmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name);
            var dmId = usdmId.DepartmentId;
            var MaDviQly = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == dmId).DepartmentCode;

            ViewBag.MaDviQly = MaDviQly;

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(x => x.MA_DVIQLY == MaDviQly && x.ListCFG_DOIGCS_NVIEN.Any(y => y.USERID == usdmId.UserId)).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS.ToString(),
                Text = x.TEN_DOI,
                Selected = true
            }).ToList();
            return View();
        }

        public ActionResult DoiTruongKyBangKe()
        {
            ViewBag.Title = "Đội trưởng ký bảng kê";
            var usdmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name);
            var dmId = usdmId.DepartmentId;
            var MaDviQly = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == dmId).DepartmentCode;


            //ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll().ToList().Select(x => new SelectListItem
            //{
            //    Value = x.MA_DOIGCS.ToString(),
            //    Text = x.TEN_DOI
            //}).ToList();
            //return View();
            ViewBag.MaDviQly = MaDviQly;

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(x => x.MA_DVIQLY == MaDviQly && x.ListCFG_DOIGCS_NVIEN.Any(y => y.USERID == usdmId.UserId)).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS.ToString(),
                Text = x.TEN_DOI,
                Selected = true
            }).ToList();
            return View();
        }

        public ActionResult DieuHanhKyBangKe()
        {
            ViewBag.Title = "Điều hành GCS ký bảng kê";
            var dmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
            ViewBag.MaDviQly = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == dmId).DepartmentCode;

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
            var paging = Request.Params.ToPaging("ID_LICHGCS");
            var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
            var roleId = Uow.RepoBase<WebpagesUsersInRoles>().GetOne(x => x.UserId == userId).RoleId;

            var tblGCS_LICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //var tblGCS_LICHGCS = Uow.RepoBase<GCS_LICHGCS>().GETALL().ToList();
            var tblLOAI_BANGKE_DONVI = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll();
            var tblGCS_BANGKE_LICH = Uow.RepoBase<GCS_BANGKE_LICH>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

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
                                               where a.MA_LOAIBANGKE == bangKeLich.MA_LOAIBANGKE && a.ID_LICHGCS == gcsLichGcs.ID_LICHGCS
                                               select a.MA_BANGKELICH).FirstOrDefault(),
                              MA_LOAIBANGKE = bangKeLich.MA_LOAIBANGKE

                          }).ToList();


            foreach (BANGKE_LICH bkl in result)
            {
                if (bkl.MA_LOAIBANGKE != null)
                {
                    bkl.File = (from a in tblFL_FILE where a.MA_BANGKELICH == bkl.MA_BANGKELICH select a.FilePath).FirstOrDefault();
                    ViewBag.Link = bkl.File;
                }
            }
            //ktra thứ tự ký
            //nếu thứ tự ký (ThuTuKy) > 1 thì lọc theo tình trạng ký của thứ tự ThuTuKy-1
            result = GetByPrevSignOrder(result, roleId);

            foreach (BANGKE_LICH bkelich in result)
            {
                bkelich.TrangThaiKy = tblGCS_CHITIET_KY.Any(i => i.UserId == userId && i.MA_BANGKELICH == bkelich.MA_BANGKELICH.Value);
            }
            if (findModel.TrangThaiKy.HasValue)
                result = result.Where(i => i.TrangThaiKy == findModel.TrangThaiKy && i.MA_LOAIBANGKE == findModel.MaLoaiBangKe).ToList();

            paging.data = result;

            //return Json(paging.data, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                data = paging.data,
                draw = paging.draw,
                Key = paging.Key,
                OrderDirection = paging.OrderDirection,
                OrderKey = paging.OrderKey,
                recordsFiltered = paging.recordsFiltered,
                recordsTotal = paging.recordsTotal,
                Skip = paging.Skip,
                Take = paging.Take
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy ra danh sách bảng kê có thể thao tác của người dùng theo role và thứ tự ký
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<BANGKE_LICH> GetByPrevSignOrder(List<BANGKE_LICH> result, int roleId)
        {
            var tblBOPHAN_KY = Uow.RepoBase<CFG_BOPHAN_KY>().GetAll();

            for (int index = result.Count - 1; index >= 0; index--)
            {
                BANGKE_LICH bkelich = result[index];
                var bphanKy =
                    tblBOPHAN_KY.FirstOrDefault(i => i.MA_LOAIBANGKE == bkelich.MA_LOAIBANGKE && i.RoleId == roleId);
                if (bphanKy != null)
                    bkelich.ThuTuKy = bphanKy.THU_TUKY;

                //SetTinhTrangKy(bkelich);
                var prevTinhTrangKy = GetPrevUserSign(bkelich, roleId);
                if (!prevTinhTrangKy)
                {
                    result.RemoveAt(index);
                }
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra trong bảng GCS_CHITIET_KY xem có user nào thuộc role trước đó đã ký bảng kê hay chưa
        /// </summary>
        /// <param name="bkelich"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private bool GetPrevUserSign(BANGKE_LICH bkelich, int roleId)
        {
            var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
            var maLoaiBke = bkelich.MA_LOAIBANGKE;
            var madviqly = bkelich.MA_DVIQLY;
            string maDoi = "";

            CFG_BOPHAN_KY prevSign;
            int prevRoleId;

            var allRolesSignMaLoaiBke =
                Uow.RepoBase<CFG_BOPHAN_KY>().GetAll(i => i.MA_LOAIBANGKE == maLoaiBke && i.MA_DVIQLY == madviqly).OrderBy(i => i.THU_TUKY).ToList();
            var orderSign = allRolesSignMaLoaiBke.FindIndex(i => i.RoleId == roleId);
            if (orderSign == 0)//nếu là người ký đầu tiên
            {

                return true;
            }
            if (orderSign == -1)//chưa được cấu hình ký bảng kê này
            {
                //todo: trả thông báo ra là User này chưa được cấu hình để ký bảng kê. (bảng sdfsdsdfsdfdf)
                return false;
            }
            prevSign = allRolesSignMaLoaiBke[orderSign - 1];
            prevRoleId = prevSign.RoleId;

            var sogcsNvien = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(i => i.USERID == userId);
            if (sogcsNvien != null)
                maDoi = sogcsNvien.MA_DOIGCS;

            var allUserByRoleId_MaDoiMaDonVi =//tim` userId
                Uow.RepoBase<WebpagesUsersInRoles>().GetAll().Where(i => i.RoleId == prevRoleId).Select(i => i.UserId).ToList();
            allUserByRoleId_MaDoiMaDonVi =
                allUserByRoleId_MaDoiMaDonVi.Where(i => Uow.RepoBase<CFG_DOIGCS_NVIEN>().Any(j => j.MA_DVIQLY == madviqly)).ToList();

            if (!string.IsNullOrEmpty(maDoi))
            {
                allUserByRoleId_MaDoiMaDonVi =
                    allUserByRoleId_MaDoiMaDonVi.Where(i => Uow.RepoBase<CFG_DOIGCS_NVIEN>().Any(j => j.MA_DOIGCS == maDoi)).ToList();
            }

            var tblGCS_CHITIET_KY = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().GetAll();
            var isSigned = tblGCS_CHITIET_KY.Any(i => allUserByRoleId_MaDoiMaDonVi.Contains(i.UserId) && i.MA_BANGKELICH == bkelich.MA_BANGKELICH.Value);

            return isSigned;
        }

        public ActionResult ViewBangKe(int ID_LICHGCS)
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignPerformDieuHanhKy(List<int> ids, string maLoaiBangKe)
        {
            var model = new CommonJsonResult();
            //var status = "DK";
            var statusNvK = "NVDK";
            var statusDvcm = "CDVC";
            var cad = new CA_DataSign();
            string MaQuyen;
            string NgayGcs;
            try
            {
                foreach (var idLichgcs in ids)
                {
                    //Create GCS_BANGKE_LICH
                    var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLichgcs);
                    var gcsSoGcs = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_SOGCS == gcsLichgcs.MA_SOGCS);

                    var gcsBangkeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetOne(i => i.ID_LICHGCS == idLichgcs
                                                                                    && i.MA_LOAIBANGKE == maLoaiBangKe);

                    // Check đã đối soát hay chưa
                    var lstDoiSoat = Uow.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_QUYEN == gcsLichgcs.MA_SOGCS
                                        && x.STR_CHECK_DSOAT == "CHUA_DOI_SOAT").ToList();
                    if (lstDoiSoat.Count != 0)
                    {
                        model.Message = "Sổ " + gcsLichgcs.MA_SOGCS + " đối soát chưa thành công. Vui lòng đối soát hết rồi" +
                                        " tiến hành ký";
                        model.Result = true;
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }

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
                        //ReportProcessor reportProcessor = new ReportProcessor();
                        //var instanceReportSource = new InstanceReportSource();
                        //var report = new rptBangKeChiSo();
                        //report.SetSourceTable(dsSo);
                        //instanceReportSource.ReportDocument = report;

                        ReportHelper reportHelper = new ReportHelper(Uow);
                        InstanceReportSource instanceReportSource = new InstanceReportSource();
                        ReportProcessor reportProcessor = new ReportProcessor();
                        MaQuyen = reportHelper.getMaQuyen(idLichgcs);
                        NgayGcs = reportHelper.getNgayGcs(idLichgcs);
                        try
                        {
                            switch (maLoaiBangKe)
                            {
                                case "BKCS":
                                    rptBangKeChiSo reportBangKeChiSo = new rptBangKeChiSo();
                                    var rptBkcs = new rptBangKeChiSo();

                                    instanceReportSource.ReportDocument = rptBkcs;

                                    var sourceBangKeChiSo = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceBangKeChiSo != null)
                                    {
                                        reportBangKeChiSo.SetSourceTable(sourceBangKeChiSo);
                                        reportBangKeChiSo.SetParamater(NgayGcs, MaQuyen);
                                    }
                                    instanceReportSource.ReportDocument = reportBangKeChiSo.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultBkcs = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                    //4.Save file vào thư mục....
                                    string fileName = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                      resultBkcs.Extension;
                                    string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                                    string filePath = directoryPath + "/" + fileName;
                                    if (!Directory.Exists(Server.MapPath(directoryPath)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPath));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                                    {
                                        fs.Write(resultBkcs.DocumentBytes, 0, resultBkcs.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
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
                                    }
                                    break;
                                case "TTBT":
                                    rptTinhTrangBatThuong reportBangKeTinhTrangBatThuong = new rptTinhTrangBatThuong();
                                    var rptTtbt = new rptTinhTrangBatThuong();

                                    instanceReportSource.ReportDocument = rptTtbt;

                                    var sourceTinhTrangBatThuong = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceTinhTrangBatThuong != null)
                                    {
                                        reportBangKeTinhTrangBatThuong.SetSourceTable(sourceTinhTrangBatThuong);
                                    }
                                    instanceReportSource.ReportDocument = reportBangKeTinhTrangBatThuong.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultTtbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                    //4.Save file vào thư mục....
                                    string fileNameTtbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                      resultTtbt.Extension;
                                    string directoryPathTtbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                                    {
                                        fs.Write(resultTtbt.DocumentBytes, 0, resultTtbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathTtbt)),
                                                Path.GetExtension(filePathTtbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        if (fileInfo == null)
                                        {
                                            fileInfo = new FL_FILE();
                                            fileInfo.FilePath = filePathTtbt;
                                            fileInfo.FileHash = fileHash;
                                            fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                            Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                        }
                                    }
                                    break;
                                case "SLBT":
                                    rptSanLuongBatThuong reportSlbt = new rptSanLuongBatThuong();
                                    var rptSlbt = new rptSanLuongBatThuong();

                                    instanceReportSource.ReportDocument = rptSlbt;

                                    var sourceSlbt = reportHelper.get_SLBT(idLichgcs);
                                    if (sourceSlbt != null)
                                    {
                                        reportSlbt.SetSourceTable(sourceSlbt);
                                    }
                                    instanceReportSource.ReportDocument = reportSlbt.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultSlbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                    //4.Save file vào thư mục....
                                    string filenameSlbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                      resultSlbt.Extension;
                                    string directoryPathSlbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathSlbt = directoryPathSlbt + "/" + filenameSlbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathSlbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathSlbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathSlbt), FileMode.Create))
                                    {
                                        fs.Write(resultSlbt.DocumentBytes, 0, resultSlbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathSlbt)),
                                                Path.GetExtension(filePathSlbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        if (fileInfo == null)
                                        {
                                            fileInfo = new FL_FILE();
                                            fileInfo.FilePath = filePathSlbt;
                                            fileInfo.FileHash = fileHash;
                                            fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                            Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                        }
                                    }
                                    break;
                                case "ASLBT":
                                    rptAnhSanLuongBatThuong reportAslbt = new rptAnhSanLuongBatThuong();
                                    var rptAslbt = new rptAnhSanLuongBatThuong();

                                    instanceReportSource.ReportDocument = rptAslbt;

                                    var sourceAslbt = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceAslbt != null)
                                    {
                                        reportAslbt.SetSourceTable(sourceAslbt);
                                    }
                                    instanceReportSource.ReportDocument = reportAslbt.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultAslbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                    //4.Save file vào thư mục....
                                    string fileNameAslbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                      resultAslbt.Extension;
                                    string directoryPathAslbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathAslbt = directoryPathAslbt + "/" + fileNameAslbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathAslbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathAslbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathAslbt), FileMode.Create))
                                    {
                                        fs.Write(resultAslbt.DocumentBytes, 0, resultAslbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathAslbt)),
                                                Path.GetExtension(filePathAslbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        if (fileInfo == null)
                                        {
                                            fileInfo = new FL_FILE();
                                            fileInfo.FilePath = filePathAslbt;
                                            fileInfo.FileHash = fileHash;
                                            fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                            Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        var mapPath = Server.MapPath(fileInfo.FilePath);
                        if (mapPath == null)
                        {
                            var maso = gcsLichgcs.MA_SOGCS;
                            var nam = gcsLichgcs.NAM;
                            var thang = gcsLichgcs.THANG;
                            var ky = gcsLichgcs.KY;
                            var maDvqLy = gcsLichgcs.MA_DVIQLY;

                            //2.gán XML => Source 
                            object dsSo = _bangKeHelper.GetBangKeChiSo(idLichgcs);

                            //3 Xuất file PDF
                            ReportHelper reportHelper = new ReportHelper(Uow);
                            InstanceReportSource instanceReportSource = new InstanceReportSource();
                            ReportProcessor reportProcessor = new ReportProcessor();
                            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);
                            MaQuyen = reportHelper.getMaQuyen(idLichgcs);
                            NgayGcs = reportHelper.getNgayGcs(idLichgcs);
                            try
                            {
                                switch (maLoaiBangKe)
                                {
                                    case "BKCS":
                                        rptBangKeChiSo reportBangKeChiSo = new rptBangKeChiSo();
                                        var rptBkcs = new rptBangKeChiSo();
                                        instanceReportSource.ReportDocument = rptBkcs;

                                        var sourceBangKeChiSo = reportHelper.getReportBkcs(idLichgcs);
                                        if (sourceBangKeChiSo != null)
                                        {
                                            reportBangKeChiSo.SetSourceTable(sourceBangKeChiSo);
                                            reportBangKeChiSo.SetParamater(NgayGcs, MaQuyen);
                                        }
                                        instanceReportSource.ReportDocument = reportBangKeChiSo.Report;
                                        //rptTong.ReportSource = instanceReportSource;
                                        RenderingResult resultBkcs = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                        //4.Save file vào thư mục....
                                        string fileName = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                          resultBkcs.Extension;
                                        string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                                        string filePath = directoryPath + "/" + fileName;
                                        if (!Directory.Exists(Server.MapPath(directoryPath)))
                                            Directory.CreateDirectory(Server.MapPath(directoryPath));
                                        using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                                        {
                                            fs.Write(resultBkcs.DocumentBytes, 0, resultBkcs.DocumentBytes.Length);
                                        }
                                        //5.Lưu thông tin vào bảng FL_File
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
                                        }
                                        break;
                                    case "TTBT":
                                        rptTinhTrangBatThuong reportBangKeTinhTrangBatThuong = new rptTinhTrangBatThuong();
                                        var rptTtbt = new rptTinhTrangBatThuong();

                                        instanceReportSource.ReportDocument = rptTtbt;

                                        var sourceTinhTrangBatThuong = reportHelper.getReportBkcs(idLichgcs);
                                        if (sourceTinhTrangBatThuong != null)
                                        {
                                            reportBangKeTinhTrangBatThuong.SetSourceTable(sourceTinhTrangBatThuong);
                                        }
                                        instanceReportSource.ReportDocument = reportBangKeTinhTrangBatThuong.Report;
                                        //rptTong.ReportSource = instanceReportSource;
                                        RenderingResult resultTtbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                        //4.Save file vào thư mục....
                                        string fileNameTtbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                          resultTtbt.Extension;
                                        string directoryPathTtbt = CommonHelper.UploadsPath + maDvqLy;
                                        string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                                        if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                                            Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                                        using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                                        {
                                            fs.Write(resultTtbt.DocumentBytes, 0, resultTtbt.DocumentBytes.Length);
                                        }
                                        //5.Lưu thông tin vào bảng FL_File
                                        using (
                                            ESDigitalSignatureManager dsm =
                                                new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathTtbt)),
                                                    Path.GetExtension(filePathTtbt)))
                                        {
                                            var fileHash = dsm.GetHashValue();
                                            if (fileInfo == null)
                                            {
                                                fileInfo = new FL_FILE();
                                                fileInfo.FilePath = filePathTtbt;
                                                fileInfo.FileHash = fileHash;
                                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                                Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                            }
                                        }
                                        break;
                                    case "SLBT":
                                        rptSanLuongBatThuong reportSlbt = new rptSanLuongBatThuong();
                                        var rptSlbt = new rptSanLuongBatThuong();

                                        instanceReportSource.ReportDocument = rptSlbt;

                                        var sourceSlbt = reportHelper.get_SLBT(idLichgcs);
                                        if (sourceSlbt != null)
                                        {
                                            reportSlbt.SetSourceTable(sourceSlbt);
                                        }
                                        instanceReportSource.ReportDocument = reportSlbt.Report;
                                        //rptTong.ReportSource = instanceReportSource;
                                        RenderingResult resultSlbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                        //4.Save file vào thư mục....
                                        string filenameSlbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                          resultSlbt.Extension;
                                        string directoryPathSlbt = CommonHelper.UploadsPath + maDvqLy;
                                        string filePathSlbt = directoryPathSlbt + "/" + filenameSlbt;
                                        if (!Directory.Exists(Server.MapPath(directoryPathSlbt)))
                                            Directory.CreateDirectory(Server.MapPath(directoryPathSlbt));
                                        using (FileStream fs = new FileStream(Server.MapPath(filePathSlbt), FileMode.Create))
                                        {
                                            fs.Write(resultSlbt.DocumentBytes, 0, resultSlbt.DocumentBytes.Length);
                                        }
                                        //5.Lưu thông tin vào bảng FL_File
                                        using (
                                            ESDigitalSignatureManager dsm =
                                                new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathSlbt)),
                                                    Path.GetExtension(filePathSlbt)))
                                        {
                                            var fileHash = dsm.GetHashValue();
                                            if (fileInfo == null)
                                            {
                                                fileInfo = new FL_FILE();
                                                fileInfo.FilePath = filePathSlbt;
                                                fileInfo.FileHash = fileHash;
                                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                                Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                            }
                                        }
                                        break;
                                    case "ASLBT":
                                        rptAnhSanLuongBatThuong reportAslbt = new rptAnhSanLuongBatThuong();
                                        var rptAslbt = new rptAnhSanLuongBatThuong();

                                        instanceReportSource.ReportDocument = rptAslbt;

                                        var sourceAslbt = reportHelper.get_SLBT(idLichgcs);
                                        if (sourceAslbt != null)
                                        {
                                            reportAslbt.SetSourceTable(sourceAslbt);
                                        }
                                        instanceReportSource.ReportDocument = reportAslbt.Report;
                                        //rptTong.ReportSource = instanceReportSource;
                                        RenderingResult resultAslbt = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                                        //4.Save file vào thư mục....
                                        string fileNameAslbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang + "_" + ky + "." +
                                                          resultAslbt.Extension;
                                        string directoryPathAslbt = CommonHelper.UploadsPath + maDvqLy;
                                        string filePathAslbt = directoryPathAslbt + "/" + fileNameAslbt;
                                        if (!Directory.Exists(Server.MapPath(directoryPathAslbt)))
                                            Directory.CreateDirectory(Server.MapPath(directoryPathAslbt));
                                        using (FileStream fs = new FileStream(Server.MapPath(filePathAslbt), FileMode.Create))
                                        {
                                            fs.Write(resultAslbt.DocumentBytes, 0, resultAslbt.DocumentBytes.Length);
                                        }
                                        //5.Lưu thông tin vào bảng FL_File
                                        using (
                                            ESDigitalSignatureManager dsm =
                                                new ESDigitalSignatureManager(System.IO.File.ReadAllBytes(Server.MapPath(filePathAslbt)),
                                                    Path.GetExtension(filePathAslbt)))
                                        {
                                            var fileHash = dsm.GetHashValue();
                                            if (fileInfo == null)
                                            {
                                                fileInfo = new FL_FILE();
                                                fileInfo.FilePath = filePathAslbt;
                                                fileInfo.FileHash = fileHash;
                                                fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                                Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
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
                    model.Message = "Kiểm tra chương trình chữ ký số!";
                    model.Result = true;
                    gcsLichgcs.STATUS_NVK = statusNvK;
                    gcsLichgcs.STATUS_DVCM = statusDvcm;
                    Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);

                    // Lưu Log
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLichgcs);
                    var logCategoryId = "KYBANGKE";
                    var contentLog = "";
                    DateTime logDate = DateTime.Now;
                    var logStatus = "DK";
                    var lstCategoryLog =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                    var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    var countThucHien = 1;
                    contentLog = userName + " " + lstCategoryLog + " " + maLoaiBangKe + " thành công";
                    WriteLog writeL = new WriteLog(Uow);
                    writeL.WriteLogGcs(logCategoryId, idLichgcs, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maLoaiBangKe, countThucHien, logStatus);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                model.Message = ex.Message + "\rn" + ex.StackTrace;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult XacNhanKyBangKe(List<int> ids, string maLoaiBangKe)
        {
            var statusDvcm = "CDVC";
            var statusNvK = "NVDK";
            string MaQuyen;
            string NgayGcs;
            int userId = new CommonUserProfile().UserID;
            string hoten = new CommonUserProfile().FullName;
            DateTime thoiGian = DateTime.Now;

            bool checkStatus = false;
            foreach (var idLichgcs in ids)
            {
                var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLichgcs);
                var gcsBangkeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetOne(i => i.ID_LICHGCS == idLichgcs
                                                                                && i.MA_LOAIBANGKE == maLoaiBangKe);
                //int idBangKeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetOne(o => o.ID_LICHGCS == idLichgcs).MA_BANGKELICH;

                // Check đã đối soát hay chưa
                var lstDoiSoat = Uow.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_QUYEN == gcsLichgcs.MA_SOGCS
                                    && x.STR_CHECK_DSOAT == "CHUA_DOI_SOAT").ToList();
                if (lstDoiSoat.Count != 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Sổ " + gcsLichgcs.MA_SOGCS
                        + " đối soát chưa thành công. Vui lòng đối soát hết rồi tiến hành ký hoặc xác nhận bảng kê"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (gcsBangkeLich == null)
                {
                    gcsBangkeLich = new GCS_BANGKE_LICH();
                    gcsBangkeLich.ID_LICHGCS = idLichgcs;
                    gcsBangkeLich.MA_LOAIBANGKE = maLoaiBangKe;
                    Uow.RepoBase<GCS_BANGKE_LICH>().Create(gcsBangkeLich);
                }

                FL_FILE fileInfo = Uow.RepoBase<FL_FILE>().GetOne(i => i.MA_BANGKELICH == gcsBangkeLich.MA_BANGKELICH);

                if (fileInfo == null)
                {
                    var maso = gcsLichgcs.MA_SOGCS;
                    var nam = gcsLichgcs.NAM;
                    var thang = gcsLichgcs.THANG;
                    var ky = gcsLichgcs.KY;
                    var maDvqLy = gcsLichgcs.MA_DVIQLY;
                    object dsSo = _bangKeHelper.GetBangKeChiSo(idLichgcs);

                    ReportHelper reportHelper = new ReportHelper(Uow);
                    InstanceReportSource instanceReportSource = new InstanceReportSource();
                    ReportProcessor reportProcessor = new ReportProcessor();
                    MaQuyen = reportHelper.getMaQuyen(idLichgcs);
                    NgayGcs = reportHelper.getNgayGcs(idLichgcs);
                    try
                    {
                        switch (maLoaiBangKe)
                        {
                            #region Bảng kê chỉ số

                            case "BKCS":
                                rptBangKeChiSo reportBangKeChiSo = new rptBangKeChiSo();
                                var rptBkcs = new rptBangKeChiSo();

                                instanceReportSource.ReportDocument = rptBkcs;

                                var sourceBangKeChiSo = reportHelper.getReportBkcs(idLichgcs);
                                if (sourceBangKeChiSo != null)
                                {
                                    reportBangKeChiSo.SetSourceTable(sourceBangKeChiSo);
                                    reportBangKeChiSo.SetParamater(NgayGcs, MaQuyen);
                                }
                                instanceReportSource.ReportDocument = reportBangKeChiSo.Report;
                                //rptTong.ReportSource = instanceReportSource;
                                RenderingResult resultBkcs = reportProcessor.RenderReport("PDF", instanceReportSource,
                                    null);

                                //4.Save file vào thư mục....
                                string fileName = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                  "_" + ky + "." +
                                                  resultBkcs.Extension;
                                string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                                string filePath = directoryPath + "/" + fileName;
                                if (!Directory.Exists(Server.MapPath(directoryPath)))
                                    Directory.CreateDirectory(Server.MapPath(directoryPath));
                                using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                                {
                                    fs.Write(resultBkcs.DocumentBytes, 0, resultBkcs.DocumentBytes.Length);
                                }
                                //5.Lưu thông tin vào bảng FL_File
                                using (
                                    ESDigitalSignatureManager dsm =
                                        new ESDigitalSignatureManager(
                                            System.IO.File.ReadAllBytes(Server.MapPath(filePath)),
                                            Path.GetExtension(filePath)))
                                {
                                    var fileHash = dsm.GetHashValue();
                                    fileInfo = new FL_FILE();
                                    fileInfo.FilePath = filePath;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                }
                                break;

                            #endregion

                            #region Tình trạng bất thường

                            case "TTBT":
                                rptTinhTrangBatThuong reportBangKeTinhTrangBatThuong = new rptTinhTrangBatThuong();
                                var rptTtbt = new rptTinhTrangBatThuong();

                                instanceReportSource.ReportDocument = rptTtbt;

                                var sourceTinhTrangBatThuong = reportHelper.getReportBkcs(idLichgcs);
                                if (sourceTinhTrangBatThuong != null)
                                {
                                    reportBangKeTinhTrangBatThuong.SetSourceTable(sourceTinhTrangBatThuong);
                                }
                                instanceReportSource.ReportDocument = reportBangKeTinhTrangBatThuong.Report;
                                //rptTong.ReportSource = instanceReportSource;
                                RenderingResult resultTtbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                    null);

                                //4.Save file vào thư mục....
                                string fileNameTtbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                      "_" + ky + "." +
                                                      resultTtbt.Extension;
                                string directoryPathTtbt = CommonHelper.UploadsPath + maDvqLy;
                                string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                                if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                                    Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                                using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                                {
                                    fs.Write(resultTtbt.DocumentBytes, 0, resultTtbt.DocumentBytes.Length);
                                }
                                //5.Lưu thông tin vào bảng FL_File
                                using (
                                    ESDigitalSignatureManager dsm =
                                        new ESDigitalSignatureManager(
                                            System.IO.File.ReadAllBytes(Server.MapPath(filePathTtbt)),
                                            Path.GetExtension(filePathTtbt)))
                                {
                                    var fileHash = dsm.GetHashValue();
                                    fileInfo = new FL_FILE();
                                    fileInfo.FilePath = filePathTtbt;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                }
                                break;

                            #endregion

                            #region Sản lượng bất thường

                            case "SLBT":
                                rptSanLuongBatThuong reportSlbt = new rptSanLuongBatThuong();
                                var rptSlbt = new rptSanLuongBatThuong();

                                instanceReportSource.ReportDocument = rptSlbt;

                                var sourceSlbt = reportHelper.get_SLBT(idLichgcs);
                                if (sourceSlbt != null)
                                {
                                    reportSlbt.SetSourceTable(sourceSlbt);
                                }
                                instanceReportSource.ReportDocument = reportSlbt.Report;
                                //rptTong.ReportSource = instanceReportSource;
                                RenderingResult resultSlbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                    null);

                                //4.Save file vào thư mục....
                                string filenameSlbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                      "_" + ky + "." +
                                                      resultSlbt.Extension;
                                string directoryPathSlbt = CommonHelper.UploadsPath + maDvqLy;
                                string filePathSlbt = directoryPathSlbt + "/" + filenameSlbt;
                                if (!Directory.Exists(Server.MapPath(directoryPathSlbt)))
                                    Directory.CreateDirectory(Server.MapPath(directoryPathSlbt));
                                using (FileStream fs = new FileStream(Server.MapPath(filePathSlbt), FileMode.Create))
                                {
                                    fs.Write(resultSlbt.DocumentBytes, 0, resultSlbt.DocumentBytes.Length);
                                }
                                //5.Lưu thông tin vào bảng FL_File
                                using (
                                    ESDigitalSignatureManager dsm =
                                        new ESDigitalSignatureManager(
                                            System.IO.File.ReadAllBytes(Server.MapPath(filePathSlbt)),
                                            Path.GetExtension(filePathSlbt)))
                                {
                                    var fileHash = dsm.GetHashValue();
                                    fileInfo = new FL_FILE();
                                    fileInfo.FilePath = filePathSlbt;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                }
                                break;

                            #endregion

                            #region Ảnh sản lượng bất thường

                            case "ASLBT":
                                rptAnhSanLuongBatThuong reportAslbt = new rptAnhSanLuongBatThuong();
                                var rptAslbt = new rptAnhSanLuongBatThuong();

                                instanceReportSource.ReportDocument = rptAslbt;

                                var sourceAslbt = reportHelper.getReportBkcs(idLichgcs);
                                if (sourceAslbt != null)
                                {
                                    reportAslbt.SetSourceTable(sourceAslbt);
                                }
                                instanceReportSource.ReportDocument = reportAslbt.Report;
                                //rptTong.ReportSource = instanceReportSource;
                                RenderingResult resultAslbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                    null);

                                //4.Save file vào thư mục....
                                string fileNameAslbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" +
                                                       thang + "_" + ky + "." +
                                                       resultAslbt.Extension;
                                string directoryPathAslbt = CommonHelper.UploadsPath + maDvqLy;
                                string filePathAslbt = directoryPathAslbt + "/" + fileNameAslbt;
                                if (!Directory.Exists(Server.MapPath(directoryPathAslbt)))
                                    Directory.CreateDirectory(Server.MapPath(directoryPathAslbt));
                                using (FileStream fs = new FileStream(Server.MapPath(filePathAslbt), FileMode.Create))
                                {
                                    fs.Write(resultAslbt.DocumentBytes, 0, resultAslbt.DocumentBytes.Length);
                                }
                                //5.Lưu thông tin vào bảng FL_File
                                using (
                                    ESDigitalSignatureManager dsm =
                                        new ESDigitalSignatureManager(
                                            System.IO.File.ReadAllBytes(Server.MapPath(filePathAslbt)),
                                            Path.GetExtension(filePathAslbt)))
                                {
                                    var fileHash = dsm.GetHashValue();
                                    fileInfo = new FL_FILE();
                                    fileInfo.FilePath = filePathAslbt;
                                    fileInfo.FileHash = fileHash;
                                    fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                    Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                }
                                break;

                            #endregion

                            default:
                                break;
                        }
                        GCS_BANGKE_LICH_CHITIET_KY chiTietKy = new GCS_BANGKE_LICH_CHITIET_KY();
                        chiTietKy.UserId = userId;
                        chiTietKy.NGUOI_KY = hoten;
                        chiTietKy.NGAY_KY = thoiGian;
                        chiTietKy.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                        gcsLichgcs.STATUS_NVK = statusNvK;
                        gcsLichgcs.STATUS_DVCM = statusDvcm;
                        var checkGcsLich = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                        var checkBkeLich = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().Create(chiTietKy);
                        if (checkGcsLich != 1 && checkBkeLich != 1)
                        {
                            return Json(new { success = false, message = "Xác nhận ký không thành công!" },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Xác nhận ký không thành công " + ex },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var mapPath = Server.MapPath(fileInfo.FilePath);
                    if (mapPath == null)
                    {
                        var maso = gcsLichgcs.MA_SOGCS;
                        var nam = gcsLichgcs.NAM;
                        var thang = gcsLichgcs.THANG;
                        var ky = gcsLichgcs.KY;
                        var maDvqLy = gcsLichgcs.MA_DVIQLY;
                        object dsSo = _bangKeHelper.GetBangKeChiSo(idLichgcs);

                        ReportHelper reportHelper = new ReportHelper(Uow);
                        InstanceReportSource instanceReportSource = new InstanceReportSource();
                        ReportProcessor reportProcessor = new ReportProcessor();
                        MaQuyen = reportHelper.getMaQuyen(idLichgcs);
                        NgayGcs = reportHelper.getNgayGcs(idLichgcs);
                        try
                        {
                            switch (maLoaiBangKe)
                            {
                                #region Bảng kê chỉ số

                                case "BKCS":
                                    rptBangKeChiSo reportBangKeChiSo = new rptBangKeChiSo();
                                    var rptBkcs = new rptBangKeChiSo();

                                    instanceReportSource.ReportDocument = rptBkcs;

                                    var sourceBangKeChiSo = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceBangKeChiSo != null)
                                    {
                                        reportBangKeChiSo.SetSourceTable(sourceBangKeChiSo);
                                        reportBangKeChiSo.SetParamater(NgayGcs, MaQuyen);
                                    }
                                    instanceReportSource.ReportDocument = reportBangKeChiSo.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultBkcs = reportProcessor.RenderReport("PDF", instanceReportSource,
                                        null);

                                    //4.Save file vào thư mục....
                                    string fileName = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                      "_" + ky + "." +
                                                      resultBkcs.Extension;
                                    string directoryPath = CommonHelper.UploadsPath + maDvqLy;
                                    string filePath = directoryPath + "/" + fileName;
                                    if (!Directory.Exists(Server.MapPath(directoryPath)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPath));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Create))
                                    {
                                        fs.Write(resultBkcs.DocumentBytes, 0, resultBkcs.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(
                                                System.IO.File.ReadAllBytes(Server.MapPath(filePath)),
                                                Path.GetExtension(filePath)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        fileInfo = new FL_FILE();
                                        fileInfo.FilePath = filePath;
                                        fileInfo.FileHash = fileHash;
                                        fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                        Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                    }
                                    break;

                                #endregion

                                #region Tình trạng bất thường

                                case "TTBT":
                                    rptTinhTrangBatThuong reportBangKeTinhTrangBatThuong = new rptTinhTrangBatThuong();
                                    var rptTtbt = new rptTinhTrangBatThuong();

                                    instanceReportSource.ReportDocument = rptTtbt;

                                    var sourceTinhTrangBatThuong = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceTinhTrangBatThuong != null)
                                    {
                                        reportBangKeTinhTrangBatThuong.SetSourceTable(sourceTinhTrangBatThuong);
                                    }
                                    instanceReportSource.ReportDocument = reportBangKeTinhTrangBatThuong.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultTtbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                        null);

                                    //4.Save file vào thư mục....
                                    string fileNameTtbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                          "_" + ky + "." +
                                                          resultTtbt.Extension;
                                    string directoryPathTtbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                                    {
                                        fs.Write(resultTtbt.DocumentBytes, 0, resultTtbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(
                                                System.IO.File.ReadAllBytes(Server.MapPath(filePathTtbt)),
                                                Path.GetExtension(filePathTtbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        fileInfo = new FL_FILE();
                                        fileInfo.FilePath = filePathTtbt;
                                        fileInfo.FileHash = fileHash;
                                        fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                        Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                    }
                                    break;

                                #endregion

                                #region Sản lượng bất thường

                                case "SLBT":
                                    rptSanLuongBatThuong reportSlbt = new rptSanLuongBatThuong();
                                    var rptSlbt = new rptSanLuongBatThuong();

                                    instanceReportSource.ReportDocument = rptSlbt;

                                    var sourceSlbt = reportHelper.get_SLBT(idLichgcs);
                                    if (sourceSlbt != null)
                                    {
                                        reportSlbt.SetSourceTable(sourceSlbt);
                                    }
                                    instanceReportSource.ReportDocument = reportSlbt.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultSlbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                        null);

                                    //4.Save file vào thư mục....
                                    string filenameSlbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" + thang +
                                                          "_" + ky + "." +
                                                          resultSlbt.Extension;
                                    string directoryPathSlbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathSlbt = directoryPathSlbt + "/" + filenameSlbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathSlbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathSlbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathSlbt), FileMode.Create))
                                    {
                                        fs.Write(resultSlbt.DocumentBytes, 0, resultSlbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(
                                                System.IO.File.ReadAllBytes(Server.MapPath(filePathSlbt)),
                                                Path.GetExtension(filePathSlbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        fileInfo = new FL_FILE();
                                        fileInfo.FilePath = filePathSlbt;
                                        fileInfo.FileHash = fileHash;
                                        fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                        Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                    }
                                    break;

                                #endregion

                                #region Ảnh sản lượng bất thường

                                case "ASLBT":
                                    rptAnhSanLuongBatThuong reportAslbt = new rptAnhSanLuongBatThuong();
                                    var rptAslbt = new rptAnhSanLuongBatThuong();

                                    instanceReportSource.ReportDocument = rptAslbt;

                                    var sourceAslbt = reportHelper.getReportBkcs(idLichgcs);
                                    if (sourceAslbt != null)
                                    {
                                        reportAslbt.SetSourceTable(sourceAslbt);
                                    }
                                    instanceReportSource.ReportDocument = reportAslbt.Report;
                                    //rptTong.ReportSource = instanceReportSource;
                                    RenderingResult resultAslbt = reportProcessor.RenderReport("PDF", instanceReportSource,
                                        null);

                                    //4.Save file vào thư mục....
                                    string fileNameAslbt = gcsBangkeLich.MA_LOAIBANGKE + "_" + maso + "_" + nam + "_" +
                                                           thang + "_" + ky + "." +
                                                           resultAslbt.Extension;
                                    string directoryPathAslbt = CommonHelper.UploadsPath + maDvqLy;
                                    string filePathAslbt = directoryPathAslbt + "/" + fileNameAslbt;
                                    if (!Directory.Exists(Server.MapPath(directoryPathAslbt)))
                                        Directory.CreateDirectory(Server.MapPath(directoryPathAslbt));
                                    using (FileStream fs = new FileStream(Server.MapPath(filePathAslbt), FileMode.Create))
                                    {
                                        fs.Write(resultAslbt.DocumentBytes, 0, resultAslbt.DocumentBytes.Length);
                                    }
                                    //5.Lưu thông tin vào bảng FL_File
                                    using (
                                        ESDigitalSignatureManager dsm =
                                            new ESDigitalSignatureManager(
                                                System.IO.File.ReadAllBytes(Server.MapPath(filePathAslbt)),
                                                Path.GetExtension(filePathAslbt)))
                                    {
                                        var fileHash = dsm.GetHashValue();
                                        fileInfo = new FL_FILE();
                                        fileInfo.FilePath = filePathAslbt;
                                        fileInfo.FileHash = fileHash;
                                        fileInfo.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                                        Uow.RepoBase<FL_FILE>().Create(fileInfo);
                                    }
                                    break;

                                #endregion

                                default:
                                    break;
                            }

                        }
                        catch (Exception ex)
                        {
                            return Json(new { checkStatus, message = "Xác nhận ký không thành công " + ex },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    GCS_BANGKE_LICH_CHITIET_KY chiTietKy = new GCS_BANGKE_LICH_CHITIET_KY();
                    chiTietKy.UserId = userId;
                    chiTietKy.NGUOI_KY = hoten;
                    chiTietKy.NGAY_KY = thoiGian;
                    chiTietKy.MA_BANGKELICH = gcsBangkeLich.MA_BANGKELICH;
                    gcsLichgcs.STATUS_NVK = statusNvK;
                    gcsLichgcs.STATUS_DVCM = statusDvcm;
                    var checkGcsLich = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                    var checkBkeLich = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().Create(chiTietKy);
                    if (checkGcsLich != 1 && checkBkeLich != 1)
                    {
                        return Json(new { success = false, message = "Xác nhận ký không thành công!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                // Lưu Log
                var logCategoryId = "XN_KYBANGKE";
                var contentLog = "";
                DateTime logDate = DateTime.Now;
                var maBangKeLich = "";
                var logStatus = "XNK";
                var lstCategoryLog =
                    Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                var countThucHien = 1;
                contentLog = userName + " " + lstCategoryLog + " " + maLoaiBangKe + " thành công";
                WriteLog writeL = new WriteLog(Uow);
                writeL.WriteLogGcs(logCategoryId, idLichgcs, gcsLichgcs.MA_SOGCS, gcsLichgcs.KY, gcsLichgcs.THANG, gcsLichgcs.NAM,
                    contentLog, userId, logDate, maBangKeLich,
                    countThucHien, logStatus);
            }
            
            return Json(new { success = true, message = "Xác nhận ký thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDOI_GCS()
        {
            string username = User.Identity.Name;
            var listDOIGCS = new List<D_DOIGCS>();
            var idpartment = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
            string MaDonVi = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == idpartment).DepartmentCode; //lấy mã đơn vị thông qua 2 bảng UserProfile và Administrator_Department
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
                var dmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
                var maDonviQuanly = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == dmId).DepartmentCode;
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
        public ActionResult TraLaiBangKe(int idLich, string maBangke, string lyDo)
        {
            try
            {
                var statusNvK = "NVCK";
                var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                var dmId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
                var userInRoles = Uow.RepoBase<WebpagesUsersInRoles>().GetOne(x => x.UserId == userId).RoleId;

                var doitruong = Uow.RepoBase<WebpagesUsersInRoles>().GetAll(x => x.RoleId == 4).ToList();
                var lstUser = Uow.RepoBase<UserProfile>().GetAll().ToList();

                var getMaillDt = (from dt in doitruong
                             join lst in lstUser
                             on dt.UserId equals lst.UserId into allEmployees
                             from result in allEmployees.DefaultIfEmpty()
                             select new
                             {
                                 ID = result.DepartmentId, result.Email
                             }).Where(x => x.ID == dmId).Select(x => x.Email).FirstOrDefault();

                var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLich);
                var gcsBangkeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetOne(i => i.ID_LICHGCS == idLich && i.MA_LOAIBANGKE == maBangke);
                var gcsBangkeLichChitietKy = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().GetOne(i => i.MA_BANGKELICH == gcsBangkeLich.MA_BANGKELICH);
                FL_FILE fileInfo = Uow.RepoBase<FL_FILE>().GetOne(i => i.MA_BANGKELICH == gcsBangkeLich.MA_BANGKELICH);
                var mapPath = Server.MapPath(fileInfo.FilePath);
                if (idLich != 0 || maBangke != null)
                {
                    if (userInRoles == 5)
                    {
                        if (System.IO.File.Exists(mapPath))
                        {
                            System.IO.File.Delete(mapPath);
                        }
                        if (gcsLichgcs.TRALAI_COUNT == null)
                        {
                            gcsLichgcs.TRALAI_COUNT = 1;
                            var kq = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                            if (kq != 1)
                            {
                                return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            gcsLichgcs.TRALAI_COUNT += 1;
                            var kq = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                            if (kq != 1)
                            {
                                return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        
                        string hoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == gcsLichgcs.USERID).FullName;
                        string title = "EVN HANOI: Trả lại bảng kê";
                        string content = "Tổ/Đội thực hiện: " + gcsLichgcs.MA_DOIGCS + " - " + Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == gcsLichgcs.MA_DOIGCS).TEN_DOI + "<br/>";
                        content += "Cán bộ Ghi chỉ số thực hiện: " + hoTen + "<br/>";
                        content += "Chi tiết sổ cần bổ sung, cập nhật thêm chỉ số: <br/>";

                        string tenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == gcsLichgcs.MA_SOGCS && o.MA_DVIQLY == gcsLichgcs.MA_DVIQLY).TEN_SOGCS;
                        content += "- Sổ " + gcsLichgcs.MA_SOGCS + ": " + tenSo + "<br/>";
                        content += "Bộ phận trả: Điều hành<br/>";
                        content += "Trả lại: Lần " + gcsLichgcs.TRALAI_COUNT + "<br/>";
                        content += "Lý do: " + lyDo + "<br/>";

                        SendMail send = new SendMail();
                        send.Send_Email_Cc(Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == gcsLichgcs.USERID).Email, getMaillDt, content, title, "");

                        gcsLichgcs.STATUS_NVK = statusNvK;
                        var lstBkLichCtKy = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().Delete(gcsBangkeLichChitietKy);
                        var lstFlFile = Uow.RepoBase<FL_FILE>().Delete(fileInfo);
                        var lstBkLich = Uow.RepoBase<GCS_BANGKE_LICH>().Delete(gcsBangkeLich);
                        var lstLichGcs = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                        if (lstBkLichCtKy != 1 && lstFlFile != 1 && lstBkLich != 1 && lstLichGcs != 1)
                        {
                            return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        //var mapPath = Server.MapPath("~/TemplateFile/" + Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == idLich).MA_DVIQLY.Trim() + @"/" + gcsLichgcs.MA_SOGCS + "-" + gcsLichgcs.NAM + "-" + gcsLichgcs.THANG + "-" + gcsLichgcs.KY + ".pdf");

                        //var gcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_QUYEN == gcsLichgcs.MA_SOGCS).ToList();
                        //foreach (var lst in gcsHhu)
                        //{
                        //    lst.STR_CHECK_DSOAT = "CHUA_DOI_SOAT";
                        //    var up = Uow.RepoBase<GCS_CHISO_HHU>().Update(lst);
                        //}
                        if (System.IO.File.Exists(mapPath))
                        {
                            System.IO.File.Delete(mapPath);
                        }
                        if (gcsLichgcs.TRALAI_COUNT == null)
                        {
                            gcsLichgcs.TRALAI_COUNT = 1;
                            var kq = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                            if (kq != 1)
                            {
                                return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            gcsLichgcs.TRALAI_COUNT += 1;
                            var kq = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                            if (kq != 1)
                            {
                                return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        string hoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == gcsLichgcs.USERID).FullName;
                        string title = "EVN HANOI: Trả lại bảng kê";
                        string content = "Tổ/Đội thực hiện: " + gcsLichgcs.MA_DOIGCS + " - " + Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == gcsLichgcs.MA_DOIGCS).TEN_DOI + "<br/>";
                        content += "Cán bộ Ghi chỉ số thực hiện: " + hoTen + "<br/>";
                        content += "Chi tiết sổ cần bổ sung, cập nhật thêm chỉ số: <br/>";

                        string tenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == gcsLichgcs.MA_SOGCS && o.MA_DVIQLY == gcsLichgcs.MA_DVIQLY).TEN_SOGCS;
                        content += "- Sổ " + gcsLichgcs.MA_SOGCS + ": " + tenSo + "<br/>";
                        content += "Bộ phận trả: Đội trưởng<br/>";
                        content += "Trả lại: Lần " + gcsLichgcs.TRALAI_COUNT + "<br/>";
                        content += "Lý do: " + lyDo + "<br/>";

                        SendMail send = new SendMail();
                        send.Send_Email(Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == gcsLichgcs.USERID).Email, content, title, "");

                        gcsLichgcs.STATUS_NVK = statusNvK;
                        var lstBkLichCtKy = Uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().Delete(gcsBangkeLichChitietKy);
                        var lstFlFile = Uow.RepoBase<FL_FILE>().Delete(fileInfo);
                        var lstBkLich = Uow.RepoBase<GCS_BANGKE_LICH>().Delete(gcsBangkeLich);
                        var lstLichGcs = Uow.RepoBase<GCS_LICHGCS>().Update(gcsLichgcs);
                        if (lstBkLichCtKy != 1 && lstFlFile != 1 && lstBkLich != 1 && lstLichGcs != 1)
                        {
                            return Json(new { success = false, message = "Trả lại bảng kê không thành công!" },
                                JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                // Lưu Log
                var logCategoryId = "TL_BANGKE";
                var contentLog = "";
                DateTime logDate = DateTime.Now;
                var logStatus = "TL";
                var lstCategoryLog =
                    Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                var countThucHien = 1;
                contentLog = userName + " " + lstCategoryLog + " " + maBangke + " thành công";
                WriteLog writeL = new WriteLog(Uow);
                writeL.WriteLogGcs(logCategoryId, idLich, gcsLichgcs.MA_SOGCS, gcsLichgcs.KY, gcsLichgcs.THANG, gcsLichgcs.NAM,
                    contentLog, userId, logDate, maBangke,
                    countThucHien, logStatus);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Trả lại bảng kê không thành công!", ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Trả lại bảng kê thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}
