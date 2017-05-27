function OnViewProtocolClick(s, e) {
    window.open(ROOT_URL + "Protocol/details/" + e);
}

function ProtocolPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}

function ProtocolBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        protocolAuditHistory.Show();
    }
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}
function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function ProtocolAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditProtocolId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function ProtocolOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}