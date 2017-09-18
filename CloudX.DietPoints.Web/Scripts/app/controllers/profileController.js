app.controller("profileController", function($scope, profileService, passwordValidator) {
    $scope.passwordValidator = passwordValidator;
    $scope.errors = [];
    $scope.save = function() {
        profileService.save($scope.user).success(function() {
            $scope.errors = [];
            $scope.success = true;
            })
            .error(function(response) {
                $scope.errors = parseErrors(response);
            });
    };
});