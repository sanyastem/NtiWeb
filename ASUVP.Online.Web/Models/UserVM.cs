using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Web.Models.Entity;

namespace ASUVP.Online.Web.Models
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо ввести повторный пароль.")]
        [Compare("Password", ErrorMessage = "Пароль и повторный пароль должны совпадать.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле Активный обязательно для заполнения")]
        public bool Active { get; set; }

        public List<Company> Companies { get; set; }
        public List<Guid?> UserCompanies { get; set; }
        public List<ContactList> Contacts { get; set; }

        public UserVM()
        {
            using (var context = new ProcData())
            {
                Companies = context.CompanyGet(null).ToList();
                Contacts = context.ContactListGet().ToList();
            }
        }

        public static UserVM GetUser(Guid? id)
        {
            using (var context = new ProcData())
            {
                var user = new UserGet();
                if (id != null && id != Guid.Empty)
                {
                    user = context.UserGet(id).FirstOrDefault();
                }
                UserVM editUser = new UserVM();
                if (user != null)
                {
                    editUser = new UserVM();
                    if (user.ContactId != null) editUser.ContactId = (Guid) user.ContactId;
                    editUser.UserCompanies = context.UserCompaniesListGet(id).ToList();
                    editUser.Id = user.Id;
                    editUser.Name = user.UserName;
                    editUser.Active = user.IsActive;
                }

                return editUser;
            }
        }

    }
}