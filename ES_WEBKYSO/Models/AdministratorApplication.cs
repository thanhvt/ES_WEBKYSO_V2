using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;
namespace ES_WEBKYSO.Models
{
    [Table("Administrator_Application")]
    public partial class AdministratorApplication : IEntity
    {

        [Key]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string ApplicationId { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Key { get; set; }

    }
}