using Core.Base;
using MediatR;

namespace Core.Features.Authentication.Queries.Models
{
    public class ConfirmEmailQuery : IRequest<Response<string>>
    {
        public string token { get; set; }
        public int userId { get; set; }
    }
}
