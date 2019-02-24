using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_RoleInGroups")]
    public partial class AdministratorRoleInGroups : IEntity
    {

        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int RoleGroupId { get; set; }

        [Column("UserProfile_UserId")]
        public int? UserProfileUserId { get; set; }

        [ForeignKey("RoleGroupId")]
        public virtual AdministratorRoleGroups HasAdministratorRoleGroups { get; set; }

        [ForeignKey("UserProfileUserId")]
        public virtual UserProfile HasUserProfile { get; set; }

    }
}