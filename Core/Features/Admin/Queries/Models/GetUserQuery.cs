using Core.Base;
using Core.Features.Admin.Queries.Results;
using MediatR;

namespace Core.Features.Admin.Queries.Models
{
    public class GetUserQuery : IRequest<Response<GetUserResult>>
    {
        public GetUserQuery(int id) 
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
