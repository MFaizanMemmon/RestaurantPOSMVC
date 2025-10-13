using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;

namespace RestaurantManagementUI.Interfaces
{
    public interface IPOS
    {
        Task<IEnumerable<tbl_Main>> GetAllOrders();
        Task<IEnumerable<OrderDetailViewModel>> GetAllOrderDetail();
        Task<tbl_Main> GetOrderHeaderByID(int id);
        Task<int> AddOrderHeader(tbl_Main header);
        Task<int> AddOrderDetail(List<tbl_Detail> details);

        Task<int> UpdateOrderHeader(tbl_Main header);

        Task<int> UpdateReceivedPayment(int mainId, decimal received, decimal change, int masterConfigParentId);

    }
}
