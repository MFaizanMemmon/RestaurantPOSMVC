using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.View_Models
{
    public class GetAllOrdersViewModel
    {
        public IEnumerable<tbl_Main> OrderHeader { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
