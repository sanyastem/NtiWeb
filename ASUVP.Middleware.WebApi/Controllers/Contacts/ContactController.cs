using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Contacts
{
    public class ContactController : BaseApiController<IContactManager, Contact, ContactDto>
    {
        public ContactController(IContactManager manager, IMapper mapper, IEventLogger logger)
            : base(manager, mapper, logger)
        {
        }
    }
}