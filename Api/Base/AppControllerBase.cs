using System.Net;
using Asp.Versioning;
using Core.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AppControllerBase : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new InvalidOperationException("IMediator service is unavailable");
    
        public ObjectResult HandleResult<T> (Response<T> result)
        {
            Dictionary<string, object?>? response = result.ToNonNullDictionary();

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                case HttpStatusCode.Forbidden:
                    return new ObjectResult(response) { StatusCode = 403 };
                default:
                    return new BadRequestObjectResult(response);
            }
        }
    }
}
