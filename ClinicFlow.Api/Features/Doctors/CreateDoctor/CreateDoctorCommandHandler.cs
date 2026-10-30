using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.CreateDoctor ;

    public class CreateDoctorCommandHandler(IValidator<CreateDoctorCommand> validator, ClinicDbContext dbContext)
        : IRequestHandler<CreateDoctorCommand, Result<CreateDoctorResponse>>
    {
        public async Task<Result<CreateDoctorResponse>> Handle(CreateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var validatedData = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validatedData.IsValid)
            {
                var errors = validatedData.Errors
                    .Select(failure => Error.Validation(
                        code: failure.PropertyName,
                        description: failure.ErrorMessage))
                    .ToList();
                return errors;
            }
            var doctor = new Doctor() { DoctorName = request.Name, DoctorSpecialization = request.Specialization };
            await dbContext.Doctors.AddAsync(
                doctor,
                cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new CreateDoctorResponse(
                doctor.DoctorId,
                doctor.DoctorName,
                doctor.DoctorSpecialization);

            return response;
        }
    }