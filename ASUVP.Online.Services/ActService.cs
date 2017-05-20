using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IActService
    {
        List<ActDetails> GetActDetails(Guid id);
        void DeleteActs(Guid[] ids);
        List<ActList> GetActsByParametrs(Guid id, string periodType, string dateBeg, string dateEnd, string reportPeriod, string agreementId, string agrManagerId, string statusId, string epStatusId);
        Act GetAct(Guid id);

    }
    class ActService : BaseHttpService, IActService
    {
        private readonly string ActGroup = "ACT-TEO";

        public ActService(IEventLogger logger) : base(logger)
        {
        }
        public List<ActDetails> GetActDetails(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ActDetailsGet(id).ToList();
            }
        }

        public Act GetAct(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ActGet(AuthManager.User.UserId, id).FirstOrDefault();
            }
        }

        public void DeleteActs(Guid[] ids)
        {
            if (ids == null) return;

            foreach (var id in ids)
            {
                //delete agreements
            }
        }

        public List<ActList> GetActsByParametrs(Guid id, string periodType, string dateBeg, string dateEnd, string reportPeriod, string agreementId, string agrManagerId, string statusId, string epStatusId)
        {
            var acts = new List<ActList>();

            if (string.IsNullOrEmpty(periodType) && string.IsNullOrEmpty(dateBeg) && string.IsNullOrEmpty(dateEnd) && string.IsNullOrEmpty(reportPeriod) && string.IsNullOrEmpty(agreementId)
                && string.IsNullOrEmpty(agrManagerId) && string.IsNullOrEmpty(statusId) && string.IsNullOrEmpty(epStatusId))
                return acts;

            using (var context = new ProcData())
            {
                DateTime beginTime = new DateTime(1990, 1, 1);
                DateTime endTime = new DateTime(2050, 1, 1);
                if (!string.IsNullOrEmpty(dateBeg))
                    DateTime.TryParse(dateBeg, out beginTime);
                if (!string.IsNullOrEmpty(dateEnd))
                    DateTime.TryParse(dateEnd, out endTime);

                int period = -1;
                int.TryParse(periodType, out period);

                Guid report;
                Guid.TryParse(reportPeriod, out report);

                Guid agreement;
                Guid.TryParse(agreementId, out agreement);

                Guid agrManager;
                Guid.TryParse(agrManagerId, out agrManager);

                Guid status;
                Guid.TryParse(statusId, out status);

                Guid epStatus;
                Guid.TryParse(epStatusId, out epStatus);

                acts = context.ActListGet(
                    id,
                    ActGroup,
                    period == -1? 1: period,
                    beginTime,
                    endTime,
                    //report == Guid.Empty ? (Guid?)null : report,
                    0,
                    new DateTime(1990, 1, 1),
                    new DateTime(2050, 1, 1),
                    agreement == Guid.Empty ? (Guid?)null : agreement,
                    agrManager == Guid.Empty ? (Guid?)null : agrManager,
                    status == Guid.Empty? (Guid?)null : status,
                    epStatus == Guid.Empty? (Guid?)null : epStatus).ToList();
            }


            return acts;
        }
    }


}
