//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASUVP.Core.DataAccess.Model
{
    using System;
    
    public partial class ProtocolDetails
    {
        public System.Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string MeasureName { get; set; }
        public string NDS { get; set; }
        public decimal Summa { get; set; }
        public decimal SummaNDS { get; set; }
        public decimal SummaWithNDS { get; set; }
        public string CurrencyName { get; set; }
        public string CountryName { get; set; }
        public string StationFromName { get; set; }
        public string StationToName { get; set; }
        public string FreightName { get; set; }
        public string GroupTypeName { get; set; }
        public string CarParkName { get; set; }
    }
}
