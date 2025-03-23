namespace Domain.Contacts;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
