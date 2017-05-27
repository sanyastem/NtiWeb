
$(document).ready(function () {
    DateBegin.SetEnabled(true);
    DateEnd.SetEnabled(true);
    var p = comboBoxDocumentDate.GetValue();
    if (p !== "1") {
        //var today = new Date();
        DateBegin.SetDate(new Date(1900, 0, 1));
        DateEnd.SetDate(new Date(2050, 0, 1));
        //DateBegin.SetEnabled(false);
        //DateEnd.SetEnabled(false);
        $('#SPOPeriod').hide();
    }
});

String.prototype.replaceAll = function (target, replacement) {
    return this.split(target).join(replacement);
};
function AddZeroToDate(element) {
    if (element < 10) {
        element = '0' + element;
    }
    return element;
}

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day === 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}


function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function OnDateBeginChanged(s, e) {
    if (DateEnd.GetDate() !== '') {
        var dateBeg = DateBegin.GetDate();
        var dateEnd = DateEnd.GetDate();
        if (dateEnd < dateBeg) {
            DateBegin.SetDate(dateEnd);
        }
    }
}
function OnDateEndChanged(s, e) {
    if (DateBegin.GetDate() !== '') {
        var dateBeg = DateBegin.GetDate();
        var dateEnd = DateEnd.GetDate();
        if (dateEnd < dateBeg) {
            DateEnd.SetDate(dateBeg);
        }
    }
}
function PeriodChanged(s, e) {
    if (s.GetValue().toString() !== '1') {
        $('#SPOPeriod').hide();
        DateBegin.SetText('');
        DateEnd.SetText('');
        //DateBegin.SetEnabled(false);
        //DateEnd.SetEnabled(false);

        var today = new Date();
        var pastDate = new Date();

        if (s.GetValue().toString() === '0') {
            DateBegin.SetDate(new Date(1900, 0, 1));
            DateEnd.SetDate(new Date(2050, 0, 1));
        }

        if (s.GetValue().toString() === '2') {
            DateBegin.SetDate(today);
            DateEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '3') {
            today = addDays(today, -1);
            DateBegin.SetDate(today);
            DateEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '4') {
            var week = getMonday(new Date());
            var endWeek = addDays(week, 6);
            DateBegin.SetDate(week);
            DateEnd.SetDate(endWeek);

        }
        if (s.GetValue().toString() === '5') {
            pastDate = addDays(today, -6);
            DateBegin.SetDate(pastDate);
            DateEnd.SetDate(today);

        }

        if (s.GetValue().toString() === '6') {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            DateBegin.SetDate(firstDay);
            DateEnd.SetDate(lastDay);

        }
        if (s.GetValue().toString() === '7') {
            var k = today.getMonth() + 1;
            if (k >= 1 && k <= 3) {
                DateBegin.SetDate(new Date(today.getFullYear(), 0, 1));
                DateEnd.SetDate(new Date(today.getFullYear(), 2, 31));

            }
            if (k >= 4 && k <= 6) {
                DateBegin.SetDate(new Date(today.getFullYear(), 3, 1));
                DateEnd.SetDate(new Date(today.getFullYear(), 5, 30));
            }
            if (k >= 7 && k <= 9) {
                DateBegin.SetDate(new Date(today.getFullYear(), 6, 1));
                DateEnd.SetDate(new Date(today.getFullYear(), 8, 30));
            }
            if (k >= 10 && k <= 12) {
                DateBegin.SetDate(new Date(today.getFullYear(), 9, 1));
                DateEnd.SetDate(new Date(today.getFullYear(), 11, 31));
            }


        }
        if (s.GetValue().toString() === '8') {
            DateBegin.SetDate(new Date(today.getFullYear(), 0, 1));
            DateEnd.SetDate(new Date(today.getFullYear(), 11, 31));
        }
    } else {
        DateBegin.SetEnabled(true);
        DateEnd.SetEnabled(true);
        $('#SPOPeriod').show();
    }
}