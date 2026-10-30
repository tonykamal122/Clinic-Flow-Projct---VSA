using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.CreateDoctor ;

    public record CreateDoctorCommand(string Name, string Specialization) : IRequest<Result<CreateDoctorResponse>>;

    public record CreateDoctorResponse(int Id, string Name, string Specialization);

    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Specialization).NotEmpty().WithMessage("Specialization is required");
        }
    }


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

    public class CreateDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/doctors",
                async (CreateDoctorCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(command, cancellationToken);
                    return response.IsSuccess
                        ? Results.Created($"/doctors/{response.Value.Id}", response.Value)
                        : Results.BadRequest(response.Errors);
                });
        }
    }