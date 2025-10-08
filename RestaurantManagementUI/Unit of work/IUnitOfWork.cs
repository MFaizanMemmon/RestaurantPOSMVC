using RestaurantManagementUI.Interfaces;

namespace RestaurantManagementUI.Unit_of_work
{
    public interface IUnitOfWork : IDisposable
    {
        ICategory Categories { get; }

        IProduct Products { get; }
        ITable Table { get; }
        IStaff Staffs { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

    }
}
