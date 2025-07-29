namespace StripeDataGenerator.Models;

public class SaasProduct
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ProductTier> Tiers { get; set; } = new();
    public string Category { get; set; } = string.Empty;
}

public class ProductTier
{
    public string Name { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public int UserLimit { get; set; }
    public List<string> Features { get; set; } = new();
    public string StripePriceId { get; set; } = string.Empty;
}
