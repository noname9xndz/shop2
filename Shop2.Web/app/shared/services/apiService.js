


//apiservice dùng chung cho cả app
(function (app) {
    app.factory('apiService', apiService);

    // http 1 service có sẵn của angular
    apiService.$inject = ['$http', 'notificationService', 'authenticationService'];
    function apiService($http, notificationService,authenticationService) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        }
        // định nghĩa phương thức get
        function get(url, params, success, failed) {
            authenticationService.setHeader(); //add token hiện tại vào để xác nhận
            $http.get(url, params).then(function (result) {
                success(result);
            }),
                function (error) {
                    failed(error);
                }
        }

        // định nghĩa post
        function post(url, data, success, failed) {
            authenticationService.setHeader(); //add token hiện tại vào để xác nhận
            $http.post(url, data).then(function (result) {
                success(result)
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Bạn cần đăng nhập');
                }
                else if (failed != null) {
                    failed(error);
                }
                failed(error);
            });
        }

        // định nghĩa put
        function put(url, data, success, failed) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result)
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Bạn cần đăng nhập');
                }
                else if (failed != null) {
                    failed(error);
                }
                failed(error);
            });
        }

        function del(url, params, success, failed) {
            authenticationService.setHeader();
            $http.delete(url, params).then(function (result) {
                success(result);
            }),
                function (error) {
                    failed(error);
                }
        }
    }
})(angular.module('shop2.common'));