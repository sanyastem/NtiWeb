using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASUVP.Online.Web.Models.Claim
{
    public class ClaimCondition
    {
        public string ClaimCoordination { get; set; }
        public string PlaneCoordination { get; set; }
        public string ProtocolPriceCoordination { get; set; }
        public string ProtocolName { get; set; }
        public Guid ProtocolId { get; set; }
        public string PrepaidScore { get; set; }
        public string ScoreName { get; set; }
        public Guid ScoreId { get; set; }
    }
}