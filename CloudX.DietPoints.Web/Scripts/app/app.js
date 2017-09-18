var app = angular.module("DietPointsApp", ["ngRoute", "ngResource", "ui.bootstrap", "chart.js"]);

app.config(function($routeProvider) {

    $routeProvider
        .when("/profile", {
            controller: "profileController",
            templateUrl: "/scripts/app/views/profile.html"
        });

    $routeProvider
        .when("/dietPoints", {
            controller: "dietPointsController",
            templateUrl: "/scripts/app/views/dietPoints.html"
        })
        .otherwise({ redirectTo: "/dietPoints" });
});

function parseErrors(response) {
    var errors = [];

    if (response.modelState != null) {
        for (var key in response.modelState) {
            for (var i = 0; i < response.modelState[key].length; i++) {
                errors.push(response.modelState[key][i]);
            }
        }
    }

    if (errors.length == 0)
        errors.push(response.message);
    
    return errors;
}