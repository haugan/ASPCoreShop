using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models.ShoppingCartViewModel
{
    public class CustomerOrdersViewModel
    {
        public int CustomerID { get; set; }
        [Display(Name="Customer #")]
        public string CustomerNumber { get; set; }
        public int OrderCount { get; set; }
    }
}
