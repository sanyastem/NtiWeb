using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using Microsoft.AspNet.Identity;

namespace ASUVP.Online.Services
{
    public interface IUserService
    {
        List<UserList> GetUserList();
        List<UserDetailsList> GetUserDetailsList(Guid? userId);
        UserGet GetUserById(Guid id);
        string UserInsert(string name, string password, string contactId);
        string UserUpdate(Guid userid, Guid contactId, string userName, bool isActive);
        Guid GetUserIdByName(string name);
    }

    public class UserService : BaseHttpService, IUserService
    {
        public UserService(IEventLogger logger) : base(logger)
        {

        }

        public List<UserList> GetUserList()
        {
            List<UserList> users = new List<UserList>();

            using (var context = new ProcData())
            {
                users = context.UserListGet().ToList();
            }

            return users;
        }

        public List<UserDetailsList> GetUserDetailsList(Guid? userId)
        {
            List<UserDetailsList> userDetailsList = new List<UserDetailsList>();

            if (userId != null)
            {
                using (var context = new ProcData())
                {
                    userDetailsList = context.UserDetailsListGet(userId.Value).ToList();
                }
            }

            return userDetailsList;
        }

        public UserGet GetUserById(Guid id)
        {
            UserGet user;
            using (var context = new ProcData())
            {
                user = context.UserGet(id).ToList().FirstOrDefault();
            }
            return user;
        }

        public string UserInsert(string name, string password, string contactId)
        {
            var hasher = new PasswordHasher();
            Guid contact;
            Guid.TryParse(contactId, out contact);
            string message;

            using (var context = new ProcData())
            {
                message = context.UserInsert(contact, name, hasher.HashPassword(password), Guid.NewGuid().ToString(), AuthManager.User.UserId).ToList().FirstOrDefault().ToString();
            }
            return message;
        }

        public string UserUpdate(Guid userid, Guid contactId, string userName, bool isActive)
        {
            string result;

            using (var context = new ProcData())
            {
                result = context.UserUpdate(userid, contactId, userName, isActive, AuthManager.User.UserId).ToList().FirstOrDefault().ToString();
            }

            return result;
        }

        public Guid GetUserIdByName(string name)
        {
            Guid result = new Guid();

            using (var context = new ProcData())
            {
                var firstOrDefault = context.UserByNameGet(name).ToList().FirstOrDefault();
                if (firstOrDefault != null)
                    result = (Guid) firstOrDefault;
            }

            return result;
        }
    }
}