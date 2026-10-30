using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Features.Doctors.GetDoctorById;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.UpdateDoctor ;


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

    public record UpdateDoctorResponse(string Name, string Spec);

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
    public class UpdateDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("/doctors/",
                async (
                    UpdateDoctorCommand request,
                    CancellationToken cancellationToken,
                    ISender sender) =>
                {
                    var command = new UpdateDoctorCommand(
                        request.Id,
                        request.Name,
                        request.Spec);

                    var res = await sender.Send(command, cancellationToken);

                    if (res.IsSuccess)
                        return Results.Ok(res.Value);

                    return Results.Problem(
                        title: "Failed to update doctor",
                        detail: string.Join(", ",
                            res.Errors.Select(e => e.Description)),
                        statusCode: 404);
                });
        }
    }