using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.CauHinh.Models
{
    public class CAUHINH_NVGCS
    {
        public int MA_SOGCS_NVIEN { get; set; }
        public string MA_DVIQLY { get; set; }
        public string MA_SOGCS { get; set; }
        public string TEN_SOGCS { get; set; }
        public string MA_DOIGCS { get; set; }
        public int? MA_NVIEN_GCS { get; set; }
        public string TEN_NVIEN_GCS { get; set; }
    }
}