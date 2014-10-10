var app = angular.module("app",
    ["ui.router",
     "hubs.service",
     "event-agregator",
     "app.home",
     "app.header"]);
app.value('$', $);
app.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

    $locationProvider.html5Mode(true);
    $urlRouterProvider.otherwise('/');

    $stateProvider
        .state('home', { url: '/', templateUrl: '/app/home.html', controller: 'HomeController' })
        .state("myprofile", { url: '/profile', templateUrl: '/profile/view', controller: 'ProfileController' });
});

app.controller('AppCtrl', ['$scope', '$rootScope', "signalsService", "eventAggregatorService", function ($scope, $rootScope, signalsService, eventAggregatorService) {
    $scope.init = function (user) {
        signalsService.initialize();
    };
}]); 