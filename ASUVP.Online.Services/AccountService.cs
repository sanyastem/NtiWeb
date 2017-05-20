using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IAccountService
    {
        AccountDetails GetAccountDetails(Guid id);
        void DeleteAccounts(Guid[] ids);
        List<AccountList> GetAccountsByParametrs(Guid companyId, string periodType, string dateBeg, string dateEnd, string reportPeriod, string agreementId, string agrManagerId, string statusId, string epStatusId);
        Account GetAccount(Guid id);
    }



    public class AccountService : BaseHttpService, IAccountService
    {
        private readonly string AccountGroup = "ACCOUNT-TEO";

        public AccountService(IEventLogger logger) : base(logger)
        {
        }

        public AccountDetails GetAccountDetails(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.AccountDetailsGet(id).FirstOrDefault();
            }
        }

        public Account GetAccount(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.AccountGet(AuthManager.User.CompanyId, id).FirstOrDefault();
            }
        }

        public void DeleteAccounts(Guid[] ids)
        {
            if (ids == null) return;

            foreach (var id in ids)
            {
                //delete agreements
            }
        }

        public List<AccountList> GetAccountsByParametrs(Guid companyId, string periodType, string dateBeg, string dateEnd, string reportPeriod, string agreementId, string agrManagerId, string statusId, string epStatusId)
        {
            var Accounts = new List<AccountList>();
            if (string.IsNullOrEmpty(periodType) && string.IsNullOrEmpty(dateBeg) && string.IsNullOrEmpty(dateEnd) && string.IsNullOrEmpty(reportPeriod) && string.IsNullOrEmpty(agreementId)
                && string.IsNullOrEmpty(statusId) && string.IsNullOrEmpty(epStatusId) && string.IsNullOrEmpty(agrManagerId))
                return Accounts;

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

                Guid epsStatus;
                Guid.TryParse(epStatusId, out epsStatus);


                Accounts = context.AccountListGet(
                    companyId,
                    AccountGroup,
                    period == -1 ? 0 : period,
                    beginTime,
                    endTime,
                    report == Guid.Empty ? (Guid?)null : report,
                    agreement == Guid.Empty ? (Guid?)null : agreement,
                    agrManager == Guid.Empty ? (Guid?)null : agrManager,
                    status == Guid.Empty? (Guid?)null : status,
                    epsStatus == Guid.Empty? (Guid?)null : epsStatus).ToList();
            }

            return Accounts;
        }
    }


}
