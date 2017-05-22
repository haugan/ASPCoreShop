using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models.StoreViewModels
{
    public class CustomerOrdersGroup
    {
        public int CustomerID { get; set; }
        [Display(Name="Customer #")]
        public string CustomerNumber { get; set; }
        public int OrderCount { get; set; }
    }
}
