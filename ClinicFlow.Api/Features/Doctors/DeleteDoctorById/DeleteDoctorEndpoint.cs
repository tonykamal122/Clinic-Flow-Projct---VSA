using ClinicFlow.Api.Common.Abstractions;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.DeleteDoctorById ;

    public class DeleteDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/delete/{id:int}", async (int id, CancellationToken token, ISender sender) =>
            {
                var res = await sender.Send(new DeleteDoctorCommand(id), token);
                return res.Value;
            });
        } 
    }