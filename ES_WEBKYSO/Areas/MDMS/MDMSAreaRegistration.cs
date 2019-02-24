using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.MDMS
{
    public class MDMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MDMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MDMS_default",
                "MDMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
