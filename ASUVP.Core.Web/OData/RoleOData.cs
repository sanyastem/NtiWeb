using System.Collections.Generic;

namespace ASUVP.Core.Web.OData
{
    public class RoleOData : BaseOData
    {
        public RoleOData()
        {
            Permissions = new List<PermissionOData>();
        }

        public string Name { get; set; }
        public IList<PermissionOData> Permissions { get; set; }
    }
}