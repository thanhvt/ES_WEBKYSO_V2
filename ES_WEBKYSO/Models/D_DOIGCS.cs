using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;


namespace ES_WEBKYSO.Models
{
    [Table("D_DOIGCS")]
    public class D_DOIGCS : IEntity
    {
        [Key]
        [Column("Ma_DoiGcs")]
        [Display(Name = "Mã đội")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Mã đơn vị phải ít nhất ba ký tự")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mã đơn vị không được quá 50 ký tự")]
        public string MA_DOIGCS { get; set; }

        [Column("Ten_Doi")]
        [Display(Name = "Tên đội")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Mã đơn vị phải ít nhất ba ký tự")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Mã đơn vị không được quá 100 ký tự")]
        public string TEN_DOI { get; set; }

        [Column("Ma_DViQLy")]
        [MaxLength(6, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DVIQLY { get; set; }

        [Column("Ghi_Chu")]
        [Display(Name = "Ghi chú")]
        [UIHint("TextInput")]
        public string GHI_CHU { get; set; }

        public virtual ICollection<GCS_LICHGCS> ListGCS_LICHGCS { get; set; }
        public virtual ICollection<CFG_DOIGCS_NVIEN> ListCFG_DOIGCS_NVIEN { get; set; }
    }
}