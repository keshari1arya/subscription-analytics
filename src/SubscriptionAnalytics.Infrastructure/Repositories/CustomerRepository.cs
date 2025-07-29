using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.StripeCustomers)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByCustomerIdAsync(string customerId)
    {
        return await _context.Customers
            .Include(c => c.StripeCustomers)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.StripeCustomers)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetByTenantIdAsync(Guid tenantId)
    {
        return await _context.Customers
            .Include(c => c.StripeCustomers)
            .Where(c => c.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string customerId)
    {
        return await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .Include(c => c.StripeCustomers)
            .FirstOrDefaultAsync(c => c.Email == email);
    }
}
