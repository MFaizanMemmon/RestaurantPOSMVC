using Dapper;
using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;
using System.Data;

namespace RestaurantManagementUI.Repository
{
    public class ProductRepo : IProduct , IConnectionString
    {
        private IDbConnection _connection { get; set; }
        private IDbTransaction _transaction { get; set; }
        public void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _connection = transaction.Connection;
            _transaction = transaction;
        }

        public async Task<IEnumerable<PorductViewModel>> GetAllProduct()
        {
            return await _connection.QueryAsync<PorductViewModel>("SP_GetAllProducts",transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<PorductViewModel> GetProductByID(int? id)
        {
            string query = "SELECT * FROM tbl_Product WHERE ProductID = @ProductID";
            return await _connection.QueryFirstOrDefaultAsync<PorductViewModel>(query, new { ProductID = id }, transaction: _transaction);
        }

        public async Task<int> DeleteProduct(int? id)
        {
            return await _connection.ExecuteAsync("SP_DeleteProduct", new { ProductID = id }, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddProduct(tbl_Product product)
        {
            return await _connection.ExecuteAsync("SP_AddProduct", new
            {
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            }, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateProduct(tbl_Product product)
        {
            return await _connection.ExecuteAsync("SP_UpdateProduct", new
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            }, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }
    }
}
