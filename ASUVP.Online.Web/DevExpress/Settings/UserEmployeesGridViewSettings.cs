using System;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class UserEmployeesGridViewSettings
    {
        public static GridViewSettings Configure(Guid userId)
        {
            var settings = new GridViewSettings
            {
                Name = $"User_{Math.Abs(userId.GetHashCode())}",
                CallbackRouteValues = new {Controller = "Employee", Action = "UserEmployeesGridView", userId}
            };

            settings.KeyFieldName = "Id";

            settings.SettingsDetail.MasterGridName = "UsersGridView";

            settings.ApplyEditing(new DevExpressGridViewEditingSettings
            {
                ControllerName = "Employee",
                FormCaption = "Роли пользователя",
                CreateButtonVisible = false,
                EditButtonVisible = true,
                DeleteButtonVisible = false
            });

            settings.Columns.Add("ShortName", "Название").SortOrder = ColumnSortOrder.Ascending;
            settings.Columns.Add("Note", "Примечание");

            settings.ApplyBasicLayout();
            settings.ApplyDetailedRows();

            return settings;
        }
    }
}