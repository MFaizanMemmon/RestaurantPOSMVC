using Dapper;
using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.View_Models;
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



        public async Task<IEnumerable<OrderDetailViewModel>> GetAllOrderDetail()
        {
            string query = @"select d.*,p.ProductName from TblDetail d 
                            inner join Product p on p.ProductID = d.ProID";
            return await _connection.QueryAsync<OrderDetailViewModel>(query);
        }

        public async Task<tbl_Main> GetOrderHeaderByID(int id)
        {
            const string query = @"
                            SELECT *
                            FROM tblMain
                            WHERE MainID = @id";

            var result = await _connection.QueryFirstOrDefaultAsync<tbl_Main>(
                query,
                new { id },
                transaction: _transaction
            );

            return result;
        }
        public async Task<int> UpdateOrderHeader(tbl_Main header)
        {
            const string query = @"
        UPDATE tblMain
        SET
            Date = ISNULL(@Date, Date),
            TableName = ISNULL(@TableName, TableName),
            WaiterName = ISNULL(@WaiterName, WaiterName),
            Status = ISNULL(@Status, Status),
            OrderType = ISNULL(@OrderType, OrderType),
            Total = ISNULL(@Total, Total),
            Recieved = ISNULL(@Recieved, Recieved),
            [Change] = ISNULL(@Change, [Change]),
            DriverID = ISNULL(@DriverID, DriverID),
            CustName = ISNULL(@CustName, CustName),
            CustPhone = ISNULL(@CustPhone, CustPhone),
            IsPrint = ISNULL(@IsPrint, IsPrint),
            IsPrintUnPaid = ISNULL(@IsPrintUnPaid, IsPrintUnPaid),
            PaidDateTime = ISNULL(@PaidDateTime, PaidDateTime)
        WHERE MainID = @MainID;
        
        SELECT @MainID;";

            var result = await _connection.QuerySingleAsync<int>(query, header, _transaction);
            return result;
        }

        public async Task<int> UpdateReceivedPayment(int mainId, decimal received, decimal change, int masterConfigParentId)
        {
            string query = @"
        UPDATE tblMain
        SET 
            Recieved = @Recieved,
            [Change] = @Change,
            MasterConfigParentID = @MasterConfigParentID,
            Status = 'Paid',
            PaidDateTime = GETDATE()
        WHERE MainID = @MainID;
        SELECT @MainID;";

            var parameters = new
            {
                MainID = mainId,
                Recieved = received,
                Change = change,
                MasterConfigParentID = masterConfigParentId
            };

            try
            {
                return await _connection.QuerySingleAsync<int>(query, parameters, _transaction);
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }



    }
}
