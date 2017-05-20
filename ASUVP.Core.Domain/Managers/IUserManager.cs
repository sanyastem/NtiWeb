using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.Domain.Managers
{
    public interface IUserManager : IEntityManager<User>
    {
        Task Create(User entity, string password, Guid? contactId, IList<Guid> companies, Guid createdBy);
        Task Update(User entity, Guid? contactId, IList<Guid> companies, Guid updatedBy);
    }
}