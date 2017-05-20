using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class ClaimExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "ClaimView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Claim.xlsx";


            settings.KeyFieldName = nameof(ClaimList.Id);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.RegNumber);
                column.Caption = "№ заявки";
                column.Width = 160;
                column.ToolTip = "Номер заявки";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.DocDate);
                column.Caption = "Дата";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 100;
                column.ToolTip = "Дата подачи Заявки";

            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.AgreementName);
                column.Caption = "Договор";
                column.Width = 220;
                column.ToolTip = "Договор с Клиентом";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.DateBeg);
                column.Caption = "Погрузка С";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 140;
                column.ToolTip = "Дата начала погркзки";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.DateEnd);
                column.Caption = "Погрузка ПО";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 140;
                column.ToolTip = "Дата завершения погрузки";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.StFromName);
                column.Caption = "Ст.отправления";
                column.Width = 220;
                column.ToolTip = "Наименование станции отправления (погрузки)";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.StToName);
                column.Caption = "Ст.Назначения";
                column.Width = 220;
                column.ToolTip = "Наименование станции назначения (выгрузки)";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.CarCount);
                column.Caption = "Вагонов";
                column.Width = 100;
                column.ToolTip = "Количество вагонов по Заявке";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.FrWeight);
                column.Caption = "Вес (т)";
                column.Width = 120;
                column.ToolTip = "Общий вес груза по Заявке";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.FrETSNGName);
                column.Caption = "Груз";
                column.Width = 120;
                column.ToolTip = "Наиманование груза ЕТСНГ";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.TelegrammList);
                column.Caption = "Телеграмма ЭТРАН";
                column.Width = 220;
                column.ToolTip = "Перечень выпущенных телеграмм";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.ContactFIO);
                column.Caption = "Менеджер";
                column.Width = 220;
                column.ToolTip = "Менеджер по Договору";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.ApprovalPerformerHint);
                column.Caption = "Согл Э";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(ClaimList.ApprovalPerformerImgPath))}");
                    string imageHint = "";
                    try
                    {
                        imageHint = DataBinder.Eval(c.DataItem, nameof(ClaimList.ApprovalPerformerHint)).ToString();
                        //ViewContext.Writer.Write($"<img style=\"display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.ApprovalCustomerHint);
                column.Caption = "Согл К";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(ClaimList.ApprovalCustomerImgPath))}");
                    string imageHint = "";
                    try
                    {
                        imageHint = DataBinder.Eval(c.DataItem, nameof(ClaimList.ApprovalCustomerHint)).ToString();
                        //ViewContext.Writer.Write($"<img style=\"display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.SigningPerformerHint);
                column.Caption = "ЭП Э";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(ClaimList.SigningPerformerImgPath))}");
                    string imageHint = "";
                    try
                    {
                        imageHint = DataBinder.Eval(c.DataItem, nameof(ClaimList.SigningPerformerHint)).ToString();
                        //ViewContext.Writer.Write($"<img style=\"display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ClaimList.SigningCustomerHint);
                column.Caption = "ЭП К";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(ClaimList.SigningCustomerImgPath))}");
                    string imageHint = "";
                    try
                    {
                        imageHint = DataBinder.Eval(c.DataItem, nameof(ClaimList.SigningCustomerHint)).ToString();
                        //ViewContext.Writer.Write($"<img style=\"display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
            });


            return settings;
        }

    }
}