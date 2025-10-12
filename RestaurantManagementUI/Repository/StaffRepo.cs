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

        public async Task<int> AddStaff(StaffViewModel staffViewModel)
        {
            return await _connection.ExecuteAsync("insert into Staff (StaffName, StaffPhone, RoleID, StaffRole, UserName, UserPassword) values (@StaffName, @StaffPhone, @RoleID, @StaffRole, @UserName, @UserPassword)", staffViewModel, transaction: _transaction);
        }

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
            string query = @"
                 SELECT 
                 s.StaffID,
                 s.StaffName,
                 s.UserName,
                 s.UserPassword,
                 s.RoleID,
                 r.RoleName as 'StaffRole'
                    FROM Staff s
                    LEFT JOIN tblRole r ON s.RoleID = r.RoleID
                    ORDER BY s.StaffID DESC";

            return await _connection.QueryAsync<tbl_Staff>(query, transaction: _transaction);
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

        public async Task<tbl_Staff?> StaffLogin(string userName, string userPassword)
        {
            return await _connection.QueryFirstOrDefaultAsync<tbl_Staff>("select * from Staff where UserName = @UserName and UserPassword = @UserPassword", new { UserName = userName, UserPassword = userPassword }, transaction: _transaction);
        }

        public async Task<int> UpdateStaff(StaffViewModel staffViewModel)
        {
            return await _connection.ExecuteAsync("update Staff set StaffName = @StaffName, StaffPhone = @StaffPhone, RoleID = @RoleID, StaffRole = @StaffRole, UserName = @UserName, UserPassword = @UserPassword where StaffID = @StaffID", staffViewModel, transaction: _transaction);
        }
    }
}
