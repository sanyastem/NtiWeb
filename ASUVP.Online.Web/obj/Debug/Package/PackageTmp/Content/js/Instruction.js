function OnDeleteInstructionClick(s, e) {
    $.ajax({
        type: "POST",
        url: '@Url.Action("DeleteInstructions", "Instruction")',
        data: { keys: e },
        success: function (data) { InstructionView.Refresh(); }
    });
}

function OnViewInstructionClick(s, e) {
    window.open(ROOT_URL + "Instruction/details/" + e);
}

function InstructionBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        instructionAuditHistory.Show();
    }
}

function InstructionAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditInstructionId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function InstructionPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}

function InstructionOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}