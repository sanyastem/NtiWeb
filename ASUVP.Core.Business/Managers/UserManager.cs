using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ASUVP.Core.Business.Identity;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;
using ASUVP.Core.Exceptions;

namespace ASUVP.Core.Business.Managers
{
    public class UserManager : EntityManager<User>, IUserManager
    {
        private readonly IdentityManager _identity;
        private readonly IUserEmployeeManager _manager;

        public UserManager(IRepository repository, IUserEmployeeManager manager, IdentityManager identity)
            : base(repository)
        {
            _manager = manager;
            _identity = identity;
        }

        public async Task Create(User user, string password, Guid? contactId, IList<Guid> companiesIds, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(user);
            //Contract.RequiresParameterNotNull(companiesIds);
            Contract.RequiresParameterNotNull(createdBy);
            //Contract.RequiresNotEmptyString(password); // todo: what about windows users

            var entity = Repository.Find<User>(e => e.UserName.ToUpper() == user.UserName.ToUpper()).FirstOrDefault();
            if (entity != null) throw new EntityAlreadyExistsException();

            user.Id = Guid.NewGuid();
            user.IsCreatedBy(createdBy);

            if (contactId.HasValue)
            {
                var contact = Repository.GetById<Contact>(contactId.Value);
                if (contact != null) user.Contact = contact;
            }

            if (companiesIds.Any())
            {
                user.AddEmployees(companiesIds, createdBy);
            }

            await _identity.CreateAsync(user, password);
        }

        public async Task Update(User user, Guid? contactId, IList<Guid> companiesIds, Guid updatedBy)
        {
            Contract.RequiresParameterNotNull(user);
            Contract.RequiresParameterNotNull(companiesIds);
            Contract.RequiresParameterNotNull(updatedBy);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity =
                    Repository.Find<User>(e => e.UserName.ToUpper() == user.UserName.ToUpper()).FirstOrDefault();
                if (entity != null && entity.Id != user.Id) throw new EntityAlreadyExistsException();

                entity = await GetByIdAsync(user.Id);
                if (entity == null) throw new EntityNotFoundException();

                entity.UserName = user.UserName;
                entity.IsActive = user.IsActive;
                entity.IsUpdatedBy(updatedBy);

                if (contactId.HasValue)
                {
                    var contact = Repository.GetById<Contact>(contactId.Value);
                    if (contact != null) entity.Contact = contact;
                }

                var userCompanies = entity.Employees.Select(e => e.CompanyId).ToList();
                var employeesToAdd = companiesIds.Except(userCompanies).ToList();
                var employeesToRemove = userCompanies.Except(companiesIds).ToList();

                await _manager.DeleteRange(entity.Id, employeesToRemove);
                await _manager.CreateRange(entity.Id, employeesToAdd, updatedBy);

                scope.Complete();
            }
        }
    }
}