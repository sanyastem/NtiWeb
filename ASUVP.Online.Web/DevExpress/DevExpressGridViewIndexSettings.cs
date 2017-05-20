using System.Collections.Generic;

namespace ASUVP.Online.Web.DevExpress
{
    public class DevExpressGridViewIndexSettings
    {
        public DevExpressGridViewIndexSettings()
        {
            Partials = new List<string>();
        }

        public IList<string> Partials { get; set; }
        public bool ShowExportToXlsxButton { get; set; }
        public DevExpressGridViewDeletingSettings DeletingSettings { get; set; }
    }
}