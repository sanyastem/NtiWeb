using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASUVP.Online.Services
{
    public interface IMailService
    {
        List<UserMailSentList> GetSentMails();
        UserMail GetMailDetails(Guid? id);
        void UpdateUserMailViewTime(Guid? id);
        List<UserMailReceivedList> GetReceivedMails();
        void SendMail(string subject, string body, Guid receiverCompanyId);
    }

    public class MailService : IMailService
    {
        public UserMail GetMailDetails(Guid? id)
        {
            using (var db = new ProcData())
            {
                return db.UserMailGet(id).FirstOrDefault();
            }
        }

        public List<UserMailSentList> GetSentMails()
        {
            using (var db = new ProcData())
            {
                return db.UserMailSentListGet(AuthManager.User.CompanyId).ToList();
            }
        }

        public void UpdateUserMailViewTime(Guid? id)
        {
            using (var db = new ProcData())
            {
                db.UserMailViewTimeUpdate(id, DateTime.Now, AuthManager.User.UserId);
            }
        }

        public List<UserMailReceivedList> GetReceivedMails()
        {
            using (var db = new ProcData())
            {
                return db.UserMailReceivedListGet(AuthManager.User.CompanyId).ToList();
            }
        }

        public void SendMail(string subject, string body, Guid receiverCompanyId)
        {
            using (var db = new ProcData())
            {
                db.UserMailInsert(subject, body, AuthManager.User.UserId, AuthManager.User.CompanyId,
                    receiverCompanyId);
            }
        }
    }
}
