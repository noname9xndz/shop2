

(function () {
    //cấu hình router
    angular.module('shop2.productCategories', ['shop2.common']).config(config);
    // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('product_categories', {
                url: "/product_categories",
                templateUrl: "/app/components/product_categories/productCategoryListView.html",
                controller: "productCategoryListController"
            })
            .state('productCategoryAdd', {
                url: "/productCategoryAdd",
                templateUrl: "/app/components/product_categories/productCategoryAddView.html",
                controller: "productCategoryAddController"
            })
            .state('productCategoryEdit', {
                url: "/productCategoryEdit/:id",
                templateUrl: "/app/components/product_categories/productCategoryEditView.html",
                controller: "productCategoryEditController"
            }); ; 

    }
})();// chỉ ra module này thuộc tp nào không có bỏ trống