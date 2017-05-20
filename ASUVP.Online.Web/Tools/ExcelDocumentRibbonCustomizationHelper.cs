using System;
using System.Collections;
using System.Linq;
using DevExpress.Spreadsheet;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Web;

namespace ASUVP.Online.Web.Tools
{
    public static class ExcelDocumentRibbonCustomizationHelper
    {
        public static RibbonTab GetCustomRibbonTab()
        {
            RibbonTab ribbonTab = new RibbonTab("Главная");
            ribbonTab.Groups.AddRange(new RibbonGroup[] { GetFileCommonGroup(), GetUndoGroup(), GetViewGroup()/*, GetCustomRibbonGroup()*/ });
            return ribbonTab;
        }

        static SRFileCommonGroup GetFileCommonGroup()
        {
            SRFileCommonGroup fileCommonGroup = new SRFileCommonGroup();
            var saveAs = new SRFileSaveAsCommand();
            saveAs.Text = "Скачать";
            saveAs.ToolTip = "Скачать документ";

            fileCommonGroup.Items.AddRange(new RibbonItemBase[] { saveAs, new SRFilePrintCommand() });
            return fileCommonGroup;
        }

        static SRUndoGroup GetUndoGroup()
        {
            SRUndoGroup undoGroup = new SRUndoGroup();
            undoGroup.Items.AddRange(new RibbonItemBase[] { new SRFileUndoCommand(), new SRFileRedoCommand() });
            return undoGroup;
        }

        static SRViewGroup GetViewGroup()
        {
            SRViewGroup viewGroup = new SRViewGroup();
            viewGroup.Items.Add(new SRFullScreenCommand());
            return viewGroup;
        }

        //static RibbonGroup GetCustomRibbonGroup()
        //{
        //    RibbonGroup ribbonGroup = new RibbonGroup();
        //    RibbonDropDownButtonItem dropDownItem = CreateButtonItem<RibbonDropDownButtonItem>("CustomDropDownToggleButtons", "Custom DropDown Toggle Buttons", "setup_properties_32x32");
        //    dropDownItem.Items.AddRange(new RibbonDropDownButtonItem[] {
        //    CreateButtonItem<RibbonDropDownToggleButtonItem>("DropDownToggleButton1", "DropDown Toggle Button 1", RibbonItemSize.Small, ""),
        //    CreateButtonItem<RibbonDropDownToggleButtonItem>("DropDownToggleButton2", "DropDown Toggle Button 2", RibbonItemSize.Small, "content_checkbox_16x16")
        //});
        //    ribbonGroup.Items.AddRange(new RibbonItemBase[] {
        //    CreateButtonItem<RibbonOptionButtonItem>("CustomOption", "Custom option", "content_notes_32x32"),
        //    CreateButtonItem<RibbonButtonItem>("CustomButton", "Custom button", "grid_cards_32x32"),
        //    dropDownItem
        //});
        //    return ribbonGroup;
        //}

        static T CreateButtonItem<T>(string name, string text, string iconID) where T : RibbonButtonItem
        {
            return CreateButtonItem<T>(name, text, RibbonItemSize.Large, iconID);
        }

        static T CreateButtonItem<T>(string name, string text, RibbonItemSize size, string iconID) where T : RibbonButtonItem
        {
            var item = Activator.CreateInstance<T>();
            item.Name = name;
            item.Text = text;
            item.Size = size;
            if (size == RibbonItemSize.Large)
                item.LargeImage.IconID = iconID;
            else
                item.SmallImage.IconID = iconID;
            return item;
        }
    }
}