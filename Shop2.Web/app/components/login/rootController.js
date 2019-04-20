﻿(function (app) {
    // chứa các phương thức dùng chung cho toàn app
    app.controller('rootController', rootController);
    

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService'];

    function rootController($state, authData, loginService, $scope, authenticationService) {
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
        $scope.sideBar ="/app/shared/views/sideBar.html"
      //authenticationService.validateRequest();
    }

})(angular.module('shop2'));