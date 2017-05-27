$(document).ready(function () {
    var г = UseDate.GetValue();
    $('#SPOUseDate').hide();
    if (г === "0") {
        UseDateBeg.SetDate(new Date(1900, 0, 1));
        UseDateEnd.SetDate(new Date(2050, 0, 1));
        UseDateBeg.SetEnabled(false);
        UseDateEnd.SetEnabled(false);

    }
});

function OnUseDateBeginChanged(s, e) {
    if (UseDateEnd.GetDate() !== '') {
        var dateBeg = UseDateBeg.GetDate();
        var dateEnd = UseDateEnd.GetDate();
        if (dateEnd < dateBeg) {
            UseDateBeg.SetDate(dateEnd);
        }
    }
}
function OnUseDateEndChanged(s, e) {
    if (UseDateBeg.GetDate() !== '') {
        var dateBeg = UseDateBeg.GetDate();
        var dateEnd = UseDateEnd.GetDate();
        if (dateEnd < dateBeg) {
            UseDateEnd.SetDate(dateBeg);
        }
    }
}
function UseDateChanged(s, e) {
    if (s.GetValue().toString() !== '1') {
        $('#SPOUseDate').hide();
        UseDateBeg.SetText('');
        UseDateEnd.SetText('');

        var today = new Date();
        var pastDate = new Date();

        if (s.GetValue().toString() === '0') {
            UseDateBeg.SetDate(new Date(1900, 0, 1));
            UseDateEnd.SetDate(new Date(2050, 0, 1));
        }

        if (s.GetValue().toString() === '2') {
            UseDateBeg.SetDate(today);
            UseDateEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '3') {
            today = addDays(today, -1);
            UseDateBeg.SetDate(today);
            UseDateEnd.SetDate(today);
        }
        if (s.GetValue().toString() === '4') {
            var week = getMonday(new Date());
            var endWeek = addDays(week, 6);
            UseDateBeg.SetDate(week);
            UseDateEnd.SetDate(endWeek);

        }
        if (s.GetValue().toString() === '5') {
            pastDate = addDays(today, -6);
            UseDateBeg.SetDate(pastDate);
            UseDateEnd.SetDate(today);
        }

        if (s.GetValue().toString() === '6') {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            UseDateBeg.SetDate(firstDay);
            UseDateEnd.SetDate(lastDay);

        }
        if (s.GetValue().toString() === '7') {
            var k = today.getMonth() + 1;
            if (k >= 1 && k <= 3) {
                UseDateBeg.SetDate(new Date(today.getFullYear(), 0, 1));
                UseDateEnd.SetDate(new Date(today.getFullYear(), 2, 31));

            }
            if (k >= 4 && k <= 6) {
                UseDateBeg.SetDate(new Date(today.getFullYear(), 3, 1));
                UseDateEnd.SetDate(new Date(today.getFullYear(), 5, 30));
            }
            if (k >= 7 && k <= 9) {
                UseDateBeg.SetDate(new Date(today.getFullYear(), 6, 1));
                UseDateEnd.SetDate(new Date(today.getFullYear(), 8, 30));
            }
            if (k >= 10 && k <= 12) {
                UseDateBeg.SetDate(new Date(today.getFullYear(), 9, 1));
                UseDateEnd.SetDate(new Date(today.getFullYear(), 11, 31));
            }


        }
        if (s.GetValue().toString() === '8') {
            UseDateBeg.SetDate(new Date(today.getFullYear(), 0, 1));
            UseDateEnd.SetDate(new Date(today.getFullYear(), 11, 31));
        }
    } else {
        UseDateBeg.SetEnabled(true);
        UseDateEnd.SetEnabled(true);
        $('#SPOUseDate').show();
    }
}