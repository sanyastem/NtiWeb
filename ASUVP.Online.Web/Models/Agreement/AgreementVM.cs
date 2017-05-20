using System.Collections.Generic;

namespace ASUVP.Online.Web.Models.Agreement
{
    public class AgreementVM
    {
        public List<Core.DataAccess.Model.AgreementList> Agreements { get; set; }
        public FilterModel Filter { get; set; }
    }
}