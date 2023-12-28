$(document).ready(function () {

    douoptions.viewable = true;

    $(douoptions.fields).each(function () {
        if (this.field == "PId") {
            this.formatter = function (v, d) {
                //return v;
                return '<div id="sumweek" weektype = "1" style="cursor: pointer; color: #1e0cfc;">' + v + '</div>';
            }
            this.events = {
                'click #sumweek': function (e, value, row, index) {
                    $("#_table").bootstrapTable('showColumn', 'ctrl');
                    $(".btn-view-data-manager").eq(index).trigger("click");
                    $("#_table").bootstrapTable('hideColumn', 'ctrl');
                }
            }
        }
    });

    var $_masterTable = $("#_table").DouEditableTable(douoptions); //初始dou table
    $("#_table").bootstrapTable('hideColumn', 'ctrl');
})