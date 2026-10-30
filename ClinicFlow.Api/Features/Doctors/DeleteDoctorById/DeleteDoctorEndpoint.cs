using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
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

    public record DeleteDoctorResponse(string msg);

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

    public class DeleteDoctorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/doctors/{id:int}", async (int id, CancellationToken token, ISender sender) =>
            {
                var res = await sender.Send(new DeleteDoctorCommand(id), token);
                return res.Value;
            });
        } 
    }