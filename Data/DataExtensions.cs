using Application.Abstractions;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExtensions
{
    public static IServiceCollection AddContext(this IServiceCollection serviceCollection)
    {
        var dbHost = "localhost";
        var dbPort = "5432";
        var dbName = "Nodes";
        var dbUser = "postgres";
        var dbPassword = "123456";
        
        serviceCollection.AddDbContext<AppDbContext>(x =>
        {
            x.UseNpgsql($"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}");
        });

        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INodeRepository, NodeRepository>();
        serviceCollection.AddScoped<INodeGroupRepository, NodeGroupRepository>();

        return serviceCollection;
    }
}