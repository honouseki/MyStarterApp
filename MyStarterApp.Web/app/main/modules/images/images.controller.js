(function(){
    "use strict";
    angular
        .module("mainApp")
        .controller("imagesController", ImagesController);

    ImagesController.$inject = ["$scope", "imageUploadService"];

    function ImagesController($scope, ImageUploadService) {
        var vm = this;
        vm.$scope = $scope;
        vm.$onInit = _onInit;
        vm.imageUploadService = ImageUploadService;

        // Separated for demonstration
        // 'upload' for links, 'upload2' for files
        vm.upload = _upload;
        vm.upload2 = _upload2;

        vm.imageItem = {};

        function _onInit() {
            console.log("ImagesController");
        }

        function _upload() {
            console.log(vm.imageItem);
            vm.imageUploadService.insert(vm.imageItem)
                .then(success).catch(error);
            function success(res) {
                console.log(res);
            }
            function error(err) {
                console.log(err);
            }
        }

        function _upload2() {
            console.log(vm.imageItem);
        }
        
    }
})();