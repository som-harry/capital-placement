﻿
namespace CapitalReplacement.Persistence.Configurations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;

        }

        public int CommitChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> CommitChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
