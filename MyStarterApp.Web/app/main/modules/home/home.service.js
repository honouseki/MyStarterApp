(function () {
    "use strict";
    angular
        .module("mainApp")
        .factory("homeService", HomeService);

    HomeService.$inject = ["$http", "$q"];

    function HomeService($http, $q) {
        return {
            getEchoTest: _getEchoTest
        };

        function _getEchoTest(str) {
            return $http.get("/api/defaulttest/" + str, { withCredentials: true })
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