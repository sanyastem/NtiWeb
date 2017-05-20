using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;

namespace ASUVP.Online.Web.Controllers
{
    public class FilterController : Controller
    {
        private readonly IFilterService _service;

        public FilterController(IFilterService service)
        {
            _service = service;
        }

        public ActionResult FilterManager()
        {
            return PartialView("Filter/FilterManager", _service.GetManagers(AuthManager.User.CompanyId));
        }

        public ActionResult FilterTemplate()
        {
            return PartialView("Filter/FilterTemplate", _service.GetTemplates(AuthManager.User.CompanyId));
        }

        public ActionResult FilterAgreement()
        {
            return PartialView("Filter/FilterAgreement", _service.GetAgreements(AuthManager.User.CompanyId));
        }

        public ActionResult FilterSigning()
        {
            return PartialView("Filter/StateOfSigning", _service.GetSigningsState());
        }

        public ActionResult FilterCoordination()
        {
            return PartialView("Filter/StateOfCoordination", _service.GetCoordinationState());
        }

        public ActionResult FilterClient()
        {
            return PartialView("Filter/Client", _service.GetClients());
        }

        public ActionResult FilterReportPeriod()
        {
            return PartialView("Filter/ReportPeriod", _service.GetReportPeriods());
        }

        public ActionResult FilterStation()
        {
            return PartialView("Filter/Station", _service.GetStation());
        }

        public ActionResult InstructionType()
        {
            return PartialView("Filter/InstructionType", _service.GetInstructionType());
        }
    }
}