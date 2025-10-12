using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementUI.View_Models
{
    public class PorductViewModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? CategoryName { get; set; }

        // Path to stored image
        public string? ImageUrl { get; set; }


        [NotMapped]
        public IFormFile? ProductUrl { get; set; }

        public List<SelectListItem>? Categories { get; set; }
        public int CategoryID { get; set; }
    }
}
