using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Api.Features.Doctors.DeleteAllDoctors ;

    public record DeleteAllDoctorsCommand() : IRequest<Unit>;

    // public record DeleteAllDoctorsResponse();

    public class DeleteAllDoctorsCommandHandler(ClinicDbContext dbContext) :
        IRequestHandler<DeleteAllDoctorsCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAllDoctorsCommand request,
            CancellationToken cancellationToken)
        {
            var doctors = await dbContext.Doctors.ToListAsync(cancellationToken: cancellationToken);

            if (doctors.Count == 0)
                return Unit.Value;

            dbContext.Doctors.RemoveRange(doctors);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class DeleteAllDoctorsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/doctors", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new DeleteAllDoctorsCommand(),
                    cancellationToken);

                return Results.NoContent();
            });
        }
    }