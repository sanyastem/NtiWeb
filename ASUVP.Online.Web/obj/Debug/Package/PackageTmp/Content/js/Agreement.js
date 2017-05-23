function AddZeroToDate(element) {
    if (element < 10) {
        element = '0' + element;
    }
    return element;
}

function OnViewAgreementClick(s, e) {
    window.open(ROOT_URL + "Agreement/details/" + e);
}
function OnAddAgreementClick(s, e) {
    window.open(ROOT_URL + "Agreement/Create");
}
function OnEditAgreementClick(s, e) {
    window.open(ROOT_URL + "Agreement/Edit/" + e);
}



function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}


function OnDeleteAgreementClick(s, e) {
    $.ajax({
        type: "POST",
        url: ROOT_URL + "/agreement/DeleteAgreements",
        data: { keys: e },
        success: function (data) { AgreementView.Refresh(); }
    });
}

var idAgreement;

String.prototype.replaceAll = function (target, replacement) {
    return this.split(target).join(replacement);
};

function AgreementBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        agreementAuditHistory.Show();
    }
}

function AgreementAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditDocumentId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function AgreementPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}

function AgreementOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}


//function DownloadSignedData(userguid, filename) {
//    window.location = ROOT_URL + 'agreement/DownloadSignedData' + '?userGuid=' + userguid + '&fileName=' + filename;
//}

function PackSignedAgreement(filecontent, filename, signedfile, docAttach) {
    $.ajax({
        url: ROOT_URL + 'agreement/PackSignedAgreement',
        type: 'POST',
        data: {
            fileContent: filecontent,
            fileName: filename,
            signedData: signedfile,
            docAttachId: docAttach
        },
        cache: false,
        success: function (data) {
            if (!data.success) {
                // Handle error
            } else {
                signatureMessage.SetContentHtml(data.message);
                signatureMessage.Show();
                //DownloadSignedData(data.guid, data.filename);
            }
        }
    });
}

function AgreementSign() {
    var idDoc = $("#agreementId").val();
    $.ajax({
        url: ROOT_URL+'agreement/ConvertAgreementToBase64',
        type: 'GET',
        data: { id: idDoc },
        cache: false,
        success: function (obj) {
            if (!obj.success) {
                alert(obj.message);
            } else {
                try {
                    var signedFile = SignCreate('ЗАО ""Русагротранс""', obj.fileContent);

                    signedFile.then(
                        function (result) {
                            PackSignedAgreement(obj.fileContent, obj.fileName, result, obj.attachId);
                        },
                        function (error) {
                            alert(error);
                        });
                }
                catch (e) {
                    alert(e.message);
                }
            }
        }
    })
}