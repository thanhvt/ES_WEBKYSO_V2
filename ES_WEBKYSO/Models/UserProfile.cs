using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("UserProfile")]
    public partial class UserProfile : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(56)]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Column("SkypeID", TypeName = "nvarchar")]
        [MaxLength(100)]
        public string SkypeId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string AuthAccountId { get; set; }

        public int? GenderId { get; set; }

        public int? DepartmentId { get; set; }

        public virtual ICollection<AdministratorDepartment> ListAdministratorDepartment { get; set; }

        public virtual ICollection<WebpagesUsersInRoles> ListWebpagesUsersInRoles { get; set; }

        public virtual ICollection<AdministratorRoleInGroups> ListAdministratorRoleInGroups { get; set; }

        public virtual ICollection<AdministratorNotifications> ListAdministratorNotifications { get; set; }
        
        public virtual ICollection<CFG_SOGCS_NVIEN> ListSOGCS_NVIEN { get; set; }

        //[ForeignKey("USERID")]
        //public virtual DM_DOIGCS HasDM_DOIGCS { get; set; }
    }
}