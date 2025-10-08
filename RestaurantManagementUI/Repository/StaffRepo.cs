using Dapper;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;
using System.Data;

namespace RestaurantManagementUI.Repository
{
    public class StaffRepo : IConnectionString, IStaff
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public async Task<int> DeleteStaff(int? id)
        {
            return await _connection.ExecuteAsync("delete from Staff where StaffID = @StaffID", new { StaffID = id }, transaction: _transaction);
        }

        public async Task<IEnumerable<tbl_Role>> GetAllRoles()
        {
            return await _connection.QueryAsync<tbl_Role>("select * from TblRole order by 1 desc", transaction: _transaction);
        }

        public async Task<IEnumerable<tbl_Staff>> GetAllStaff()
        {
            return await _connection.QueryAsync<tbl_Staff>("select * from Staff order by 1 desc", transaction: _transaction);
        }

        public async Task<StaffViewModel> GetStaffByID(int? id)
        {
            return await _connection.QueryFirstOrDefaultAsync<StaffViewModel>("select * from Staff where StaffID = @StaffID", new { StaffID = id }, transaction: _transaction);
        }

        public void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _connection = transaction.Connection;
            _transaction = transaction;
        }
    }
}
