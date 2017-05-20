using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IUserNotificationService
    {
        Guid AddUserNotification(Guid userFromId, Guid companyFromId, Guid userToId, Guid companyToId, string text, string url, Guid objectId);
        void UpdateNotificationState(Guid[] ids);
        List<UserNotificationList> GetUserNotifications(Guid managerId, Guid? notofocationId);
    }

    public class UserNotificationService : BaseHttpService, IUserNotificationService
    {
        public UserNotificationService(IEventLogger logger) : base(logger)
        {
        }

        public Guid AddUserNotification(Guid userFromId, Guid companyFromId, Guid userToId, Guid companyToId, string text, string url, Guid objectId)
        {
            using (var context = new ProcData())
            {
                var a =context.UserNotificationInsert(userFromId, companyFromId, userToId, companyToId, text, url, objectId).ToList();
                var z = Guid.Parse(a.First().ToString());
                context.SaveChanges();
                return z;
            }
        }

        public void UpdateNotificationState(Guid[] ids)
        {
            if (ids==null || ids.Length==0) return;
            using (var context = new ProcData())
            {
                foreach (var id in ids)
                {
                    context.UserNotificationChangeState(id, AuthManager.User.UserId);
                    context.SaveChanges();
                }
            }
        }

        public List<UserNotificationList> GetUserNotifications(Guid managerId, Guid? notofocationId)
        {
            List<UserNotificationList> notifications = new List<UserNotificationList>();

            using (var context = new ProcData())
            {
                notifications = context.UserNotificationListGet(notofocationId, managerId).ToList();
            }

            return notifications;
        }
    }
}
