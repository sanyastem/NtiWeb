using System;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress.Settings
{
    public static class EmployeeRolesGridViewSettings
    {
        public static GridViewSettings Configure(Guid employeeId)
        {
            var settings = new GridViewSettings {Name = $"EmployeeRolesGridView_{employeeId}"};
            settings.SettingsDetail.MasterGridName = $"UserEmployeesGridView";

            settings.KeyFieldName = "Id";

            settings.CallbackRouteValues = new
            {
                Controller = "Employee",
                Action = "EmployeeRolesGridView",
                EmployeeId = employeeId
            };

            settings.Columns.Add("Name", "Название").SortOrder = ColumnSortOrder.Ascending;

            settings.ApplyBasicLayout();

            return settings;
        }
    }
}