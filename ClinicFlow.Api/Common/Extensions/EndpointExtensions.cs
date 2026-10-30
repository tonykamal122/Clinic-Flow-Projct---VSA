using System.Reflection;
using ClinicFlow.Api.Common.Abstractions;

namespace ClinicFlow.Api.Common.Extensions ;

    public static class EndpointExtensions
    {
        public static IServiceCollection AddEndpoints(
            this IServiceCollection services,
            Assembly assembly)
        {
            var endpointTypes = assembly
                .DefinedTypes
                .Where(type =>
                    type is { IsAbstract: false, IsInterface: false } &&
                    typeof(IEndpoint).IsAssignableFrom(type));

            foreach (var type in endpointTypes)
            {
                services.AddTransient(typeof(IEndpoint), type);
            }

            return services;
        }


        public static WebApplication MapEndpoints(
            this WebApplication app)
        {
            var endpoints = app.Services.GetServices<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }

            return app;
        }
    }