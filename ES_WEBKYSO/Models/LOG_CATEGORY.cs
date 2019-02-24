using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("LOG_CATEGORY")]
    public partial class LOG_CATEGORY : IEntity
    {
        [Key]
        [Column("LOG_CATEGORY_ID")]
        public string LOG_CATEGORY_ID { get; set; }

        [Column("LOG_CATEGORY_NAME")]
        public string LOG_CATEGORY_NAME { get; set; }
    }
}