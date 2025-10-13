using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.View_Models
{
    public class ReceivedPaymentViewModel
    {
        public int MainID { get; set; }
        public List<SelectListItem>? PaymentTypeList { get; set; }
        public int PaymentTypeID { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
    }
}
