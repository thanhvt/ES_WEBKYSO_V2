using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.CauHinh.Models
{
    public class Result
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public BOPHANKY BoPhanKy { get; set; }
    }
}