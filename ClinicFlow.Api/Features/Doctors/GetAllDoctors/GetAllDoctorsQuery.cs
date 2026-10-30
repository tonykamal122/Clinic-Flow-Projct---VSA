using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Domain.Entities;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetAllDoctors ;

    public record GetAllDoctorsQuery() : IRequest<Result<List<GetAllDoctorsResponse>>>;