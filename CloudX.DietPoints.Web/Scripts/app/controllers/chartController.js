app.controller("chartController", function ($scope, $uibModalInstance, entriesService) {

    $scope.title = "Diet Points per Food Type";
    $scope.labels = null;
    $scope.data = null;

    var section = angular.element(document.querySelector('#dietPoints-uiresults'));

    if ((section) && (section.children().length > 1))
    {
        var dataChilds = section.children(1).children();
        var totalChilds = dataChilds.length;

        var newLabels = new Array();
        var newData = new Array();

        var foodTypeIndex = 3;
        var dietPointsIndex = foodTypeIndex + 1;

        var h = 0;
        for (var idx = 1; idx < totalChilds; idx++) {
            var foodType = dataChilds[idx].cells[foodTypeIndex].innerText;
            var dataMeasurement = new Number(dataChilds[idx].cells[dietPointsIndex].innerText);

            var i = newLabels.indexOf(foodType);

            if (i === -1) {
                newLabels.push(foodType);
                newData.push(dataMeasurement);
            }
            else {
                newData[i] += dataMeasurement;
            }
        }

        $scope.labels = newLabels;
        $scope.data = newData;
    }

    $scope.close = function () {
        $uibModalInstance.dismiss("cancel");
    };
});