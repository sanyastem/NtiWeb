using System.Web.OData.Builder;
using ASUVP.Core.Web.OData;
using Microsoft.OData.Edm;

namespace ASUVP.Middleware.WebApi
{
    public static class EdmModelBuilder
    {
        public static IEdmModel Build()
        {
            var builder = new ODataConventionModelBuilder {Namespace = "OData"};

            builder.EntitySet<UserOData>("UserOData");
            builder.EntitySet<RoleOData>("RoleOData");
            builder.EntitySet<PermissionOData>("PermissionOData");
            builder.EntitySet<AgreementOData>("AgreementOData");
            builder.EntitySet<ContactOData>("ContactOData");
            builder.EntitySet<CompanyOData>("CompanyOData");
            builder.EntitySet<EmployeeOData>("EmployeeOData");

            foreach (var entity in builder.EntitySets)
            {
                entity.EntityType.Namespace = "OData.Models";
            }

            return builder.GetEdmModel();
        }
    }
}