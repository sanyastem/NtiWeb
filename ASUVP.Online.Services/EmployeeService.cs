using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using RestSharp;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IEmployeeService
    {
        //Task<RestResponse> UpdateEmployee(EmployeeDto employee);
        //EmployeeDto GetEmployee(Guid id);
        void EmployeeInsert(Guid userId, Guid companyId);
        void EmployeeDelete(Guid userId, Guid companyId);
        void EmployeeRoleInsert(Guid companyId, Guid userId, Guid roleId);
        void EmployeeRoleDelete(Guid employeeId, Guid roleId);
        List<EmployeeRolesList> EmployeeRolesGet(Guid companyId, Guid userId);
    }

    public class EmployeeService : BaseHttpService, IEmployeeService
    {
        public EmployeeService(IEventLogger logger) : base(logger)
        {
        }

        public void EmployeeInsert(Guid userId, Guid companyId)
        {
            using (var context = new ProcData())
            {
                context.EmployeeInsert(userId, companyId, AuthManager.User.UserId);
            }
        }

        public void EmployeeDelete(Guid userId, Guid companyId)
        {
            using (var context = new ProcData())
            {
                context.EmployeeDelete(userId, companyId, AuthManager.User.UserId);
            }
        }

        public void EmployeeRoleInsert(Guid companyId, Guid userId, Guid roleId)
        {
            using (var context = new ProcData())
            {
                context.EmployeeRoleInsert(companyId, userId, roleId, AuthManager.User.UserId);
            }
        }

        public void EmployeeRoleDelete(Guid employeeId, Guid roleId)
        {
            using (var context = new ProcData())
            {
                context.EmployeeRoleDelete(employeeId, roleId, AuthManager.User.UserId);
            }
        }

        public List<EmployeeRolesList> EmployeeRolesGet(Guid companyId, Guid userId)
        {
            using (var context = new ProcData())
            {
                return context.EmployeeRolesListGet(companyId, userId).ToList();
            }
        }

        //public async Task<RestResponse> UpdateEmployee(EmployeeDto employee)
        //{
        //    var request = new RestRequest("employee", Method.PUT)
        //    {
        //        RequestFormat = DataFormat.Json
        //    };

        //    request.AddBody(employee);

        //    return await ExecuteAsync<RestResponse>(request);
        //}

        //public EmployeeDto GetEmployee(Guid id)
        //{
        //    var request = new RestRequest("employee", Method.GET);
        //    request.AddParameter("id", id);

        //    return Execute<EmployeeDto>(request);
        //}

        //public async Task<RestResponse> AddEmployee(EmployeeDto employee)
        //{
        //    var request = new RestRequest("employee", Method.POST)
        //    {
        //        RequestFormat = DataFormat.Json
        //    };

        //    request.AddBody(employee);

        //    return await ExecuteAsync<RestResponse>(request);
        //}

        //public async Task<RestResponse> DeleteEmployee(Guid id)
        //{
        //    var request = new RestRequest("employee", Method.DELETE)
        //    {
        //        RequestFormat = DataFormat.Json
        //    };

        //    request.AddParameter("id", id);

        //    return await ExecuteAsync<RestResponse>(request);
        //}
    }
}