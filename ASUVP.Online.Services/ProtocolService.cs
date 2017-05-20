using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IProtocolService
    {
        List<ProtocolDetails> GetProtocolDetails(Guid id);
        Protocol GetProtocol(Guid id);
        void DeleteProtocols(Guid[] ids);
        List<ProtocolList> GetProtocolsByParametrs(Guid id, string periodType, string dateBeg, string dateEnd,
            string usePeriod, string useDateBeg,
            string useDateEnd, string agreementId,
            string agrManagerId, string statusId, string epStatusId);

    }

    class ProtocolService : BaseHttpService, IProtocolService
    {
        private readonly string ProtocolGroup = "PROTOCOL-TEO";

        public ProtocolService(IEventLogger logger) : base(logger)
        {
        }

        public List<ProtocolDetails> GetProtocolDetails(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ProtocolDetailsGet(id).ToList();
            }
        }
        public Protocol GetProtocol(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ProtocolGet(AuthManager.User.CompanyId, id).FirstOrDefault();
            }
        }

        public void DeleteProtocols(Guid[] ids)
        {
            if (ids == null) return;

            foreach (var id in ids)
            {
                //delete protocols
            }
        }

        public List<ProtocolList> GetProtocolsByParametrs(Guid id, string periodType, string dateBeg, string dateEnd,
            string usePeriod, string useDateBeg,string useDateEnd,
            string agreementId,string agrManagerId, string statusId, string epStatusId)
        {
            var protocols = new List<ProtocolList>();

            if (string.IsNullOrEmpty(periodType) && string.IsNullOrEmpty(dateBeg) && string.IsNullOrEmpty(dateEnd) &&
                string.IsNullOrEmpty(usePeriod) && string.IsNullOrEmpty(useDateBeg) && string.IsNullOrEmpty(useDateEnd) &&
                string.IsNullOrEmpty(agreementId) && string.IsNullOrEmpty(agrManagerId) && string.IsNullOrEmpty(statusId)&&
                string.IsNullOrEmpty(epStatusId))
                return protocols;

            using (var context = new ProcData())
            {
                DateTime beginTime = new DateTime(1990, 1, 1);
                DateTime endTime = new DateTime(2050, 1, 1);
                if (!string.IsNullOrEmpty(dateBeg))
                    DateTime.TryParse(dateBeg, out beginTime);
                if (!string.IsNullOrEmpty(dateEnd))
                    DateTime.TryParse(dateEnd, out endTime);

                DateTime useBeginTime = new DateTime(1990, 1, 1);
                DateTime useEndTime = new DateTime(2050, 1, 1);
                if (!string.IsNullOrEmpty(useDateBeg))
                    DateTime.TryParse(useDateBeg, out useBeginTime);
                if (!string.IsNullOrEmpty(useDateEnd))
                    DateTime.TryParse(useDateEnd, out useEndTime);

                int period = -1;
                int.TryParse(periodType, out period);

                int periodUse = -1;
                int.TryParse(periodType, out periodUse);



                Guid agreement;
                Guid.TryParse(agreementId, out agreement);

                Guid agrManager;
                Guid.TryParse(agrManagerId, out agrManager);

                Guid status;
                Guid.TryParse(statusId, out status);

                Guid epsStatus;
                Guid.TryParse(epStatusId, out epsStatus);



                protocols = context.ProtocolListGet(
                    id,
                    ProtocolGroup,
                    period == -1 ? 1 : period,
                    beginTime,
                    endTime,
                    periodUse == -1 ? 1 : periodUse,
                    useBeginTime,
                    useEndTime,
                    agreement == Guid.Empty ? (Guid?)null : agreement,
                    agrManager == Guid.Empty ? (Guid?)null : agrManager,
                    status == Guid.Empty ? (Guid?)null : status,
                    epsStatus == Guid.Empty ? (Guid?)null : epsStatus).ToList();
            }

            return protocols;

        }
    }
}
