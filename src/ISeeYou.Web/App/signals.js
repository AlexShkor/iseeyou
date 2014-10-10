'use strict';

angular.module("hubs.service", ['event-agregator'])
    .service('signalsService', ['eventAggregatorService', function (eventAggregatorService) {

        var proxy = null;

        var initialize = function () {

            $.connection.hub.url = "/signalr";

            var connection = $.hubConnection();
            connection.logging = true;
            proxy = connection.createHubProxy('usersHub');

            connection.start().done(function () {
                console.log("SignalR Started");
            })
            .fail(function () {
                console.log("SignalR faild");
            });
            proxy.on('testevent', function (data) {
                eventAggregatorService.publish('testevent', data);
            });

        };

        var invoke = function (method, data) {
            proxy.invoke(method, data);
        };

        return {
            initialize: initialize,
            invoke: invoke
        };
    }]);
