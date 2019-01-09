

// khai báo controller
(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope','apiService'];
    function productCategoryListController($scope,apiService) {
        $scope.productCategories = [];

        // lấy dữ liệu từ service
        $scope.getProductCategories = getProductCategories;

        function getProductCategories() {
            apiService.get('/api/productcategory/getall', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('load productcategory failed');
                });
        }
        $scope.getProductCategories();

    }
})(angular.module('shop2.productCategories')); // chỉ ra controller này thuộc module nào