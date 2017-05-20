using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;


namespace ASUVP.Online.Web.Models.SupplementaryAgreement
{
    [Serializable]
    public class SupplementaryAgreementWizardVM
    {
        public Guid? AgreementId { get; set; }
        public Guid DocumentId { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        [Required]
        public string DocNumber { get; set; }
        public string RegNumber { get; set; }
        [Required]
        public DateTime? DocDate { get; set; }
        public Guid? StatusId { get; set; }
        public string StatusName { get; set; }
        [Required]
        public string DocName { get; set; }
        public Guid? CustomerAddressId { get; set; }
        public Guid? CustomerBankId { get; set; }
        public Guid? CustomerCompanyId { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyLabel { get; set; }
        public Guid? CustomerContactId { get; set; }
        public Guid? PerformerAddressId { get; set; }
        public Guid? PerformerBankId { get; set; }
        public Guid? PerformerCompanyId { get; set; }
        public string PerformerCompanyName { get; set; }
        public string PerformerCompanyLabel { get; set; }
        public Guid? PerformerContactId { get; set; }
        public DateTime? DateBeg { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStop { get; set; }
        public string Note { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? IRSPerevozkiId { get; set; }

        // --------------STEP 2------------------
        public List<DocumentCondition> Conditions { get; set; }

        public List<DocumentConditionList> PossibleConditions { get; set; }

        // --------------STEP 3-------------------
        public List<DocumentAttach> Attachments { get; set; }
    }  
}