//文件上传
; RoadUI.File = function ()
{
    var instance = this;
    this.init = function ($files)
    {
        $files.each(function ()
        {
            $file = $(this);
            var id = $file.attr("id") || "";
            var name = $file.attr("name") || "";
            var filetype = $file.attr("filetype") || "";
            var value = $file.val();
            var validate = $file.attr("validate") || "";
            var ismobile = "1" == RoadUI.Core.query("ismobile");
            if (name.length == 0)
            {
                name = id;
            }
            var $hide = $('<input type="hidden" id="' + id + '" name="' + name + '" value="" ' + (validate && validate.length > 0 ? 'validate="' + validate + '"' : '') + '/>');
            var $but = $('<input type="button" class="mybutton" style="margin:0;" value="附件" />');
            $file.attr("id", id + "_text");
            $file.attr("name", name + "_text");
            $file.attr("readonly", "readonly");
            $file.removeClass().addClass("mytext");
            $file.css({"border-right": "0" });
            $hide.val(value);
            if (value.length > 0)
            {
                $file.val('共' + value.split('|').length + '个文件');
            }
            $but.bind("click", function ()
            {
                var val = $(this).prev().prev().val();
                new RoadUI.Window().open({ id: "file_" + id, url: "/Controls/UploadFiles/" + (ismobile ? "Index_App" : "Index") + "?eid=" + id + "&files=" + val + "&filetype=" + filetype, width: ismobile ? 320 : 800, height: ismobile ? 300 : 500, title: "附件", showclose: true });
            });
            if (validate && validate.length > 0)
            {
                $file.removeAttr("validate");
                //$but.after('<span class="msg"></span>');
            }
            $file.after($but).before($hide);
        });
    };
}