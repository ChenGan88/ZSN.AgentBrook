//弹出层选择
; RoadUI.SelectDiv = function ()
{
    var instance = this;
    this.init = function ($divs)
    {
        $divs.each(function (index)
        {
            var $_div = $divs.eq(index);
            var id = $_div.attr("id") || "";
            var name = $_div.attr("name") || "";
            var value = $_div.val() || "";
            var title = $_div.attr("title") || "";
            var appid = $_div.attr("appid") || "";
            var titlefield = $_div.attr("titlefield") || "";
            var pkfield = $_div.attr("pkfield") || "";
            var disabled = $_div.prop("disabled");
            var ismobile = "1" == RoadUI.Core.query("ismobile");
            $_div.prop("readonly", true);
            var $hide = $('<input type="hidden" id="' + id + '" name="' + name + '" value="' + (value || "") + '" />');
            var $but = $('<input type="button" ' + (disabled ? 'disabled="disabled"' : '') + ' title="' + title + '" class="mybutton" style="margin:0;" value="选择" />');
            $_div.attr("id", id + "_text");
            $_div.attr("name", name + "_text");
            $_div.css({ "border-right": "0" });

            $_div.removeClass().addClass("mytext");
            if (value && value.length > 0)
            {
                $.ajax({
                    url: RoadUI.Core.rooturl() + "/Controls/SelectDiv/GetTitles?values=" + value + "&applibaryid=" + appid + "&titlefield=" + titlefield + "&pkfield=" + pkfield, type: "get", async: false, cache: false, success: function (txt)
                    {
                        $_div.val(txt);
                    }
                });
            }

            if ($_div.prop("disabled"))
            {
                $but.prop("disabled", true);
                $but.removeClass().addClass("buttondisabled");
            }
            else
            {
                $but.bind("click", function ()
                {
                    var $obj = $(this).prev().prev();
                    var val = $obj.val();
                    var $obj1 = $(this).prev();
                    var title = $obj1.attr("showtitle") || "选择";
                    var appid = $obj1.attr("appid") || "";
                    var width = $obj1.attr("width") || $obj1.attr("data-windowwidth");
                    var height = $obj1.attr("height") || $obj1.attr("data-windowheight");
                    var paramsname = $obj1.attr("paramsname") || "";
                    var paramsvalue = eval(RoadUI.Core.decodeUri($obj1.attr("paramsvalue") || ""));
                    if (isNaN(width))
                    {
                        width = 500;
                    }
                    if (isNaN(height))
                    {
                        width = 450;
                    }
                    if (ismobile)
                    {
                        if (width > 320)
                        {
                            width = 320;
                        }
                        if (height > 450)
                        {
                            height = 450;
                        }
                    }
                    var params = "eid=" + id + "&values=" + val + "&applibaryid=" + appid + "&appid=" + RoadUI.Core.query("appid") + "&" + paramsname + "=" + paramsvalue;
                    new RoadUI.Window().open({ id: "selectdiv_" + id, url: "/Controls/SelectDiv/Index?" + params, width: width, height: height, resize: false, title: title, openerid: RoadUI.Core.query("tabid") || "" });
                });
            }

            $_div.after($but).before($hide);
        });
    };
    this.setValue = function (objorid)
    {
        var $obj;
        if (typeof (objorid) == "string")
        {
            $obj = $("#" + objorid);
        }
        else
        {
            $obj = $(objorid);
        }

        if (!$obj || $obj.size() == 0) return;
        var value = $obj.val();
        var appid = $obj.next().attr("appid") || "";
        var titlefield = $obj.next().attr("titlefield") || "";
        var pkfield = $obj.next().attr("pkfield") || "";
        if (value && value.length > 0)
        {
            $.ajax({
                url: RoadUI.Core.rooturl() + "/Controls/SelectDiv/GetTitles?values=" + value + "&applibaryid=" + appid + "&titlefield=" + titlefield + "&pkfield=" + pkfield, type: "get", async: false, cache: false, success: function (txt)
                {
                    $obj.next().val(txt);
                }
            });
        }
        else
        {
            $obj.next().val('');
        }

    };
}