(function(){
    "use strict";
    angular
        .module("mainApp")
        .factory("linksService", LinksService);

    LinksService.$inject = ["$http", "$q"];

    function LinksService($http, $q) {
        return {
            getDeviantArt: _getDeviantArt
        };

        function _getDeviantArt(data) {
            return $http.post("/api/linksscraper/deviantart", data, { withCredentials: true })
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