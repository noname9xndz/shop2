
// khai báo 1 hàm nặc danh
(function () {
    //cấu hình router cho shop2
    angular.module('shop2', ['shop2.products',
        'shop2.productCategories',
        'shop2.application_groups',
        'shop2.application_roles',
        'shop2.application_users',
        'shop2.pages',
        'shop2.common'])
        .config(config)
        .config(configAuthentication);

    // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider giúp điều hướng trang
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouteProvider) {
        $stateProvider
            // chứa trang admin (template chính,cha chứa các template con)
            .state('base', {
                url: '',
                templateUrl: '/app/shared/views/baseView.html',
                abstract: true
            })

            // các trang kế thừa ,trang con trong trang admin
            // trang login
            .state('login', {
                url: "/login",
                templateUrl: "/app/components/login/loginView.html",
                controller: "loginController"
            })
            // phần thao tác trang admin
            .state('home', {
                url: "/admin",
                parent: 'base',
                // truyền vào 1 trang html và  dùng <div ui-view> </div> để gọi nó ở trang index
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            });
        // nếu không phải trường hợp nào sẽ trả về trang login
        $urlRouteProvider.otherwise('/login');

    }

    // check request lên có token hay ko
    // interceptors giúp quản trị việc tương tác giữa client và server
    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {
                    return config;
                },
                requestError: function (rejection) {

                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401") {
                        $location.path('/login');
                    }
                    return response;
                },
                responseError: function (rejection) {

                    if (rejection.status == "401") {
                        $location.path('/login');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }

})();// chỉ ra module này thuộc tp nào không có bỏ trống