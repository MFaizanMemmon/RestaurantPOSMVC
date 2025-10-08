using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;
using System.Data;

namespace RestaurantManagementUI.Interfaces
{
    public interface IProduct
    {
        Task<IEnumerable<PorductViewModel>> GetAllProduct();
        Task<PorductViewModel> GetProductByID(int? id);

        Task<int> AddProduct(tbl_Product product);

        Task<int> UpdateProduct(tbl_Product product);

        Task<int> DeleteProduct(int? id);

    }
}
