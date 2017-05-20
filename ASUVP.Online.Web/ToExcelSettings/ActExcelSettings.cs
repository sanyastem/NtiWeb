using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Web.Tools;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.ToExcelSettings
{
    public class ActExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "ActView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Act.xlsx";

            settings.KeyFieldName = nameof(ActList.Id);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.DocNumber);
                column.Caption = "Номер";
                column.ToolTip = "Номер Акта";
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.DocDate);
                column.Caption = "Дата";
                column.ToolTip = "Дата акта";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.DocName);
                column.Caption = "Наименование";
                column.ToolTip = "Наименование Акта";
                column.Width = 350;
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = nameof(ActList.DocReportPeriod);
            //    column.Caption = "Отчетный период";
            //    column.ToolTip = "Отчетный период Акта";
            //    column.Width = 150;
            //});

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.SummaWithNDS);
                column.Caption = "Сумма с НДС";
                column.ToolTip = "Сумма Акта с учетом НДС";
                column.Width = 150;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.SummaNDS);
                column.Caption = "Сумма НДС";
                column.ToolTip = "Сумма НДС Акта";
                column.Width = 150;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.CurrencyName);
                column.Caption = "Валюта";
                column.ToolTip = "Валюта Акта";
                column.Width = 200;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.AgreementName);
                column.Caption = "Договор";
                column.ToolTip = "Договор, к которому создан Акт";
                column.Width = 250;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.CustomerCompanyName);
                column.Caption = "Клиент";
                column.ToolTip = "Клиент по Договору Акта";
                column.Width = 220;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.PerformerCompanyName);
                column.Caption = "Экспедитор";
                column.ToolTip = "Экспедитор по Договору Акта";
                column.Width = 220;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.AgrManagerName);
                column.Caption = "Менеджер";
                column.ToolTip = "Ответственный менеджер по Договору";
                column.Width = 220;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.ApprovalPerformerImgId);
                column.Caption = "Согл Э";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.ApprovalPerformerImgId))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.ApprovalPerformerImgId))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус согласования Акта Экспедитором";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.ApprovalCustomerImgId);
                column.Caption = "Согл К";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.ApprovalCustomerImgId))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.ApprovalCustomerImgId))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус согласования Акта Клиентом";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.SigningPerformerImgId);
                column.Caption = "ЭП Э";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.SigningPerformerImgId))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.SigningPerformerImgId))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус подписания ЭП Акта Экспедитором";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.SigningCustomerImgId);
                column.Caption = "ЭП К";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.SigningCustomerImgId))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ActList.SigningCustomerImgId))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус подписания ЭП Акта Клиентом";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ActList.TemplateName);
                column.Caption = "Шаблон";
                column.ToolTip = "Шаблон Акта";
                column.Width = 250;
            });

            return settings;
        }
    }
}