using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_Department")]
    public partial class AdministratorDepartment : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Email { get; set; }

        public int ParentId { get; set; }

        public int DepartmentLevel { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string DepartmentCode { get; set; }

        public int DepartmentIndex { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        public int CreateUser { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("CreateUser")]
        public virtual UserProfile HasUserProfile { get; set; }

    }
}