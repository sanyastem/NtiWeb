using System;
using System.Collections.Generic;

namespace ASUVP.Core.Web.OData
{
    public class EmployeeOData : BaseOData
    {
        public EmployeeOData()
        {
            Roles = new List<RoleOData>();
        }

        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string ShortName { get; set; }
        public string Note { get; set; }
        public IList<RoleOData> Roles { get; set; }
    }
}