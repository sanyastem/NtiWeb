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
    
    public partial class TemplateGetClaim_Result
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateBeg { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public System.Guid TemplateTypeId { get; set; }
        public Nullable<System.Guid> OwnerFirmId { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RecTimeStamp { get; set; }
    }
}
