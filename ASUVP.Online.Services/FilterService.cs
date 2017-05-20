using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;

namespace ASUVP.Online.Services
{
    public interface IFilterService
    {
        List<SelectListItem> GetManagers(Guid? companyId);
        List<SelectListItem> GetTemplates(Guid? companyId);
        List<SelectListItem> GetSigningsState();
        List<SelectListItem> GetCoordinationState();
        List<SelectListItem> GetClients();
        List<SelectListItem> GetAgreements(Guid? companyId);
        List<SelectListItem> GetReportPeriods();
        List<SelectListItem> GetStation();
        List<SelectListItem> GetInstructionType();
    }
    public class FilterService : BaseHttpService, IFilterService
    {

        public FilterService(IEventLogger logger) : base(logger)
        {

        }

        public List<SelectListItem> GetManagers(Guid? companyId)
        {
            using (var context = new ProcData())
            {
                var managers =
                    context.EmployeeGet(companyId)
                        .Select(x => new SelectListItem { Text = x.AgrOwnerFIO, Value = x.AgrOwnerId.ToString() })
                        .ToList();
                return managers;
            }
        }

        public List<SelectListItem> GetTemplates(Guid? companyId)
        {
            using (var context = new ProcData())
            {
                var templates =
                context.TemplateGet(companyId)
                    .Select(x => new SelectListItem { Text = x.TemplateName, Value = x.Id.ToString() })
                    .ToList();
                return templates;
            }
        }

        public List<SelectListItem> GetSigningsState()
        {
            using (var context = new ProcData())
            {
                var signing =
                context.SigningStatusListGet()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();
                return signing;
            }
        }

        public List<SelectListItem> GetCoordinationState()
        {
            using (var context = new ProcData())
            {
                var signing =
                context.ApprovalStatusListGet()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();
                return signing;
            }
        }

        public List<SelectListItem> GetAgreements(Guid? companyId)
        {
            using (var context = new ProcData())
            {
                var agreements = context.AgreementNumberListGet(companyId);
                var list = agreements.Select(x => new SelectListItem { Text = x.AgreementName, Value = x.AgreementId.ToString() }).ToList();
                return list;
            }
        }

        public List<SelectListItem> GetClients()
        {
            using (var context = new ProcData())
            {
                var clients = context.ClientsGet();
                var list = clients.Select(x => new SelectListItem { Text = x.ShortName, Value = x.Id.ToString() }).ToList();
                return list;
            }
        }

        public List<SelectListItem> GetReportPeriods()
        {
            using (var context = new ProcData())
            {
                var report = context.ReportPeriodListGet();
                var reports = report.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                reports.Insert(0, new SelectListItem() {Text = "Все", Value = "0"});
                return reports;
            }
        }

        public List<SelectListItem> GetStation()
        {
            using (var context = new ProcData())
            {
                var stations = new List<SelectListItem>() { new SelectListItem() { Text = "Все", Value = "0" } };//context.ClientsGet();
                //var list = clients.Select(x => new SelectListItem { Text = x.ShortName, Value = x.Id.ToString() }).ToList();
                return stations;
            }
        }

        public List<SelectListItem> GetInstructionType()
        {
            using (var context = new ProcData())
            {
                var types = new List<SelectListItem>() { new SelectListItem() { Text = "Все", Value = "0" } };//context.ClientsGet();
                //var list = clients.Select(x => new SelectListItem { Text = x.ShortName, Value = x.Id.ToString() }).ToList();
                return types;
            }
        }
    }
}
