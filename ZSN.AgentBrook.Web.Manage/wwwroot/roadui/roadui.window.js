//弹出层
; RoadUI.Window = function ()
{
    this.opts = {};
    var instance = this;
    this.open = function (options)
    {
        var defaults = {
            id: "",
            title: "",
            url: "",
            ico: "",
            showico: true,
            showclose: true,
            left: 0,
            top: 0,
            height: 300,
            width: 600,
            ismodal: true, //是否模态窗口
            move: true, //是否可以移动
            zindex: 9999,
            resize: true,
            opener: null,
            openerid: "",
            elementid: "", //要显示的DIV层的内容
            showtitle: true, //是否显示标题栏
            showmaskdiv : true //为模态窗口时是否显示遮照层
        };
        this.opts = $.extend(defaults, options);
        this.opts.opener = this.getOpener();

        this.opts.id = this.getID();
        var $opener = $(this.opts.opener);
        if ($opener == null || $opener.size() == 0)
        {
            throw "父窗口为空!"; return false;
        }

        var $openerBody = $(this.opts.opener.document.body);
        var $openerDocument = $(this.opts.opener.document);
        if ($openerBody == null || $openerBody.size() == 0)
        {
            throw "父窗口body为空!"; return false;
        }

        if (this.opts.left == 0)
        {
            this.opts.left = ($opener.width() - this.opts.width) / 2;
        }
        if (this.opts.top == 0)
        {
            this.opts.top = ($opener.height() - this.opts.height) / 2 + $(this.opts.opener.document).scrollTop();
        }
        if (this.opts.ismodal)//如果是模态窗口，则添加遮罩层
        {
            var $maskdiv = $('<div id="' + this.opts.id + '_maskdiv" class="' + (this.opts.showmaskdiv ? "window_maskdiv" : "window_maskdiv1") + '" style="z-index:' + (this.opts.zindex - 1) + ';"></div>', $openerDocument);
            $maskdiv.width($opener.width()).height(Math.max($openerBody.get(0).clientHeight, $opener.height()));
            $openerBody.append($maskdiv);
        }

        var $maindiv = $('<div id="' + this.opts.id + '" class="window_maindiv" style="left:' + this.opts.left + 'px;top:' + this.opts.top + 'px;width:' + this.opts.width + 'px;height:' + this.opts.height + 'px;z-index:' + this.opts.zindex + ';"></div>', $openerDocument);
        var $titlediv = $('<div id="' + this.opts.id + '_titlediv" class="window_title"></div>', $openerDocument);
        var $dragdiv = $('<div id="' + this.opts.id + '_dragdiv" style="position:absolute;left:0px;top:46px;height:' + (this.opts.height).toString() + 'px;display:none;width:' + (this.opts.width).toString() + 'px;background:#f6f6f6;filter:alpha(opacity=30);-khtml-opacity:0.3;-moz-opacity:.3;opacity:0.3;"></div>', $openerDocument);

        var $titlediv_title = $('<div class="' + (this.opts.showico ? 'window_title_title_ico' : 'window_title_title') + '">' + this.opts.title + '</div>', $openerDocument);
        if (this.opts.showico && this.opts.ico && $.trim(this.opts.ico).length > 0)
        {
            $titlediv_title.css({ 'background-image': 'url(' + this.opts.ico + ')' });
        }
        //双击关闭窗口
        $titlediv_title.bind("dblclick", function () { instance.close($(this).parent().parent().attr("id")); });

        var $titlediv_button = $();
        if (this.opts.showclose)
        {
            $titlediv_button = $('<div class="window_title_button">&nbsp;</div>', $openerDocument);
            $titlediv_button.bind("mouseover", function ()
            {
                $(this).removeClass().addClass("window_title_button1");
            }).bind("mouseout", function ()
            {
                $(this).removeClass().addClass("window_title_button");
            }).bind("click", function ()
            {
                instance.close($(this).parent().parent().attr("id"));
            });
        }
        $titlediv.append($titlediv_title, $titlediv_button, '<div style="clear:both;"></div>');

        var isShowUrl = this.opts.url && $.trim(this.opts.url).length > 0;
        var isShowDiv = this.opts.elementid && $.trim(this.opts.elementid).length > 0;

        var $bodydiv = isShowUrl ? $('<div class="window_body"></div>', $openerDocument) : $('<div class="window_body"></div>');
        var bodydivHeight = this.opts.height - (this.opts.resize ? 39 : 26);
        $bodydiv.css({ "height": bodydivHeight + "px" });
        if (isShowUrl)
        {
            var url = this.opts.url;
            if (url.indexOf('?') >= 0)
            {
                url += "&iframeid=" + this.opts.id + "&openerid=" + this.opts.openerid;
            }
            else
            {
                url += "?iframeid=" + this.opts.id + "&openerid=" + this.opts.openerid;
            }
            url = url.substr(0, 1) == "/" ? RoadUI.Core.rooturl() + url : url;
            var $iframe = $('<div style="-webkit-overflow-scrolling:touch;overflow:auto;"><iframe id="' + this.opts.id + '_iframe" name="' + this.opts.id + '_iframe" src="' + url + '" frameborder="0" marginheight="0" marginwidth="0" border="0" style="border:none 0;margin:0;padding:0;width:100%;height:' + bodydivHeight + 'px;"></iframe></div>', $openerDocument);
            if (this.opts.title.isNullOrEmpty())
            {
                $iframe.bind("load", function ()
                {
                    if (!instance.opts.title || $.trim(instance.opts.title).length == 0)
                    {
                        var title = "";
                        try
                        {
                            title = $("head title", $(this).contents()).html();
                        } catch (e) { }
                        instance.setTitle(title);
                    }
                });
            }
            $bodydiv.append($iframe);
        }
        else if (isShowDiv)
        {
            var $ele = $("#" + this.opts.elementid, $openerDocument);
            $ele.appendTo($bodydiv);
            $ele.show();
        }
        if (this.opts.showtitle)
        {
            $maindiv.append($titlediv);
        }
        $maindiv.append($bodydiv);
        if (this.opts.resize)
        {
            $resizediv = $('<div class="window_resize"><div class="window_resize_img">&nbsp;</div></div>', $openerDocument);
        }
        $maindiv.append($dragdiv);
        if (isShowUrl)
        {
            $openerBody.append($maindiv);
        }
        else if (isShowDiv)
        {
            var $form = $("form", $openerDocument);
            //页面有FORM的时候要将内容加到form下，以便于表单提交
            if ($form.size() > 0)
            {
                $form.append($maindiv);
            }
            else
            {
                $openerBody.append($maindiv);
            }
        }

        var maindiv = $maindiv.get(0);
        var titlediv = $titlediv.get(0);
        //var resizediv = $resizediv ? $resizediv.get(0) : null;
        if (this.opts.move)
        {
            var dragBody = maindiv;
            var handle = titlediv;
            $opener.get(0).Drag.init(handle, dragBody);
            dragBody.onDragStart = function (left, top, mouseX, mouseY)
            {
                $dragdiv.show(); //非ie浏览器下在拖拽时用一个层遮住iframe，以免光标移入iframe失去拖拽响应
            }
            dragBody.onDragEnd = function (left, top, mouseX, mouseY)
            {
                $dragdiv.hide();
            }
        };
        //if (RoadUI.Core.isIe6Or7()) { try { $('#' + this.opts.id + '_maskdiv', $body).bgiframe(); } catch (e) { } }
        return this.opts.id;
    };
    this.getOpener = function ()
    {
        if (this.opts.opener)
        {
            return this.opts.opener;
        }
        if (this.opts.openerid)
        {
            iframesArray = [];
            addIframe(top.document);
            for (var i = 0; i < iframesArray.length; i++)
            {
                if ($(iframesArray[i]).attr("id") == this.opts.openerid || $(iframesArray[i]).attr("name") == this.opts.openerid)
                {
                    return iframesArray[i].contentWindow;
                }
            }
        }
        return top;
    };
    this.getOpenerWindow = function ()
    {
        iframesArray = [];
        var openerid = RoadUI.Core.query("openerid") || "";
        if (openerid && openerid.length > 0)
        {
            openerid += "_iframe";
        }
        var ele = $();
        var iframes = $(top.document).find("iframe");
        if (openerid && openerid.length > 0)
        {
            for (var i = iframes.size() - 1; i >= 0; i--)
            {
                if (openerid && openerid.length > 0 && openerid == iframes.eq(i).attr("id"))
                {
                    return iframes.eq(i).get(0).contentWindow;
                }
            }
        }

        iframesArray.push(top);
        addIframe(top.document);
        for (var i = 0; i < iframesArray.length; i++)
        {
            if (openerid && openerid.length > 0 && openerid == iframesArray[i].attr("id"))
            {
                return iframesArray[i].get(0).contentWindow;
            }
        }

        return null;
    };
    this.getID = function ()
    {
        var id = this.opts.id != null && this.opts.id != undefined && $.trim(this.opts.id).length > 0 ? this.opts.id : "roadui_window_" + Math.random().toString();
        return id.replaceAll('.', '');
    };
    this.setTitle = function (title)
    {
        this.opts.title = title;
        var mainid = this.opts.id;
        var $titlediv = $(">div:first", $("#" + mainid + "_titlediv", $(this.opts.opener.document)));
        if ($titlediv == null || $titlediv.size() == 0)
        {
            return false;
        }
        $titlediv.text(title);
        return true;
    };
    this.close = function (id)
    {
        if (!id || id.trim().length == 0)
        {
            id = RoadUI.Core.query("iframeid");
        }
        return closeWindow(id);
    };
    this.closeAll = function ()
    {
        return closeWindow();
    };
    function closeWindow(id)
    {
        var amount = 0;
        var $maindiv = !id || id.trim().length == 0 ? $("div[id^='roadui_window_']", top.document) : $("#" + id, top.document);
        for (var x = 0; x < $maindiv.size() ; x++)
        {
            try
            {
                $maindiv.eq(x).find("iframe").attr("src", "about:blank");
                CollectGarbage();
            } catch (e) { }
            try
            {
                $("#" + $maindiv.eq(x).attr("id") + "_maskdiv", top.document).remove();
                $maindiv.eq(x).find("iframe").remove();
                $maindiv.eq(x).remove();
                amount++;
            } catch (e) { }
        }

        iframesArray = [];
        addIframe(top.document);
        for (var i = 0; i < iframesArray.length; i++)
        {
            try
            {
                if (!iframesArray[i].contentWindow || !iframesArray[i].contentWindow.document || iframesArray[i].contentWindow.document == null)
                {
                    continue;
                }
            }
            catch (e)
            {
                continue;
            }
            var $maindiv1 = null;
            try
            {
                $maindiv1 = !id || id.trim().length == 0 ? $("div[id^='roadui_window_']", iframesArray[i].contentWindow.document) : $("#" + id, iframesArray[i].contentWindow.document);
            } catch (e) { }
            if ($maindiv1 != null)
            {
                for (var j = 0; j < $maindiv1.size() ; j++)
                {
                    try
                    {
                        $maindiv1.eq(j).find("iframe").attr("src", "about:blank");
                        $("#" + $maindiv1.eq(j).attr("id") + "_maskdiv", iframesArray[i].contentWindow.document).remove();
                        $maindiv1.eq(j).find("iframe").remove();
                        $maindiv1.eq(j).remove();
                        amount++;
                        if (document.all)
                        {
                            CollectGarbage();
                        }
                    } catch (e) { }
                }
            }
        }
        return amount;
    };
    var iframesArray = [];
    this.getOpenerElement = function (id)
    {
        iframesArray = [];
        var openerid = RoadUI.Core.query("openerid") || "";
        if (openerid && openerid.length > 0)
        {
            openerid += "_iframe";
        }
        var ele = $();
        var iframes = $(top.document).find("iframe");
        if (openerid && openerid.length > 0)
        {
            for (var i = iframes.size() - 1; i >= 0; i--)
            {
                if (openerid && openerid.length > 0 && openerid == iframes.eq(i).attr("id"))
                {
                    var obj = iframes.eq(i).get(0).contentWindow.document.getElementById(id);
                    if (obj)
                    {
                        return $(obj);
                    }
                }
            }
        }
        if (ele.size() == 0)
        {
            iframesArray.push(top);
            addIframe(top.document);
            for (var i = 0; i < iframesArray.length; i++)
            {
                var doc = null;
                try
                {
                    doc = iframesArray[i].contentWindow.document;
                }
                catch (e)
                {
                    doc = iframesArray[i].document;
                }
                if (doc)
                {
                    var obj = doc.getElementById(id);
                    if (obj)
                    {
                        iframesArray = [];
                        return $(obj);
                    }
                }
            }
        }
        return ele;
    };

    var addIframe = function (doc)
    {
        try
        {
            var iframes = $(doc).find("iframe");
            for (var i = 0; i < iframes.size() ; i++)
            {
                iframesArray.push(iframes.eq(i).get(0));
                addIframe(iframes.eq(i).get(0).contentWindow.document);
            }
        }
        catch (e) { }
    };

    this.reloadOpener = function (url, id, type) //type为webapi刷新函数 不为空表示webapi刷新
    {
        if (!id || id.trim().length == 0)
        {
            id = RoadUI.Core.query("refreshwindowid");
        }
        if (!id || id.trim().length == 0)
        {
            id = RoadUI.Core.query("openerid");
        }
        if (!id || id.trim().length == 0)
        {
            id = RoadUI.Core.query("iframeid");
        }
        
        iframesArray = [];
        addIframe(top.document);
        var isRefresh = false;
        for (var i = 0; i < iframesArray.length; i++)
        {
            if (id + "_iframe" == $(iframesArray[i]).attr("id") || id == $(iframesArray[i]).attr("id"))
            {
                var win = iframesArray[i].contentWindow;
                if (type && $.trim(type).length>0)
                {
                    eval("win." + type);
                }
                else
                {
                    win.location = !url || $.trim(url).length == 0 ? win.location : url;
                }
                if (!isRefresh)
                {
                    isRefresh = true;
                }
            }
        }
        return isRefresh;
    };
    this.resize = function (width, height)
    {
        if (!width || !height)
        {
            return;
        }
        var $maindiv = $("#" + this.opts.id, $(this.opts.opener.document));
        if ($maindiv == null || $maindiv.size() == 0)
        {
            return;
        }
        $maindiv.css({ "height": height + "px", "width": width + "px" });
        var $bodydiv = $(".window_body", $maindiv);
        if ($bodydiv == null || $bodydiv.size() == 0)
        {
            return;
        }
        var bodydivHeight = height - (this.opts.resize ? 39 : 26);
        $bodydiv.css({ "height": bodydivHeight + "px" });
        var $iframe = $("iframe", $bodydiv);
        if ($iframe && $iframe.size() > 0)
        {
            $iframe.css({ "height": bodydivHeight + "px" });
        }
    }
    var doBothDrag = function (x, y, maindiv)
    {
        if (x < 110)
        {
            x = 110;
        }

        maindiv.style.width = (x - 8) + 'px';
        if (y < 35)
        {
            y = 35;
        }

        maindiv.style.height = (y - 8) + 'px';
        instance.resize(x - 8, y - 8);
    };
    var draging = function (oElement, fnGetPos, fnOnDrag, win)
    {
        win = win || window;
        var obj = this;
        this.oElement = oElement;
        this.fnGetPos = fnGetPos;
        this.fnOnDrag = fnOnDrag;
        this.__oStartOffset__ = { x: 0, y: 0 };

        this.fnOnMouseUp = function (ev)
        {
            obj.stopDrag(win.event || ev);
        };

        this.fnOnMouseMove = function (ev)
        {
            obj.doDrag(win.event || ev);
        };

        this.oElement.onmousedown = function (ev)
        {
            obj.startDrag(win.event || ev);
        };
    }
    draging.prototype.startDrag = function (oEvent)
    {
        var oPos = this.fnGetPos();
        var x = oEvent.clientX;
        var y = oEvent.clientY;

        this.__oStartOffset__.x = x - oPos.x;
        this.__oStartOffset__.y = y - oPos.y;

        if (this.oElement.setCapture)
        {
            this.oElement.setCapture();

            this.oElement.onmouseup = this.fnOnMouseUp;
            this.oElement.onmousemove = this.fnOnMouseMove;
        }
        else
        {
            document.addEventListener("mouseup", this.fnOnMouseUp, true);
            document.addEventListener("mousemove", this.fnOnMouseMove, true);

            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
        }
    };
    draging.prototype.stopDrag = function (oEvent)
    {
        if (this.oElement.releaseCapture)
        {
            this.oElement.releaseCapture();

            this.oElement.onmouseup = null;
            this.oElement.onmousemove = null;
        }
        else
        {
            document.removeEventListener("mouseup", this.fnOnMouseUp, true);
            document.removeEventListener("mousemove", this.fnOnMouseMove, true);

            window.releaseEvents(Event.MOUSE_MOVE | Event.MOUSE_UP);
        }
    };
    draging.prototype.doDrag = function (oEvent)
    {
        var x = oEvent.clientX;
        var y = oEvent.clientY;

        this.fnOnDrag(x - this.__oStartOffset__.x, y - this.__oStartOffset__.y);
    };
}
var Drag = {
    "obj": null,
    "init": function (handle, dragBody, e)
    {
        if (e == null) handle.onmousedown = Drag.start;
        handle.root = dragBody;
        if (isNaN(parseInt(handle.root.style.left))) handle.root.style.left = "0px";
        if (isNaN(parseInt(handle.root.style.top))) handle.root.style.top = "0px";
        handle.root.onDragStart = new Function();
        handle.root.onDragEnd = new Function();
        handle.root.onDrag = new Function();
        if (e != null)
        {
            var handle = Drag.obj = handle;
            e = Drag.fixEvent(e);
            var top = parseInt(handle.root.style.top);
            var left = parseInt(handle.root.style.left);
            handle.root.onDragStart(left, top, e.pageX, e.pageY);
            handle.lastMouseX = e.pageX;
            handle.lastMouseY = e.pageY;
            document.onmousemove = Drag.drag;
            document.onmouseup = Drag.end;
        }
    },
    "start": function (e)
    {
        var handle = Drag.obj = this;
        e = Drag.fixEvent(e);
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);
        try { handle.root.onDragStart(left, top, e.pageX, e.pageY); } catch (e) { }
        handle.lastMouseX = e.pageX;
        handle.lastMouseY = e.pageY;
        document.onmousemove = Drag.drag;
        document.onmouseup = Drag.end;
        return false;
    },
    "drag": function (e)
    {
        e = Drag.fixEvent(e);
        var handle = Drag.obj;
        var mouseY = e.pageY;
        var mouseX = e.pageX;
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);

        if (document.all) { Drag.obj.setCapture(); } else { e.preventDefault(); }; //作用是将所有鼠标事件捕获到handle对象，对于firefox，以用preventDefault来取消事件的默认动作：
        var currentLeft, currentTop;
        currentLeft = left + mouseX - handle.lastMouseX;
        currentTop = top + (mouseY - handle.lastMouseY);
        if (currentLeft < 0) currentLeft = 0;
        if (currentTop < 0) currentTop = 0;
        if (currentLeft > parseInt($(window).width()) - parseInt(handle.root.style.width))
            currentLeft = parseInt($(window).width()) - parseInt(handle.root.style.width);
        if (currentTop > parseInt($(window).height()) - parseInt(handle.root.style.height))
            currentTop = parseInt($(window).height()) - parseInt(handle.root.style.height);
        handle.root.style.left = currentLeft + "px";
        handle.root.style.top = currentTop + "px";
        handle.lastMouseX = mouseX;
        handle.lastMouseY = mouseY;
        handle.root.onDrag(currentLeft, currentTop, e.pageX, e.pageY);
        return false;
    },
    "end": function ()
    {
        if (document.all) { Drag.obj.releaseCapture(); }; //取消所有鼠标事件捕获到handle对象
        document.onmousemove = null;
        document.onmouseup = null;
        try { Drag.obj.root.onDragEnd(parseInt(Drag.obj.root.style.left), parseInt(Drag.obj.root.style.top)); } catch (e) { }
        Drag.obj = null;
    },
    "fixEvent": function (e)//格式化事件参数对象
    {
        var sl = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
        var st = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        if (typeof e == "undefined") e = window.event;
        if (typeof e.layerX == "undefined") e.layerX = e.offsetX;
        if (typeof e.layerY == "undefined") e.layerY = e.offsetY;
        if (typeof e.pageX == "undefined") e.pageX = e.clientX + sl - document.body.clientLeft;
        if (typeof e.pageY == "undefined") e.pageY = e.clientY + st - document.body.clientTop;
        return e;
    }
};
//jquery.bgiframe.pack.js
eval(function (p, a, c, k, e, r) { e = function (c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) r[e(c)] = k[c] || e(c); k = [function (e) { return r[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }('(b($){$.m.E=$.m.g=b(s){h($.x.10&&/6.0/.I(D.B)){s=$.w({c:\'3\',5:\'3\',8:\'3\',d:\'3\',k:M,e:\'F:i;\'},s||{});C a=b(n){f n&&n.t==r?n+\'4\':n},p=\'<o Y="g"W="0"R="-1"e="\'+s.e+\'"\'+\'Q="P:O;N:L;z-H:-1;\'+(s.k!==i?\'G:J(K=\\\'0\\\');\':\'\')+\'c:\'+(s.c==\'3\'?\'7(((l(2.9.j.A)||0)*-1)+\\\'4\\\')\':a(s.c))+\';\'+\'5:\'+(s.5==\'3\'?\'7(((l(2.9.j.y)||0)*-1)+\\\'4\\\')\':a(s.5))+\';\'+\'8:\'+(s.8==\'3\'?\'7(2.9.S+\\\'4\\\')\':a(s.8))+\';\'+\'d:\'+(s.d==\'3\'?\'7(2.9.v+\\\'4\\\')\':a(s.d))+\';\'+\'"/>\';f 2.T(b(){h($(\'> o.g\',2).U==0)2.V(q.X(p),2.u)})}f 2}})(Z);', 62, 63, '||this|auto|px|left||expression|width|parentNode||function|top|height|src|return|bgiframe|if|false|currentStyle|opacity|parseInt|fn||iframe|html|document|Number||constructor|firstChild|offsetHeight|extend|browser|borderLeftWidth||borderTopWidth|userAgent|var|navigator|bgIframe|javascript|filter|index|test|Alpha|Opacity|absolute|true|position|block|display|style|tabindex|offsetWidth|each|length|insertBefore|frameborder|createElement|class|jQuery|msie'.split('|'), 0, {}));
