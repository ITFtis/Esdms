$(document).ready(function () {

    douoptions.title = '專案請款';

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
                ['專案請款', '專案請款學者明細'],
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

        if (!isAdd) {
            editBtn = '<li class="ms-auto">' + editBtn + '</li>';
            $(editBtn).appendTo($('#_tabs').closest('div[class=tab-content]').siblings());
        }

        $('#btnBack').click(function () {
            location.reload();
        });

        //保留確定按鈕
        $container.find('.modal-footer button').hide();
        $container.find('.modal-footer').find('.btn-primary').show();

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

            _opt.title = '專案請款學者明細';

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