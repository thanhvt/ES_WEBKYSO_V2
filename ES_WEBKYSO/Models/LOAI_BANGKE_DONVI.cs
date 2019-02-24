using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("LOAI_BANGKE_DONVI")]
    public class LOAI_BANGKE_DONVI : IEntity
    {
        [Key]
        [Column("Id")]
        [UIHint("TextInput")]
        public int ID { get; set; }

        [Column("Ma_LoaiBangKe")]
        [UIHint("DropDownListInput")]
        [Display(Name = "Mã loại bảng kê đơn vị")]
        [Required(ErrorMessage = "{0} Chưa chọn bảng kê")]
        public string MA_LOAIBANGKE { get; set; }

        [Column("Ma_DviQly")]
        [UIHint("TextInput")]
        [Display(Name = "Mã đơn vị quản lý")]
        [Required(ErrorMessage = "{0} Chưa chọn đơn vị quản lý")]
        public string MA_DVIQLY { get; set; }

        [Column("Ghi_Chu")]
        [UIHint("TextInput")]
        [Display(Name = "Ghi chú")]
        public string GHI_CHU { get; set; }

        #region Not map zone
        [NotMapped]
        [Display(Name = "Tên loại bảng kê")]
        public string Ten_LoaiBangKe { get; set; }




        #endregion
        [ForeignKey("MA_LOAIBANGKE")]
        public virtual D_LOAI_BANGKE HasD_LOAI_BANGKE { get; set; }
    }
}