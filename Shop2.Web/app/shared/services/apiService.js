


//service dùng chung cho cả app
(function (app) {
    app.factory('apiService', apiService);

    // http 1 service có sẵn của angular
    apiService.$inject = ['$http'];
    function apiService($http) {
        return {
            get : get
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
    }
})(angular.module('shop2.common'));