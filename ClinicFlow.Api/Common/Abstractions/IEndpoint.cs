namespace ClinicFlow.Api.Common.Abstractions ;

    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }