using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("CFG_BOPHAN_KY")]
    public class CFG_BOPHAN_KY : IEntity
    {
        [Key]
        [Column("MA_BOPHAN_KY")]
        public int MA_BOPHAN_KY { get; set; }

        [Column("MA_DVIQLY")]
        [Display(Name = "Mã đơn vị quản lý")]
        [UIHint("TextInput")]
        public string MA_DVIQLY { get; set; }

        [Column("MA_LOAIBANGKE")]
        [UIHint("DropDownListInput")]
        [Display(Name = "Loại bảng kê đơn vị")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_LOAIBANGKE { get; set; }

        [Column("RoleId")]
        [UIHint("DropDownListInput")]
        [Display(Name = "Bộ phận ký")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int RoleId { get; set; }

        [Column("THU_TUKY")]
        [UIHint("DropDownListInput")]
        [Display(Name = "Thứ tự ký")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int THU_TUKY { get; set; }

        [Column("MO_TA")]
        [UIHint("TextInput")]
        public string MO_TA { get; set; }

        #region Mapkey

        [NotMapped]
        [Display(Name = "Loại bảng kê")]
        public string Ten_LoaiBangKe { get; set; }
       
        [ForeignKey("MA_LOAIBANGKE")]
        public virtual D_LOAI_BANGKE HasD_LOAI_BANGKE { get; set; }
        [NotMapped]
        [Display(Name = "Tên bộ phận ký")]
        public string Ten_BoPhanKy { get; set; }

        [ForeignKey("RoleId")]
        public virtual WebpagesRoles HasWebpagesRoles { get; set; }
        #endregion
    }
}