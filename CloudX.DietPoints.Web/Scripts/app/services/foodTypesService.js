app.service("foodTypesService", function($http) {
    var baseUrl = "/api/foodTypes/"

    this.getAll = function() {
        return $http.get(baseUrl);
    };
});