using ClinicFlow.Api.Features.Doctors.CreateDoctor;
using MediatR;

namespace ClinicFlow.Api.Common.CommonResponses ;

    public class Result
    {
        private List<Error> _errors = [];

        public bool IsSuccess => _errors.Count == 0;

        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors => _errors;

        protected Result()
        {
        }

        protected Result(Error error)
        {
            _errors.Add(error);
        }

        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static Result Ok() => new();

        public static Result Fail(Error error) => new(error);

        public static Result Fail(List<Error> errors) => new(errors);
    }

    public class Result<TValue> : Result 
    {
        private readonly TValue _value;

        public TValue Value => _value;

        // Success result with value
        private Result(TValue value) : base()
        {
            _value = value;
        }

        // Failure result with one error
        private Result(Error error) : base(error)
        {
            _value = default!;
        }

        // Failure result with multiple errors
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        // Success result with value
        public static Result<TValue> Ok(TValue value) => new(value);

        // Failure result with one error
        public static new Result<TValue> Fail(Error error) => new(error);

        // Failure result with multiple errors
        public static new Result<TValue> Fail(List<Error> errors) => new(errors);


        // Implicit conversion operators

        // Remember:
        // Result<string> res = new Result<string>("100");
        // Result<string> res = "100";

        public static implicit operator Result<TValue>(TValue value)
            => Ok(value);

        public static implicit operator Result<TValue>(Error error)
            => Fail(error);

        public static implicit operator Result<TValue>(List<Error> errors)
            => Fail(errors);
    }