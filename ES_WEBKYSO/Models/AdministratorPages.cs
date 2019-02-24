using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_Pages")]
    public partial class AdministratorPages : IEntity
    {

        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid PageId { get; set; }

        [Column(TypeName = "nvarchar")]
        public string PageTitle { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Url { get; set; }

        public virtual ICollection<AdministratorPageInRoles> ListAdministratorPageInRoles { get; set; }

    }
}