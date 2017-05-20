using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public static class DevExpressGridViewSettingsExtentions
    {
        private const string GridViewAction = "GridView";
        private const string AddAction = "Add";
        private const string UpdateAction = "Update";
        private const string DeleteAction = "Delete";

        public static void ApplyRouting(this GridViewSettings settings, string controller)
        {
            settings.CallbackRouteValues = new {Controller = controller, Action = GridViewAction};

            settings.CustomBindingRouteValuesCollection.Add(GridViewOperationType.Paging,
                new {Controller = controller, Action = nameof(GridViewOperationType.Paging)});
            settings.CustomBindingRouteValuesCollection.Add(GridViewOperationType.Filtering,
                new {Controller = controller, Action = nameof(GridViewOperationType.Filtering)});
            settings.CustomBindingRouteValuesCollection.Add(GridViewOperationType.Sorting,
                new {Controller = controller, Action = nameof(GridViewOperationType.Sorting)});
            settings.CustomBindingRouteValuesCollection.Add(GridViewOperationType.Grouping,
                new {Controller = controller, Action = nameof(GridViewOperationType.Grouping)});
        }

        public static void ApplyBasicLayout(this GridViewSettings settings)
        {
            settings.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCells;

            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowHeaderFilterButton = true;
            settings.Settings.UseFixedTableLayout = true;

            settings.SettingsBehavior.AllowFocusedRow = true;            
            //CS0618
            //settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Disabled; //ColumnResizeMode.NextColumn;

            settings.SettingsPopup.HeaderFilter.Height = 200;

            settings.Width = Unit.Percentage(100);

            foreach (MVCxGridViewColumn column in settings.Columns)
            {
                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            }
        }

        public static void ApplyAdvancedLayout(this GridViewSettings settings)
        {
            settings.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden; // if is Visible, breaks responsive layout
            settings.Settings.ShowFooter = true;
            settings.Settings.ShowGroupPanel = false;
            settings.Settings.ShowGroupedColumns = true;

            settings.SettingsContextMenu.Enabled = true;
            settings.SettingsSearchPanel.Visible = true;

            settings.ApplyBasicLayout();
        }

        public static void ApplyPaging(this GridViewSettings settings)
        {
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.NumericButtonCount = 5;
            settings.SettingsPager.Position = PagerPosition.Bottom;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new[] {"10", "20", "50"};
        }

        public static void ApplyEditing(this GridViewSettings settings, DevExpressGridViewEditingSettings editing)
        {
            settings.SettingsEditing.AddNewRowRouteValues =
                new {Controller = editing.ControllerName, Action = AddAction};
            settings.SettingsEditing.UpdateRowRouteValues =
                new {Controller = editing.ControllerName, Action = UpdateAction};
            settings.SettingsEditing.DeleteRowRouteValues =
                new {Controller = editing.ControllerName, Action = DeleteAction};

            settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;

            settings.SettingsText.PopupEditFormCaption = editing.FormCaption;

            settings.SettingsPopup.EditForm.Modal = true;
            settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
            settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            settings.SettingsPopup.EditForm.Width = Unit.Pixel(editing.FormWidth);

            settings.CommandColumn.Caption = " "; // check

            settings.CommandColumn.Visible = editing.CreateButtonVisible || editing.EditButtonVisible ||
                                             editing.DeleteButtonVisible;
            settings.CommandColumn.ShowNewButtonInHeader = editing.CreateButtonVisible;
            settings.CommandColumn.ShowEditButton = editing.EditButtonVisible;

            if (editing.DeleteButtonVisible)
            {
                var btnDelete = new GridViewCommandColumnCustomButton {ID = "btnDelete", Text = "Удалить"};
                settings.CommandColumn.CustomButtons.Add(btnDelete);
                settings.ClientSideEvents.CustomButtonClick = "function(s, e) { OnCustomButtonClick(s, e); }";
            }

            settings.CommandColumn.Width = Unit.Pixel(200);
        }

        public static void ApplyExportToXls(this GridViewSettings settings, string filename)
        {
            settings.SettingsExport.ExportSelectedRowsOnly = false;
            settings.SettingsExport.FileName = $"{filename}.xlsx";
        }

        public static void ApplyDetailedRows(this GridViewSettings settings)
        {
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;
            settings.SettingsDetail.ShowDetailRow = true;
        }
    }
}