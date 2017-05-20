using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace ASUVP.Online.Web.Tools
{
    public class HtmlEditorHelper
    {
        public const string ImagesDirectory = "~/Content/HtmlEditor/Images/";
        public const string ThumbnailsDirectory = "~/Content/HtmlEditor/Thumbnails/";
        public const string UploadDirectory = "~/Content/HtmlEditor/UploadFiles/";
        public const string HtmlLocation = "~/Content/HtmlEditor/Html/";

        public static readonly UploadControlValidationSettings ImageUploadValidationSettings =
            new UploadControlValidationSettings
            {
                AllowedFileExtensions = new[] {".jpg", ".jpeg", ".jpe", ".gif", ".png"},
                MaxFileSize = 4000000
            };

        static HtmlEditorFileSaveSettings _fileSaveSettings;
        public static HtmlEditorFileSaveSettings FileSaveSettings
        {
            get
            {
                if (_fileSaveSettings == null)
                {
                    _fileSaveSettings = new HtmlEditorFileSaveSettings();
                    _fileSaveSettings.FileSystemSettings.UploadFolder = ImagesDirectory + "Upload/";
                }
                return _fileSaveSettings;
            }
        }

        static MVCxHtmlEditorImageSelectorSettings _imageSelectorSettings;
        public static HtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (_imageSelectorSettings == null)
                {
                    _imageSelectorSettings = new MVCxHtmlEditorImageSelectorSettings();
                    SetHtmlEditorImageSelectorSettings(_imageSelectorSettings);
                }
                return _imageSelectorSettings;
            }
        }
        public static MVCxHtmlEditorImageSelectorSettings SetHtmlEditorImageSelectorSettings(MVCxHtmlEditorImageSelectorSettings settingsImageSelector)
        {
            settingsImageSelector.UploadCallbackRouteValues = new { Controller = "Features", Action = "FeaturesImageSelectorUpload" };

            settingsImageSelector.Enabled = true;
            settingsImageSelector.CommonSettings.RootFolder = ImagesDirectory;
            settingsImageSelector.CommonSettings.ThumbnailFolder = ThumbnailsDirectory;
            settingsImageSelector.CommonSettings.AllowedFileExtensions = new [] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
            settingsImageSelector.EditingSettings.AllowCreate = true;
            settingsImageSelector.EditingSettings.AllowDelete = true;
            settingsImageSelector.EditingSettings.AllowMove = true;
            settingsImageSelector.EditingSettings.AllowRename = true;
            settingsImageSelector.UploadSettings.Enabled = true;
            settingsImageSelector.FoldersSettings.ShowLockedFolderIcons = true;

            settingsImageSelector.PermissionSettings.AccessRules.Add(
                new FileManagerFolderAccessRule
                {
                    Path = "",
                    Upload = Rights.Deny
                });
            return settingsImageSelector;
        }

        public static string GeHtmlContentByFileName(string fileName)
        {
            return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(
                $"{HtmlLocation}{fileName}"));
        }
        public static string GeHtmlContentByFileName(string fileName, bool demoPageIsInRoot)
        {
            string result = GeHtmlContentByFileName(fileName);
            return demoPageIsInRoot ? result : result.Replace("Content/", "../Content/");
        }
    }
}