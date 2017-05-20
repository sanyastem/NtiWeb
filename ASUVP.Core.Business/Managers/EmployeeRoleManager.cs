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
    public class EmployeeRoleManager : Disposable, IEmployeeRoleManager
    {
        private readonly IRepository _repository;

        public EmployeeRoleManager(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateRange(Guid employeeId, IList<Guid> roleIds, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(employeeId);
            Contract.RequiresParameterNotNull(roleIds);
            Contract.RequiresParameterNotNull(createdBy);

            var list = roleIds.Select(roleId => new EmployeeRole
            {
                EmployeeId = employeeId,
                RoleId = roleId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            });

            _repository.AddRange(list);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteRange(Guid employeeId, IList<Guid> roleIds)
        {
            Contract.RequiresParameterNotNull(employeeId);
            Contract.RequiresParameterNotNull(roleIds);

            var list =
                await _repository.FindAsync<EmployeeRole>(e => e.EmployeeId == employeeId && roleIds.Contains(e.RoleId));

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