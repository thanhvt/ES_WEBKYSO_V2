using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_RoleGroups")]
    public partial class AdministratorRoleGroups : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleGroupId { get; set; }

        [Column(TypeName = "nvarchar")]
        public string RoleGroupName { get; set; }

        public virtual ICollection<AdministratorRoleInGroups> ListAdministratorRoleInGroups { get; set; }

    }
}