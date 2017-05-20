using System;

namespace ASUVP.Online.Web.Models
{
    public class NewMailVM
    {
        public Guid CompanyId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}