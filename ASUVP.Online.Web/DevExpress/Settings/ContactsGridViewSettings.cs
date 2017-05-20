using ASUVP.Core.Web.Dto;
using ASUVP.Core.Web.Security;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class ContactsGridViewSettings
    {
        public static GridViewSettings Configure(string controller)
        {
            var settings = new GridViewSettings {Name = "ContactsGridView"};

            settings.KeyFieldName = "Id";
            settings.ApplyRouting(controller);
            settings.ApplyEditing(new DevExpressGridViewEditingSettings
            {
                FormCaption = "Контакт",
                ControllerName = "Contact",
                CreateButtonVisible = AuthManager.User.IsInRole(AuthPermissions.ContactCreate),
                EditButtonVisible = AuthManager.User.IsInRole(AuthPermissions.ContactEdit),
                DeleteButtonVisible = AuthManager.User.IsInRole(AuthPermissions.ContactDelete)
            });

            settings.ApplyPaging();

            settings.Columns.Add(nameof(ContactDto.LastName), "Фамилия").SortOrder = ColumnSortOrder.Ascending;
            settings.Columns.Add(nameof(ContactDto.FirstName), "Имя");
            settings.Columns.Add(nameof(ContactDto.MiddleName), "Отчество");
            settings.Columns.Add(nameof(ContactDto.Email), "Электронная почта");
            settings.Columns.Add(nameof(ContactDto.Phone), "Телефон");
            settings.Columns.Add("CompanyName", "Фирма");

            settings.ApplyAdvancedLayout();
            settings.ApplyExportToXls(controller);

            return settings;
        }
    }
}