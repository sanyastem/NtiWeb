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
    public class ProtocolExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "ProtocolView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Protocol.xlsx";

            // Export-specific settings

            settings.KeyFieldName = nameof(ProtocolList.Id);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.DocNumber);
                column.Caption = "Номер";
                column.Width = 100;
                column.ToolTip = "Номер Протокола";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.DocDate);
                column.Caption = "Дата";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 100;
                column.ToolTip = "Дата Протокола";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.DocName);
                column.Caption = "Наименование";
                column.Width = 250;
                column.ToolTip = "Наименование Протокола";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.DateBeg);
                column.Caption = "Дата начала";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 140;
                column.ToolTip = "Дата начала действия Протокола";

            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.DateEnd);
                column.Caption = "Дата окончания";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.Width = 160;
                column.ToolTip = "Дата окончания действия Протокола";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.AgreementName);
                column.Caption = "Договор";
                column.Width = 220;
                column.ToolTip = "Договор, к которому создан Протокол";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.ClaimName);
                column.Caption = "Заявка";
                column.Width = 200;
                column.ToolTip = "Заявка, к которой создан Протокол";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.CustomerCompanyName);
                column.Caption = "Клиент";
                column.Width = 200;
                column.ToolTip = "Клиент по Протокол";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.PerformerCompanyName);
                column.Caption = "Экспедитор";
                column.Width = 200;
                column.ToolTip = "Экспедитор по Протокол";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.AgrManagerName);
                column.Caption = "Менеджер";
                column.Width = 200;
                column.ToolTip = "Ответственный менеджер по Договору";
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.ApprovalPerformerImgPath);
                column.Caption = "Согл Эк";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.ApprovalPerformerImgPath))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.ApprovalPerformerImgPath))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус согласования Экспедитором";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.ApprovalCustomerImgPath);
                column.Caption = "Согл Кл";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.ApprovalCustomerImgPath))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.ApprovalCustomerImgPath))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус согласования Клиентом";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.SigningPerformerImgPath);
                column.Caption = "ЭП Эк";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.SigningPerformerImgPath))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.SigningPerformerImgPath))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус подписания ЭП Экспедитором";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.SigningCustomerImgPath);
                column.Caption = "ЭП Кл";
                column.Width = 110;
                column.SetDataItemTemplateContent(c =>
                {
                    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.SigningCustomerImgPath))).ImgPath}");
                    string imageHint = "";
                    try
                    {
                        imageHint = StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(ProtocolList.SigningCustomerImgPath))).StatusName;
                        //ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                    }
                    catch { }
                });
                column.ToolTip = "Статус подписания ЭП Клиентом";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(ProtocolList.TemplateName);
                column.Caption = "Шаблон";
                column.Width = 200;
                column.ToolTip = "Шаблон Протокола";
            });
            return settings;
        }
    }
}