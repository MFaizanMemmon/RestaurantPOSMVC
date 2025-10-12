using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using System;
using System.Data;
using System.Data.Common;

namespace RestaurantManagementUI.Unit_of_work
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
     
        private bool _disposed;

        public ICategory Categories { get; }

        public IProduct Products { get; }

        public ITable Table { get; }

        public IStaff Staffs { get;  }
        public IPOS POS { get; }
        public UnitOfWork(DBConnection dbConnection, ICategory categories, IProduct products,ITable table,IStaff staff,IPOS pos)
        {
            _connection = dbConnection.CreateConnection();
            Categories = categories;
            Products = products;
            Table = table;
            Staffs = staff;
            POS = pos;
            if (Categories is IConnectionString catRepo)
            {
                catRepo.SetConnection(_connection);
            }

            if (Products is IConnectionString prodRepo)
            {
                prodRepo.SetConnection(_connection);
            }

            if (Table is IConnectionString tableRepo)
            {
                tableRepo.SetConnection(_connection);
            }
            if (Staffs is IConnectionString staffRepo)
            {
                staffRepo.SetConnection(_connection);
            }
            if (POS is IConnectionString posRep)
            {
                posRep.SetConnection(_connection);
            }

        }

        // Start transaction
        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();

            // Attach transaction to repositories if needed
            if (Categories is IConnectionString catRepo)
            {
                catRepo.SetConnection(_connection);
                catRepo.SetTransaction(_transaction);
            }

            if (Products is IConnectionString prodRepo)
            {
                prodRepo.SetConnection(_connection);
                prodRepo.SetTransaction(_transaction);
            }

            if (Table is IConnectionString tableRepo)
            {
                tableRepo.SetConnection(_connection);
                tableRepo.SetTransaction(_transaction);
            }
            if (Staffs is IConnectionString staffRepo)
            {
                staffRepo.SetConnection(_connection);
                staffRepo.SetTransaction(_transaction);
            }
            if (POS is IConnectionString posRep)
            {
                posRep.SetConnection(_connection);
                posRep.SetTransaction(_transaction);
            }

        }

        // Commit changes
        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        // Rollback on failure
        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        // Dispose pattern to release connection and transaction
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            // Don't close or dispose the connection manually — DI will handle it
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _transaction?.Dispose();

                        if (_connection != null && _connection.State != ConnectionState.Closed)
                            _connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Dispose error: " + ex.Message);
                    }
                }

                _disposed = true;
            }
        }


        public void BeingTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
