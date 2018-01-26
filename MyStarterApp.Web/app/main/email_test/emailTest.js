// Have the service and the controller here...just to run it. And the component ()
// This is just to test being able to send an email from the front-end

// Component
(function () {
    "use strict";
    angular
        .module("mainApp")
        .component("emailTestComponent", {
            templateUrl: "/app/main/email_test/emailTest.html",
            controller: "emailTestController"
        });
})();

// Controller
(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("emailTestController", EmailTestController);

    EmailTestController.$inject = ["$scope", "emailTestService"];

    function EmailTestController($scope, EmailTestService) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.emailTestService = EmailTestService;
        vm.sendEmail = _sendEmail;

        function _onInit() {
            console.log("EmailTestController");
        }

        function _sendEmail() {
            vm.emailTestService.emailTest()
                .then(function (res) { console.log(res); })
                .catch(function (err) { console.log(err); });
        }
    }
})();

// Service
(function () {
    "use strict";
    angular
        .module('mainApp')
        .factory("emailTestService", EmailTestService);

    EmailTestService.$inject = ["$http", "$q"];

    function EmailTestService($http, $q) {
        return {
            emailTest: _emailTest
        };

        function _emailTest() {
            return $http.get("/api/defaulttest/emailtest", { withCredentials: true })
                .then(success).catch(error);
        }

        function success(res) {
            return res;
        }

        function error(err) {
            return $q.reject(err);
        }
    }
})();