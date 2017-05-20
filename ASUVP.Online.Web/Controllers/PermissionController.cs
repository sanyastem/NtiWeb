using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASUVP.Online.OData;

namespace ASUVP.Online.Web.Controllers
{
    public class PermissionController : BaseController
    {
        private readonly IODataClient _context;

        public PermissionController(IODataClient context)
        {
            _context = context;
        }

        public ActionResult PermissionsGridLookup(ICollection<Guid> selected)
        {
            ViewBag.Permissions = _context.Permissions();
            return PartialView(selected);
        }
    }
}