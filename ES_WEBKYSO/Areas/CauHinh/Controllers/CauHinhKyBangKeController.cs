using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.Common;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Areas.CauHinh.Models;

namespace ES_WEBKYSO.Areas.CauHinh.Controllers
{
    public class CauHinhKyBangKeController : BaseController
    {
        //
        // GET: /CauHinh/CauHinhKyBangKe/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("ID");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<CFG_BANGKE_DONVI>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //list<cauhinh_dmbk> ds_kybangke = new list<cauhinh_dmbk>();

            //foreach (var item in data)
            //{
            //    cauhinh_dmbk kybangke = new cauhinh_dmbk();
            //    kybangke.id = item.id;
            //    kybangke.ma_dviqly = item.ma_dviqly;
            //    kybangke.ma_loai_bke_dvi = item.ma_loai_bke_dvi;
            //    kybangke.ghi_chu = item.ghi_chu;
            //    kybangke.ten_loai_bke_dvi = UnitOfWork.repobase<d_loai_bangke>().getone(o => o.ma_loaibangke == item.ma_loai_bke_dvi).ten_loaibangke;
            //    ds_kybangke.add(kybangke);
            //}
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            string MaDonVi = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Thêm thông tin ký bảng kê";
            ViewBag.MaDviQly = MaDonVi;
            ViewBag.madonvi = MaDonVi;
            var listBangKe = Uow.RepoBase<D_LOAI_BANGKE>().GetAll().ToList();
            ViewData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
            {
                Value = x.MA_LOAIBANGKE,
                Text = x.TEN_LOAIBANGKE
            }).ToList();
            return View();
        }

        public ActionResult GetAllLoaiBangKe()
        {
            var dataUser = Uow.RepoBase<D_LOAI_BANGKE>().GetAll().ToList(); //lấy toàn bộ loại bảng kê
            string str = "";
            foreach (var item in dataUser)
            {
                str += "<option value=\"" + item.MA_LOAIBANGKE + "\">" + item.TEN_LOAIBANGKE + "</option>";
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CFG_BANGKE_DONVI model)
        {
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_BANGKE_DONVI>().GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_LOAIBANGKE == model.MA_LOAIBANGKE);
                if (modelOrig != null)
                {
                    TempData["Error"] = "Thêm bảng kê không thành công vì bảng kê đã tồn tại";
                    return Redirect("/CauHinh/CauHinhKyBangKe");
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Ký bảng kê đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                int kq = Uow.RepoBase<CFG_BANGKE_DONVI>().Create(model);

                if (kq == 1)
                {
                    TempData["Success"] = "Thêm thành công bảng kê";
                    return Redirect("/CauHinh/CauHinhKyBangKe");
                    //return Json(new { status = true, messenger = "Thêm mới thông tin ký bảng kê thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["Error"] = "Thêm bảng kê thất bại";
                    return Redirect("/CauHinh/CauHinhKyBangKe");
                    //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(int ID)
        {
            ViewBag.Title = "Sửa thông tin ký bảng kê";
            var model = Uow.RepoBase<CFG_BANGKE_DONVI>().GetOne(ID);
            // truyền giá trị sang dropdownlist
            var listBangKe = Uow.RepoBase<D_LOAI_BANGKE>().GetAll().ToList();
            ViewData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
            {
                Value = x.MA_LOAIBANGKE,
                Text = x.TEN_LOAIBANGKE,
                Selected = x.MA_LOAIBANGKE == model.MA_LOAIBANGKE ? true : false
            }).ToList();
            // Lấy model T ra theo id
           
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return Redirect("/CauHinh/CauHinhKyBangKe");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(CFG_BANGKE_DONVI model)
        {
            ViewBag.Title = "Cập nhật thông tin thông tin ký bảng kê";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["Error"] = "";
                return Redirect("/CauHinh/CauHinhKyBangKe");
            }

            var modelOrig = Uow.RepoBase<CFG_BANGKE_DONVI>().GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_LOAIBANGKE == model.MA_LOAIBANGKE && x.ID != model.ID);
            if (modelOrig != null)
            {
                TempData["Error"] = "Sửa không thành công vì ký bảng kê đã tồn tại";
                return Redirect("/CauHinh/CauHinhKyBangKe");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy bản ghi" }, JsonRequestBehavior.AllowGet);
            }

            // var newModel = Utility.ApplyChange(modelOrig, model, true);

            int kq = Uow.RepoBase<CFG_BANGKE_DONVI>().Update(model);

            if (kq == 1)
            {
                TempData["Success"] = "Sưa thành công ký bảng kê";
                return Redirect("/CauHinh/CauHinhKyBangKe");
                //return Json(new { status = true, messenger = "Sửa thông tin ký bảng kê thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Error"] = "Sửa ký bảng kê thất bại do thiếu dữ liệu";
                return Redirect("/CauHinh/CauHinhKyBangKe");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            if (ID.ToString() == null)
            {
                TempData["Error"] = "Không tìm thấy ký bảng kê";
                return Redirect("/CauHinh/CauHinhKyBangKe");
                // return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_BANGKE_DONVI>().GetOne(x => x.ID == ID);
            if (model == null)
            {
                TempData["Error"] = "Xóa xảy ra lỗi vì không tìm thấy bảng kê";
                return Redirect("/CauHinh/CauHinhKyBangKe");
                //return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var ret = Uow.RepoBase<CFG_BANGKE_DONVI>().Delete(model);

                if (ret > 0)
                {
                    TempData["Success"] = "Xóa thành công ký bảng kê";
                    return Redirect("/CauHinh/CauHinhKyBangKe");
                    //return Json(new { success = true, message = "Xóa thông thông tin ký bảng kê thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                // ignored
            }
            TempData["Error"] = "Xóa ký bảng kê thất bại ";
            return Redirect("/CauHinh/CauHinhKyBangKe");
            //return Json(new { success = false, message = "Xóa thông tin bảng kê không thành công" }, JsonRequestBehavior.AllowGet);
        }

    }
}
