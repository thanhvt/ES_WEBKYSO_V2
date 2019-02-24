using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.DataContext;

namespace ES_WEBKYSO.Models
{
    [Table("ConfigInput")]
    public partial class ConfigInput : IEntity
    {
        [Key]
        [Column("ConfigId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfigId { get; set; }

        [Column("TypeInput")]
        [MaxLength(50, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Loại tham số")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string TypeInput { get; set; }

        [Column("Desctiption")]
        [MaxLength(200, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Mô tả")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Desctiption { get; set; }

        [Column("Value")]
        [MaxLength(500, ErrorMessage = "{0} không được dài quá {1} ký tự")]
        [Display(Name = "Giá trị")]
        [UIHint("TextInput")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Value { get; set; }

    }
}