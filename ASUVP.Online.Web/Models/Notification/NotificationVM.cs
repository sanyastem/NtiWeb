using System;

namespace ASUVP.Online.Web.Models.Notification
{
    public class NotificationVM
    {
        public Guid Id { get; set; }
        public Guid? UserFromId { get; set; }
        public Guid? UserToID { get; set; }
        public string UserFromName { get; set; }
        public string UserToName { get; set; }
        public Guid? CompanyFromId { get; set; }
        public Guid? CompanyToId { get; set; }
        public string CompanyFromName { get; set; }
        public string CompanyToName { get; set; }
        public string Text { get; set; }
        public string CreatedOn { get; set; }
        public string ViewDate { get; set; }
        public Guid? ObjectId { get; set; }
        public string Url { get; set; }
    }
}