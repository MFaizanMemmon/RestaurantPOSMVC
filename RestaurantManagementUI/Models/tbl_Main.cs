namespace RestaurantManagementUI.Models
{
    public class tbl_Main
    {
        public int MainID { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string? TableName { get; set; }
        public string? WaiterName { get; set; }
        public string? Status { get; set; }
        public string? OrderType { get; set; }
        public decimal Total { get; set; }
        public decimal Received { get; set; }
        public decimal Changed { get; set; }
        public int DriverID { get; set; }
        public string? CustomerName { get; set; }
        public bool IsPrint { get; set; }
        public bool IsPrintUnPaid { get; set; }
        public DateTime PrintDateTime { get; set; }

    }
}
