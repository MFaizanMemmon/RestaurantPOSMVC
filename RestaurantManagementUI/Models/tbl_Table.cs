using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementUI.Models
{
    public class tbl_Table
    {
        public int Tid { get; set; }

        [Required(ErrorMessage = "Table Name is required.")]
        [StringLength(50, ErrorMessage = "Table Name cannot exceed 50 characters.")]
        public string? TName { get; set; }
    }
}
