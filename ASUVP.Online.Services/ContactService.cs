using System;
using System.Threading.Tasks;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace ASUVP.Online.Services
{
    public interface IContactService
    {
        //Task<RestResponse> AddContact(ContactDto contact);
        //Task<RestResponse> UpdateContact(ContactDto contact);
        //Task<RestResponse> DeleteContact(Guid id);
        //ContactDto GetContact(Guid id);

        List<ContactList> ContactListGet();
        ContactDetails ContactDetails(Guid id);
        void DeleteContacts(Guid[] keys);
        void ContactCreate(string f, string i, string o, string phone, string email, Guid? companyId, Guid createdBy);
        void ContactUpdate(Guid id, string f, string i, string o, string phone, string email, Guid? companyId, Guid createdBy);

    }

    public class ContactService : BaseHttpService, IContactService
    {
        public ContactService(IEventLogger logger) : base(logger)
        {
        }

        public void ContactCreate(string f, string i, string o, string phone, string email, Guid? companyId, Guid createdBy)
        {
            using (var context = new ProcData())
            {
                context.ContactInsert(f, i, o, phone, email, companyId, createdBy);
            }
        }

        public ContactDetails ContactDetails(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.ContactDetailsGet(id).FirstOrDefault();
            }
        }

        public List<ContactList> ContactListGet()
        {
            using (var context = new ProcData())
            {
                return context.ContactListGet().ToList();
            }
        }

        public void ContactUpdate(Guid id, string f, string i, string o, string phone, string email, Guid? companyId, Guid createdBy)
        {
            using (var context = new ProcData())
            {
                context.ContactUpdate(id, f, i, o, phone, email, companyId, createdBy);
            }
        }

        public void DeleteContacts(Guid[] keys)
        {
            using (var context = new ProcData())
            {
                if (keys!=null)
                {
                    foreach (var key in keys)
                    {
                        context.ContactDelete(key);
                    }
                }
            }
        }

    }
}