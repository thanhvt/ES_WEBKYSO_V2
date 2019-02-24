using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("GCS_CHITIET_KY")]
    public class GCS_CHITIET_KY : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Ma_BangKeLich")]
        [Display(Name = "Mã bảng kê lịch")]
        [UIHint("DropDownListInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int MA_BANGKELICH { get; set; }
        [Column("UserId")]
        [Display(Name = "Mã người dùng")]
        [UIHint("DropDownListInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int UserId { get; set; }

        [Column("Nguoi_Ky")]
        [Display(Name = "Người ký")]
        public string NGUOI_KY { get; set; }

        [Column("NGAY_KY", TypeName = "datetime")]
        [Display(Name = "Người ký")]
        public DateTime NGAY_KY { get; set; }

        [Column("GHI_CHU")]
        [Display(Name = "Ghi chú")]
        public string GHI_CHU { get; set; }

        [NotMapped]
        [Display(Name = "Ngày ký")]
        [UIHint("TimeInput")]
        public string NGAY_KYString { get; set; }


    }
}