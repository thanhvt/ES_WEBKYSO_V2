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
using ES_WEBKYSO.Repository;

namespace ES_WEBKYSO.Areas.DanhMucHeThong.Controllers
{
    public class QuanLySoGcsController : BaseController
    {
        //
        // GET: /DanhMucHeThong/QuanLyD_SOGCS/
        #region Declaration

        private readonly CmisRepository _cmisRepository;

        #endregion


        #region Constructor

        public QuanLySoGcsController()
        {
            _cmisRepository = new CmisRepository();
        }

        #endregion
        public ActionResult Index()
        {
            ViewBag.Title = "Thông tin sổ GCS";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //var data = new Paging();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Thêm thông tin sổ GCS";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            ViewBag.LoaiSo = new List<SelectListItem> {
                 new SelectListItem { Value = "TP" , Text = "TP" },
                 new SelectListItem { Value = "KH" , Text = "KH" },
                 new SelectListItem { Value = "DN" , Text = "DN" }
             };
            ViewBag.HinhThuc = new List<SelectListItem> {
                 new SelectListItem { Value = "MDMS" , Text = "MDMS" },
                 new SelectListItem { Value = "MTB" , Text = "MTB" },
                 new SelectListItem { Value = "HHU" , Text = "HHU" }
             };
            //ViewBag.TinhTrang = new List<SelectListItem>
            //{
            //    new SelectListItem {Text = "DHD", Value = "DHD"},
            //    new SelectListItem {Text = "KHD", Value = "KHD"}
            //};

            return View();
        }

        [HttpPost]
        public ActionResult Create(D_SOGCS model)
        {
            if (ModelState.IsValid)
            {
                var modelOrig =
                    Uow.RepoBase<D_SOGCS>()
                        .GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_SOGCS == model.MA_SOGCS);
                if (modelOrig != null)
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Sổ đã tồn tại" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Sổ đã tồn tại";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }

                int kq = Uow.RepoBase<D_SOGCS>().Create(model);

