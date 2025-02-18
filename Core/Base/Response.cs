using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public class Response<T>
    {
        #region Fields
        public HttpStatusCode? StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public object? Meta { get; set; }

        public bool? Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
        #endregion

        #region Constructors
        public Response()
        {
            
        }

        public Response(T data, string message)
        {
            Data = data;
            Message = message;
        }

        public Response(string message)
        {
            Message = message;
        }

        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
        #endregion

        #region Methods
        // Method to return non-null fields as a dictionary
        public Dictionary<string, object?> ToNonNullDictionary()
        {
            var nonNullFields = new Dictionary<string, object?>();

            if (StatusCode != null)
                nonNullFields.Add(nameof(StatusCode), StatusCode);
            else nonNullFields.Add(nameof(StatusCode), 500);
            if (Meta != null)
                nonNullFields.Add(nameof(Meta), Meta);
            if (Succeeded != null)
                nonNullFields.Add(nameof(Succeeded), Succeeded);
            if (Message != null)
                nonNullFields.Add(nameof(Message), Message);
            if (Errors != null && Errors.Count > 0)
                nonNullFields.Add(nameof(Errors), Errors);
            if (Data != null)
                nonNullFields.Add(nameof(Data), Data);

            return nonNullFields;
        }
        #endregion
    }
}