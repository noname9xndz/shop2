(


    function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService',
        function ($scope, loginService, $injector, notificationService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {

                    //if (response != null && response.error != undefined) {
                    //    notificationService.displayError("Đăng nhập không đúng.");

                        if (response != null && response.data.error != undefined) {
                            notificationService.displayError(response.data.error_description);
                    }
                    else {
                        var stateService = $injector.get('$state');
                        stateService.go('home');
                    }
                });
            }
        }]);
})(angular.module('shop2'));