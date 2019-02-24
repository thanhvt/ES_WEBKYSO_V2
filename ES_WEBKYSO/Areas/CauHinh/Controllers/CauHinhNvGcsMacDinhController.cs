using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ES_WEBKYSO.Repository.ServiceRepository.QuanTriHeThong;
using ES_WEBKYSO.Areas.CauHinh.Models;

namespace ES_WEBKYSO.Areas.CauHinh.Controllers
{
    public class CauHinhNvGcsMacDinhController : BaseController
    {
        //
        // GET: /CauHinh/CauHinhNVGcsMacDinh/

        public ActionResult Index()
        {
            return View();
        }
        #region lấy dữ liệu khởi tạo khi mở trang
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            string MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            List<CAUHINH_NVGCS> ListCauHinh = new List<CAUHINH_NVGCS>();
            try
            {
                findModel.MaDonVi = MA_DVIQLY;
                var paging = Request.Params.ToPaging("CAUHINH_GCS");
                var dataNV = Uow.RepoBase<CFG_SOGCS_NVIEN>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

                var dataSOGCS = Uow.RepoBase<D_SOGCS>().GETALL().ToList(); //lấy toàn bộ danh sách sổ                
                var dataUser = Uow.RepoBase<UserProfile>().GetAll().ToList(); //lấy toàn bộ nhân viên

                //join giữa các bảng
                var joinProfile = dataNV.Join(dataUser, d1 => d1.USERID, d2 => d2.UserId, (SOGCS_NV, UserProFile) => new { SOGCS_NV, UserProFile }).ToList();
                var data = (from SOGCS in dataSOGCS
                            join SOGCS_NV in joinProfile on SOGCS.MA_SOGCS equals SOGCS_NV.SOGCS_NV.MA_SOGCS// into grouping
                            //from SOGCS_NV in grouping.DefaultIfEmpty()
                            select new { SOGCS, SOGCS_NV }).ToList();
                foreach (var item in data)
                {
                    CAUHINH_NVGCS cauhinh = new CAUHINH_NVGCS();
                    cauhinh.MA_SOGCS_NVIEN = item.SOGCS_NV.SOGCS_NV.MA_SOGCS_NVIEN;
                    cauhinh.MA_DVIQLY = item.SOGCS_NV.SOGCS_NV.MA_DVIQLY;
                    cauhinh.MA_SOGCS = item.SOGCS_NV.SOGCS_NV.MA_SOGCS;
                    cauhinh.TEN_SOGCS = item.SOGCS.TEN_SOGCS;
                    cauhinh.MA_DOIGCS = item.SOGCS_NV.SOGCS_NV.MA_DOIGCS;
                    cauhinh.MA_NVIEN_GCS = item.SOGCS_NV.SOGCS_NV.USERID;
                    cauhinh.TEN_NVIEN_GCS = item.SOGCS_NV.UserProFile.FullName;
                    ListCauHinh.Add(cauhinh);
                }
                paging.data = ListCauHinh;
                return Json(paging, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Lỗi lấy dữ liệu! " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllNVIEN()
        {
            //var dataUser = UnitOfWork.RepoBase<UserProfile>().GETALL().ToList(); //lấy toàn bộ nhân viên
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

        [HttpPost]
        public JsonResult GetNV_GCS_TheoDoi(string MaDoi)
        {
            try
            {
                MaDoi = MaDoi.Trim();
                FindModelGcs findModel = new FindModelGcs();
                findModel.MaDoi = MaDoi;
                findModel.MaDonVi = new CommonUserProfile().MA_DVIQLY;
                var listNVGCS = Uow.RepoBase<CFG_DOIGCS_NVIEN>().ManagerGetAllForIndex(findModel, MaDoi);

                List<CAUHINH_NVGCS> listUserProfile = new List<CAUHINH_NVGCS>();
                foreach (CFG_DOIGCS_NVIEN item in listNVGCS)
                {
                    var NVGCS = Uow.RepoBase<UserProfile>().GetOne(x => x.UserId == item.USERID); //(x => x.UserId == item.USERID);
                    CAUHINH_NVGCS cauhinh = new CAUHINH_NVGCS();
                    cauhinh.MA_NVIEN_GCS = NVGCS.UserId;
                    cauhinh.TEN_NVIEN_GCS = NVGCS.FullName;
                    listUserProfile.Add(cauhinh);
                }
                return Json(listUserProfile, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        #endregion

        #region Tạo mới cấu hình
        public ActionResult Create()
        {
            string MaDiQLy = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Thêm cấu hình nhân viên Ghi chỉ số mặc định";
            ViewBag.MaDviQly = MaDiQLy;
            ViewData["USERID"] = new List<UserProfile>() //UnitOfWork.RepoBase<UserProfile>().GetAll().ToList().
            .Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FullName
            }).ToList();
            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MaDiQLy).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS,
                Text = x.TEN_DOI
            }).ToList();
            //lấy sổ chưa đc cấu hình và chuyển lên combo
            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GetAll(o => o.MA_DVIQLY == MaDiQLy).ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetAll(o => o.MA_DVIQLY == MaDiQLy).ToList();
            dataSOGCS.RemoveAll(x => dataCAUHINH.Any(y => y.MA_SOGCS == x.MA_SOGCS));
            ViewData["MA_SOGCS"] = dataSOGCS.Select(x => new SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS
            }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CFG_SOGCS_NVIEN model)
        {
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_DOIGCS == model.MA_DOIGCS && x.MA_DVIQLY == model.MA_DVIQLY && x.MA_SOGCS == model.MA_SOGCS && x.USERID == model.USERID);
                if (modelOrig != null)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Cấu hình Nhân viên ghi chỉ số đã tồn tại";
                    return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
                }

