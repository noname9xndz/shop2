

(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);
    //$state đói tượng của ui router để điều hướng
  // $stateParams đói tượng của ui router giúp lấy ra các tham số trong uri
    productCategoryEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams','commonService'];
    function productCategoryEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {

        $scope.productCategory = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        // viết sự kiện submit
        $scope.UpdateProductCategory = UpdateProductCategory;

// chuẩn hóa seo
        $scope.GetSeoTitle = GetSeoTitle;
// nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name)
        }
//  lấy về id
        function loadProductCategoryDetail() {
            apiService.get('api/productcategory/getbyid/' + $stateParams.id,null, function (result) {
                $scope.productCategory = result.data;  
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProductCategory() {
            apiService.put('api/productcategory/update', $scope.productCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
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
        loadProductCategoryDetail();

    }
})(angular.module('shop2.productCategories'));