

// khai báo controller
(function (app) {
    app.controller('productCategoryListController', productCategoryListController);
    // truyền đối tượng apiService  để get dữ liệu từ webapi
    productCategoryListController.$inject = ['$scope', 'apiService','notificationService'];

    function productCategoryListController($scope, apiService, notificationService) {
        // lấy tạo mảng productCategories
        $scope.productCategories = [];

        $scope.page = 0;
        $scope.pageCount = 0;

        // lấy dữ liệu từ webapi
        $scope.getProductCategories = getProductCategories;
        $scope.keyword = '';

        $scope.search = search;

        function search() {
            getProductCategories();
        }
            
        function getProductCategories(page) {
            page = page || 0;
            var cofig = {
                //params 1 định dạng để truyền params vào uri
                params: {
                    keyword: $scope.keyword,
                    page : page ,
                    pageSize : 20
                }
            }

            apiService.get('/api/productcategory/getall', cofig, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('không có bản ghi nào được tìm thấy');
                }
                else {
                    notificationService.displaySuccess('tìm thấy ' + result.data.TotalCount + ' bản ghi');
                }
                // lấy về data 
                $scope.productCategories = result.data.Items;
                // lấy về giá trị dùng để phân trang
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;

            }, function () {
                console.log('load productcategory failed');
                });
        }
        $scope.getProductCategories();

    }
})(angular.module('shop2.productCategories')); // chỉ ra controller này thuộc module nào