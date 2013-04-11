function goBack() {
    history.go(-1);
}

function removePlaceHolder(element, class_name) {
    if (element.hasClass(class_name)) {
        element.removeClass(class_name);
        element.val("");
    }
}

function refreshFormValidations() {
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("form");
    $('form').validate();
}

function placeHolder(element, class_name) {
    element.addClass(class_name);

    $('input[type="submit"]').click(function (eventobject) {
        removePlaceHolder(element, class_name);
    });

    $('button[type="submit"]').click(function (eventobject) {
        removePlaceHolder(element, class_name);
    });

    element.click(function () {
        removePlaceHolder(element, class_name);
    });
}