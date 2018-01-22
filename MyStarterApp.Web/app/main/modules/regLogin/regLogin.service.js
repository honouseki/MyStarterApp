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
            insert: _insert,
            login: _login,
            logout: _logout,
            checkUsername: _checkUsername,
            checkEmail: _checkEmail,
            getCurrentUser: _getCurrentUser
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

        function _login(data) {
            return $http.post("/api/users/login/" + data.remember, data, { withCredentials: true })
                .then(success).catch(error);
        }

        function _logout() {
            return $http.get("/api/users/logout", { withCredentials: true })
                .then(success).catch(error);
        }

        function _checkUsername(username) {
            return $http.get("/api/users/checkusername/" + username, { withCredentials: true })
                .then(success).catch(error);
        }

        function _checkEmail(email) {
            return $http.get("/api/users/checkemail/" + email + "/", { withCredentials: true })
                .then(success).catch(error);
        }

        function _getCurrentUser() {
            return $http.get("/api/users/getcurrentuser/", { withCredentials: true })
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