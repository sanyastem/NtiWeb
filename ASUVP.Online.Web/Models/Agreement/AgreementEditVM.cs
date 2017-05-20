using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models.Agreement
{
    public class AgreementEditVM
    {
        public List<Template> Templates { get; set; }
        public List<Company> Companies { get; set; }
        public Core.DataAccess.Model.Agreement Agreement { get; set; }

    }
}