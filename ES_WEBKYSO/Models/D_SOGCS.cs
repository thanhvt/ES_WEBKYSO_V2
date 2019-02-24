using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("D_SOGCS")]
    public partial class D_SOGCS : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_SOGCS", Order = 1)]
        public int ID_SOGCS { get; set; }

        [Key]
        [Column("MA_DVIQLY", Order = 2)]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Mã đơn vị phải ít nhất ba ký tự")]
        [StringLength(6, MinimumLength = 3, ErrorMessage = "Mã đơn vị không được quá 6 ký tự")]
        public string MA_DVIQLY { get; set; }

        [Key]
        [Column("MA_SOGCS", Order = 3)]
        [MaxLength(50, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Mã sổ GCS")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_SOGCS { get; set; }

        [Column("TEN_SOGCS")]
        [MaxLength(100, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Tên sổ GCS")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string TEN_SOGCS { get; set; }

        [Column("SO_KY")]
        [Display(Name = "Số kỳ")]
        [UIHint("TextInput")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số kỳ phải là số nguyên dương")]
        public int? SO_KY { get; set; }

        [Column("TINH_TRANG")]
        [Display(Name = "Tình trạng")]
        [UIHint("TextInput")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Tình trạng phải là số")]
        public int? TINH_TRANG { get; set; }

        [Column("LOAI_SOGCS")]
        [Display(Name = "Loại sổ")]
        [UIHint("TextInput")]
        public string LOAI_SOGCS { get; set; }

        [Column("NGAY_GHI")]
        [Display(Name = "Ngày ghi")]
        [UIHint("TextInput")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ngày ghi phải là số nguyên dương")]
        public int NGAY_GHI { get; set; }

        [Column("HINH_THUC")]
        [Display(Name = "Hình thức")]
        [UIHint("TextInput")]
        public string HINH_THUC { get; set; }

        [Column("TRANG_THAI")]
        [Display(Name = "Trạng thái")]
        [UIHint("TextInput")]
        public string TRANG_THAI { get; set; }
    }
}