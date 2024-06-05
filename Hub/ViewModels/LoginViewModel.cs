using System.ComponentModel.DataAnnotations;

namespace Hub.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Email Addres")]
        [Required(ErrorMessage ="Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
