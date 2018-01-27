(function(){
    "use strict";
    angular
        .module("mainApp")
        .factory("imageUploadService", ImageUploadService);

    ImageUploadService.$inject = ["$http", "$q"];

    function ImageUploadService($http, $q) {
        return {
            insert: _insert
        };

        function _insert(data) {
            return $http.post("api/images", data, { withCredentials: true })
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