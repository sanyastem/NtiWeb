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
    
    public partial class ActList
    {
        public System.Guid Id { get; set; }
        public string DocNumber { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string DocName { get; set; }
        public Nullable<System.DateTime> DateBeg { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<decimal> SummaWithNDS { get; set; }
        public Nullable<decimal> SummaNDS { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<System.Guid> AgreementId { get; set; }
        public string AgreementName { get; set; }
        public string CustomerCompanyName { get; set; }
        public string PerformerCompanyName { get; set; }
        public string AgrManagerName { get; set; }
        public string HasAttachmentImgPath { get; set; }
        public string HasAttachmentHint { get; set; }
        public Nullable<int> ApprovalPerformerImgId { get; set; }
        public Nullable<int> ApprovalCustomerImgId { get; set; }
        public Nullable<int> SigningPerformerImgId { get; set; }
        public Nullable<int> SigningCustomerImgId { get; set; }
        public string TemplateName { get; set; }
    }
}
