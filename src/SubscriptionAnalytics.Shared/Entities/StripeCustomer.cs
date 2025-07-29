namespace SubscriptionAnalytics.Shared.Entities;

public class StripeCustomer : BaseTenantEntity
{
    public Guid CustomerId { get; set; } // FK to Customer
    public string StripeCustomerId { get; set; } = string.Empty; // Stripe's customer ID
    public string? DefaultPaymentMethod { get; set; }
    public string? TaxInfo { get; set; }
    public string? TaxInfoVerification { get; set; }
    public bool Delinquent { get; set; }
    public int? Discount { get; set; }
    public string? InvoicePrefix { get; set; }
    public string? InvoiceSettings { get; set; }
    public string? NextInvoiceSequence { get; set; }
    public string? PreferredLocales { get; set; }
    public string? Shipping { get; set; }
    public string? Source { get; set; }
    public string? Subscriptions { get; set; }
    public string? Tax { get; set; }
    public string? TaxExempt { get; set; }
    public string? TestClock { get; set; }

    // Navigation property
    public virtual Customer Customer { get; set; } = null!;
}