                if (kq == 1)
                {
                    //return Json(new { status = true, messenger = "Thêm mới thông tin sổ thành công" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Thêm mới thông tin sổ thành công";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }
                else
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "QuanLySoGcs");
            }
        }

        public ActionResult Update(string soGcsId)
        {
            ViewBag.Title = "Sửa thông tin sổ GCS";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_SOGCS == soGcsId);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index");
            }
            ViewBag.LoaiSo = new List<SelectListItem> {
                 new SelectListItem { Value = "TP" , Text = "TP" },
                 new SelectListItem { Value = "KH" , Text = "KH" },
                 new SelectListItem { Value = "DN" , Text = "DN" }
             };
            ViewBag.HinhThuc = new List<SelectListItem> {
                 new SelectListItem { Value = "MDMS" , Text = "MDMS" },
                 new SelectListItem { Value = "MTB" , Text = "MTB" },
                 new SelectListItem { Value = "HHU" , Text = "HHU" }
             };
            //ViewBag.TinhTrang = new List<SelectListItem> {
            //     new SelectListItem { Value = "0" , Text = "Không hoạt động" },
            //     new SelectListItem { Value = "1" , Text = "Đang hoạt động" },
            // };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(D_SOGCS model)
        {
            ViewBag.Title = "Cập nhật thông tin thông tin sổ";
            //ViewData["D_SOGCS"] = UnitOfWork.RepoBase<D_SOGCS>().GetAll().ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ID.ToString(),
            //    Text = x.TEN_D_SOGCS
            //}).ToList();
            if (ModelState.IsValid)
            {
                var modelOrig =
                    Uow.RepoBase<D_SOGCS>()
                        .GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_SOGCS == model.MA_SOGCS);
                if (modelOrig == null)
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy bản ghi" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Xảy ra lỗi: Không tìm thấy bản ghi";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true);

                int kq = Uow.RepoBase<D_SOGCS>().Update(newModel);

                if (kq == 1)
                {
                    //return Json(new { status = true, messenger = "Sửa thông tin sổ thành công" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Sửa thông tin sổ thành công";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }
                else
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" },
                //    JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "QuanLySoGcs");
            }
        }

        [HttpPost]
        public ActionResult Delete(string soGcsId)
        {
            if (soGcsId == null)
            {
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" },
                //    JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Xảy ra lỗi: Xảy ra lỗi: Không tìm thấy sổ";
                return RedirectToAction("Index", "QuanLySoGcs");
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_SOGCS == soGcsId);
            if (model == null)
            {
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" },
                //    JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Xảy ra lỗi: Xảy ra lỗi: Không tìm thấy sổ";
                return RedirectToAction("Index", "QuanLySoGcs");
            }
            try
            {
                var ret = Uow.RepoBase<D_SOGCS>().Delete(model);

                if (ret > 0)
                {
                    //return Json(new { success = true, message = "Xóa thông thông tin sổ thành công" },
                    //    JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Xóa thông thông tin sổ thành công";
                    return RedirectToAction("Index", "QuanLySoGcs");
                }
            }
            catch
            {
                // ignored
            }
            //return Json(new { success = false, message = "Xóa thông tin hợp đồng cung cấp tài nguyên không thành công" },
            //    JsonRequestBehavior.AllowGet);
            TempData["MessageStatus"] = false;
            TempData["Success"] = "Xảy ra lỗi: Xóa thông tin sổ không thành công";
            return RedirectToAction("Index", "QuanLySoGcs");
        }

        [HttpPost]
        public ActionResult JsonDataSyncCmis(FindModelGcs findModel, string search, string maDvQly)
        {
            //var lstS0Gcs = Uow.RepoBase<D_SOGCS>().GetAll().Where(i => i.MA_DVIQLY == maDvQly);
            //var configInput = Uow.RepoBase<CFG_SERVICE_CONFIG>().GetOne(x => x.TypeInput == "SVTONTHAT");
            //Service_TonThat.Service_TonThat ser = new Service_TonThat.Service_TonThat();
            //if (configInput != null)
            //{
            //    ser.Url = configInput.Value;
            //}
            //else
            //{
            //    return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
            //        JsonRequestBehavior.AllowGet);
            //}

            ////var getSoFromCmis = ser.ExecuteQuery("Select * from D_SOGCS");

            ////foreach (DataRow lstSo in getSoFromCmis.Tables[0].Rows)
            ////{
            ////var lstS0GcsValue = UnitOfWork.RepoBase<D_SOGCS>().GetAll().Where(i => i.MA_DVIQLY == maDvQly &&  i.MA_SOGCS == lstSo["MA_SOGCS"].ToString());

            ////    if (lstS0GcsValue.Count() == 0)
            ////    {
            ////        model.MA_SOGCS = lstSo["MA_SOGCS"].ToString();
            ////    }

            ////}

            //return Json(new { success = true, message = "Đồng bộ CMIS thành công!" },
            //        JsonRequestBehavior.AllowGet);
            var strURL_CMISInterface = "";
            var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVTONTHAT");
            //Service_TonThat.Service_TonThat ser = new Service_TonThat.Service_TonThat();
            if (configInput != null)
            {
                strURL_CMISInterface = configInput.Value;
            }
            else
            {
                return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
                    JsonRequestBehavior.AllowGet);
            }

            _cmisRepository.strURL_CMISInterface = strURL_CMISInterface + "getDanhMuc";
            var dsSoFromCmis = _cmisRepository.GetSoFromCmis(maDvQly);
            if (dsSoFromCmis == null || dsSoFromCmis.Tables.Count <= 0 || dsSoFromCmis.Tables[0].Rows.Count <= 0)
            {
                return Json(new { success = false, message = "Không có dữ liệu!" },
                    JsonRequestBehavior.AllowGet);
            }
            var dtSoFromCmis = dsSoFromCmis.Tables[0];

            foreach (DataRow rows in dtSoFromCmis.Rows)
            {
                string getDviQly = rows["maDviqly"].ToString();
                string getMaSo = rows["maSogcs"].ToString();
                var soGcs = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_DVIQLY == getDviQly && o.MA_SOGCS == getMaSo);

                if (soGcs == null)
                {
                    D_SOGCS model = new D_SOGCS();
                    model.MA_DVIQLY = rows["maDviqly"].ToString();
                    model.MA_SOGCS = rows["maSogcs"].ToString();
                    model.TEN_SOGCS = rows["tenSogcs"].ToString();
                    model.SO_KY = (rows["soKy"] == null ? 0 : Convert.ToInt32(rows["soKy"]));
                    model.TINH_TRANG = (rows["tinhTrang"] == null ? 0 : Convert.ToInt32(rows["tinhTrang"]));
                    model.TRANG_THAI = "CL";
                    model.LOAI_SOGCS = rows["loaiSogcs"].ToString();
                    if (rows["ngayGhi"].ToString() == "")
                    {
                        model.NGAY_GHI = 0;
                    }
                    else
                    {
                        int result;
                        int.TryParse(rows["ngayGhi"].ToString(), out result);
                        model.NGAY_GHI = result;
                        Uow.RepoBase<D_SOGCS>().Create(model);
                    }
                }

            }

            //foreach (DataRow rows in dtSoFromCmis.Rows)
            //{
            //    string getDviQly = rows["MA_DVIQLY"].ToString();
            //    string getMaSo = rows["MA_SOGCS"].ToString();
            //    var soGcs = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_DVIQLY == getDviQly && o.MA_SOGCS == getMaSo);

            //    if (soGcs == null)
            //    {
            //        D_SOGCS model = new D_SOGCS();
            //        model.MA_DVIQLY = rows["MA_DVIQLY"].ToString();
            //        model.MA_SOGCS = rows["MA_SOGCS"].ToString();
            //        model.TEN_SOGCS = rows["TEN_SOGCS"].ToString();
            //        model.SO_KY = (rows["SO_KY"] == null ? 0 : Convert.ToInt32(rows["SO_KY"]));
            //        model.TINH_TRANG = (rows["TINH_TRANG"] == null ? 0 : Convert.ToInt32(rows["TINH_TRANG"]));
            //        model.TRANG_THAI = "CL";
            //        model.LOAI_SOGCS = rows["LOAI_SOGCS"].ToString();
            //        if (rows["NGAY_GHI"].ToString() == "")
            //        {
            //            model.NGAY_GHI = 0;
            //        }
            //        else
            //        {
            //            int result;
            //            int.TryParse(rows["NGAY_GHI"].ToString(), out result);
            //            model.NGAY_GHI = result;
            //            Uow.RepoBase<D_SOGCS>().Create(model);
            //        }
            //    }

            //}
            return Json(new { success = true, message = "Đồng bộ CMIS thành công!" },
                    JsonRequestBehavior.AllowGet);
        }
    }

}
