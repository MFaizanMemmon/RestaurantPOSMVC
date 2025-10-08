using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;

namespace RestaurantManagementUI.Interfaces
{
    public interface IStaff
    {
        Task<IEnumerable<tbl_Staff>> GetAllStaff();
        Task<IEnumerable<tbl_Role>> GetAllRoles();
        Task<StaffViewModel> GetStaffByID(int? id);

        Task<int> AddStaff(StaffViewModel staffViewModel);
        Task<int> UpdateStaff(StaffViewModel staffViewModel);
        Task<int> DeleteStaff(int? id);

    }
}
