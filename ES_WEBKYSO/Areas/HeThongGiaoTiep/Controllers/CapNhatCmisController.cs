using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using Microsoft.Ajax.Utilities;
using System.Xml;
using System.Xml.Serialization;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class CapNhatCmisController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/CapNhatCmis/
        //Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách sổ GCS";
            ViewBag.MaDonVi = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        //[HttpPost]
        //public ActionResult GetJson(FindModelGcs findModel)
        //{
        //    var paging = Request.Params.ToPaging("Year");
        //    // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
        //    var data = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

        //    paging.data = data;
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
            var roleId = Uow.RepoBase<WebpagesUsersInRoles>().GetOne(x => x.UserId == userId).RoleId;

            //var tblGCS_LICHGCS = Uow.RepoBase<GCS_LICHGCS>().GetAll()
            //                    .Where(i => i.MA_DVIQLY == findModel.MaDonVi
            //                                || (   
            //                                    i.NGAY_GHI == findModel.NgayGhi
            //                                    || i.MA_SOGCS.Contains(findModel.MaSo)
            //                                    )
            //                                 && i.KY == findModel.Ky
            //                                && (i.THANG == findModel.Thang)
            //                                && i.NAM == findModel.Nam).ToList();
            var tblGCS_LICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
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
                              STATUS_DVCM = gcsLichGcs.STATUS_DVCM,
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

            if (findModel.TrangThai == "CDVC")
            {
                BANGKE_LICH bkl = null;
                //bkl.TrangThaiKy == true;
                //foreach (BANGKE_LICH bkelich in result)
                //{
                //    bkelich.TrangThaiKy = true;
                //}
                if (findModel.TrangThaiKy.HasValue)
                    result = result.Where(i => i.TrangThaiKy == findModel.TrangThaiKy && i.MA_LOAIBANGKE == findModel.MaLoaiBangKe).ToList();
            }
            else
            {
                foreach (BANGKE_LICH bkelich in result)
                {
                    bkelich.TrangThaiKy = tblGCS_CHITIET_KY.Any(i => i.UserId == userId && i.MA_BANGKELICH == bkelich.MA_BANGKELICH.Value);
                }
                if (findModel.TrangThaiKy.HasValue)
                    result = result.Where(i => i.TrangThaiKy == findModel.TrangThaiKy && i.MA_LOAIBANGKE == findModel.MaLoaiBangKe).ToList();
            }
            

            paging.data = result;//sau day moi thuc hien phan trang

            return Json(paging, JsonRequestBehavior.AllowGet);
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

        public ActionResult JsonLuuDuLieuCmis(FindModelGcs findModel, string search, List<int> ids, string MaDonVi, int Ky, int Thang, int Nam)
        {
            var lstError = new List<string>();
            var lstSuccess = new List<string>();
            try
            {
            //    int nam = Convert.ToInt32(findModel.Nam);
            //    int thang = Convert.ToInt32(findModel.Thang);
            //    int ky = Convert.ToInt32(findModel.Ky);
                var statusDvcm = "DDVC";

                if (Thang == 0 && Ky == 0 || Nam == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn kỳ, tháng và năm" }, JsonRequestBehavior.AllowGet);
                }
                var data = Uow.RepoBase<D_SOGCS>()
                   .ManagerGetAllForIndex(findModel, search)
                   .ToList()
                   .Select(x => x.MA_SOGCS).ToList();
                var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "CMIS");
                var configInputTonThat = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVTONTHAT");
                var strURL_CMISInterface = "";
                //Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
                //Service_TonThat.Service_TonThat serTonThat = new Service_TonThat.Service_TonThat();
                if (configInput != null || configInputTonThat != null)
                {
                    strURL_CMISInterface = configInput.Value + "WriteHHCService";
                }
                else
                {
                    return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
                        JsonRequestBehavior.AllowGet);
                }
                //if (data.Count == 0)
                //{
                //    return Json(new { success = false, message = "Không có dữ liệu sổ" }, JsonRequestBehavior.AllowGet);
                //}
                foreach (var soId in ids)
                {
                    var soGcsId = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_SOGCS;
                    var loaiSoGcs = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_SOGCS == soGcsId).LOAI_SOGCS;
                    //var mapPath = Server.MapPath("~/TemplateFile/" + Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_DVIQLY.Trim() + @"/" + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml");
                    var mapPath = Utility.getXMLPath() + Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_DVIQLY.Trim() + @"/" + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml";
                    DataSet ds = new DataSet();
                    ds.ReadXml(mapPath);
                    String strXML = "";
                    using (StreamReader sr = new StreamReader(mapPath))
                    {
                        strXML += sr.ReadToEnd();
                    }

                    //if (loaiSoGcs == "DN")
                    //{
                    //    dataFromCmis = serTonThat.WriteXmlHHCTT(MaDonVi, soGcsId, Ky, Thang, Nam, ds);
                    //}
                    //else
                    //{
                    //    dataFromCmis = ser.WriteHHCService(MaDonVi, soGcsId, Ky, Thang, Nam, ds);
                    //}

                    dynamic product = new JObject();
                    product.MA_DVIQLY = MaDonVi;
                    product.MA_SOGCS = soGcsId;
                    product.KY = Ky + "";
                    product.THANG = Thang + "";
                    product.NAM = Nam + "";
                    product.XML_HHC = strXML;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL_CMISInterface);
                    request.Method = "POST";

                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    Byte[] byteArray = encoding.GetBytes(product.ToString());

                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";

                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    try
                    {
                        WebResponse webResponse = request.GetResponse();
                        using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            var response = responseReader.ReadToEnd();
                            Console.Out.WriteLine(response);
                            if (!response.Contains("<ERROR>"))
                            {
                                var kqLichGcs =
                              Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId && x.KY == Ky && x.THANG == Thang && x.NAM == Nam);
                                if (kqLichGcs != null)
                                {
                                    kqLichGcs.STATUS_DVCM = statusDvcm;
                                    int lichqkq = Uow.RepoBase<GCS_LICHGCS>().Update(kqLichGcs);
                                }
                                else
                                {
                                    return Json(new { success = true, message = "Không tồn tại mã sổ " + soGcsId + "!" },
                                        JsonRequestBehavior.AllowGet);
                                }
                                lstSuccess.Add(soGcsId);
                                // Lưu Log
                                var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                                var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId);
                                var logCategoryId = "CMIS_CAPNHAT";
                                var contentLog = "";
                                DateTime logDate = DateTime.Now;
                                var maBangKeLich = "";
                                var logStatus = "DSVC";
                                var lstCategoryLog =
                                    Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                                var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                                var countThucHien = 1;
                                contentLog = userName + " " + lstCategoryLog + " thành công";
                                WriteLog writeL = new WriteLog(Uow);
                                writeL.WriteLogGcs(logCategoryId, soId, soGcsId, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maBangKeLich,
                                    countThucHien, logStatus);
                            }
                            else
                            {
                                lstError.Add(soGcsId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = true, message = "Lỗi service ! " + ex.Message }, JsonRequestBehavior.AllowGet);
                    } 
                }
                if (lstSuccess.Count > 0)
                {
                    //return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + " đã lấy thành công." }, JsonRequestBehavior.AllowGet);
                    return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + " đã được đẩy dữ liệu về CMIS thành công!." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công." + " Mã sổ: " + string.Join(",", lstError) + " đang ở trạng thái không phải xuất HHC!" }, JsonRequestBehavior.AllowGet);
                    return Json(new { success = true, message = "Đẩy dữ liệu về CMIS không thành công!"}, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = true, message = "Lỗi ! " + ex.Message}, JsonRequestBehavior.AllowGet);
            }
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
    }
}
