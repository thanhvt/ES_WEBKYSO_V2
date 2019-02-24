using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Models
{
    public class PHANCONG
    {
        public int ID_LICHGCS { get; set; }
        public string MA_DVIQLY { get; set; }
        public string MA_SOGCS { get; set; }
        public string TEN_SOGCS { get; set; }
        public string HINHTHUC { get; set; }
        public int NGAY_GHI { get; set; }
        public int KY { get; set; }
        public int THANG { get; set; }
        public int NAM { get; set; }
        public string MA_DOIGCS { get; set; }
        public int? USERID { get; set; }
        public string TEN_NVIEN_GCS { get; set; }
        public string STATUS { get; set; }
        public string TINHTRANGSO { get; set; }
        public string MABANGKE { get; set; }
        //public string LOAI_SOGCS { get; set; }
    }
}