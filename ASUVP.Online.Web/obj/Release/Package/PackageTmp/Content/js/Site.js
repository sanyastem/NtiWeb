if (typeof frame !== 'undefined') {
    frame.onresize = function () {
        var width = parseInt($("#content-inner").width());// + "px";
        if (width < 974) {
            width = 974;
        }
        var c = width+"px";
        $(".dxgvCSD:first").css("width", c);
        $("#AgreementView").css("width", c);
        $("#rbAgreementView_T0").css("width", c);
        $("#AccountView").css("width", c);
        $("#rbAccountView_T0").css("width", c);
        $("#ProtocolView").css("width", c);
        $("#rbProtocolView_T0").css("width", c);
        $("#InstructionView").css("width", c);
        $("#rbInstructionView_T0").css("width", c);
        $("#ActView").css("width", c);
        $("#rbActView_T0").css("width", c);
        $("#ContactView").css("width", c);
        $("#rbContactView_T0").css("width", c);
        $("#RoleView").css("width", c);
        $("#rbRoleView_T0").css("width", c);
        $(".RoundView").css("width", c);
        $(".dxgvPagerBottomPanel_Moderno").css("width", c);
        $("#rbClaimView_T0").css("width", c);
        $("#rbNotificationView_T0").css("width", c);
        $("#NotificationView").css("width", c);
    }
}

function parseJsonDate(jsonDate) {
    var offset = new Date().getTimezoneOffset() * 60000;
    var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);
    if (parts[2] == undefined) parts[2] = 0;
    if (parts[3] == undefined) parts[3] = 0;
    d = new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
    date = d.getDate() + 1;
    date = date < 10 ? "0" + date : date;
    mon = d.getMonth() + 1;
    mon = mon < 10 ? "0" + mon : mon;
    year = d.getFullYear();
    return (mon + "/" + date + "/" + year);
};
function parseJsonDateToString(jsonDate) {
    if (jsonDate === null) { return ''; }
    var offset = new Date().getTimezoneOffset() * 60000;
    var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);
    if (parts[2] == undefined) parts[2] = 0;
    if (parts[3] == undefined) parts[3] = 0;
    d = new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
    date = d.getDate() + 1;
    date = date < 10 ? "0" + date : date;
    mon = d.getMonth() + 1;
    mon = mon < 10 ? "0" + mon : mon;
    year = d.getFullYear();
    return (date + "." + mon + "." + year);
};

function ConvertToDate(selector) {
    var from = selector.split(".");
    return new Date(from[2], from[1] - 1, from[0]);
}


function DownloadDocumentAttach(s,e){
                    var href = ROOT_URL + 'Filter/Download' + '?id=' + key;
                location.href = href;
}

function onDocAttachClick(agrId) {
    DocumentAttachPopup.Show();
    DocumentAttachGrid.callbackUrl = ROOT_URL + "/document/documentattachpopupdetails?id=" + agrId;
    DocumentAttachGrid.Refresh();
}

function OnBtnAttachPrintClick(attachId) {
    var href = ROOT_URL + "Document/Preview?id=" + attachId;
    window.open(href, "_blank");
}
function OnAgreementSendMail(agreechId) {
    var href = ROOT_URL + "Mail/AgreementSendMail/?id=" + agreechId;
    window.open(href, "_blank");
}
function OnProtocolSendMail(protocolchId) {
    var href = ROOT_URL + "Mail/ProtocolSendMail/?id=" + protocolchId;
    window.open(href, "_blank");
}
function OnAccountSendMail(accountId) {
    var href = ROOT_URL + "Mail/AccountSendMail/?id=" + accountId;
    window.open(href, "_blank");
}
function OnClaimSendMail(claimId) {
    var href = ROOT_URL + "Mail/ClaimSendMail/?id=" + claimId;
    window.open(href, "_blank");
}
function OnActSendMail(actId) {
    var href = ROOT_URL + "Mail/ActSendMail/?id=" + actId;
    window.open(href, "_blank");
}
function OnInstructionSendMail(actId) {
    var href = ROOT_URL + "Mail/InstructionSendMail/?id=" + actId;
    window.open(href, "_blank");
}
