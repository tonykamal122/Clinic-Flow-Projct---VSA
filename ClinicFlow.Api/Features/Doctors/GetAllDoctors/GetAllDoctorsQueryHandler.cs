using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Api.Features.Doctors.GetAllDoctors ;

    public class GetAllDoctorsQueryHandler(ClinicDbContext _dbContext) : IRequestHandler<GetAllDoctorsQuery,Result<List<GetAllDoctorsResponse>>>
    {
        public async Task<Result<List<GetAllDoctorsResponse>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctor =
                await _dbContext.Doctors.Select(
                    it => new GetAllDoctorsResponse(it.DoctorId, it.DoctorName, it.DoctorSpecialization)).ToListAsync();
            return  doctor;
        }
    }