using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Report;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Главная страница";
            return View();
        }

        //[HttpGet]
        //public ActionResult CreateSign()
        //{
        //    ViewBag.Title = "Done";
        //    using (var context = new ProcData())
        //    {
        //        byte[] fileData;
        //        using (FileStream fs = new FileStream(@"d:\Test3.doc", FileMode.Open))
        //        {
        //            fileData = new byte[fs.Length];
        //            fs.Read(fileData, 0, fileData.Length);
        //            context.DocumentAttachInsert(Guid.Parse("18fe2f47-db83-4f3a-88ca-0321148c4a7e"), "Test3.doc", fileData, AuthManager.User.UserId);

        //        }

        //        //        byte[] fileData1;
        //        //        using (FileStream fs1 = new FileStream(@"d:\Test.xls", FileMode.Open))
        //        //        {
        //        //            fileData1 = new byte[fs1.Length];
        //        //            fs1.Read(fileData1, 0, fileData1.Length);
        //        //            context.DocumentAttachInsert(Guid.Parse("add665a5-ce6f-4152-be0f-00a36e8baee6"), "Test.xls", fileData1, AuthManager.User.UserId);

        //        //        }



        //        //        byte[] fileData2;
        //        //        using (FileStream fs2 = new FileStream(@"d:\Test2.xlsx", FileMode.Open))
        //        //        {
        //        //            fileData2 = new byte[fs2.Length];
        //        //            fs2.Read(fileData2, 0, fileData2.Length);
        //        //            context.DocumentAttachInsert(Guid.Parse("f1f87b26-fbc1-4aa3-acd5-00c7e573d8ab"), "Test2.xlsx", fileData2, AuthManager.User.UserId);

        //        //        }



        //        //byte[] fileData3;
        //        //using (FileStream fs3 = new FileStream(@"d:\Test5.pdf", FileMode.Open))
        //        //{
        //        //    fileData3 = new byte[fs3.Length];
        //        //    fs3.Read(fileData3, 0, fileData3.Length);
        //        //    context.DocumentAttachInsert(Guid.Parse("c39ec25f-eb77-4315-90ef-02ada5345235"), "Test5.pdf", fileData3, AuthManager.User.UserId);

        //        //}
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}