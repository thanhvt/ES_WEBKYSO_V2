using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.BaoCao
{
    public class BaoCaoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaoCao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaoCao_default",
                "BaoCao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
