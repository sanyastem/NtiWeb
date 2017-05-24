using ASUVP.Online.Services;
using System;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Web.Models;

namespace ASUVP.Online.Web.Controllers
{
    public class MailController : BaseController
    {
        private readonly IMailService _service;
        private readonly IAgreementService _agree;
        private readonly IProtocolService _protocol;
        private readonly IAccountService _account;
        private readonly IClaimService _claim;
        private readonly IActService _act;
        private readonly IInstructionService _inst;
        public MailController(IMailService service, IAgreementService agree = null, IProtocolService protocol = null,
            IAccountService account = null, IClaimService claim = null, IActService act = null, IInstructionService inst = null)
        {
            _service = service;
            _agree = agree;
            _protocol = protocol;
            _account = account;
            _claim = claim;
            _act = act;
            _inst = inst;
        }

        // GET: Mail
        public ActionResult Index()
        {
            ViewBag.Title = "Почта";
            var sentMailsVm = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return View(sentMailsVm);
        }

        public ActionResult SentMailViewPartial()
        {
            var model = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return PartialView("_SentMailViewPartial", model);
        }

        public ActionResult ReceivedMailViewPartial()
        {
            var model = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return PartialView("_ReceivedMailTabsView", model);
        }

        [HttpGet]
        public ActionResult Details(Guid? id)
        {
            ViewBag.Title = "Просмотр сообщения";
            ViewBag.ID = id;
            var mail = _service.GetMailDetails(id);

            // Обновляем письмо, если оно не было прочитано
            //  происходит только при первом открытии

            bool isUnreaded = false;
            if (mail.ReceiverCompanyId == AuthManager.User.CompanyId)
            {
                
                if (mail.ViewerId == null)
                {
                    isUnreaded = true;
                }
            }

            if (!isUnreaded) return View(mail);

            _service.UpdateUserMailViewTime(id);

            return View(mail);
        }
        public ActionResult AgreementSendMail(Guid id)
        {
            ViewBag.Title = "Отправка договора";
            var model = _agree.GetAgreement(id);
            if (AuthManager.User.CompanyId != model.CustomerCompanyId && AuthManager.User.CompanyId != model.PerformerCompanyId)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            var mail = new NewMailVM { Subject = "Отправка договора", Body = model.TemplateName + "<br>" + model.DocNumber + "<br>" + model.RegNumber + "<br>" + model.DocDate.ToString() + "<br>" + model.StatusName + "<br>" + model.DocName + "<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyName + "<br>" + model.DateBeg.ToString() + "<br>" + model.DateEnd.ToString() + "<br>" + model.DateStop.ToString() + "<br>" + model.Note };
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult ProtocolSendMail(Guid id)
        {
            ViewBag.Title = "Отправка протокола";
            var model = _protocol.GetProtocol(id); 
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            var mail = new NewMailVM { Subject = "Отправка протокола", Body = model.TemplateName + "<br>" + model.DocNumber +  "<br>" + model.DocDate.ToString() + "<br>" + model.DocStatusName + "<br>" + model.DocName + "<br>"+ model.AgreementName +"<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyName + "<br>" + model.DateBeg.ToString() + "<br>" + model.DateEnd.ToString() + "<br>" + model.DateStop.ToString() + "<br>" + model.ClaimName + "<br>" + model.DocNote };
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult AccountSendMail(Guid id)
        {
            ViewBag.Title = "Отправка счета";
            var model = _account.GetAccount(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            var mail = new NewMailVM { Subject = "Отправка счета", Body = model.TemplateName + "<br>" + model.DocNumber + "<br>" + model.DocDate.ToString() + "<br>" + model.DocName + "<br>" + model.AgreementName + "<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyName + "<br>" + model.ClaimName + "<br>" + model.DateBeg.ToString() + "<br>" + model.DateEnd.ToString() + "<br>" + model.RepoptPeriodName + "<br>" + model.SummaWithNDS.ToString() + "<br>" + model.CurrencyName +"<br>"+ model.CurrencyName};
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult ClaimSendMail(Guid id)
        {
            ViewBag.Title = "Отправка заявки";
            var model = _claim.GetClaim(id);
            var modelRoute = _claim.GetClaimRouteDetails(model.StationFromId.ToString(), model.StationToId.ToString());
            var modelDocument = _claim.GetClaimConditions(id);
            var loading = _claim.GetClaimLoadingSchedule(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            string txtMail = "";
            foreach (var item in modelRoute)
            {
                foreach (var doc in modelDocument)
                {
                    

                   
                            txtMail = "Шаблон<br>" + model.TemplateName + "<br> Договоры:<br>" + model.AgreementName + "<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyName
                            + "<br>--------------------<br>" + model.DocNumber + "<br>" + model.RegNumber + "<br>" + model.DocDate + "<br>" + model.DocStatusName + "<br>Погрузка С<br>" + model.DateBeg + "<br>"
                            + model.DateEnd + "<br>" + model.ReportPeriodName + "<br>" + model.PlanType + "<br>Состояние заявки<br>Согласование графика погрузки<br>" + model.ClaimStatusName + "<br>Согласование графика погрузки<br>" + model.LoadingScheduleStatusName + "<br>Протокол<br>"
                            + model.ProtocolName + "<br>Подвижной состав<br>Парк контейнеров<br>" + model.ContParkName + "<br>Род контейнеров<br>" + model.ContKindName + "<br>Парк вагонов<br>" + model.CarParkName
                            + "<br>Род вагонов<br>" + model.CarKindName + "<br>Тип вагонов<br>" + model.CarTypeName + "<br>Вид группы<br>" + model.GroupTypeName + "<br>Вид отправки<br>" + model.ShipmentName
                            + "<br>Страна отправления<br>" + model.CountryFromName + "<br>Страна назначения<br>" + model.CountryToName + "<br>Станция отправления<br>" + model.StationFromCode + "<br>" + model.RailWayFromName
                            + "<br>" + model.StationFromName + "<br>Станция назначения<br>" + model.StationToCode + "<br>" + model.RailWayToName + "<br>" + model.StationToName + "<br>Вид сообщения<br>"
                            + model.TransferTypeName + "<br>Тарифное расстояние (км.)<br>" + model.Distance + "<br>Нормативный срок доставки (дней)<br>" + model.PeriodOfDelivery + "<br>Детализация маршрута:" + model.RouteName + "<br>№<br>" + item.Position + "<br>Страна<br>"
                            + item.CountryName + "<br>Станция входа (код)<br>" + item.StationInCode + "<br>Станция входа<br>" + item.StationInName + "<br>Дорога входа<br>" + item.RailWayInName + "<br>Станция выхода (код)<br>" + item.StationOutCode
                            + "<br>Дорога выхода<br>" + item.RailWayOutName + "<br>Расстояние (км)<br>" + item.Distance + "<br>Груз ЕТСНГ<br>" + model.FrETSNGCode + "<br>" + model.FrETSNGName + "<br>Груз ГНГ<br>"
                            + model.FrGNGCode + "<br>" + model.FrGNGName + "<br>Грузоотправитель<br>" + model.LoadFromTGNL + "<br>" + model.LoadFromName + "<br>Грузополучатель<br>" + model.LoadToTGNL + "<br>"
                            + model.LoadToName + "<br>Вес груза (тонн)<br>" + model.FrWeight + "<br>Количество контейнеров (шт.)<br>" + model.ContCount + "Количество вагонов (шт.)" + model.CarCount + "<br>Примечание<br>"
                            + model.ClaimNote + "<br>Дополнительные условия<br>Условие<br>" + doc.Name + "<br>Значение<br>" + doc.ValueString + "<br>Примечание<br>" + doc.ConditionLimitValue + "<br>Группа<br>"
                            + doc.ConditionGroupName + "<br>График погрузки<br>Версия графика<br>" + loading.Version + "<br>Состояние<br>" + loading.StatusName + "<br>Дата договора<br>" + loading.LoadDate
                            + "<br>Тонн<br>" + loading.FrWeight + "<br>Вагонов<br>" + loading.CarCount;
                    
                }
            }
            var mail = new NewMailVM() { Subject = "Отправка заявки", Body = txtMail };
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult ActSendMail(Guid id)
        {
            ViewBag.Title = "Отправка акта";
            var model = _act.GetAct(id);
            var modelService = _act.GetActDetails(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            string txtMail = "";
            foreach (var item in modelService)
            {
                txtMail = "Шаблоны<br>" + model.TemplateName + "<br>Номер<br>" + model.DocNumber + "<br>" + model.DocDate.ToString() + "<br>" + model.DocStatusName + "<br>Наименование<br>" + model.DocName + "<br>Договор<br>" + model.CustomerCompanyLabel + "<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyLabel + "<br>" + model.PerformerCompanyName + "<br>Отчетный период<br>" + model.DateBeg.ToString() + "<br>" + model.DateEnd.ToString() + "<br>" + model.RepoptPeriodName + "<br>Сумма счета<br>" + model.SummaWithNDS.ToString() + "<br>" + model.CurrencyName + "<br>В том числе НДС<br>"
               + model.SummaNDS.ToString() + "<br>Примечание<br>" + model.DocNote + "<br>Услуги<br>" + item.ServiceName + "<br>Ставка НДС<br>" + item.NDS + "<br>Сумма без НДС<br>" + item.Summa + "<br>Сумма НДС<br>"
               + item.SummaNDS + "<br>Сумма с НДС" + item.SummaWithNDS + "<br>Валюта<br>" + item.CurrencyName + "<br>Параметры<br>" + item.ConditionName
           ;
            }
           
            var mail = new NewMailVM
            {
                Subject = "Отправка акта",
                Body = txtMail
            };
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult InstructionSendMail(Guid id)
        {
            ViewBag.Title = "Отправка инструкции";
            var model = _inst.GetInstruction(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };
            var mail = new NewMailVM { Subject = "Отправка инструкции", Body = "Шаблон<br>" + model.TemplateName + "<br>Номер<br>" + model.DocNumber + "<br>Дата<br>" + model.DocDate.ToString() + "<br>" + model.DocStatusName + "<br>Наименование<br>" + model.DocName + "<br>Договор<br>" + model.AgreementName + "<br>" + model.CustomerCompanyLabel + "<br>" + model.CustomerCompanyName + "<br>" + model.PerformerCompanyLabel + "<br>" + model.PerformerCompanyName + "<br>Заявка<br>" + model.ClaimName + "<br>Станция<br>" + model.StationCode + "<br>" + model.RailWayName + "<br>" + model.StationName};
            return View("~/Views/Mail/ComposeMail.cshtml", mail);
        }
        public ActionResult UpdateMail(Guid? id)
        {
            ViewBag.Title = "Редактировать письмо";
            var mail = _service.GetMailDetails(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };

            var model = new NewMailVM { Body = mail.Body, Subject = mail.Subject };

            return View("ComposeMail", model);
        }
        public ActionResult ComposeMail()
        {
            ViewBag.Title = "Написать письмо";
            ViewBag.RootPageDetails = new [] { "Index", "Mail", "Почта" };

            var model = new NewMailVM { Body = "", Subject = "" };

            return View("ComposeMail", model);
        }

        public PartialViewResult HtmlEditor()
        { 
            return PartialView("_HtmlEditor");
        }

        [HttpPost]
        public ActionResult SendMail(NewMailVM newMail)
        {
            var mail = newMail;

            _service.SendMail(mail.Subject, mail.Body, newMail.CompanyId);

            return Redirect($"{Url.RouteUrl(new {Controller = "Mail", Action = "Index"})}#sent");
        }
    }
}
