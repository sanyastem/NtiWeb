//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASUVP.Core.DataAccess.Model
{
    using System;
    
    public partial class Agreement
    {
        public System.Guid DocumentId { get; set; }
        public System.Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string DocNumber { get; set; }
        public string RegNumber { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public Guid? StatusId { get; set; }
        public string StatusName { get; set; }
        public string DocName { get; set; }
        public Guid? CustomerCompanyId { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyLabel { get; set; }
        public Guid? PerformerCompanyId { get; set; }
        public string PerformerCompanyName { get; set; }
        public string PerformerCompanyLabel { get; set; }
        public Guid? CustomerBankId { get; set; }
        public string CustomerBankName { get; set; }
        public Guid? PerformerBankId { get; set; }
        public string PerformerBankName { get; set; }
        public Guid? CustomerAddressId { get; set; }
        public string CustomerAddressName { get; set; }
        public Guid? PerformerAddressId { get; set; }
        public string PerformerAddressName { get; set; }
        public Guid? CustomerSignerId { get; set; }
        public string CustomerSignerName { get; set; }
        public Guid? PerformerSignerId { get; set; }
        public string PerformerSignerName { get; set; }
        public Nullable<System.DateTime> DateBeg { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateStop { get; set; }
        public Nullable<int> CustomerManagerId { get; set; }
        public string CustomerManagerName { get; set; }
        public Nullable<int> PerformerManagerId { get; set; }
        public string PerformerManagerName { get; set; }
        public string Note { get; set; }
        public System.Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> IRSPerevozkiId { get; set; }
    }
}
