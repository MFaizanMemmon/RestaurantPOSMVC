using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.View_Models
{
    public class ConfirmOrderViewModel
    {
        public tbl_Main OrderHeader { get; set; }

        public List<tbl_Detail> OrderDetail { get; set; }
    }
}
