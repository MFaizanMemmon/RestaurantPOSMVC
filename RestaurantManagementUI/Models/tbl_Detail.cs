namespace RestaurantManagementUI.Models
{
    public class tbl_Detail
    {
        public int DetailID { get; set; }
        public int? MainID { get; set; }
        public int? ProID { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
