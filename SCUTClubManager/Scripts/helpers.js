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

function ValidationRules(required, required_msg, regex, regex_msg) {
    this.required = required;
    this.requiredMsg = required_msg == null ? "X" : required_msg;
    this.regex = regex;
    this.regexMsg = regex_msg == null ? "X" : required_msg;
}

function textboxFor(name, default_msg, class_name, options) {
    var textbox = "<input type='text' name='" + name + "' value='" + default_msg + "' class='" + class_name + "' ";
    var is_validation_required = false;

    if (options != null) {
        if (options.required) {
            is_validation_required = true;
            textbox += "data-val-required='" + options.required_msg + "' ";
        }
        if (options.regex != null) {
            is_validation_required = true;
            textbox += "data-val-regex='" + options.regex_msg + "' data-val-regex-pattern='" + options.regex + "' ";
        }

        if (is_validation_required) {
            textbox += "data-val='true' ";
        }
    }

    textbox += "/><span data-valmsg-for='" + name + "' data-valmsg-replace='true'></span>"
    
    return textbox;
}

function textareaFor(name, default_msg, class_name, options) {
    var textarea = "<textarea name='" + name + "' class='" + class_name + "' ";
    var is_validation_required = false;

    if (options.required) {
        is_validation_required = true;
        textarea += "data-val-required='" + options.required_msg + "' ";
    }
    if (options.regex != null) {
        is_validation_required = true;
        textarea += "data-val-regex='" + options.regex_msg + "' data-val-regex-pattern='" + options.regex + "' ";
    }

    if (is_validation_required) {
        textarea += "data-val='true' ";
    }

    textarea += ">" + default_msg + "</textarea><span data-valmsg-for='" + name + "' data-valmsg-replace='true'></span>"

    return textarea;
}

function indexInName(item, index) {
    var name = item.attr("name");
    var left_bracket = name.indexOf("[");
    var right_bracket = name.indexOf("]");
    var orig_index = name.substring(left_bracket + 1, right_bracket);

    if (index == null) {
        return orig_index;
    } else {
        item.attr("name", name.replace(orig_index, index));
        return item;
    }
}

function addItem(values) {
    if (value.length != this.items.length) {
        throw new Error("The length of parameter value does not match the length of items.");
    }

    for (var i = 0; i < items.length; ++i) {
        indexInName($(this.items[i], this.itemCount));
}

function DynamicList(items, container, on_insert, on_remove) {
    if (items == null) {
        throw new Error("Parameter items cannot be null.");
    }

    if (container == null) {
        throw new Error("Parameter container cannot be null.");
    }

    this.items = new Array();

    for (var i = 0; i < items.length; ++i) {
        this.items[i] = items[i];
    }

    this.onInsert = on_insert;
    this.onDelete = on_delete;
    this.itemCount = 0;
    this.add = addItem;
    this.container = container;
}