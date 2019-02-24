using Common.Helpers;
using ES_WEBKYSO.Areas.CauHinh.Models;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.CauHinh.Controllers
{
    public class CauHinhBoPhanKyController : BaseController
    {
        Result result = new Result();
        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            // truyền giá trị sang dropdownlist
            string madonvi = ViewBag.MA_DVIQLY.ToString();
            var listBangKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll()
               .Join(Uow.RepoBase<D_LOAI_BANGKE>().GetAll(),
                  o => o.MA_LOAIBANGKE,
                  d => d.MA_LOAIBANGKE,
                  (o, d) => new {
                      MA_LOAIBANGKE = o.MA_LOAIBANGKE,
                      TEN_LOAIBANGKE = d.TEN_LOAIBANGKE,
                      MA_DVIQLY = o.MA_DVIQLY
                  }
               ).Where(m => m.MA_DVIQLY == madonvi)
               .ToList();
            if(TempData["MA_LOAI_BKE_DVI"] == null)
            {
                ViewData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
                {
                    Value = x.MA_LOAIBANGKE,
                    Text = x.TEN_LOAIBANGKE,
                    Selected = x.MA_LOAIBANGKE == "BKCS" ? true : false
                }).ToList();
            }
           else
            {
                ViewData["MA_LOAI_BKE_DVI"] = TempData["MA_LOAI_BKE_DVI"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(BOPHANKY boPhanKy)
        {
            var paging = Request.Params.ToPaging("ID");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<CFG_BOPHAN_KY>().ManagerGetAllForIndex(boPhanKy, paging.OrderKey, ref paging).ToList();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            string madonvi = ViewBag.MA_DVIQLY.ToString();
            var listBangKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll()
                .Join(Uow.RepoBase<D_LOAI_BANGKE>().GetAll(),
                   o => o.MA_LOAIBANGKE,
                   d => d.MA_LOAIBANGKE,
                   (o, d) => new {
                       MA_LOAIBANGKE = o.MA_LOAIBANGKE,
                       TEN_LOAIBANGKE = d.TEN_LOAIBANGKE,
                       MA_DVIQLY = o.MA_DVIQLY
                   }
                ).Where(m => m.MA_DVIQLY == madonvi)
                .ToList();
            var listBoPhanKy = Uow.RepoBase<WebpagesRoles>().GetAll().ToList();
            ViewData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
            {
                Value = x.MA_LOAIBANGKE,
                Text = x.TEN_LOAIBANGKE
            }).ToList();
            ViewData["MA_BO_PHAN_KY"] = listBoPhanKy.Select(x => new SelectListItem
            {
                Value = x.RoleId.ToString(),
                Text = x.RoleName.ToString()
            }).ToList();
            int size = listBoPhanKy.Count();
            List<SelectListItem> list_item = new List<SelectListItem>();
            for (int i = 1; i <= size; i++)
            {
                SelectListItem item = new SelectListItem();
                item.Value = i.ToString();
                item.Text = i.ToString();
                list_item.Add(item);
            }
            ViewData["THUTU_KY"] = list_item.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CFG_BOPHAN_KY model)
        {
            if (ModelState.IsValid)
            {
                string madonvi = model.MA_DVIQLY.ToString();
                var listBangKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll()
                    .Join(Uow.RepoBase<D_LOAI_BANGKE>().GetAll(),
                       o => o.MA_LOAIBANGKE,
                       d => d.MA_LOAIBANGKE,
                       (o, d) => new {
                           MA_LOAIBANGKE = o.MA_LOAIBANGKE,
                           TEN_LOAIBANGKE = d.TEN_LOAIBANGKE,
                           MA_DVIQLY = o.MA_DVIQLY
                       }
                    ).Where(m => m.MA_DVIQLY == madonvi)
                    .ToList();
                TempData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
                {
                    Value = x.MA_LOAIBANGKE,
                    Text = x.TEN_LOAIBANGKE,
                    Selected = x.MA_LOAIBANGKE == model.MA_LOAIBANGKE ? true : false
                }).ToList();
                var modelOrig = Uow.RepoBase<CFG_BOPHAN_KY>().GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_LOAIBANGKE == model.MA_LOAIBANGKE && x.RoleId == model.RoleId);
                if (modelOrig != null)
                {
                    TempData["Error"] = "Thêm bộ phận ký không thành công vì bộ phận đã tồn tại";
                    return Redirect("/CauHinh/CauHinhBoPhanKy");

                }

                int kq = Uow.RepoBase<CFG_BOPHAN_KY>().Create(model);

                if (kq == 1)
                {
                    TempData["Success"] = "Thêm thành công bộ phận ký";
                    return Redirect("/CauHinh/CauHinhBoPhanKy");

                }
                else
                {
                    TempData["Error"] = "Thêm bộ phận ký thất bại";
                    return Redirect("/CauHinh/CauHinhBoPhanKy");

                }

            }
            else
            {
                TempData["Error"] = "Xảy ra lỗi: thiếu dữ liệu ";
                return Redirect("/CauHinh/CauHinhBoPhanKy");

            }
        }

        public ActionResult Update(int ID)
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Sửa thông tin bộ phận ký";
            var model = Uow.RepoBase<CFG_BOPHAN_KY>().GetOne(ID);
            // truyền giá trị sang dropdownlist
            string madonvi = ViewBag.MA_DVIQLY.ToString();
            var listBangKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll()
               .Join(Uow.RepoBase<D_LOAI_BANGKE>().GetAll(),
                  o => o.MA_LOAIBANGKE,
                  d => d.MA_LOAIBANGKE,
                  (o, d) => new {
                      MA_LOAIBANGKE = o.MA_LOAIBANGKE,
                      TEN_LOAIBANGKE = d.TEN_LOAIBANGKE,
                      MA_DVIQLY = o.MA_DVIQLY
                  }
               ).Where(m => m.MA_DVIQLY == madonvi)
               .ToList();
            var listBoPhanKy = Uow.RepoBase<WebpagesRoles>().GetAll().ToList();
            ViewData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
            {
                Value = x.MA_LOAIBANGKE,
                Text = x.TEN_LOAIBANGKE,
                Selected = x.MA_LOAIBANGKE == model.MA_LOAIBANGKE ? true : false
            }).ToList();

            ViewData["MA_BO_PHAN_KY"] = listBoPhanKy.Select(x => new SelectListItem
            {
                Value = x.RoleId.ToString(),
                Text = x.RoleName.ToString(),
                Selected = x.RoleId == model.RoleId ? true : false
            }).ToList();
            int size = listBoPhanKy.Count();
            List<SelectListItem> list_item = new List<SelectListItem>();
            for (int i = 1; i <= size; i++)
            {
                SelectListItem item = new SelectListItem();
                item.Value = i.ToString();
                item.Text = i.ToString();
                list_item.Add(item);
            }
            foreach (var item in list_item)
            {
                if (item.Value == model.THU_TUKY.ToString())
                    item.Selected = true;
                else
                    item.Selected = false;
            }
            ViewData["THUTU_KY"] = list_item.ToList();

            if (model == null)
            {
                result.Status = false;
                result.Message = "Xảy ra lỗi: Không tìm thấy bản ghi";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(CFG_BOPHAN_KY model)
        {
            ViewBag.Title = "Cập nhật thông tin thông tin ký bảng kê";
            var listBangKe = Uow.RepoBase<CFG_BANGKE_DONVI>().GetAll()
                .Join(Uow.RepoBase<D_LOAI_BANGKE>().GetAll(),
                   o => o.MA_LOAIBANGKE,
                   d => d.MA_LOAIBANGKE,
                   (o, d) => new {
                       MA_LOAIBANGKE = o.MA_LOAIBANGKE,
                       TEN_LOAIBANGKE = d.TEN_LOAIBANGKE,
                       MA_DVIQLY = o.MA_DVIQLY
                   }
                ).Where(m => m.MA_DVIQLY == model.MA_DVIQLY)
                .ToList();
            TempData["MA_LOAI_BKE_DVI"] = listBangKe.Select(x => new SelectListItem
            {
                Value = x.MA_LOAIBANGKE,
                Text = x.TEN_LOAIBANGKE,
                Selected = x.MA_LOAIBANGKE == model.MA_LOAIBANGKE ? true : false
            }).ToList();
            var modelOrig = Uow.RepoBase<CFG_BOPHAN_KY>().GetOne(x => x.MA_DVIQLY == model.MA_DVIQLY && x.MA_LOAIBANGKE == model.MA_LOAIBANGKE && x.RoleId == model.RoleId && x.THU_TUKY == model.THU_TUKY && x.MO_TA == model.MO_TA);
            if (modelOrig != null)
            {
                TempData["Error"] = "Sửa không thành công vì ký bảng kê đã tồn tại";
                return Redirect("/CauHinh/CauHinhBoPhanKy");
            }
           
            int kq = Uow.RepoBase<CFG_BOPHAN_KY>().Update(model);

            if (kq == 1)
            {
                TempData["Success"] = "Sửa thành công ký bảng kê";
                return Redirect("/CauHinh/CauHinhBoPhanKy");
            }
            else
            {
                TempData["Error"] = "Sửa ký bảng kê thất bại do thiếu dữ liệu";
                return Redirect("/CauHinh/CauHinhBoPhanKy");
            }
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            if (ID.ToString() == null)
            {
                result.Status = false;
                result.Message = "Xảy ra lỗi: Không tìm thấy bản ghi";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_BOPHAN_KY>().GetOne(ID);
            if (model == null)
            {
                result.Status = false;
                result.Message = "Xảy ra lỗi: Không tìm thấy bản ghi";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var ret = Uow.RepoBase<CFG_BOPHAN_KY>().Delete(model);

                if (ret > 0)
                {
                    result.Status = true;
                    result.Message = "Xóa bộ phận ký thành công";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                // ignored
            }
            result.Status = false;
            result.Message = "Xảy ra lỗi: Không tìm thấy bản ghi";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
