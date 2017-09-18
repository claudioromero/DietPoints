app.controller("entriesFilterController", function($scope, $uibModalInstance, filter) {
    $scope.filter = {
        filterTime: filter.timeFrom != null,
        filterDate: filter.dateFrom != null
    };

    if (!$scope.filter.filterTime) {
        $scope.filter.filterTime = false;
        $scope.filter.timeFrom = new Date();
        $scope.filter.timeFrom.setHours(0);
        $scope.filter.timeFrom.setMinutes(0);
        $scope.filter.timeTo = new Date();
        $scope.filter.timeTo.setHours(23);
        $scope.filter.timeTo.setMinutes(59);
    } else {
        $scope.filter.timeFrom = filter.timeFrom;
        $scope.filter.timeTo = filter.timeTo;
    }

    if (!$scope.filter.filterDate) {
        $scope.filter.filterDate = false;
        $scope.filter.dateFrom = new Date();
        $scope.filter.dateFrom.setDate($scope.filter.dateFrom.getDate() - 1); //Yesterday
        $scope.filter.dateTo = new Date();
    } else {
        $scope.filter.dateFrom = filter.dateFrom;
        $scope.filter.dateTo = filter.dateTo;
    }

    $scope.doFilter = function() {
        var filter = {};
        if ($scope.filter.filterDate) {
            filter.dateFrom = $scope.filter.dateFrom;
            filter.dateTo = $scope.filter.dateTo;
            filter.dateFrom.setHours(0);
            filter.dateFrom.setMinutes(0);
            filter.dateFrom.setSeconds(0);
            filter.dateTo.setHours(23);
            filter.dateTo.setMinutes(59);
            filter.dateTo.setSeconds(59);
        } else {
            filter.dateFrom = null;
            filter.dateTo = null;
        }
        if ($scope.filter.filterTime) {
            filter.timeFrom = $scope.filter.timeFrom;
            filter.timeTo = $scope.filter.timeTo;
            filter.timeFrom.setSeconds(0);
            filter.timeTo.setSeconds(59);
        } else {
            filter.timeFrom = null;
            filter.timeTo = null;
        }
        $uibModalInstance.close(filter);
    };

    $scope.close = function() {
        $uibModalInstance.dismiss("cancel");
    };
});