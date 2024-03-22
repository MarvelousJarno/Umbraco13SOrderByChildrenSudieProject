angular.module('umbraco').controller('OrderChildrenByPropertyPluginController', function ($scope, $http, $routeParams) {
    $http({
        method: "GET",
        url: "/Umbraco/backoffice/GetData/OrderByChildrenApi/GetAllPropertiesOfChildren",
        params: {
            currentPageId: $routeParams.id
        }
    }).then(function (response) {
        $scope.model.PropertiesToSortOn = response.data;
        console.log($scope.model.value);
    }, function (response) {
        console.log(response);
        //todo error afhandeling
    });
});