using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync().ConfigureAwait(false);

        await SeedAsync(context).ConfigureAwait(false);
    }
    
    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context).ConfigureAwait(false);
        await SeedProductAsync(context).ConfigureAwait(false);
        await SeedOrdersWithItemsAsync(context).ConfigureAwait(false);
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync().ConfigureAwait(false))
        {
            await context.Customers.AddRangeAsync(InitialData.Customers).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    private static async Task SeedProductAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync().ConfigureAwait(false))
        {
            await context.Products.AddRangeAsync(InitialData.Products).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync().ConfigureAwait(false))
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
