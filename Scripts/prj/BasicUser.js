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

    //主表(EmpData) 基本資料
    douoptions.afterCreateEditDataForm = function ($container, row) {

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
    
    var a = {};
    a.item = '<span class="btn btn-secondary glyphicon glyphicon-open-file"> 匯入專家資料</span>';
    a.event = 'click .glyphicon-open-file';
    a.callback = function importBasicUser(evt) {
        
        $("#upFile").trigger("click");

        ////var $element = $('body');
        ////helper.misc.showBusyIndicator($element, { timeout: 300 * 60 * 1000 });
        ////$.ajax({
        ////    url: app.siteRoot + 'City/ImportCityRoad',
        ////    datatype: "json",
        ////    type: "Get",
        ////    timeout: 300 * 60 * 1000, //300分
        ////    success: function (data) {
        ////        if (data.result) {
        ////            alert("匯入成功");
        ////        } else {
        ////            alert("匯入失敗：\n" + data.errorMessage);
        ////        }
        ////    },
        ////    complete: function () {
        ////        helper.misc.hideBusyIndicator($element, { timeout: 180000 });
        ////    },
        ////    error: function (xhr, status, error) {
        ////        var err = eval("(" + xhr.responseText + ")");
        ////        alert(err.Message);
        ////        helper.misc.hideBusyIndicator($element, { timeout: 180000 });
        ////    }
        ////});
        
    };

    douoptions.appendCustomToolbars = [a];

    var $_masterTable = $("#_table").DouEditableTable(douoptions).on($.dou.events.add, function (e, row) {

        //錨點
        var aPoint = $_masterTable.instance.settings.rootParentContainer;
        $('html,body').animate({ scrollTop: $(aPoint).offset().top }, "show");

        //trigger清單(新增row)編輯按鈕的，
        $_masterTable.DouEditableTable("editSpecificData", row);

    }); //初始dou table

    var accept = ['.xlsx'];
    var $iptFile = $('<input id="upFile" type="file" multiple accept=' + accept.join(',') + ' name="upFileReport"  />');
    $('.glyphicon.glyphicon-open-file').after($iptFile);
    $iptFile.hide();

    var $sample = $('<a class="btn btn-secondary" href = "' + app.siteRoot + 'DocsWeb/Sample/(Sample)專家資料匯入範本.xlsx">匯入範本</a>');
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


    //專長
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
                $('.ftisuserhistorycontroller .modal-dialog').find('[data-fn="ActivityCategoryId"] option[value=""]').remove();
                
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
                    
                    ActivityCategoryId.selectpicker('deselectAll');                    
                }

                //參與紀錄 會議類別change事件
                ChangeActivityCategoryType(isAdd);
                

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

    //參與紀錄 會議類別change事件
    function ChangeActivityCategoryType(isAdd) {
        $('.ftisuserhistorycontroller .modal-dialog').find("[data-fn=ActivityCategoryType]").change(function () {
            if (isAdd) {
                //會議名稱：新增－多選
                ResetSelectpickerActivityCategoryType();
            }

            //1.會內2.會外資料填寫
            var type = $('.modal-dialog [data-fn="ActivityCategoryType"]').val();
            if (type == 1) {
                $('.modal-dialog  [data-field="ActivityCategoryJoinNum"]').hide();
                $('.modal-dialog  [data-field="Year"]').show();
                $('.modal-dialog  [data-field="ProjectId"]').show();
                $('.modal-dialog  [data-field="OutYear"]').hide();

                //預設值
                $('.modal-dialog  [data-field="ActivityCategoryId"] label').text('會議名稱')
                $('[data-fn="ActivityCategoryJoinNum"]').val('');
                $('[data-fn="OutYear"]').val('');
            }
            else if (type == 2) {
                $('.modal-dialog  [data-field="ActivityCategoryJoinNum"]').show();
                $('.modal-dialog  [data-field="Year"]').hide();
                $('.modal-dialog  [data-field="ProjectId"]').hide();
                $('.modal-dialog  [data-field="OutYear"]').show();

                //預設值
                $('.modal-dialog  [data-field="ActivityCategoryId"] label').text('會議委辦單位')
                $('[data-fn="Year"]').val('');
                $('[data-fn="ProjectId"]').val('');
            }
        });
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