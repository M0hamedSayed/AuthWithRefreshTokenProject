using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using MediatR;

namespace Core.Features.Admin.Commands.Models
{
    public class RestoreUserQuery : IRequest<Response<string>>
    {
        public RestoreUserQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
