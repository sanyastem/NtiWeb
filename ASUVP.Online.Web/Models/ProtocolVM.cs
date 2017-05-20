using ASUVP.Core.DataAccess.Model;
using System.Collections.Generic;

namespace ASUVP.Online.Web.Models
{
    public class ProtocolVM
    {
        public List<ProtocolList> Protocols { get; set; }
        public FilterModel Filter { get; set; }
    }
}