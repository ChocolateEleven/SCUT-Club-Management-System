function goBack() {
    history.go(-1);
}

function placeHolder(element, class_name) {
    element.addClass(class_name);
    element.click(function () {
        if ($(this).hasClass(class_name)) {
            $(this).removeClass(class_name);
            $(this).val("");
        }
    });
}