using ES_WEBKYSO.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Models
{
    [Table("CA_DataSignForDB")]
    public class CA_DataSignForDB : IEntity
    {
        [Key]
        [Column("DataID")]
        public int DataID { get; set; }

        [Column("DataSignID")]
        public int DataSignID { get; set; }

        [Column("ColumnName")]
        public string ColumnName { get; set; }

        [Column("ColumnValue")]
        public string ColumnValue { get; set; }

        [Column("ColumnType")]
        public int ColumnType { get; set; }
    }
}