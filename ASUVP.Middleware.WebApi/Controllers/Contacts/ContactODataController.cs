using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Contacts
{
    public class ContactODataController : BaseODataController<IContactManager, Contact, ContactOData>
    {
        public ContactODataController(IContactManager manager, IMapper mapper) : base(manager, mapper)
        {
        }
    }
}