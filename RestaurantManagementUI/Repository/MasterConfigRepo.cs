using Dapper;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using System.Data;

namespace RestaurantManagementUI.Repository
{
    public class MasterConfigRepo : IConnectionString , IMasterConfig
    {
        public IDbConnection _connection { get; set; }
        public IDbTransaction _transaction { get; set; }
        public void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _connection = transaction.Connection;
            _transaction = transaction;
        }
        public async Task<IEnumerable<tbl_MasterConfig>> GetAllMasterConfigByParentID(int id)
        {
            return await _connection.QueryAsync<tbl_MasterConfig>(
                @"SELECT ConfigID, ConfigName 
                  FROM dbo.tbl_MasterConfig
                  WHERE ParentID = @Id
                  ORDER BY ConfigID ", new { Id = id }, _transaction
            );
        }

        
    }
}
