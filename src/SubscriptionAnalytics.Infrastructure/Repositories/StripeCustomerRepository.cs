using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Repositories;

public class StripeCustomerRepository : IStripeCustomerRepository
{
    private readonly AppDbContext _context;

    public StripeCustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StripeCustomer?> GetByIdAsync(Guid id)
    {
        return await _context.StripeCustomers
            .Include(sc => sc.Customer)
            .FirstOrDefaultAsync(sc => sc.Id == id);
    }

    public async Task<StripeCustomer?> GetByStripeCustomerIdAsync(string stripeCustomerId)
    {
        return await _context.StripeCustomers
            .Include(sc => sc.Customer)
            .FirstOrDefaultAsync(sc => sc.StripeCustomerId == stripeCustomerId);
    }

    public async Task<IEnumerable<StripeCustomer>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.StripeCustomers
            .Include(sc => sc.Customer)
            .Where(sc => sc.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<StripeCustomer>> GetByTenantIdAsync(Guid tenantId)
    {
        return await _context.StripeCustomers
            .Include(sc => sc.Customer)
            .Where(sc => sc.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<StripeCustomer> AddAsync(StripeCustomer stripeCustomer)
    {
        _context.StripeCustomers.Add(stripeCustomer);
        await _context.SaveChangesAsync();
        return stripeCustomer;
    }

    public async Task<StripeCustomer> UpdateAsync(StripeCustomer stripeCustomer)
    {
        _context.StripeCustomers.Update(stripeCustomer);
        await _context.SaveChangesAsync();
        return stripeCustomer;
    }

    public async Task DeleteAsync(Guid id)
    {
        var stripeCustomer = await _context.StripeCustomers.FindAsync(id);
        if (stripeCustomer != null)
        {
            _context.StripeCustomers.Remove(stripeCustomer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string stripeCustomerId)
    {
        return await _context.StripeCustomers.AnyAsync(sc => sc.StripeCustomerId == stripeCustomerId);
    }

    public async Task<StripeCustomer?> GetByStripeCustomerIdAndTenantAsync(string stripeCustomerId, Guid tenantId)
    {
        return await _context.StripeCustomers
            .Include(sc => sc.Customer)
            .FirstOrDefaultAsync(sc => sc.StripeCustomerId == stripeCustomerId && sc.TenantId == tenantId);
    }
}
