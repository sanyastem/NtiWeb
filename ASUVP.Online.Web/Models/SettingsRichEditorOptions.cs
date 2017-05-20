using System.ComponentModel.DataAnnotations;
using System.Web;
using DevExpress.Web.ASPxRichEdit;

namespace ASUVP.Online.Web.Models
{
    public class SettingsRichEditorOptions
    {
        const string SettingsRichEditorOptionsKey = "SettingsRichEditorOptions";
        public SettingsRichEditorOptions()
        {
            BehaviorSettings = new ASPxRichEditBehaviorSettings();
            DocumentCapabilitiesSettings = new ASPxRichEditDocumentCapabilitiesSettings();
        }

        public static SettingsRichEditorOptions Current
        {
            get
            {
                if (HttpContext.Current.Session[SettingsRichEditorOptionsKey] == null)
                    HttpContext.Current.Session[SettingsRichEditorOptionsKey] = new SettingsRichEditorOptions();
                return (SettingsRichEditorOptions)HttpContext.Current.Session[SettingsRichEditorOptionsKey];
            }
            set { HttpContext.Current.Session[SettingsRichEditorOptionsKey] = value; }
        }

        [Display(Name = "Behavior")]
        public ASPxRichEditBehaviorSettings BehaviorSettings { get; set; }
        [Display(Name = "Document Capabilities")]
        public ASPxRichEditDocumentCapabilitiesSettings DocumentCapabilitiesSettings { get; set; }
    }
}