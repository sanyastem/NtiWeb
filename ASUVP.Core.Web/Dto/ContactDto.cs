using System;

namespace ASUVP.Core.Web.Dto
{
    public class ContactDto : BaseDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? CompanyId { get; set; }
    }
}