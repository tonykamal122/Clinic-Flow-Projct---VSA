using ClinicFlow.Api.Common.Abstractions;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.CreateDoctor ;

    public class CreateDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/createdoctor",
                async (CreateDoctorCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(command, cancellationToken);
                    return response.IsSuccess
                        ? Results.Created($"/doctors/{response.Value.Id}", response.Value)
                        : Results.BadRequest(response.Errors);
                });
        }
    }