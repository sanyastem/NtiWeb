using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASUVP.Online.Web.Models.Agreement
{
    public class AgreementToSave
    {
        public string DocumentId { get; set; }
        public string DateBeg { get; set; }
        public string DateEnd { get; set; }
        public string DateStop { get; set; }
        public string DocDate { get; set; }
        public string TemplateName { get; set; }
        public string CustomerName { get; set; }
        public string PerformerName { get; set; }
        public string DocNumber { get; set; }
        public string CustomerRegNumber { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}