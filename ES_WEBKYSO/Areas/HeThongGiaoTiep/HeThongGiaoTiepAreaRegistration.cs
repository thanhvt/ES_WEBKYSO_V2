using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep
{
    public class HeThongGiaoTiepAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HeThongGiaoTiep";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HeThongGiaoTiep_default",
                "HeThongGiaoTiep/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
