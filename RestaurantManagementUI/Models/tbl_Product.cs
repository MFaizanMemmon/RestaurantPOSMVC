namespace RestaurantManagementUI.Models
{
    public class tbl_Product
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
