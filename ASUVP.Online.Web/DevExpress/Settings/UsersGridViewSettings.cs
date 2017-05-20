using ASUVP.Core.Web.Security;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class UsersGridViewSettings
    {
        public static GridViewSettings Configure(string contoroller)
        {
            var settings = new GridViewSettings {Name = "UsersGridView"};

            settings.KeyFieldName = "Id";

            settings.ApplyRouting("User");

            settings.ApplyEditing(new DevExpressGridViewEditingSettings
            {
                ControllerName = "User",
                FormCaption = "Пользователь",
                CreateButtonVisible = AuthManager.User.IsInRole(AuthPermissions.UserCreate),
                EditButtonVisible = AuthManager.User.IsInRole(AuthPermissions.UserEdit),
                DeleteButtonVisible = AuthManager.User.IsInRole(AuthPermissions.UserDelete)
            });

            settings.ApplyPaging();

            settings.Columns.Add("LastName", "Фамилия").SortOrder = ColumnSortOrder.Ascending;
            settings.Columns.Add("FirstName", "Имя");
            settings.Columns.Add("MiddleName", "Отчество");
            settings.Columns.Add("UserName", "Логин");

            settings.ApplyAdvancedLayout();

            settings.ApplyDetailedRows();
            settings.ApplyExportToXls("users");

            return settings;
        }
    }
}