using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.Interfaces
{
    public interface ITable
    {
        Task<IEnumerable<tbl_Table>> GetAllTable();
        Task<tbl_Table> GetTableByID(int? id);
        Task<int> AddTable(tbl_Table table);
        Task<int> UpdateTable(tbl_Table table);
        Task<int> DeleteTable(int? id);

    }
}
