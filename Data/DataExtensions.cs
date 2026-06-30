using Application.Abstractions;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExtensions
{
    public static IServiceCollection AddContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dbHost = configuration["DB_HOST"] ?? throw new InvalidOperationException("env-parameter DB_HOST is required");
        var dbPort = configuration["DB_PORT"] ?? throw new InvalidOperationException("env-parameter DB_PORT is required");
        var dbName = configuration["DB_NAME"] ?? throw new InvalidOperationException("env-parameter DB_NAME is required");
        var dbUser = configuration["DB_USER"] ?? throw new InvalidOperationException("env-parameter DB_USER is required");
        var dbPassword = configuration["DB_PASSWORD"] ?? throw new InvalidOperationException("env-parameter DB_PASSWORD is required");
        
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