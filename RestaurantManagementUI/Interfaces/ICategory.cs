using RestaurantManagementUI.Models;
using System.Data;

namespace RestaurantManagementUI.Interfaces
{
    public interface ICategory
    {
        Task<int> AddCategory(tbl_Category category);
        Task<int> UpdateCategory(tbl_Category category);
        Task<int> DeleteCategory(int Id);
        Task<IEnumerable<tbl_Category>> GetAllCategory();
        Task<tbl_Category> GetCategoryByID(int id);
       
    }
}
