using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IDocumentService
    {
        void DocumentUpdate(Guid? documentId, string docNumber, string customerRegNumber, string performerRegNumber,
            DateTime? docDate, string customerName, string performerName,
            Guid? customerCompanyId, Guid? customerContactId, Guid? performerCompanyId, Guid? performerContactId,
            Guid? statusId, Guid? templateId, string note);

        Guid? DocumentInsert(string docNumber, string customerRegNumber, string performerRegNumber, DateTime? docDate,
            string customerName,
            string performerName, Guid? customerCompanyId, Guid? customerContactId, Guid? performerCompanyId,
            Guid? performerContactId, Guid? statusId, Guid? templateId,
            string note);

        List<DocumentConditionList> GetAllDocumentConditions();
        void DocumentConditionInsert(Guid? documentId, Guid? conditionId, string value, string conditionValueLimit);
        void DocumentConditionUpdate(Guid documentId, Guid conditionId, string value, string conditionValueLimit);
        void DocumentConditionDelete(Guid documentId, Guid conditionId);

        List<DocumentAttachList> DocumentAttachListGet(Guid documentId);
        void DocumentAttachmentInsert(Guid documentId, string fileName, byte[] fileContent, string note);
        void DocumentAttachmentUpdate(Guid id, string note);
        void DocumentAttachmentDelete(Guid id);
    }

    public class DocumentService : BaseHttpService, IDocumentService
    {
        public DocumentService(IEventLogger logger) : base(logger)
        {
        }

        public void DocumentUpdate(Guid? documentId, string docNumber, string customerRegNumber,
            string performerRegNumber, DateTime? docDate, string customerName, string performerName,
            Guid? customerCompanyId, Guid? customerContactId, Guid? performerCompanyId, Guid? performerContactId,
            Guid? statusId, Guid? templateId, string note)
        {
            using (var context = new ProcData())
            {
                context.DocumentUpdate(documentId, docNumber, customerRegNumber,
                    performerRegNumber, docDate, customerName, performerName,
                    customerCompanyId, customerContactId, performerCompanyId,
                    performerContactId, statusId, templateId, note, AuthManager.User.UserId);
            }
        }

        public Guid? DocumentInsert(string docNumber, string customerRegNumber, string performerRegNumber,
            DateTime? docDate, string customerName,
            string performerName, Guid? customerCompanyId, Guid? customerContactId, Guid? performerCompanyId,
            Guid? performerContactId, Guid? statusId, Guid? templateId,
            string note)
        {
            using (var context = new ProcData())
            {
                return context.DocumentInsert(docNumber, customerRegNumber,
                        performerRegNumber, docDate, customerName, performerName,
                        customerCompanyId, customerContactId, performerCompanyId, performerContactId, statusId,
                        templateId,
                        note, AuthManager.User.UserId)
                    .FirstOrDefault();
            }
        }

        public List<DocumentAttachList> DocumentAttachListGet(Guid documentId)
        {
            using (var context = new ProcData())
            {
                return context.DocumentAttachListGet(documentId).ToList();
            }

        }

        public List<DocumentConditionList> GetAllDocumentConditions()
        {
            using (var db = new ProcData())
            {
                return db.DocumentConditionListGet().ToList();
            }
        }

        public void DocumentConditionInsert(Guid? documentId, Guid? conditionId, string value, string conditionValueLimit)
        {
            using (var db = new ProcData())
            {
                string valueString = value;
                int? valueInteger = null;
                double? valueFloat = null;
                decimal? valueMoney = null;
                Guid? valueGuid = null;
                DateTime? valueDateTime = null;

                //switch (value)
                //{
                //    case "uniqueidentifier":
                //        valueGuid = Guid.Parse(value);
                //        break;
                //    case "string":
                //        valueString = value;
                //        break;
                //    case "int":
                //        valueInteger = int.Parse(value);
                //        break;
                //    case "double":
                //        valueFloat = double.Parse(value);
                //        break;
                //        case "decimal"
                //}

                db.DocumentConditionInsert(documentId, conditionId, valueString, valueInteger, valueFloat, valueMoney,
                    valueGuid, valueDateTime, conditionValueLimit, AuthManager.User.UserId);
            }
        }

        public void DocumentConditionUpdate(Guid documentId, Guid conditionId, string value, string conditionValueLimit)
        {
            string valueString = value;
            int? valueInteger = null;
            double? valueFloat = null;
            decimal? valueMoney = null;
            Guid? valueGuid = null;
            DateTime? valueDateTime = null;

            using (var db = new ProcData())
            {
                db.DocumentConditionUpdate(documentId, conditionId, valueString, valueInteger, valueFloat, valueMoney, valueGuid,
                    valueDateTime, conditionValueLimit, AuthManager.User.UserId);
            }
        }

        public void DocumentConditionDelete(Guid documentId, Guid conditionId)
        {
            using (var db = new ProcData())
            {
                db.DocumentConditionDelete(documentId, conditionId, AuthManager.User.UserId);
            }
        }

        public void DocumentAttachmentInsert(Guid documentId, string fileName, byte[] fileContent, string note)
        {
            using (var db = new ProcData())
            {
                db.DocumentAttachInsert(documentId, fileName, fileContent, note, AuthManager.User.UserId);
            }
        }

        public void DocumentAttachmentUpdate(Guid id, string note)
        {
            using (var db = new ProcData())
            {
                db.DocumentAttachUpdate(id, note, AuthManager.User.UserId);
            }
        }

        public void DocumentAttachmentDelete(Guid id)
        {
            using (var db = new ProcData())
            {
                db.DocumentAttachDelete(id, AuthManager.User.UserId);
            }
        }
    }
}
