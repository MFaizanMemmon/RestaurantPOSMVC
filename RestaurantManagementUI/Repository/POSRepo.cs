using Dapper;
using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using System.Data;
using System.Data.Common;

namespace RestaurantManagementUI.Repository
{
    public class POSRepo : IPOS, IConnectionString
    {
        private IDbConnection _connection;
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
        public async Task<IEnumerable<tbl_Main>> GetAllOrders()
        {
            return await _connection.QueryAsync<tbl_Main>(
                @"SELECT MainID, Date, [Time],
                 TableName, WaiterName, Status, OrderType, Total, DriverID,
                 CustName, CustPhone
          FROM dbo.tblMain
          ORDER BY MainID DESC"
            );
        }

        public async Task<int> AddOrderHeader(tbl_Main header)
        {
            string query = @"
        INSERT INTO tblMain 
        (Date, Time, TableName, WaiterName, Status, OrderType, Total, Recieved, [Change], DriverID, CustName, CustPhone, IsPrint, IsPrintUnPaid, PaidDateTime)
        VALUES 
        (@Date, CONVERT(TIME, GETDATE()), @TableName, @WaiterName, @Status, @OrderType, @Total, @Recieved, @Change, @DriverID, @CustName, @CustPhone, @IsPrint, @IsPrintUnPaid, @PaidDateTime);
        SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _connection.QuerySingleAsync<int>(query, header, _transaction);
        }

        public async Task<int> AddOrderDetail(List<tbl_Detail> details)
        {
            string query = @"INSERT INTO tblDetail
                        (MainID, ProID, Qty, Price, Amount)
                        VALUES
                        (@MainID, @ProID, @Qty, @Price, @Amount)";

            return await _connection.ExecuteAsync(query, details, _transaction);
        }

    }
}
