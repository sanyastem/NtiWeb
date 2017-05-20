//using ASUVP.Core.Configuration;
//using CryptoPro.Sharpei.Xml;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Packaging;
//using System.Security.Cryptography;
//using System.Security.Cryptography.X509Certificates;
//using System.Security.Cryptography.Xml;
//using System.Web;
//using System.Xml;

namespace ASUVP.Core.Web.Signing
{
    public static class Sign
    {
        //static readonly string RT_OfficeDocument = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
        //static readonly string OfficeObjectID = "idOfficeObject";
        //static readonly string SignatureID = "idPackageSignature";
        //static readonly string ManifestHashAlgorithm = CPSignedXml.XmlDsigGost3411Url;

        //public static Stream SignWordDocument(Stream wordDoc)
        //{
        //    string certName = ConfigManager.AppSetting<string>("sign:cert");
        //    string certPass = ConfigManager.AppSetting<string>("cert:pass");

        //    string tempFolder = HttpRuntime.CodegenDir;
        //    string tempWordFile = $"{tempFolder}\\{Guid.NewGuid()}.docx";

        //    if (!File.Exists(certName)) throw new FileNotFoundException("Certificate was not found.");

        //    X509Certificate2Collection collection = new X509Certificate2Collection();
        //    collection.Import(certName, certPass, X509KeyStorageFlags.PersistKeySet);

        //    var cert = collection[0];

        //    using (var fileStream = File.Create(tempWordFile))
        //    {
        //        wordDoc.Seek(0, SeekOrigin.Begin);
        //        wordDoc.CopyTo(fileStream);
        //    }

        //    using (Package package = Package.Open(tempWordFile))
        //    {
        //        SignAllParts(package, cert);
        //    }

        //    return File.Open(tempWordFile, FileMode.Open);
        //}

        //public static void SignWordDocument(string wordDoc)
        //{
        //    var certName = ConfigManager.AppSetting<string>("sign:cert");

        //    X509Certificate2 certificate = new X509Certificate2(certName);

        //    using (Package package = Package.Open(wordDoc))
        //    {
        //        SignAllParts(package, certificate);
        //    }
        //}

        //private static void SignAllParts(Package package, X509Certificate2 certificate)
        //{
        //    List<Uri> PartstobeSigned = new List<Uri>();
        //    List<PackageRelationshipSelector> SignableReleationships = new List<PackageRelationshipSelector>();

        //    foreach (PackageRelationship relationship in package.GetRelationshipsByType(RT_OfficeDocument))
        //    {
        //        // Pass the releationship of the root. This is decided based on the RT_OfficeDocument
        //        // http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument
        //        CreateListOfSignableItems(relationship, PartstobeSigned, SignableReleationships);
        //    }
        //    // Create the DigitalSignature Manager
        //    PackageDigitalSignatureManager dsm = new PackageDigitalSignatureManager(package);
        //    dsm.CertificateOption = CertificateEmbeddingOption.InSignaturePart;
        //    dsm.HashAlgorithm = ManifestHashAlgorithm;
        //    try
        //    {
        //        DataObject officeObject = CreateOfficeObject(SignatureID, dsm.HashAlgorithm);
        //        Reference officeObjectReference = new Reference("#" + OfficeObjectID);
        //        var sgn = dsm.Sign(PartstobeSigned, certificate, SignableReleationships, SignatureID, new DataObject[] { officeObject }, new Reference[] { officeObjectReference });
        //    }
        //    catch (CryptographicException ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private static DataObject CreateOfficeObject(string signatureID, string hashAlgorithm)
        //{
        //    XmlDocument document = new XmlDocument();
        //    document.LoadXml(string.Format(
        //    "<OfficeObject>" +
        //        "<SignatureProperties xmlns=\"http://www.w3.org/2000/09/xmldsig#\">" +
        //            "<SignatureProperty Id=\"idOfficeV1Details\" Target=\"{0}\">" +
        //                "<SignatureInfoV1 xmlns=\"http://schemas.microsoft.com/office/2006/digsig\">" +
        //                  "<SetupID></SetupID>" +
        //                  "<ManifestHashAlgorithm>{1}</ManifestHashAlgorithm>" +
        //                  "<SignatureProviderId>{2}</SignatureProviderId>" +
        //                "</SignatureInfoV1>" +
        //            "</SignatureProperty>" +
        //        "</SignatureProperties>" +
        //    "</OfficeObject>", signatureID, ManifestHashAlgorithm, "{F5AC7D23-DA04-45F5-ABCB-38CE7A982553}"));
        //    DataObject officeObject = new DataObject();
        //    // do not change the order of the following two lines
        //    officeObject.LoadXml(document.DocumentElement); // resets ID
        //    officeObject.Id = OfficeObjectID; // required ID, do not change
        //    return officeObject;
        //}

        //private static void CreateListOfSignableItems(PackageRelationship relationship, List<Uri> partstobeSigned, List<PackageRelationshipSelector> signableReleationships)
        //{
        //    // This function adds the releation to SignableReleationships. And then it gets the part based on the releationship. Parts URI gets added to the PartstobeSigned list.
        //    PackageRelationshipSelector selector = new PackageRelationshipSelector(relationship.SourceUri, PackageRelationshipSelectorType.Id, relationship.Id);
        //    signableReleationships.Add(selector);
        //    if (relationship.TargetMode == TargetMode.Internal)
        //    {
        //        PackagePart part = relationship.Package.GetPart(PackUriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri));
        //        if (partstobeSigned.Contains(part.Uri) == false)
        //        {
        //            partstobeSigned.Add(part.Uri);
        //            // GetRelationships Function: Returns a Collection Of all the releationships that are owned by the part.
        //            foreach (PackageRelationship childRelationship in part.GetRelationships())
        //            {
        //                CreateListOfSignableItems(childRelationship, partstobeSigned, signableReleationships);
        //            }
        //        }
        //    }
        //}
    }
}
