app.service("dietPointsService", function($http) {
    this.getDietPointsInfo = function() {
        return $http.get("/api/me/dietPoints");
    };

    this.setDietPointsConfiguration = function (config) {
        return $http.put("/api/me/dietPoints/configuration", config);
    };
});