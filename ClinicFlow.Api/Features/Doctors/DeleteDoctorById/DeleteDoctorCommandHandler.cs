using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Api.Features.Doctors.DeleteDoctorById ;

    public class DeleteDoctorCommandHandler(IValidator<DeleteDoctorCommand> validator, ClinicDbContext dbContext) :
        IRequestHandler<DeleteDoctorCommand, Result<DeleteDoctorResponse?>>
    {
        public async Task<Result<DeleteDoctorResponse?>> Handle(DeleteDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var validatedData = await validator.ValidateAsync(request);
            if (!validatedData.IsValid)
            {
               return validatedData.Errors.Select(it => Error.Validation(code: it.PropertyName,
                    description: it.ErrorMessage)).ToList();
            }
            var doctor = dbContext.Doctors.FirstOrDefault(it => it.DoctorId == request.Id);
            if (doctor is not null)
            {
                dbContext.Doctors.Remove(doctor);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new DeleteDoctorResponse("Doctor successfully deleted");
            }
            return new DeleteDoctorResponse($"Doctor not found with id {request.Id}");
        }
    }

   