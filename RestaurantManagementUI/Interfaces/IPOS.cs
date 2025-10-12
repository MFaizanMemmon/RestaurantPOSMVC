using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.Interfaces
{
    public interface IPOS
    {
        Task<IEnumerable<tbl_Main>> GetAllOrders();

        Task<int> AddOrderHeader(tbl_Main header);
        Task<int> AddOrderDetail(List<tbl_Detail> details);

    }
}
