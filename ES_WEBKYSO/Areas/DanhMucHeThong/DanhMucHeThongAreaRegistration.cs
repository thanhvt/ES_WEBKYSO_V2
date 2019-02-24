using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.DanhMucHeThong
{
    public class DanhMucHeThongAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DanhMucHeThong";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DanhMucHeThong_default",
                "DanhMucHeThong/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
