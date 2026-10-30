using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Api.Features.Doctors.GetAllDoctors ;

    public record GetAllDoctorsQuery() : IRequest<Result<List<GetAllDoctorsResponse>>>;

    public record GetAllDoctorsResponse(int Id, string Name, string Specialization);

    public class GetAllDoctorsQueryHandler(ClinicDbContext _dbContext) :
        IRequestHandler<GetAllDoctorsQuery, Result<List<GetAllDoctorsResponse>>>
    {
        public async Task<Result<List<GetAllDoctorsResponse>>> Handle(GetAllDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            var doctor =
                await _dbContext.Doctors.Select(
                    it => new GetAllDoctorsResponse(it.DoctorId, it.DoctorName, it.DoctorSpecialization)).ToListAsync();
            return doctor;
        }
    }

    public class GetAllDoctorsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/doctors", async (ISender sender, CancellationToken cancellationToken) =>
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