var app = angular.module("poker",
    ["ui.router",
     "hubs.service",
     "event-agregator",
     "poker.home",
     "poker.header",
     "poker.tables",
     "poker.game"]);
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