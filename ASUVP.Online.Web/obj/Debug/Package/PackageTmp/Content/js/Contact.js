function OnViewContactClick(s, e) {
    window.open(ROOT_URL + "Contact/Details/" + e);
}
function OnAddContactClick(s, e) {
    window.open(ROOT_URL + "Contact/Create");
}
function OnEditContactClick(s, e) {
    window.open(ROOT_URL + "Contact/Edit/" + e);
}
function OnDeleteContactClick(s, e) {
    $.ajax({
        type: "POST",
        url: ROOT_URL + "/contact/DeleteContacts",
        data: { keys: e },
        success: function (data) { ContactView.Refresh(); }
    });
}