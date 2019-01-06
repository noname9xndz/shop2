

(function () {
    //cấu hình router
    angular.module('shop2.products', ['shop2.common']).config(config);
     // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('products', {
                url: "/products",
                templateUrl: "/app/components/products/productListView.html",
                controller: "productListController"
            })
            .state('product_add', {
                url: "/product_add",
                templateUrl: "/app/components/products/productAddView.html",
                controller: "productAddController"
            });

    }
})();// chỉ ra module này thuộc tp nào không có bỏ trống