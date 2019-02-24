using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("GCS_CHISO_HHU")]
    public class GCS_CHISO_HHU : IEntity
    {
        [Key]
        [Column("ID")]
        [Display(Name = "ID")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int ID { get; set; }

        [Column("MA_NVGCS")]
        [Display(Name = "Mã nhân viên GCS")]
        [UIHint("TextInput")]
        public string MA_NVGCS { get; set; }

        [Column("MA_KHANG")]
        [Display(Name = "Mã khách hàng")]
        [UIHint("TextInput")]
        public string MA_KHANG { get; set; }

        [Column("MA_DDO")]
        [Display(Name = "MA_DDO")]
        [UIHint("TextInput")]
        public string MA_DDO { get; set; }
        
        [Column("MA_DVIQLY")]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DVIQLY { get; set; }

        [Column("MA_GC")]
        [Display(Name = "Mã GC")]
        [UIHint("TextInput")]
        public string MA_GC { get; set; }

        [Column("MA_QUYEN")]
        [Display(Name = "Mã sổ")]
        [UIHint("TextInput")]
        public string MA_QUYEN { get; set; }

        [Column("MA_TRAM")]
        [Display(Name = "Mã trạm")]
        [UIHint("TextInput")]
        public string MA_TRAM { get; set; }
        
        [Column("BOCSO_ID")]
        [Display(Name = "Mã bộ GCS")]
        [UIHint("TextInput")]
        public long? BOCSO_ID { get; set; }

        [Column("LOAI_BCS")]
        [Display(Name = "Loại Bộ chỉ số")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string LOAI_BCS { get; set; }

        [Column("LOAI_CS")]
        [Display(Name = "Loại chỉ số")]
        [UIHint("TextInput")]
        public string LOAI_CS { get; set; }

        [Column("TEN_KHANG")]
        [Display(Name = "Tên khách hàng")]
        [UIHint("TextInput")]
        public string TEN_KHANG { get; set; }

        [Column("DIA_CHI")]
        [Display(Name = "Địa chỉ")]
        [UIHint("TextInput")]
        public string DIA_CHI { get; set; }

        [Column("MA_NN")]
        [Display(Name = "Mã NN")]
        [UIHint("TextInput")]
        public string MA_NN { get; set; }

        [Column("SO_HO")]
        [Display(Name = "Số hộ")]
        [UIHint("TextInput")]
        public Decimal? SO_HO { get; set; }

        [Column("MA_CTO")]
        [Display(Name = "Mã công tơ")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_CTO { get; set; }

        [Column("SERY_CTO")]
        [Display(Name = "Sery công tơ")]
        [UIHint("TextInput")]
        public string SERY_CTO { get; set; }

        [Column("HSN")]
        [Display(Name = "Hệ số nhân")]
        [UIHint("TextInput")]
        public Decimal? HSN { get; set; }

        [Column("CS_CU")]
        [Display(Name = "Chỉ số cũ")]
        [UIHint("TextInput")]
        public Decimal? CS_CU { get; set; }

        [Column("TTR_CU")]
        [Display(Name = "TTR_CU")]
        [UIHint("TextInput")]
        public string TTR_CU { get; set; }

        [Column("SL_CU")]
        [Display(Name = "SL_CU")]
        [UIHint("TextInput")]
        public Decimal? SL_CU { get; set; }

        [Column("SL_TTIEP")]
        [Display(Name = "SL_TTIEP")]
        [UIHint("TextInput")]
        public int? SL_TTIEP { get; set; }

        [Column("NGAY_CU")]
        [Display(Name = "NGAY_CU")]
        [UIHint("TextInput")]
        public DateTime? NGAY_CU { get; set; }

        [Column("CS_MOI")]
        [Display(Name = "CS_MOI")]
        [UIHint("TextInput")]
        public Decimal? CS_MOI { get; set; }

        [Column("TTR_MOI")]
        [Display(Name = "TTR_MOI")]
        [UIHint("TextInput")]
        public string TTR_MOI { get; set; }

        [Column("SL_MOI")]
        [Display(Name = "SL_MOI")]
        [UIHint("TextInput")]
        public Decimal? SL_MOI { get; set; }

        [Column("CHUOI_GIA")]
        [Display(Name = "CHUOI_GIA")]
        [UIHint("TextInput")]
        public string CHUOI_GIA { get; set; }

        [Column("KY")]
        [Display(Name = "KY")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int KY { get; set; }

        [Column("THANG")]
        [Display(Name = "THANG")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int THANG { get; set; }

        [Column("NAM")]
        [Display(Name = "NAM")]
        [UIHint("TextInput")]
        public int NAM { get; set; }

        [Column("NGAY_MOI")]
        [Display(Name = "NGAY_MOI")]
        [UIHint("TextInput")]
        public DateTime? NGAY_MOI { get; set; }

        [Column("NGUOI_GCS")]
        [Display(Name = "NGUOI_GCS")]
        [UIHint("TextInput")]
        public string NGUOI_GCS { get; set; }

        [Column("SL_THAO")]
        [Display(Name = "SL_THAO")]
        [UIHint("TextInput")]
        public Decimal? SL_THAO { get; set; }

        [Column("KIMUA_CSPK")]
        [Display(Name = "KIMUA_CSPK")]
        [UIHint("TextInput")]
        public Int16? KIMUA_CSPK { get; set; }

        [Column("MA_COT")]
        [Display(Name = "MA_COT")]
        [UIHint("TextInput")]
        public string MA_COT { get; set; }

        [Column("CGPVTHD")]
        [Display(Name = "CGPVTHD")]
        [UIHint("TextInput")]
        public string CGPVTHD { get; set; }

        [Column("HTHUC_TBAO_DK")]
        [Display(Name = "HTHUC_TBAO_DK")]
        [UIHint("TextInput")]
        public string HTHUC_TBAO_DK { get; set; }

        [Column("DTHOAI_SMS")]
        [Display(Name = "DTHOAI_SMS")]
        [UIHint("TextInput")]
        public string DTHOAI_SMS { get; set; }

        [Column("EMAIL")]
        [Display(Name = "EMAIL")]
        [UIHint("TextInput")]
        public string EMAIL { get; set; }

        [Column("THOI_GIAN")]
        [Display(Name = "THOI_GIAN")]
        [UIHint("TextInput")]
        public string THOI_GIAN { get; set; }

        [Column("X")]
        [Display(Name = "X")]
        [UIHint("TextInput")]
        public Decimal? X { get; set; }

        [Column("Y")]
        [Display(Name = "Y")]
        [UIHint("TextInput")]
        public Decimal? Y { get; set; }

        [Column("SO_TIEN")]
        [Display(Name = "SO_TIEN")]
        [UIHint("TextInput")]
        public Decimal? SO_TIEN { get; set; }

        [Column("HTHUC_TBAO_TH")]
        [Display(Name = "HTHUC_TBAO_TH")]
        [UIHint("TextInput")]
        public string HTHUC_TBAO_TH { get; set; }

        [Column("TENKHANG_RUTGON")]
        [Display(Name = "TENKHANG_RUTGON")]
        [UIHint("TextInput")]
        public string TENKHANG_RUTGON { get; set; }

        [Column("TTHAI_DBO")]
        [Display(Name = "TTHAI_DBO")]
        [UIHint("TextInput")]
        public byte? TTHAI_DBO { get; set; }

        [Column("DU_PHONG")]
        [Display(Name = "DU_PHONG")]
        [UIHint("TextInput")]
        public string DU_PHONG { get; set; }

        [Column("TEN_FILE")]
        [Display(Name = "TEN_FILE")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string TEN_FILE { get; set; }

        [Column("GHICHU")]
        [Display(Name = "GHICHU")]
        [UIHint("TextInput")]
        public string GHICHU { get; set; }

        [Column("SLUONG_1")]
        [Display(Name = "SLUONG_1")]
        [UIHint("TextInput")]
        public Decimal? SLUONG_1 { get; set; }

        [Column("SLUONG_2")]
        [Display(Name = "SLUONG_2")]
        [UIHint("TextInput")]
        public Decimal? SLUONG_2 { get; set; }

        [Column("SLUONG_3")]
        [Display(Name = "SLUONG_3")]
        [UIHint("TextInput")]
        public Decimal? SLUONG_3 { get; set; }

        [Column("SO_HOM")]
        [Display(Name = "SO_HOM")]
        [UIHint("TextInput")]
        public string SO_HOM { get; set; }

        [Column("TT_KHAC")]
        [Display(Name = "TT_KHAC")]
        [UIHint("TextInput")]
        public string TT_KHAC { get; set; }

        [Column("ANH_GCS")]
        [Display(Name = "ANH_GCS")]
        [UIHint("TextInput")]
        public string ANH_GCS { get; set; }

        [Column("PMAX")]
        [Display(Name = "PMAX")]
        [UIHint("TextInput")]
        public Decimal? PMAX { get; set; }

        [Column("NGAY_PMAX")]
        [Display(Name = "NGAY_PMAX")]
        [UIHint("TextInput")]
        public DateTime? NGAY_PMAX { get; set; }

        [Column("STR_CHECK_DSOAT")]
        [Display(Name = "STR_CHECK_DSOAT")]
        [UIHint("TextInput")]
        public string STR_CHECK_DSOAT { get; set; }
    }
}