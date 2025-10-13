using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.Interfaces
{
    public interface IMasterConfig
    {
        Task<IEnumerable<tbl_MasterConfig>> GetAllMasterConfigByParentID(int id);

    }
}
