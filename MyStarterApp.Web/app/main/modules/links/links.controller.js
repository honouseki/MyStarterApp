(function () {
    "use strict";
    angular
        .module("mainApp")
        .controller("linksController", LinksController);

    LinksController.$inject = ["$scope", "linksService"];

    function LinksController($scope, LinksService) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.linksService = LinksService;

        vm.getDeviantArt = _getDeviantArt;

        vm.item = {};
        vm.result = {};

        function _onInit() {
            console.log("LinksController");
        }

        function _getDeviantArt() {
            vm.linksService.getDeviantArt(vm.item)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
                vm.result = res.data.item;
            }
            function error(err) {
                console.log(err);
            }
        }
    }
})();