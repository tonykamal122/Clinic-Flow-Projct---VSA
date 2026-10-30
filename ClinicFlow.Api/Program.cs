using System.Reflection;
using ClinicFlow.Api.Common;
using ClinicFlow.Api.Common.Extensions;
using ClinicFlow.Api.Data;
using ClinicFlow.Api.Features.Doctors.CreateDoctor;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace ClinicFlow.Api ;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddProblemDetails();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpoints(typeof(Program).Assembly);
            //db context configs
            builder.Services.AddDbContext<ClinicDbContext>( opt =>
                opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            //validation and mediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });
            builder.Services.AddValidatorsFromAssembly(typeof(CreateDoctorCommand).Assembly);
            
            //Add Global handler
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapEndpoints();

            app.MapScalarApiReference();
            
            app.Run();
            
        }
    }