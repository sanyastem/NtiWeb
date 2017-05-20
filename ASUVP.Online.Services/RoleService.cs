using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using ASUVP.Core.Web.Security;
using RestSharp;

namespace ASUVP.Online.Services
{
    public interface IRoleService
    {
        List<Role> RolesGet();
        Role RoleGet(Guid? id);
        List<PermissionList> RolePermissionsListGet(Guid? id);
        string RoleInsert(string modelName);
        void RolePermissionsInsert(Guid newRoleId, Guid? permissionId);
        void RoleUpdate(Guid modelId, string modelName);
        void RolePermissionsDelete(Guid modelId, Guid? oldPermissionId);
    }

    public class RoleService : BaseHttpService, IRoleService
    {
        public RoleService(IEventLogger logger) : base(logger)
        {
        }

        public List<Role> RolesGet()
        {
            var roles = new List<Role>();
            using (var context = new ProcData())
            {
                roles = context.RoleGet(null).ToList();
            }
            return roles;
        }
        public Role RoleGet(Guid? id)
        {
            using (var context = new ProcData())
            {
                return context.RoleGet(id).ToList().FirstOrDefault();
            }
        }

        public List<PermissionList> RolePermissionsListGet(Guid? id)
        {
            using (var context = new ProcData())
            {
                return context.PermissionListGet(id).ToList();
            }
        }

        public string RoleInsert(string modelName)
        {
            using (var context = new ProcData())
            {
               return context.RoleInsert(modelName, AuthManager.User.UserId).ToList().FirstOrDefault().ToString();
            }
        }

        public void RolePermissionsInsert(Guid newRoleId, Guid? permissionId)
        {
            using (var context = new ProcData())
            {
                context.RolePermissionInsert(newRoleId, permissionId, AuthManager.User.UserId);
            }
        }

        public void RoleUpdate(Guid modelId, string modelName)
        {
            using (var context = new ProcData())
            {
                context.RoleUpdate(modelId, modelName, AuthManager.User.UserId);
            }
        }

        public void RolePermissionsDelete(Guid modelId, Guid? oldPermissionId)
        {
            using (var context = new ProcData())
            {
                context.RolePermissionDelete(modelId, oldPermissionId, AuthManager.User.UserId);
            }
        }
    }
}