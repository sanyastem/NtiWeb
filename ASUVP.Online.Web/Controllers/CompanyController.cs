using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASUVP.Online.OData;
using ASUVP.Online.Services;

namespace ASUVP.Online.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        public ActionResult CompaniesGridLookup(List<Guid?> selected)
        {
            ViewBag.Companies = _service.GetCompanyList();
            return PartialView(selected);
        }

        public ActionResult CompanyGridLookup(Guid? selected)
        {
            ViewBag.Companies = _service.GetCompanyList();
            return PartialView(selected);
        }

        public ActionResult CustomerCompanyGridLookup(Guid? selected)
        {
            ViewBag.Companies = _service.GetCompanyList();
            return PartialView(selected);
        }

        public ActionResult PerformerCompanyGridLookup(Guid? selected)
        {
            ViewBag.Companies = _service.GetCompanyList();
            return PartialView(selected);
        }

        //private readonly IODataClient _context;

        //public CompanyController(IODataClient context)
        //{
        //    _context = context;
        //}

        //public ActionResult CompanyGridLookup(Guid? selected)
        //{
        //    ViewBag.Companies = _context.Companies();
        //    return PartialView(selected);
        //}

        //public ActionResult CompaniesGridLookup(List<Guid?> selected)
        //{
        //    ViewBag.Companies = _context.Companies();
        //    return PartialView(selected);
        //}
    }
}