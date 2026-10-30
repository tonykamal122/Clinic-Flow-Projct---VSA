using ClinicFlow.Api.Common.CommonResponses;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.DeleteDoctorById ;

    public record DeleteDoctorCommand(int Id) : IRequest<Result<DeleteDoctorResponse?>>;

    public class DeleteValidator : AbstractValidator<DeleteDoctorCommand>
    {
        public DeleteValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Doctor id must be greater than 0");
        }
    }