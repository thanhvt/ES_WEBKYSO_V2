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
    [Table("CA_DataSign")]
    public class CA_DataSign : IEntity
    {
        [Key]
        [Column("DataSignID")]
        public int DataSignID { get; set; }

        [Column("KeySign")]
        public string KeySign { get; set; }

        [Column("FileID")]
        public int FileID { get; set; }

        [Column("FileData")]
        public string FileData { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }

        [Column("ProgramSign")]
        public string ProgramSign { get; set; }

        [Column("UserSign")]
        public string UserSign { get; set; }

        [Column("SignTime")]
        public DateTime SignTime { get; set; }

        [Column("MethodName")]
        public string MethodName { get; set; }
    }
}