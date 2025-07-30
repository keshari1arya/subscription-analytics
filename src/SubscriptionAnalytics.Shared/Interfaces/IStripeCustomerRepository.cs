using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IStripeCustomerRepository
{
    Task<StripeCustomer?> GetByIdAsync(Guid id);
    Task<StripeCustomer?> GetByStripeCustomerIdAsync(string stripeCustomerId);
    Task<IEnumerable<StripeCustomer>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<StripeCustomer>> GetByTenantIdAsync(Guid tenantId);
    Task<StripeCustomer> AddAsync(StripeCustomer stripeCustomer);
    Task<StripeCustomer> UpdateAsync(StripeCustomer stripeCustomer);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(string stripeCustomerId);
    Task<StripeCustomer?> GetByStripeCustomerIdAndTenantAsync(string stripeCustomerId, Guid tenantId);
}
