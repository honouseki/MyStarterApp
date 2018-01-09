(function () {
    "use strict";
    angular
        .module("mainApp" + ".routes", []);

    app.config(_configureStates);

    _configureStates.$inject = ["$stateProvider", "$locationProvider", "$urlRouterProvider"];

    function _configureStates($stateProvider, $locationProvider, $urlRouterProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
        $urlRouterProvider.otherwise("/home");
        $stateProvider
            .state({
                name: "home",
                url: "/home",
                templateUrl: "/app/main/modules/home/home.html",
                title: "Home Page",
                controller: "homeController as homeCtrl"
            })
            ;
    }
})();