using ASUVP.Core.Configuration;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Services;
using DevExpress.Web;
using DevExpress.Web.Mvc;


namespace ASUVP.Online.Web.ToExcelSettings
{
    public class InstructionExcelSettings
    {
        public static GridViewSettings GetGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "instructionView";
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = "Instruction.xlsx";

            // Export-specific settings  

            settings.KeyFieldName = nameof(InstructionList.Id);
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.DocNumber);
                column.Caption = "Номер";
                column.Width = 120;
                column.ToolTip = "Номер инструкции";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.DocDate);
                column.Caption = "Дата";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.PropertiesEdit.DisplayFormatString = ConfigManager.ShortDateTimeFormat;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                column.ToolTip = "Дата инструкции";
                column.Width = 160;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.DocName);
                column.Caption = "Наименование";
                column.Width = 300;
                column.ToolTip = "Наименование инструкции";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.AgreementName);
                column.Caption = "Договор";
                column.Width = 200;
                column.ToolTip = "Договор";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.ClaimName);
                column.Caption = "Заявка";
                column.Width = 200;
                column.ToolTip = "Заявка";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.CustomerCompanyName);
                column.Caption = "Клиент";
                column.Width = 150;
                column.ToolTip = "Наименование Клиента";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.PerformerCompanyName);
                column.Caption = "Экспедитор";
                column.Width = 150;
                column.ToolTip = "Наименование Экспедитора";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.AgrManagerName);
                column.Caption = "Менеджер";
                column.Width = 120;
                column.ToolTip = "Ответственный менеджер по Договору";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.ApprovalInstructionHint);
                column.Caption = "Согл К";
                column.Width = 110;
                //column.SetDataItemTemplateContent(c =>
                //{
                //    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(InstructionList.ApprovalInstructionImgPath))}");
                //    string imageHint = "";
                //    try
                //    {
                //        imageHint = DataBinder.Eval(c.DataItem, nameof(InstructionList.ApprovalInstructionHint)).ToString();
                //        ViewContext.Writer.Write($"<img style=\"float:none; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                //    }
                //    catch { }
                //});
                column.ToolTip = "Статус согласования инструкции";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = nameof(InstructionList.SigningInstructionHint);
                column.Caption = "ЭП Э";
                column.Width = 110;
                //column.SetDataItemTemplateContent(c =>
                //{
                //    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{DataBinder.Eval(c.DataItem, nameof(InstructionList.SingingInstructionImgPath))}");
                //    string imageHint = "";
                //    try
                //    {
                //        imageHint = DataBinder.Eval(c.DataItem, nameof(InstructionList.SigningInstructionHint)).ToString();
                //        ViewContext.Writer.Write($"<img style=\"float:none; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                //    }
                //    catch { }
                //});
                column.ToolTip = "Статус подписания ЭП Инстркуции";
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = nameof(InstructionList.ApprovalInstructionImgId);
            //    column.Caption = "Согл И";
            //    column.Width = 110;
                //column.SetDataItemTemplateContent(c =>
                //{
                //    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(InstructionList.ApprovalInstructionImgId))).ImgPath}");
                //    string imageHint = "";
                //    try
                //    {
                //        imageHint = StatusManager.GetApprovalStatus((int)DataBinder.Eval(c.DataItem, nameof(InstructionList.ApprovalInstructionImgId))).StatusName;
                //        ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                //    }
                //    catch { }
                //});
            //    column.ToolTip = "Статус согласования инструкции";
            //});

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = nameof(InstructionList.SigningInstructionImgId);
            //    column.Caption = "ЭП И";
            //    column.Width = 110;
                //column.SetDataItemTemplateContent(c =>
                //{
                //    string imageUrl = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~{StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(InstructionList.SigningInstructionImgId))).ImgPath}");
                //    string imageHint = "";
                //    try
                //    {
                //        imageHint = StatusManager.GetSigningStatus((int)DataBinder.Eval(c.DataItem, nameof(InstructionList.SigningInstructionImgId))).StatusName;
                //        ViewContext.Writer.Write($"<img style=\"float:none !important; display: block; margin-left: auto; margin-right: auto;\" width=\"32\" height=\"32\" alt=\"Нет данных\" src=\"{imageUrl}\" title=\"{imageHint}\" />");
                //    }
                //    catch { }
                //});
            //    column.ToolTip = "Статус подписания ЭП Инстркуции";
            //});
            return settings;
        }
    }
}