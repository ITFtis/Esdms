$(document).ready(function () {

    douoptions.viewable = true;

    $(douoptions.fields).each(function () {
        if (this.field == "PId") {
            this.formatter = function (v, d) {
                //return v;
                return '<div id="divGo" style="cursor: pointer; color: #1e0cfc;">' + v + '</div>';
            }
            this.events = {
                'click #divGo': function (e, value, row, index) {
                    $("#_table").bootstrapTable('showColumn', 'ctrl');
                    $(".btn-view-data-manager").eq(index).trigger("click");
                    $("#_table").bootstrapTable('hideColumn', 'ctrl');

                    //append PId button
                    setTimeout(function () {
                        var footer = '<div class="modal-footer justify-content-start"> \
                                          <div class="detail-view-field"> \
                                             <b>新身分證</b> \
                                          </div> \
                                          <div class="field-content col-sm-3"> \
                                             <input id="newPId" type="text" class="form-control" maxlength="20"> \
                                          </div> \
                                          <button id="btnUpdatePid" type="button" class="btn btn-primary " style=""> 確 定 </button> \
                                      </div>';
                        $('.modal-dialog .modal-body').append(footer);

                        $('#btnUpdatePid').click(function () {
                            jspConfirmYesNo($("body"), { content: "確認更新身分證資料" }, function (confrim) {
                                if (confrim) {

                                    var PId = value;
                                    var newPId = $('#newPId').val();

                                    helper.misc.showBusyIndicator();
                                    $.ajax({
                                        url: app.siteRoot + 'Fun_BasicUserPId/UpdatePId',
                                        datatype: "json",
                                        type: "Get",
                                        data: { PId: PId, newPId: newPId },
                                        success: function (data) {
                                            if (data.result) {
                                                jspAlertMsg($("body"), { autoclose: 2000, content: '身分證更新成功!!', classes: 'modal-sm' },
                                                    function () {
                                                        location.reload();
                                                    });
                                            }
                                            else {
                                                alert("身分證更新失敗：\n" + data.errorMessage);
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
                        });
                    }, 300);
                    //End append PId button
                }
            }
        }
    });

    var $_masterTable = $("#_table").DouEditableTable(douoptions); //初始dou table
    $("#_table").bootstrapTable('hideColumn', 'ctrl');
})