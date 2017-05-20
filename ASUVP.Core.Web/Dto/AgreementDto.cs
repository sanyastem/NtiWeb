namespace ASUVP.Core.Web.Dto
{
    public class AgreementDto : BaseDto
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CustomerCompany { get; set; }
        public string TargetCompany { get; set; }
    }
}