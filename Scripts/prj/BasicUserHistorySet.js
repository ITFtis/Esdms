var BasicUserHistorySet = function ($container, FtisUHId) {    
    helper.misc.showBusyIndicator();
    $.ajax({
        url: $.AppConfigOptions.baseurl + 'UserHistorySet/getDataDetail',
        datatype: "json",
        type: "POST",
        data: { "FtisUHId": FtisUHId },
        success: function (_opt) {

            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;

            _opt.beforeCreateEditDataForm = function (row, callback) {
                row.FtisUHId = FtisUHId;

                callback();
            };

            _opt.addServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Add', callback);
            };

            _opt.updateServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Update', callback);
            };

            _opt.deleteServerData = function (row, callback) {
                transactionDouClientDataToServer(row, $.AppConfigOptions.baseurl + 'UserHistorySet/Delete', callback);
            };

            //實體Dou js                                
            $container.douTable(_opt);

            helper.misc.hideBusyIndicator();
        },
        complete: function () {
            helper.misc.hideBusyIndicator();
        }
    });

}