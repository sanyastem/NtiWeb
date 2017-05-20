using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models.Claim
{
    public class ClaimVM
    {
        public FilterModel Filter { get; set; }
        public List<ClaimList> Claims { get; set; }
    }
}