using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("CFG_SOGCS_NVIEN")]
    public class CFG_SOGCS_NVIEN : IEntity
    {
        [Key]
        [Column("MA_SOGCS_NVIEN")]
        [Display(Name = "ID")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int MA_SOGCS_NVIEN { get; set; }

        [Column("MA_DOIGCS")]
        [Display(Name = "Đội GCS")]
        [UIHint("DropDownListInput")]
        [StringLength(10)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DOIGCS { get; set; }

        [Column("MA_DVIQLY")]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(6)]
        public string MA_DVIQLY { get; set; }

        [Column("MA_SOGCS")]
        [Display(Name = "Mã sổ")]
        [UIHint("DropDownListInput")]
        [StringLength(9)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_SOGCS { get; set; }

        [Column("USERID")]
        [Display(Name = "Mã nhân viên GCS")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [UIHint("DropDownListInput")]
        public int? USERID { get; set; }

        [NotMapped]
        [Display(Name ="Tên nhân viên")]
        public string FullName { get; set; }

        [ForeignKey("USERID")]
        public virtual UserProfile HasUserProfile { get; set; }
        
    }
}