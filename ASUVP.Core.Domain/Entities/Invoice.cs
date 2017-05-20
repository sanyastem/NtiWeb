using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASUVP.Core.Domain.Entities
{
    public class Account : Entity
    {
        public string Name { get; set; }
        public string ClientName { get; set; }
        public int? Number { get; set; }
        public string AgreementName { get; set; }
        public string Request { get; set; }
        public DateTime? AccountDate { get; set; }
        public string PerformerName { get; set; }
        public string EmployeeName { get; set; }
        public string SignatoryFirst { get; set; }
        public string SignatorySecond { get; set; }
        public bool? ElectronicSignature { get; set; }
        public byte? ProtocolStatus { get; set; }
        public byte? RequestStatus { get; set; }
        public string CreatorName { get; set; }
        public string ConfirmedName { get; set; }
    }
}
