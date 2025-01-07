//Combox
; RoadUI.Combox = function ()
{
    var instance = this;
    this.init = function ($comboxs)
    {
        $comboxs.each(function ()
        {
            var id = $(this).attr('id');
            var name = $(this).attr('name') || id;
            var validate = $(this).attr('validate');
            var title = $(this).attr("title") || "";
            var readonly = true;// $(this).attr("readonly") && "readonly" == $(this).attr("readonly").toLowerCase();
            var disabled = $(this).prop("disabled");
            var width1 = $(this).attr("width1");
            var height1 = $(this).attr("height1");
            var datatype = $(this).attr("datatype");
            var change = $(this).attr("onchange");
            var isflow = $(this).attr("isflow");
            var type1 = $(this).attr("type1");
            var datasource = $(this).attr("datasource");
            var listmode = $(this).attr("listmode");
            if (RoadUI.Core.isIe6Or7())
            {
                $(this).css({ "display": "inline-block" });
            }

            var offsetTop = RoadUI.Core.offsetTop($(this).get(0));
            var offsetLeft = RoadUI.Core.offsetLeft($(this).get(0));
            var tagName = $(this).get(0).tagName.toLowerCase();
            var comboxWidth = $(this).width();
            var divWidth = width1 ? "width:" + width1 + "px;" : RoadUI.Core.isIe6Or7() ? "width:" + $(this).width() + "px;" : "width:auto;";
            var divHeight = height1 ? "height:" + height1 + "px;" : RoadUI.Core.isIe6Or7() ? "" : "height:auto;";
            var $hide = $('<input type="text" style="display:none;" id="' + id + '" name="' + name + '" ' + (change ? change.indexOf('"') >= 0 ? 'onchange=\'' + change + '\'' : 'onchange="' + change + '"' : '')
                + (isflow ? 'isflow="' + isflow + '"' : '') + (type1 ? 'type1="' + type1 + '"' : '')
                + (datasource ? 'datasource="' + datasource + '"' : '') + (listmode ? 'listmode="' + listmode + '"' : '')
                + 'tagname="' + tagName + '"' + '/>');
            var $input = $('<input type="text" autocomplete="off" class="comboxtext1" style="' + $(this).attr("style") + '" id="' + id + '_text" name="' + name + '_text" value="" />');
            var $div = $('<div id="' + id + '_selectdiv" style="' + divWidth + divHeight + 'white-space:nowrap;' + '" class="comboxdiv"></div>');

            if (readonly)
            {
                $input.prop("readonly", true);
            }
            initElement($input, "comboxtext");

            var $wrapDiv = $('<span style="display:inline-block;"></span>');
            var $wrapDiv1 = $('<div></div>');
            $wrapDiv1.append($hide).append($input);
            $wrapDiv.append($wrapDiv1).append($div);
            $(this).after($wrapDiv);
            $div.hide();

            var isTable = false;
            switch (tagName)
            {
                case "select":
                    var multiple = $(this).prop("multiple");
                    var $options = $('option,optgroup', $(this));
                    var texts = [];
                    var values = [];
                    for (var i = 0; i < $options.size() ; i++)
                    {
                        if ($options.eq(i).get(0).tagName.toLowerCase() == "optgroup")
                        {
                            var $divoptgroup = $('<div class="comboxoptgroup">' + $options.eq(i).text() + '</div>');
                            $div.append($divoptgroup);
                            continue;
                        }
                        if ($options.eq(i).attr("selected") && $options.eq(i).prop("selected"))
                        {
                            texts.push($options.eq(i).text());
                            values.push($options.eq(i).val());
                        }
                        var optionValue = $options.eq(i).val();
                        var $divoptions = $('<div class="' + ($options.eq(i).prop("selected") ? "comboxoption1" : "comboxoption") + '" ' +
                            'value="' + optionValue + '"></div>');
                        if (multiple)
                        {
                            var $checkbox = $('<input type="checkbox" id="checkbox_' + id + "_" + optionValue + '" name="radio_' + id + '" value="' + optionValue + '" '
                                + ($options.eq(i).prop("selected") && $options.eq(i).attr("selected") ? 'checked="checked"' : '')
                                + ' style="vertical-align:middle;"/>');
                            $divoptions.append($checkbox);
                            $checkbox.bind("click", function ()
                            {
                                instance.setValue($(this).val(), $(this).next().text(), id, true, $(this).prop("checked"));
                                $(this).parent().children("div").removeClass().addClass("comboxoption");
                                $(this).removeClass().addClass("comboxoption1");
                            });
                        }
                        else
                        {
                            $divoptions.bind("click", function ()
                            {
                                instance.setValue($(this).attr("value"), $(this).text(), id, false, $(this).prop("checked"));
                                $(this).parent().children("div[class!='comboxoptgroup']").removeClass().addClass("comboxoption");
                                $(this).removeClass().addClass("comboxoption1");
                            });
                        }

                        $divoptions.hover(function ()
                        {
                            $(this).removeClass().addClass('comboxoption1');
                        }, function ()
                        {
                            var cvalue = $("#" + id).val();
                            if (!cvalue || (',' + cvalue + ',').indexOf(',' + $(this).attr("value") + ',') == -1)
                            {
                                $(this).removeClass().addClass('comboxoption');
                            }
                        });
                        var $label = $('<label style="vertical-align:middle;" ' + (multiple ? 'for="checkbox_' + id + "_" + optionValue + '"' : '') + '>' + $options.eq(i).text() + '</label>');
                        $divoptions.append($label);
                        $div.append($divoptions);
                    }
                    $hide.val(values.join(','));
                    $input.val(texts.join(',')).attr("title", texts.join(','));
                    break;
                case "span":
                case "div":
                    isTable = true;
                    var $table = $(this).children("table");
                    var $query = $("[class='querybar']", $(this));
                    if ($query.size() > 0)
                    {
                        $query.css("height", "22px");
                        $("input[type='button']", $query).bind('click', function ()
                        {
                            var query = [];
                            $(this).parent().children("input[type!='button']").each(function ()
                            {
                                query.push($(this).attr("id") + "=" + encodeURIComponent($(this).val()));
                            });
                            $table.attr("query", "&" + query.join('&'));
                            instance.loadData(10, 1, $table);
                        });
                        $div.append($query);
                    }

                    var multiple = ($(this).attr("multiple") && ("multiple" == $(this).attr("multiple").toLowerCase()
                       || "1" == $(this).attr("multiple") || "true" == $(this).attr("multiple").toLowerCase()))
                        || ($(this).attr("more") && ("1" == $(this).attr("more").toLowerCase() || "true" == $(this).attr("more").toLowerCase()));
                    if (multiple)
                    {
                        $table.prop("multiple", true);
                    }
                    $table.attr("comboxid", id);
                    if ($("tbody tr", $table).size() == 0)
                    {
                        instance.loadData(10, 1, $table);
                    }
                    else
                    {
                        instance.setTable(id, $table, $('tbody tr', $table), "");
                    }
                    $div.append($table);
                    //$div.css("overflow", "hidden");
                    //new RoadUI.Grid({ table: $table, resizeCol:false });
                    break;
            }

            if (offsetTop + (RoadUI.Core.isIE() ? 30 : 0) + $div.height() > $(window).height())
            {
                offsetTop = offsetTop - $div.height() - (RoadUI.Core.isIE() ? 2 : 3);
                if (offsetTop)
                {
                    $div.css("top", (offsetTop < 0 ? 0 : offsetTop) + "px");
                }
            }
            if (offsetLeft + $div.width() >= $(window).width() - 30)
            {
                offsetLeft = offsetLeft - ($div.width() - comboxWidth) - 2;
            }
            if (offsetLeft)
            {
                $div.css("left", (offsetLeft < 0 ? 0 : offsetLeft) + "px");
            }
            if (disabled)
            {
                $input.prop("disabled", true);
            }
            else
            {
                $input.bind("focus", function ()
                {
                    $div.show();
                }).bind("click", function ()
                {
                    $div.show();
                }).bind("blur", function ()
                {
                    $div.hover(function () { $div.show(); }, function () { $div.hide(); });
                });
            }
            $(this).remove();
        });
    };

    this.loadData = function (size, number, $table)
    {
        if (typeof ($table) == "string")
        {
            $table = $("#" + $table.replace('.', '\\.') + "_selectdiv table");
        }
        var url = $table.attr("dataurl");
        var query = $table.attr("query");
        $.ajax({
            url: (url || "") + (url.indexOf('?') >= 0 ? "&" : "?") + "pagesize=" + (size || 10) + "&pagenumber=" + (number || 1) + (query || ""), dataType: "json", async: false, cache: false, success: function (json)
            {
                var pager = RoadUI.Core.getPager(json.count, size || 10, number || 1, "", "new RoadUI.Combox().loadData", $table.attr("comboxid"));
                instance.setTable($table.attr('comboxid'), $table, json.data, pager);
            }
        });
    };

    this.initValue = function (id, value)
    {
        var $obj = $('#' + id.replace('.', '\\.'));
        $obj.val(value);
        var tagname = $obj.attr('tagname');
        var text = "";
        switch (tagname)
        {
            case "select":
                var texts = [];
                var $divs = $("#" + id.replace('.', '\\.') + "_selectdiv div");
                for (var i = 0; i < $divs.size() ; i++)
                {
                    var value1 = $divs.eq(i).attr("value");
                    if (("," + value + ",").indexOf("," + value1 + ",") >= 0)
                    {
                        texts.push($divs.eq(i).text());
                        $divs.eq(i).removeClass().addClass('comboxoption1');
                    }
                    else
                    {
                        $divs.eq(i).removeClass().addClass('comboxoption');
                    }
                }
                text = texts.join(',');
                break;
            case "span":
            case "div":
                var listmode = $obj.attr("listmode");
                var datasource = $obj.attr("datasource");
                if ("3" != datasource)
                {
                    var $trs = $("#" + id.replace('.', '\\.') + "_selectdiv table tbody tr");
                    var texts = [];
                    for (var i = 0; i < $trs.size() ; i++)
                    {
                        var $td = $("td:first", $trs.eq(i));
                        var value1 = $td.attr("value");
                        if (("," + value + ",").indexOf("," + value1 + ",") >= 0)
                        {
                            var txt = $td.text();
                            texts.push(txt);
                            $td.attr("selected", "selected");
                            $("input[type='radio'],input[type='checkbox']", $td).prop("checked", true);
                        }
                    }
                    text = texts.join(',');
                }
                else
                {
                    var $table = $("#" + id.replace('.', '\\.') + "_selectdiv table");
                    var gettextsurl = $table.attr("gettextsurl");
                    $.ajax({
                        url: gettextsurl + (gettextsurl.indexOf('?') >= 0 ? '&' : '?') + "values=" + encodeURIComponent(value), cache: false, async: false, success: function (txt)
                        {
                            text = txt;
                            var $trs = $("tbody tr", $table);
                            for (var i = 0; i < $trs.size() ; i++)
                            {
                                var $td = $("td:first", $trs.eq(i));
                                var value1 = $td.attr("value");
                                if (("," + value + ",").indexOf("," + value1 + ",") >= 0)
                                {

                                    $td.attr("selected", "selected");
                                    $("input[type='radio'],input[type='checkbox']", $td).prop("checked", true);
                                }
                            }
                        }
                    });
                }
                break;
        }
        $obj.next().val(text);
    };

    this.setTable = function (id, $table, trs, pager)
    {
        $table = $table || $("#" + id.replace('.', '\\.') + "_selectdiv table");
        var multiple = $table.prop("multiple");

        $("tbody", $table).html(trs || "");
        if (pager)
        {
            if ($('tfoot', $table).size() == 0)
            {
                $table.append('<tfoot><tr><td style="text-align:center" colspan="' + $('thead tr th', $table).size() + '">' + (pager || "") + '</td></tr></tfoot>');
            }
            else
            {
                $('tfoot tr td', $table).html(pager || "");
            }
        }
        else
        {
            $('tfoot tr td', $table).remove();
        }
        var $hidde = $("#" + id.replace('.', '\\.'));
        var $text = $("#" + id.replace('.', '\\.') + "_text");
        var values = ($hidde.val() || "").split(',');
        var texts = ($text.val() || "").split(',');
        $("tbody tr", $table).each(function ()
        {
            var $td = $("td:first", $(this));
            var $tdHtml = $td.html();
            var selected = $td.attr("selected") && ("selected" == $td.attr("selected").toLowerCase() || "1" == $td.attr("selected") || "true" == $td.attr("selected").toLowerCase());
            if (!selected)
            {
                selected = $td.attr("isselected") && ("selected" == $td.attr("isselected").toLowerCase() || "1" == $td.attr("isselected") || "true" == $td.attr("isselected").toLowerCase());
            }
            if (!selected)
            {
                var value = $("#" + id.replace('.', '\\.')).val() || "";
                if (value && $td.attr("value") && ("," + value + ",").indexOf("," + ($td.attr("value") || "") + ",") >= 0)
                {
                    selected = true;
                }
            }

            if (selected)
            {
                values.push($td.attr("value"));
                texts.push($td.text());
            }
            if (multiple)
            {
                var $checkbox = $('<input ' + (selected ? 'checked="checked"' : '') + ' id="checkbox_' + id.replace('.', '_') + '_' + $td.attr("value") + '" name="radio_' + id.replace('.', '_') + '" type="checkbox" value="' + $td.attr("value") + '" style="vertical-align:middle;"/>');
                var $label = $('<label for="checkbox_' + id.replace('.', '_') + '_' + $td.attr("value") + '" style="vertical-align:middle;">' + $tdHtml + '</label>');
                $checkbox.bind("click", function ()
                {
                    instance.setValue($(this).val(), $(this).next().text(), id, true, $(this).prop('checked'));
                });
                $td.html('').append($checkbox, $label);
            }
            else
            {
                var $radio = $('<input ' + (selected ? 'checked="checked"' : '') + ' id="radio_' + id.replace('.', '_') + '_' + $td.attr("value") + '" name="radio_' + id.replace('.', '_') + '" type="radio" value="' + $td.attr("value") + '" style="vertical-align:middle;"/>');
                var $label = $('<label for="radio_' + id.replace('.', '_') + '_' + $td.attr("value") + '" style="vertical-align:middle;">' + $tdHtml + '</label>');
                $radio.bind("click", function ()
                {
                    instance.setValue($(this).val(), $(this).next().text(), id, false, $(this).prop('checked'));
                });
                $td.html('').append($radio, $label);
            }
        });
        $table.attr('border', '0');
        $table.attr('cellpadding', '1');
        $table.attr('cellspacing', '0');
        $table.css('width', '100%');
        $('tbody tr td', $table).css("padding", "0 0 0 3px");
        $('thead tr td', $table).css("padding", "0 0 0 3px");
        $table.removeClass().addClass("listtable");
        new RoadUI.Init().table();

        var values1 = [];
        var texts1 = [];
        for (var i = 0; i < values.length; i++)
        {
            var isIn = false;
            for (var j = 0; j < values1.length; j++)
            {
                if (values1[j] == values[i])
                {
                    isIn = true;
                    break;
                }
            }
            if (!isIn)
            {
                values1.push(values[i]);
                texts1.push(texts[i]);
            }
        }

        $hidde.val(values1.join(','));
        var txts = texts1.join(',');
        $text.val(txts).attr("title", txts);

        return $table;
    };

    this.setValue = function (value, text, id, isMultiple, checked)
    {
        var $valueele = $("#" + id.replace('.', '\\.'));
        var $textele = $("#" + id.replace('.', '\\.') + "_text");
        if (isMultiple)
        {
            if (checked)
            {
                value = $valueele.val() ? $valueele.val() + "," + value : value;
                text = $textele.val() ? $textele.val() + "," + text : text;
            }
            else
            {
                var values = ($valueele.val() || "").split(',');
                var texts = ($textele.val() || "").split(',');
                var values1 = [];
                var texts1 = [];
                for (var i = 0; i < values.length; i++)
                {
                    if (values[i] != value)
                    {
                        values1.push(values[i]);
                        texts1.push(texts[i]);
                    }
                }
                value = values1.join(',');
                text = texts1.join(',');
            }
        }
        $textele.val(text).attr("title", text);
        $valueele.val(value);
        if (!isMultiple)
        {
            $("#" + id.replace('.', '\\.') + "_selectdiv").hide();
        }
        $valueele.change();
    };

    this.reSet = function (id, options)
    {
        var $obj = $("#" + id.replace(".", "\\."));
        if ($obj.size() == 0)
        {
            return;
        }
        var $selectdiv = $("#" + id.replace(".", "\\.") + "_selectdiv");
        if ($selectdiv.size() == 0)
        {
            return;
        }
        var isflow = $obj.attr("isflow");
        var type1 = $obj.attr("type1");
        var change = $obj.attr("onchange");
        var multiple = $("input[type='checkbox']", $selectdiv).size() > 0;
        var width = $selectdiv.width();
        var height = $selectdiv.height();

        var select = '<select class="mycombox" id="' + id + '" name="' + id + '" ' + (multiple ? ' multiple="multiple"' : '') +
            (width ? ' width1="' + width + '"' : '') + (height ? ' height1="' + height + '"' : '') +
            (isflow ? ' isflow="' + isflow + '"' : '') + (type1 ? ' type1="' + type1 + '"' : '') +
            (change ? change.indexOf('"') >= 0 ? 'onchange=\'' + change + '\'' : 'onchange="' + change + '"' : '');
        select += '>';
        select += options;
        select += '</select>';
        var $select = $(select);
        $obj.parent().parent().after($select);
        $obj.parent().parent().remove();
        this.init($select);
    }
}