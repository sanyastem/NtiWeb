using System.Web.UI.WebControls;
using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class SupplementaryAgreementExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "SupplementaryAgreement";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "SupplementaryAgreement.xlsx";
            settings.KeyFieldName = nameof(SupplementaryAgreementList.Id);

            settings.Columns.Add(c =>
            {
                c.FieldName = nameof(SupplementaryAgreementList.DocNumber);
                c.Caption = "Номер";
                c.Width = Unit.Pixel(120);
            });

            settings.Columns.Add(c => {
                c.FieldName = nameof(SupplementaryAgreementList.DocDate);
                c.Caption = "Дата";
                c.Width = Unit.Pixel(100);
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
            });
            settings.Columns.Add(nameof(SupplementaryAgreementList.DocName), "Наименование");
            settings.Columns.Add(nameof(SupplementaryAgreementList.DocStatus), "Статус").Width = Unit.Pixel(100);
            settings.Columns.Add(c =>
            {
                c.FieldName = nameof(SupplementaryAgreementList.DateBeg);
                c.Caption = "Действует С";
                c.Width = Unit.Pixel(150);
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = nameof(SupplementaryAgreementList.DateEnd);
                c.Caption = "Действует ПО";
                c.Width = Unit.Pixel(150);
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
            });
            settings.Columns.Add(nameof(SupplementaryAgreementList.TemplateName), "Шаблон");


            return settings;
        }
    }
}