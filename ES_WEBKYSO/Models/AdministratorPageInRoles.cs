using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_PageInRoles")]
    public partial class AdministratorPageInRoles : IEntity
    {

        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "uniqueidentifier")]
        public Guid PageId { get; set; }

        [ForeignKey("RoleId")]
        public virtual WebpagesRoles HasWebpagesRoles { get; set; }

        [ForeignKey("PageId")]
        public virtual AdministratorPages HasAdministratorPages { get; set; }

    }
}