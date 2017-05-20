using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class AccountExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "AccountView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Account.xlsx";

            settings.KeyFieldName = nameof(AccountList.Id);

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.DocNumber);
                column.Caption = "Номер";
                column.Width = 105;
                column.ToolTip = "Номер Счета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.DocDate);
                column.Caption = "Дата ";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 100;
                column.ToolTip = "Дата счета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.DocName);
                column.Caption = "Наименование";
                column.Width = 250;
                column.ToolTip = "Наименование Счета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.ReportPeriod);
                column.Caption = "Отчетный период";
                column.Width = 180;
                column.ToolTip = "Отчетный период Счета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.SummaWithNDS);
                column.Caption = "Сумма с НДС";
                column.Width = 150;
                column.ToolTip = "Сумма счета с учетом НДС";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.SummaNDS);
                column.Caption = "Сумма НДС";
                column.Width = 140;
                column.ToolTip = "Сумма НДС Cчета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.CurrencyName);
                column.Caption = "Валюта";
                column.Width = 150;
                column.ToolTip = "Валюта Cчета";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.AgreementName);
                column.Caption = "Договор";
                column.Width = 250;
                column.ToolTip = "Договор, по которому выставлен Счет";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.ClaimName);
                column.Caption = "Заявка";
                column.Width = 250;
                column.ToolTip = "Заявка, к которой выставлен Счет";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.CustomerCompanyName);
                column.Caption = "Клиент";
                column.Width = 220;
                column.ToolTip = "Наименование Клиента";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.PerformerCompanyName);
                column.Caption = "Экспедитор";
                column.Width = 220;
                column.ToolTip = "Наименование Экспедитора";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.AgrManagerName);
                column.Caption = "Менеджер";
                column.Width = 220;
                column.ToolTip = "Ответственный менеджер по Договору";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.Signer1Name);
                column.Caption = "Подписант 1";
                column.Width = 220;
                column.ToolTip = "Подписант 1 счета (Бухгалтер)";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AccountList.Signer2Name);
                column.Caption = "Подписант 2";
                column.Width = 220;
                column.ToolTip = "Подписант 2 счета (Руководитель)";
            });
            return settings;
        }
    }
}