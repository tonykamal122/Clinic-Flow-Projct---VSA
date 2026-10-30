using ClinicFlow.Api.Common;
using ClinicFlow.Api.Common.CommonResponses;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.CreateDoctor ;

    public record CreateDoctorCommand(string Name, string Specialization) : IRequest<Result<CreateDoctorResponse>>;

    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Specialization).NotEmpty().WithMessage("Specialization is required");
        }
    }