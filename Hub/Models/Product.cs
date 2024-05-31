using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hub.Data.Enum;

namespace Hub.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public ProductCategory Category { get; set; }

        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        [ForeignKey("User")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }
    }
}
