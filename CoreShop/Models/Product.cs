using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class Product
    {
        [Display(Name="ID")]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; } // PRIMARY KEY

        [Required]
        [Display(Name="Product #")]
        public string ProductNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency), Range(1,99999)]
        public int Price { get; set; }

        [Display(Name="Item is on sale")]
        public bool OnSale { get; set; }

        [Display(Name="Image 1")]
        public string ImgURL1 { get; set; }

        [Display(Name="Image 2")]
        public string ImgURL2 { get; set; }

        [Display(Name="Image 3")]
        public string ImgURL3 { get; set; }

        [Display(Name="Short description")]
        public string DescriptionShort { get; set; }

        [Display(Name="Long description")]
        public string DescriptionLong { get; set; }
    }
}
