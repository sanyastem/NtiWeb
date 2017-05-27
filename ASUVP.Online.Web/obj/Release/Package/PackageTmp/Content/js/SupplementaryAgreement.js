function OnViewSupplementaryAgreementClick(s,e,agrId) {
    window.open(ROOT_URL + "SupplementaryAgreement/details/" + e + "?agrId=" + agrId);
}

function OnEditSupplementaryAgreementClick(s,e,agrId) {
    window.open(ROOT_URL + "SupplementaryAgreement/AddOrUpdateWizard/" + e + "?agrId=" + agrId);
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function SupplementaryAgreementBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        auditDocumentId.SetValue('');
        auditCreatedOn.SetValue('');
        auditCreatedByName.SetValue('');
        auditUpdatedOn.SetValue('');
        auditUpdatedByName.SetValue('');
        $.ajax({
            url: ROOT_URL + "/supplementaryagreement/Audit",
            type: 'GET',
            data: { id: id },
            cache: false,
            success: function (obj) {
                auditDocumentId.SetValue(obj.DocumentId);

                var createdOn = parseJsonDateToString(obj.CreatedOn);
                auditCreatedOn.SetValue(createdOn);

                auditCreatedByName.SetValue(obj.CreatedByName);

                var updatedOn = parseJsonDateToString(obj.UpdatedOn);
                auditUpdatedOn.SetValue(updatedOn);

                auditUpdatedByName.SetValue(obj.UpdatedByName);
            }
        });
        agreementAuditHistory.Show();
    }
}

function SupplementaryAgreementAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditDocumentId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function SupplementaryAgreementOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}

function OnDeleteSupplementaryAgreementClick(s, e) {
    $.ajax({
        type: "POST",
        url: ROOT_URL + "/agreement/DeleteAgreements",
        data: { keys: e },
        success: function (data) { DocumentSupplementaryGrid.Refresh(); }
    });
}