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

        // Sends the link directly to the service (where it'll handle the file upload with a URL string parameter)
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

        // Takes in the file (files[0]) via 'file-model' directive, sending it through a number of functions to ultimately upload the image
        function _upload2() {
            console.log(vm.imageItem);
            // Convert the image to a base64 string for use
            convertToBase64(vm.imageItem.rawImage);
        }

        // Converts image to base64
        function convertToBase64(image) {
            console.log(image);
            var base64String = "";
            // FileReader object allows web apps to asynchronously read file content/raw data buffers
            var reader = new FileReader();
            // FileReader.readAsDataUrl reads contents of specified blob (image file);
            // When completed, data is stored in FileReader.result as a URL representing the file's data as a base64 encoded string
            reader.readAsDataURL(image);
            // FileReader.onload runs after a successful reading operation is completed (basically a success function)
            reader.onload = function () {
                // We use the result populated earlier and store it into 'base64String'
                base64String = reader.result;
                // Extract image info from base64 string for upload
                extractImage(base64String);
            };
            // FileReader.onerror; error function
            reader.onerror = function (err) {
                console.log(err);
            }
        }

        function extractImage(img) {
            // Splitting the 64base string to pull out the right data
            var imageInfo = img.split(",");
            // Grabbing the file extension from the string (jpeg/png/etc.)
            var getExtension = imageInfo[0].split("/");
            var extension = getExtension[1].split(";");
            // Storing those values into our 'imageItem' variable
            vm.imageItem.encodedImageFile = imageInfo[1];
            vm.imageItem.fileExtension = "." + extension[0];

            // Upload file!
            vm.upload();
        }
    }
})();