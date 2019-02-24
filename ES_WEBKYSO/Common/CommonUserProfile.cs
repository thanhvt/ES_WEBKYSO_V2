using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Common
{
    public class CommonUserProfile
    { //lấy các thông tin của nhân viên đăng nhập hệ thống bao gồm: UserID, MA_DVIQLY, DepartmentName, FullName
        private UserProfileController userProfile = new UserProfileController(HttpContext.Current.User.Identity.Name);
        public int UserID
        {
            get
            {
                return userProfile.UserID;
            }
            set
            {
                UserID = value;
            }
        }
        public string FullName
        {
            get
            {
                return userProfile.FullName;
            }
            set
            {
                FullName = value;
            }
        }

        public string MA_DVIQLY
        {
            get
            {
                return userProfile.MA_DVIQLY;
            }
            set
            {
                MA_DVIQLY = value;
            }
        }
        public string DepartmentName
        {
            get
            {
                return userProfile.DepartmentName;
            }
            set
            {
                DepartmentName = value;
            }
        }

        private class UserProfileController : BaseController
        {//class truy cập database giúp lấy dữ liệu theo UserName truyền vào
            private string userName;
            public UserProfileController(string UserName)
            {
                userName = UserName;
            }
            public int UserID
            {
                get
                {
                    return Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == userName).UserId;
                }
                set
                {
                    UserID = value;
                }
            }
            public string MA_DVIQLY
            {
                get
                {
                    var id = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == userName).DepartmentId;
                    return Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == id).DepartmentCode;
                }
                set
                {
                    MA_DVIQLY = value;
                }
            }
            public string FullName
            {
                get
                {
                    return Uow.RepoBase<UserProfile>().GetOne(o => o.UserName == userName).FullName;
                }
                set
                {
                    FullName = value;
                }
            }
            public string DepartmentName
            {
                get
                {
                    var id = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == userName).DepartmentId;
                    return Uow.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == id).DepartmentName;
                }
                set
                {
                    DepartmentName = value;
                }
            }
        }
    }
}