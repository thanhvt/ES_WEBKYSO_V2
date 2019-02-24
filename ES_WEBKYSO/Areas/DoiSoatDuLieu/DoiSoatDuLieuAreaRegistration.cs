using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.DoiSoatDuLieu
{
    public class DoiSoatDuLieuAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DoiSoatDuLieu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DoiSoatDuLieu_default",
                "DoiSoatDuLieu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
