using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetDoctorById ;

    public class GetDoctorByIdHandler(IValidator<GetDoctorByIdQuery> validator, ClinicDbContext dbContext) :
        IRequestHandler<GetDoctorByIdQuery, Result<GetDoctorByIdResponse>>
    {
        public async Task<Result<GetDoctorByIdResponse>> Handle(GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            var validatedData = await validator.ValidateAsync(new GetDoctorByIdQuery(request.Id));
            if (!validatedData.IsValid)
            {
                return validatedData.Errors.Select(it => Error.Validation(code: it.PropertyName,
                    description: it.ErrorMessage
                    )).ToList();
            }
            var doctor = dbContext.Doctors.FirstOrDefault(it => it.DoctorId == request.Id); if (doctor is null)
            {
                return Error.NotFound(
                    code: "Doctor.NotFound",
                    description: $"Doctor not found with id {request.Id}"
                    );
            }
            return new GetDoctorByIdResponse(doctor.DoctorId, doctor.DoctorName, doctor.DoctorSpecialization);
        }
    }