using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("CFG_DOIGCS_NVIEN")]
    public class CFG_DOIGCS_NVIEN : IEntity
    {
        [Key]
        [Column("ID")]
        [Display(Name = "Mã danh mục")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int ID { get; set; }

        [Column("MA_DVIQLY")]
        [Display(Name = "Mã đơn vị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DVIQLY { get; set; }

        [Column("MA_DOIGCS")]
        [Display(Name = "Mã đội")]
        [UIHint("DropDownListInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_DOIGCS { get; set; }

        [Column("USERID")]
        [Display(Name = "Nhân viên GCS")]
        [UIHint("DropDownListInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int? USERID { get; set; }

        //[ForeignKey("USERID")]
        //public virtual ICollection<UserProfile> ListUserProfile { get; set; }
        [ForeignKey("USERID")]
        public virtual UserProfile HasUserProfile { get; set; }

        [ForeignKey("MA_DOIGCS")]
        public virtual D_DOIGCS HasD_DOIGCS { get; set; }
    }
}