app.controller("configureDietPointsPerDayController", function ($scope, $uibModalInstance, dietPoints) {
    $scope.model = { dietPoints: dietPoints };

    $scope.save = function() {
        $uibModalInstance.close($scope.model.dietPoints);
    };

    $scope.close = function() {
        $uibModalInstance.dismiss("cancel");
    };
});