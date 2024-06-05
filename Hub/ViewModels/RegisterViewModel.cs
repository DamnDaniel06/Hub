using System.ComponentModel.DataAnnotations;

namespace Hub.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "First Name")]
        public string First { get; set; }
        [Display(Name = "Last Name")]
        public string Last { get; set; }
        [Display(Name = "Email Addres")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Confirm Password is required")]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string confirmPassword { get; set; }
    }
}
