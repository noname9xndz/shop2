


//chứa các thông tin authentication(xác thực) mà đã đăng nhập xong
// 

//c1 : hay viết
(function (app) {
    'use strict';
    app.factory('authData', authData);

    authData.$inject = [];
       // kiểm tra user đăng nhập chưa
    function authData() {
        var authDataFactory = {};
        var authentication = {
            // chưa đăng nhập
            IsAuthenticated: false,
            userName: ""
        };
        authDataFactory.authenticationData = authentication;
        return authDataFactory;
    }
})(angular.module('shop2.common'));

//c2 cách viết ngắn gọi :  app.factory('authData', ['$scope',function () {} : dùng gì tiêm đó

//(function (app) {
//    'use strict';
//    app.factory('authData', [function () {
//        // kiểm tra user đăng nhập chưa

//        var authDataFactory = {};

//        var authentication = { 
//            // chưa đăng nhập
//            IsAuthenticated: false,
//            userName: ""
//        };
//        authDataFactory.authenticationData = authentication;

//        return authDataFactory;
//    }]);
//})(angular.module('shop2.common'));
