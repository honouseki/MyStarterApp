(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("homeController", HomeController);

    HomeController.$inject = ["$scope", "homeService"];

    function HomeController($scope, HomeService) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.homeService = HomeService;

        // AJAX Calls
        vm.echoTest = _echoTest;

        // Using the given object in this item to test Angular's two-way binding
        vm.item = {};

        function _onInit() {
            console.log("Home Controller");
        }

        function _echoTest() {
            vm.homeService.getEchoTest(vm.item.inputString)
                .then(function (res) { console.log(res); })
                .catch(function (err) { console.log(err); });
        }
    }
})();