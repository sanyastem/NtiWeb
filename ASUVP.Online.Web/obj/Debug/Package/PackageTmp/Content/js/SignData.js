var CADESCOM_CADES_BES = 1;
var CAPICOM_CURRENT_USER_STORE = 2;
var CAPICOM_MY_STORE = "My";
var CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED = 2;
var CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME = 1;
var CADESCOM_BASE64_TO_BINARY = 1;
var CADESCOM_CADES_X_LONG_TYPE_1 = 0x5d;

function SignCreate(certSubjectName, dataToSign) {
    return new Promise(function(resolve, reject){
        cadesplugin.async_spawn(function *(args) {
            try {
                var oStore = yield cadesplugin.CreateObjectAsync("CAPICOM.Store");
                yield oStore.Open(CAPICOM_CURRENT_USER_STORE, CAPICOM_MY_STORE,
                    CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                var CertificatesObj = yield oStore.Certificates;
                var oCertificates = yield CertificatesObj.Find(
                    CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME, certSubjectName);

                var Count = yield oCertificates.Count;
                if (Count == 0) {
                    throw("Certificate not found: " + args[0]);
                }
                var oCertificate = yield oCertificates.Item(1);
                var oSigner = yield cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");
                yield oSigner.propset_Certificate(oCertificate);
                var oSignedData = yield cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");
                // По-моему здесь нужно ещё что-то мудрить, я так и не разобрался
                yield oSignedData.propset_ContentEncoding(1); //CADESCOM_BASE64_TO_BINARY
                yield oSignedData.propset_Content(dataToSign);

                var sSignedMessage = yield oSignedData.SignCades(oSigner, CADESCOM_CADES_BES);

                yield oStore.Close();

                args[2](sSignedMessage);
            }
            catch (e)
            {
                args[3]("Failed to create signature. Error: " + cadesplugin.getLastError(e));
            }
        }, certSubjectName, dataToSign, resolve, reject);
    });
}
