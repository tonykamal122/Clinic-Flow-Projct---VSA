using ClinicFlow.Api.Common.Abstractions;
using ClinicFlow.Api.Common.CommonResponses;
using ClinicFlow.Api.Data;
using FluentValidation;
using MediatR;

namespace ClinicFlow.Api.Features.Doctors.GetDoctorById ;

    public record GetDoctorByIdQuery(int Id) : IRequest<Result<GetDoctorByIdResponse>>;

    public class GetDoctorByIdQueryValidator : AbstractValidator<GetDoctorByIdQuery>
    {
        public GetDoctorByIdQueryValidator()
        {
            RuleFor(query => query.Id).GreaterThan(0).WithMessage("Doctor id must be greater than 0");
        }
    }

    public record GetDoctorByIdResponse(int Id, string Name, string Spec);

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
            var doctor = dbContext.Doctors.FirstOrDefault(it => it.DoctorId == request.Id);
            if (doctor is null)
            {
                return Error.NotFound(
                    code: "Doctor.NotFound",
                    description: $"Doctor not found with id {request.Id}"
                    );
            }
            return new GetDoctorByIdResponse(doctor.DoctorId, doctor.DoctorName, doctor.DoctorSpecialization);
        }
    }

    public class GetDoctorByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/doctors/{id:int}", async (int id, CancellationToken cancellationToken, ISender sender) =>
            {
                var res = await sender.Send(new GetDoctorByIdQuery(id), cancellationToken);

                if (res.IsSuccess)
                    return Results.Ok(res.Value);

                return Results.Problem(
                    title: "Failed to get doctor",
                    detail: string.Join(", ", res.Errors.Select(e => e.Description)),
                    statusCode: 404);
            });
        }
    }