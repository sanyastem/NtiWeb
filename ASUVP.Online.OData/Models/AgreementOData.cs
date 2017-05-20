using System;
using Microsoft.OData.Client;

namespace ASUVP.Online.OData.Models
{
    public partial class AgreementOData
    {
        [OriginalName("Date")]
        public DateTime UnboundDate => Date.DateTime;

        [OriginalName("StartDate")]
        public DateTime UnboundStartDate => StartDate.DateTime;

        [OriginalName("EndDate")]
        public DateTime UnboundEndDate => EndDate.DateTime;
    }
}