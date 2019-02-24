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
    public class DanhMucDoiController : BaseController
    {
        //
        // GET: /DanhMucHeThong/DanhMucDoi/

        public ActionResult Index()
        {
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<D_DOIGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Thêm thông tin đội";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        [HttpPost]
        public ActionResult Create(D_DOIGCS model)
        {
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<D_DOIGCS>().GetOne(x => x.MA_DOIGCS == model.MA_DOIGCS);
                if (modelOrig != null)
                {
                    //TempData["MessageStatus"] = false;
                    //TempData["Error"] = "Xảy ra lỗi: Đội đã tồn tại";
                    //return RedirectToAction("Index");
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Thêm mới không thành công: Đã có mã đội này trong hệ thống, vui lòng kiểm tra lại";
                    return RedirectToAction("Index", "DanhMucDoi");
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Đội đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                int kq = Uow.RepoBase<D_DOIGCS>().Create(model);

                if (kq == 1)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Thêm mới thông tin sổ thành công";
                    return RedirectToAction("Index", "DanhMucDoi");
                }
                else
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "DanhMucDoi");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["MessageStatus"] = true;
                TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "DanhMucDoi");
            }
        }

        public ActionResult Update(string maDoi)
        {
            ViewBag.Title = "Sửa thông tin sổ đội";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_DOIGCS>().GetOne(x => x.MA_DOIGCS == maDoi);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index", "DanhMucDoi");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(D_DOIGCS model)
        {
            ViewBag.Title = "Cập nhật thông tin thông tin đội";
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<D_DOIGCS>().GetOne(x => x.MA_DOIGCS == model.MA_DOIGCS);
                if (modelOrig == null)
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy bản ghi" }, JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Không tìm thấy bản ghi";
                    return RedirectToAction("Index", "DanhMucDoi");
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true);

                int kq = Uow.RepoBase<D_DOIGCS>().Update(newModel);

                if (kq == 1)
                {
                    //return Json(new { status = true, messenger = "Sửa thông tin đội thành công" }, JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Cập nhật thông tin đội thành công";
                    return RedirectToAction("Index", "DanhMucDoi");
                }
                else
                {
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Không có bản ghi nào cần cập nhật";
                    return RedirectToAction("Index", "DanhMucDoi");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "DanhMucDoi");
            }
        }
        [HttpPost]
        public ActionResult Delete(string maDoi)
        {
            if (maDoi == null)
            {
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Không tìm thấy mã đội";
                return RedirectToAction("Index", "DanhMucDoi");
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<D_DOIGCS>().GetOne(x => x.MA_DOIGCS == maDoi);
            if (model == null)
            {
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Không tìm thấy mã đội";
                return RedirectToAction("Index", "DanhMucDoi");
            }
            try
            {
                var ret = Uow.RepoBase<D_DOIGCS>().Delete(model);

                if (ret > 0)
                {
                    //return Json(new { success = true, message = "Xóa thông thông tin đội thành công" }, JsonRequestBehavior.AllowGet);
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Xóa thông thông tin đội thành công";
                    return RedirectToAction("Index", "DanhMucDoi");
                }
            }
            catch
            {
                // ignored
            }
            //return Json(new { success = false, message = "Xóa thông tin đội không thành công" }, JsonRequestBehavior.AllowGet);
            TempData["MessageStatus"] = false;
            TempData["Success"] = "Xóa thông thông tin đội không thành công";
            return RedirectToAction("Index", "DanhMucDoi");
        }

    }
}
