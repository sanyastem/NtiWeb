function OnNotificationClick(s, e) {
    var z = s.GetRowKey(e.visibleIndex);
    window.open(ROOT_URL + "Notification/UpdateNotification/" + z);
 }

function ChangeRowColor(s, e) {
    $("#NotificationView_DXDataRow" + e.visibleIndex).css("background-color", "white");
    //var key = NotificationView.GetRowKey(e.visibleIndex);
    //var idNotification = NotificationView.GetRowCellValue(e.visibleIndex, "ViewDate");в
    //var c = parseInt($("#notificationsCount").text());
    
    //var c2 = c - 1;
    //if (c2 > 0) {
    //    $("#notificationsCount").text(c2);
    //} else {
    //    $("#notificationsCount").text(0);
    //    $("#notificationsCount").css("visibility", "hidden");
    //}
}