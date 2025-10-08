using RestaurantManagementUI.Models;
using Dapper;
using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using System.Data;

namespace RestaurantManagementUI.Repository
{
    public class CategoryRepo : ICategory , IConnectionString
    {
        private  IDbConnection _connection;
        private IDbTransaction _transaction;

        public void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
            _connection = transaction.Connection;
        }

        public Task<int> AddCategory(tbl_Category category)
        {
            string query = "insert into Category (CategoryName) values (@CategoryName)";

            return _connection.ExecuteAsync(query, category, _transaction);

        }

        public async Task<int> DeleteCategory(int Id)
        {
            string query = "DELETE FROM Category WHERE CategoryId = @CategoryID";
            return await _connection.ExecuteAsync(query, new { CategoryID = Id }, transaction: _transaction);
        }


        public async Task<IEnumerable<tbl_Category>> GetAllCategory()
        {
            string query = "select * from Category order by 1 desc";

            return await _connection.QueryAsync<tbl_Category>(query, null, _transaction);

        }

        public async Task<tbl_Category> GetCategoryByID(int id)
        {
            string query = "select * from Category where CategoryID = @ID";

            var category = await _connection.QueryFirstOrDefaultAsync<tbl_Category>(query, new { ID = id }, _transaction);
            if (category == null)
            {
                throw new InvalidOperationException($"Category with ID {id} not found.");
            }
            return category;

        }

       

        public Task<int> UpdateCategory(tbl_Category category)
        {
            string query = "update Category set CategoryName = @CategoryName where CategoryID = @CategoryID";

            return _connection.ExecuteAsync(query, category, _transaction);

        }
    }
}
