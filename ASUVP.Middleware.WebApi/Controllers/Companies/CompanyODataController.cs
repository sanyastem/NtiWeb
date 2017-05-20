using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Companies
{
    public class CompanyODataController : BaseODataController<ICompanyManager, Company, CompanyOData>
    {
        public CompanyODataController(ICompanyManager manager, IMapper mapper) : base(manager, mapper)
        {
        }
    }
}