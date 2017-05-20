$(document).ready(function () {
    var г = comboBoxShipmentDate.GetValue();
    if (г === "0") {
        ShipmentBegin.SetDate(new Date(1900, 0, 1));
        ShipmentEnd.SetDate(new Date(2050, 0, 1));
        ShipmentBegin.SetEnabled(false);
        ShipmentEnd.SetEnabled(false);

    }
    $('#SPOShipment').hide();
});

function OnShipmentBeginChanged(s, e) {
    if (ShipmentEnd.GetDate() !== '') {
        var dateBeg = ShipmentBegin.GetDate();
        var dateEnd = ShipmentEnd.GetDate();
        if (dateEnd < dateBeg) {
            ShipmentBegin.SetDate(dateEnd);
        }
    }
}
function OnShipmentEndChanged(s, e) {
    if (ShipmentBegin.GetDate() !== '') {
        var dateBeg = ShipmentBegin.GetDate();
        var dateEnd = ShipmentEnd.GetDate();
        if (dateEnd < dateBeg) {
            ShipmentEnd.SetDate(dateBeg);
        }
    }
}
function ShipmentChanged(s, e) {
    if (s.GetValue().toString() !== '1') {
        $('#SPOShipment').hide();
        ShipmentBegin.SetText('');
        ShipmentEnd.SetText('');
        //ShipmentBegin.SetEnabled(false);
        //ShipmentEnd.SetEnabled(false);


        var today = new Date();
        var pastDate = new Date();

        if (s.GetValue().toString() === '0') {
            ShipmentBegin.SetDate(new Date(1900, 0, 1));
            ShipmentEnd.SetDate(new Date(2050, 0, 1));
        }

        if (s.GetValue().toString() === '2') {
            ShipmentBegin.SetDate(today);
            ShipmentEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '3') {
            today = addDays(today, -1);
            ShipmentBegin.SetDate(today);
            ShipmentEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '4') {
            var week = getMonday(new Date());
            var endWeek = addDays(week, 6);
            ShipmentBegin.SetDate(week);
            ShipmentEnd.SetDate(endWeek);

        }
        if (s.GetValue().toString() === '5') {
            pastDate = addDays(today, -6);
            ShipmentBegin.SetDate(pastDate);
            ShipmentEnd.SetDate(today);
        }

        if (s.GetValue().toString() === '6') {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            ShipmentBegin.SetDate(firstDay);
            ShipmentEnd.SetDate(lastDay);

        }
        if (s.GetValue().toString() === '7') {
            var k = today.getMonth() + 1;
            if (k >= 1 && k <= 3) {
                ShipmentBegin.SetDate(new Date(today.getFullYear(), 0, 1));
                ShipmentEnd.SetDate(new Date(today.getFullYear(), 2, 31));

            }
            if (k >= 4 && k <= 6) {
                ShipmentBegin.SetDate(new Date(today.getFullYear(), 3, 1));
                ShipmentEnd.SetDate(new Date(today.getFullYear(), 5, 30));
            }
            if (k >= 7 && k <= 9) {
                ShipmentBegin.SetDate(new Date(today.getFullYear(), 6, 1));
                ShipmentEnd.SetDate(new Date(today.getFullYear(), 8, 30));
            }
            if (k >= 10 && k <= 12) {
                ShipmentBegin.SetDate(new Date(today.getFullYear(), 9, 1));
                ShipmentEnd.SetDate(new Date(today.getFullYear(), 11, 31));
            }


        }
        if (s.GetValue().toString() === '8') {
            ShipmentBegin.SetDate(new Date(today.getFullYear(), 0, 1));
            ShipmentEnd.SetDate(new Date(today.getFullYear(), 11, 31));
        }
    } else {
        ShipmentBegin.SetEnabled(true);
        ShipmentEnd.SetEnabled(true);
        $('#SPOShipment').show();
    }
}