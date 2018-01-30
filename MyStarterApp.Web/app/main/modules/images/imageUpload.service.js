(function(){
    "use strict";
    angular
        .module("mainApp")
        .factory("imageUploadService", ImageUploadService);

    ImageUploadService.$inject = ["$http", "$q"];

    function ImageUploadService($http, $q) {
        return {
            insert: _insert,
            selectAll: _selectAll,
            selectById: _selectById,
            delete: _delete
        };

        function _insert(data) {
            return $http.post("api/images", data, { withCredentials: true })
                .then(success).catch(error);
        }

        function _selectAll() {
            return $http.get("api/images", { withCredentials: true })
                .then(success).catch(error);
        }

        function _selectById(id) {
            return $http.get("api/images/" + id, { withCredentials: true })
                .then(success).catch(error);
        }

        function _delete(data) {
            return $http.delete("api/images/delete",
                { data, headers: {"Content-Type": "application/json;charset=utf-8"} })
                .then(success).catch(error);
        }

        function success(res) {
            return res;
        }

        function error(err) {
            return err;
        }
    }
})();