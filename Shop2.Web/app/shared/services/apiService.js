


//service dùng chung cho cả app
(function (app) {
    app.factory('apiService', apiService);

    // http 1 service có sẵn của angular
    apiService.$inject = ['$http','notificationService'];
    function apiService($http, notificationService) {
        return {
            get: get,
            post:post
        }
        // định nghĩa phương thức get
        function get(url, params, success, failed) {
            $http.get(url, params).then(function (result) {
                success(result);
            }),
                function (error) {
                    failed(error);
                }
        }
        // định nghĩa post
        function post(url, data, success, failed) {
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
    }
})(angular.module('shop2.common'));