using ASUVP.Core.DataAccess.Model;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using ASUVP.Online.Web.Models;
using DevExpress.Pdf;
using System.Collections.Generic;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = 
        AuthPermissions.AgreementView + "," + AuthPermissions.AccountView + "," +
        AuthPermissions.ActView + "," + AuthPermissions.ClaimView + "," +
        AuthPermissions.InstructionView + "," + AuthPermissions.ProtocolView)]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _service;
        public const string SESSION_KEY = "PdfFile";

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Preview(Guid? id)
        {
            using (var context = new ProcData())
            {
                DocumentAttach docAttach = context.DocumentAttachGet(id).FirstOrDefault();
                ViewBag.Title = "Вложение";
                return View("Preview", docAttach);
            }
        }
       /* public ActionResult PreviewMail(Guid? id)
        {
            using (var context = new ProcData())
            {
                DocumentAttach docAttach = context.DocumentAttachGet(id).FirstOrDefault();
                ViewBag.Title = "Вложение";
                docAttach.Content = "";
                return View("Preview", docAttach);
            }
        }*/
        public FileResult Download(Guid id)
        {
            using (var context = new ProcData())
            {
                var docAttach = context.DocumentAttachGet(id).ToList().FirstOrDefault();
                if (docAttach != null)
                {
                    //string tempFolder = HttpRuntime.CodegenDir;

                    //string tempWordFile = $"{tempFolder}\\{docAttach.FName}";

                    //using (FileStream fs = new FileStream($"{tempFolder}\\{docAttach.FName}", FileMode.OpenOrCreate))
                    //{
                    //    fs.Write(docAttach.Content, 0, docAttach.Content.Length);
                    //}

                    //byte[] fileBytes = System.IO.File.ReadAllBytes(tempWordFile);

                    //System.IO.File.Delete(tempWordFile);

                    return File(docAttach.Content, System.Net.Mime.MediaTypeNames.Application.Octet, docAttach.FName);
                }
                return null;
            }
        }

        public ActionResult PdfPreview(Guid? id)
        {
            if (id != null)
            {
                byte[] fileContent = null;
                using (var context = new ProcData())
                {
                    DocumentAttach docAttach = context.DocumentAttachGet(id).FirstOrDefault();
                    fileContent = docAttach.Content;
                }
                Session[SESSION_KEY] = fileContent;
                ViewBag.DocumentId = (Guid)id;
            }

            PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor();

            MemoryStream stream = new MemoryStream((byte[])Session[SESSION_KEY]);
            documentProcessor.LoadDocument(stream);

            List<PdfPageModel> model = new List<PdfPageModel>();
            for (int pageNumber = 1; pageNumber <= documentProcessor.Document.Pages.Count; pageNumber++)
            {
                model.Add(new PdfPageModel(documentProcessor)
                {
                    PageNumber = pageNumber
                });
            }

            return PartialView(model);
        }

        public ActionResult PdfPreviewAllPages(Guid? id)
        {
            if (id != null)
            {
                byte[] fileContent = null;
                using (var context = new ProcData())
                {
                    DocumentAttach docAttach = context.DocumentAttachGet(id).FirstOrDefault();
                    fileContent = docAttach.Content;
                }
                Session[SESSION_KEY] = fileContent;
            }

            PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor();

            MemoryStream stream = new MemoryStream((byte[])Session[SESSION_KEY]);
            documentProcessor.LoadDocument(stream);

            List<PdfPageModel> model = new List<PdfPageModel>();
            for (int pageNumber = 1; pageNumber <= documentProcessor.Document.Pages.Count; pageNumber++)
            {
                model.Add(new PdfPageModel(documentProcessor)
                {
                    PageNumber = pageNumber
                });
            }
            return View(model);
        }

        public ActionResult DocumentAttachPopupDetails(Guid id)
        {
            ViewBag.AgrId = id;
            return PartialView("Modal/DocumentAttachPopupDetails", _service.DocumentAttachListGet(id));
        }
    }
}