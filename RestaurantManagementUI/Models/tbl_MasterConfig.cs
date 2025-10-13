namespace RestaurantManagementUI.Models
{
    public class tbl_MasterConfig
    {
        public int? ConfigID { get; set; }
        public int? ParentID { get; set; }
        public string? ConfigName { get; set; }
        public int? ConfigValue { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public int CreatedDate { get; set; }
    }
}
