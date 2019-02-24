using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("D_IMEI")]
    public partial class D_IMEI : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Imei")]
        [Display(Name = "IMEI")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string IMEI { get; set; }

        [Column("Ma_DViQLy")]
        [MaxLength(6, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DVIQLY { get; set; }

        [Column("Loai_May")]
        [MaxLength(100, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Loại máy")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string LOAI_MAY { get; set; }

        [Column("Ngay_Cap", TypeName = "datetime")]
        public DateTime? NGAY_CAP { get; set; }

        [Column("Nguoi_Cap")]
        [MaxLength(100, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Người cấp")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string NGUOI_CAP { get; set; }

        [Column("Nguoi_Dung")]
        [MaxLength(100, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Người dùng")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string NGUOI_DUNG { get; set; }

        #region Not map zone

        [NotMapped]
        [Display(Name = "Ngày cấp")]
        [UIHint("TimeInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string NgayCapString { get; set; }

        #endregion
    }
}