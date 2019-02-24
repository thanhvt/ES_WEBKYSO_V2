using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.CauHinh.Models
{
    public class BOPHANKY
    {
        public int MA_BOPHAN_KY { get; set; }
        public string MA_DVIQLY { get; set; }
        public string MA_LOAIBANGKE { get; set; }
        public int RoleId { get; set; }
        public int THU_TUKY { get; set; }
        public string MO_TA { get; set; }
        public string Ten_LoaiBangKe { get; set; }
        public string Ten_BoPhanKy { get; set; }
    }
}