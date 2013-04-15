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

function placeHolder(element, class_name, default_val_field, additional_submit) {
    element.addClass(class_name);

    if (default_val_field == null) {
        element.attr("data-defaultVal", element.val());
    } else {
        element.attr(default_val_field, element.val());
    }

    $('input[type="submit"]').click(function (eventobject) {
        removePlaceHolder(element, class_name);
    });

    $('button[type="submit"]').click(function (eventobject) {
        removePlaceHolder(element, class_name);
    });

    element.click(function () {
        removePlaceHolder(element, class_name);
    });

    if (additional_submit != null) {
        $(additional_submit).click(function () {
            removePlaceHolder(element, class_name);
        });
    }
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

function indexIn(attr, item, index) {
    var name = item.attr(attr);
    var left_bracket = name.indexOf("[");
    var right_bracket = name.indexOf("]");
    var orig_index = name.substring(left_bracket + 1, right_bracket);

    if (index == null) {
        return orig_index;
    } else {
        item.attr(attr, name.replace(orig_index, index));
        return item;
    }
}

function dynamicList_DeleteItem(itemToDelete, caller) {
    if (caller.onDelete != null) {
        caller.onDelete(itemToDelete.children(".DynamicListItemContent"));
    }

    // 获取被删除元素的类别
    var class_name = "." + itemToDelete[0].className;
    // 更新被删除的元素后所有元素Name属性的下标
    $(itemToDelete).nextAll(class_name).each(function (index, element) {
        var orig_index = indexIn("id", $(element));
        var new_index = orig_index - 1;

        $(element).children(".DynamicListItemContent").each(function (child_index, child_element) {
            indexIn("name", $(child_element), new_index);
        });

        indexIn("id", $(element), new_index);
    });

    // 移除被删除元素
    $(itemToDelete).remove();
    caller.itemCount--;
}

function dynamicList_AddItem(values) {
    if (this.onInsert != null) {
        this.onInsert(this.itemContents, values);
    }

    var item = $('<div class="DynamicListItem" id="dynamic_list_item[' + this.itemCount + ']"></div>');
    $(this.container).append(item);

    for (var i = 0; i < this.itemContents.length; ++i) {
        var content = indexIn("name", $(this.itemContents[i]), this.itemCount).addClass("DynamicListItemContent").appendTo(item);

        if (i < values.length) {
            content.val(values[i]);
        }
    }

    var caller = this;
    var button = $('<button type="button">删除</button>');

    button.click(function () {
        dynamicList_DeleteItem(item, caller);
    });
    button.appendTo(item);

    this.itemCount++;

    var children = this.insertPanel.children(".DynamicListInsertValue");

    children.each(function (index, element) {
        $(element).val($(element).attr("data-defaultVal"));
        $(element).addClass("PlaceHolder");
    });

    refreshFormValidations();
}

function DynamicList(item_contents, container, insert_contents, on_insert, on_remove) {
    if (item_contents == null) {
        throw new Error("Parameter items cannot be null.");
    }

    if (container == null) {
        throw new Error("Parameter container cannot be null.");
    }

    this.itemContents = new Array();

    for (var i = 0; i < item_contents.length; ++i) {
        this.itemContents[i] = item_contents[i];
    }

    this.onInsert = on_insert;
    this.onDelete = on_remove;
    this.itemCount = 0;
    this.add = dynamicList_AddItem;
    this.container = container;

    var insert_panel = $('<div class="DynamicListInsertPanel"></div>');
    this.insertPanel = insert_panel;

    insert_panel.insertAfter($(this.container));

    var button = $('<button type="button" id="dynamic_list_add_item">增加</button>');

    for (var i = 0; i < insert_contents.length; ++i) {
        var content = $(insert_contents[i]);

        content.addClass("DynamicListInsertValue");
        insert_panel.append(content);

        placeHolder(content, "PlaceHolder", "data-defaultVal", button);
    }

    var caller = this;

    insert_panel.append(button);
    button.click(function () {
        var values = new Array();

        $('div.DynamicListInsertPanel').children('.DynamicListInsertValue').each(function (index, element) {
            values[index] = $(element).val();
        });

        caller.add(values);
    });
}