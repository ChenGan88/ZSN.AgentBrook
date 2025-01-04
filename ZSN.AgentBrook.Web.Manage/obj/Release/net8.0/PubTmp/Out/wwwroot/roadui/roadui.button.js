//按钮
; RoadUI.Button = function ()
{
    var instance = this;
    this.init = function ($buttons)
    {
        return;
        if (!$buttons || $buttons.size() == 0)
        {
            return false;
        }
        $buttons.addClass("button1").bind("mouseover", function ()
        {
            $(this).removeClass().addClass("button2");
        }).bind("mouseout", function ()
        {
            $(this).removeClass().addClass("button1");
        });
        $buttons.each(function ()
        {
            if ($(this).prop("disabled"))
            {
                $(this).unbind("mouseout").unbind("mouseover").removeClass().addClass("buttondisabled");
            }
        });
    };

}
