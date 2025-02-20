using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;
using Infrastructure.Base;

namespace Infrastructure.Interfaces
{
    public interface IRefreshTokensRepository: IGenericRepositoryAsync<UserRefreshTokens>
    {
    }
}
