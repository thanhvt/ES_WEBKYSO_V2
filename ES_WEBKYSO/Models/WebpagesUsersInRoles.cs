using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("webpages_UsersInRoles")]
    public partial class WebpagesUsersInRoles : IEntity
    {

        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual WebpagesRoles HasWebpagesRoles { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile HasUserProfile { get; set; }

    }
}