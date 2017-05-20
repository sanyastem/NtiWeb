using System;

namespace ASUVP.Core.Domain.Entities
{
    public class Protocol : Entity
    {
        public string ProtocolNumber { get; set; }
        public DateTime? ProtocolDate { get; set; }
        public string ProtocolName { get; set; }
        public string AgreementName { get; set; }
        public string RequestNumber { get; set; }
        public string Currency { get; set; }
        public decimal? RateWithoutTax { get; set; }
        public string RateUnit { get; set; }
        public decimal? VatPercent { get; set; }
        public string VatNomination { get; set; }
        public decimal? RateWithTax { get; set; }
        public string CargoName { get; set; }
        public string InitialCarrierName { get; set; }
        public string DispatchCarrierName { get; set; }
        public string ClientName { get; set; }
        public string ManagerName { get; set; }
        public byte? RATStatus { get; set; }
        public byte? ClientStatus { get; set; }
        public string SignerName { get; set; }
        public bool? EDSStatus { get; set; }
    }
}
