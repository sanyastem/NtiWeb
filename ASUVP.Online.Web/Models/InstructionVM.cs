using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Services;

namespace ASUVP.Online.Web.Models
{
    public class InstructionVM
    {
        public List<InstructionList> Instructions { get; set; }
        public FilterModel Filter { get; set; }
    }
}