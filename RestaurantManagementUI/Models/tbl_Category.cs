using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementUI.Models
{
    public class tbl_Category
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(25, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string? CategoryName { get; set; }
    }
}
