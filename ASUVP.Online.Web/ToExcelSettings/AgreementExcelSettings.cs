using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class AgreementExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "AgreementView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Agreement.xlsx";

            settings.CallbackRouteValues = new {Controller = "Home", Action = "GridViewPartial"};

            // Export-specific settings
            settings.KeyFieldName = nameof(AgreementList.Id);

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.DocNumber);
                column.Caption = "Номер";
                column.Width = 125;
                column.ToolTip = "Номер договора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.RegNumber);
                column.Caption = "Рег. номер";
                column.Width = 125;
                column.ToolTip = "Регистрационный номер договора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.DocDate);
                column.Caption = "Дата";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 100;
                column.ToolTip = "Дата договора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.Balance);
                column.Caption = "Сальдо";
                column.Width = 120;
                column.ToolTip = "Оперативное сальдо по Договору";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.DocName);
                column.Caption = "Наименование";
                column.Width = 220;
                column.ToolTip = "Наименование Договора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.DateBeg);
                column.Caption = "Дата начала";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 145;
                column.ToolTip = "Дата начала действия Договора";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.DateEnd);
                column.Caption = "Дата окончания";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 160;
                column.ToolTip = "Дата окончания действия Договора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.CustomerCompanyName);
                column.Caption = "Клиент";
                column.Width = 220;
                column.ToolTip = "Клиент по Договору";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.PerformerCompanyName);
                column.Caption = "Экспедитор";
                column.Width = 220;
                column.ToolTip = "Экспедитор по Договору";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.AgrManagerName);
                column.Caption = "Менеджер";
                column.Width = 220;
                column.ToolTip = "Ответственный менеджер по Договору";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.ApprovalPerformerHint);
                column.Caption = "Согл Э";
                column.Width = 110;
                column.ToolTip = "Статус согласования Экспедитором";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.ApprovalCustomerHint);
                column.Caption = "Согл К";
                column.Width = 110;
                column.ToolTip = "Статус согласования Клиентом";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.SigningPerformerHint);
                column.Caption = "ЭП Э";
                column.Width = 110;
                column.ToolTip = "Статус подписания ЭП Экспедитором";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.SigningCustomerHint);
                column.Caption = "ЭП К";
                column.Width = 110;
                column.ToolTip = "Статус подписания ЭП Клиентом";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(AgreementList.TemplateName);
                column.Caption = "Шаблон";
                column.Width = 250;
                column.ToolTip = "Шаблон Договора";
            });
            return settings;
        }
    }
}