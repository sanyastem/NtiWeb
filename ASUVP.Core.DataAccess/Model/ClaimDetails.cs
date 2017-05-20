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
    
    public partial class ClaimDetails
    {
        public System.Guid Id { get; set; }
        public string StatusName { get; set; }
        public string DocNumber { get; set; }
        public string RegNumber { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string DocName { get; set; }
        public System.DateTime DateBeg { get; set; }
        public System.DateTime DateEnd { get; set; }
        public string DatePeriod { get; set; }
        public System.Guid AgreementId { get; set; }
        public string AgreementName { get; set; }
        public Guid? StFromId { get; set; }
        public string StFromCode6 { get; set; }
        public string StFromName { get; set; }
        public string RWFromName { get; set; }
        public Guid? StToId { get; set; }
        public string StToCode6 { get; set; }
        public string StToName { get; set; }
        public string RWToName { get; set; }
        public int CarCount { get; set; }
        public decimal FrWeight { get; set; }
        public string FrETSNGCode { get; set; }
        public string FrETSNGName { get; set; }
        public Guid? ContactId { get; set; }
        public string ContactFIO { get; set; }
        public string TelegrammList { get; set; }
        public Guid? ApprovalPerformerImgId { get; set; }
        public Guid? ApprovalCustomerImgId { get; set; }
        public Guid? SigningPerformerImgId { get; set; }
        public Guid? SigningCustomerImgId { get; set; }
        public Nullable<int> ApprovalPerformer { get; set; }
        public Nullable<int> ApprovalCustomer { get; set; }
        public Nullable<int> SigningPerformer { get; set; }
        public Nullable<int> SigningCustomer { get; set; }
    }
}