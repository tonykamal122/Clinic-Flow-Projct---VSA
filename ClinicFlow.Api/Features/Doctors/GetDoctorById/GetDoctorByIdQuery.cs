using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetDoctorById ;

    public record GetDoctorByIdQuery(int Id) : IRequest<Result<GetDoctorByIdResponse>>;

    public class GetDoctorByIdQueryValidator : AbstractValidator<GetDoctorByIdQuery>
    {
        public GetDoctorByIdQueryValidator()
        {
            RuleFor(query => query.Id).GreaterThan(0).WithMessage("Doctor id must be greater than 0");
        }
    }