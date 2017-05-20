using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public static class DevExpressGridLookupSettingsExtensions
    {
        public static void ApplyLayoutSettings(this GridLookupSettings settings)
        {
            settings.CommandColumn.Visible = true;
            settings.CommandColumn.Caption = " ";
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.Width = Unit.Pixel(40);

            settings.GridViewProperties.Settings.ShowFilterRow = true;
            settings.GridViewProperties.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;

            settings.GridViewProperties.SettingsPager.PageSize = 5;
        }
    }
}