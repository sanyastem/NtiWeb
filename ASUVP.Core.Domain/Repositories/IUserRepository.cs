using System;
using ASUVP.Core.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace ASUVP.Core.Domain.Repositories
{
    public interface IUserRepository : IRepository, IUserPasswordStore<User, Guid>, IUserSecurityStampStore<User, Guid>
    {
    }
}