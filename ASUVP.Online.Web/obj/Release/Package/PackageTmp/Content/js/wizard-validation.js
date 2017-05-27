var isAfterDate = function (startDateStr, endDateStr) {
    var inDate = new Date(startDateStr),
        eDate = new Date(endDateStr);

    var compStartDate = new Date(inDate.getFullYear(), inDate.getMonth(), inDate.getDate()).valueOf();
    var compEndDate = new Date(eDate.getFullYear(), eDate.getMonth(), eDate.getDate()).valueOf();

    if (compStartDate > compEndDate) {
        return false;
    }
    return true;
};

jQuery.validator.addMethod("isAfterStartDate", function (value, element) {
    return isAfterDate(DateBeg.GetDate(), value);
});

jQuery.validator.addMethod("isAfterEndDate", function (value, element) {
    return isAfterDate(DateEnd.GetDate(), value);
});


$().ready(function () {
    $("#wizard").validate({
        showErrors: function (errorMap, errorList) {
        },
        rules: {
            TemplateId: {
                required: true
            },
            DocNumber: {
                required: true
            },
            DocName: {
                required: true
            },
            DocDate: {
                required: true
            },
            DateEnd: {
                isAfterStartDate: true
            },
            DateStop: {
                isAfterStartDate: true,
                isAfterEndDate: true
            }
        },
        messages: {
            TemplateId: {
                required: "Шаблон документа обязателен"
            },
            DocNumber: {
                required: "Номер документа обязателен"
            },
            DocName: {
                required: "Наименование обязательно"
            },
            DocDate: {
                required: "Дата документа обязательна"
            },
            DateEnd: {
                isAfterStartDate: "Дата окончания действия не может быть меньше даты начала"
            },
            DateStop: {
                isAfterStartDate: "Дата завершения не может быть меньше даты начала",
                isAfterEndDate: "Дата завершения не может быть меньше даты окончания периода действия"
            }
        }
    });
});