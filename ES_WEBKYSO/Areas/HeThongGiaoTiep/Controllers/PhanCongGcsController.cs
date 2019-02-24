using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using System.IO;
using Ionic.Zip;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class PhanCongGcsController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/PhanCongGcs/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        #region khởi tạo giá trị khi mở trang
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("ID_LICHGCS");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            //var dataSOGCS = UnitOfWork.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findModel, "").ToList();
            //var dataSOGCS = Uow.RepoBase<D_SOGCS>().GETALL().ToList();
            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GETALL().ToList();
            //if (findModel.TrangThai == null) findModel.TrangThai = "CPC";
            var dataLICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //var dataLICHGCS = Uow.RepoBase<GCS_LICHGCS>().GETALL().ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GETALL().ToList();


            //var dataPHANCONG = dataSOGCS.Join(dataCAUHINH, m1  => m1.MA_SOGCS, m2 => m2.MA_SOGCS, (phancong, cauhinh) => new { phancong, cauhinh }).ToList();
            var data = (from SOGCS in dataSOGCS
                        join CAUHINH in dataCAUHINH on SOGCS.MA_SOGCS equals CAUHINH.MA_SOGCS into grouping
                        from SOGCS_NV in grouping.DefaultIfEmpty()
                        select new { SOGCS, SOGCS_NV }).ToList();
            var dataPHANCONG = (from CAUHINH in data
                                join LICH in dataLICHGCS on new { p1 = CAUHINH.SOGCS.MA_SOGCS, p2 = CAUHINH.SOGCS.MA_DVIQLY, p3 = CAUHINH.SOGCS.SO_KY } equals new { p1 = LICH.MA_SOGCS, p2 = LICH.MA_DVIQLY, p3 = LICH.KY } //into grouping
                                select new { CAUHINH, LICH });
            List<PHANCONG> listPhanCong = new List<PHANCONG>();
            foreach (var item in dataPHANCONG)
            {
                PHANCONG phancong = new PHANCONG();
                phancong.ID_LICHGCS = item.LICH.ID_LICHGCS;
                phancong.MA_DVIQLY = item.CAUHINH.SOGCS.MA_DVIQLY;
                phancong.MA_SOGCS = item.CAUHINH.SOGCS.MA_SOGCS;
                phancong.TEN_SOGCS = item.CAUHINH.SOGCS.TEN_SOGCS;
                phancong.HINHTHUC = item.CAUHINH.SOGCS.HINH_THUC;
                phancong.NGAY_GHI = Convert.ToInt32(item.CAUHINH.SOGCS.NGAY_GHI);
                phancong.KY = Convert.ToInt32(item.LICH.KY);
                phancong.THANG = Convert.ToInt32(item.LICH.THANG);
                phancong.NAM = Convert.ToInt32(item.LICH.NAM);
                phancong.MA_DOIGCS = item.LICH.MA_DOIGCS;
                phancong.USERID = item.LICH.USERID;
                phancong.STATUS = item.LICH.STATUS_PC;
                if (phancong.USERID != null)
                {
                    phancong.TEN_NVIEN_GCS = Uow.RepoBase<UserProfile>().GetOne(phancong.USERID).FullName;
                }
                listPhanCong.Add(phancong);
            }
            paging.data = listPhanCong;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
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
        public JsonResult GetNV_GCS_TheoDoi(string MaDoi)
        {
            try
            {
                MaDoi = MaDoi.Trim();
                FindModelGcs findModel = new FindModelGcs();
                findModel.MaDoi = MaDoi;
                var id = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).DepartmentId;
                findModel.MaDonVi = new CommonUserProfile().MA_DVIQLY;
                var listNVGCS = Uow.RepoBase<CFG_DOIGCS_NVIEN>().ManagerGetAllForIndex(findModel, MaDoi); //lấy danh sách NVGCS thuộc đội

                List<DANHMUC> listUserProfile = new List<DANHMUC>();
                foreach (CFG_DOIGCS_NVIEN item in listNVGCS)
                {
                    var NVGCS = Uow.RepoBase<UserProfile>().GetOne(x => x.UserId == item.USERID); //(x => x.UserId == item.USERID);
                    DANHMUC dm = new DANHMUC();
                    dm.MA = NVGCS.UserId;
                    dm.TEN = NVGCS.FullName;
                    listUserProfile.Add(dm);
                }
                return Json(listUserProfile, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        #endregion

        #region lưu phân công, bao gồm phân công lại và phân công mới theo cấu hình
        [HttpPost]
        public JsonResult LuuPhanCong(List<int> ids, string MaDoi, string USERID)
        {
            MaDoi = MaDoi.Trim();
            int userid = 0;
            if (USERID != "") userid = Convert.ToInt32(USERID);
            string msg = "";
            string msgCAUHINH = ""; //danh sách các sổ chưa đc cấu hình
            CommonJsonResult result = new CommonJsonResult();
            List<GCS_LICHGCS> listLichNEW = new List<GCS_LICHGCS>();
            if (MaDoi == "" && userid == 0)
            {//không chọn đội và nhân viên (phân công theo cấu hình mặc định)
                foreach (var item in ids)
                {
                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    if (lichGcs.USERID != null && (lichGcs.MA_DOIGCS != null || lichGcs.MA_DOIGCS != "")) continue; //không cập nhật lại các sổ đã phân công khi phân công theo cấu hình mặc định
                    if (lichGcs.STATUS_CNCS == "DTH") continue; //không cập nhật các sổ đã thực hiện
                    CFG_SOGCS_NVIEN CauHinhGCS;
                    CauHinhGCS = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_DVIQLY == lichGcs.MA_DVIQLY && x.MA_SOGCS == lichGcs.MA_SOGCS);
                    if (CauHinhGCS != null)
                    { //cập nhật thông tin cấu hình vào bảng lịch - thêm vào list Lịch mới
                        lichGcs.STATUS_PC = "DPC";
                        lichGcs.STATUS_CNCS = "CCN";
                        lichGcs.NHANSO_MTB = "false";
                        lichGcs.MA_DOIGCS = CauHinhGCS.MA_DOIGCS;
                        lichGcs.USERID = CauHinhGCS.USERID;
                        listLichNEW.Add(lichGcs);
                    }
                    else
                    {//thêm sổ vào thông báo "Mời cấu hình sổ"
                        msgCAUHINH += lichGcs.MA_SOGCS + ", ";
                    }
                }
            }
            else
            if (MaDoi != "" && userid != 0)
            {//chọn đội và nhân viên (phân công lại)
                foreach (var item in ids)
                {
                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    //if (LichGCS.USERID != null && (LichGCS.MA_DOIGCS != null || LichGCS.MA_DOIGCS != "")) continue; //không cập nhật các sổ đã phân công
                    if (lichGcs.STATUS_CNCS == "DTH") continue; //không cập nhật các sổ đã thực hiện

                    lichGcs.STATUS_PC = "DPC";
                    lichGcs.STATUS_CNCS = "CCN";
                    lichGcs.NHANSO_MTB = "false";
                    lichGcs.MA_DOIGCS = MaDoi; //mã đội phân công mới
                    lichGcs.USERID = userid; //USERID phân công mới
                    listLichNEW.Add(lichGcs);
                }
            }
            else
            {//ko cấu hình do thiếu giá trị MaDoi hoặc UserID
                result.Result = false;
                result.Message = "Mời chọn Tổ đội và Người dùng!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (msgCAUHINH != "")
            {
                msg = "Phân công không thành công!\nMời cấu hình nhân viên GCS sổ " + msgCAUHINH;
                result.Result = false;
                result.Message = msg;
            }
            else
            {
                msg = "Phân công Sổ thành công!";
                result.Result = true;
                result.Message = msg;

                foreach (var item in ids)
                {
                    // Lưu Log
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    var logCategoryId = "PC_THUCONG";
                    var contentLog = "";
                    DateTime logDate = DateTime.Now;
                    var maBangKeLich = "";
                    var logStatus = "DPC";
                    var lstCategoryLog =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                    var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                        var countThucHien = 1;
                        contentLog = userName + " " + lstCategoryLog + " sổ " + lichGcs.TEN_SOGCS + " thành công";
                        WriteLog writeL = new WriteLog(Uow);
                        writeL.WriteLogGcs(logCategoryId, item, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maBangKeLich,
                            countThucHien, logStatus);
                }

            }
            bool check = true; //kiểm tra cập nhật tất cả thành công ko?
            msg = "";//biến lưu tạm các sổ cập nhật ko thành công
            if (result.Result && listLichNEW.Count != 0)
            {
                foreach (var item in listLichNEW)
                {
                    var ret = Uow.RepoBase<GCS_LICHGCS>().Update(item);
                    if (ret > 0 == false)
                    {
                        check = false;
                        msg += item.MA_SOGCS + ", ";
                    }
                }
            }
            if (check == false)
            {//thông báo lỗi khi update SQL thất bại
                result.Result = false;
                result.Message = "Phân công sổ " + msg + "không thành công!";
            }
            if(check && result.Result) //kiểm tra cập nhật thành công và phân công thành công thì gửi mail thông báo
            {// gửi mail sau khi phân công thành công
                CreateMail(MaDoi, userid, listLichNEW);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void CreateMail(string MADOI, int USERID, List<GCS_LICHGCS> listLich)
        {
            if (MADOI != "" && USERID != 0)
            {//gửi mail khi phân công lại
                string HoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == USERID).FullName;
                string title = "EVN HANOI: Phân công ghi chỉ số";
                string content = "Tổ/Đội thực hiện: " + MADOI + " - " + Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == MADOI).TEN_DOI + "<br/>";
                content += "Cán bộ Ghi chỉ số thực hiện: " + HoTen + "<br/>";
                content += "Chi tiết các sổ được phân công: <br/>";
                foreach (var item in listLich)
                {
                    string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                    content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                }
                SendMail send = new SendMail();
                send.Send_Email(Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == USERID).Email, content, title, "");
            }
            else
            {//gửi mail khi cấu hình
                List<TempMail> listMail = new List<TempMail>();
                var list = listLich;
                //tạo danh sách user gửi mail
                foreach (var item in list)
                {
                    var value = listMail.Find(o => o.UserID == item.USERID);
                    if (value != null)
                    {//thêm nội vào nội dung email sẽ gửi cho user
                        listMail.Remove(value);
                        string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                        value.Content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                        listMail.Add(value);
                    }
                    else
                    {//thêm mail sẽ gửi cho user
                        TempMail temp = new TempMail();
                        temp.MaDoi = item.MA_DOIGCS;
                        temp.TenDoi = Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == temp.MaDoi).TEN_DOI;
                        temp.UserID = item.USERID;
                        temp.HoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == temp.UserID).FullName;
                        temp.Email = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == temp.UserID).Email;
                        string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                        temp.Content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                        listMail.Add(temp);
                    }
                }
                foreach (var item in listMail)
                {
                    string title = "EVN HANOI: Phân công ghi chỉ số";
                    string content = "Tổ/Đội thực hiện: " + item.MaDoi + " - " + item.TenDoi + "<br/>";
                    content += "Cán bộ Ghi chỉ số thực hiện: " + item.HoTen + "<br/>";
                    content += "Chi tiết các sổ được phân công: <br/>";

                    content += item.Content;
                    SendMail send = new SendMail();
                    send.Send_Email(item.Email, content, title, "");
                }

            }
        }
        #endregion
        public ActionResult Delete(int? ID_LICHGCS)
        {
            if (ID_LICHGCS == null)
            {
                return Json(new { success = false, message = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == ID_LICHGCS);
            if (model == null)
            {
                return Json(new { success = false, message = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                model.MA_DOIGCS = null;
                model.USERID = null;
                model.STATUS_PC = "CPC";
                model.STATUS_CNCS = "";
                model.NHANSO_MTB = "false";
                var ret = Uow.RepoBase<GCS_LICHGCS>().Update(model);

                if (ret > 0)
                {
                    // Lưu Log
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == ID_LICHGCS);
                    var logCategoryId = "HUY_PC";
                    var contentLog = "";
                    DateTime logDate = DateTime.Now;
                    var maBangKeLich = "";
                    var logStatus = "CPC";
                    var lstCategoryLog =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                    var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    var countThucHien = 1;
                    contentLog = userName + " " + lstCategoryLog + " sổ " + lichGcs.TEN_SOGCS + " thành công";
                    WriteLog writeL = new WriteLog(Uow);
                    writeL.WriteLogGcs(logCategoryId, ID_LICHGCS, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maBangKeLich,
                        countThucHien, logStatus);
                    return Json(new { success = true, message = "Hủy phân công sổ " + model.MA_SOGCS + " thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                // ignored
            }
            return Json(new { success = false, message = "Hủy phân công không thành công!" }, JsonRequestBehavior.AllowGet);
        }

        #region tải file xuống
        public FileResult Download(int ID_LICHGCS)
        {
            var LichGCS = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == ID_LICHGCS);
            //string path = Server.MapPath("~/") + @"TemplateFile\" + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
            string path = Utility.getXMLPath() + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
            byte[] fileBytes = null;
            string fileName = LichGCS.FILE_XML;
            if (new FileInfo(path).Exists)
            {
                fileBytes = System.IO.File.ReadAllBytes(path);
            }            
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public FileResult DownloadFiles()
        {
            List<int> ids = Request["txtLichID"].Split(',').Select(Int32.Parse).ToList<int>();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                string fileType = "application/octet-stream";

                var outputStream = new MemoryStream();
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                foreach (var item in ids)
                {
                    var LichGCS = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    //string path = Server.MapPath("~/") + @"TemplateFile\" + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
                    string path = Utility.getXMLPath() + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
                    try { if (new FileInfo(path).Exists) zip.AddFile(path, ""); } catch { }
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
                outputStream.Position = 0;
                return new FileStreamResult(outputStream, fileType);
            }

        }
        #endregion
    }
}
