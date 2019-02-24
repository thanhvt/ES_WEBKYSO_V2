using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Administrator.Library;
using Administrator.Library.Filters;
using Administrator.Library.Models;
using PagedList;
using WebMatrix.WebData;
using Administrator.Department.Models;
using Administrator.Department.Helpers;

namespace Administrator.Controllers
{
    [InitializeAdministrator]
    [Authorize]
    public class CustomController : Controller
    {
        private int PageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        private DepartmentUnitOfWork departmentUnitOfWork = new DepartmentUnitOfWork();

        #region Quản lý user

        [AuthorizeRolesLibrary]
        public ActionResult UserManager([DefaultValue(1)] int page, [DefaultValue("")] string search)
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            StaticPagedList<Administrator.Department.Models.UserModel> model;

            //đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            using (var db = new DepartmentContext())
            {
                //Nếu đơn vị cấp tổng thì lấy all
                if (departmentId == 0)
                {
                    model = new StaticPagedList<Administrator.Department.Models.UserModel>(db.UserProfiles.Where(item => item.UserName.Contains(search) && item.UserName != "Administrator").Select(item => new Administrator.Department.Models.UserModel
                        {
                            UserId = item.UserId,
                            UserName = item.UserName,
                            FullName = item.FullName,
                            Status = db.webpages_Memberships.Where(stt => stt.UserId == item.UserId).Select(stt => stt.IsConfirmed).FirstOrDefault(),
                            DepartmentName = db.Administrator_Department.Where(d => d.DepartmentId == item.DepartmentId).Select(d => d.DepartmentName).FirstOrDefault()
                        }).OrderBy(item => item.UserName).Skip((page - 1) * PageSize).Take(PageSize).ToList(), page, PageSize,
                        db.UserProfiles.Where(item => item.UserName.Contains(search) && item.UserName != "Administrator").Count());
                }
                //Nếu đơn vị cấp thành viên chỉ lấy các user của cấp con đơn vị đó
                else
                {
                    var listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll();
                    DepartmentHelper helper = new DepartmentHelper();
                    List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                    listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());

                    model = new StaticPagedList<Administrator.Department.Models.UserModel>(db.UserProfiles.Where(item => item.UserName.Contains(search) && listChild.Contains(item.DepartmentId) && item.UserName != "Administrator").Select(item => new Administrator.Department.Models.UserModel
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        FullName = item.FullName,
                        Status = db.webpages_Memberships.Where(stt => stt.UserId == item.UserId).Select(stt => stt.IsConfirmed).FirstOrDefault(),
                        DepartmentName = db.Administrator_Department.Where(d => d.DepartmentId == item.DepartmentId).Select(d => d.DepartmentName).FirstOrDefault()
                    }).OrderBy(item => item.UserName).Skip((page - 1) * PageSize).Take(PageSize).ToList(), page, PageSize,
                        db.UserProfiles.Where(item => item.UserName.Contains(search) && listChild.Contains(item.DepartmentId) && item.UserName != "Administrator").Count());
                }
            }
            return View(model);
        }        

        [AuthorizeRolesLibrary]
        public ActionResult AddUser()
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));

            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(RegisterModel model, int drDepartmentId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WebSecurity.CreateUserAndAccount(model.UserName.ToLower(), model.Password, new { Email = model.Email, FullName = model.FullName, GenderId = model.GenderId, DepartmentID = drDepartmentId }, false);
                    TempData["MessageStatus"] = true;
                    TempData["Message"] = "Thêm mới người dùng thành công";
                    return RedirectToAction("AddUser");
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }

            DepartmentHelper helper = new DepartmentHelper();

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));

            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRolesLibrary]
        public ActionResult DeleteUser(int deleting)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var target = db.webpages_Memberships.Where(item => item.UserId == deleting).FirstOrDefault();
                    target.IsConfirmed = false;
                    db.SaveChanges();
                }
                TempData["MessageStatus"] = true;
                TempData["Message"] = "Xóa người dùng thành công";
                return RedirectToAction("UserManager");
            }
            catch
            {
                TempData["MessageStatus"] = false;
                TempData["Message"] = "Xóa người dùng không thành công";
                return RedirectToAction("UserManager");
            }
        }

        [AuthorizeRolesLibrary]
        public ActionResult Authorize(int editing)
        {
            using (var db = new UsersContext())
            {
                @ViewBag.editing = editing;
                var listRole = db.webpages_Roles.Select(item => new webpages_RolesModel
                {
                    RoleId = item.RoleId,
                    RoleName = item.RoleName,
                    Description = item.Description,
                    IsChecked = false
                }).ToList();

                var listUserInRole = db.webpages_UsersInRoles.Where(item => item.UserId == editing).Select(item => item.RoleId).ToList();

                foreach (var item in listRole)
                {
                    if (listUserInRole.Contains(item.RoleId))
                    {
                        item.IsChecked = true;
                    }
                }

                return View(listRole);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorize(List<webpages_RolesModel> model, int userId)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var target = db.webpages_UsersInRoles.Where(item => item.UserId == userId).ToList();
                    if (target.Count > 0)
                    {
                        foreach (var item in target)
                            db.webpages_UsersInRoles.Remove(item);
                    }

                    if (model != null && model.Count > 0)
                        foreach (var item in model)
                        {
                            if (item.IsChecked)
                            {
                                var targetUsersInRole = new webpages_UsersInRoles();
                                targetUsersInRole.UserId = userId;
                                targetUsersInRole.RoleId = item.RoleId;
                                db.webpages_UsersInRoles.Add(targetUsersInRole);
                            }
                        }
                    db.SaveChanges();
                }
                TempData["MessageStatus"] = true;
                TempData["Message"] = "Phân quyền người dùng thành công";
                return RedirectToAction("UserManager");
            }
            catch
            {
                ModelState.AddModelError("", "Phân quyền người dùng không thành công.");
            }
            return View();
        }

        [AuthorizeRolesLibrary]
        public ActionResult ResetPass(string editing)
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            ResetPassModel model = new ResetPassModel
            {
                UserName = editing,
                NewPassword = "",
                ConfirmPassword = ""
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPass(ResetPassModel model)
        {
            try
            {
                var verificationToken = WebSecurity.GeneratePasswordResetToken(model.UserName, 1440);
                WebSecurity.ResetPassword(verificationToken, model.NewPassword);
                TempData["MessageStatus"] = true;
                TempData["Message"] = "Reset mật khẩu thành công";
                return RedirectToAction("UserManager");
            }
            catch
            {
                ModelState.AddModelError("", "Reset mật khẩu không thành công.");
            }
            return View();
        }

        #endregion

        #region Chuyển đơn vị
        [AuthorizeRolesLibrary]
        public ActionResult ChangeLevel(int userId)
        {
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            DepartmentHelper helper = new DepartmentHelper();

            var model = departmentUnitOfWork.UserProfileRepository.GetByID(userId);

            //Đơn vị hiện tại
            int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

            IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
            listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));
            
            //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
            if (departmentId != 0)
            {
                List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
            }
            listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
            @ViewBag.ListDepartment = listDepartment;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeLevel(Administrator.Department.Models.UserProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new DepartmentContext())
                    {
                        var target = db.UserProfiles.Where(item => item.UserId == model.UserId).FirstOrDefault();
                        target.DepartmentId = model.DepartmentId;
                        db.SaveChanges();
                    }

                    TempData["MessageStatus"] = true;
                    TempData["Message"] = "Chuyển đơn vị thành công";
                    return RedirectToAction("UserManager", "Custom");
                }
            }
            catch
            {
                DepartmentHelper helper = new DepartmentHelper();

                ModelState.AddModelError("", "Chuyển đơn vị không thành công");
                //Đơn vị hiện tại
                int departmentId = departmentUnitOfWork.UserProfileRepository.Get(filter: q => q.UserName == User.Identity.Name).FirstOrDefault().DepartmentId;

                IEnumerable<Administrator.Department.Models.Administrator_Department> listDepartment = new List<Administrator.Department.Models.Administrator_Department>();
                listDepartment = departmentUnitOfWork.Administrator_DepartmentRepository.GetAll(filter: q => q.IsActive == true, orderBy: q => q.OrderBy(item => item.DepartmentIndex));

                //Nếu là đơn vị cấp thành viên thì chỉ lấy các đơn vị cấp dưới
                if (departmentId != 0)
                {
                    List<int> listChild = helper.GetAllChild(listDepartment.ToList(), departmentId).Select(item => item.DepartmentId).ToList();
                    listChild.Add(listDepartment.Where(item => item.DepartmentId == departmentId).Select(item => item.DepartmentId).FirstOrDefault());
                    listDepartment = listDepartment.Where(item => listChild.Contains(item.DepartmentId));
                }
                listDepartment = helper.SetLevelParentChildren(listDepartment, 0);
                @ViewBag.ListDepartment = listDepartment;
            }

            return View(model);
        }
        #endregion        

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        protected override void Dispose(bool disposing)
        {
            departmentUnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
