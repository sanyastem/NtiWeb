namespace ASUVP.Core.Web.OData
{
    public class ContactOData : BaseOData
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
    }
}