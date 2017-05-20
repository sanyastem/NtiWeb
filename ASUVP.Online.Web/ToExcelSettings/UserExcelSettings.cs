using System.Web.UI.WebControls;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class UserExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "UserView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "User.xlsx";


            settings.KeyFieldName = nameof(UserList.UserId);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(UserList.FIO);
                column.Caption = "ФИО";
                column.ToolTip = "Фамилия Имя Отчество";
                column.Width = Unit.Percentage(50);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(UserList.Login);
                column.Caption = "Логин";
                column.ToolTip = "Логин пользователя";
                column.Width = Unit.Percentage(30);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(UserList.IsActive);
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                column.Caption = "Активен";
                column.ToolTip = "Активен";
                column.Width = Unit.Percentage(10);
            });


            return settings;
        }

    }
}