using ASUVP.Core.Web.Security;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class RolesGridViewSettings
    {
        public static GridViewSettings Configure(string gridViewName)
        {
            var settings = new GridViewSettings {Name = "RolesGridView"};

            settings.KeyFieldName = "Id";

            settings.Columns.Add("Name", "Название").SortOrder = ColumnSortOrder.Ascending;

            settings.ApplyEditing(new DevExpressGridViewEditingSettings
            {
                ControllerName = "Role",
                FormCaption = "Роль",
                CreateButtonVisible = AuthManager.User.IsInRole(AuthPermissions.RoleCreate),
                EditButtonVisible = AuthManager.User.IsInRole(AuthPermissions.RoleEdit),
                DeleteButtonVisible = AuthManager.User.IsInRole(AuthPermissions.RoleDelete)
            });

            settings.ApplyAdvancedLayout();
            settings.ApplyRouting(gridViewName);
            settings.ApplyPaging();
            settings.ApplyExportToXls(gridViewName);
            settings.ApplyDetailedRows();

            return settings;
        }
    }
}