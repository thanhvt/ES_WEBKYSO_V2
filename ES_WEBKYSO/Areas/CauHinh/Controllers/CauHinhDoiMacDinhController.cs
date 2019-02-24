using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ES_WEBKYSO.Models;
using Common.Helpers;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Areas.CauHinh.Models;
using ES_WEBKYSO.Common;

namespace ES_WEBKYSO.Areas.CauHinh.Controllers
{
    public class CauHinhDoiMacDinhController : BaseController
    {
        //
        // GET: /CauHinh/CauHinhDoiMacDinh/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        #region khởi tạo giá trị khi mở trang
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            var dataDM_DOI = Uow.RepoBase<CFG_DOIGCS_NVIEN>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

            List<DM_DOI> data = new List<DM_DOI>();
            foreach (var item in dataDM_DOI)
            {
                DM_DOI doi = new DM_DOI();
                doi.ID = item.ID;
                doi.MA_DVIQLY = item.MA_DVIQLY;
                doi.MA_DOIGCS = item.MA_DOIGCS;
                doi.TEN_DOIGCS = Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == doi.MA_DOIGCS).TEN_DOI;
                doi.MA_NVIEN_GCS = item.USERID;
                doi.TEN_NVIEN_GCS = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == doi.MA_NVIEN_GCS).FullName;
                data.Add(doi);
            }
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllNVIEN()
        {
            var dataUser = Uow.RepoBase<UserProfile>().GetAll().ToList(); //lấy toàn bộ nhân viên
            string str = "";
            foreach (var item in dataUser)
            {
                if (item.UserName.ToLower() != "administrator")
                {
                    str += "<option value=\"" + item.UserId + "\">" + item.FullName + "</option>";
                }
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region customer
        private List<UserProfile> getUserChuaCauHinh(string MaDonVi)
        { //danh sách nhân viên chưa thuộc đội nào
            int DepartmentID = Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentCode == MaDonVi).DepartmentId;
            var listUser = Uow.RepoBase<UserProfile>().GetAll(o => o.DepartmentId == DepartmentID).ToList();
            var listDaCauHinh = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetAll(o => o.MA_DVIQLY == MaDonVi).ToList();
            listUser.RemoveAll(x => listDaCauHinh.Any(y => y.USERID == x.UserId));
            return listUser;
        }
        #endregion

        #region thêm mới cấu hình đội
        public ActionResult Create()
        {
            string MaDViQLy = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Thêm cấu hình đội mặc định";
            ViewBag.MaDviQly = MaDViQLy;
            var listUser = getUserChuaCauHinh(MaDViQLy); //lấy về danh sách nhân viên chưa thuộc đội nào
            ViewData["USERID"] = listUser.Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FullName
            }).ToList();
            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MaDViQLy).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS,
                Text = x.TEN_DOI
            }).ToList();
            //lấy sổ chưa đc cấu hình và chuyển lên combo
            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GetAll(o => o.MA_DVIQLY == MaDViQLy).ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetAll(o => o.MA_DVIQLY == MaDViQLy).ToList();
            dataSOGCS.RemoveAll(x => dataCAUHINH.Any(y => y.MA_SOGCS == x.MA_SOGCS));
            ViewData["MA_SOGCS"] = dataSOGCS.Select(x => new SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS
            }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CFG_DOIGCS_NVIEN model)
        {
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(x => x.MA_DOIGCS == model.MA_DOIGCS && x.MA_DVIQLY == model.MA_DVIQLY && x.USERID == model.USERID);
                if (modelOrig != null)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Cấu hình Đội đã tồn tại";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }

                int kq = Uow.RepoBase<CFG_DOIGCS_NVIEN>().Create(model);

                if (kq == 1)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Thêm mới cấu hình Đội thành công";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }
                else
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "CauHinhDoiMacDinh");
            }

        }
        #endregion

        #region cập nhật cấu hình
        public ActionResult Update(int ID)
        {
            string MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Sửa cấu hình đội mặc định";
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(x => x.ID == ID);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index");
            }
            var listChuaCauHinh = getUserChuaCauHinh(MA_DVIQLY);
            listChuaCauHinh.Add(Uow.RepoBase<UserProfile>().GetOne(model.USERID)); //thêm nhân viên của cấu hình hiện tại vào list chưa cấu hình
            ViewData["USERID"] = listChuaCauHinh.Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FullName,
                Selected = (x.UserId == model.USERID)
            }).ToList();

            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS,
                Text = x.TEN_DOI,
                Selected = (x.MA_DOIGCS == model.MA_DOIGCS)
            }).ToList();

            //ViewBag.IDUser = model.USERID;
            //ViewBag.MaDoi = model.MA_DOIGCS.Trim();
            return View(model);
        }
        public ActionResult Update_sm(CFG_DOIGCS_NVIEN model)
        {
            ViewBag.Title = "Cập nhật cấu hình đội mặc định";
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(x => x.ID == model.ID);
                if (modelOrig == null)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Không tìm thấy bản ghi";
                    return RedirectToAction("Index");
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true); //thay đổi giá trị của model
                if (newModel == modelOrig)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Cập nhật không thành công\nCấu hình đã tồn tại.";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }

                int kq = Uow.RepoBase<CFG_DOIGCS_NVIEN>().Update(newModel);

                if (kq == 1)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Cập nhật cấu hình đội thành công";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }
                else
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "CauHinhDoiMacDinh");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["MessageStatus"] = false;
                TempData["Success"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "CauHinhDoiMacDinh");
            }
        }
        #endregion

        #region xóa cấu hình
        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy cấu hình" }, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(x => x.ID == ID);
            if (model == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy cấu hình" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var ret = Uow.RepoBase<CFG_DOIGCS_NVIEN>().Delete(model);

                if (ret > 0)
                {
                    return Json(new { success = true, message = "Xóa Cấu hình đội mặc định thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xóa Cấu hình mặc định nhân viên GCS không thành công\n" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Xóa Cấu hình đội mặc định không thành công" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
