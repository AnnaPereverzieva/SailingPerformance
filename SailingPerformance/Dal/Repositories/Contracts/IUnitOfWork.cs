namespace Dal.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
    }
}
