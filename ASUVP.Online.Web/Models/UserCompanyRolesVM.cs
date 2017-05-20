using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models
{
    public class UserCompanyRolesVM
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public List<Guid?> Roles { get; set; }
        public List<Role> CompanyRoles { get; set; }
        public UserCompanyRolesVM() { }

        public static UserCompanyRolesVM GetUserCompanyRolesVM(Guid? companyId, Guid? userId)
        {
            using (var context = new ProcData())
            {
                UserCompanyRolesVM result = new UserCompanyRolesVM();

                if (companyId.HasValue && userId.HasValue)
                {
                    result.CompanyId = companyId.Value;
                    result.UserId = userId.Value;
                    result.CompanyRoles = context.RoleGet(null).ToList();
                }

                return result;
            }
        }
    }
}