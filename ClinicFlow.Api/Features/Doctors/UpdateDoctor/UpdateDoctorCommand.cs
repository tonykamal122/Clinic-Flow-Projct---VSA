using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Domain.Entities;
using ClinicFlow.Api.Features.Doctors.UpdateDoctor;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetDoctorById ;

    public record UpdateDoctorCommand(int Id, string Name, string Spec) : IRequest<Result<UpdateDoctorResponse>>;

    public class UpdaeDoctorQueryValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdaeDoctorQueryValidator()
        {
            RuleFor(query => query.Id).GreaterThan(0).WithMessage("Doctor id must be greater than 0");
            RuleFor(query => query.Name).NotEmpty().WithMessage("Doctor name cannot be empty");
            RuleFor(query => query.Spec).NotEmpty().WithMessage("Doctor specification cannot be empty");
        }
    }