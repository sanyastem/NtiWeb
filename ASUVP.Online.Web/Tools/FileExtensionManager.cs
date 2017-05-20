using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASUVP.Online.Web.Tools
{
    public static class FileExtensionManager
    {
        public static string GetFilePath(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf('.'));

            switch (fileExt)
            {
                case ".doc":
                    {
                        return "~/Content/img/file_extensions/doc.png";
                    }
                case ".docx":
                    {
                        return "~/Content/img/file_extensions/docx.png";
                    }
                case ".xls":
                    {
                        return "~/Content/img/file_extensions/xls.png";
                    }
                case ".xlsx":
                    {
                        return "~/Content/img/file_extensions/xlsx.png";
                    }
                case ".pdf":
                    {
                        return "~/Content/img/file_extensions/pfd.png";
                    }
            }

            return string.Empty;
        }
    }
}