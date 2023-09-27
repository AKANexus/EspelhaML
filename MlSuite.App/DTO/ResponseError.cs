using System.Text.Json.Nodes;
using FluentValidation.Results;

namespace MlSuite.App.DTO
{
    public record ResponseError
    {
        public ResponseError(string error, object details, ResponseErrorCode errorCode)
        {
            Error = error;
            Details = details;
            ErrorCode = (int)errorCode;
        }

        public ResponseError(string error, string message, ResponseErrorCode errorCode)
        {
            Error = error;
            Details = message;
            ErrorCode = (int)errorCode;
        }

        public ResponseError(string error, List<ValidationFailure> details, ResponseErrorCode errorCode)
        {
            Error = error;
            Details = new JsonObject();
            int variant = 1;
            foreach (ValidationFailure failure in details)
            {
                if (details.Count(x => x.PropertyName == failure.PropertyName) > 1)
                {
                    ((JsonObject)Details).Add(failure.FormattedMessagePlaceholderValues["PropertyName"] + "_" + variant++, failure.ErrorMessage);
                }
                else
                {
                    variant = 1;
                    ((JsonObject)Details).Add(failure.FormattedMessagePlaceholderValues["PropertyName"].ToString() ?? "%", failure.ErrorMessage);

                }
            }
            ErrorCode = (int)errorCode;
        }

        public string Error { get; set; }
        public object Details { get; set; }
        public int ErrorCode { get; set; }
    }

    public enum ResponseErrorCode
    {
        Duplicate = 1,
        ValidationFailure,
        NullBody,
        InvalidFilterArgument,
        Revoked,
        Unauthorised,
        MissingTenant,
        EntryNotFound,
        [Obsolete("Try and use a more specific error code, when possible.")]
        Generic = 999,
        MissingArgument
    }
}
