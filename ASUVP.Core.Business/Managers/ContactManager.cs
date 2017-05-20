using System;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;
using ASUVP.Core.Exceptions;

namespace ASUVP.Core.Business.Managers
{
    public class ContactManager : EntityManager<Contact>, IContactManager
    {
        public ContactManager(IRepository repository) : base(repository)
        {
        }

        public override async Task Update(Contact contact, Guid updatedBy)
        {
            Contract.RequiresParameterNotNull(contact);
            Contract.RequiresParameterNotNull(updatedBy);

            var entity = Repository.GetById<Contact>(contact.Id);
            if (entity == null) throw new EntityNotFoundException();

            entity.FirstName = contact.FirstName;
            entity.LastName = contact.LastName;
            entity.MiddleName = contact.MiddleName;
            entity.Email = contact.Email;
            entity.Phone = contact.Phone;
            entity.CompanyId = contact.CompanyId;
            entity.IsUpdatedBy(updatedBy);

            await Repository.SaveChangesAsync();
        }
    }
}