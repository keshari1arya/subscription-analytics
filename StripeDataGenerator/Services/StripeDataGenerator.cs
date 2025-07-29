using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using StripeDataGenerator.Models;

namespace StripeDataGenerator.Services;

public class StripeDataGenerator
{
    private readonly ILogger<StripeDataGenerator> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _connectedAccountId;
    private readonly List<SaasProduct> _products;

    public StripeDataGenerator(ILogger<StripeDataGenerator> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        // Try to get API key from configuration, then environment variable
        _apiKey = _configuration["Stripe:ApiKey"] ??
                  Environment.GetEnvironmentVariable("STRIPE_API_KEY") ??
                  throw new InvalidOperationException("Stripe API key not found in configuration or STRIPE_API_KEY environment variable");

        _connectedAccountId = _configuration["Stripe:ConnectedAccountId"] ?? throw new InvalidOperationException("Connected account ID not found");

        StripeConfiguration.ApiKey = _apiKey;

        _products = InitializeProducts();
    }

    private List<SaasProduct> InitializeProducts()
    {
        return new List<SaasProduct>
        {
            new SaasProduct
            {
                Id = "projectflow-pro",
                Name = "ProjectFlow Pro",
                Description = "Advanced project management and team collaboration platform",
                Category = "Project Management",
                Tiers = new List<ProductTier>
                {
                    new ProductTier
                    {
                        Name = "Basic",
                        MonthlyPrice = 29.00m,
                        YearlyPrice = 290.00m,
                        UserLimit = 5,
                        Features = new List<string> { "Task Management", "Team Chat", "File Sharing", "Basic Reports" }
                    },
                    new ProductTier
                    {
                        Name = "Pro",
                        MonthlyPrice = 79.00m,
                        YearlyPrice = 790.00m,
                        UserLimit = 25,
                        Features = new List<string> { "Advanced Analytics", "Time Tracking", "Custom Workflows", "API Access", "Priority Support" }
                    },
                    new ProductTier
                    {
                        Name = "Enterprise",
                        MonthlyPrice = 199.00m,
                        YearlyPrice = 1990.00m,
                        UserLimit = -1, // Unlimited
                        Features = new List<string> { "Unlimited Users", "Advanced Security", "Custom Integrations", "Dedicated Support", "SLA Guarantee" }
                    }
                }
            },
            new SaasProduct
            {
                Id = "datainsight-analytics",
                Name = "DataInsight Analytics",
                Description = "Business intelligence and data analytics platform",
                Category = "Analytics",
                Tiers = new List<ProductTier>
                {
                    new ProductTier
                    {
                        Name = "Starter",
                        MonthlyPrice = 49.00m,
                        YearlyPrice = 490.00m,
                        UserLimit = 3,
                        Features = new List<string> { "Basic Dashboards", "Data Import", "Standard Reports", "Email Alerts" }
                    },
                    new ProductTier
                    {
                        Name = "Professional",
                        MonthlyPrice = 129.00m,
                        YearlyPrice = 1290.00m,
                        UserLimit = 10,
                        Features = new List<string> { "Advanced Visualizations", "Real-time Data", "Custom Metrics", "API Access", "Advanced Security" }
                    },
                    new ProductTier
                    {
                        Name = "Enterprise",
                        MonthlyPrice = 299.00m,
                        YearlyPrice = 2990.00m,
                        UserLimit = -1,
                        Features = new List<string> { "Unlimited Data Sources", "Custom AI Models", "White-label Options", "Dedicated Infrastructure", "24/7 Support" }
                    }
                }
            },
            new SaasProduct
            {
                Id = "securecloud-backup",
                Name = "SecureCloud Backup",
                Description = "Enterprise-grade cloud backup and disaster recovery solution",
                Category = "Backup & Recovery",
                Tiers = new List<ProductTier>
                {
                    new ProductTier
                    {
                        Name = "Basic",
                        MonthlyPrice = 19.00m,
                        YearlyPrice = 190.00m,
                        UserLimit = 1,
                        Features = new List<string> { "100GB Storage", "Automatic Backup", "File Recovery", "Mobile Access" }
                    },
                    new ProductTier
                    {
                        Name = "Professional",
                        MonthlyPrice = 59.00m,
                        YearlyPrice = 590.00m,
                        UserLimit = 5,
                        Features = new List<string> { "1TB Storage", "Version Control", "Advanced Encryption", "Cross-platform Sync", "Priority Support" }
                    },
                    new ProductTier
                    {
                        Name = "Enterprise",
                        MonthlyPrice = 149.00m,
                        YearlyPrice = 1490.00m,
                        UserLimit = -1,
                        Features = new List<string> { "Unlimited Storage", "Advanced Security", "Compliance Tools", "Custom Retention Policies", "Dedicated Support" }
                    }
                }
            }
        };
    }

