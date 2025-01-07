//TreeTable
; RoadUI.TreeTable = function ()
{
    var instance = this;
    var childs = [];
    var parents = [];
    var openimg = RoadUI.Core.rooturl() + '/images/ico/folder_classic_opened.png';
    var closeimg = RoadUI.Core.rooturl() + '/images/ico/folder_classic.png';
    var leafimg = RoadUI.Core.rooturl() + '/images/ico/topic.gif';
    this.init = function (options)
    {
        var defaults = {
            id: "",
            $table: null
        };
        this.opts = $.extend(defaults, options);
        if (!this.opts.$table)
        {
            this.opts.$table = $("#" + this.opts.id);
        }
        if (!this.opts.$table)
        {
            throw "要初始化的对象为空!"; return false;
        }
        var $trs = $("tbody tr", this.opts.$table);
        
        for (var i = 0; i < $trs.size() ; i++)
        {
            var $tr = $trs.eq(i);
            var id = $tr.attr("id");
            var pid = $tr.attr("data-pid") || "0";
            var depth = this.getDepth(pid);
            $tr.attr("data-depth", depth);
            var $td = $("td:first", $tr);
            $td.css({ "padding-left": (depth * 16 + 5).toString() + "px" });
            var hasChilds = this.hasChilds(id);
            var imgsrc = "";
            var img = '';
            if (hasChilds)
            {
                imgsrc = openimg;
                img = '<img style="vertical-align:middle;" onclick="new RoadUI.TreeTable().toggleChilds(this);" src="' + imgsrc + '" />';
            }
            else
            {
                imgsrc = leafimg;
                img = '<img style="vertical-align:middle;" src="' + imgsrc + '" />';
            }
            
            $td.html(img + '<span style="vertical-align:middle;">' + $td.html() + '</span>');
        }
    };

    this.toggleChilds = function (img)
    {
        var $tr = $(img).parent().parent();
        childs = [];
        this.getChilds($tr);
        var imgsrc = $(img).attr("src");
        var isOpen = imgsrc.indexOf(openimg) >= 0;
        var isHide = imgsrc.indexOf(closeimg) >= 0;
        if (isOpen || isHide)
        {
            if (isOpen)
            {
                $(img).attr("src", closeimg);
            }
            else
            {
                $(img).attr("src", openimg);
            }
        }
        for (var i = 0; i < childs.length; i++)
        {
            var img1 = childs[i].find('img');
            var imgsrc1 = $(img1).attr("src");
            var isOpen1 = imgsrc1.indexOf(openimg) >= 0;

            if (isOpen)
            {
                childs[i].hide();
            }
            else
            {
                childs[i].show();
            }
            if (isOpen1)
            {
                new RoadUI.TreeTable().toggleChilds1(img1);
            }
        }
    };

    this.toggleChilds1 = function (img)
    {
        var $tr = $(img).parent().parent();
        childs = [];
        this.getChilds($tr);
       
        for (var i = 0; i < childs.length; i++)
        {
            var img1 = childs[i].find('img');
            var imgsrc1 = $(img1).attr("src");
            var isOpen1 = imgsrc1.indexOf(openimg) >= 0;
            var isHide = childs[i].is(":hidden");
            if (isHide)
            {
                childs[i].show();
            }
            else
            {
                childs[i].hide();
            }
            if (isOpen1)
            {
                new RoadUI.TreeTable().toggleChilds1(img1);
            }

        }
    }
    
    this.hasChilds = function (id)
    {
        var $tr = $("[data-pid='" + id + "']", this.opts.$table);
        return $tr.size() > 0;
    };
    this.getChilds = function ($tr)
    {
        var id = $tr.attr("id");
        var $tr1 = $("[data-pid='" + id + "']", $tr.parent());
        if ($tr1.size() > 0)
        {
            for (var i = 0; i < $tr1.size() ; i++)
            {
                childs.push($tr1.eq(i));
            }
        }
    };
    this.getAllChilds = function ($tr)
    {
        var id = $tr.attr("id");
        var $tr1 = $("[data-pid='" + id + "']", $tr.parent());
        if ($tr1.size() > 0)
        {
            for (var i = 0; i < $tr1.size() ; i++)
            {
                childs.push($tr1.eq(i));
                this.getAllChilds($tr1.eq(i));
            }
        }
    };
    this.getParent = function ($tr)
    {
        var pid = $tr.attr("data-pid");
        var $tr1 = $("#" + pid , $tr.parent());
        if ($tr1.size() > 0)
        {
            this.parents.push($tr1);
        }
    };
    this.getAllParents = function ($tr)
    {
        var pid = $tr.attr("data-pid");
        var $tr1 = $("#" + pid, $tr.parent());
        if ($tr1.size() > 0)
        {
            this.parents.push($tr1);
            this.getAllParents($tr1);
        }
    };

    this.getDepth = function (id)
    {
        var $tr = $("#" + id + "", this.opts.$table);
        if ($tr.size() > 0)
        {
            var parentDepth = this.getDepth($tr.attr("data-pid"));
            return parentDepth + 1;
        }
        return 0;
    };

}