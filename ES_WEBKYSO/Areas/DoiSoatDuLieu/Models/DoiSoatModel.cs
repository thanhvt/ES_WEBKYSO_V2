using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using esDigitalSignature.iTextSharp.text;

namespace ES_WEBKYSO.Areas.DoiSoatDuLieu.Models
{
    public class DoiSoatModel
    {
        public string MaDonVi { get; set; }
        public string MaSo { get; set; }
        public List<string> MaSos { get; set; }
        public int? Thang { get; set; }
        public int? Ky { get; set; }
        public int? Nam { get; set; }
        public string LocCongTo { get; set; }
    }
}