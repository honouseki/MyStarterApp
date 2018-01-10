(function () {
    "use strict";
    window.APP = window.APP || {};
    APP.NAME = "mainApp";
    angular
        .module(APP.NAME, [
            "ui.router",
            APP.NAME + ".routes",
            "ngRoute",
            "ngMaterial",
            "ngMessages",
            "ngCookies"
        ]);
})();