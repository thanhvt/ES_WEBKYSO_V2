using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("GCS_LICHGCS")]
    public partial class GCS_LICHGCS : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_LICHGCS")]
        public int ID_LICHGCS { get; set; }

        [Key]
        [Column("MA_DVIQLY", Order = 1)]
        public string MA_DVIQLY { get; set; }

        [Key]
        [Column("MA_SOGCS", Order = 2)]
        public string MA_SOGCS { get; set; }

        [Key]
        [Column("KY", Order = 3)]
        public int? KY { get; set; }

        [Key]
        [Column("THANG", Order = 4)]
        public int? THANG { get; set; }

        [Key]
        [Column("NAM", Order = 5)]
        public int? NAM { get; set; }

        [Column("TEN_SOGCS")]
        public string TEN_SOGCS { get; set; }

        [Column("HINH_THUC")]
        public string HINH_THUC { get; set; }

        [Column("NGAY_GHI")]
        public int NGAY_GHI { get; set; }

        [Column("GIO")]
        public int? GIO { get; set; }

        [Column("MA_DOIGCS")]
        [Display(Name = "Tổ đội")]
        [UIHint("DropDownListInput")]
        public string MA_DOIGCS { get; set; }

        [Column("USERID")]
        [Display(Name = "Nhân viên")]
        [UIHint("DropDownListInput")]
        public int? USERID { get; set; }

        [Column("STATUS")]
        public string STATUS { get; set; }

        [Column("STATUS_PC")]
        public string STATUS_PC { get; set; }

        [Column("STATUS_CNCS")]
        public string STATUS_CNCS { get; set; }

        [Column("STATUS_NVK")]
        public string STATUS_NVK { get; set; }

        [Column("STATUS_DTK")]
        public string STATUS_DTK { get; set; }

        [Column("STATUS_DHK")]
        public string STATUS_DHK { get; set; }

        [Column("STATUS_DVCM")]
        public string STATUS_DVCM { get; set; }

        [Column("NHANSO_MTB")]
        public string NHANSO_MTB { get; set; }

        [Column("TRALAI_COUNT")]
        public int? TRALAI_COUNT { get; set; }

        [Column("NGAY_CKY")]
        public DateTime? NGAY_CKY { get; set; }

        [Column("NGAY_TAO")]
        public DateTime? NGAY_TAO { get; set; }

        [Column("NGUOI_TAO")]
        public string NGUOI_TAO { get; set; }

        [Column("NGAY_SUA")]
        public DateTime? NGAY_SUA { get; set; }

        [Column("NGUOI_SUA")]
        public string NGUOI_SUA { get; set; }

        [Column("FILE_XML")]
        public string FILE_XML { get; set; }

        #region Not map zone

        [NotMapped]
        //[ImportInclude(Order = 1, ForDataTable = 1)]
        [Display(Name = "Tổ đội")]
        public string Ten_Doi { get; set; }

        #endregion

        [ForeignKey("MA_DOIGCS")]
        public virtual D_DOIGCS HasD_DOIGCS { get; set; }

        [NotMapped]
        [Display(Name = "Tên nhân viên")]
        public string FullName { get; set; }

        [ForeignKey("USERID")]
        public virtual UserProfile HasUserProfile { get; set; }

        //public virtual ICollection<GCS_BANGKE_LICH> ListGCS_BANGKE_LICH { get; set; }
    }
}