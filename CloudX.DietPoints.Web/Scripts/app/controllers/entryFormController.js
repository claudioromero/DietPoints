app.controller("entryFormController", function ($scope, $uibModalInstance, entry, entriesService, foodTypesService) {

    $scope.entry = Object.assign({}, entry);

    foodTypesService.getAll()
        .success(function (foodTypes) {
            $scope.foodTypes = foodTypes;
        })
        .error(function (response) {
            $scope.errors = parseErrors(response);
        });

    $scope.foodTypes = 
    $scope.errors = [];

    // Typeahead with model-level selection
    //$scope.selected = undefined;
    $scope.meals = [];

    // For sample purposes we will fill the typeahead with the grid contents.
    // The grid is our data source in this example.
    var section = angular.element(document.querySelector('#dietPoints-uiresults'));

    if ((section) && (section.children().length > 1)) {
        var dataChilds = section.children(1).children();
        var totalChilds = dataChilds.length;
        var newLabels = new Array();
        var newData = new Array();

        var h = 0;
        for (var idx = 1; idx < totalChilds; idx++) {
            var meal = dataChilds[idx].cells[1].innerText;
            var cal = dataChilds[idx].cells[2].innerText;
            var foodType = dataChilds[idx].cells[3].innerText;
            var i = newLabels.indexOf(meal);
            if (i === -1) {
                var m = { meal: meal, calories: cal, foodType: foodType }
                newLabels.push(meal);
                newData.push(m);
            }
        }

        $scope.meals = newData;
    }

    // When the user selects a typeahead item populate the remaining fields, not just the label.
    $scope.onSelect = function ($item, $model, $label) {
        $scope.$item = $item;
        $scope.$model = $model;
        $scope.$label = $label;

        // This can be improved. I am being a bit lazy here. This is just an example.
        for (var i = 0; i < $scope.foodTypes.length; i++)
        {
            if ($item.foodType == $scope.foodTypes[i].name)
            {
                $scope.entry.foodType = { id: $scope.foodTypes[i].id, name: $scope.foodTypes[i].name }
                break;
            }
        }

        // Update the model according to the user's selection
        $scope.entry.meal = $scope.$item.meal;
        $scope.entry.calories = parseInt($scope.$item.calories);
    };

    /*
    * This validation callback is needed because of the use of the typeahead widget.
    * When using a typeahead the scope entry bound to it looses the value when clicking on the save button.
    */
    $scope.validateSelection = function () {
        var x = angular.element(document.getElementById("meal"));
        $scope.entry.meal = x.val();
    };

    $scope.save = function () {
        entriesService.save($scope.entry)
                      .success(function () {
                          $uibModalInstance.close();
                      })
                      .error(function (response) {
                          $scope.errors = parseErrors(response);
                      });
    };

    $scope.close = function () {
        $uibModalInstance.dismiss("cancel");
    };
});