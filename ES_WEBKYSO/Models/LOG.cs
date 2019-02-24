using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("LOG")]
    public partial class LOG : IEntity
    {
        [Key]
        [Column("ID_LOG")]
        public int ID_LOG { get; set; }

        [Column("LOG_CATEGORY_ID")]
        public string LOG_CATEGORY_ID { get; set; }

        [Column("ID_LICHGCS")]
        public int? ID_LICHGCS { get; set; }

        [Column("MA_SOGCS")]
        public string MA_SOGCS { get; set; }
        
        [Column("KY")]
        public int? KY { get; set; }
        
        [Column("THANG")]
        public int? THANG { get; set; }
        
        [Column("NAM")]
        public int? NAM { get; set; }

        [Column("CONTENTS")]
        public string CONTENTS { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("LOG_DATE")]
        public DateTime? LOG_DATE { get; set; }

        [Column("MA_LOAIBANGKE")]
        public string MA_LOAIBANGKE { get; set; }

        [Column("COUNT_THUCHIEN")]
        public int? COUNT_THUCHIEN { get; set; }

        [Column("LOG_STATUS_ID")]
        public string LOG_STATUS_ID { get; set; }
    }
}