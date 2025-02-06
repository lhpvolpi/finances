using Finances.Core.Contexts.SharedContext.Data;

namespace Finances.Infra.Data;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly FinancesDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(FinancesDbContext context)
        => this._context = context;

    public void BeginTransaction()
        => this._transaction = this._context.Database.BeginTransaction();

    public void Commit()
    {
        this._context.SaveChanges();
        this._transaction.Commit();

        this.Dispose();
    }

    public void Rollback()
    {
        this._transaction?.Rollback();

        this.Dispose();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed)
            return;

        if (disposing)
            this._transaction?.Dispose();

        this._disposed = true;
    }
}


