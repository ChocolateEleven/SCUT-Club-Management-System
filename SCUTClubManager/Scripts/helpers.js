function goBack() {
    history.go(-1);
}

function removePlaceHolder(element, class_name) {
    if (element.hasClass(class_name)) {
        element.removeClass(class_name);
        element.val("");
    }
}

function resetForm(form) {
    $(form)
        .find('.field-validation-error')
        .removeClass('field-validation-error')
        .addClass('field-validation-valid');

    $(form)
        .find('.input-validation-error')
        .removeClass('input-validation-error')
}

function refreshFormValidations() {
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("form");
    $('form').validate();
}

function placeHolder(element, class_name, additional_submit) {
    var e = $(element);

    e.addClass(class_name);
    e.data("place-holder-default-val", e.val());
    e.data("place-holder-class", class_name);

    $('input[type="submit"]').click(function (eventobject) {
        removePlaceHolder(e, class_name);
    });

    $('button[type="submit"]').click(function (eventobject) {
        removePlaceHolder(e, class_name);
    });

    element.focus(function () {
        removePlaceHolder(e, class_name);
    });

    if (additional_submit != null) {
        $(additional_submit).click(function () {
            removePlaceHolder(e, class_name);
        });
    }
}

function resetPlaceHolder(element) {
    var e = $(element);
    var class_name = e.data("place-holder-class");

    if (class_name != null) {
        if (!e.hasClass(class_name)) {
            e.addClass(class_name);
            e.val(e.data("place-holder-default-val"));
        }
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
        if (options.required != null && options.required) {
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

    if (options.required != null && options.required) {
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
            if (child_element.tagName == "SPAN") {
                indexIn("data-valmsg-for", $(child_element), new_index);
            } else {
                indexIn("name", $(child_element), new_index);
            }
        });

        indexIn("id", $(element), new_index);
    });

    // 移除被删除元素
    $(itemToDelete).remove();
    caller.itemCount--;

    if (caller.itemCount < caller.capacity) {
        $('#dynamic_list_add_item').removeAttr('disabled');
    }
}

function dynamicList_AddItem(values) {
    var should_add = true;

    if (this.onInsert != null) {
        should_add = this.onInsert(this.itemContents, values);
    }

    if (this.capacity != -1 && this.itemCount >= this.capacity) {
        should_add = false;
    }

    if (should_add) {
        var item = $('<' + this.itemTag + ' class="DynamicListItem" id="dynamic_list_item[' +
            this.itemCount + ']"></' + this.itemTag + '>');
        $(this.container).append(item);

        for (var i = 0; i < this.itemContents.length; ++i) {
            var content = $(this.itemContents[i]);

            for (var j = 0; j < content.length; ++j) {
                if (content[j].tagName == "SPAN") {
                    indexIn("data-valmsg-for", $(content[j]), this.itemCount);
                } else {
                    indexIn("name", $(content[j]), this.itemCount);
                }
            }

            content.addClass("DynamicListItemContent").appendTo(item);

            if (values != null && i < values.length) {
                content.val(values[i]);
            }
        }

        if (this.itemCount >= this.minElementNum) {
            var caller = this;
            var button = $('<button class="DynamicListItemDelete" type="button">删除</button>');

            button.click(function () {
                dynamicList_DeleteItem(item, caller);
            });
            button.appendTo(item);
        }

        this.itemCount++;

        if (this.capacity != -1 && this.itemCount >= this.capacity) {
            $('#dynamic_list_add_item').attr('disabled', 'true');
        }

        var children = this.insertPanel.children(".DynamicListInsertValue");

        children.each(function (index, element) {
            resetPlaceHolder(element);
        });

        refreshFormValidations();
    }
}

function DynamicList(item_contents, container, insert_contents, on_insert, on_remove, item_tag, capacity, min_element_num) {
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
    this.itemTag = item_tag == null ? "div" : item_tag;
    this.capacity = capacity == null ? -1 : capacity;
    this.minElementNum = min_element_num == null ? 0 : min_element_num;

    var insert_panel = $('<form class="DynamicListInsertPanel"></form>');
    this.insertPanel = insert_panel;

    insert_panel.insertAfter($(this.container));

    var button = $('<button type="button" id="dynamic_list_add_item">增加</button>');

    if (insert_contents != null) {
        for (var i = 0; i < insert_contents.length; ++i) {
            var content = $(insert_contents[i]);

            content.addClass("DynamicListInsertValue");
            insert_panel.append(content);

            placeHolder(content, "PlaceHolder", button);
        }
    }

    refreshFormValidations();

    var caller = this;

    insert_panel.append(button);
    button.click(function () {
        var values = new Array();

        if ($('.DynamicListInsertPanel').valid()) {
            $('form.DynamicListInsertPanel').children('.DynamicListInsertValue').each(function (index, element) {
                    values[index] = $(element).val();
            });

            caller.add(values);
        }
    });

    $('form[class!="DynamicListInsertPanel"]').submit(function () {
        $('form.DynamicListInsertPanel').children('.DynamicListInsertValue').each(function (index, element) {
            $(element).removeAttr("data-val");
        });
    });
    
    for (var i = 0; i < this.minElementNum; ++i) {
        this.add();
    }
}

function dropDownListFor(name, id, class_name, items, default_item_index, options, callback) {
    var drop_down_list = "<select ";
    var is_validation_required = false;

    if (name != null) {
        drop_down_list += "name='" + name + "' ";
    }
    if (id != null) {
        drop_down_list += "id='" + id + "' ";
    }
    if (class_name != null) {
        drop_down_list += "class='" + class_name + "' ";
    }
    if (default_item_index != null) {
        drop_down_list += "value='" + items[default_item_index].value + "' ";
    }
    if (options != null) {
        if (options.required != null && options.required) {
            is_validation_required = true;
            drop_down_list += "data-val-required='" + options.required_msg + "' ";
        }
        if (is_validation_required) {
            drop_down_list += "data-val='true' ";
        }
    }
    if (callback != null) {
        drop_down_list += "onchange='" + callback + "' ";
    }

    drop_down_list += ">";

    if (items != null) {
        for (var i = 0; i < items.length; ++i) {
            drop_down_list += "<option value='" + items[i].value + "'>" + items[i].key + "</option>";
        }
    }

    drop_down_list += "</select>"

    return drop_down_list;
}