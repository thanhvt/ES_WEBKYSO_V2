using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Configuration;
using Administrator.Library;
using Administrator.Department.Models;
using Administrator.Department.Helpers;
using PagedList;

namespace Administrator.Controllers
{
    public class DepartmentController : Controller
    {
        private int PageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        private DepartmentUnitOfWork departmentUnitOfWork = new DepartmentUnitOfWork();

        #region Trang list đơn vị
        [AuthorizeRolesLibrary]
        public ActionResult DepartmentManager([DefaultValue(1)] int page, [DefaultValue("")] string search)
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            
            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;            
            
            //Nếu là đơn vị cấp tổng
            if (departmentId == 0)
            {
                StaticPagedList<Administrator.Department.Models.Administrator_Department> model = new StaticPagedList<Administrator.Department.Models.Administrator_Department>(
                     helper.SetLevelParentChildren(departmentUnitOfWork.Administrator_DepartmentRepository.Get(filter: q => q.DepartmentName.Contains(search),
                     orderBy: q => q.OrderBy(item => item.DepartmentIndex), page: page, pagesize: PageSize), 0),
                     page, PageSize, departmentUnitOfWork.Administrator_DepartmentRepository.TotalRows(filter: q => q.DepartmentName.Contains(search)));

                return View(model);
            }
            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            else
            {
                var listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(orderBy: q => q.OrderBy(item => item.DepartmentIndex));
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());

                StaticPagedList<Administrator.Department.Models.Administrator_Department> model = new StaticPagedList<Administrator.Department.Models.Administrator_Department>(
                     helper.SetLevelParentChildren(departmentUnitOfWork.Administrator_DepartmentRepository.Get(filter: q => q.DepartmentName.Contains(search) && listChild.Contains(q.DepartmentId),
                     orderBy: q => q.OrderBy(item => item.DepartmentIndex), page: page, pagesize: PageSize), 0),
                     page, PageSize, departmentUnitOfWork.Administrator_DepartmentRepository.TotalRows(filter: q => q.DepartmentName.Contains(search)));

                return View(model);
            }
        }
        #endregion

        #region Thêm mới đơn vị
        [AuthorizeRolesLibrary]
        public ActionResult AddDepartment()
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));
            int departmentLevel = 0;

            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                departmentLevel = listDepartment.Where(item => item.DepartmentId == departmentId).FirstOrDefault().DepartmentLevel;
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;
            @ViewBag.DepartmentId = departmentId;
            @ViewBag.DepartmentLevel = departmentLevel + 1;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartment([Bind(Include = "DepartmentName,Address,PhoneNumber,Email,ParentId,DepartmentLevel,DepartmentCode,DepartmentIndex,CreateDate,CreateUser,IsActive")]Administrator.Department.Models.Administrator_Department model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreateDate = DateTime.Now;
                    model.CreateUser = 1;
                    model.IsActive = true;
                    departmentUnitOfWork.Administrator_DepartmentRepository.Insert(model);
                    departmentUnitOfWork.Save();
                    TempData["MessageStatus"] = true;
                    TempData["Message"] = "Thêm mới đơn vị thành công";
                    return RedirectToAction("AddDepartment");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Thêm mới đơn vị không thành công.");
            }
            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive ==true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));
            int departmentLevel = 0;

            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                departmentLevel = listDepartment.Where(item => item.DepartmentId == departmentId).FirstOrDefault().DepartmentLevel;
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;
            @ViewBag.DepartmentId = departmentId;
            @ViewBag.DepartmentLevel = departmentLevel + 1;

            return View(model);
        }
        #endregion

        #region Sửa đơn vị
        [AuthorizeRolesLibrary]
        public ActionResult EditDepartment(int editing)
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));

            int departmentLevel = listDepartment.Where(item => item.DepartmentId == editing).FirstOrDefault().DepartmentLevel;

            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;
            @ViewBag.DepartmentId = departmentId;
            @ViewBag.DepartmentLevel = departmentLevel;
            departmentUnitOfWork.Dispose();

            departmentUnitOfWork = new DepartmentUnitOfWork();
            var model = departmentUnitOfWork.Administrator_DepartmentRepository.GetByID(editing);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment(Administrator.Department.Models.Administrator_Department model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new DepartmentContext())
                    {
                        var target = db.Administrator_Department.Where(item => item.DepartmentId == model.DepartmentId).FirstOrDefault();
                        target.DepartmentName = model.DepartmentName;
                        target.Address = model.Address;
                        target.ParentId = model.ParentId;
                        target.PhoneNumber = model.PhoneNumber;
                        target.Email = model.Email;
                        target.DepartmentCode = model.DepartmentCode;
                        target.DepartmentLevel = model.DepartmentLevel;
                        db.SaveChanges();
                        TempData["MessageStatus"] = true;
                        TempData["Message"] = "Cập nhật đơn vị thành công";
                        return RedirectToAction("DepartmentManager", "Department");

                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Cập nhật đơn vị không thành công.");

                DepartmentHelper helper = new DepartmentHelper();

                //Đơn vị hiện tại
                int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

                IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
                listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));

                int departmentLevel = listDepartment.Where(item => item.DepartmentId == model.DepartmentId).FirstOrDefault().DepartmentLevel;

                //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
                if (departmentId != 0)
                {
                    List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                    listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                    listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
                }
                listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
                @ViewBag.ListDepartment = listDepartment;
                @ViewBag.DepartmentId = departmentId;
                @ViewBag.DepartmentLevel = departmentLevel;
            }
            return View(model);
        }

        #endregion

        #region Xóa đơn vị

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDepartment(int deleting)
        {
            try
            {
                using (var db = new DepartmentContext())
                {
                    var target = db.Administrator_Department.Where(item => item.DepartmentId == deleting).FirstOrDefault();
                    target.IsActive = false;
                    db.SaveChanges();
                }
                TempData["MessageStatus"] = true;
                TempData["Message"] = "Xóa đơn vị thành công";
                return RedirectToAction("DepartmentManager");
            }
            catch
            {
                TempData["MessageStatus"] = false;
                TempData["Message"] = "Xóa đơn vị không thành công";
                return RedirectToAction("DepartmentManager");
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            departmentUnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
