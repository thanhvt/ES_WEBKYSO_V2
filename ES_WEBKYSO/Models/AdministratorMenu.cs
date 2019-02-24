using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("Administrator_Menu")]
    public partial class AdministratorMenu : IEntity
    {

        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid MenuId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid ParentId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Style { get; set; }

        public int Index { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid PageId { get; set; }

        public bool ShowUp { get; set; }

    }
}