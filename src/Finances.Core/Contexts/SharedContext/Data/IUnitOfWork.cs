namespace Finances.Core.Contexts.SharedContext.Data;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();

    void Commit();

    void Rollback();
}

