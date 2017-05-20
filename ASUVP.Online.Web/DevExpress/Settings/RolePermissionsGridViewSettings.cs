using System;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class RolePermissionsGridViewSettings
    {
        public static GridViewSettings Configure(Guid roleId)
        {
            var settings = new GridViewSettings {Name = $"PermissionsGridView_{roleId}"};

            settings.KeyFieldName = "Id";

            settings.Columns.Add("Name", "Название").SortOrder = ColumnSortOrder.Ascending;
            settings.Columns.Add("Code", "Код");
            settings.Columns.Add("Code2", "Код 2");
            settings.Columns.Add("Module", "Модуль").GroupIndex = 0;

            settings.SettingsDetail.MasterGridName = "RolesGridView";
            settings.CallbackRouteValues = new {Controller = "Role", Action = "RolePermissionsGridView", roleId};

            settings.ApplyBasicLayout();

            return settings;
        }
    }
}