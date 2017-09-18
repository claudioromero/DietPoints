app.service("entriesService", function($http, $routeParams) {
    var baseUrl = "/api/me/entries/"

    this.delete = function(id) {
        return $http.delete(baseUrl + id);
    };

    this.getEntries = function(query) {
        var param = "?page=" + query.page;

        if (query.dateFrom != null) {
            param += "&dateFrom=" + query.dateFrom.toISOString();
            param += "&dateTo=" + query.dateTo.toISOString();
        }

        if (query.timeFrom != null) {
            param += "&timeFrom=" + query.timeFrom.toISOString();
            param += "&timeTo=" + query.timeTo.toISOString();
        }

        return $http.get(baseUrl + param);
    };

    this.save = function(entry) {
        if (entry.id == 0) {
            return $http.post(baseUrl, entry);
        } else {
            return $http.put(baseUrl + entry.id, entry);
        }
    };
});