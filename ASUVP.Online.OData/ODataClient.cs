using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using Microsoft.OData.Client;
using System;
using System.Linq;

namespace ASUVP.Online.OData
{
    public interface IODataClient
    {
        IQueryable Roles();
        IQueryable RolePermissions(Guid id);
        IQueryable Permissions();
        IQueryable Agreements();
        IQueryable Contacts();
        IQueryable Companies();
        IQueryable Users();
        IQueryable Employees();
        IQueryable UserEmployees(Guid userId);
        IQueryable EmployeeRoles(Guid employeeId);

    }


    public class ODataClient : IODataClient
    {
        private readonly ProcData _pdContext;
        private readonly Container _container;

        public ODataClient()
        {
            var endpoint = ConfigManager.AppSetting<string>("odata:BaseUrl");

            _container = new Container(new Uri(endpoint));
            _container.BuildingRequest += OnBuildingRequest;

            _pdContext = new ProcData();
        }

        public IQueryable Roles()
        {
            return _container.RoleOData;
        }

        public IQueryable RolePermissions(Guid id)
        {
            return _container.RoleOData.Where(r => r.Id == id).SelectMany(p => p.Permissions);
        }

        public IQueryable Permissions()
        {
            return _container.PermissionOData;
        }

        public IQueryable Agreements()
        {
            return null;
        }

        public IQueryable Contacts()
        {
            return _container.ContactOData;
        }

        public IQueryable Companies()
        {
            return _container.CompanyOData;
        }

        public IQueryable Users()
        {
            return _container.UserOData;
        }

        public IQueryable Employees()
        {
            return _container.EmployeeOData;
        }

        public IQueryable UserEmployees(Guid userId)
        {
            return _container.EmployeeOData.Where(e => e.UserId == userId);
        }

        public IQueryable EmployeeRoles(Guid employeeId)
        {
            return _container.EmployeeOData.Where(e => e.Id == employeeId).SelectMany(e => e.Roles);
        }

        protected void OnBuildingRequest(object sender, BuildingRequestEventArgs e)
        {
            e.Headers.Add("Authorization", $"Bearer {AuthManager.Token ?? string.Empty}");
        }
    }
}