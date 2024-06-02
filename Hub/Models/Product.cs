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

        private ProductCategory category;
        public string Category 
        {
            //get { return (ProductCategory)Enum.Parse(typeof(ProductCategory), category, true); }
            //set { category = Category.ToString(); } 

            get { return Category = category.ToString(); }
            set { category = (ProductCategory)Enum.Parse(typeof(ProductCategory), value, true); }
        }
        

        [Display(Name = "Production Date")]
        private DateTime productionDate;
        public DateTime ProductionDate 
        {
            get { return productionDate; }
            set { productionDate = DateTime.Now; } 
        }

        [ForeignKey("User")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
    }
}
