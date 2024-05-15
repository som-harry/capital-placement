namespace CapitalReplacement.Application.Interfaces.Persistence
{
    public interface IUnitOfWork 
    {
        Task<int> CommitChangesAsync();
        int CommitChanges();

        Task CommitAsync();
    }
}
