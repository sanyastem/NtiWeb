using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Agreements
{
    public class AgreementController : BaseApiController<IAgreementManager, Agreement, AgreementDto>
    {
        public AgreementController(IAgreementManager manager, IMapper mapper, IEventLogger logger)
            : base(manager, mapper, logger)
        {
        }
    }
}