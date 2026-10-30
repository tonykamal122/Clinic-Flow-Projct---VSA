using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Features.Doctors.GetDoctorById;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.UpdateDoctor ;

    public class UpdateDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("/doctor/",
                async (
                    UpdateDoctorCommand request,
                    CancellationToken cancellationToken,
                    ISender sender) =>
                {
                    var command = new UpdateDoctorCommand(
                        request.Id,
                        request.Name,
                        request.Spec);

                    var res = await sender.Send(command, cancellationToken);

                    if (res.IsSuccess)
                        return Results.Ok(res.Value);

                    return Results.Problem(
                        title: "Failed to update doctor",
                        detail: string.Join(", ",
                            res.Errors.Select(e => e.Description)),
                        statusCode: 404);
                });
        }
    }