using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("webpages_Roles")]
    public partial class WebpagesRoles : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string RoleName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        public string Description { get; set; }

        public virtual ICollection<AdministratorPageInRoles> ListAdministratorPageInRoles { get; set; }

        public virtual ICollection<WebpagesUsersInRoles> ListWebpagesUsersInRoles { get; set; }

    }
}