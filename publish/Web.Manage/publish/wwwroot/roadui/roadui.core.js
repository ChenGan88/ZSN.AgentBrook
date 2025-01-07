RoadUI = function () { };
RoadUI.Core = {
    allFrames: [],
    getAllFrames: function (frame)
    {
        if (!frame)
        {
            frame = top;
            this.allFrames.push(frame);
        }
        var frames = frame.frames;
        for (var i = 0; i < frames.length; i++)
        {
            this.allFrames.push(frames[i]);
            this.getAllFrames(frames[i]);
        }
    },
    newid: function (isMiddline)
    {
        var guid = "";
        isMiddline = isMiddline == undefined ? true : isMiddline;
        for (var i = 1; i <= 32; i++)
        {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if (isMiddline && (i == 8 || i == 12 || i == 16 || i == 20))
            {
                guid += "-";
            }
        }
        return guid;
    },
    rooturl: function ()
    {
        if (top && top.rootdir != undefined)
        {
            return top.rootdir;
        }
        else
        {
            var cookie = $.cookies.get("rootdir");
            return cookie == null || cookie.toString() == "undefined" ? "" : cookie.toString();
        }
    },
    query: function (name, search)
    {
        if (!search)
        {
            search = document.location.search;
        }
        var pattern = new RegExp("[?&]" + name + "\=([^&]+)", "g");
        var matcher = pattern.exec(search);
        var items = null;
        if (null != matcher)
        {
            try
            {
                items = decodeURIComponent(decodeURIComponent(matcher[1]));
            } catch (e)
            {
                try
                {
                    items = decodeURIComponent(matcher[1]);
                } catch (e)
                {
                    items = matcher[1];
                }
            }
        }
        return items;
    },
    open: function (url, width, height, name)//弹出居中窗口
    {
        //弹出窗口的宽度
        var iWidth = width || 700;
        //弹出窗口的高度
        var iHeight = height || 500;
        var y = (window.screen.availHeight - 30 - iHeight) / 2;    //获得窗口的垂直位置;
        var x = (window.screen.availWidth - 10 - iWidth) / 2;      //获得窗口的水平位置;
        return window.open(url, '', 'height=' + iHeight.toString() + ',width=' + iWidth.toString() + ',top=' + y.toString() + ',left=' + x.toString() + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no');
    },
    formatDate: function (date, format) {
        date = new Date(date);
        var map = {
            "M": date.getMonth() + 1, //月份 
            "d": date.getDate(), //日 
            "h": date.getHours(), //小时 
            "m": date.getMinutes(), //分 
            "s": date.getSeconds(), //秒 
            "q": Math.floor((date.getMonth() + 3) / 3), //季度 
            "S": date.getMilliseconds() //毫秒 
        };
        format = format.replace(/([yMdhmsqS])+/g, function(all, t){
            var v = map[t];
            if(v !== undefined){
                if(all.length > 1){
                    v = '0' + v;
                    v = v.substr(v.length-2);
                }
                return v;
            }
            else if(t === 'y'){
                return (date.getFullYear() + '').substr(4 - all.length);
            }
            return all;
        });
        return format;
    },
    decodeUri: function (uri)
    {
        try
        {
            if (!uri || $.trim(uri).length == 0)
            {
                return "";
            }
            return decodeURIComponent(uri);
        }
        catch (e)
        {
            return uri;
        }
    },
    accDiv: function (arg1, arg2)//返回值：arg1除以arg2的精确结果
    {
        var t1 = 0, t2 = 0, r1, r2;
        try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
        try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
        with (Math)
        {
            r1 = Number(arg1.toString().replace(".", ""))
            r2 = Number(arg2.toString().replace(".", ""))
            return (r1 / r2) * pow(10, t2 - t1);
        }
    },
    accMul: function (arg1, arg2)//返回值：arg1乘以 arg2的精确结果
    {
        var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
        try { m += s1.split(".")[1].length } catch (e) { }
        try { m += s2.split(".")[1].length } catch (e) { }
        return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
    },
    accAdd: function (arg1, arg2)// 返回值：arg1加上arg2的精确结果
    {
        var r1, r2, m, c;
        try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
        try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
        c = Math.abs(r1 - r2);
        m = Math.pow(10, Math.max(r1, r2))
        if (c > 0)
        {
            var cm = Math.pow(10, c);
            if (r1 > r2)
            {
                arg1 = Number(arg1.toString().replace(".", ""));
                arg2 = Number(arg2.toString().replace(".", "")) * cm;
            }
            else
            {
                arg1 = Number(arg1.toString().replace(".", "")) * cm;
                arg2 = Number(arg2.toString().replace(".", ""));
            }
        }
        else
        {
            arg1 = Number(arg1.toString().replace(".", ""));
            arg2 = Number(arg2.toString().replace(".", ""));
        }
        return (arg1 + arg2) / m
    },
    accSub: function (arg1, arg2)// 返回值：arg1减去arg2的精确结果
    {
        var r1, r2, m, n;
        try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
        try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
        m = Math.pow(10, Math.max(r1, r2));
        //last modify by deeka
        //动态控制精度长度
        n = (r1 >= r2) ? r1 : r2;
        return ((arg1 * m - arg2 * m) / m).toFixed(n);
    },
    isIe6Or7: function ()
    {
        return this.isIE6 || this.isIE7;
    },
    isIE: function ()
    {
        return navigator.appName == "Microsoft Internet Explorer";
    },
    isIE6: function ()
    {
        return navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE6.0";
    },
    isIE7: function ()
    {
        return navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE7.0";
    },
    isIE8: function ()
    {
        return navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE8.0";
    },
    intDiv: function (number1, number2)//整除
    {
        var num1 = Math.round(number1);
        var num2 = Math.round(number2);
        var result = num1 / num2;
        if (result >= 0)
        {
            result = Math.floor(result);
        }
        else
        {
            result = Math.ceil(result);
        }
        return result;
    },
    offsetTop:function (elements){
        var top = elements.offsetTop;
        var parent = elements.offsetParent;
        while (parent)
        {
            top += parent.offsetTop;
            parent = parent.offsetParent;
        };
        return top;
    },
    offsetLeft:function (elements){
        var left = elements.offsetLeft;
        var parent = elements.offsetParent;
        while (parent)
        {
            left += parent.offsetLeft;
            parent = parent.offsetParent;
        };
        return left;
    },
    loadOptions: function linkageField(select)
    {
        var $select = $(select);
        var linkagefield = $select.attr("linkagefield");
        var linkagesource = $select.attr("linkagesource");
        var linkagesourcetext = $select.attr("linkagesourcetext");
        var linkagesourceconn = $select.attr("linkagesourceconn");
        var issubtable = "1" == $select.attr("issubtable");

        var value = $select.val();
        if (!linkagefield || linkagefield.length == 0)
        {
            return;
        }
        if (!linkagesource || linkagesource.length == 0)
        {
            return;
        }
        var $linkage = $(document.getElementById(linkagefield));
        if ($linkage.size() == 0)
        {
            return;
        }
        if (!value || value.length == 0)
        {
            $linkage.html('');
            return;
        }
        switch (linkagesource)
        {
            case "sql":
            case "dict":
                $.ajax({
                    url: this.rooturl() + "/WorkFlowRun/GetLinkageOptions", async: false, cache: false, type: "POST", data: { "linkagesource": linkagesource, "linkagesourcetext": linkagesourcetext, "linkagesourceconn": linkagesourceconn, "value": value }, success: function (options)
                    {
                        $linkage.html(options);
                    }
                });
                break;
            case "url":
                linkagesourcetext = $.trim(linkagesourcetext).substr(0, 1) == "/" ? this.rooturl() + linkagesourcetext : linkagesourcetext;
                $.ajax({
                    url: linkagesourcetext, async: false, cache: false, type: "POST", data: { "value": value }, success: function (options)
                    {
                        $linkage.html(options);
                    }
                });
                break;
        }
        if ($linkage.attr("onchange"))
        {
            RoadUI.Core.loadOptions($linkage.get(0));
        }
    },
    getPager:function (count, size, number, param, loadDataFunName, eleId)
    {
        eleId = eleId || "";
        var pager = '';
        size = size || 15;
        number = number || 1;
        //得到共有多少页
        var pageCount = count <= 0 ? 1 : count % size == 0 ? parseInt(count / size) : parseInt(count / size) + 1;
        if (pageCount <= 1)//只有一页则返回空
        {
             return "";
        }
        if (number < 1)
        {
            number = 1;
        }
        else if (number > pageCount)
        {
            number = pageCount;
        }
        //构造分页字符串
        var displaySize = 10;//中间显示的页数
        pager += "<div>";
        pager += "<span style='margin-right:15px;'>共 " + count.toString() + " 条  每页 " + size.toString() + " 条</span>";
        pager += "<a class=\"b\" style=\"margin-right:10px;\" href=\"javascript:eval('" + loadDataFunName + "(" + size + "," + 1 + ",\\'" + eleId + "\\')');\">首页</a>";
        pager += "<a class=\"b\" style=\"margin-right:10px;\" href=\"javascript:eval('" + loadDataFunName + "(" + size + "," + (number - 1) + ",\\'" + eleId + "\\')');\">上一页</a>";
        pager += "<a class=\"b\" style=\"margin-right:10px;\" href=\"javascript:eval('" + loadDataFunName + "(" + size + "," + (number + 1) + ",\\'" + eleId + "\\')');\">下一页</a>";
        pager += "<a class=\"b\" href=\"javascript:eval('" + loadDataFunName + "(" + size + "," + pageCount + ",\\'" + eleId + "\\')');\">尾页</a>";  
        pager += "</div>";
        return pager;
    },
    getPager1: function (count ,size, number, funName)
    {
        if (!count)
        {
            return "";
        }
        size = this.getPageSize(size);
        number = this.getPageNumber(number);

        //得到共有多少页
        var pageCount = count <= 0 ? 1 : count % size == 0 ? parseInt(count / size) : parseInt(count / size) + 1;
        if (pageCount <= 1)//只有一页则返回空
        {
            return "";
        }
        if (number < 1)
        {
            number = 1;
        }
        else if (number > pageCount)
        {
            number = pageCount;
        }

        var displaySize = 10;
        var jsFunctionName = funName;
        var returnPagerString = "";

        //构造分页字符串
        returnPagerString+="<div>";
        returnPagerString+="<span style='margin-right:15px;'>共 " + count.toString() + " 条  每页 <input type='text' id='tnt_count' title='输入数字可改变每页显示条数' class='pagertxt' onchange=\"javascript:$.cookies.set('pagesize', this.value);" + jsFunctionName + "(this.value," + number.toString() + ");\" value='" + size.toString() + "' /> 条  ";
        returnPagerString+="转到 <input type='text' id='paernumbertext' title='输入数字可跳转页' value=\"" + number.toString() + "\" onchange=\"javascript:" + jsFunctionName + "(" + size.toString() + ", this.value);\" class='pagertxt'/> 页</span>";
        if (number > 1)
        {
            returnPagerString+="<a class=\"pager\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + "," + (number - 1).toString() + ");\"><span class=\"pagerarrow\">«</span></a>";
        }
        //添加第一页
        if (number >= displaySize / 2 + 3)
        {
            returnPagerString+="<a class=\"pager\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + ",1);\">1…</a>";
        }
        else
        {
            returnPagerString+="<a class=\"" + (1 == number ? "pagercurrent" : "pager") + "\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + ",1);\">1</a>";
        }

        //添加中间数字
        var star = number - displaySize / 2;
        var end = number + displaySize / 2;
        if (star < 2)
        {
            end += 2 - star;
            star = 2;
        }
        if (end > pageCount - 1)
        {
            star -= end - (pageCount - 1);
            end = pageCount - 1;
        }
        if (star < 2)
        {
            star = 2;
        }

        for (var i = star; i <= end; i++)
        {
            returnPagerString+="<a class=\"" + (i == number ? "pagercurrent" : "pager") + "\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + "," + i.toString() + ");\">" + i.toString() + "</a>";
        }
        //添加最后页
        if (number <= pageCount - (displaySize / 2))
        {
            returnPagerString+="<a class=\"pager\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + "," + pageCount.toString() + ");\">…" + pageCount.toString() + "</a>";
        }
        else if (pageCount > 1)
        {
            returnPagerString+="<a class=\"" + (pageCount == number ? "pagercurrent" : "pager") + "\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + "," + pageCount.toString() + ");\">" + pageCount.toString() + "</a>";
        }
        if (number < pageCount)
        {
            returnPagerString+="<a class=\"pager\" href=\"javascript:" + jsFunctionName + "(" + size.toString() + "," + (number + 1).toString() + ");\"><span class=\"pagerarrow\">»</span></a>";
        }
        returnPagerString+="</div>";

        return returnPagerString;
    },
    getPageSize: function (size)
    {
        if (!size || isNaN(size))
        {
            size = $.cookies.get("pagesize");
        }
        if (!size || isNaN(size))
        {
            size = 15;
        }
        return size;
    },
    getPageNumber: function(number)
    {
        if (isNaN(number))
        {
            number = 1;
        }
        return number;
    },
    showWait: function (msg)
    {
        var html = '<div id="showwaitdiv" style="display:none;width:200px;text-align:center;padding-top:15px;">';
        html += '<img src="' + this.rooturl() + '/images/loading/load4.gif" style="vertical-align:middle; padding-right:6px;"/>';
        html += msg || '正在加载...';
        html += '</div>';
        $(window.document.body).append(html);
        new RoadUI.Window().open({ elementid: "showwaitdiv", opener:window, width:200, height:50, showclose:false, showtitle:false, ismodal:true});
    },
    closeWait: function ()
    {
        //$("#showwaitdiv").remove();
        new RoadUI.Window().close('showwaitdiv');
    },
    ajaxContentType: "application/json; charset=utf-8",
    showError: function (json)
    {
        var errjson = JSON.parse(json.responseText);
        alert(errjson.Message);
    },
    checkLogin: function (json)
    {
        if (!json)
        {
            return false;
        }
        if (json.loginstatus && json.loginstatus == -1)
        {
            top.login(json.url || "");
            return false;
        }
        return true;
    },
    serializeForm: function ($form)
    {
        var o = {};
        var a = $form.serializeArray();
        $.each(a, function ()
        {
            if (o[this.name] !== undefined)
            {
                if (!o[this.name].push)
                {
                    //o[this.name] = [o[this.name]];
                    this.value = o[this.name] + "," + (this.value || '');
                }
                o[this.name] = this.value || '';
            } else
            {
                o[this.name] = this.value || '';
            }
        });
        return o;
    }
};

