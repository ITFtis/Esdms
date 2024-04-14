$(document).ready(function () {

    var id_toTab;
    //特定角色使用功能
    var adminRoles = ['admin', 'ftisadmin'];
    var adminUsed = loginRoles.some(r => adminRoles.includes(r));
    
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

    //主導覽連結
    var mlist;

    //主表(EmpData) 基本資料
    douoptions.afterCreateEditDataForm = function ($container, row) {

        mlist = [];
        mlist.push('專家');
        mlistShow();

        var isAdd = JSON.stringify(row) == '{}';

        var $_oform = $("#_tabs");
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

        var $p1_2 = $('div[data-field=OfficePhone2]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(02-23****** #123)</span>';
        $(remind).appendTo($p1_2);

        var $p2 = $('div[data-field=Fax]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(02-23****** #123)</span>';
        $(remind).appendTo($p2);

        var $p3 = $('div[data-field=PrivatePhone]').find('label');
        var remind = '<span class="text-danger fw-lighter pull-right">格式(0912******)</span>';
        $(remind).appendTo($p3);

        //姓名重複判別按鈕        
        var $p4 = $('div[data-field=Name]').find('label');
        var remind = '<span id="aViewName" style="cursor: pointer" href="#" class="badge text-bg-primary fw-lighter pull-right">姓名重複判別</span>';
        $(remind).appendTo($p4);

        $("#aViewName").click(function () {            
            var PId = $('[data-fn="PId"]').val();
            var Name = $('[data-fn="Name"]').val();
            var msg = ExistName(PId, Name);

            if (msg != '') {
                jspAlertMsg($("body"), { autoclose: 5000, content: msg }, null);
            }
            else {
                jspAlertMsg($("body"), { autoclose: 3000, content: "姓名尚未重覆", classes: 'modal-sm' }, null);
            }
        });            

        if (IsView) {
            $(".modal-dialog input").prop("disabled", true);
            $(".modal-dialog select").prop("disabled", true);
            $(".modal-dialog textarea").prop("disabled", true);
            $(".modal-dialog .modal-footer").hide();
            $("#aViewName").hide();
        }

        if (isAdd) {
            //新增隱藏身分代碼, 由Controler(Add)產出
            //$('.modal-dialog [data-field="PId"]').hide();
            $('.modal-dialog [data-fn="PId"]').prop("disabled", true);
            $('.modal-dialog [data-fn="PId"]').val('--系統產出--');
        }
        else {
            //主表新增集合沒資料(預設集合)
            var iniObj = { PId: row.PId };

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
            ['基本資料', '專長', '專家參與紀錄'],
            [$_oform, $_d4EditDataContainer, $_d7EditDataContainer]);

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

                        //不做callback，避免tab切換，互相影響
                        jspAlertMsg($("body"), { autoclose: 2000, content: '基本資料更新成功!!', classes: 'modal-sm' },
                            function () {
                                //(啟動)tab切換
                                $('a[href="' + id_toTab + '"]').tab('show');
                                id_toTab = undefined;

                                $('html,body').animate({ scrollTop: $_masterTable.offset().top }, "show");
                            });                        
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

    //清單匯出
    var b = {};
    b.item = '<span class="btn btn-secondary glyphicon glyphicon-download-alt"> 匯出清單</span>';
    b.event = 'click .glyphicon-download-alt';
    b.callback = function exportQdate(evt) {

        var content = '<div class="row"> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkSex" /> \
                                    <label class="form-check-label" for="ChkSex">性別</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkOnJob" /> \
                                    <label class="form-check-label" for="ChkOnJob">在職狀況</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPrivatePhone" /> \
                                    <label class="form-check-label" for="ChkPrivatePhone">手機號碼</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkOfficePhone" /> \
                                    <label class="form-check-label" for="ChkOfficePhone">辦公室電話</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkOfficePhone2" /> \
                                    <label class="form-check-label" for="ChkOfficePhone2">辦公室電話2</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkFax" /> \
                                    <label class="form-check-label" for="ChkFax">傳真</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkOfficeEmail" /> \
                                    <label class="form-check-label" for="ChkOfficeEmail">辦公_Email</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPrivateEmail" /> \
                                    <label class="form-check-label" for="ChkPrivateEmail">私人_Email</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkCityCode" /> \
                                    <label class="form-check-label" for="ChkCityCode">辦公_縣市</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkZIP" /> \
                                    <label class="form-check-label" for="ChkZIP">辦公_鄉鎮市區</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkOfficeAddress" /> \
                                    <label class="form-check-label" for="ChkOfficeAddress">辦公_地址</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPCityCode" /> \
                                    <label class="form-check-label" for="ChkPCityCode">住家_縣市</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPZIP" /> \
                                    <label class="form-check-label" for="ChkPZIP">住家_鄉鎮市區</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPAddress" /> \
                                    <label class="form-check-label" for="ChkPAddress">住家_地址</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkNote" /> \
                                    <label class="form-check-label" for="ChkNote">備註</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkCategoryId" checked/> \
                                    <label class="form-check-label" for="ChkCategoryId">人員類別</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkUnitName" checked/> \
                                    <label class="form-check-label" for="ChkUnitName">單位系所</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkPosition" checked/> \
                                    <label class="form-check-label" for="ChkPosition">職稱</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkstrExpertises" checked/> \
                                    <label class="form-check-label" for="ChkstrExpertises">專長</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkvmOutCount" checked/> \
                                    <label class="form-check-label" for="ChkvmOutCount">會外評選</label> \
                                </div> \
                            </div> \
                            <div class="detail-view-field pb-2 col-6"> \
                                <div class="detail-view-field form-check checkbox-xl"> \
                                    <input class="form-check-input" type="checkbox" value="" id="ChkvmInCount" checked/> \
                                    <label class="form-check-label" for="ChkvmInCount">會內參與</label> \
                                </div> \
                            </div> \
                        </div>';

        jspConfirmYesNo($("body"), { content: content }, function (confrim) {
            if (confrim) {
                //匯出Excel
                var conditions = GetFilterParams($_masterTable)
                var paras;
                if (conditions.length > 0) {
                    paras = { key: 'filter', value: JSON.stringify(conditions) };
                }

                var sortName = $_masterTable.bootstrapTable('getOptions').sortName;
                var sortOrder = $_masterTable.bootstrapTable('getOptions').sortOrder;

                //勾選欄位
                var chks = {};
                chks.ChkSex = $('#ChkSex').prop("checked");
                chks.ChkOnJob = $('#ChkOnJob').prop("checked");
                chks.ChkPrivatePhone = $('#ChkPrivatePhone').prop("checked");
                chks.ChkOfficePhone = $('#ChkOfficePhone').prop("checked");
                chks.ChkOfficePhone2 = $('#ChkOfficePhone2').prop("checked");
                chks.ChkFax = $('#ChkFax').prop("checked");
                chks.ChkOfficeEmail = $('#ChkOfficeEmail').prop("checked");
                chks.ChkPrivateEmail = $('#ChkPrivateEmail').prop("checked");
                chks.ChkCityCode = $('#ChkCityCode').prop("checked");
                chks.ChkZIP = $('#ChkZIP').prop("checked");
                chks.ChkOfficeAddress = $('#ChkOfficeAddress').prop("checked");
                chks.ChkPCityCode = $('#ChkPCityCode').prop("checked");
                chks.ChkPZIP = $('#ChkPZIP').prop("checked");
                chks.ChkPAddress = $('#ChkPAddress').prop("checked");
                chks.ChkNote = $('#ChkNote').prop("checked");
                chks.ChkCategoryId = $('#ChkCategoryId').prop("checked");
                chks.ChkUnitName = $('#ChkUnitName').prop("checked");
                chks.ChkPosition = $('#ChkPosition').prop("checked");
                chks.ChkstrExpertises = $('#ChkstrExpertises').prop("checked");
                chks.ChkvmOutCount = $('#ChkvmOutCount').prop("checked");
                chks.ChkvmInCount = $('#ChkvmInCount').prop("checked");

                helper.misc.showBusyIndicator();
                $.ajax({
                    url: app.siteRoot + 'BasicUser/ExportList',
                    datatype: "json",
                    type: "POST",
                    data: { paras: [paras], sort: sortName, order: sortOrder, chks: chks },
                    success: function (data) {
                        if (data.result) {
                            location.href = app.siteRoot + data.url;
                        } else {
                            alert("查詢失敗：\n" + data.errorMessage);
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
            }
        })        
    };

    var ad1 = {};
    ad1.item = '<span class="btn btn-secondary glyphicon glyphicon-open-file"> 匯入專家資料</span>';
    ad1.event = 'click .glyphicon-open-file';
    ad1.callback = function importBasicUser(evt) {        
        $("#upFile").trigger("click");                
    };
    
    //特定角色使用功能
    if (adminUsed) {
        douoptions.appendCustomToolbars = [b, ad1];
    }
    else {
        douoptions.appendCustomToolbars = [b];
    }

    douoptions.queryFilter = function (params, callback) {
        var SubjectDetailId = params.find(a => a.key == "SubjectDetailId");
        SubjectDetailId.value = SubjectDetailId.value.join(',');

        callback();
    }

    douoptions.tableOptions.onLoadSuccess = function (datas) {
        //alert('123');        
        if (datas && datas.rows.length == 0) {
            //條件顯示：無符合資料
            var aryFilter = [];

            var conditions = GetFilterParams($_masterTable);
            $.each(conditions, function (index, v) {
                if (v.value) {                    
                    switch (v.key) {
                        case "Name":
                            aryFilter.push("姓名(" + v.value + ")");
                            break;
                        case "strExpertises":
                            aryFilter.push("專長(" + v.value + ")");
                            break;
                        case "SubjectId":
                            var s = $('.fixed-table-toolbar').find('[data-fn="SubjectId"] option[value=' + v.value + ']').text();
                            aryFilter.push("專長類別(" + s + ")");
                            break;
                        case "SubjectDetailId":
                            var ary = [];
                            $.each(v.value.split(','), function (a, text) {
                                ary.push($('.fixed-table-toolbar').find('[data-fn="SubjectDetailId"] option[value=' + text + ']').text());
                            })
                            aryFilter.push("專長領域(" + ary.join('、') + ")");
                            break;
                        case "DuplicateName":
                            var s = '';
                            if (v.value == "Y") {
                                s = "是";
                            }
                            else if (v.value == "N") {
                                s = "否";
                            }

                            aryFilter.push("重覆姓名(" + s + ")");
                            break;
                    }
                }
            });

            if (aryFilter.length > 0) {
                var str = aryFilter.join(' ,') + '</br>' + '  無符合資料';
                $('.no-records-found td').html("篩選條件：" + str);
            }
        }
    }

    var $_masterTable = $("#_table").DouEditableTable(douoptions).on($.dou.events.add, function (e, row) {

        //錨點
        var aPoint = $_masterTable.instance.settings.rootParentContainer;
        $('html,body').animate({ scrollTop: $(aPoint).offset().top }, "show");

        //trigger清單(新增row)編輯按鈕的，
        $_masterTable.DouEditableTable("editSpecificData", row);

    }); //初始dou table

    $('[data-fn="SubjectDetailId"] option[value=""]').remove();
    //多選
    var $SubjectDetailId = $('.filter-toolbar-plus').find("[data-fn=SubjectDetailId]")
        .attr('multiple', true).selectpicker({
            noneSelectedText: '請挑選專長領域',
            actionsBox: true,
            selectAllText: '全選',
            deselectAllText: '取消已選',
            selectedTextFormat: 'count > 1',
            countSelectedText: function (sc, all) {
                return '專長領域:挑' + sc + '個'
            }
        });

    $('[data-fn="SubjectId"]').change(function () {
        $SubjectDetailId.selectpicker('deselectAll');
        RestSelectpickerSubjectId();
    })

    //特定角色使用功能
    if ($('.glyphicon.glyphicon-open-file').length > 0) {
        var accept = ['.xlsx'];
        var $iptFile = $('<input id="upFile" type="file" multiple accept=' + accept.join(',') + ' name="upFileReport"  />');
        $('.glyphicon.glyphicon-open-file').after($iptFile);
        $iptFile.hide();

        var $sample = $('<a class="btn btn-secondary" href = "' + app.siteRoot + 'DocsWeb/Sample/(Sample)範本專家資料匯入.xlsx">下載範本</a>');
        $('.glyphicon.glyphicon-open-file').after($sample);

        $iptFile.on("change", function () {
            //限定檔案大小
            var maxSize = 10 * 1024 * 1024;  //10MB
            if (this.files[0].size > maxSize) {
                alert("檔案大小限制:10MB");
                return;
            };

            var fileData = new FormData();

            $.each($("#upFile").get(0).files, function (index, obj) {
                fileData.append(this.name, obj);
            });

            var $element = $('body');
            helper.misc.showBusyIndicator($element, { timeout: 300 * 60 * 1000 });
            $.ajax({
                url: app.siteRoot + 'BasicUser/UpFile',
                datatype: "json",
                type: "POST",
                data: fileData,
                contentType: false,
                processData: false,
                success: function (data) {
                    //清空檔案
                    $("#upFile").val('');

                    if (data.result) {
                        $('.bootstrap-table').find('.btn-confirm').trigger('click');
                        alert("匯入成功");
                    } else {
                        alert("匯入失敗：\n" + data.errorMessage);
                    }

                    helper.misc.hideBusyIndicator();
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
        });
    }

    //特定帳號顯示功能(vPower.json)
    //匯出功能
    $('.glyphicon-download-alt').css("display", "none");
    if (aryVPower != null) {
        //json原編(有)
        if (aryVPower.indexOf('2') > -1) {
            $('.glyphicon-download-alt').css("display", "block");
        }
    }
    else {
        //json原編(無)
        if (adminUsed) {
            $('.glyphicon-download-alt').css("display", "block");
        }
    }

    //專長
    function SetDouDa4(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'Expertise/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '專長';

            if (IsView) {
                _opt.addable = false;
                _opt.editable = false;
                _opt.deleteable = false;
            }

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

            _opt.addServerData =
                function (row, callback) {
                    var _filed = douHelper.getField($_d4Table.instance.settings.fields, "SubjectDetailId");
                    var objs = _filed.editFormtter.getValue.call(_filed, $('.expertisecontroller [data-fn="SubjectDetailId"]'));

                    var rows = [];

                    
                    $.each(objs, function (index, value) {
                        var nrow = jQuery.extend({}, row);
                        nrow.SubjectDetailId = this;
                        rows.push(nrow);
                    });

                    if (rows.length == 0) {
                        alert('尚未挑選專長領域');
                        return;
                    }

                    transactionDouClientDataToServer(rows, $.AppConfigOptions.baseurl + 'Expertise/Add', function () {

                        //取消(pop window)
                        $('.expertisecontroller.data-edit-jspanel').find('.modal-footer .btn.btn-default').first().trigger('click');

                        jspAlertMsg($("body"), { autoclose: 2000, content: "專長資料新增成功" },
                            function () {
                                //重新組Dou清單頁
                                helper.misc.showBusyIndicator();
                                $.ajax({
                                    url: app.siteRoot + 'BasicUser/GetBasicUser',
                                    datatype: "json",
                                    type: "Get",
                                    data: { PId: PId },
                                    async: false,
                                    success: function (datas) {                                        
                                        $_d4EditDataContainer.douTable('destroy');
                                        SetDouDa4(datas[0].Expertises, PId);
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
                            });
                    });
                };

            _opt.afterCreateEditDataForm = function ($container, row) {

                var isAdd = row.Id == null;

                $('.expertisecontroller .modal-dialog').find('[data-fn="SubjectId"] option[value=""]').remove();
                $('.expertisecontroller .modal-dialog').find('[data-fn="SubjectDetailId"] option[value=""]').remove();

                //專長領域：新增－多選。修改－單選
                if (isAdd) {                    
                    ///多選
                    var SubjectDetailId = $('.expertisecontroller .modal-dialog').find("[data-fn=SubjectDetailId]")
                        .attr('multiple', true).selectpicker({
                            noneSelectedText: '請挑選專長',
                            actionsBox: true,
                            selectAllText: '全選',
                            deselectAllText: '取消已選',
                            selectedTextFormat: 'count > 1',
                            countSelectedText: function (sc, all) {
                                return '專長:挑' + sc + '個'
                            }
                        });

                    $('.expertisecontroller .modal-dialog').find("[data-fn=SubjectId]").change(function () {
                        ResetSelectpickerSubjectId();
                    });
                }
            }

            //實體Dou js
            $_d4Table = $_d4EditDataContainer.douTable(_opt);            
        });
    };

    //意見
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

    //經歷
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

    //專家參與紀錄
    function SetDouDa7(datas, PId) {
        $.getJSON($.AppConfigOptions.baseurl + 'FTISUserHistory/GetDataManagerOptionsJson', function (_opt) { //取model option

            _opt.title = '專家參與紀錄';

            if (IsView) {
                _opt.addable = false;
                _opt.editable = false;
                _opt.deleteable = false;
            }

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

                var isAdd = row.Id == null;

                $('.ftisuserhistorycontroller .modal-dialog').find('[data-fn="ActivityCategoryType"] option[value=""]').remove();                
                
                if (isAdd) {
                    //會議名稱：新增－多選。修改－單選
                    var ActivityCategoryId = $('.ftisuserhistorycontroller .modal-dialog').find("[data-fn=ActivityCategoryId]")
                        .attr('multiple', true).selectpicker({
                            noneSelectedText: '請挑選會議',
                            actionsBox: true,
                            selectAllText: '全選',
                            deselectAllText: '取消已選',
                            selectedTextFormat: 'count > 1',
                            countSelectedText: function (sc, all) {
                                return '會議:挑' + sc + '個'
                            }
                        });

                    $('.ftisuserhistorycontroller .modal-dialog').find('[data-fn="ActivityCategoryId"] option[value=""]').remove();
                    ActivityCategoryId.selectpicker('deselectAll').selectpicker('val', '');                    
                }

                //會議類別綁定事件(Change)
                ChangeActivityCategoryType();                                

                //(下拉)參與紀錄 年度連動專案
                $('.modal-dialog [data-fn="Year"]').change(function () {
                    YearGearing();
                });

                //(下拉)參與紀錄 年度連動專案 ini
                YearGearing();

                //加提示字
                var $p1 = $('div[data-field=Date]').find('label');
                var remind = '<span class="text-danger fw-lighter pull-right">非必填</span>';
                $(remind).appendTo($p1);

                //特定角色使用功能
                if (!adminUsed) {
                    $('.modal-dialog [data-field="ActivityCategoryType"] option[value="2"]').hide();
                }
            };

            _opt.addServerData =
                function (row, callback) {
                    var _filed = douHelper.getField($_d7Table.instance.settings.fields, "ActivityCategoryId");
                    var objs = _filed.editFormtter.getValue.call(_filed, $('.ftisuserhistorycontroller [data-fn="ActivityCategoryId"]'));

                    var rows = [];


                    $.each(objs, function (index, value) {
                        var nrow = jQuery.extend({}, row);
                        nrow.ActivityCategoryId = this;
                        rows.push(nrow);
                    });

                    //沒選會議
                    if (rows.length == 0) {
                        rows.push(row);
                    }                    

                    transactionDouClientDataToServer(rows, $.AppConfigOptions.baseurl + 'FTISUserHistory/Add', function () {

                        //取消(pop window)
                        $('.ftisuserhistorycontroller.data-edit-jspanel').find('.modal-footer .btn.btn-default').first().trigger('click');

                        jspAlertMsg($("body"), { autoclose: 2000, content: "會議資料新增成功" },
                            function () {
                                //重新組Dou清單頁
                                helper.misc.showBusyIndicator();
                                $.ajax({
                                    url: app.siteRoot + 'BasicUser/GetBasicUser',
                                    datatype: "json",
                                    type: "Get",
                                    data: { PId: PId },
                                    async: false,
                                    success: function (datas) {
                                        $_d7EditDataContainer.douTable('destroy');
                                        SetDouDa7(datas[0].FTISUserHistorys, PId);
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
                            });
                    });
                };

            var a = {};
            a.item = function (v, r) {
                if (r.ActivityCategoryType == 2) {
                    var btn = "";
                    if (!r.IsImport) {
                        btn = '<span class="pe-1"></span><span id="OutSet" class="btn btn-default btn-sm ">會外組別</span>';
                    }
                    return btn;
                }
                else {
                    return '';
                }
            }
            a.event = 'click #OutSet';
            a.callback = function GoUserHistorySet(evt, value, row, index) {

                //資料維護：專家參與紀錄(會外組別標案) 多對多
                var tr = $('.bootstrap-table.ftisuserhistorycontroller table.table tbody tr')[index];
                var mtitle = $(tr).find('.dou-field-OutYear').text() + '年度'
                            + $(tr).find('.dou-field-ActivityCategoryId').text();

                var FtisUHId = row.Id;
                mlist.push('會外組別' + '(' + mtitle + ')');
                mlistShow();                
                //主導覽連結
                $('.bootstrap-table.ftisuserhistorycontroller').closest('.tab-content').parent().hide();
                //BasicUserHistorySet.js
                BasicUserHistorySet($('#_downtable'), FtisUHId);                
            };

            _opt.appendCustomFuncs = [a];

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

    //專家清單查詢 專長領域
    function RestSelectpickerSubjectId() {
        var SubjectId = $('.filter-toolbar-plus').find("[data-fn=SubjectId]").val();

        var $ele = $('.filter-toolbar-plus').find("[data-fn=SubjectDetailId]");
        $ele.find('[data-subjectid!="' + SubjectId + '"]').hide();
        $ele.selectpicker('refresh');//.selectpicker('val', '');
    }

    //專長 Reset
    function ResetSelectpickerSubjectId() {
        var SubjectId = $('.modal-dialog').find("[data-fn=SubjectId]").val();

        var $ele = $('.modal-dialog').find("[data-fn=SubjectDetailId]");
        $ele.find('[data-subjectid!="' + SubjectId + '"]').hide();
        $ele.selectpicker('refresh').selectpicker('val', '');
    }

    //參與紀錄 會議
    function ResetSelectpickerActivityCategoryType() {
        var ActivityCategoryType = $('.modal-dialog').find("[data-fn=ActivityCategoryType]").val();

        var $ele = $('.modal-dialog').find("[data-fn=ActivityCategoryId]");
        $ele.find('[data-activitycategorytype!="' + ActivityCategoryType + '"]').hide();
        $ele.selectpicker('refresh').selectpicker('val', '');
    }

    //參與紀錄 會議類別綁定事件(Change)
    function ChangeActivityCategoryType() {
        $('.ftisuserhistorycontroller .modal-dialog').find("[data-fn=ActivityCategoryType]").change(function () {
            var multiple = $('.ftisuserhistorycontroller .modal-dialog').find("[data-fn=ActivityCategoryId]").attr('multiple');

            //change會議類別 SetUI
            SetUIChangeActivityCategory(multiple);

            if ($('.ftisuserhistorycontroller .modal-dialog').find("[data-fn=ActivityCategoryType]").is(":focus")) {
                //change會議類別 SetValue
                SetValueChangeActivityCategory(multiple);
            } 
        })
    }

    //參與紀錄 => change會議類別 SetUI
    function SetUIChangeActivityCategory(multiple) {
        //1.會內2.會外資料填寫
        var type = $('.modal-dialog [data-fn="ActivityCategoryType"]').val();
        if (type == 1) {
            $('.modal-dialog  [data-field="ActivityCategoryJoinNum"]').hide();
            $('.modal-dialog  [data-field="Year"]').show();
            $('.modal-dialog  [data-field="ProjectId"]').show();
            $('.modal-dialog  [data-field="OutYear"]').hide();
            $('.modal-dialog  [data-field="DCode"]').show();
            $('.modal-dialog  [data-field="Owner"]').show();

            //預設值
            $('.modal-dialog  [data-field="ActivityCategoryId"] label').text('會議名稱');
        }
        else if (type == 2) {
            $('.modal-dialog  [data-field="ActivityCategoryJoinNum"]').show();
            $('.modal-dialog  [data-field="Year"]').hide();
            $('.modal-dialog  [data-field="ProjectId"]').hide();
            $('.modal-dialog  [data-field="OutYear"]').show();
            $('.modal-dialog  [data-field="DCode"]').hide();
            $('.modal-dialog  [data-field="Owner"]').hide();

            //預設值
            $('.modal-dialog  [data-field="ActivityCategoryId"] label').text('會議委辦單位');
        }

        if (multiple != null) {
            ResetSelectpickerActivityCategoryType();
        }
    }

    //參與紀錄 => change會議類別 SetValue
    function SetValueChangeActivityCategory(multiple) {        
        //1.會內2.會外資料填寫
        var type = $('.modal-dialog [data-fn="ActivityCategoryType"]').val();
        if (type == 1) {
            //預設值                        
            if (multiple == null) {
                //單選
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').val('');
            }
            else {
                //多選
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').selectpicker({ noneSelectedText: '請挑選會議名稱' }).selectpicker('refresh')
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').selectpicker('deselectAll').selectpicker('val', '');
            }
            
            $('[data-fn="ActivityCategoryJoinNum"]').val('');
            $('[data-fn="OutYear"]').val('');
        }
        else if (type == 2) {
            //預設值                        
            if (multiple == null) {
                //單選
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').val('');
            }
            else {
                //多選
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').selectpicker({ noneSelectedText: '請挑選委辦單位' }).selectpicker('refresh')
                $('.modal-dialog').find('[data-fn="ActivityCategoryId"]').selectpicker('deselectAll').selectpicker('val', '');
            }            
            $('[data-fn="Year"]').val('');
            $('[data-fn="ProjectId"]').val('');
            $('[data-fn="DCode"]').val('');
            $('[data-fn="Owner"]').val('');
        }        
    }

    //(下拉)參與紀錄 年度連動專案
    function YearGearing() {        
        var $select = $('.modal-dialog [data-fn="ProjectId"]').parent().find('datalist');
        var year = $('.modal-dialog [data-fn="Year"]').val();
        
        if (year) {
            //全關
            $select.find('option').prop('disabled', true);

            //112年度顯示
            $select.find('[data-year="' + year + '"]').prop('disabled', false);
        }
        else {
            //全開
            $select.find('option').prop('disabled', false);
        }
    }

    function mlistShow() {

        var content = '';

        if (mlist.length == 1) {
            content = '<span class="fs-4">專家</span>';
        }
        else {
            $.each(mlist, function (index, value) {
                //1.專家 2.會外組別            
                if (content == '') {
                    var text = '<span name="smap" class="text-decoration-underline text-primary fs-4" style="cursor:pointer" type="' + value + '">' + value + '</span>';
                    content = text;
                }
                else {
                    content += '<span class="fs-4 ps-2 pe-2">→</span>' + '<span class="fs-4">' + value + '</span>';
                }
            });
        }

        $('#mlist').html(content);

        //event 點選
        $('#mlist [name="smap"]').on('click', function () {
            //關閉 downtable
            $('#_downtable').douTable('destroy');
            //開啟 主導覽連結
            $('.bootstrap-table.ftisuserhistorycontroller').closest('.tab-content').parent().show();

            mlist = [];
            mlist.push('專家');
            content = '<span class="fs-4">專家</span>';
            $('#mlist').html(content);
        });
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

            //(alert訊息需自行觸發)trigger dialog button-close
            $('.modal-dialog .btn-close').trigger('click');
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