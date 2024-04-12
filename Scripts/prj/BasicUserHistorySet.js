var BasicUserHistorySet = function ($container, FtisUHId) {    
    helper.misc.showBusyIndicator();
    $.ajax({
        url: $.AppConfigOptions.baseurl + 'UserHistorySet/getDataDetail',
        datatype: "json",
        type: "POST",
        data: { "FtisUHId": FtisUHId },
        success: function (_setopt) {

            _setopt.title = '處室組別';

            //取消自動抓後端資料
            _setopt.tableOptions.url = undefined;

            _setopt.beforeCreateEditDataForm = function (row, callback) {
                row.FtisUHId = FtisUHId;

                callback();
            };

            _setopt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Add', callback);
            };

            _setopt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Update', callback);
            };

            _setopt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Delete', callback);
            };

            _setopt.afterCreateEditDataForm = function ($container, row) {
                
                if (row.Id == undefined)
                    return;

                var oId = row.Id;
                $.getJSON($.AppConfigOptions.baseurl + 'UserHistorySetBid/GetDataManagerOptionsJson', function (_opt) {

                    _opt.title = '標案';

                    //取消自動抓後端資料
                    _opt.tableOptions.url = undefined;

                    //給detail集合
                    row.Details = row.Details || []; //無detail要實體參考，之後detail編輯才能跟master有關聯(前端物件)
                    _opt.datas = row.UserHistorySetBids;

                    _opt.beforeCreateEditDataForm = function (row, callback) {
                        row.UHSetId = oId;

                        callback();
                    }

                    //Master的編輯物件
                    var $_oform = $container.find(".data-edit-form-group");

                    //Detail的編輯物件
                    $_editDataContainer = $('<div style="background-color: #FFFFf1;padding: .5rem;border-radius: .5rem;">').appendTo($_oform.parent());

                    //實體Dou js
                    $_detailTable = $('<table>').appendTo($_editDataContainer).douTable(_opt);
                    //預設值
                    $('.bootstrap-table.userhistorysetbidcontroller .fixed-table-toolbar .btn-add-data-manager').text('新增(標案名稱)')
                });
            }

            //實體Dou js                                
            $container.douTable(_setopt);
            //預設值
            $('.bootstrap-table.userhistorysetcontroller .fixed-table-toolbar .btn-add-data-manager').text('新增(處室組別)')

            helper.misc.hideBusyIndicator();
        },
        complete: function () {
            helper.misc.hideBusyIndicator();
        }
    });

}