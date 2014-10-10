'use strict';

angular.module('event-agregator', [])
.factory('eventAggregatorService', ['$rootScope', function ($rootScope) {
    return {

        subscriptions: {},

        publish: function (name, data) {
            $rootScope.$emit(name, data);
        },
        subscribe: function (name, callback) {
            this.subscriptions[name] = $rootScope.$on(name, callback);
        },

        unsubscribe: function (name) {
            this.subscriptions[name]();
            delete this.subscriptions[name];
        }
    };
}]);;