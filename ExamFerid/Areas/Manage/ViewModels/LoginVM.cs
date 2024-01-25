using System.ComponentModel.DataAnnotations;

namespace ExamFerid.Areas.Manage.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemmembered { get; set; }
    }
}
