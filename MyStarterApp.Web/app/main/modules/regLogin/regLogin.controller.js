(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("regLoginController", RegLoginController);

    RegLoginController.$inject = ["$scope", "regLoginService"];

    function RegLoginController($scope, RegLoginService) {
        // Setting view model, scope, onInit function, and services
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.regLoginService = RegLoginService;

        // Setting functions
        vm.register = _register;
        vm.login = _login;

        // Declared variables
        vm.regItem = {};
        vm.logItem = {};

        function _onInit() {
            console.log("regLoginController");
            // Running Admin Select All Test
            vm.regLoginService.adminSelectAll()
                .then(function (res) { console.log(res); })
                .catch(function (err) { console.log(err); });
            // Running Select By Username Test
            vm.regLoginService.selectByUsername("carrot")
                .then(function (res) { console.log(res); })
                .catch(function (err) { console.log(err); });
        }

        function _register() {
            console.log(vm.regItem);
            vm.regLoginService.insert(vm.regItem)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
            }
            function error(err) {
                console.log(err);
            }
        }

        function _login() {
            console.log(vm.logItem);
        }
    }
})();