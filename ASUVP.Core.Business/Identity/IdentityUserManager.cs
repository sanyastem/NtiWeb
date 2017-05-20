using System;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Repositories;
using Microsoft.AspNet.Identity;

namespace ASUVP.Core.Business.Identity
{
    public class IdentityManager : UserManager<User, Guid>
    {
        public IdentityManager(IUserRepository repository) : base(repository)
        {
            PasswordValidator = new IdentityPasswordValidator();
            UserValidator = new UserValidator<User, Guid>(this) {AllowOnlyAlphanumericUserNames = false};
        }
    }
}