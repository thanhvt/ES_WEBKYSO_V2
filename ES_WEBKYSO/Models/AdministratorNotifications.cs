using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_Notifications")]
    public partial class AdministratorNotifications : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Content { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Link { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Icon { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Type { get; set; }

        public bool IsRead { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile HasUserProfile { get; set; }

    }
}