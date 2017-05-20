using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASUVP.Online.Web.Hubs
{
    public class UserConnection
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }
}