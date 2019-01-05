
//khai báo module
var myApp = angular.module("myModule", []);

myApp.directive("shop2Directive", shop2Directive);
// khai báo controller 1 cách tường minh
myApp.controller("myController1", myController1);
//myApp.controller("myController2", myController2); 
//myApp.controller("allController", allController); 
//myController.$inject = ['$scope']; // mặc định $scope sẽ được tiêm vào myController

//ví dụ về $scope
//function myController1($scope) {
//    $scope.message = "this is my message from Controller1";

//}
//function myController2($scope) {
//    $scope.message = "this is my message from Controller2";}

//ví dụ về $rootscope
//function myController1($rootScope,$scope) {
//    $rootScope.message = "this is my message from Controller1";

//}
//function myController2($scope) {}

//ví dụ về $scope lồng nhau : tính kế thừa , thực thi bên ngoài trước nhưng 
//controller bên trong hoàn toàn có thể ghi đè
//function allController($scope) {
//    $scope.message = "All Scope";

//}
//function myController1($scope) {
//    $scope.message = "this is my message from Controller2";
//}
//function myController2($scope) {}

// ví dụ về service
myApp.service("ValidatorService", ValidatorService);
myController1.$inject = ['$scope', "ValidatorService"];
function myController1($scope, ValidatorService) {
    //ValidatorService.check(1000);
    //$scope.message = ValidatorService.check(1000);
    //$scope.check2 = function () {
    //    $scope.message = ValidatorService.check(1000);
    //}
    //$scope.so=1

    // cơ chế two way binding
        $scope.check2 = function () {
            $scope.message = ValidatorService.check($scope.so);
    }
  
}
function ValidatorService($window) {
    return {
        check : checkNumber
    }
    function checkNumber(x) {
        if (x % 2 == 0) {
            //$window.alert("số chẵn");
            return "số chẵn";
        }
        else {
            //$window.alert("số lẻ");
            return "số lẻ";
        }
    }
}

// ví dụ về Directive Cho phép mở rộng HTML theo ý của bạn và bạn có thể tùy chỉnh lại các thuộc tính, phần tử, … (elements,) 
//function shop2Directive() {
//    return {
//        //template : "<h1>đây là ví dụ về directive</h1>"
//        templateUrl: "/Scripts/spa/TestDirective.html"
//    }
//}

// ví dụ về Hạn chế truy cập cho Directive 
//  "E" cho Element , "A" cho Attribute , "C" cho Class 
//  "M" cho Comment , Mặc định, giá trị "EA" sẽ hạn chế cho cả Element và Attribute 
function shop2Directive() {
    return {
        restrict: "A",
        //template : "<h1>đây là ví dụ về directive</h1>"
        templateUrl: "/Scripts/spa/TestDirective.html"
    }
}