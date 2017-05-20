using ASUVP.Core.DataAccess.Model;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class ContactExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "ContactView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Contact.xlsx";


            settings.KeyFieldName = nameof(ContactList.Id);
            settings.Columns.Add(nameof(ContactList.F), "Фамилия").Width = 300;
            settings.Columns.Add(nameof(ContactList.I), "Имя").Width = 225;
            settings.Columns.Add(nameof(ContactList.O), "Отчество").Width = 225;
            settings.Columns.Add(nameof(ContactList.Email), "Электронная почта").Width = 225;
            settings.Columns.Add(nameof(ContactList.Phone), "Телефон").Width = 225;
            settings.Columns.Add(nameof(ContactList.Company), "Фирма").Width = 225;

            return settings;
        }

    }
}