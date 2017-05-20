using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    internal class UserEmployeeManager : Disposable, IUserEmployeeManager
    {
        private readonly IRepository _repository;

        public UserEmployeeManager(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateRange(Guid userId, IList<Guid> companyIds, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(userId);
            Contract.RequiresParameterNotNull(companyIds);
            Contract.RequiresParameterNotNull(createdBy);

            var list = companyIds.Select(companyId => new Employee
            {
                UserId = userId,
                CompanyId = companyId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            });

            _repository.AddRange(list);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteRange(Guid userId, IList<Guid> companyIds)
        {
            Contract.RequiresParameterNotNull(userId);
            Contract.RequiresParameterNotNull(companyIds);

            var list =
                await _repository.FindAsync<Employee>(e => e.UserId == userId && companyIds.Contains(e.CompanyId));

            _repository.RemoveRange(list);
            await _repository.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            _repository?.Dispose();
        }
    }
}