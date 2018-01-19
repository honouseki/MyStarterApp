(function () {
    "use strict";
    angular
        .module("mainApp")
        .factory("regLoginService", RegLoginService);

    RegLoginService.$inject = ["$http", "$q"];

    function RegLoginService($http, $q) {
        return {
            adminSelectAll: _adminSelectAll,
            selectByUsername: _selectByUsername,
            insert: _insert
        }

        function _adminSelectAll() {
            return $http.get("/api/users", { withCredentials: true })
                .then(success).catch(error);
        }

        function _selectByUsername(username) {
            return $http.get("/api/users/" + username, { withCredentials: true })
                .then(success).catch(error);
        }

        function _insert(data) {
            return $http.post("/api/users/", data, { withCredentials: true })
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