    public async Task GenerateDataAsync()
    {
        _logger.LogInformation("Starting Stripe data generation for connected account: {AccountId}", _connectedAccountId);

        try
        {
            // Step 0: Clean up existing data
            await CleanupExistingDataAsync();

            // Step 1: Create products and prices
            await CreateProductsAndPricesAsync();

            // Step 2: Generate customers
            var customers = await GenerateCustomersAsync();

            // Step 3: Generate subscriptions
            await GenerateSubscriptionsAsync(customers);

            _logger.LogInformation("Data generation completed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data generation");
            throw;
        }
    }

    private async Task CleanupExistingDataAsync()
    {
        _logger.LogInformation("Cleaning up existing data...");

        try
        {
            // Create request options for connected account
            var requestOptions = new RequestOptions { StripeAccount = _connectedAccountId };

            // Delete all subscriptions
            var subscriptions = await new SubscriptionService().ListAsync(
                new SubscriptionListOptions { Limit = 1000 },
                requestOptions
            );

            _logger.LogInformation("Found {Count} subscriptions to delete", subscriptions.Data.Count);
            var deletedSubscriptions = 0;

            foreach (var subscription in subscriptions.Data)
            {
                try
                {
                    await new SubscriptionService().CancelAsync(subscription.Id, new SubscriptionCancelOptions(), requestOptions);
                    deletedSubscriptions++;

                    if (deletedSubscriptions % 10 == 0)
                    {
                        _logger.LogInformation("Deleted {Count} subscriptions...", deletedSubscriptions);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to delete subscription {SubscriptionId}: {Error}", subscription.Id, ex.Message);
                }
            }

            _logger.LogInformation("Successfully deleted {Count} subscriptions", deletedSubscriptions);

            // Delete all customers
            var customers = await new CustomerService().ListAsync(
                new CustomerListOptions { Limit = 1000 },
                requestOptions
            );

            _logger.LogInformation("Found {Count} customers to delete", customers.Data.Count);
            var deletedCustomers = 0;

            foreach (var customer in customers.Data)
            {
                try
                {
                    await new CustomerService().DeleteAsync(customer.Id, new CustomerDeleteOptions(), requestOptions);
                    deletedCustomers++;

                    if (deletedCustomers % 10 == 0)
                    {
                        _logger.LogInformation("Deleted {Count} customers...", deletedCustomers);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to delete customer {CustomerId}: {Error}", customer.Id, ex.Message);
                }
            }

            _logger.LogInformation("Successfully deleted {Count} customers", deletedCustomers);

            // Deactivate all prices first (required before product deletion)
            var prices = await new PriceService().ListAsync(
                new PriceListOptions { Active = true, Limit = 1000 },
                requestOptions
            );

            _logger.LogInformation("Found {Count} prices to deactivate", prices.Data.Count);
            var deactivatedPrices = 0;

            foreach (var price in prices.Data)
            {
                try
                {
                    var priceUpdateOptions = new PriceUpdateOptions { Active = false };
                    await new PriceService().UpdateAsync(price.Id, priceUpdateOptions, requestOptions);
                    deactivatedPrices++;

                    if (deactivatedPrices % 10 == 0)
                    {
                        _logger.LogInformation("Deactivated {Count} prices...", deactivatedPrices);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to deactivate price {PriceId}: {Error}", price.Id, ex.Message);
                }
            }

            _logger.LogInformation("Successfully deactivated {Count} prices", deactivatedPrices);

            // Now delete all products (prices are deactivated, so products can be deleted)
            var products = await new ProductService().ListAsync(
                new ProductListOptions { Limit = 1000 },
                requestOptions
            );

            _logger.LogInformation("Found {Count} products to delete", products.Data.Count);
            var deletedProducts = 0;

            foreach (var product in products.Data)
            {
                try
                {
                    await new ProductService().DeleteAsync(product.Id, new ProductDeleteOptions(), requestOptions);
                    deletedProducts++;

                    if (deletedProducts % 5 == 0)
                    {
                        _logger.LogInformation("Deleted {Count} products...", deletedProducts);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to delete product {ProductId}: {Error}", product.Id, ex.Message);
                }
            }

            _logger.LogInformation("Successfully deleted {Count} products", deletedProducts);
            _logger.LogInformation("Cleanup completed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup");
            throw;
        }
    }

    private async Task CreateProductsAndPricesAsync()
    {
        _logger.LogInformation("Creating products and prices...");

        var requestOptions = new RequestOptions { StripeAccount = _connectedAccountId };

        foreach (var product in _products)
        {
            // Create product
            var productOptions = new ProductCreateOptions
            {
                Name = product.Name,
                Description = product.Description,
                Metadata = new Dictionary<string, string>
                {
                    { "category", product.Category },
                    { "product_id", product.Id }
                }
            };

            var stripeProduct = await new ProductService().CreateAsync(productOptions, requestOptions);
            _logger.LogInformation("Created product: {ProductName} with ID: {ProductId}", product.Name, stripeProduct.Id);

            // Create prices for each tier
            foreach (var tier in product.Tiers)
            {
                var faker = new Bogus.Faker();
                var historicalDate = faker.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now);

                // Monthly price
                var monthlyPriceOptions = new PriceCreateOptions
                {
                    Product = stripeProduct.Id,
                    UnitAmount = (long)(tier.MonthlyPrice * 100), // Convert to cents
                    Currency = "usd",
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = "month"
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "tier", tier.Name },
                        { "billing_cycle", "monthly" },
                        { "user_limit", tier.UserLimit.ToString() },
                        { "created_date", historicalDate.ToString("yyyy-MM-dd") }
                    }
                };

                var monthlyPrice = await new PriceService().CreateAsync(monthlyPriceOptions, requestOptions);
                tier.StripePriceId = monthlyPrice.Id;

                _logger.LogInformation("Created monthly price for {Tier}: ${Price}/month", tier.Name, tier.MonthlyPrice);

                // Yearly price
                var yearlyPriceOptions = new PriceCreateOptions
                {
                    Product = stripeProduct.Id,
                    UnitAmount = (long)(tier.YearlyPrice * 100), // Convert to cents
                    Currency = "usd",
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = "year"
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "tier", tier.Name },
                        { "billing_cycle", "yearly" },
                        { "user_limit", tier.UserLimit.ToString() },
                        { "created_date", historicalDate.ToString("yyyy-MM-dd") }
                    }
                };

                var yearlyPrice = await new PriceService().CreateAsync(yearlyPriceOptions, requestOptions);
                _logger.LogInformation("Created yearly price for {Tier}: ${Price}/year", tier.Name, tier.YearlyPrice);
            }
        }
    }

    private async Task<List<Customer>> GenerateCustomersAsync()
    {
        var customerCount = _configuration.GetValue<int>("DataGeneration:CustomerCount");
        _logger.LogInformation("Generating {CustomerCount} customers...", customerCount);

        var customers = new List<Customer>();
        var faker = new Bogus.Faker();
        var requestOptions = new RequestOptions { StripeAccount = _connectedAccountId };

        for (int i = 0; i < customerCount; i++)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Email = faker.Internet.Email(),
                Name = faker.Name.FullName(),
                Phone = faker.Phone.PhoneNumber("###-###-####"),
                Address = new AddressOptions
                {
                    Line1 = faker.Address.StreetAddress(),
                    City = faker.Address.City(),
                    State = faker.Address.State(),
                    PostalCode = faker.Address.ZipCode(),
                    Country = "US"
                },
                Metadata = new Dictionary<string, string>
                {
                    { "source", "data_generator" },
                    { "created_date", faker.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now).ToString("yyyy-MM-dd") }
                }
            };

            var customer = await new CustomerService().CreateAsync(customerOptions, requestOptions);
            customers.Add(customer);

            // Skip payment method creation for now since it's causing issues
            // Customers will be created without payment methods, subscriptions will use trial periods

            if ((i + 1) % 10 == 0)
            {
                _logger.LogInformation("Created {Count} customers...", i + 1);
            }
        }

        _logger.LogInformation("Successfully created {Count} customers", customers.Count);
        return customers;
    }

    private async Task GenerateSubscriptionsAsync(List<Customer> customers)
    {
        var subscriptionCount = _configuration.GetValue<int>("DataGeneration:SubscriptionCount");
        _logger.LogInformation("Generating {SubscriptionCount} subscriptions...", subscriptionCount);

        var faker = new Bogus.Faker();
        var requestOptions = new RequestOptions { StripeAccount = _connectedAccountId };

        for (int i = 0; i < subscriptionCount; i++)
        {
            try
            {
                var customer = faker.PickRandom(customers);
                var product = faker.PickRandom(_products);
                var tier = faker.PickRandom(product.Tiers);

                var subscriptionOptions = new SubscriptionCreateOptions
                {
                    Customer = customer.Id,
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = tier.StripePriceId
                        }
                    },
                    TrialPeriodDays = 90, // Longer trial period to avoid payment method issues
                    CollectionMethod = "charge_automatically", // Use automatic collection
                    Metadata = new Dictionary<string, string>
                    {
                        { "source", "data_generator" },
                        { "product", product.Name },
                        { "tier", tier.Name },
                        { "created_date", faker.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now).ToString("yyyy-MM-dd") }
                    }
                };

                // Randomly cancel some subscriptions by setting trial end in the future
                if (faker.Random.Bool(0.15f)) // 15% cancellation rate
                {
                    var futureDate = DateTime.Now.AddDays(faker.Random.Int(1, 30)); // 1-30 days in the future
                    subscriptionOptions.TrialEnd = futureDate;
                }

                await new SubscriptionService().CreateAsync(subscriptionOptions, requestOptions);

                if ((i + 1) % 10 == 0)
                {
                    _logger.LogInformation("Created {Count} subscriptions...", i + 1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to create subscription {Index}: {Error}", i, ex.Message);
            }
        }

        _logger.LogInformation("Successfully created {Count} subscriptions", subscriptionCount);
    }
}
