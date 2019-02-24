using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.ModelParameter
{
    public class FindModelGcs
    {
        public int? Thang { get; set; }
        public int? Ky { get; set; }
        public int? Nam { get; set; }
        public string TrangThai { get; set; }
        public string MaSo { get; set; }
        public int? NgayGhi { get; set; }
        public string HinhThuc { get; set; }
        public string ThaoTac { get; set; }
        public string MaDonVi { get; set; }
        public string TenSo { get; set; }
        public string LoaiSo { get; set; }
        public  bool? TrangThaiKy { get; set; }

        // Tìm kiếm cho User - Imei
        public string NguoiDung { get; set; }
        public string LoaiMay { get; set; }
        public string NguoiCap { get; set; }
        public DateTime? NgayCapString { get; set; }

        // Tìm kiếm cho danh mục Đội
        public string MaDoi { get; set; }
        public string TenDoi { get; set; }


        public string LoaiThamSo { get; set; }
        public string GiaTri { get; set; }

        public int Gio { get; set; }

        //tìm kiếm cho phân công sổ
        public int? USERID { get; set; }
        public string MaLoaiBangKe { get; set; }

        // ký loại bảng kê
        public int? ID { get; set; }
        public string MaBangKe { get; set; }
        public string GhiChu { get; set; }
        public string TenBangKe { get; set; }
    }
}