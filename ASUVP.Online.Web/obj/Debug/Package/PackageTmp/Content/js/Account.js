function OnDeleteAccountClick(s, e) {
    $.ajax({
        type: "POST",
        url: '@Url.Action("DeleteAccounts", "Account")',
        data: { keys: e },
        success: function (data) { AccountView.Refresh(); }
    });
}

function OnViewAccountClick(s, e) {
    window.open(ROOT_URL + "account/details/" + e);
}

function AccountBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        accountAuditHistory.Show();
    }
}

function AccountAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditAccountId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function AccountPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}

function AccountOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}