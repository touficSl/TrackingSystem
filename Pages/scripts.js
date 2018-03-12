(function () {
    var demo = window.demo = {};
    var grid;
 
    demo.GridCreated = function (sender) {
        grid = sender;
    };
 
    demo.HierarchyExpanded = function (sender, args) {
        var firstClientDataKeyName = args.get_tableView().get_clientDataKeyNames()[0];
        alert(firstClientDataKeyName + ":'" + args.getDataKeyValue(firstClientDataKeyName) + "' expanded.");
    }
 
    demo.HierarchyCollapsed = function (sender, args) {
        var firstClientDataKeyName = args.get_tableView().get_clientDataKeyNames()[0];
        alert(firstClientDataKeyName + ":'" + args.getDataKeyValue(firstClientDataKeyName) + "' collapsed.");
    }
 
    demo.ExpandCollapseFirstMasterTableViewItem = function () {
        var firstMasterTableViewRow = grid.get_masterTableView().get_dataItems()[0];
        if (firstMasterTableViewRow.get_expanded()) {
            firstMasterTableViewRow.set_expanded(false);
        }
        else {
            firstMasterTableViewRow.set_expanded(true);
        }
    }
})();