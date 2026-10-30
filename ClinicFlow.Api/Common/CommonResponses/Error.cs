namespace ClinicFlow.Api.Common.CommonResponses ;

    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType ErrorType { get; }

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        public static Error Failure(
            string code = "General.Failure",
            string description = "A General Failure Has Occurred.")
            => new(code, description, ErrorType.Failure);

        public static Error Validation(
            string code = "General.Validation",
            string description = "A Validation Error Has Occurred.")
            => new(code, description, ErrorType.Validation);

        public static Error NotFound(
            string code = "General.NotFound",
            string description = "The Requested Resource Was Not Found.")
            => new(code, description, ErrorType.NotFound);

        public static Error UnAuthorized(
            string code = "General.UnAuthorized",
            string description = "You Are Not Authorized To Access This Resource.")
            => new(code, description, ErrorType.UnAuthorized);

        public static Error Forbidden(
            string code = "General.Forbidden",
            string description = "You Do Not Have Permission To Access This Resource.")
            => new(code, description, ErrorType.Forbidden);

        public static Error InvalidCredential(
            string code = "General.InvalidCredential",
            string description = "The Provided Credentials Are Invalid.")
            => new(code, description, ErrorType.InvalidCredential);

        public Task ExecuteAsync(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }