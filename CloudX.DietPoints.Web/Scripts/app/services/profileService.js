app.service("profileService", function($http) {
    this.save = function(profile) {
        return $http.put("/api/me/password", profile);
    };
});