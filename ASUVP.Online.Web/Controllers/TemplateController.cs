using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult TemplateLookup(Guid? templateId)
        {
            if (templateId == Guid.Empty)
                return PartialView("Lookup/TemplateLookup");
            else
                return PartialView("Lookup/TemplateLookup", templateId);
        }
    }
}