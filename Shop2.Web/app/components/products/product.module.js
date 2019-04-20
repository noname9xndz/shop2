

(function () {
    //cấu hình router
    angular.module('shop2.products', ['shop2.common']).config(config);
     // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('products', {
                url: "/products",
                parent : 'base',
                templateUrl: "/app/components/products/productListView.html",
                controller: "productListController"
            })
            .state('productAdd', {
                url: "/productAdd",
                parent: 'base',
                templateUrl: "/app/components/products/productAddView.html",
                controller: "productAddController"
            })
            .state('product_import', {
                url: "/product_import",
                parent: 'base',
                templateUrl: "/app/components/products/productImportView.html",
                controller: "productImportController"
            })
            .state('productEdit', {
                url: "/productEdit/:id",
                parent: 'base',
                templateUrl: "/app/components/products/productEditView.html",
                controller: "productEditController"
            });

    }
})();// chỉ ra module này thuộc tp nào không có bỏ trống