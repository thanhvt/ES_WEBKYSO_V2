using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.CauHinh.Models
{
    public class DM_DOI
    {
        public int ID { get; set; }
        public string MA_DVIQLY { get; set; }
        public string MA_DOIGCS { get; set; }
        public string TEN_DOIGCS { get; set; }
        public int? MA_NVIEN_GCS { get; set; }
        public string TEN_NVIEN_GCS { get; set; }
    }
}