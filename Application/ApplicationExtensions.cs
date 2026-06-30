using Application.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INodeService, NodeService>();
        serviceCollection.AddScoped<INodeGroupService, NodeGroupService>();

        return serviceCollection;
    }

    public static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
    {
        MappingConfig.Configure();
        serviceCollection.AddMapster();
        return serviceCollection;
    }
    
}