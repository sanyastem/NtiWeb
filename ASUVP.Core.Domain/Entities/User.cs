using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace ASUVP.Core.Domain.Entities
{
    public class User : Entity, IUser<Guid>
    {
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public string UserName { get; set; }
        public bool HasContact() => Contact != null;
        public bool IsInactive() => !IsActive || IsDeleted;

        public override void IsDeletedBy(Guid deletedBy)
        {
            if (Employees != null && !Employees.Any())
            {
                foreach (var employee in Employees)
                {
                    employee.IsDeletedBy(deletedBy);
                }
            }

            IsActive = false;
            base.IsDeletedBy(deletedBy);
        }

        public List<Company> UserCompanies()
        {
            var list = new List<Company>();

            if (Employees == null || !Employees.Any()) return list;

            list.AddRange(Employees.Where(e => !e.IsDeleted).Select(x => x.Company));
            return list;
        }

        public string FullName()
        {
            return Contact != null ? $"{Contact.LastName} {Contact.FirstName} {Contact.MiddleName}" : string.Empty;
        }

        public string CompanyName(Guid companyId)
        {
            if (Employees == null || !Employees.Any()) return string.Empty;

            var company = Employees.Select(e => e.Company).FirstOrDefault(c => c.Id == companyId);
            return company?.ShortName;
        }

        public List<string> EmployeePermissions(Guid companyId)
        {
            var list = new List<string>();

            var employee = Employees.FirstOrDefault(e => e.CompanyId == companyId && e.UserId == Id);
            if (employee == null) return list;

            var employeeRoles = employee.EmployeeRoles.Where(er => !er.IsDeleted).Select(e => e.Role).ToList();

            foreach (
                var permissions in
                    employeeRoles.Select(role => role.RolePermissions.Select(e => e.Permission).Distinct()))
            {
                list.AddRange(permissions.Select(permission => permission.Code2));
            }

            return list;
        }

        public void AddEmployees(IList<Guid> companies, Guid createdBy)
        {
            var userEmployees = companies.Select(companyId => new Employee
            {
                UserId = Id,
                CompanyId = companyId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            });

            Employees = userEmployees.ToList();
        }
    }
}