                int kq = Uow.RepoBase<CFG_SOGCS_NVIEN>().Create(model);

                if (kq == 1)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Thêm mới cấu hình Nhân viên ghi chỉ số thành công";
                    return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
                }
                else
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
            }

        }
        #endregion
        #region cập nhật cấu hình
        public ActionResult Update(int MA_SOGCS_NVIEN)
        {
            string MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            ViewBag.Title = "Sửa cấu hình nhân viên Ghi chỉ số mặc định";
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_SOGCS_NVIEN == MA_SOGCS_NVIEN);
            if (model == null)
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Không tìm thấy bản ghi";
                return RedirectToAction("Index");
            }
            ViewData["USERID"] = new List<UserProfile>() //UnitOfWork.RepoBase<UserProfile>().GetAll().ToList().
            .Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FullName
            }).ToList();
            ViewData["MA_DOIGCS"] = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY).ToList().Select(x => new SelectListItem
            {
                Value = x.MA_DOIGCS,
                Text = x.TEN_DOI,
                Selected = (x.MA_DOIGCS == model.MA_DOIGCS)
            }).ToList();
            //lấy sổ chưa đc cấu hình và chuyển lên combo
            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY).ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY).ToList();
            dataSOGCS.RemoveAll(x => dataCAUHINH.Any(y => y.MA_SOGCS == x.MA_SOGCS));
            dataSOGCS.Add(Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == model.MA_SOGCS && o.MA_DVIQLY == model.MA_DVIQLY));
            ViewData["MA_SOGCS"] = dataSOGCS.Select(x => new SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS,
                Selected = (x.MA_SOGCS == model.MA_SOGCS)
            }).ToList();
            ViewBag.IDUser = model.USERID;
            ViewBag.MaDoi = model.MA_DOIGCS.Trim();
            return View(model);
        }
        public ActionResult Update_sm(CFG_SOGCS_NVIEN model)
        {
            ViewBag.Title = "Cập nhật cấu hình nhân viên Ghi chỉ số mặc định";
            if (ModelState.IsValid)
            {
                var modelOrig = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_SOGCS_NVIEN == model.MA_SOGCS_NVIEN);
                if (modelOrig == null)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Không tìm thấy bản ghi";
                    return RedirectToAction("Index");
                }

                var newModel = Utility.ApplyChange(modelOrig, model, true); //thay đổi giá trị của model

                int kq = Uow.RepoBase<CFG_SOGCS_NVIEN>().Update(newModel);

                if (kq == 1)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Cập nhật cấu hình Nhân viên ghi chỉ số thành công";
                    return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
                }
                else
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                    return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi thiếu hoặc sai dữ liệu nhập vào");
                TempData["MessageStatus"] = false;
                TempData["Success"] = "Xảy ra lỗi: Lỗi thiếu hoặc sai dữ liệu nhập vào";
                return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
            }
        }
        #endregion
        #region xóa cấu hình
        [HttpPost]
        public ActionResult Delete(int? MA_SOGCS_NVIEN)
        {
            if (MA_SOGCS_NVIEN == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            // Lấy model T ra theo id
            var model = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_SOGCS_NVIEN == MA_SOGCS_NVIEN);
            if (model == null)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var ret = Uow.RepoBase<CFG_SOGCS_NVIEN>().Delete(model);

                if (ret > 0)
                {
                    return Json(new { success = true, message = "Xóa Cấu hình mặc định nhân viên GCS thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                // ignored
            }
            return Json(new { success = false, message = "Xóa Cấu hình mặc định nhân viên GCS không thành công" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
