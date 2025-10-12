using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementUI.Models
{
    public class tbl_Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product Price must be greater than zero.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }

        [StringLength(200, ErrorMessage = "Image URL cannot exceed 200 characters.")]
        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string? ImageUrl { get; set; }
    }
}
