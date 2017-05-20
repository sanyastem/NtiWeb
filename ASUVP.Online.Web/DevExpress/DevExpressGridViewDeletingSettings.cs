namespace ASUVP.Online.Web.DevExpress
{
    public class DevExpressGridViewDeletingSettings
    {
        public DevExpressGridViewDeletingSettings(string parentId, string message)
        {
            ParentId = parentId;
            Message = message;
        }

        public string ParentId { get; set; }
        public string Message { get; set; }
    }
}