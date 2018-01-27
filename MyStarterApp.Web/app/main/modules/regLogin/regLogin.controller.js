(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("regLoginController", RegLoginController);

    RegLoginController.$inject = ["$scope", "regLoginService", "$location", "$rootScope"];

    function RegLoginController($scope, RegLoginService, $location, $rootScope) {
        // Setting view model, scope, onInit function, and services
        var vm = this;
        vm.$scope = $scope;
        vm.$rootScope = $rootScope;
        vm.$onInit = _onInit;
        vm.regLoginService = RegLoginService;

        // Setting functions
        vm.register = _register;
        vm.login = _login;
        vm.checkUser = _checkUser;
        vm.checkEmail = _checkEmail;

        // Declared variables
        vm.regItem = {};
        vm.logItem = {
            remember: false
        };
        vm.loginFail = false;
        vm.suspended = false;
        vm.userExists = false;
        vm.emailExists = false;

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

        // Register
        function _register() {
            vm.regLoginService.insert(vm.regItem)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                // First checks the result item; if it's -1, this indicates that the username already exists 
                if (res.data.item == -1) {
                    vm.userExists = true;
                } else {
                    // Upon successful registration, proceed to log in user
                    vm.logItem = vm.regItem;
                    vm.login();
                }
            }
            function error(err) {
                console.log(err);
                alert("Registration failed!");
            }
        }

        // Login
        function _login() {
            vm.regLoginService.login(vm.logItem)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                if (res.data.item == 1) {
                    // Logs user in
                    console.log("Logged in!");
                    vm.loginFail = false;
                    vm.suspended = false;
                    // Broadcasts user login to the rest of the app, then redirects user back to the home page
                    vm.$rootScope.$broadcast("loginSuccess");
                    $location.path("/home");
                } else if (res.data.item == -1) {
                    // If returned -1, indicates a suspended account
                    console.log("SUSPENDED ACCOUNT!");
                    vm.suspended = true;
                    vm.loginFail = false;
                } else {
                    // Fails login if username/password is incorrect
                    alert("Login failed!");
                    vm.loginFail = true;
                    vm.suspended = false;
                }
            }
            function error(err) {
                console.log(err);
                alert("Server failed to log you in!");
                vm.loginFail = false;
                vm.suspended = false;
            }
        }

        // Check Username
        function _checkUser(username) {
            vm.regLoginService.checkUsername(username)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                if (res.data.item !== 0) {
                    console.log("Username taken!")
                    vm.userExists = true;
                } else {
                    vm.userExists = false;
                }
            }
            function error(err) {
                console.log(err);
            }
        }

        // Check Email
        function _checkEmail(email) {
            vm.regLoginService.checkEmail(vm.regItem.email)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                if (res.data.item !== 0) {
                    console.log("Email taken!")
                    vm.emailExists = true;
                } else {
                    vm.emailExists = false;
                }
            }
            function error(err) {
                console.log(err);
            }
        }
    }
})();