using ClinicFlow.Api.Common.Abstractions;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetDoctorById ;

    public class GetDoctorByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/doctor/{id:int}", async (int id, CancellationToken cancellationToken , ISender sender) =>
            {
                var res = await sender.Send(new GetDoctorByIdQuery(id), cancellationToken);

                if (res.IsSuccess)
                    return Results.Ok(res.Value);

                return Results.Problem(
                    title: "Failed to get doctor",
                    detail: string.Join(", ", res.Errors.Select(e => e.Description)),
                    statusCode: 404);
            });
        }
    }