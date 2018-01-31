(function () {
    "use strict";
    var app = angular.module("mainApp" + ".routes", []);

    app.config(_configureStates);

    _configureStates.$inject = ["$stateProvider", "$locationProvider", "$urlRouterProvider"];

    function _configureStates($stateProvider, $locationProvider, $urlRouterProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
        // This sets the default landing page to the home page
        $urlRouterProvider.otherwise("/home");
        $stateProvider
            .state({
                name: "home",
                url: "/home",
                templateUrl: "/app/main/modules/home/home.html",
                title: "Home Page",
                controller: "homeController as homeCtrl"
            })
            .state({
                name: "regLogin",
                url: "/login",
                templateUrl: "/app/main/modules/regLogin/regLogin.html",
                title: "Register/Login Page",
                controller: "regLoginController as regLoginCtrl"
            })
            .state({
                name: "images",
                url: "/images",
                templateUrl: "/app/main/modules/images/images.html",
                title: "Images Page",
                controller: "imagesController as imagesCtrl"
            })
            .state({
                name: "links",
                url: "/links",
                templateUrl: "/app/main/modules/links/links.html",
                title: "Links Scraping Page",
                controller: "linksController as linksCtrl"
            })
            ;
    }
})();