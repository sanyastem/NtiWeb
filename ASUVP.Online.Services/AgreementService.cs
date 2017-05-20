using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IAgreementService
    {
        void DeleteAgreements(Guid[] ids);
        List<AgreementList> GetAgreementsByParametrs(string manager, string template, bool active);
        Agreement GetAgreement(Guid id);
        List<DocumentCondition> GetAgreementConditions(Guid id, DateTime? date);
        List<SelectListItem> GetAgreementNumberList(Guid id);
        List<SupplementaryAgreementList> GetSupplementaryAgreementList(Guid id, bool? onlyActive);
        SupplementaryAgreement GetSupplementaryAgreement(Guid supplementaryAgreementId);
        void AgreementUpdate(Guid? documentId, DateTime? dateBeg, DateTime? dateEnd, DateTime? dateStop, Guid? 
            customerBankId, Guid? customerAddressId, Guid? performerBankId, Guid? performerAddressId);
        void SupplementaryAgreementInsert(Guid? agrId, Guid? suplAgrId);
        void AgreementInsert(Guid? documentId, DateTime? dateBeg, DateTime? dateEnd, DateTime? dateStop,
            Guid? customerBankId, Guid? customerAddressId, Guid? performerBankId, Guid? performerAddressId);
        void SupplementaryAgreementUpdate(Guid? supplAgrId);
    }

    public class AgreementService : BaseHttpService, IAgreementService
    {
        private readonly string AgreementGroup = "AGREEMENT-TEO";

        public AgreementService(IEventLogger logger) : base(logger)
        {
        }

        public void DeleteAgreements(Guid[] ids)
        {
            if (ids == null) return;

            using (var context = new ProcData())
            {
                foreach (var id in ids)
                {
                    context.AgreementDetele(id);
                }
            }
        }

        public Agreement GetAgreement(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.AgreementGet(AuthManager.User.CompanyId, id).FirstOrDefault();
            }
        }


        public List<DocumentCondition> GetAgreementConditions(Guid id, DateTime? date)
        {
            var result = new List<DocumentCondition>();

            using (var context = new ProcData())
            {
                result = context.DocumentConditionGet(id, date).ToList();
            }

            return result;
        }

        public SupplementaryAgreement GetSupplementaryAgreement(Guid supplementaryAgreementId)
        {
            using (var context = new ProcData())
            {
                return context.SupplementaryAgreementGet(AuthManager.User.UserId, supplementaryAgreementId).FirstOrDefault();
            }
        }

        public List<SupplementaryAgreementList> GetSupplementaryAgreementList(Guid id, bool? onlyActive)
        {
            var result = new List<SupplementaryAgreementList>();

            using (var context = new ProcData())
            {
                result = context.SupplementaryAgreementListGet(id, onlyActive).ToList();
            }

            return result;
        }

        public void SupplementaryAgreementInsert(Guid? agrId, Guid? suplAgrId)
        {
            using (var context = new ProcData())
            {
                context.SupplementaryAgreementInsert(agrId, suplAgrId, AuthManager.User.UserId);
            }
        }

        public List<SelectListItem> GetAgreementNumberList(Guid id)
        {
            using (var context = new ProcData())
            {
                    var agreements = context.AgreementNumberListGet(id);
                var list = agreements.Select(x => new SelectListItem { Text = x.AgreementName, Value = x.AgreementId.ToString() }).ToList();
                return list;
            }
        }

        public List<AgreementList> GetAgreementsByParametrs(string manager, string template, bool active)
        {
            var date = DateTime.Now;
            var agreements = new List<AgreementList>();

            if (string.IsNullOrEmpty(manager) && string.IsNullOrEmpty(template))
                return agreements;

            using (var context = new ProcData())
            {
                Guid contactId = Guid.Empty;
                Guid templateId = Guid.Empty;
                Guid.TryParse(manager, out contactId);
                Guid.TryParse(template, out templateId);

                agreements = context.AgreementListGet(AuthManager.User.CompanyId, AgreementGroup,
                    contactId == Guid.Empty ? (Guid?)null : contactId,
                    templateId == Guid.Empty ? (Guid?)null : templateId, active).ToList();
            }

            return agreements;
        }

        public void AgreementUpdate(Guid? documentId, DateTime? dateBeg, DateTime? dateEnd, DateTime? dateStop, Guid? customerBankId, Guid? customerAddressId, Guid? performerBankId, Guid? performerAddressId)
        {
            using (var context = new ProcData())
            {
                context.AgreementUpdate(documentId, dateBeg, dateEnd, dateStop,
                    customerBankId, customerAddressId,
                    performerBankId, performerAddressId, AuthManager.User.UserId);
            }
        }

        public void AgreementInsert(Guid? documentId, DateTime? dateBeg, DateTime? dateEnd, DateTime? dateStop,
            Guid? customerBankId, Guid? customerAddressId, Guid? performerBankId, Guid? performerAddressId)
        {
            using (var context = new ProcData())
            {
                context.AgreementInsert(documentId, dateBeg, dateEnd, dateStop, customerBankId, customerAddressId, performerBankId, performerAddressId, AuthManager.User.UserId);
            }
        }

        public void SupplementaryAgreementUpdate(Guid? supplAgrId)
        {
            using (var context = new ProcData())
            {
                context.SupplementaryAgreementUpdate(supplAgrId, AuthManager.User.UserId);
            }
        }
    }
}