using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.CauHinh
{
    public class CauHinhAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CauHinh";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CauHinh_default",
                "CauHinh/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
