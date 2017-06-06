function OnDeleteClaimClick(s, e) {
    $.ajax({
        type: "POST",
        url: '@Url.Action("DeleteClaims", "Claim")',
        data: { keys: e },
        success: function (data) { ClaimView.Refresh(); }
    });
}

function OnViewClaimClick(s, e) {
    window.open(ROOT_URL + "Claim/details/" + e);
}
function OnAddClaimClick(s, e) {
    
    $('#addClaim').dialog("open");
   
}

function ClaimBtnAuditItemClick(s, e, id) {
    if (e.item.name === "EditHistory") {
        claimAuditHistory.Show();
    }
}

function ClaimAuditCopyIdToBuffer(s, e) {
    var copyTextarea = document.querySelector('#auditClaimId_I');
    copyTextarea.select();
    document.execCommand('copy');
}

function InitPopupMenuHandler(s, e) {
    $("#btnAudit").bind("contextmenu", OnPreventContextMenu);
}

function OnPreventContextMenu(evt) {
    evt.preventDefault();
}

function ClaimPrintClick(key) {
    window.location = ROOT_URL + 'document/preview' + '?id=' + key;
}


function ClaimOpenIrs(key) {
    window.open('les-lt://mskirs1/Traffic/Traffic/Command=RunReferenceEditor/Id=' + key);
}

//$.ajax({
//    url: ROOT_URL + "/Claim/details",
//    type: 'GET',
//data: { id: e },
//cache: false,
//success: function (obj) {
//    $("#detailsClaimId").val(e);

//    $('#detailsRegNumber').text(obj.RegNumber);
//    var detailsDocDate = new Date(parseInt(obj.DocDate.substr(6)));
//    $('#detailsDocDate').text(detailsDocDate.toLocaleDateString());
//    $('#detailsAgreementName').text(obj.AgreementName);
//    var detailsDataBeg = new Date(parseInt(obj.DataBeg.substr(6)));
//    $('#detailsDataBeg').text(detailsDataBeg.toLocaleDateString());
//    var detailsDataEnd = new Date(parseInt(obj.DataEnd.substr(6)));
//    $('#detailsDataEnd').text(detailsDataEnd.toLocaleDateString());
//        $('#detailsStToName').text(obj.StToName);
//        $('#detailsStFromName').text(obj.StFromName);
//        $('#detailsCarCount').text(obj.CarCount);
//        $('#detailsFrWeight').text(obj.FrWeight);
//        $('#detailsFrETSNGName').text(obj.FrETSNGName);
//        $('#detailsTelegrammList').text(obj.TelegrammList);
//        $('#detailsContactFIO').text(obj.ContactFIO);
//        $('#detailsApprovalPerformerImageHint').text(obj.ApprovalPerformerImageHint);
//        $('#detailsApprovalCustomerImageHint').text(obj.ApprovalCustomerImageHint);
//        $('#detailsSigningPerformerImageHint').text(obj.SigningPerformerImageHint);
//        $('#detailsSigningCustomerImageHint').text(obj.SigningCustomerImageHint);


//        ClaimDetailsPopup.Show();
//    }
//});

$(function () {

    $('#addClaim').dialog({
        autoOpen: false,
        width: '50%',
        closeText: "X"
    });
});