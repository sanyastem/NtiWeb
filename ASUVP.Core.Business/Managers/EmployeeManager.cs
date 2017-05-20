using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;
using ASUVP.Core.Exceptions;

namespace ASUVP.Core.Business.Managers
{
    public class EmployeeManager : EntityManager<Employee>, IEmployeeManager
    {
        private readonly IEmployeeRoleManager _manager;

        public EmployeeManager(IRepository repository, IEmployeeRoleManager manager) : base(repository)
        {
            _manager = manager;
        }

        public IQueryable<Role> GetEmployeeRoles(Guid employeeId)
        {
            var role = Repository.GetById<Employee>(employeeId);
            return role?.EmployeeRoles.Select(e => e.Role).AsQueryable();
        }

        public async Task Update(Employee employee, IList<Guid> roles, Guid updatedBy)
        {
            Contract.RequiresParameterNotNull(employee);
            Contract.RequiresParameterNotNull(roles);
            Contract.RequiresParameterNotNull(updatedBy);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = await Repository.GetByIdAsync<Employee>(employee.Id);
                if (entity == null) throw new EntityNotFoundException();

                entity.IsUpdatedBy(updatedBy); // todo: do we really need it?
                await Repository.SaveChangesAsync();

                var employeeRoles = entity.EmployeeRoles.Select(e => e.RoleId).ToList();
                var rolesToAdd = roles.Except(employeeRoles).ToList();
                var rolesToRemove = employeeRoles.Except(roles).ToList();

                await _manager.DeleteRange(entity.Id, rolesToRemove);
                await _manager.CreateRange(entity.Id, rolesToAdd, updatedBy);

                scope.Complete();
            }
        }
    }
}