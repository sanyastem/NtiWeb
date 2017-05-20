using System.Web.UI.WebControls;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class RoleExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "RoleView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Role.xlsx";


            settings.KeyFieldName = nameof(Role.Id);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(Role.Name);
                column.Caption = "Название";
                column.ToolTip = "Название роли";
                column.Width = Unit.Percentage(100);
            });

            return settings;
        }

    }
}