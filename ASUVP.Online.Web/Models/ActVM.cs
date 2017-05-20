using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models
{
    public class ActVM
    {
        public List<ActList> Acts { get; set; }
        public FilterModel Filter { get; set; }
    }
}