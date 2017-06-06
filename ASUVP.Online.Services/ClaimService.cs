using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IClaimService
    {
        ClaimDetails GetClaimDetails(Guid id);
        void DeleteClaims(Guid[] ids);
        List<ClaimList> GetClaimsByParametrs(string period, string dateBeg, string dateEnd,
            string shipment, string shipmentBeg, string shipmentEnd,
            string coordination, string signing, string agreement, string manager, string approvalStatus, string signingStatu);
        List<Template> GetTemplate();
        List<ClaimRouteDetails> GetClaimRouteDetails(string stFrom, string stTo);

        Claim GetClaim(Guid id);
        ClaimRollingStock GetClaimRollingStock(Guid id);
        List<DocumentCondition> GetClaimConditions(Guid id);
        ClaimLoadingSchedule GetClaimLoadingSchedule(Guid id);
    }
    public class ClaimService : BaseHttpService, IClaimService
    {
        private readonly string PROTOCOL_GROUP = "CLAIM-TEO";

        public ClaimService(IEventLogger logger) : base(logger)
        {
        }

        public ClaimDetails GetClaimDetails(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ClaimDetailsGet(id, null, PROTOCOL_GROUP).FirstOrDefault();
            }
        }

        public void DeleteClaims(Guid[] ids)
        {
            if (ids == null) return;

            foreach (var id in ids)
            {
                //delete Claims
            }
        }

        public List<ClaimList> GetClaimsByParametrs(string period, string dateBeg, string dateEnd,
            string shipment, string shipmentBeg, string shipmentEnd,
            string coordination, string signing, string agreement, string manager, string approvalStatus, string signingStatus)
        {
            var claims = new List<ClaimList>();

            if (string.IsNullOrEmpty(period) && string.IsNullOrEmpty(dateBeg) && string.IsNullOrEmpty(dateEnd) && string.IsNullOrEmpty(shipment) && string.IsNullOrEmpty(shipmentBeg)
                && string.IsNullOrEmpty(shipmentEnd) && string.IsNullOrEmpty(coordination) && string.IsNullOrEmpty(signing) && string.IsNullOrEmpty(agreement) && string.IsNullOrEmpty(manager) 
                && string.IsNullOrEmpty(approvalStatus) && string.IsNullOrEmpty(signingStatus))
            return claims;

            using (var context = new ProcData())
            {
                int docPer = -1;
                DateTime docDateBeg = new DateTime(1990, 1, 1);
                DateTime docDateEnd = new DateTime(2050, 1, 1);
                int loadPer = -1;
                DateTime loadPerBeg = new DateTime(1990, 1, 1);
                DateTime loadPerEnd = new DateTime(2050, 1, 1);
                Guid man;
                Guid sign;
                Guid coord;
                Guid agr;
                Guid.TryParse(manager, out man);
                Guid.TryParse(coordination, out sign);
                Guid.TryParse(signing, out coord);
                Guid.TryParse(agreement, out agr);
                int.TryParse(period, out docPer);
                int.TryParse(shipment, out loadPer);
                if (!string.IsNullOrEmpty(dateBeg)) DateTime.TryParse(dateBeg, out docDateBeg);
                if (!string.IsNullOrEmpty(dateEnd)) DateTime.TryParse(dateEnd, out docDateEnd);
                if (!string.IsNullOrEmpty(shipmentBeg)) DateTime.TryParse(shipmentBeg, out loadPerBeg);
                if (!string.IsNullOrEmpty(shipmentEnd)) DateTime.TryParse(shipmentEnd, out loadPerEnd);
                Guid approvalStatusId;
                Guid signingStatusId;
                Guid.TryParse(approvalStatus, out approvalStatusId);
                Guid.TryParse(signingStatus, out signingStatusId);

                claims = context.ClaimListGet(AuthManager.User.CompanyId, PROTOCOL_GROUP,
                    docPer == -1 ? (int?)null : docPer,
                    docDateBeg, docDateEnd, 
                    loadPer == -1 ? (int?)null : loadPer,
                    loadPerBeg, loadPerEnd, 
                    agr == Guid.Empty? (Guid?) null : agr,
                    coord == Guid.Empty ? (Guid?)null : coord,
                    sign == Guid.Empty ? (Guid?)null : sign,
                    man == Guid.Empty ? (Guid?)null : man,
                    approvalStatusId == Guid.Empty ? (Guid?)null : approvalStatusId,
                    signingStatusId == Guid.Empty ? (Guid?)null : signingStatusId
                    ).ToList();
            }
            return claims;
        }

        public Claim GetClaim(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ClaimGet(AuthManager.User.UserId, id).FirstOrDefault(); 
            }
        }

        public ClaimRollingStock GetClaimRollingStock(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ClaimRollingStockGet(id, null, PROTOCOL_GROUP).FirstOrDefault();
            }
        }
        public List<ClaimRouteDetails> GetClaimRouteDetails(string stFrom, string stTo)
        {
            Guid from;
            Guid to;
            Guid.TryParse(stFrom, out from);
            Guid.TryParse(stTo, out to);

            using (var context = new ProcData())
            {
                return context.ClaimRouteDetailsGet(from == Guid.Empty ? (Guid?)null : from,
                    to == Guid.Empty ? (Guid?)null : to, null).ToList();
            }
        }
        public List<DocumentCondition> GetClaimConditions(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.DocumentConditionGet(id, null).ToList();
            }
        }
        public ClaimLoadingSchedule GetClaimLoadingSchedule(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ClaimLoadingScheduleGet(id, null, PROTOCOL_GROUP).FirstOrDefault();
            }
        }

        public List<Template> GetTemplate()
        {
            using (var context = new ProcData())
            {
                return context.GetTemplate().ToList();
            }
        }
    }
}
