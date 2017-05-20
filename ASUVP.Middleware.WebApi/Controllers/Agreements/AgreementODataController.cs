using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Agreements
{
    public class AgreementODataController : BaseODataController<IAgreementManager, Agreement, AgreementOData>
    {
        public AgreementODataController(IAgreementManager manager, IMapper mapper) : base(manager, mapper)
        {
        }
    }
}