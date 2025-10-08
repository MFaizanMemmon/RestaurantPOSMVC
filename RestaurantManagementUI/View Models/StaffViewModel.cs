using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.View_Models
{
    public class StaffViewModel : tbl_Staff
    {
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
