using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models
{
    public class MailVM
    {
        public List<UserMailSentList> SentMails { get; set; }
        public List<UserMailReceivedList> ReceivedMails { get; set; }
        public FilterModel Filter { get; set; }
    }
}