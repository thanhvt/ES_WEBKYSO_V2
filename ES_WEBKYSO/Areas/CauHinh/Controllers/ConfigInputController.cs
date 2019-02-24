using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Areas.CauHinh.Controllers
{
    public class ConfigInputController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs find)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<CFG_SERVICE_CONFIG>().ManagerGetAllForIndex(find, paging.OrderKey, ref paging).ToList();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int ConfigId)
        {
            ViewBag.Title = "Sửa thông tin cấu hình tham số";
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_SERVICE_CONFIG>().GetOne(x => x.ConfigId == ConfigId);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(CFG_SERVICE_CONFIG model)
        {
            ViewBag.Title = "Cập nhật thông tin cáu hình tham số";
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_SERVICE_CONFIG>().GetOne(x => x.ConfigId == model.ConfigId);
                if (modelOrig == null)
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy bản ghi" }, JsonRequestBehavior.AllowGet);
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true);

                int kq = Uow.RepoBase<CFG_SERVICE_CONFIG>().Update(newModel);

                if (kq == 1)
                {
                    return Json(new { status = true, messenger = "Sửa thông tin cấu hình tham số" }, JsonRequestBehavior.AllowGet);
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
    }
}