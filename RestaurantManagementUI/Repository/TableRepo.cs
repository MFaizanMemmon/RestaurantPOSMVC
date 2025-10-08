using Dapper;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using System.Data;
using System.Transactions;

namespace RestaurantManagementUI.Repository
{
    public class TableRepo : ITable, IConnectionString
    {
        private  IDbConnection _connectionString;
        private  IDbTransaction _transaction;

        public async Task<int> AddTable(tbl_Table table)
        {
            return await _connectionString.ExecuteAsync("insert into Tables (TName) values (@TName)", new { TName = table.TName }, transaction: _transaction);
        }

        public async Task<int> DeleteTable(int? id)
        {
            return await _connectionString.ExecuteAsync("delete from Tables where Tid = @Tid", new { Tid = id }, transaction: _transaction);
        }

        public async Task<IEnumerable<tbl_Table>> GetAllTable()
        {
           return await _connectionString.QueryAsync<tbl_Table>("select * from Tables order by 1 desc", transaction: _transaction);
        }

        public async Task<tbl_Table> GetTableByID(int? id)
        {
           return await _connectionString.QueryFirstOrDefaultAsync<tbl_Table>("select * from Tables where Tid = @Tid", new { Tid = id }, transaction: _transaction);
        }

        public void SetConnection(IDbConnection connection)
        {
            _connectionString = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _connectionString = transaction.Connection;
            _transaction = transaction;
        }

        public async Task<int> UpdateTable(tbl_Table table)
        {
            return await _connectionString.ExecuteAsync("update Tables set TName = @TName where Tid = @Tid", new { TName = table.TName, Tid = table.Tid }, transaction: _transaction);
        }
    }
}
