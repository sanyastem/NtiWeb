using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using ASUVP.Online.Web.Models;
using ASUVP.Online.Web.Models.Agreement;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;
using ASUVP.Online.Web.Models.Entity;
using ASUVP.Core.Web.Report;
using System.Web;
using System.Net;
using System.Text;
using ASUVP.Online.Web.Hubs;
using ASUVP.Online.Web.Models.Notification;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.AgreementView)]
    public class AgreementController : BaseController
    {
        private readonly IAgreementService _service;
        private readonly IUserNotificationService _notificationService;
        private readonly IDocumentService _documentService;

        public AgreementController(IAgreementService service, IUserNotificationService notificationService, IDocumentService documentService)
        {
            _service = service;
            _notificationService = notificationService;
            _documentService = documentService;
        }

        public ActionResult Index(string manager = null, string template = null, bool active = true)
        {
            ViewBag.Title = "Договоры";

            var model = new AgreementVM()
            {
                Filter = new FilterModel() { AgrManagerId = manager, TemplateId = template, OnlyActive = active }
            };

            return View(model);
        }

        public ActionResult AgreementViewPartial(string manager = null, string template = null, bool active = false)
        {

            var model = new AgreementVM()
            {
                Filter = new FilterModel() { AgrManagerId = manager, TemplateId = template, OnlyActive = active },
                Agreements = _service.GetAgreementsByParametrs(manager, template, active)
            };

            return PartialView("_AgreementViewPartial", model);
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.GetAgreement(id);
            if (AuthManager.User.CompanyId != model.CustomerCompanyId && AuthManager.User.CompanyId != model.PerformerCompanyId)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            ViewBag.Title = $"Договор №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
            ViewBag.RootPageDetails = new string[] { "Index", "Agreement", "Договоры" };
            return View(model);
        }

        public PartialViewResult AgreementConditionData(Guid id, DateTime? date)
        {
            ViewBag.AgreementId = id;
            var model = _service.GetAgreementConditions(id, date);
            return PartialView("Details/_AgreementConditionData", model);
        }

        public PartialViewResult AgreementConditionPanel(Guid id)
        {
            var model = _service.GetAgreement(id);
            return PartialView("Details/_AgreementConditionPanel", model);
        }

        public PartialViewResult AgreementSupplementaryData(Guid id, bool? onlyActive)
        {
            ViewBag.AgreementId = id;
            ViewBag.OnlyActive = onlyActive;
            var model = _service.GetSupplementaryAgreementList(id, onlyActive);
            return PartialView("Details/_AgreementSupplementaryData", model);
        }

        public PartialViewResult AgreementSupplementaryPanel(Guid id)
        {
            var model = _service.GetAgreement(id);
            return PartialView("Details/_AgreementSupplementaryPanel", model);
        }

        [HttpGet]
        public ActionResult AgrDetails(Guid id)
        {
            return Json(_service.GetAgreement(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string manager, string template, bool active)
        {
            var filteredList = new List<AgreementList>();
            var models = _service.GetAgreementsByParametrs(manager, template, active);

            if (models != null && models.Count > 0)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                {
                    filteredList.AddRange(from element in models
                                          let ee =
                                              new ExpressionEvaluator(TypeDescriptor.GetProperties(element),
                                                  CriteriaOperator.Parse(filterExpression))
                                          where (bool)ee.Evaluate(element)
                                          select element);
                }
                else
                {
                    filteredList = models;
                }

                if (filteredList.Count == 0)
                    return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);


                string handle = Guid.NewGuid().ToString();

                using (var memoryStream = new MemoryStream())
                {
                    GridViewExtension.WriteXlsx(AgreementExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт договоров {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SupplemetaryExportTo(string filterExpression, Guid agrId, bool active)
        {
            var filteredList = new List<SupplementaryAgreementList>();
            var models = _service.GetSupplementaryAgreementList(agrId, active); ;

            if (models != null && models.Count > 0)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                {
                    filteredList.AddRange(from element in models
                                          let ee =
                                              new ExpressionEvaluator(TypeDescriptor.GetProperties(element),
                                                  CriteriaOperator.Parse(filterExpression))
                                          where (bool)ee.Evaluate(element)
                                          select element);
                }
                else
                {
                    filteredList = models;
                }

                if (filteredList.Count == 0)
                    return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);


                string handle = Guid.NewGuid().ToString();

                using (var memoryStream = new MemoryStream())
                {
                    GridViewExtension.WriteXlsx(SupplementaryAgreementExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт доп соглашений {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.AgreementDelete)]
        public JsonResult DeleteAgreements(Guid[] keys)
        {
            _service.DeleteAgreements(keys);
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.AgreementCreate)]
        public ActionResult Create()
        {
            EditableAgreement editAgreement = new EditableAgreement
            {
                CustomerName = AuthManager.User.CompanyName,
                CustomerCompanyId = AuthManager.User.CompanyId,
                PerformerCompanyId = AuthManager.User.CompanyId
            };
            ViewBag.RootPageDetails = new[] { "Index", "Agreement", "Договоры" };
            ViewBag.Title = "Добавление договора";

            return View("CreateOrEdit", editAgreement);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.AgreementEdit)]
        public ActionResult Edit(Guid id)
        {
            var editAgreement = EditableAgreement.GetAgreement(id);
            if (AuthManager.User.CompanyId != editAgreement.CustomerCompanyId && AuthManager.User.CompanyId != editAgreement.PerformerCompanyId)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            ViewBag.RootPageDetails = new[] { "Index", "Agreement", "Договоры" };
            if (editAgreement.DocDate != null)
                ViewBag.Title = $"Редактирование договора №{editAgreement.DocNumber} от {editAgreement.DocDate.Value.ToShortDateString()}";

            return View("CreateOrEdit", editAgreement);
        }
        
        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.AgreementEdit + "," + AuthPermissions.AgreementCreate)]
        public ActionResult CreateOrEdit(EditableAgreement model)
        {
            if (!ModelState.IsValid)
                return View("CreateOrEdit", model);

            model.CustomerName = EditableAgreement.GetCompanyById(model.CustomerCompanyId).ShortName;
            if (model.PerformerCompanyId.HasValue)
            {
                model.PerformerName = EditableAgreement.GetCompanyById(model.PerformerCompanyId.Value).ShortName;
            }



            if (!string.IsNullOrEmpty(model.CustomerRegNumber))
            {
                model.PerformerRegNumber = model.CustomerRegNumber;
            }
            else
            {
                model.CustomerRegNumber = model.PerformerRegNumber;
            }

            string customerName = $"{model.CustomerRegNumber}//{model.CustomerName}";
            string performerName = $"{model.PerformerRegNumber}//{model.PerformerName}";


            _documentService.DocumentUpdate(model.DocumentId, model.DocNumber, model.CustomerRegNumber,
                model.PerformerRegNumber, model.DocDate, customerName, performerName,
                model.CustomerCompanyId, model.CustomerContactId, model.PerformerCompanyId,
                model.PerformerContactId, model.StatusId, model.TemplateId, model.Note);

            _service.AgreementUpdate(model.DocumentId, model.DateBeg, model.DateEnd, model.DateStop,
                model.CustomerBankId, model.CustomerAddressId, model.PerformerBankId, model.PerformerAddressId);

            ViewBag.MessageSuccess = "Договор успешно добавлен";

            if (model.DocumentId != null)
            {
                var idNotificstion = _notificationService.AddUserNotification(AuthManager.User.UserId, AuthManager.User.CompanyId, AuthManager.User.UserId, AuthManager.User.CompanyId, "Договор " + model.DocNumber + " был изменен", "agreement|details|", (Guid)model.DocumentId);
                var not = _notificationService.GetUserNotifications(AuthManager.User.UserId, idNotificstion).FirstOrDefault();
                var message = new NotificationVM()
                {
                    Id = not.Id,
                    UserFromId = not.UserFromId,
                    UserToID = not.UserToID,
                    UserFromName = not.UserFromName,
                    UserToName = not.UserToName,
                    CompanyFromName = not.CompanyFromName,
                    CompanyToName = not.CompanyToName,
                    Text = not.Text,
                    CreatedOn = not.CreatedOn.ToString(),
                    ViewDate = not.ViewDate.ToString(),
                    Url = not.Url,
                    ObjectId = not.ObjectId
                };

                var contextHub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                contextHub.Clients.All.DisplayMessage(AuthManager.User.UserId.ToString(), message);
            }


            return RedirectToAction("Index");
        }
        

        public ActionResult CustomerLookUp(EditableAgreement agreement)
        {
            return PartialView("Lookup/CustomerCompanyLookup", agreement);
        }

        public ActionResult PerformerLookUp(EditableAgreement agreement)
        {
            return PartialView("Lookup/PerformerCompanyLookup", agreement);
        }



        #region signature

        [HttpGet]
        public ActionResult Download(string fileGuid, string fileName)
        {
            if (!string.IsNullOrEmpty(fileGuid) && !string.IsNullOrEmpty(fileName) && TempData[fileGuid] != null)
            {
                var data = (byte[])TempData[fileGuid];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return Json(new { status = "error", message = "Ошибка создания файла." });
            }
        }

        [HttpGet]
        public ActionResult DownloadReport(Guid id)
        {
            var agreement = _service.GetAgreement(id);

            if (agreement.PerformerCompanyId != AuthManager.User.CompanyId &&
                agreement.CustomerCompanyId != AuthManager.User.CompanyId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (agreement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                AgreementReport report = new AgreementReport(agreement, AuthManager.User);
                string tempFolder = HttpRuntime.CodegenDir;
                string tempWordFile = $"{tempFolder}\\{Guid.NewGuid()}.docx";
                report.CreatePackage(tempWordFile);

                // Нужно как-то удалять временный файл, потому что будет захламляться место

                // Тут я пытался сделать через стримы, но я уже не помню почему не получилось
                //var fileStream = new FileStream(tempWordFile, FileMode.Open);
                //var downloadStream = new MemoryStream();
                //fileStream.CopyTo(downloadStream);
                //fileStream.Close();

                return File(tempWordFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{agreement.DocumentId}.docx");
            }
        }

        [HttpGet]
        public JsonResult ConvertAgreementToBase64(Guid id)
        {
            // Подпись всё равно почему-то не проверяется в cryptcp

            //var agreement = _service.GetAgreement(id);
            //if (agreement.PerformerCompanyId != AuthManager.User.CompanyId &&
            //    agreement.CustomerCompanyId != AuthManager.User.CompanyId)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "У пользователя нет доступа к данному договору."
            //    }, JsonRequestBehavior.AllowGet);
            //}
            //if (agreement == null)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Данный договор не существует."
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //AgreementReport report = new AgreementReport(agreement, AuthManager.User);
            //string tempFolder = HttpRuntime.CodegenDir;
            //string newGuid = Guid.NewGuid().ToString();
            //string tempWordFile = $"{tempFolder}\\{newGuid}.docx";
            //report.CreatePackage(tempWordFile);

            using (var context = new ProcData())
            {
                //получили файл
                var docAttach = context.DocumentAttachGet(id).ToList().FirstOrDefault();
                string tempFolder = HttpRuntime.CodegenDir;
                string tempWordFile = $"{tempFolder}\\{docAttach.FName}";
                //создали файл на сервере
                using (FileStream fs = new FileStream($"{tempFolder}\\{docAttach.FName}", FileMode.OpenOrCreate))
                {
                    fs.Write(docAttach.Content, 0, docAttach.Content.Length);
                }
                //конвертировали созданный файл в Base64String
                string base64str =
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(Convert.ToBase64String(System.IO.File.ReadAllBytes(tempWordFile))));
                //удалили файл с сервера
                System.IO.File.Delete(tempWordFile);
                //
                return Json(new
                {
                    success = true,
                    fileContent = base64str,
                    fileName = $"{docAttach.FName}",
                    attachId = docAttach.Id
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult PackSignedAgreement(string fileContent, string fileName, string signedData, string docAttachId)
        {
            if (string.IsNullOrEmpty(fileContent))
            {
                return Json(new
                {
                    success = false,
                    message = "Ошибка при получении исходного файла."
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new
                {
                    success = false,
                    message = "Ошибка при получении названия исходного файла"
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(signedData))
            {
                return Json(new
                {
                    success = false,
                    message = "Ошибка подписания файла."
                }, JsonRequestBehavior.AllowGet);
            }

            using (var context = new ProcData())
            {
                Guid attachId;
                Guid.TryParse(docAttachId, out attachId);
                if (attachId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ошибка. Исходный файл не найден, подпись невозможна."
                    }, JsonRequestBehavior.AllowGet);
                }
                //var z = Convert.FromBase64String(signedData);
                context.DocumentESignatureInsert(attachId, AuthManager.User.CompanyId, signedData, AuthManager.User.UserId);

            }


            string userGuid = Guid.NewGuid().ToString();

            var originalFileBytes = Convert.FromBase64String(fileContent);
            TempData[userGuid + "_orig"] = $"{HttpRuntime.CodegenDir}\\{fileName}"; // Тут хранится путь к файлу
            TempData[userGuid + "_sign"] = signedData;

            return Json(new
            {
                success = true,
                guid = userGuid,
                filecontent = fileContent,
                filename = $"{fileName}",
                signeddata = signedData,
                message = "Договор успешно подписан. Повторное подписание приведет к удалению предыдущей подписи."
            });
        }

        //[HttpGet]
        //public ActionResult DownloadSignedData(string userGuid, string fileName)
        //{
        //    if (!string.IsNullOrEmpty(userGuid) && !string.IsNullOrEmpty(fileName) && (TempData[userGuid + "_sign"] != null) && (TempData[userGuid + "_orig"] != null))
        //    {
        //        string origFile = (string)TempData[userGuid + "_orig"];
        //        string signedData = (string)TempData[userGuid + "_sign"];

        //        string tempFolder = HttpRuntime.CodegenDir;
        //        string zipFile = $"{tempFolder}\\{Guid.NewGuid()}.zip";
        //        string guidOrigFile = Guid.NewGuid().ToString();
        //        string tempWordFile = origFile;
        //        string tempSignWordFile = $"{tempFolder}\\{Path.GetFileNameWithoutExtension(origFile)}.docx.sig";

        //        System.IO.File.WriteAllBytes(tempSignWordFile, Encoding.UTF8.GetBytes(signedData));

        //        using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
        //        {
        //            archive.CreateEntryFromFile(origFile, System.IO.Path.GetFileName(origFile));

        //            archive.CreateEntryFromFile(tempSignWordFile, System.IO.Path.GetFileName(tempSignWordFile));
        //        }

        //        System.IO.File.Delete(origFile);
        //        System.IO.File.Delete(tempSignWordFile);

        //        return File(zipFile, "application/zip", System.IO.Path.GetFileName(zipFile));
        //    }
        //    else
        //    {
        //        // Problem - Log the error, generate a blank file,
        //        //           redirect to another controller action - whatever fits with your application
        //        return Json(new { status = "error", message = "Ошибка создания файла." });
        //    }
        //}

        [HttpPost]
        public JsonResult SigningAgreements(Guid[] keys)
        {
            foreach (var key in keys)
            {
                // sign agreements
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
