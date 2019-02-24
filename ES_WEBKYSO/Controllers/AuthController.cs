using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Administrator.Department.Models;
using ES_AuthSDK;
using WebMatrix.WebData;

namespace ES_WEBKYSO.Controllers
{
    public class AuthController : BaseController
    {
        //
        // GET: /Auth/

        public ActionResult Index(string token)
        {
            var userInfo = EsAuth.Query(token, "/userinfo");
            var userName = userInfo.ListData?.FirstOrDefault(x => x.Key == "UserName")?.Value;
            
            if (userInfo.Status == true && userName != null)
            {
                var existAccount = Uow.RepoBase<Models.UserProfile>().GetOne(x => x.AuthAccountId == userName);
                if (existAccount != null)
                {
                    FormsAuthentication.SetAuthCookie(existAccount.UserName, true);
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorAuthLogin"] = "AuthId '" + userName + "' chưa liên kết với tài khoản nào.";
            }
            else
            {
                TempData["ErrorAuthLogin"] = "Không truy vấn được server Authen.";
            }
            return RedirectToAction("Login", "Account", new { area = "Administrator" });
        }

    }
}
