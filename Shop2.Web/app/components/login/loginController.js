

(function (app) {
    app.controller('loginController', loginController);

    loginController.$inject = ['$scope','$state'];
    function loginController($scope,$state) {

        $scope.login = function () {
            $state.go('home');
        }
    }

})(angular.module('shop2'));