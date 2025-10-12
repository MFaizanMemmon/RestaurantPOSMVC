using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.View_Models
{
    public class AdminPOSViewModel
    {
        public List<tbl_Table?> Tables { get; set; }
        public List<tbl_Staff?> Staff { get; set; }
        public List<tbl_Category?> Categories { get; set; }
        public List<tbl_Product?> Products { get; set; }
    }
}
