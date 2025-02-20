using Core.Base;
using MediatR;

namespace Core.Features.Authentication.Queries.Models
{
    public class ResendConfirmEmailQuery: IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
