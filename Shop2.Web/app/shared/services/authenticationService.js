



(function (app) {
    'use strict';
    // $q là 1 service giúp chúng ta sử dụng, tạo ra 1 đối tượng promise(giúp đảm bảo việc thực thi trước sau)
    // $window là service giúp ta lưu sessionStorage
    //--LocalStorageModule đảm bảo khi fresh tài khoản vẫn ở trạng thái đăng nhập :https://www.npmjs.com/package/angular-local-storage , bản chất là lưu dữ liệu ở localStorage
    //http://forum.tedu.com.vn/thread/cach-khac-phuc-loi-refresh-mat-username-%C4%91ang-nhap-trong-trang-admin/
    app.service('authenticationService', authenticationService);

    authenticationService.$inject = ['$http', '$q', '$window', 'localStorageService','authData'];

    function authenticationService($http, $q, $window, localStorageService,authData) {

        var tokenInfo;

        this.setTokenInfo = function (data) {
            tokenInfo = data;
            // $window.sessionStorage["TokenInfo"] = JSON.stringify(tokenInfo); lưu token trên session
            localStorageService.set("TokenInfo", JSON.stringify(tokenInfo)); // dùng LocalStorageModule lưu token ở dạng key ,value

            this.getTokenInfo = function () {
                return tokenInfo;
            }

            this.removeToken = function () {
                tokenInfo = null;
                // $window.sessionStorage["TokenInfo"] = null;
                localStorageService.set("TokenInfo", null);
            }

            this.init = function () {

                var tokenInfo = localStorageService.get("TokenInfo");
                //if ($window.sessionStorage["TokenInfo"]) {
                if (tokenInfo) {
                    // tokenInfo = JSON.parse(localStorageService.get("TokenInfo"));
                    tokenInfo = JSON.parse(tokenInfo);
                    // lấy các đối tượng từ audata
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.IsAuthenticated = tokenInfo.IsAuthenticated;
                    authData.authenticationData.userName = tokenInfo.userName;
                }
            }
            // gán header của token trả về gán vào header Authorization
            //this.setHeader = function () {
            //    delete $http.defaults.headers.common['X-Requested-With'];
            //    if ((tokenInfo != undefined) && (tokenInfo.accessToken != undefined) && (tokenInfo.accessToken != null) && (tokenInfo.accessToken != "")) {
            //        $http.defaults.headers.common['Authorization'] = 'Bearer ' + tokenInfo.accessToken;
            //        $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            //    }
            //}
            this.setHeader = function () {
                delete $http.defaults.headers.common['X-Requested-With'];
                if ((authData.authenticationData != undefined) && (authData.authenticationData.accessToken != undefined) && (authData.authenticationData.accessToken != null) && (authData.authenticationData.accessToken != "")) {
                    $http.defaults.headers.common['Authorization'] = 'Bearer ' + authData.authenticationData.accessToken;
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

    }
}) (angular.module('shop2.common'));