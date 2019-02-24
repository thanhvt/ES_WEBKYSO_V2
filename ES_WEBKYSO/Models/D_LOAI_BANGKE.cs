using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("D_LOAI_BANGKE")]
    public class D_LOAI_BANGKE : IEntity
    {
        [Key]
        [Column("Ma_LoaiBangKe")]
        [Display(Name = "Mã loại bảng kê")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string MA_LOAIBANGKE { get; set; }

        [Column("Ten_LoaiBangKe")]
        [Display(Name = "Tên loại bảng kê")]
        [UIHint("DropDownListInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string TEN_LOAIBANGKE { get; set; }

        [Column("Mo_Ta")]
        [Display(Name = "Mô tả")]
        public string MO_TA { get; set; }

        public virtual ICollection<GCS_BANGKE_LICH> ListGCS_BANGKE_LICH { get; set; }
        public virtual ICollection<CFG_BANGKE_DONVI> ListCFG_BANGKE_DONVI { get; set; }
    }
}