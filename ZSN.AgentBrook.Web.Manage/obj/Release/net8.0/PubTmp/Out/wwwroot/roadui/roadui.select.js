//下拉选择
; RoadUI.Select = function ()
{
    var instance = this;
    this.init = function ($selects)
    {
        initElement($selects, "select");
    };
    this.init2 = function ($selects)
    {
        $selects.select2({ allowClear: true, placeholder: "" });
    };
}