RoadUI.Xml = {
    getXmlDom: function ()
    {
        var xmldoc;
        if (window.ActiveXObject)
        {
            xmldoc = new ActiveXObject("Microsoft.XMLDOM");
        }
        else
        {
            if (document.implementation && document.implementation.createDocument)
            {
                xmldoc = document.implementation.createDocument("", "doc", null);
            }
        }
        return xmldoc;
    },
    loadXML: function (xml)
    {
        var xmldoc = RoadUI.Xml.getXmlDom();
        xmldoc.async = false;
        try
        {
            xmldoc.loadXML(xml);
        }
        catch (e)
        {
            xmldoc = new DOMParser().parseFromString(xml, "text/xml");
        }
        return xmldoc;
    },
    getElementValue: function (elements)
    {
        return elements && elements.length > 0 && elements[0].firstChild ? elements[0].firstChild.nodeValue : "";
    }
};

String.prototype.isInteger = function ()
{
    return (new RegExp(/^\d+$/).test(this));
};
String.prototype.isNumber = function (value, element)
{
    return (new RegExp(/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/).test(this));
};
String.prototype.trim = function ()
{
    return this.replace(/(^\s*)|(\s*$)/g, "");
};
String.prototype.isNullOrEmpty =function()
{
    return !this || this.length == 0 || this.trim().length == 0;
};
String.prototype.startWith = function (pattern)
{
    return this.indexOf(pattern) === 0;
};
String.prototype.endWith = function (pattern)
{
    var d = this.length - pattern.length;
    return d >= 0 && this.lastIndexOf(pattern) === d;
};
String.prototype.encodeTXT = function ()
{
    return (this).replaceAll('&', '&amp;').replaceAll("<", "&lt;").replaceAll(">", "&gt;").replaceAll(" ", "&nbsp;");
};
String.prototype.isMail = function ()
{
    return (new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(this.trim()));
};
String.prototype.isPhone = function ()
{
    return (new RegExp(/(^([0-9]{3,4}[-])?\d{3,8}(-\d{1,6})?$)|(^\([0-9]{3,4}\)\d{3,8}(\(\d{1,6}\))?$)|(^\d{3,8}$)/).test(this));
};
String.prototype.isUrl = function ()
{
    return (new RegExp(/^[a-zA-z]+:\/\/([a-zA-Z0-9\-\.]+)([-\w .\/?%&=:]*)$/).test(this));
};
String.prototype.isExternalUrl = function ()
{
    return this.isUrl() && this.indexOf("://" + document.domain) == -1;
};
String.prototype.replaceAll = function (s1, s2, ignoreCase)
{
    var str = this;
    if ('.' == s1)
    {
        while (str.indexOf(s1) != -1)
        {
            str = str.replace(s1, s2);
        }
        return str;
    }
    else
    {
        if (!RegExp.prototype.isPrototypeOf(s1))
        {
            return str.replace(new RegExp(s1, (ignoreCase ? "gi" : "g")), s2);
        }
        else
        {
            return str.replace(s1, s2);
        }
    }
};
String.prototype.isDate = function ()
{
    var str = this;
    var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
    var r = str.match(reg);
    if (r == null)
    {
        reg = /^(\d+)\/(\d{1,2})\/(\d{1,2})$/;
        r = str.match(reg);
    }
    if (r == null) return false;
    r[2] = r[2] - 1;
    var d = new Date(r[1], r[2], r[3]);
    if (d.getFullYear() != r[1]) return false;
    if (d.getMonth() != r[2]) return false;
    if (d.getDate() != r[3]) return false;
    return true;
}
String.prototype.isDateTime = function ()
{
    var str = this;
    var reg = /^(\d+)-(\d{1,2})-(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    var r = str.match(reg);
    if (r == null)
    {
        reg = /^(\d+)\/(\d{1,2})\/(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        r = str.match(reg);
    }
    if (r == null) return false;
    r[2] = r[2] - 1;
    var d = new Date(r[1], r[2], r[3], r[4], r[5], r[6]);
    if (d.getFullYear() != r[1]) return false;
    if (d.getMonth() != r[2]) return false;
    if (d.getDate() != r[3]) return false;
    if (d.getHours() != r[4]) return false;
    if (d.getMinutes() != r[5]) return false;
    if (d.getSeconds() != r[6]) return false;
    return true;
}
String.prototype.removeHtml = function ()
{
    var str = this;
    return str.replace(/<[^>]+>/g, "");//去掉所有的html标记
};
Date.prototype.format = function (format)
{
    var o = {
        "M+": this.getMonth() + 1, 
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }
    if (/(y+)/.test(format))
    {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o)
    {
        if (new RegExp("(" + k + ")").test(format))
        {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};
Array.prototype.unique = function ()
{
    var res = [];
    var json = {};
    for (var i = 0; i < this.length; i++)
    {
        if (!json[this[i]])
        {
            res.push(this[i]);
            json[this[i]] = 1;
        }
    }
    return res;
};
Array.prototype.remove = function (obj)
{
    for (var i = 0; i < this.length; i++)
    {
        var temp = this[i];
        if (!isNaN(obj))
        {
            temp = i;
        }
        if (temp == obj)
        {
            for (var j = i; j < this.length; j++)
            {
                this[j] = this[j + 1];
            }
            this.length = this.length - 1;
        }
    }
};

var currentFocusObj = null; //当前焦点对象
function initElement($elements, type)
{
    return;
    //if (!$elements || $elements.size() == 0)
    //{
    //    return;
    //}
    
    //var cssType = type;
    //$elements.addClass(cssType + "1")
    //.bind("mouseover", function ()
    //{
    //    $(this).removeClass().addClass(cssType + ("select" == cssType ? "3" : "2"));
    //}).bind("mouseout", function ()
    //{
    //    if (currentFocusObj == null || $(this).get(0) !== currentFocusObj)
    //    {
    //        $(this).removeClass().addClass(cssType + "1");
    //    }
    //}).bind("focus", function ()
    //{
    //    if (currentFocusObj != null)
    //    {
    //        var css = $(currentFocusObj).attr("class").replace("1", "").replace("2", "").replace("3", "");
    //        $(currentFocusObj).removeClass().addClass(css + "1");
    //    }

    //    $(this).removeClass().addClass(cssType + ("select" == cssType ? "3" : "2"));
    //    currentFocusObj = $(this).get(0);
    //});
}

//处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外  
function banBackSpace(e)
{
    var ev = e || window.event;//获取event对象     
    var obj = ev.target || ev.srcElement;//获取事件源     
    var t = obj.type || obj.getAttribute('type');//获取事件源类型    
    //获取作为判断条件的事件类型  
    var vReadOnly = obj.getAttribute('readonly');
    var vEnabled = obj.getAttribute('enabled');
    //处理null值情况  
    vReadOnly = (vReadOnly == null) ? false : vReadOnly;
    vEnabled = (vEnabled == null) ? true : vEnabled;
    //当敲Backspace键时，事件源类型为密码或单行、多行文本的，  
    //并且readonly属性为true或enabled属性为false的，则退格键失效  
    var flag1 = (ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea" || t == "search")
                && (vReadOnly == true || vEnabled != true)) ? true : false;
    //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效  
    var flag2 = (ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea" && t != "search")
                ? true : false;
    //判断  
    if (flag2)
    {
        return false;
    }
    if (flag1)
    {
        return false;
    }
}

//禁止后退键 作用于Firefox、Opera  
document.onkeypress = banBackSpace;
//禁止后退键  作用于IE、Chrome  
document.onkeydown = banBackSpace;