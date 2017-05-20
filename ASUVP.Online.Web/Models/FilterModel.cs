namespace ASUVP.Online.Web.Models
{
    public class FilterModel
    {
        public string TemplateId { get; set; }
        public string AgrManagerId { get; set; }
        public bool OnlyActive { get; set; }
        public string EPStatusId { get; set; }
        public string StatusId { get; set; }
        public string InstructionTemplateId { get; set; }
        public string ClientId { get; set; }
        public string Agreement { get; set; }
        public string Station { get; set; }
        public string ReportPeriod { get; set; }


        public string DocPeriodType { get; set; }
        public string DocDateBeg { get; set; }
        public string DocDateEnd { get; set; }


        public string ShipmentType { get; set; }
        public string ShipmentBeg { get; set; }
        public string ShipmentEnd { get; set; }

        public string UsePeriod { get; set; }
        public string UseDateBeg { get; set; }
        public string UseDateEnd { get; set; }


    }
}