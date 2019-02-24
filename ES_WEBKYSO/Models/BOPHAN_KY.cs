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
    [Table("BOPHAN_KY")]
    public class BOPHAN_KY : IEntity
    {
        [Key]
        [Column("MA_BOPHAN_KY")]
        public int MA_BOPHAN_KY { get; set; }

        [Column("MA_DVIQLY")]
        public string MA_DVIQLY { get; set; }

        [Column("MA_LOAIBANGKE")]
        public string MA_LOAIBANGKE { get; set; }

        [Column("RoleId")]
        public int RoleId { get; set; }

        [Column("THU_TUKY")]
        public int THU_TUKY { get; set; }

        [Column("MO_TA")]
        public string MO_TA { get; set; }
    }
}