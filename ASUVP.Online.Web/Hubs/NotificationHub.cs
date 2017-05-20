using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Models.Notification;
using Microsoft.AspNet.SignalR;

namespace ASUVP.Online.Web.Hubs
{
    public class NotificationHub : Hub
    {
        //public static List<UserConnection> userConnections = new List<UserConnection>();

        //public override Task OnConnected()
        //{
        //    using (var context = new ProcData())
        //    {
        //        string name = Context.User.Identity.Name;

        //        var userConnection = new UserConnection()
        //        {
        //            UserId = Guid.Parse(context.UserByNameGet(name).FirstOrDefault().ToString()),
        //            UserName = name,
        //            ConnectionId = Context.ConnectionId
        //        };
        //        userConnections.RemoveAll(x => x.UserName == name);
        //        userConnections.Add(userConnection);

        //        return base.OnConnected();
        //    }
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.User.Identity.Name;
        //    userConnections.RemoveAll(x => x.UserName == name);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public void SendNotification(Guid userId)
        //{
        //        var connectionId = userConnections.First(x => x.UserId == userId).ConnectionId;
        //        Clients.Client(connectionId).NotificationIncrease(userId);
        //}

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!userConnections.Any(x => x.UserName == name))
        //    {
        //        using (var context = new ProcData())
        //        {
        //            var userConnection = new UserConnection()
        //            {
        //                UserId =
        //                    Guid.Parse(context.UserByNameGet(Context.User.Identity.Name).FirstOrDefault().ToString()),
        //                UserName = Context.User.Identity.Name,
        //                ConnectionId = Context.ConnectionId
        //            };
        //            userConnections.Add(userConnection);
        //        }
        //    }

        //    return base.OnReconnected();
        //}
    }
}