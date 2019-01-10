﻿

(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);
    //$state đói tượng của ui router để điều hướng
    productCategoryAddController.$inject = ['apiService', '$scope','notificationService','$state'];
    function productCategoryAddController(apiService, $scope, notificationService,$state) {

        $scope.productCategory = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        // viết sự kiện submit
        $scope.AddProductCaterory = AddProductCaterory;

        function AddProductCaterory() {
            apiService.post('api/productcategory/create', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                $state.go('product_categories');
            }, function(error){
                notificationService.displaySuccess('Thêm mới không thành công');
            });
        }
        
        function loadParentCategories() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('cannot get list parent');
                });
        }
        loadParentCategories();

    }
})(angular.module('shop2.productCategories'));