app.controller("dietPointsController", function ($scope, $uibModal, $filter, confirmDialog, dietPointsService, entriesService) {
    clearFilterAndSearch();
    refreshDietPoints();

    // By default we will limit the ng-repeat to 5 items
    $scope.numberOfPreviewItems = 5;

    // The number of records per page served by the REST API
    $scope.recordsPerPage = 10;

    // Indicates whether the "Load more" button is visible or not
    $scope.showLoadMore = true;

    $scope.search = search;

    $scope.pageChanged = function () {
        getEntries();
    };

    $scope.deleteEntry = function (entry) {
        var options = {
            message: "Are you sure?",
            title: "Delete Entry",
            confirm: function () {
                entriesService.delete(entry.id).success(function () {
                    getEntries();
                    refreshDietPoints();
                });
            }
        };
        confirmDialog(options);
    };

    $scope.clearFilterAndSearch = clearFilterAndSearch;

    $scope.filterWhen = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "/scripts/app/views/entriesFilter.html",
            controller: "entriesFilterController",
            resolve: {
                filter: function () { return $scope.filter }
            },
            windowClass: "filter-when"
        });

        modalInstance.result.then(function (filter) {
            $scope.filter = filter;
            getEntries();
        });
    }

    $scope.addNewEntry = function () {
        openEntryForm({ id: 0, date: new Date() });
    };

    $scope.editEntry = function (entry) {
        openEntryForm(entry);
    };

    $scope.configureExpectedDietPointsPerDay = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "/scripts/app/views/configureDietPointsPerDay.html",
            controller: "configureDietPointsPerDayController",
            resolve: {
                dietPoints: function () { return $scope.dietPoints.expectedDailyDietPoints }
            },
            windowClass: "expected-daily-dietPoints-configuration"
        });

        modalInstance.result.then(function (dietPoints) {
            dietPointsService.setDietPointsConfiguration({
                expectedDailyDietPoints: dietPoints
            }).success(refreshDietPoints);
        });
    };

    function getEntries() {
        entriesService.getEntries({
            page: $scope.currentPage,
            dateFrom: $scope.filter.dateFrom,
            dateTo: $scope.filter.dateTo,
            timeFrom: $scope.filter.timeFrom,
            timeTo: $scope.filter.timeTo
        }).success(function (response) {
            $scope.totalRecordsCount = response.totalRecords;
            $scope.entries = response.items;

            // The two lines below are required in order to prevent performance issues, 
            // even if you implemented pagination on the server side.
            $scope.totalDisplayed = $scope.numberOfPreviewItems;
            $scope.showLoadMore = true;
        });
    };

    function search() {
        $scope.currentPage = 1;
        getEntries();
    };

    // This function takes care of the performance issues.
    // The idea is simple: we want to limit the ng-repeat and load things as we go, rather than loading the whole dataset at once.
    $scope.loadMore = function () {
        $scope.totalDisplayed += $scope.numberOfPreviewItems;
        if ($scope.totalDisplayed >= $scope.recordsPerPage) { $scope.showLoadMore = false; }
    };

    // This function renders the chart
    $scope.viewChart = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "/scripts/app/views/chart.html",
            controller: "chartController"
        });
    };

    function openEntryForm(entry) {
        var modalInstance = $uibModal.open({
            templateUrl: "/scripts/app/views/entryForm.html",
            controller: "entryFormController",
            resolve: {
                entry: function () { return entry }
            }
        });

        modalInstance.result.then(function () {
            getEntries();
            refreshDietPoints();
        });
    }

    function refreshDietPoints() {
        dietPointsService.getDietPointsInfo()
            .success(function (response) {
                $scope.dietPoints = response;
            });
    }

    function clearFilterAndSearch() {
        $scope.filter = {
            dateFrom: null,
            dateTo: null,
            timeFrom: null,
            timeTo: null
        };
        search();
    }
});