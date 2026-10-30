using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Features.Doctors.GetDoctorById;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.UpdateDoctor ;

    public class UpdateDoctorHandler(IValidator<UpdateDoctorCommand> validator, ClinicDbContext dbContext) :
        IRequestHandler<UpdateDoctorCommand, Result<UpdateDoctorResponse>>
    {
        public async Task<Result<UpdateDoctorResponse>> Handle(UpdateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var validatedData = await validator.ValidateAsync(new UpdateDoctorCommand(request.Id,request.Name,request.Spec));
            if (!validatedData.IsValid)
            {
                return validatedData.Errors.Select(it => Error.Validation(code: it.PropertyName,
                    description: it.ErrorMessage
                    )).ToList();
            }
            var doctor = dbContext.Doctors.FirstOrDefault(it => it.DoctorId == request.Id);
            if (doctor is null)
            {
                return Error.NotFound(
                    code: "Doctor.NotFound",
                    description: $"Doctor not found with id {request.Id}"
                    );
            }
            doctor.DoctorName = request.Name;
            doctor.DoctorSpecialization = request.Spec;
            dbContext.Doctors.Update(doctor);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateDoctorResponse(doctor.DoctorName, doctor.DoctorSpecialization);
        }
    }