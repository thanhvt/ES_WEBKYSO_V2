using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Administrator.Department.Models
{
    public class DepartmentContext : DbContext
    {
        public DepartmentContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Administrator_Department> Administrator_Department { get; set; }
        public DbSet<webpages_Membership> webpages_Memberships { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(150)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string SkypeID { get; set; }
        [MaxLength(200)]
        public string CompanyName { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        public int GenderId { get; set; }
        public int DepartmentId { get; set; }
    }

    [Table("Administrator_Department")]
    public class Administrator_Department
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        [MaxLength(500)]
        [Required(ErrorMessage = "Tên đơn vị không được bỏ trống")]
        public string DepartmentName { get; set; }
        [MaxLength(20)]
        [Required(ErrorMessage = "Mã đơn vị không được bỏ trống")]
        public string DepartmentCode { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public int ParentId { get; set; }
        public int DepartmentLevel { get; set; }
        [Required(ErrorMessage = "STT không được bỏ trống")]
        public int DepartmentIndex { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("UserProfile")]
        public int CreateUser { get; set; }
        public bool IsActive { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }

    [Table("webpages_Membership")]
    public class webpages_Membership
    {
        [Key]
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }
}