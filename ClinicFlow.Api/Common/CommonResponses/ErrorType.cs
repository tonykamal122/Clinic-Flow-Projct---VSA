namespace ClinicFlow.Api.Common.CommonResponses ;

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        UnAuthorized = 3,
        Forbidden = 4,
        InvalidCredential = 5
    }