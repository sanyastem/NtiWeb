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
    
    public partial class UserNotificationList
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> UserFromId { get; set; }
        public Nullable<System.Guid> UserToID { get; set; }
        public string UserFromName { get; set; }
        public string UserToName { get; set; }
        public Nullable<System.Guid> CompanyFromId { get; set; }
        public Nullable<System.Guid> CompanyToId { get; set; }
        public string CompanyFromName { get; set; }
        public string CompanyToName { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ViewDate { get; set; }
        public Nullable<System.Guid> ObjectId { get; set; }
    }
}
