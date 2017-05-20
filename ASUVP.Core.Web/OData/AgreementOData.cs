using System;

namespace ASUVP.Core.Web.OData
{
    public class AgreementOData : BaseOData
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerCompany { get; set; }
        public Guid? CustomerCompanyId { get; set; }
        public string TargetCompany { get; set; }
        public Guid? TargetCompanyId { get; set; }
    }
}