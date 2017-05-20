using ASUVP.Core.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ASUVP.Online.Web.Attributes;
using DevExpress.Web;

namespace ASUVP.Online.Web.Models.Contact
{
    public class ContactEditable
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Требуется поле Фамилия.", AllowEmptyStrings = false)]
        public string F { get; set; }

        public string I { get; set; }

        public string O { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный Email.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать компанию из списка.")]
        [ValidGuid]
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }
        public List<Company> Companies { get; set; }

        public static ContactEditable GetContact(Guid? id)
        {
            using (var context = new ProcData())
            {
                var contact = context.ContactDetailsGet(id).FirstOrDefault();
                var contactEditable = new ContactEditable();
                if (contact != null)
                {
                    contactEditable.Id = contact.Id;
                    contactEditable.F = contact.F;
                    contactEditable.I = contact.I;
                    contactEditable.O = contact.O;
                    contactEditable.Email = contact.Email;
                    contactEditable.Phone = contact.Phone;
                    contactEditable.CompanyId = contact.CompanyId ?? Guid.Empty;
                    contactEditable.CompanyName = contact.CompanyName;
                    contactEditable.Companies = context.CompanyGet(null).ToList();
                }

                return contactEditable;
            }
        }

        public static ContactEditable GetEmptyContact()
        {
            using (var context = new ProcData())
            {
                var contactEditable = new ContactEditable();
                    contactEditable.Companies = context.CompanyGet(null).ToList();
                return contactEditable;
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