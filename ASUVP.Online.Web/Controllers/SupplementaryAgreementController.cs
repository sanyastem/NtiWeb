using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using ASUVP.Online.Web.Models.SupplementaryAgreement;
using ASUVP.Online.Web.Hubs;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.AgreementView)]
    public class SupplementaryAgreementController : BaseController
    {
        private readonly IAgreementService _service;
        private readonly IDocumentService _documentService;
        private readonly IUserNotificationService _notificationService;

        public SupplementaryAgreementController(IAgreementService service, IDocumentService documentService, IUserNotificationService notificationService)
        {
            _service = service;
            _documentService = documentService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public ActionResult AddOrUpdateWizard(Guid? id, Guid? agrId)
        {
            SupplementaryAgreementWizardVM wizard = new SupplementaryAgreementWizardVM();

            if (agrId != null)
            {
                wizard.PossibleConditions = _documentService.GetAllDocumentConditions();

                Agreement agr = _service.GetAgreement((Guid)agrId);

                if (id == Guid.Empty || id == null)
                {
                    // Добавление нового
                    wizard.AgreementId = agrId;

                    wizard.CustomerCompanyLabel = agr.CustomerCompanyLabel;
                    wizard.CustomerCompanyId = agr.CustomerCompanyId;
                    wizard.CustomerCompanyName = agr.CustomerCompanyName;
                    wizard.CustomerContactId = agr.CustomerSignerId;
                    wizard.CustomerBankId = agr.CustomerBankId;
                    wizard.CustomerAddressId = agr.CustomerAddressId;

                    wizard.PerformerCompanyId = agr.PerformerCompanyId;
                    wizard.PerformerCompanyName = agr.PerformerCompanyName;
                    wizard.PerformerCompanyLabel = agr.PerformerCompanyLabel;
                    wizard.PerformerContactId = agr.PerformerSignerId;
                    wizard.PerformerBankId = agr.PerformerBankId;
                    wizard.PerformerAddressId = agr.PerformerAddressId;

                    wizard.StatusId = agr.StatusId;

                    wizard.Conditions = new List<DocumentCondition>();
                    wizard.Attachments = new List<DocumentAttach>();

                    ViewBag.Title = "Добавление нового дополнительного соглашения";
                }
                else
                {
                    // Редактирование
                    SupplementaryAgreement suplAgr = _service.GetSupplementaryAgreement((Guid)id);

                    wizard.AgreementId = agrId;

                    wizard.CustomerCompanyLabel = suplAgr.CustomerCompanyLabel;
                    wizard.CustomerCompanyId = suplAgr.CustomerCompanyId;
                    wizard.CustomerCompanyName = suplAgr.CustomerCompanyName;
                    wizard.CustomerContactId = suplAgr.CustomerSignerId;
                    wizard.CustomerBankId = suplAgr.CustomerBankId;
                    wizard.CustomerAddressId = suplAgr.CustomerAddressId;

                    wizard.PerformerCompanyId = suplAgr.PerformerCompanyId;
                    wizard.PerformerCompanyName = suplAgr.PerformerCompanyName;
                    wizard.PerformerCompanyLabel = suplAgr.PerformerCompanyLabel;
                    wizard.PerformerContactId = suplAgr.PerformerSignerId;
                    wizard.PerformerBankId = suplAgr.PerformerBankId;
                    wizard.PerformerAddressId = suplAgr.PerformerAddressId;

                    wizard.DateBeg = suplAgr.DateBeg;
                    wizard.DateEnd = suplAgr.DateEnd;
                    wizard.DateStop = suplAgr.DateStop;
                    wizard.DocDate = suplAgr.DocDate;
                    wizard.DocName = suplAgr.DocName;
                    wizard.DocNumber = suplAgr.DocNumber;
                    wizard.DocumentId = suplAgr.DocumentId;
                    wizard.IRSPerevozkiId = suplAgr.IRSPerevozkiId;
                    wizard.Note = suplAgr.Note;
                    wizard.RegNumber = suplAgr.RegNumber;
                    wizard.StatusId = suplAgr.StatusId;
                    wizard.StatusName = suplAgr.StatusName;
                    wizard.TemplateId = suplAgr.TemplateId;

                    wizard.Conditions = _service.GetAgreementConditions((Guid)id, null);
                    wizard.Attachments = new List<DocumentAttach>();

                    List<DocumentAttachList> attaches = _documentService.DocumentAttachListGet((Guid)id);
                    if (attaches.Count > 0)
                    {
                        foreach (var attach in attaches)
                        {
                            DocumentAttach newAtt = new DocumentAttach();
                            newAtt.Id = attach.Id;
                            newAtt.FName = attach.FName;
                            newAtt.Note = attach.Note;
                            wizard.Attachments.Add(newAtt);
                        }
                    }

                    ViewBag.Title = $"Редактирование дополнительного соглашения №{wizard.DocNumber}";
                }

                ViewBag.RootPageDetails = new string[] { "Details", "Agreement", $"Договор №{agr.DocNumber}", agrId.ToString() };
                ViewBag.IndexPageDetails = new string[] { "Index", "Agreement", "Договоры" };
            }

            Session["wizard"] = wizard;

            return View("AddOrUpdateWizard", wizard);
        }

        [HttpPost]
        public ActionResult AddOrUpdateWizard(SupplementaryAgreementWizardVM wizard)
        {
            Guid templateId = wizard.TemplateId;
            //string docNumber = wizard.DocNumber;
            //DateTime docDate = wizard.DocDate.Value;
            wizard = (SupplementaryAgreementWizardVM) Session["wizard"];
            wizard.TemplateId = templateId;

            string message = string.Empty;

            if (wizard.DocumentId == Guid.Empty || wizard.DocumentId == null)
            {
                // Добавление нового
                Guid? docId = _documentService.DocumentInsert(wizard.DocNumber, wizard.RegNumber, wizard.RegNumber,
                    wizard.DocDate,
                    wizard.CustomerCompanyName, wizard.PerformerCompanyName, wizard.CustomerCompanyId,
                    wizard.CustomerContactId,
                    wizard.PerformerCompanyId,
                    wizard.PerformerContactId, wizard.StatusId, wizard.TemplateId, wizard.Note);

                _service.SupplementaryAgreementInsert(wizard.AgreementId, docId);
                _service.AgreementInsert(docId, wizard.DateBeg, wizard.DateEnd, wizard.DateStop,
                    wizard.CustomerBankId, wizard.CustomerAddressId, wizard.PerformerBankId, wizard.PerformerAddressId);

                foreach (var cond in wizard.Conditions)
                {
                    _documentService.DocumentConditionInsert(docId, cond.ConditionId, cond.ValueString,
                        cond.ConditionLimitValue);
                }

                message = $"Дополнительное соглашение №{wizard.DocNumber} успешно добавлено.";
                ViewBag.Title = "Добавление нового дополнительного соглашения";

                // оповещение
                var idNotificstion = _notificationService.AddUserNotification(AuthManager.User.UserId,
                    AuthManager.User.CompanyId, AuthManager.User.UserId, AuthManager.User.CompanyId,
                    $"Добавлено дополнительное соглашение №{wizard.DocNumber}.", "agreement|details|", (Guid) docId);
                var contextHub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                contextHub.Clients.All.DisplayMessage(AuthManager.User.UserId.ToString(), message);
            }
            else
            {
                // ШАГ 1

                // Редактирование
                _documentService.DocumentUpdate(wizard.DocumentId, wizard.DocNumber, wizard.RegNumber,
                    wizard.RegNumber, wizard.DocDate,
                    wizard.DocName, wizard.DocName, wizard.CustomerCompanyId, wizard.CustomerContactId,
                    wizard.PerformerCompanyId,
                    wizard.PerformerContactId, wizard.StatusId, wizard.TemplateId, wizard.Note);
                _service.SupplementaryAgreementUpdate(wizard.DocumentId);
                _service.AgreementUpdate(wizard.DocumentId, wizard.DateBeg, wizard.DateEnd, wizard.DateStop,
                    wizard.CustomerBankId, wizard.CustomerAddressId, wizard.PerformerBankId,
                    wizard.PerformerAddressId);

                // ШАГ 2

                var currentConditions = wizard.Conditions.ToList();
                var oldConditions = _service.GetAgreementConditions(wizard.DocumentId, null);

                // Получаем список удалённых условий
                var deletedConditions = oldConditions.ToList();
                deletedConditions.RemoveAll(c => currentConditions.Any(wc => wc.ConditionId == c.ConditionId));

                foreach (var documentCondition in deletedConditions)
                {
                    _documentService.DocumentConditionDelete(wizard.DocumentId, documentCondition.ConditionId);
                }

                // Удаляем все неизменившиеся условия
                currentConditions.RemoveAll(c => oldConditions.Any(wc => wc.ConditionId == c.ConditionId &&
                                                                         string.Equals(wc.ValueString?.Trim(), c.ValueString?.Trim(),
                                                                             StringComparison.OrdinalIgnoreCase)
                                                                         && string.Equals(wc.ConditionLimitValue?.Trim(),
                                                                             c.ConditionLimitValue?.Trim(),
                                                                             StringComparison.OrdinalIgnoreCase)));

                // Добавляем либо изменяем текущие
                foreach (var cond in currentConditions)
                {
                    bool updated = false;

                    foreach (var oldCond in oldConditions)
                    {
                        if (cond.ConditionId == oldCond.ConditionId &&
                            (!string.Equals(cond.ValueString?.Trim(), oldCond.ValueString?.Trim(),
                                 StringComparison.OrdinalIgnoreCase) || !string.Equals(cond.ConditionLimitValue?.Trim(),
                                 oldCond.ConditionLimitValue?.Trim(), StringComparison.OrdinalIgnoreCase)))
                        {
                            _documentService.DocumentConditionUpdate(wizard.DocumentId, cond.ConditionId, cond.ValueString?.Trim(),
                                cond.ConditionLimitValue?.Trim());
                            updated = true;
                        }
                    }

                    if (!updated)
                        _documentService.DocumentConditionInsert(wizard.DocumentId, cond.ConditionId,
                            cond.ValueString?.Trim(), cond.ConditionLimitValue?.Trim());
                }

                // ШАГ 3

                var currentFiles = wizard.Attachments.ToList();
                var oldFiles = _documentService.DocumentAttachListGet(wizard.DocumentId);

                // Получаем список удалённых
                var deletedFiles = oldFiles.ToList();
                deletedFiles.RemoveAll(f => currentFiles.Any(cf => string.Equals(cf.FName?.Trim(), f.FName?.Trim(),
                    StringComparison.OrdinalIgnoreCase)));

                foreach (var deletedFile in deletedFiles)
                {
                    _documentService.DocumentAttachmentDelete(deletedFile.Id);
                }

                // Удаляем все неизменившиеся файлы
                currentFiles.RemoveAll(f => oldFiles.Any(
                    of => string.Equals(of.FName?.Trim(), f.FName?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                          string.Equals(of.Note?.Trim(), f.Note?.Trim(), StringComparison.OrdinalIgnoreCase)));

                // Добавляем текущие, либо изменяем
                foreach (var documentAttach in currentFiles)
                {
                    bool isUpdated = false;

                    foreach (var oldFile in oldFiles)
                    {
                        if (string.Equals(documentAttach.FName?.Trim(), oldFile.FName?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(documentAttach.Note?.Trim(), oldFile.Note?.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            _documentService.DocumentAttachmentUpdate(documentAttach.Id, documentAttach.Note);
                            isUpdated = true;
                        }
                    }

                    if (!isUpdated)
                        _documentService.DocumentAttachmentInsert(wizard.DocumentId, documentAttach.FName?.Trim(),
                            documentAttach.Content, documentAttach.Note?.Trim());
                }

                // FINALE

                message = $"Дополнительное соглашение №{wizard.DocNumber} успешно изменено.";
                ViewBag.Title = $"Редактирование дополнительного соглашения №{wizard.DocNumber}";

                // оповещение
                var idNotificstion = _notificationService.AddUserNotification(AuthManager.User.UserId,
                    AuthManager.User.CompanyId, AuthManager.User.UserId, AuthManager.User.CompanyId,
                    $"Дополнительное соглашение №{wizard.DocNumber} изменено.", "agreement|details|",
                    wizard.DocumentId);
                var contextHub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                contextHub.Clients.All.DisplayMessage(AuthManager.User.UserId.ToString(), message);
            }

            Agreement agr = _service.GetAgreement((Guid) wizard.AgreementId);

            ViewBag.RootPageDetails = new[]
                {"Details", "Agreement", $"Договор №{agr.DocNumber}", wizard.AgreementId.ToString()};
            ViewBag.IndexPageDetails = new[] {"Index", "Agreement", "Договоры"};

            return View("_Step4Complete", (object) message);

        }

        [HttpGet]
        public ActionResult Details(Guid id, Guid? agrId)
        {
            SupplementaryAgreement model = _service.GetSupplementaryAgreement(id);

            if (agrId.HasValue)
            {
                Agreement agreement = _service.GetAgreement((Guid)agrId);
                if (model.DocDate.HasValue)
                    ViewBag.Title = $"Дополнительное соглашение №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
                else
                    ViewBag.Title = $"Дополнительное соглашение №{model.DocNumber}";
                ViewBag.RootPageDetails = new string[] { "Details", "Agreement", $"Договор №{agreement.DocNumber}", agrId.ToString() };
                ViewBag.IndexPageDetails = new string[] { "Index", "Agreement", "Договоры" };
            }
            return View("Details", model);
        }

        public PartialViewResult ConditionData(Guid id, DateTime? date)
        {
            ViewBag.AgreementId = id;
            var model = _service.GetAgreementConditions(id, date);
            return PartialView("Details/_ConditionData", model);
        }

        public PartialViewResult ConditionPanel(Guid id)
        {
            var model = _service.GetSupplementaryAgreement(id);
            return PartialView("Details/_ConditionPanel", model);
        }

        [HttpGet]
        public ActionResult Audit(Guid id)
        {
            var model = _service.GetSupplementaryAgreement(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SupplementaryConditions()
        {
            return PartialView("EditorTemplates/_Step2Conditions", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewCondition(DocumentCondition condition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Conditions as List<DocumentCondition>);

                    if (!wizard.Exists(c => c.ConditionId == condition.ConditionId))
                    {
                        condition.ValueString = condition.ValueString?.Trim();
                        wizard.Add(condition);
                    }
                    else
                    {
                        throw new Exception("Такое условие уже существует.");
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Пожалуйста, заполните необходимые поля.";

            return PartialView("EditorTemplates/_Step2Conditions", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCondition(DocumentCondition condition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Conditions as List<DocumentCondition>);
                    int updateIndex = wizard.FindIndex(c => c.ConditionId == condition.ConditionId);
                    wizard.RemoveAt(updateIndex);
                    wizard.Insert(updateIndex, condition);

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Пожалуйста, заполните необходимые поля.";

            return PartialView("EditorTemplates/_Step2Conditions", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCondition(Guid conditionId)
        {
            var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Conditions as List<DocumentCondition>);
            int index = wizard.FindIndex(c => c.ConditionId == conditionId);
            wizard.RemoveAt(index);

            return PartialView("EditorTemplates/_Step2Conditions", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }

        public PartialViewResult SupplementaryAttachments()
        {
            return PartialView("EditorTemplates/_Step3Attachments", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewAttach(string content, string fName)
        {
            var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Attachments as List<DocumentAttach>);

            try
            {
                if (!wizard.Exists(a => a.FName == fName))
                {
                    DocumentAttach newAttach = new DocumentAttach();
                    newAttach.Id = Guid.NewGuid();
                    newAttach.Content = Convert.FromBase64String(content);
                    newAttach.FName = fName;
                    wizard.Add(newAttach);
                }
                else
                {
                    throw new Exception("Вложение с таким названием уже добавлено.");
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("EditorTemplates/_Step3Attachments", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateAttach(DocumentAttach attach)
        {
            var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Attachments as List<DocumentAttach>);

            int updateIndex = wizard.FindIndex(c => c.Id == attach.Id);
            wizard.RemoveAt(updateIndex);
            wizard.Insert(updateIndex, attach);

            return PartialView("EditorTemplates/_Step3Attachments", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAttach(Guid id)
        {
            var wizard = ((Session["wizard"] as SupplementaryAgreementWizardVM).Attachments as List<DocumentAttach>);
            int index = wizard.FindIndex(c => c.Id == id);
            wizard.RemoveAt(index);

            return PartialView("EditorTemplates/_Step3Attachments", (Session["wizard"] as SupplementaryAgreementWizardVM));
        }
    }
}