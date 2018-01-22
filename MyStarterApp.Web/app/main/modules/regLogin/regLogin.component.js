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

    RegLoginCompController.$inject = ["$scope", "$location", "regLoginService"];

    function RegLoginCompController($scope, $location, RegLoginService) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.regLoginService = RegLoginService;

        vm.onLoginSuccess = _onLoginSuccess;
        vm.getCurrentUser = _getCurrentUser;
        vm.logout = _logout;

        vm.loginStatus = false;
        vm.username = "No currently-logged in user";

        function _onInit() {
            console.log("RegLoginCompController");
            vm.getCurrentUser();
            vm.$scope.$on("loginSuccess", vm.onLoginSuccess);
        }

        function _onLoginSuccess() {
            console.log("Login Successful");
            // Upon successful login, retrieve current user's information
            vm.getCurrentUser();
        }

        function _getCurrentUser() {
            vm.regLoginService.getCurrentUser()
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                // Actions upon receiving user's immediate information
                // May consider their roles, what permissions are given based on if their account is confirmed, etc.
                // For the sake of example, username is set to display
                if (res.data.item !== null) {
                    // res.data.item will return null if no user is logged in
                    vm.username = res.data.item.username.toUpperCase();
                    vm.loginStatus = true;
                }
            }
            function error(err) {
                console.log(err);
            }
        }

        function _logout() {
            vm.regLoginService.logout()
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                vm.loginStatus = false;
                vm.username = "No currently-logged in user";
                $location.path("/home");
            }
            function error(err) {
                console.log(err);
            }
        }
    }
})();