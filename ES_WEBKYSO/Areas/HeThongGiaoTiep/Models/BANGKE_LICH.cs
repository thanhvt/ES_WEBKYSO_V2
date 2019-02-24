using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Models
{
    public class BANGKE_LICH
    {
        public int? ID_LICHGCS { get; set; }
        public string MA_DVIQLY { get; set; }
        public string MA_SOGCS { get; set; }
        public string TEN_SOGCS { get; set; }
        public string HINH_THUC { get; set; }
        public int? NGAY_GHI { get; set; }
        public int? KY { get; set; }
        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public string MA_DOIGCS { get; set; }
        public int? USERID { get; set; }
        public string FullName { get; set; }
        public string STATUS_NVK { get; set; }
        public string STATUS_DVCM { get; set; }
        public int? MA_BANGKELICH { get; set; }
        public string MA_LOAIBANGKE { get; set; }
        public bool TrangThaiKy { get; set; }
        public int ThuTuKy { get; set; }
        public string File { get; set; }
    }
}