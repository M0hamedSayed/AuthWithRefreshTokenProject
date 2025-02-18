using System.Net;

namespace Core.Base
{
    public class ResponseHandler
    {
        public ResponseHandler() {}

        #region Methods
        public Response<T> Deleted<T>(string? Message)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = Message is null ? "Deleted Successfully" : Message
            };
        }
        public Response<T> Success<T>(T entity, object? Meta)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Data Returned Successfully",
                Meta = Meta
            };
        }
        public Response<T> Unauthorized<T>(string? Message)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = Message is null ? "You are not Authorized To Access This Resources" : Message
            };
        }
        public Response<T> BadRequest<T>(string? Message)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message is null ? "Request Can't be Understand" : Message
            };
        }

        public Response<T> UnprocessableEntity<T>(string? Message)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = Message is null ? "There are Validation Errors Or Syntax Errors" : Message
            };
        }


        public Response<T> NotFound<T>(string? message)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message is null ? "Not Found" : message
            };
        }

        public Response<T> Created<T>(T entity, object? Meta)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created Successfully",
                Meta = Meta
            };
        }
        #endregion
    }
}
