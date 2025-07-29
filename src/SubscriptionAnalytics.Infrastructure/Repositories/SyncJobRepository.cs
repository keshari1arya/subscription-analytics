using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Repositories;

public class SyncJobRepository : ISyncJobRepository
{
    private readonly AppDbContext _context;

    public SyncJobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SyncJob?> GetByIdAsync(Guid id)
    {
        return await _context.SyncJobs.FirstOrDefaultAsync(sj => sj.Id == id);
    }

    public async Task<IEnumerable<SyncJob>> GetByTenantIdAsync(Guid tenantId)
    {
        return await _context.SyncJobs
            .Where(sj => sj.TenantId == tenantId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync();
    }

    public async Task<SyncJob> AddAsync(SyncJob syncJob)
    {
        _context.SyncJobs.Add(syncJob);
        await _context.SaveChangesAsync();
        return syncJob;
    }

    public async Task<SyncJob> UpdateAsync(SyncJob syncJob)
    {
        _context.SyncJobs.Update(syncJob);
        await _context.SaveChangesAsync();
        return syncJob;
    }
}
