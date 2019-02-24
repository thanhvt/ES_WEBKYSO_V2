using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.ModelParameter
{
    public class BangKeModel
    {
        // ký loại bảng kê
        public int? ID { get; set; }
        public string MaBangKe { get; set; }
        public string GhiChu { get; set; }
        public string TenBangKe { get; set; }
        public string MaDonVi { get; set; }
    }
}