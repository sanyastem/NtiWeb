using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public static class DevExpressFormLayoutSettings
    {
        public static void ApplyFormActions<T>(this FormLayoutSettings<T> settings, string gridViewName) where T : class
        {
            settings.Items.AddGroupItem(group =>
            {
                group.GroupBoxDecoration = GroupBoxDecoration.None;
                group.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                group.ColCount = 2;

                group.Items.Add(i =>
                {
                    i.ShowCaption = DefaultBoolean.False;
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.Button;
                    var button = (ButtonSettings) i.NestedExtensionSettings;
                    button.Name = "update";
                    button.Text = "Сохранить";
                    button.ClientSideEvents.Click = "function (s,e) {" + gridViewName + ".UpdateEdit(); }";
                });

                group.Items.Add(i =>
                {
                    i.ShowCaption = DefaultBoolean.False;
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.Button;
                    var button = (ButtonSettings) i.NestedExtensionSettings;
                    button.Name = "cancel";
                    button.Text = "Отмена";
                    button.ClientSideEvents.Click = "function (s,e) {" + gridViewName + ".CancelEdit();}";
                });
            });
        }
    }
}