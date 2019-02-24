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

namespace ES_WEBKYSO.Areas.DanhMucHeThong.Controllers
{
    public class PhanQuyenUserImeiController : BaseController
    {
        //
        // GET: /DanhMucHeThong/PhanQuyenUserIMEI/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<D_IMEI>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Thêm thông tin User-IMEI";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        [HttpPost]
        public ActionResult Create(D_IMEI model)
        {
            ViewBag.Title = "Thêm thông tin User-IMEI";
            if (model.NgayCapString != null)
            {
                DateTime tryParseForCod;
                if (DateTime.TryParseExact(model.NgayCapString, "d'/'M'/'yyyy", CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out tryParseForCod))
                {
                    model.NGAY_CAP = tryParseForCod;
                }
                else
                {
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<D_IMEI>().GetOne(x => x.ID == model.ID && x.IMEI == model.IMEI);
                if (modelOrig != null)
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Sổ đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                int kq = Uow.RepoBase<D_IMEI>().Create(model);

                if (kq == 1)
                {
                    return Json(new { status = true, messenger = "Thêm mới thông tin User-IMEI thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(int? D_IMEIId)
        {
            ViewBag.Title = "Sửa thông tin sổ GCS";
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_IMEI>().GetOne(x => x.ID == D_IMEIId.Value);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index");
            }
            model.NgayCapString = model.NGAY_CAP?.ToString("dd'/'MM'/'yyyy") ?? "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(D_IMEI model)
        {
            ViewBag.Title = "Cập nhật thông tin thông tin sổ";
            if (model.NgayCapString != null)
            {
                DateTime tryParseForCod;
                if (DateTime.TryParseExact(model.NgayCapString, "d'/'M'/'yyyy", CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out tryParseForCod))
                {
                    model.NGAY_CAP = tryParseForCod;
                }
                else
                {
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<D_IMEI>().GetOne(x => x.ID == model.ID);
                if (modelOrig == null)
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy bản ghi" }, JsonRequestBehavior.AllowGet);
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true);

                int kq = Uow.RepoBase<D_IMEI>().Update(newModel);

                if (kq == 1)
                {
                    return Json(new { status = true, messenger = "Sửa thông tin User-IMEI thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int? D_IMEIId)
        {
            if (D_IMEIId == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_IMEI>().GetOne(x => x.ID == D_IMEIId.Value);
            if (model == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var ret = Uow.RepoBase<D_IMEI>().Delete(model);

                if (ret > 0)
                {
                    return Json(new { success = true, message = "Xóa thông thông tin User-IMEI thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                // ignored
            }
            return Json(new { success = false, message = "Xóa thông tin phân quyền User-IMEI không thành công" }, JsonRequestBehavior.AllowGet);
        }
    }
}
