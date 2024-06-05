using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using Hub.Data.Enum;
using Microsoft.AspNetCore.Identity;

namespace Hub.Models
{
    public class User:IdentityUser
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public ICollection<Product> products { get ; set ; }
    }
}
