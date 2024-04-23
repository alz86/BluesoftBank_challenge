using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Bluesoft.Bank.Business;

namespace Bluesoft.Bank.DataAccess.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankDbContext _context;
        private IDbContextTransaction? _transaction;
        private int _transactionCount = 0;

        public UnitOfWork(BankDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
            _transactionCount++;
        }

        public async Task CommitAsync()
        {
            _transactionCount--;
            if (_transactionCount == 0 && _transaction != null)
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            _transactionCount = 0;
        }

        public async Task SaveAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("An open transaction is pending, commit or rollback the transaction before saving.");

            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
