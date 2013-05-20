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
    
    if (class_name == null || class_name == "") {
        class_name = "PlaceHolder";
    }

    e.addClass(class_name);
    e.data("place-holder-default-val", e.val());
    e.data("place-holder-class", class_name);

    $('input[type="submit"]').click(function (eventobject) {
        removePlaceHolder(e, class_name);
    });

    $('button[type="submit"]').click(function (eventobject) {
        removePlaceHolder(e, class_name);
    });

    e.focus(function () {
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

    textbox += "/>"

    if (is_validation_required) {
        textbox += "<span data-valmsg-for='" + name + "' data-valmsg-replace='true'></span>";
    }
    
    return textbox;
}

function textareaFor(name, default_msg, class_name, options) {
    var textarea = "<textarea name='" + name + "' class='" + class_name + "' ";
    var is_validation_required = false;

    if (options != null) {
        if (options.required != null && options.required) {
            is_validation_required = true;
            textarea += "data-val-required='" + options.required_msg + "' ";
        }
        if (options.regex != null) {
            is_validation_required = true;
            textarea += "data-val-regex='" + options.regex_msg + "' data-val-regex-pattern='" + options.regex + "' ";
        }
    }

    if (is_validation_required) {
        textarea += "data-val='true' ";
    }

    textarea += ">" + default_msg + "</textarea>"

    if (is_validation_required) {
        textarea += "<span data-valmsg-for='" + name + "' data-valmsg-replace='true'></span>";
    }

    return textarea;
}

var dynamicListCount = 0;

function indexIn(attr, item, index, section) {
    var name = item.attr(attr);
    var regex;

    if (name.indexOf('[') != -1 && name.indexOf(']') != -1) {
        if (section != null) {
            regex = new RegExp(section + "\\[[0-9]+\\]");
            var matches = name.match(regex);

            if (matches != null && matches.length >= 1) {
                name = matches[0];
            }
        }

        var left_bracket = name.indexOf("[");
        var right_bracket = name.indexOf("]");
        var orig_index = name.substring(left_bracket + 1, right_bracket);

        if (index == null) {
            return orig_index;
        } else {
            var value;

            if (section != null) {
                value = item.attr(attr).replace(regex, section + "[" + index + "]");
            } else {
                value = name.replace(orig_index, index);
            }

            item.attr(attr, value);
            return item;
        }
    }
}

function dynamicList_DeleteItem(itemToDelete, caller) {
    if (itemToDelete == null && caller == null) {
        itemToDelete = $(this.container).children(".DynamicListItem").last();
        caller = this;
    }

    if (caller.onDelete != null) {
        caller.onDelete(itemToDelete.children(".DynamicListItemContent"));
    }

    // 获取被删除元素的类别
    var class_name = "." + itemToDelete[0].className;
    // 更新被删除的元素后所有元素Name属性的下标
    $(itemToDelete).nextAll(class_name).each(function (index, element) {
        var orig_index = indexIn("data-dynamic_list-item_id", $(element));
        var new_index = orig_index - 1;

        $(element).find(".DynamicListItemContent").each(function (child_index, child_element) {
            if ($(child_element).is("SPAN[data-valmsg-replace='true']")) {
                indexIn("data-valmsg-for", $(child_element), new_index, caller.modelName);
            } else if (child_element.tagName == "INPUT" || child_element.tagName == "TEXTAREA") {
                indexIn("name", $(child_element), new_index, caller.modelName);
            }
        });

        indexIn("data-dynamic_list-item_id", $(element), new_index);
    });

    // 移除被删除元素
    $(itemToDelete).remove();
    caller.itemCount--;

    if (caller.itemCount < caller.capacity) {
        caller.addBtn.removeAttr('disabled');
    }
}

function dynamicList_AddItem(values) {
    var should_add = true;
    var adding_fields = new Array();

    for (var i = 0; i < this.itemContents.length; ++i) {
        adding_fields.push($(this.itemContents[i]));
    }

    if (this.onInsert != null) {
        should_add = this.onInsert(adding_fields, values, this.itemCount);
    }

    if (this.capacity != -1 && this.itemCount >= this.capacity) {
        should_add = false;
    }

    if (should_add) {
        var item = $('<' + this.itemTag + ' class="DynamicListItem" data-dynamic_list-item_id="dynamic_list_item[' +
            this.itemCount + ']"></' + this.itemTag + '>');
        item.insertBefore($(this.insertPanel));

        for (var i = 0; i < adding_fields.length; ++i) {
            var content = adding_fields[i];

            for (var j = 0; j < content.length; ++j) {
                if (content[j].tagName == "SPAN" && $(content[j]).data("valmsg-replace")) {
                    indexIn("data-valmsg-for", $(content[j]), this.itemCount, this.modelName);
                } else if (content[j].tagName == "INPUT" || content[j].tagName == "TEXTAREA" || content[j].tagName == "SELECT") {
                    indexIn("name", $(content[j]), this.itemCount, this.modelName);
                }
            }

            content.addClass("DynamicListItemContent").appendTo(item);

            if (values != null && i < values.length && content.data("dynamic_list-item-value-preserved") != "true") {
                if (content.is("input") || content.is("textarea") || content.is("select"))
                    content.val(values[i]);
                else
                    content.text(values[i]);
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
            this.addBtn.attr('disabled', 'true');
        }

        var children = this.insertPanel.children(".DynamicListInsertValue");

        children.each(function (index, element) {
            resetPlaceHolder(element);
        });

        refreshFormValidations();

        $('button').button();
    }
}

function dynamicList_Clear() {
    var length = this.itemCount;

    for (var i = 0; i < length; ++i) {
        this.delete();
    }
}

function DynamicList(item_contents, container, insert_contents, on_insert, on_remove,
    item_tag, capacity, min_element_num, model_name) {
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
    this.id = dynamicListCount++;
    this.modelName = model_name;
    this.clear = dynamicList_Clear;
    this.delete = dynamicList_DeleteItem;

    var insert_panel = $('<form class="DynamicListInsertPanel" novalidate="novalidate"></form>');
    this.insertPanel = insert_panel;

    insert_panel.appendTo($(this.container));

    var button = $('<button type="button">增加</button>');
    this.addBtn = button;

    if (insert_contents != null) {
        for (var i = 0; i < insert_contents.length; ++i) {
            var content = $(insert_contents[i]);

            // spans for displaying validation message are not value fields.
            if (!$(content[0]).is("span[data-valmsg-replace='true']"))
                $(content[0]).addClass("DynamicListInsertValue");

            insert_panel.append(content);

            if (content.is("input") || content.is("textarea"))
                placeHolder(content, "PlaceHolder", button);
        }
    }

    refreshFormValidations();

    var caller = this;

    insert_panel.append(button);
    button.click(function () {
        var values = new Array();

        if (insert_panel.valid()) {
            insert_panel.children('.DynamicListInsertValue').each(function (index, element) {
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

    $('button').button();
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

// Thanks to this post: http://stackoverflow.com/questions/4459379/preview-an-image-before-it-is-uploaded
function imageUploader_valueChanged(uploader) {
    if (uploader.files && uploader.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(uploader).parent().parent().find("img").attr('src', e.target.result);
        }

        reader.readAsDataURL(uploader.files[0]);
    }
}

// Grabbed from http://trentrichardson.com/examples/timepicker/
function durationPicker(start_date_field, end_date_field) {
    var start_date = $(start_date_field);
    var end_date = $(end_date_field);

    var min_start_date = start_date.data("min-date");
    var min_end_date = start_date.data("min-date");

    var max_start_date = start_date.data("max-date");
    var max_end_date = end_date.data("max-date");

    start_date.datetimepicker({
        onClose: function (date, inst) {
            if (end_date.val() != '') {
                var test_start_date = start_date.datetimepicker('getDate');
                var test_end_date = end_date.datetimepicker('getDate');
                if (test_start_date > test_end_date)
                    end_date.datetimepicker('setDate', test_start_date);
            }
            else {
                end_date.val(date);
            }
        },
        onSelect: function (selected_date_time) {
            var max_date;

            if (min_end_date != null && min_end_date > start_date.datetimepicker('getDate'))
                max_date = min_end_date;
            else
                max_date = start_date.datetimepicker('getDate');

            end_date.datetimepicker('option', 'minDate', max_date);
        },
        dateFormat: "yy/m/d"
    });

    end_date.datetimepicker({
        onClose: function (date, inst) {
            if (start_date.val() != '') {
                var test_start_date = start_date.datetimepicker('getDate');
                var test_end_date = end_date.datetimepicker('getDate');
                if (test_start_date > test_end_date)
                    start_date.datetimepicker('setDate', test_end_date);
            }
            else {
                start_date.val(date);
            }
        },
        onSelect: function (selected_date_time) {
            var min_date;

            if (max_start_date != null && max_start_date < end_date.datetimepicker('getDate'))
                min_date = max_start_date;
            else
                min_date = end_date.datetimepicker('getDate');

            start_date.datetimepicker('option', 'maxDate', min_date);
        },
        dateFormat: "yy/m/d"
    });

    start_date.datetimepicker('option', 'maxDate', max_start_date);
    start_date.datetimepicker('option', 'minDate', min_start_date);
    end_date.datetimepicker('option', 'maxDate', max_end_date);
    end_date.datetimepicker('option', 'minDate', min_end_date);

    start_date[0].value = start_date[0].defaultValue;
    end_date[0].value = end_date[0].defaultValue;
}

// Modified from http://jqueryui.com/slider/#hotelrooms
function sliderField(field) {
    var textbox = $(field);
    var max_val = textbox.data("val-range-max");
    var min_val = textbox.data("val-range-min");
    var cur_val = textbox.val();

    var slider = $("<div class='Slider'></div>").insertAfter(textbox).slider({
        min: min_val == null ? 0 : min_val,
        max: max_val == null ? 100 : max_val,
        value: cur_val == null ? min_val : cur_val,
        slide: function (event, ui) {
            textbox.val(ui.value);
        }
    });

    textbox.change(function () {
        slider.slider("value", $(this).val());
    });
}

// Grabbed from http://stackoverflow.com/questions/15312529/resolve-circular-references-from-json-object combining with Json.Net which helps to solve circular references
function resolveReferences(json) {
    if (typeof json === 'string')
        json = JSON.parse(json);

    var byid = {}, // all objects by id
        refs = []; // references to objects that could not be resolved
    json = (function recurse(obj, prop, parent) {
        if (typeof obj !== 'object' || !obj) // a primitive value
            return obj;
        if (Object.prototype.toString.call(obj) === '[object Array]') {
            for (var i = 0; i < obj.length; i++)
                if ("$ref" in obj[i])
                    obj[i] = recurse(obj[i], i, obj);
                else
                    obj[i] = recurse(obj[i], prop, obj);
            return obj;
        }
        if ("$ref" in obj) { // a reference
            var ref = obj.$ref;
            if (ref in byid)
                return byid[ref];
            // else we have to make it lazy:
            refs.push([parent, prop, ref]);
            return;
        } else if ("$id" in obj) {
            var id = obj.$id;
            delete obj.$id;
            if ("$values" in obj) // an array
                obj = obj.$values.map(recurse);
            else // a plain object
                for (var prop in obj)
                    obj[prop] = recurse(obj[prop], prop, obj);
            byid[id] = obj;
        }
        return obj;
    })(json); // run it!

    for (var i = 0; i < refs.length; i++) { // resolve previously unknown references
        var ref = refs[i];
        ref[0][ref[1]] = byid[ref[2]];
        // Notice that this throws if you put in a reference at top-level
    }
    return json;
}

function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}

function fileUploader(element, btn_text, place_holder_text) {
    var e = $(element);

    var container = $("<div class='FileUploaderContainer'><button type='button' class='FileUploaderBtn'>" +
        btn_text + "</button><span class='FileUploaderPlaceHolder'>" + place_holder_text +
        "</span></div>").insertBefore(e);
    var button = container.children('button').first().css("margin-right", "10px");
    var place_holder = container.children('span').first();

    var wrapper = $('<div/>').css({ height: 0, width: 0, 'overflow': 'hidden' });
    $(element).wrap(wrapper);

    button.click(function () {
        element.click();
    }).show();

    e.change(function () {
        var value = $(this).val();

        if (value != null && value != "") {
            var i = value.lastIndexOf("\\");

            if (i < 0) {
                i = value.lastIndexOf("/");
            }

            if (i > 0) {
                value = value.substr(i + 1);
            }
        } else {
            value = place_holder_text;
        }

        place_holder.text(value);
    })
}