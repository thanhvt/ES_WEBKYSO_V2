using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Models
{
    public class TempMail
    {
        public string MaDoi { get; set; }
        public string TenDoi { get; set; }
        public int? UserID { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
}