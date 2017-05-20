using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using ASUVP.Core.Web.Security;
using DevExpress.Web;

namespace ASUVP.Online.Web.Tools
{
    public class HtmlEditorBinaryImageAutoUploadOptions
    {
        public static void ProcessUploadedFile(object sender, FileSavingEventArgs args)
        {
            using (Stream fileContent = args.UploadedFile.FileContent)
            {
                using (Image image = Image.FromStream(fileContent))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    image.Save(memoryStream, image.RawFormat);
                    args.OutputStream = memoryStream;
                    args.FileName =
                        $"{AuthManager.User.CompanyId}_{Guid.NewGuid()}{Path.GetExtension(args.FileName)}";
                }
            }
        }
    }
}