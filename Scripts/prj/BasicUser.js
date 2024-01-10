$(document).ready(function () {

    var id_toTab;

    //清單欄位排序(非預設編輯頁)
    //說明: js, controller不可設Index排序(douoptions.fields已實體，順序不再變動)
    if (!douoptions.singleDataEdit) {
        //順序設定
        var ranks = [];
        ranks.push({ field: 'strExpertises', index: '10' });

        var newFields = [];
        for (var i = 0; i < douoptions.fields.length; i++) {
            var rank = ranks.find(obj => obj.index == i);
            if (rank != null) {

                //指定(rank)欄位Field
                var f = douoptions.fields.find(obj => obj.field == rank.field);
                newFields.push(f);

                //原欄位Field
                newFields.push(douoptions.fields[i]);
            }
            else {
                if (ranks.find(obj => obj.field == douoptions.fields[i].field) == null) {
                    //原欄位
                    newFields.push(douoptions.fields[i]);
                }
            }
        }
        douoptions.fields = newFields;
    }

    douoptions.tableOptions.pageList = [10, 25, 50, 100, 'All'];

    douoptions.title = '基本資料';

    var $_d1EditDataContainer = undefined;      //Da1s編輯的容器

    var $_d1Table = undefined;  //Da1s Dou實體

    //主表(EmpData) 基本資料
    douoptions.afterCreateEditDataForm = function ($container, row) {

        var isAdd = JSON.stringify(row) == '{}';

        var $_oform = $("#_tabs");
        $_d1EditDataContainer = $('<div>').appendTo($_oform.parent());
        $_d4EditDataContainer = $('<table>').appendTo($_oform.parent());
        //////意見
        ////$_d5EditDataContainer = $('<table>').appendTo($_oform.parent());
        ////////經歷
        ////$_d6EditDataContainer = $('<table>').appendTo($_oform.parent());
        $_d7EditDataContainer = $('<table>').appendTo($_oform.parent());
        //////證照
        ////$_d8EditDataContainer = $('<table>').appendTo($_oform.parent());        

        var oPId = row.PId;
        var isChange = false;
        var isChangeText = [];

        //保留確定按鈕
        $container.find('.modal-footer button').hide();
        $container.find('.modal-footer').find('.btn-primary').show();

        //加提示字
        var $p1 = $('div[data-field=OfficePhone]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(02-23****** #123)</span>';
        $(remind).appendTo($p1);

        var $p2 = $('div[data-field=Fax]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(02-23****** #123)</span>';
        $(remind).appendTo($p2);

        var $p3 = $('div[data-field=PrivatePhone]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(0912******)</span>';
        $(remind).appendTo($p3);

        if (isAdd) {
            //新增隱藏身分代碼, 由Controler(Add)產出
            $('.modal-dialog [data-field="PId"]').hide();
            $('.modal-dialog [data-fn="PId"]').val('-');
        }
        else {            
            //主表新增集合沒資料(預設集合)
            var iniObj = { PId: row.PId };

            //1-1 個人資料
            if (row.BasicUser_Private == undefined) {
                row.BasicUser_Private = iniObj;
            }
            SetDouDa1(row.BasicUser_Private);

            //1-n 專長
            SetDouDa4(row.Expertises, oPId);

            //////1-n 意見
            ////SetDouDa5(row.UserHistoryOpinions, oPId);

            //////1-n 經歷
            ////SetDouDa6(row.Resumes, oPId);

            //1-n 專家參與紀錄
            SetDouDa7(row.FTISUserHistorys, oPId);

            //////1-n 證照
            ////SetDouDa8(row.BasicUser_Licenses, oPId);
        }

        //產tab        
        ////helper.bootstrap.genBootstrapTabpanel($_d4EditDataContainer.parent(), undefined, undefined,
        ////    ['基本資料', '個人資料', '專長', '意見', '經歷', '專家參與紀錄', '證照'],
        ////    [$_oform, $_d1EditDataContainer, $_d4EditDataContainer, $_d5EditDataContainer, $_d6EditDataContainer, $_d7EditDataContainer, $_d8EditDataContainer]);


        helper.bootstrap.genBootstrapTabpanel($_d4EditDataContainer.parent(), undefined, undefined,
            ['基本資料', '個人資料', '專長', '專家參與紀錄'],
            [$_oform, $_d1EditDataContainer, $_d4EditDataContainer, $_d7EditDataContainer]);

        //點選的Tab
        var jTabToggle = $('#_tabs').closest('div[class=tab-content]').siblings().find('a[data-toggle="tab"]');

        if (isAdd) {
            //tablist隱藏
            $('#_tabs').closest('div[class=tab-content]').siblings().hide();
        }

        //修改按鈕設定
        var editBtn = '';

        //按鈕「返回會員資料總表」
        if ($_masterTable != undefined) {
            if (isAdd) {
                //長在footer右邊
                var back = '<button id="btnBack" class="btn btn-secondary">返回專家資料總表</button>';
                $(back).appendTo($container.find('.modal-footer'));
            }
            else {
                //長在Tab右邊
                var back = '<button id="btnBack" class="btn btn-secondary">返回專家資料總表</button>';
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

        //(tab)切換前
        jTabToggle.on('hide.bs.tab', function (e) {
            id_toTab = e.relatedTarget.hash;

            isChange = false;
            isChangeText = [];

            //當下tab item
            var $_nowTabUI = null;    //當下Tab資料 使用的UI;
            var $_nowTable = null;    //當下Tab資料 使用的Dou實體;

            var actTab = $(this).html();
            if (actTab == $_masterTable.instance.settings.title) {
                $_nowTable = $_masterTable;
                $_nowTabUI = $('#_tabs').closest('div[class=tab-content]').find('.show');
            }
            else if (actTab == $_d1Table.instance.settings.title) {
                $_nowTable = $_d1Table;
                $_nowTabUI = $('#_tabs').closest('div[class=tab-content]').find('.show');
            }
            else {
                //不需異動比對(1-n)
                return true;
            }

            //input:輸入值(UI) 容器
            var $_nowContainer = $_nowTabUI.children(":first").toggleClass("data-edit-form-group");

            //欄位驗證成功，錯誤內容關閉
            $(".errormsg", $_nowContainer.find('.modal-dialog')).hide().empty();


            if ($_nowTabUI == null || $_nowTable == null) {
                alert('當下Tab資料取得失敗');
                return false;
            }

            //停止Tab切換
            var tabStop = false;

            //input:輸入值(dou實體)
            var rdataDou = $_nowTable.instance.getData().find(obj => obj.PId == oPId);

            //異動比對
            $.each($_nowTable.instance.settings.fields, function () {
                //欄位名稱
                var fn = this.field;

                //input:輸入值(UI)                
                var uiValue = douHelper.getDataEditContentValue($_nowContainer, this);

                //不需輸入值(UI)
                if (uiValue == null)
                    return; // 等於continue                

                //驗證：必填欄位
                if (!this.allowNull) {
                    if (uiValue == '') {
                        var errors = [];

                        errors.push(this.title + ":" + this.validate(uiValue, rdataDou));
                        var _emsgs = $.isArray(errors) ? errors : [errors];
                        $_nowContainer.find('.modal-dialog').trigger("set-error-message", '<span class="' + $_nowTable.instance.settings.buttonClasses.error_message + '" aria-hidden="true"></span>&nbsp; ' + _emsgs.join('<br><span class="' + $_nowTable.instance.settings.buttonClasses.error_message + '" aria-hidden="true"></span>&nbsp; '));
                        $_nowContainer.find('.modal-dialog').show();
                        $('html,body').animate({ scrollTop: $_masterTable.offset().top }, "show");

                        tabStop = true;
                        return false;
                    }
                }

                //input:輸入值(dou實體)
                var conValue = rdataDou[fn];

                if (conValue != null) {

                    //格式轉換
                    if (uiValue == "") {
                        //有異動不需轉換格式(執行:ui從有值改無值，切換tab)
                    }
                    else if (conValue != "") {
                        //日期格式比對(ui(1982-12-17), con(1982-12-17T00:00:00) => 取小統一長度)
                        if (this.datatype == 'datetime' || this.datatype == 'date') {
                            conValue = JsonDateStr2Datetime(conValue).DateFormat("yyyy/MM/dd HH:mm:ss");
                            var minLength = Math.min(uiValue.length, conValue.length);

                            uiValue = uiValue.substring(0, minLength)

                            //容器(時間可能是物件) "/Date(1224043200000)/"                                
                            conValue = conValue.substring(0, minLength);
                        }
                        else if (this.datatype == 'textarea') {
                            //換行資料庫:\r\n (備:number型別replace錯誤)
                            conValue = conValue.replaceAll('\r\n', '\n');
                        }
                    }

                    if (uiValue != conValue) {
                        isChange = true;
                        isChangeText.push(this.title);
                        //return false;
                    }
                }
                else {
                    //(異動說明)DB為Null($_nowTable無欄位資料),但UI有值
                    if (uiValue != "") {
                        isChange = true;
                        isChangeText.push(this.title);
                        //return false;
                    }
                }

            });

            //停止Tab切換(原因：必填欄位....等問題)
            if (tabStop) {
                return false;
            }

            //異動處理
            if (isChange) {
                //互動訊息
                var msg = '資料異動(' + $_nowTable.instance.settings.title + ')項目：' + '</br>'
                msg += '<ul>';
                $.each(isChangeText, function (index, value) {                    
                    msg += '<li class="mt-2">' + value + '</li>';
                })
                msg += '</ul>';

                var content = msg;

                //confirm挑選取消(重複執行，不知原因)
                var isDoing = false;

                //comfirm(確定儲存,取消還原)
                jspConfirmYesNo($("body"), { content: content }, function (confrim) {
                    if (confrim) {
                        //確定
                        $_nowTabUI.find('.modal-footer').find('.btn-primary').trigger("click");
                    }
                    else {
                        //取消
                        if (isDoing) {
                            //(啟動)tab切換
                            $('a[href="' + id_toTab + '"]').tab('show');
                            id_toTab = undefined;

                            return;
                        }

                        //取消會轉回清單，不可用
                        //$_nowTabUI.find('.modal-footer').find('.btn-default').trigger("click");

                        //還原上一個Tab編輯資料
                        //input:輸入值(Bootstrap Table找編輯資料)
                        var $_bootstrapTable;
                        $('.bootstrap-table #_table').find('.dou-field-PId').each(function (index) {
                            if ($(this).text() == oPId) {
                                $_bootstrapTable = $(this).closest("tr");
                                return false;
                            }
                        });

                        //input:輸入值(dou實體)
                        var rdataDou = $_nowTable.instance.getData().find(obj => obj.PId == oPId);

                        //還原資料異動
                        $_nowTabUI.find('.field-content [data-fn]').each(function (index) {
                            //欄位名稱
                            var fn = $(this).attr('data-fn');
                            var datatype = douHelper.getField($_nowTable.instance.settings.fields, fn).datatype;

                            //輸入值(Bootstrap Table + dou實體)
                            var conValue = '';
                            if (datatype == 'textlist') {
                                //UI(人名)，bootstrapTable(人名)，X容器(員編)
                                conValue = $_bootstrapTable.find('.dou-field-' + fn).text();
                            }
                            else {
                                //var conValue = $_nowTable.instance.getData()[0][fn];
                                conValue = rdataDou[fn];
                            }

                            //conValue(null => DB欄位值Null) ("-" => 輸入提示字：-(沒值))
                            if (conValue == null || conValue == "-")
                                conValue = '';

                            var fn_name = douHelper.getField($_nowTable.instance.settings.fields, fn).title;
                            if (datatype == 'date') {
                                $(this).find('input').val(conValue == '' ? '' : JsonDateStr2Datetime(conValue).DateFormat("yyyy-MM-dd"));
                            }
                            else if (datatype == 'datetime') {
                                $(this).find('input').val(conValue == '' ? '' : JsonDateStr2Datetime(conValue).DateFormat("yyyy-MM-dd HH:mm"));
                            }
                            else {
                                $(this).val(conValue);
                            }
                        });
                    }

                    isDoing = true;
                });

                return false;
            }
        });

    }

    douoptions.addServerData =
        function (row, callback) {
            var PId = $('.field-content [data-fn="PId"]').val();
            var Name = $('.field-content [data-fn="Name"]').val();
            var msg = ExistName(PId, Name);

            if (msg != '') {                
                jspConfirmYesNo($("body"), { content: msg }, function (confrim) {
                    if (confrim) {
                        //確定
                        transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Add', callback);
                    }
                })
            }
            else {
                //確定
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Add', callback);
            }
        };

    douoptions.updateServerData =
        function (row, callback) {
            var PId = $('.field-content [data-fn="PId"]').val();
            var Name = $('.field-content [data-fn="Name"]').val();
            var msg = ExistName(PId, Name);

            if (msg) {                                
                jspConfirmYesNo($("body"), { content: msg }, function (confrim) {
                    if (confrim) {
                        //確定儲存
                        if (id_toTab == undefined) {                            
                            transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Update', callback);
                        }
                        else {
                            transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Update', function () {
                                //(啟動)tab切換
                                $('a[href="' + id_toTab + '"]').tab('show');
                                id_toTab = undefined;
                            });
                        }
                    }
                })
            }
            else {
                //確定儲存
                if (id_toTab == undefined) {
                    transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Update', callback);
                }
                else {
                    transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'BasicUser/Update', function () {
                        //(啟動)tab切換
                        $('a[href="' + id_toTab + '"]').tab('show');
                        id_toTab = undefined;
                    });
                }
            }            
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

    var $_masterTable = $("#_table").DouEditableTable(douoptions).on($.dou.events.add, function (e, row) {

        //錨點
        var aPoint = $_masterTable.instance.settings.rootParentContainer;
        $('html,body').animate({ scrollTop: $(aPoint).offset().top }, "show");

        //trigger清單(新增row)編輯按鈕的，
        $_masterTable.DouEditableTable("editSpecificData", row);

    }); //初始dou table
    function SetDouDa1(datas) {
        $.getJSON($.AppConfigOptions.baseurl + 'BasicUser_Private/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '個人資料';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? [datas] : [{}];
            _opt.datas = datas;

            _opt.singleDataEdit = true;
            _opt.editformWindowStyle = $.editformWindowStyle.showEditformOnly;

            //////初始options預設值
            ////douHelper.setFieldsDefaultAttribute(_opt.fields);//給預設屬性

            _opt.afterCreateEditDataForm = function ($container, row) {
                
                //保留確定按鈕
                $container.find('.modal-footer button').hide();
                $container.find('.modal-footer').find('.btn-primary').show();

                //////加提示字
                ////var $p1 = $('div[data-field=da01]').find('label');
                ////var remind = '<span class="text-danger fw-lighter pull-right">同護照英文姓名</span>';
                ////$(remind).appendTo($p1);

                ////var $p3 = $('div[data-field=da06]').find('label');
                ////var remind = '<span class="text-danger fw-lighter pull-right">請取整數</span>';
                ////$(remind).appendTo($p3);

                ////var $p4 = $('div[data-field=da07]').find('label');
                ////var remind = '<span class="text-danger fw-lighter pull-right">請取整數</span>';
                ////$(remind).appendTo($p4);

                ////var $p5 = $('div[data-field=da15]').find('label');
                ////var remind = '<span class="text-danger fw-lighter pull-right">限填數字</span>';
                ////$(remind).appendTo($p5);

                ////var $p6 = $('div[data-field=da24]').find('label');
                ////var remind = '<span class="text-danger fw-lighter pull-right">限500字(Ex: 空氣污染防制技術、廢棄物資源化、薄膜水處理技術…)</span>';
                ////$(remind).appendTo($p6);

                ////var $p7 = $('div[data-field=ProfilePhoto]').find('label');
                ////var remind = '<span class="text-danger fw-lighter ms-1">可修改無法刪除</span>';
                ////$(remind).appendTo($p7);
            }

            _opt.afterUpdateServerData = _opt.afterAddServerData = function (row, callback) {
                jspAlertMsg($("body"), { autoclose: 2000, content: '個人資料更新成功!!', classes: 'modal-sm' },
                    function () {
                        $('html,body').animate({ scrollTop: $_d1Table.offset().top }, "show");
                    });

                //(no callback)更新dou的rowdata
                $_d1Table.instance.updateDatas(row);

                if (id_toTab != null) {
                    //(啟動)tab切換
                    $('a[href="' + id_toTab + '"]').tab('show');
                    id_toTab = undefined;
                }

                ////callback();
            }

            //實體Dou js                                
            $_d1Table = $_d1EditDataContainer.douTable(_opt);
        });        
    }

    function SetDouDa4(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'Expertise/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '專長';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            //////初始options預設值
            ////douHelper.setFieldsDefaultAttribute(_opt.fields);//給預設屬性

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.PId = PId;

                callback();
            };

            //_opt.tableOptions.sortName = 'da404';
            //_opt.tableOptions.sortOrder = 'desc';

            ////_opt.afterCreateEditDataForm = function ($container, row) {
            ////    //加提示字
            ////    var $p1 = $('div[data-field=da404]').find('label');
            ////    var remind = '<span class="text-danger fw-lighter pull-right">年月(201609)</span>';
            ////    $(remind).appendTo($p1);

            ////    var $p2 = $('div[data-field=da405]').find('label');
            ////    var remind = '<span class="text-danger fw-lighter pull-right">年月(20206)</span>';
            ////    $(remind).appendTo($p2);
            ////};

            //實體Dou js
            $_d4Table = $_d4EditDataContainer.douTable(_opt);
        });
    };

    function SetDouDa5(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'UserHistoryOpinion/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '意見';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.PId = PId;

                callback();
            };

            //實體Dou js
            $_d5Table = $_d5EditDataContainer.douTable(_opt);
        });
    };

    function SetDouDa6(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'Resume/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '經歷';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.PId = PId;

                callback();
            };

            //實體Dou js
            $_d6Table = $_d6EditDataContainer.douTable(_opt);
        });
    };

    function SetDouDa7(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'FTISUserHistory/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '專家參與紀錄';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {                
                row.PId = PId;

                callback();
            };

            _opt.afterCreateEditDataForm = function ($container, row) {
                //加提示字
                var $p1 = $('div[data-field=Date]').find('label');
                var remind = '<span class="text-danger fw-lighter pull-right">非必填</span>';
                $(remind).appendTo($p1);
            };

            //實體Dou js
            $_d7Table = $_d7EditDataContainer.douTable(_opt);
        });
    };

    function SetDouDa8(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'BasicUser_License/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '證照';

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            datas = datas ? datas : [{}];
            _opt.datas = datas;

            _opt.editformSize = { minWidth: 700 };
            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.PId = PId;

                callback();
            };

            //實體Dou js
            $_d8Table = $_d8EditDataContainer.douTable(_opt);
        });
    };

    function ExistName(PId, Name) {

        var result = '';

        helper.misc.showBusyIndicator();
        $.ajax({
            url: app.siteRoot + 'BasicUser/ExistName',
            datatype: "json",
            type: "Get",
            data: { PId: PId, Name: Name },
            async: false,
            success: function (data) {
                if (data.exist) {
                    var msg = '<span class="text-danger">****姓名已存在，是否儲存****</span>' + '</br>';

                    //修正 data.basicuser[0].PId

                    msg += '<ul>';
                    $.each(data.basicuser, function (index, value) {

                        var strPosition = this.Position == null ? '' : this.Position;

                        var content = '<a href="#" style="text-decoration: none" onclick = "GoEditSpecificData(\'' + this.PId + '\')">專家(' + this.PId + ' ' + this.Name + ' ' + strPosition + ')' + '</a>'                                      
                                      + '<span class="ps-3">' + '建檔人(' + this.BName + ')' + '</span>';

                        msg += '<li class="mt-2">' + content + '</li>';
                    });

                    msg += '</ul>';

                    result = msg;                    
                }
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

        return result;
    }

    
})

//切換至PId
function GoEditSpecificData(PId) {

    //alert(PId);

    $('.modal-dialog .modal-content .modal-footer').find('.取.消').trigger('click');

    //Dou 執行作業

    //重新組Dou清單頁
    helper.misc.showBusyIndicator();
    $.ajax({
        url: app.siteRoot + 'BasicUser/GetBasicUser',
        datatype: "json",
        type: "Get",
        data: { PId: PId },
        async: false,
        success: function (datas) {

            //取消(pop window)
            $('.basicusercontroller.data-edit-jspanel').find('.modal-footer .btn.btn-default').first().trigger('click');

            $('[Role="tablist"]').remove();
            $("#_table").douTable('tableReload', datas);
            
            //trigger進入編輯頁
            $('.bootstrap-table.basicusercontroller').find('td .btn-update-data-manager').trigger('click');
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