using System.Data;

namespace RestaurantManagementUI.Interfaces
{
    public interface IConnectionString
    {
        void SetConnection(IDbConnection connection);
        void SetTransaction(IDbTransaction transaction);
    }
}
