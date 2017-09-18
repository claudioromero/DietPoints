app.controller("confirmController", function($scope, $uibModalInstance, options) {
    $scope.message = options.message;
    $scope.title = options.title;

    $scope.confirm = function() {
        $uibModalInstance.close();
    };

    $scope.close = function() {
        $uibModalInstance.dismiss("cancel");
    };
});

app.factory("confirmDialog", function($uibModal) {

    return function(options) {
        var modalInstance = $uibModal.open({
            templateUrl: "/scripts/app/views/confirm.html",
            controller: "confirmController",
            resolve: {
                options: function() { return options }
            },
            windowClass: "confirmation"
        });

        modalInstance.result.then(options.confirm, options.cancel);
    };
});