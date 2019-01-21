(function (app) {
    // chứa các phương thức dùng chung cho toàn app
    app.controller('rootController', rootController);

    rootController.$inject = ['$scope', '$state'];
    function rootController($scope, $state) {
        // logout dùng chung cho cả app
        $scope.logout = function () {
            $state.go('login');
        }
    }

})(angular.module('shop2'));