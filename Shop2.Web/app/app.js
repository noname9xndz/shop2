
// khai báo 1 hàm nặc danh
(function () {
    //cấu hình router cho shop2
    angular.module('shop2', ['shop2.products', 'shop2.common']).config(config);

    // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouteProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            // truyền vào 1 trang html và  dùng <div ui-view> </div> để gọi nó ở trang index
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        // nếu không phải trường hợp nào sẽ trả về trang admin
        $urlRouteProvider.otherwise('/admin');
        
        
    }
})();// chỉ ra module này thuộc tp nào không có bỏ trống