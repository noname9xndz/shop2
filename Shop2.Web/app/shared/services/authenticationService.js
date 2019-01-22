



(function (app) {
    'use strict';
    // $q là 1 service giúp chúng ta sử dụng, tạo ra 1 đối tượng promise(giúp đảm bảo việc thực thi trước sau)
    // $window là service giúp ta lưu sessionStorage

    app.service('authenticationService', authenticationService);

     authenticationService.$inject = ['$http', '$q', '$window'];

     function authenticationService($http, $q, $window) {

         var tokenInfo;

        this.setTokenInfo = function (data) {
            tokenInfo = data;
            $window.sessionStorage["TokenInfo"] = JSON.stringify(tokenInfo);
        }

        this.getTokenInfo = function () {
            return tokenInfo;
        }

        this.removeToken = function () {
            tokenInfo = null;
            $window.sessionStorage["TokenInfo"] = null;
        }

        this.init = function () {
            if ($window.sessionStorage["TokenInfo"]) {
                tokenInfo = JSON.parse($window.sessionStorage["TokenInfo"]);
            }
        }
        // gán header của token trả về gán vào header Authorization
        this.setHeader = function () {
            delete $http.defaults.headers.common['X-Requested-With'];
            if ((tokenInfo != undefined) && (tokenInfo.accessToken != undefined) && (tokenInfo.accessToken != null) && (tokenInfo.accessToken != "")) {
                $http.defaults.headers.common['Authorization'] = 'Bearer ' + tokenInfo.accessToken;
                $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            }
        }
        // xác nhận xem user đăng nhập chưa
        this.validateRequest = function () {
            var url = 'api/home/TestMethod';
            // xác nhận thành công
            var deferred = $q.defer();
            $http.get(url).then(function () {
                deferred.resolve(null);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }

        this.init();
    }
    
})(angular.module('shop2.common'));