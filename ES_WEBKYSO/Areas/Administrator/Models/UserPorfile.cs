using System;
using System.Linq;

namespace Administrator.Department.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public string DepartmentName { get; set; }
    }
}