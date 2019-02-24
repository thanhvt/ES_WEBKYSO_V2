using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("FL_FILE")]
    public class FL_FILE : IEntity
    {
        [Key]
        [Column("FileID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }

        [Column("MA_BANGKELICH")]
        public int MA_BANGKELICH { get; set; }
        
        [Column("FilePath")]
        public string FilePath { get; set; }

        [Column("FileHash")]
        public byte[] FileHash { get; set; }

        //[NotMapped]
        //[ForeignKey("MA_BANGKELICH")]
        //public virtual GCS_BANGKE_LICH HasGCS_BANGKE_LICH { get; set; }

    }
}