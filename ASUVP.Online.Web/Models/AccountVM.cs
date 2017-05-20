using ASUVP.Core.DataAccess.Model;
using System.Collections.Generic;

namespace ASUVP.Online.Web.Models
{
    public class AccountVM
    {
        public List<AccountList> Accounts { get; set; }
        public FilterModel Filter { get; set; }
    }
}