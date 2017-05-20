using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models
{
    public class RoleVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название роли.")]
        public string Name { get; set; }

        public List<PermissionList> PermissionList { get; set; }
        public List<Guid?> RolePermissionList { get; set; }

        public RoleVM()
        {
            using (var context = new ProcData())
            {
                PermissionList = context.PermissionListGet(null).ToList();
            }
        }


        public static RoleVM GetRole(Guid? id)
        {
            using (var context = new ProcData())
            {
                var user = new Role();
                if (id != null && id != Guid.Empty)
                {
                    user = context.RoleGet(id).FirstOrDefault();
                }
                RoleVM editRole = new RoleVM();
                if (user != null)
                {
                    editRole = new RoleVM();
                    editRole.RolePermissionList = context.PermissionListGet(id).Select(x=>x.Id).ToList();
                    editRole.Id = user.Id;
                    editRole.Name = user.Name;
                }

                return editRole;
            }
        }

    }
}