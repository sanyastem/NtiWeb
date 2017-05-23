function OnDeleteActClick(s, e) {
    $.ajax({
        type: "POST",
        url: '@Url.Action("DeleteActs", "Act")',
        data: { keys: e },
        success: function (data) { ActView.Refresh(); }
    });
}

function OnViewActClick(s, e) {
    window.open(ROOT_URL + "act/details/" + e);
}

function ActBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        actAuditHistory.Show();
    }
}

function ActAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditActId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function ActPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}

function ActOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}