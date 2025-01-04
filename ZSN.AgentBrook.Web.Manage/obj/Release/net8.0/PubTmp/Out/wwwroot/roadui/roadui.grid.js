//Grid
; RoadUI.Grid = function (options)
{
    var defaults = {
        container: $(),
        table: $(), //源表格
        tableid: '', //源表格ID
        pager:$(),//包含分页代码的控件
        width: '', //列表宽度
        height: '', //列表高度
        sort: true, //是否可以排序
        sorttype: 'desc',//初始排序方式
        oddcolor: true, //是否隔行换色
        clickColor: false, //是否点击换色
        resize: true, //是否可以调整列宽
        showpager: true, //是否显示分页
        datatype: '', //数据源类型 html,json
        dataurl: '', //获取数据的地址
        instancename: '', //实例名称
        fixcol: 0, //要冻结的列
        querystring: '', //参数，分页时用
        hasdata: true, //是否已有数据，如果表格已有数据则不需要载入数据
        uniquecol: -1, //标识列
        contextmenu: true //是否有右键菜单
    };

    this.opts = $.extend(defaults, options);
    if (!this.opts.table || this.opts.table.size() == 0)
    {
        var tableid = this.opts.tableid;
        if (tableid && $.trim(tableid).length > 0)
        {
            this.opts.table = $("#" + $.trim(tableid));
        }
    }
    if (!this.opts.table || this.opts.table.size() == 0)
    {
        throw "要初始化的表格为空";
        return false; 
    }
    this.opts.isIE6 = !-[1, ] && !window.XMLHttpRequest;
    if (this.opts.isIE6)
    {
        this.opts.resize = false;
    }
    var instance = this;

    this.init = function ()
    {
        var $div = $('<div class="grid"></div>');
        var $divpager = $('<div class="gridpager"></div>');
        var $divhead = $('<div class="gridhead"></div>');
        var $divbody = $('<div class="gridbody"></div>');
        var $divlist = $('<div class="gridlist"></div>');

        var tablehtml = instance.opts.table.get(0).outerHTML;
        $divhead.append(tablehtml);
        var $table = $('table', $divhead);
        $table.attr('border', '0');
        $table.attr('cellpadding', '0');
        $table.attr('cellspacing', '0');
        $table.css('width', '100%');
        $table.removeAttr('id');
        var $table1 = $table.clone();
        $('thead', $table1).remove();
        $divbody.append($table1);
        $divlist.append($divhead, $divbody);
        $div.append($divlist);
        if (instance.opts.showpager)
        {
            $div.append($divpager);
            var $pager = instance.opts.pager;
            var pagerhtml = "";
            if ($pager && $pager > 0)
            {
                pagerhtml = $pager.html();
            }
            else
            {
                $pager = instance.opts.table.next();
                pagerhtml = $pager.html();
            }
            if ($.trim(pagerhtml).length > 0)
            {
                $divpager.html(pagerhtml);
            }
            else if(instance.opts.hasdata)
            {
                $divpager.remove();
            }
            $pager.remove();
        }
        $divhead.wrap('<div class="gridheadwrap"></div>');
        instance.opts.table.before($div);
        instance.opts.table.remove();
        $('tbody', $table).remove();
        instance.opts.container = $div;
        if (instance.opts.showpager)
        {
            if (!instance.opts.height)
            {
                instance.opts.height = $(window).height() - instance.opts.container.get(0).offsetTop - 4;
            }
            if (instance.opts.height)
            {
                $div.css("height", instance.opts.height.toString() + "px");
                $divbody.css("height", (instance.opts.height - 58).toString() + "px");
            }
        }
        //$(window.document.body).css("overflow", "hidden");
        if (instance.opts.width)
        {
            $divhead.width(instance.opts.width);
            $divbody.width(instance.opts.width);
        }
        else
        {
            $divhead.css("width", '100%');
            $divbody.css("width", '100%');
        }
        $divbody.bind("scroll", function ()
        {
            var left = $divbody.get(0).scrollLeft;
            $divhead.css('left', left - left * 2);
        });
        $firsttr = $("tbody tr:first", $table1);
        if ($firsttr.size() > 0)
        {
            var $tds = $("td", $firsttr);
            var $tds1 = $("thead tr th", $table);
            if ($tds.size() == $tds1.size())
            {
                for (var i = 0; i < $tds1.size() ; i++)
                {
                    var txt = $tds1.eq(i).html();
                    var sort = $tds1.eq(i).attr("sort") || "1";
                    var title = '<div class="gridheadtitle">' + txt + '</div>';
                    title += '<div class="gridheadsort">';
                    if (instance.opts.sort && "1" == sort)
                    {
                        title += '<div class="gridheadsortempty" colindex="' + i.toString() + '"></div>';
                    }
                    if (instance.opts.resize)
                    {
                        title += '<div class="gridheadresize" colindex="' + i.toString() + '">&nbsp;</div>';
                    }
                    title += '</div>';
                    $tds1.eq(i).html('').html(title);
                    $('.gridheadresize', $tds1.eq(i)).bind('mousedown', function (e)
                    {
                        var $divlist = $('.gridlist', instance.opts.container);
                        var $divline = $('<div class="gridheadresizeline" colindex="' + $(this).attr('colindex') + '"></div>');
                        var $maskdiv = $('<div class="gridmask"></div>');

                        $divline.css('left', (event || e).clientX);
                        $divlist.append($divline);
                        $divlist.append($maskdiv);
                        $divline.bind('mouseup', function (e)
                        {
                            $(this).remove();
                            $('.gridmask', instance.opts.container).remove();
                            var $ths = $('.gridhead table thead tr th', instance.opts.container);
                            var idx = parseInt($(this).attr('colindex'));
                            var $headTable = $(".gridhead table", instance.opts.container);
                            //var $bodyTable = $(".gridbody table", instance.opts.container);
                            var thsWidth = $ths.eq(idx).width() + $ths.eq(idx).offset().left;
                            var addWidth = (event || e).clientX - thsWidth;
                            var tableWidth = $headTable.width() + addWidth;
                            
                            $ths.eq(idx).width($ths.eq(idx).width() + addWidth);
                            var jianWidth = addWidth / $ths.size() - idx + 1;
                            for (var i = idx + 1; i < $ths.size() ; i++)
                            {
                                $ths.eq(i).width($ths.eq(i).width() - jianWidth);
                            }
                            instance.resetWidth();
                            //document.body.onselectstart = function () { return true; };
                        });
                        $maskdiv.bind('mousemove', function (e)
                        {
                            $(this).css('cursor', 'e-resize');
                            $('.gridheadresizeline', instance.opts.container).css({ "left": (event || e).clientX });
                        });
                    });
                    
                    if (instance.opts.sort && "1" == sort)
                    {
                        $(".gridheadtitle", $tds1.eq(i)).bind('click', function ()
                        {
                            var $sortdiv = $(".gridheadsort div[class^='gridheadsort']", $(this).parent());
                            var className = $sortdiv.attr('class');
                            $sortdiv.removeClass().addClass(className == "gridheadsortdesc" || className == "gridheadsortdesc1" || className == "gridheadsortempty" ? "gridheadsortasc1" : "gridheadsortdesc1");

                            var $tds2 = $('.gridhead table thead tr th', instance.opts.container)
                            var index = 0;
                            for (var i = 0; i < $tds2.size() ; i++)
                            {
                                if ($tds2.eq(i).get(0) === $(this).parent().get(0))
                                {
                                    index = i;
                                }
                                else
                                {
                                    $(".gridheadsort div[class^='gridheadsort']", $tds2.eq(i)).removeClass().addClass("gridheadsortempty");
                                }
                            }
                            instance.sort($('.gridbody table', instance.opts.container), index);
                            instance.oddColor();
                            instance.resetWidth();
                        }).css("cursor", "pointer");
                    }
                }
            }
            instance.resetWidth();
        }
        
        if (instance.opts.oddcolor)
        {
            this.oddColor();
        }
        if (instance.opts.clickColor)
        {
            this.clickColor();
        }
        if ($divbody.height() != 0 && $divbody.height() < $table1.height())//有滚动条的情况
        {
            $divhead.width($divhead.width() - 17);
            this.resetWidth();
        }
    };

    //隔行换色
    this.oddColor = function ()
    {
        var $table = $('.gridbody table', instance.opts.container);
        $("tbody tr:odd td", $table).removeClass().addClass("gridbodytrodd");
        $("tbody tr:even td", $table).removeClass().addClass("gridbodytreven");
    };

    //单击换色
    this.clickColor = function ()
    {
        $('.gridbody table tbody tr', instance.opts.container).bind("click", function ()
        {
            var className = $("td", $(this)).attr("class");
            if (className != "gridbodytrclick")
            {
                $(this).attr("oldclass", className);
                $("td", $(this)).removeClass().addClass("gridbodytrclick");
            }
            else
            {
                $("td", $(this)).removeClass().addClass($(this).attr("oldclass"));
            }
        });
    }

    this.resetWidth = function ()
    {
        var $headtable = $('.gridhead table', instance.opts.container);
        var $bodytable = $('.gridbody table', instance.opts.container)
        var $ths = $('thead tr th', $headtable);
        var $tds = $('tbody tr:first td', $bodytable);
        
        for (var i = 0; i < $ths.size() ; i++)
        {
            $tds.eq(i).width($ths.eq(i).width());
        }
    };

    this.loadData = function (size, number, isInit)
    {
        if ($.trim(instance.opts.dataurl).length == 0)
        {
            throw "获取数据地址为空";
        }
        
        size = size || 15;
        number = number || 1;
        var query = $.trim(instance.opts.querystring);
        if (query.substr(0, 1) != '&')
        {
            if (query.substr(0, 1) == '?')
            {
                query = query.substr(1, query.length);
            }
            query = '&' + query;
        }
        var height = $(window).height();
        var width = $(window).width();
        var $maskdiv = $('<div class="gridmaskdiv"><div>正在加载，请稍候...</div></div>');
        var top1 = $(top).height() / 2 - 120;
        var left1 = $(top).width() / 2 - 300;
        $maskdiv.css({ "left": left1 + "px", "top": top1 + "px" });
        $(window.document.body).append($maskdiv);
        
        $.ajax({
            url: instance.opts.dataurl + '?pagesize=' + size + '&pagenumber=' + number + query, async: true, type: "get", dataType: instance.opts.datatype, success: function (returnData)
            {
                if (returnData && returnData.loginstatus && returnData.loginstatus == -1)
                {
                    top.login();
                    return;
                }
                var tbody = '<tbody>';
                switch (instance.opts.datatype.toLowerCase())
                {
                    case 'json':
                        var data = returnData.data;
                        var count = returnData.count;
                        if ($.isArray(data))
                        {
                            for (var i = 0; i < data.length; i++)
                            {
                                tbody += '<tr>';
                                var unique = instance.opts.uniquecol >= 0 ? data[i][instance.opts.uniquecol] : "";
                                
                                for (var j = 0; j < data[i].length; j++)
                                {
                                    if (j == instance.opts.uniquecol)
                                    {
                                        continue;
                                    }
                                    
                                    tbody += '<td>';
                                    tbody += data[i][j];
                                    tbody += '</td>';
                                }
                                if (instance.opts.operation)
                                {
                                    tbody += '<td>';
                                    tbody += instance.opts.operation.replaceAll("{unique}", unique);
                                    tbody += '</td>';
                                }
                                tbody += '</tr>';
                            }
                        }
                        break;
                    case 'html':
                        tbody += returnData;
                        break;
                }
                tbody += '</tbody>';
                var $table = instance.opts.table;
                if (!$table || $table.size() == 0)
                {
                    $table = $(".gridbody table", instance.opts.container);
                }
                $('tbody', $table).remove();
                $table.append(tbody);
                if (!isInit)
                {
                    instance.opts.container.before($table);
                    instance.opts.container.remove();
                }
                instance.init();
                var pager = RoadUI.Core.getPager(count, size, number, '', instance.opts.instancename);
                if (pager.length > 0)
                {
                    $(".gridpager", instance.opts.container).html(pager);
                }
                else
                {
                    $(".gridpager", instance.opts.container).hide();
                }
                $maskdiv.remove();
            },
            error: function ()
            {

            }
        });
    };

    this.sort = function ($table, Idx)
    {
        var table = $table.get(0);
        var tbody = $('tbody', $table).get(0);
        var tr = tbody.rows;

        var trValue = new Array();
        for (var i = 0; i < tr.length; i++)
        {
            trValue[i] = tr[i];
        }
        if (tbody.sortCol == Idx)
        {
            trValue.reverse();
        }
        else
        {
            trValue.sort(function (tr1, tr2)
            {
                var param1 = tr1.cells[Idx].innerHTML.removeHtml();
                var param2 = tr2.cells[Idx].innerHTML.removeHtml();
                if (!isNaN(param1) && isNaN(param2))//如果参数1为数字，参数2为字符串
                {
                    return -1;
                }
                else if (isNaN(param1) && !isNaN(param2))//如果参数1为字符串，参数2为数字
                {
                    return 1;
                }
                else if (!isNaN(param1) && !isNaN(param2))//如果两个参数均为数字
                {
                    if (parseFloat(param1) > parseFloat(param2)) return 1;
                    if (parseFloat(param1) == parseFloat(param2)) return 0;
                    if (parseFloat(param1) < parseFloat(param2)) return -1;
                }
                else if ((param1.isDate() || param1.isDateTime()) && (param2.isDate() || param2.isDateTime()))//如果两个参数均为日期时间
                {
                    var date1 = Date.parse(param1.replaceAll('/', '-'));
                    var date2 = Date.parse(param2.replaceAll('/', '-'));
                    if (date1 > date2) return 1;
                    if (date1 == date2) return 0;
                    if (date1 < date2) return -1;
                }
                else//如果两个参数均为字符串类型
                {
                    return param1.localeCompare(param2);
                }
            });
        }

        var fragment = document.createDocumentFragment();
        for (var i = 0; i < trValue.length; i++)
        {
            fragment.appendChild(trValue[i]);
        }

        tbody.appendChild(fragment);
        tbody.sortCol = Idx;
    };

    if (!this.opts.hasdata)
    {
        this.loadData(15, 1, true);
    }
    else
    {
        this.init();
    }

    return instance;
}
