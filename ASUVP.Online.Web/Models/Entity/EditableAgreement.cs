using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Web.Models.Entity
{
    public class EditableAgreement
    {
        public Guid? DocumentId { get; set; }
        [Required]
        public DateTime DateBeg { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStop { get; set; }
        public Guid? CustomerBankId { get; set; }
        public Guid? CustomerAddressId { get; set; }
        public Guid? PerformerBankId { get; set; }
        public Guid? PerformerAddressId { get; set; }
        public string CustomerBank { get; set; }
        public string CustomerAddress { get; set; }
        public string PerformerBank { get; set; }
        public string PerformerAddress { get; set; }

        [Required]
        public string DocNumber { get; set; }
        public string Name { get; set; }
        public string CustomerRegNumber { get; set; }
        public string PerformerRegNumber { get; set; }
        public string RegNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string CustomerName { get; set; }
        public string PerformerName { get; set; }
        [Required]
        public Guid CustomerCompanyId { get; set; }

        public Guid? CustomerContactId { get; set; }
        public string CustomerContact { get; set; }
        public Guid? PerformerCompanyId { get; set; }

        public Guid? PerformerContactId { get; set; }
        public string PerformerContact { get; set; }
        public Guid? StatusId { get; set; }
        public string StatusName { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        public string Note { get; set; }
        public List<Company> Companies { get; set; }

        public static EditableAgreement GetAgreement(Guid? id)
        {
            using (var context = new ProcData())
            {
                var agreement = context.AgreementGet(AuthManager.User.CompanyId, id).FirstOrDefault();
                EditableAgreement editAgreement = null;
                if (agreement != null)
                {
                    editAgreement = new EditableAgreement();
                    editAgreement.DocumentId = agreement.DocumentId;
                    if (agreement.DateBeg.HasValue)
                        editAgreement.DateBeg = agreement.DateBeg.Value;
                    editAgreement.DateEnd = agreement.DateEnd;
                    editAgreement.DateStop = agreement.DateStop;
                    editAgreement.CustomerBankId = agreement.CustomerBankId;
                    editAgreement.CustomerAddressId = agreement.CustomerAddressId;
                    editAgreement.PerformerBankId = agreement.PerformerBankId;
                    editAgreement.PerformerAddressId = agreement.PerformerAddressId;
                    editAgreement.DocNumber = agreement.DocNumber;
                    editAgreement.Name = agreement.DocName;
                    editAgreement.RegNumber = agreement.RegNumber;
                    editAgreement.DocDate = agreement.DocDate;
                    editAgreement.CustomerName = agreement.CustomerCompanyName;
                    editAgreement.PerformerName = agreement.PerformerCompanyName;
                    if (agreement.CustomerCompanyId.HasValue)
                        editAgreement.CustomerCompanyId = agreement.CustomerCompanyId.Value;
                    editAgreement.CustomerContactId = agreement.CustomerCompanyId;
                    editAgreement.PerformerCompanyId = agreement.PerformerCompanyId;
                    editAgreement.CustomerContact = agreement.CustomerSignerName;
                    editAgreement.PerformerContact = agreement.CustomerSignerName;
                    editAgreement.PerformerContactId = agreement.PerformerCompanyId;
                    editAgreement.StatusId = agreement.StatusId;
                    editAgreement.StatusName = agreement.StatusName;
                    editAgreement.TemplateId = agreement.TemplateId;
                    editAgreement.Note = agreement.Note;
                    editAgreement.PerformerName = agreement.PerformerCompanyName;
                    editAgreement.CustomerName = agreement.CustomerCompanyName;
                    editAgreement.Companies = context.CompanyGet(null).ToList();
                }

                return editAgreement;
            }
        }

        public static List<Company> GetCompanysRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            using (var context = new ProcData())
            {
                return context.CompanyGet(null).Where(c => c.ShortName.StartsWith(args.Filter)).OrderBy(c => c.ShortName).Skip(skip).Take(take).ToList();
            }
        }

        public static List<Company> GetCompanyById(ListEditItemRequestedByValueEventArgs args)
        {
            Guid id;
            if (args.Value == null || !Guid.TryParse(args.Value.ToString(), out id))
                return null;
            using (var context = new ProcData())
            {
                return context.CompanyGet(id).ToList();
            }
        }

        public static Company GetCompanyById(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.CompanyGet(id).FirstOrDefault();
            }
        }
    }
}