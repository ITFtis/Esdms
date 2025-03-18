$(document).ready(function () {

    douoptions.title = '專案請款基本資料';

    //產製專家請款單
    var a = {};
    a.item = '<span class="ps-1"></span>' + '<span id="BtnExportInvoice" class="btn btn-sm btn-light glyphicon"> 請款單</span>';
    a.event = 'click #BtnExportInvoice';
    a.callback = function ExportInvoice(evt, value, row, index) {
        helper.misc.showBusyIndicator();
        $.ajax({
            url: app.siteRoot + 'ProjectInvoice/ExportInvoice',
            datatype: "json",
            type: "POST",
            data: {
                //"waterColor": waterColor,
                "id": row.Id
            },
            success: function (datas) {
                $(datas).each(function (index) {
                    var data = this;
                    var aryUrl = [];

                    if (data.result) {
                        //多檔案下載連結 <a>
                        const link = document.createElement('a');
                        link.href = app.siteRoot + data.url;
                        link.download = data.fileName; // 假设文件为PDF格式，可以根据实际情况修改
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    } else {
                        alert("匯出失敗：\n" + data.errorMessage);
                    }
                })
            },
            complete: function () {
                helper.misc.hideBusyIndicator();
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                helper.misc.hideBusyIndicator();
            }
        });
    }

    douoptions.appendCustomFuncs = [a];

    douoptions.afterCreateEditDataForm = function ($container, row) {

        var isAdd = JSON.stringify(row) == '{}';

        ////////(關)名稱提示下拉
        ////$('.data-edit-form-group').find('[data-fn="PrjId"]').next().empty();
        ////$('.data-edit-form-group').find('[data-fn="PrjName"]').next().empty();

        ////if (isAdd) {
        ////    //預設當年度
        ////    $('.data-edit-form-group').find('[data-fn="PrjYear"] option').eq(1).prop('selected', true);
        ////}

        var $_oform = $("#_tabs");

        //專案員工配置
        $_d5EditDataContainer = $('<table>').appendTo($_oform.parent());

        var oId = row.Id;

        if (isAdd) {
        }
        else {
            //1-n 專案員工配置
            SetDouDa5(row, row.ProjectInvoiceBasics, oId);

            helper.bootstrap.genBootstrapTabpanel($_d5EditDataContainer.parent(), undefined, undefined,
                ['專案請款基本資料', '請款專家學者明細'],
                [$_oform, $_d5EditDataContainer]);
        }

        //修改按鈕設定
        var editBtn = '';

        //按鈕「返回專案清單」
        if ($_masterTable != undefined) {
            if (isAdd) {
                //長在footer右邊
                var back = '<button id="btnBack" class="btn btn-secondary">返回專家請款明細清單</button>';
                $(back).appendTo($container.find('.modal-footer'));
            }
            else {
                //長在Tab右邊
                var back = '<button id="btnBack" class="btn btn-secondary">返回專家請款明細清單</button>';
                editBtn = back;
            }
        }

        //檢視用
        var $modal = $('.projectinvoicecontroller .modal-dialog');
        $modal.find('[data-fn="PrjName"]').attr("disabled", true);
        $modal.find('[data-fn="PrjCommissionedUnit"]').attr("disabled", true);
        $modal.find('[data-fn="PrjPjNoM"]').attr("disabled", true);
        $modal.find('[data-fn="PrjStartDate"] input').attr("disabled", true);
        $modal.find('[data-fn="PrjEndDate"] input').attr("disabled", true);

        if (!isAdd) {
            editBtn = '<li class="ms-auto">' + editBtn + '</li>';
            $(editBtn).appendTo($('#_tabs').closest('div[class=tab-content]').siblings());

            //修改，不能變更專案編號
            $modal.find('[data-fn="PrjId"]').attr("disabled", true);
        }

        $('#btnBack').click(function () {
            location.reload();
        });

        //保留確定按鈕
        $container.find('.modal-footer button').hide();
        $container.find('.modal-footer').find('.btn-primary').show();
        
        //必填
        var $p1 = $modal.find('div[data-field=PrjId]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">*必填</span>';
        $(remind).appendTo($p1);

        var $p2 = $modal.find('div[data-field=WorkItem]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">*必填</span>';
        $(remind).appendTo($p2);

        var $p3 = $modal.find('div[data-field=CostCode]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">*必填</span>';
        $(remind).appendTo($p3);

        //選擇Project
        $('.projectinvoicecontroller .modal-dialog').find('[data-fn="PrjId"]').autocomplete({
            change: function (event, ui) {
                var prjId = $(this).val();
                $.ajax({
                    url: app.siteRoot + 'ProjectInvoice/GetProject',
                    datatype: "json",
                    type: "Get",
                    data: { prjId: prjId },
                    async: false,
                    appendTo: $('.projectassigncontroller .modal-dialog'),
                    success: function (data) {
                        var $modal = $('.projectinvoicecontroller .modal-dialog');
                        if (data != null) {                            
                            $modal.find('[data-fn="PrjName"]').val(data.Name);
                            $modal.find('[data-fn="PrjCommissionedUnit"]').val(data.CommissionedUnit);
                            $modal.find('[data-fn="PrjPjNoM"]').val(data.PjNoM);                            
                            $modal.find('[data-fn="PrjStartDate"] input').val(JsonDateStr2Datetime(data.PrjStartDate).DateFormat("yyyy-MM-dd"))
                            $modal.find('[data-fn="PrjEndDate"] input').val(JsonDateStr2Datetime(data.PrjEndDate).DateFormat("yyyy-MM-dd"))                            
                        }
                        else {
                            $modal.find('[data-fn="PrjCommissionedUnit"]').val('');
                            $modal.find('[data-fn="PrjPjNoM"]').val('');
                            $modal.find('[data-fn="PrjStartDate"] input').val('');
                            $modal.find('[data-fn="PrjEndDate"] input').val('');

                            alert("查無此專案編號：" + prjId);
                        }
                    }
                });
            },
            source: function (request, response) {
                $.ajax({
                    url: app.siteRoot + 'ProjectInvoice/GetAutocompleteProject',
                    datatype: "json",
                    type: "Get",
                    data: { searchKeyword: request.term },
                    async: false,
                    appendTo: $('.projectassigncontroller .modal-dialog'),
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                value: obj.PrjId,
                                label: obj.PrjId + " " + obj.Name + (obj.PjNoM != null ? " (財)" + obj.PjNoM : ""),
                            };
                        }));
                    }
                });
            },
            delay: 0,
            minLength: 0,
        }).on('focus', function () { $(this).keydown(); });
    };

    douoptions.afterUpdateServerData = function (row, callback) {
        jspAlertMsg($("body"), { autoclose: 2000, content: '基本資料更新成功!!', classes: 'modal-sm' },
            function () {
                $('html,body').animate({ scrollTop: $_masterTable.offset().top }, "show");
            });

        //(no callback)更新dou的rowdata
        $_masterTable.instance.updateDatas(row);

        //callback();
    }

    $_masterTable = $("#_table").DouEditableTable(douoptions).on($.dou.events.add, function (e, row) {

        //錨點
        var aPoint = $_masterTable.instance.settings.rootParentContainer;
        $('html,body').animate({ scrollTop: $(aPoint).offset().top }, "show");

        //trigger清單(新增row)編輯按鈕的，
        $_masterTable.DouEditableTable("editSpecificData", row);

    });

    function SetDouDa5(orow, datas, MId) {
        $.getJSON($.AppConfigOptions.baseurl + 'ProjectInvoiceBasic/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '請款專家學者明細';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            _opt.ctrlFieldAlign = "left";
            //douHelper.getField(_opt.fields, 'MapPrjId').editable = false;

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.MId = MId;

                callback();
            };

            ////_opt.afterCreateEditDataForm = function ($container, row) {

            ////    //選擇Fno
            ////    $('.projectassigncontroller .modal-dialog').find('[data-fn="Fno"]').autocomplete({
            ////        source: function (request, response) {
            ////            $.ajax({
            ////                url: app.siteRoot + 'ProjectAssign/GetProjectFno',
            ////                datatype: "json",
            ////                type: "Get",
            ////                data: { searchKeyword: request.term },
            ////                async: false,
            ////                appendTo: $('.projectassigncontroller .modal-dialog'),
            ////                success: function (data) {
            ////                    response($.map(data, function (obj) {
            ////                        return {
            ////                            value: obj.Key,
            ////                            label: obj.Value,
            ////                        };
            ////                    }));
            ////                }
            ////            });
            ////        },
            ////        delay: 0,
            ////        minLength: 0,
            ////    }).on('focus', function () { $(this).keydown(); });
            ////}

            //實體Dou js
            $_d5Table = $_d5EditDataContainer.douTable(_opt);
        });
    };
})