using ClinicFlow.Api.Common.Abstractions;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetAllDoctors ;

    public class GetAllDoctorsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/doctors", async (ISender sender,CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetAllDoctorsQuery(),
                    cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            });
        }
    }