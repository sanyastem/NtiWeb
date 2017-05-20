using System.Web.UI.WebControls;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Web;

namespace ASUVP.Online.Web.Tools
{
    public class DocumentRibbonCustomizationHelper
    {
        public static RibbonTab GetCustomRibbonTab(bool isExtenernalRibbon)
        {
            RibbonTab ribbonTab = new RibbonTab("Главная");
            if (isExtenernalRibbon)
                ribbonTab.Groups.AddRange(new RibbonGroup[] { GetCommonGroup(), GetFontGroup(isExtenernalRibbon), GetViewGroup() });
            else
                ribbonTab.Groups.AddRange(new RibbonGroup[] { GetCommonGroup(), GetUndoGroup(), GetFontGroup(isExtenernalRibbon), GetPagesGroup(), GetViewGroup() });
            return ribbonTab;
        }

        static RERFileCommonGroup GetCommonGroup()
        {
            var commonGroup = new RERFileCommonGroup();
            commonGroup.Items.AddRange(new RibbonItemBase[] {
                new RERSaveAsCommand(RibbonItemSize.Large) { Text = "Скачать", ToolTip= "Ctrl + D" },
                new RERPrintCommand(RibbonItemSize.Large) { Text = "Печать", ToolTip = "Ctrl + P" }
            });
            return commonGroup;
        }
        static RERUndoGroup GetUndoGroup()
        {
            var undoGroup = new RERUndoGroup();
            undoGroup.Items.AddRange(new RibbonItemBase[] {
                new RERUndoCommand(RibbonItemSize.Large) { Text = "Отменить", ToolTip = "Ctrl + Z" },
                new RERRedoCommand(RibbonItemSize.Large) { Text = "Вернуть", ToolTip = "Ctrl + Y" }
            });
            return undoGroup;
        }
        static RERFontGroup GetFontGroup(bool isExtenernalRibbon)
        {
            var fontGroup = new RERFontGroup() { ShowDialogBoxLauncher = isExtenernalRibbon };
            fontGroup.Items.AddRange(new RibbonItemBase[] {
                PrepareComboBoxCommand(new RERFontNameCommand()),
                PrepareComboBoxCommand(new RERFontSizeCommand()),
                new RERFontBoldCommand(RibbonItemSize.Large) { Text = "Жирный", ToolTip = "Ctrl + B" },
                new RERFontItalicCommand(RibbonItemSize.Large) { Text = "Курсив", ToolTip = "Ctrl + I" }
            });
            return fontGroup;
        }
        static RERComboBoxCommandBase PrepareComboBoxCommand(RERComboBoxCommandBase command)
        {
            command.FillItems();
            command.PropertiesComboBox.Width = Unit.Pixel(100);
            return command;
        }
        static RERPagesGroup GetPagesGroup()
        {
            var pagesGroup = new RERPagesGroup();
            pagesGroup.Items.Add(new RibbonItemBase[] {
                new RERPageMarginsCommand(),
                new RERChangeSectionPageOrientationCommand(),
                new RERChangeSectionPaperKindCommand()
            });
            return pagesGroup;
        }
        static RERViewGroup GetViewGroup()
        {
            var viewGroup = new RERViewGroup();
            viewGroup.Items.AddRange(new RibbonItemBase[] {
                new RERToggleShowHorizontalRulerCommand(),
                new RERToggleFullScreenCommand() { ToolTip = "F11" }
            });
            return viewGroup;
        }
    }
}