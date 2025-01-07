/*!
 * easyCropper v1.0.0
 */

layui.config({
    base: '/plugs/easyCropper/' //layui自定义layui组件目录,根据自己具体项目结构而定
}).define(['jquery','layer','cropper'],function (exports) {
    var $ = layui.jquery
        ,layer = layui.layer;
    var obj = {
        // 裁剪组件初始化，在界面中加入默认隐藏的裁剪界面
        render: function(e){
            var self = this,
                elem = e.elem,
                defaultImg = e.defaultImg == undefined ? "" : e.defaultImg,
                saveW = e.saveW,
                saveH = e.saveH,
                mark = e.mark,
                size = e.size == undefined ? 1024 : e.size,
                area = e.area,
                url = e.url,
                done = e.done;
            // 根据传入的ID 每个裁剪组件div都有自己的一套ID 这样能实现一个界面多个裁剪组件
            var elemName = elem.replace("#","");
            var cropperDivId = elemName + 'Div';
            var uploadId = elemName + 'Upload';
            var imageId = elemName + 'Image';
            var previewId = elemName + 'Preview';
            console.log(cropperDivId);
            // 注意更改这里css的路径
            var cropperHtml = '<link rel="stylesheet" href="/plugs/easyCropper/cropper.css">\n' +
                '<div class="layui-fluid" style="display: none" id="'+cropperDivId+'">\n' +
                '    <div class="layui-form-item">\n' +
                '        <div class="layui-input-inline layui-btn-container" style="width: auto;">\n' +
                '            <label for="'+uploadId+'" class="layui-btn layui-btn-primary">\n' +
                '                <i class="layui-icon">&#xe67c;</i>选择图片\n' +
                '            </label>\n' +
                '            <input class="layui-upload-file" id="'+uploadId+'" type="file" value="选择图片">\n' +
                '        </div>\n' +
                '        <div class="layui-form-mid layui-word-aux">建议:图片的尺寸'+saveW+'x'+saveH+'px,大小在'+size+'kb以内</div>\n' +
                '    </div>\n' +
                '    <div class="layui-row layui-col-space15">\n' +
                '        <div class="layui-col-xs9">\n' +
                '            <div style="width:600px;height:300px;background-color: rgb(247, 247, 247);">\n' +
                '                <img id="'+imageId+'" src="'+defaultImg+'" >\n' +
                '            </div>\n' +
                '        </div>\n' +
                '        <div class="layui-col-xs3">\n' +
                '            <div id="'+previewId+'" style="width:210px;height:120px;border: 1px solid rgb(200, 200, 200);overflow:hidden">\n' +
                '            </div>\n' +
                '        </div>\n' +
                '    </div>\n' +
                '    <div class="layui-row layui-col-space15">\n' +
                '        <div class="layui-col-xs9">\n' +
                '            <div class="layui-row">\n' +
                '                <div class="layui-col-xs6">\n' +
                '                    <button type="button" class="layui-btn layui-icon layui-icon-left" cropper-event="rotate" data-option="-15" title="Rotate -15 degrees"> 向左旋转15°</button>\n' +
                '                    <button type="button" class="layui-btn layui-icon layui-icon-right" cropper-event="rotate" data-option="15" title="Rotate 15 degrees"> 向右旋转15°</button>\n' +
                '                </div>\n' +
                '                <div class="layui-col-xs5" style="text-align: right;">\n' +
                '                    <button type="button" class="layui-btn layui-icon layui-icon-refresh" cropper-event="reset" title="重置图片"></button>\n' +
                '                </div>\n' +
                '            </div>\n' +
                '        </div>\n' +
                '        <div class="layui-col-xs3">\n' +
                '            <button class="layui-btn layui-btn-fluid" cropper-event="confirmSave" type="button"> 保存修改</button>\n' +
                '        </div>\n' +
                '    </div>\n' +
                '</div>';
            $('body').append(cropperHtml);


            var content = $("#"+cropperDivId)
                ,image = $("#"+imageId)
                ,file = $("#"+uploadId);
            // 裁剪区属性设定
            var options = {
                aspectRatio: saveW / saveH, // 裁剪框比例
                preview: '#'+previewId, // 预览div
                viewMode:1,
                dragMode: 'crop',
                minCropBoxWidth: saveW,
                minCropBoxHeight: saveH,
                guides : false, // 去掉裁剪框里面白色虚线
                responsive:false, // 是否在调整窗口大小的时候重新渲染cropper
                restore:false, // 是否在调整窗口大小后恢复裁剪的区域
                checkCrossOrigin: false // 是否检测跨域
                            };
            // 点击按钮 弹出elem对应的裁剪界面
            $(elem).on('click',function () {
                layer.open({
                    title : '图片裁剪'
                    , type: 1
                    , content: content
                    , area: area
                    , success: function () {
                        image.cropper(options);
                    }
                    , cancel: function (index) {
                        layer.close(index);
                        $(content).hide();
                        image.cropper('destroy');
                    }
                });
            });
            $(".layui-btn").on('click',function () {
                var event = $(this).attr("cropper-event");
                //监听确认保存图像
                if(event === 'confirmSave'){
                    image.crossOrigin='anonymous';//解决跨域图片问题
                    image.cropper("getCroppedCanvas",{
                        width: saveW,
                        height: saveH
                    }).toBlob(function(blob){
                        var formData=new FormData();
                        var timestamp = Date.parse(new Date());
                        var filename = timestamp+'.jpg';
                        formData.append('file',blob,filename);
                        $.ajax({
                            method:"post",
                            url: url, //用于文件上传的服务器端请求地址
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function (result) {
                                $(content).hide();
                                layer.closeAll('page');
                                return done(result);
                                /*
                                if(result.code == 0){
                                    layer.msg("上传成功",{icon: 6});
                                    layer.closeAll('page');
                                    return done(result.msg);
                                }else if(result.code == -1){
                                    layer.msg(result.msg,{icon: 5});
                                }*/
                            },
                            error:function (result) {
                                layer.msg("服务器繁忙,请稍后重试",{icon: 5});
                            }
                        });
                    });
                    //监听旋转
                }else if(event === 'rotate'){
                    var option = $(this).attr('data-option');
                    image.cropper('rotate', option);
                    //重设图片
                }else if(event === 'reset'){
                    image.cropper('reset');
                }
                //文件选择
                file.change(function () {
                    var r= new FileReader();
                    // 拿到传的文件
                    var f=this.files[0];
                    console.dir(f)
                    var uploadFileSize = f.size/1024;
                    if (uploadFileSize > size){
                        layer.msg("上传文件不得超过"+size+"K",{icon:5});
                        return false;
                    }
                    r.readAsDataURL(f);
                    r.onload=function (e) {
                        image.cropper('destroy').attr('src', this.result).cropper(options).cropper('setData',{width:saveW,height:saveH});
                    };
                });
            });
        }

    };
    exports('easyCropper', obj);
});