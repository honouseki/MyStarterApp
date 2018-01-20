(function () {
    "use strict";
    angular
        .module("mainApp")
        .component("regLoginComponent", {
            templateUrl: "/app/main/modules/regLogin/regLoginDetails.html",
            controller: "regLoginCompController"
        });
})();

(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("regLoginCompController", RegLoginCompController);

    RegLoginCompController.$inject = ["$scope"];

    function RegLoginCompController($scope) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;

        vm.onLoginSuccess = _onLoginSuccess;

        vm.loginStatus = false;
        vm.username = "no currently-logged in user".toUpperCase();

        function _onInit() {
            console.log("RegLoginCompController");
            vm.$scope.$on("loginSuccess", vm.onLoginSuccess);
        }

        function _onLoginSuccess() {
            console.log("Login Successful");
            vm.loginStatus = true;
            vm.loginStatus = true;
            // Temporary hard-coded until GetCurrentUser available
            vm.username = "carrot4".toUpperCase();
        }
    }
})();