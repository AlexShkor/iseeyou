angular.module('poker.header', []).controller('HeaderCtrl', 
function ($scope, $location, eventAggregatorService, $timeout, $http) {
   
    $scope.logout = function () {
        window.location.href = "/account/LogOff";
    };

    $scope.search = function () {
        if ($scope.searchTerm) {
            $location.path("/search/" + $scope.searchTerm);
        }
    };


    eventAggregatorService.subscribe('messagereceived', function (type, data) {
        $scope.$apply(function () {
           
        });
    });
});