using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByCustomerIdAsync(string customerId);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> GetByTenantIdAsync(Guid tenantId);
    Task<Customer> AddAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(string customerId);
    Task<Customer?> GetByEmailAsync(string email);
}
