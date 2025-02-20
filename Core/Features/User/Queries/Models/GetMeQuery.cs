using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Features.User.Queries.Results;
using MediatR;

namespace Core.Features.User.Queries.Models
{
    public class GetMeQuery : IRequest<Response<GetMeResults>>
    {
    }
}
