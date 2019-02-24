using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("GCS_BANGKE_LICH")]
    public class GCS_BANGKE_LICH : IEntity
    {
        [Key]
        [Column("MA_BANGKELICH")]
        public int MA_BANGKELICH { get; set; }
        
        [Column("ID_LICHGCS")]
        public int ID_LICHGCS { get; set; }

        [Column("MA_LOAIBANGKE")]
        [Display(Name = "Loại bảng kê")]
        [UIHint("DropDownListInput")]
        public string MA_LOAIBANGKE { get; set; }

        [NotMapped]
        [Display(Name = "Tên loại bảng kê")]
        public string Ten_LoaiBangKe { get; set; }

        [ForeignKey("MA_LOAIBANGKE")]
        public virtual D_LOAI_BANGKE HasD_LOAI_BANGKE { get; set; }
        
        //public virtual ICollection<FL_FILE>  LstFL_FILE { get; set; }

        //[NotMapped]
        //[ForeignKey("ID_LICHGCS")]
        //public virtual GCS_LICHGCS HasGCS_LICHGCS { get; set; }
    }
}