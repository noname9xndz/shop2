


// phân trang dùng chung cho toàn project
(function (app) {
    'use strict';

    app.directive('pagerDirective', pagerDirective);

    function pagerDirective() {
        return {
            scope: { // giới hạn quyền cho các biến
                page: '@',
                pagesCount: '@',
                totalCount: '@', // @ chỉ đọc , = two way binding
                searchFunc: '&', // quyền cao nhất có quyền truyền vào 1 funtion
                customPath: '@'
            },
            replace: true, // replace toàn bộ nội dung  trả ra của directive này
            restrict: 'E', // directive này được sử dụng ở đâu(E là Element)
            templateUrl: '/app/shared/directives/pagerDirective.html',// đường dẫn của directives
            controller: [ // định nghĩa controller của riêng directive
                '$scope', function ($scope) {
                    $scope.search = function (i) {
                        if ($scope.searchFunc) {
                            $scope.searchFunc({ page: i });
                        }
                    };

                    $scope.range = function () {
                        if (!$scope.pagesCount) { return []; }
                        var step = 2;
                        var doubleStep = step * 2;
                        var start = Math.max(0, $scope.page - step);
                        var end = start + 1 + doubleStep;
                        if (end > $scope.pagesCount) { end = $scope.pagesCount; }

                        var ret = [];
                        for (var i = start; i != end; ++i) {
                            ret.push(i);
                        }

                        return ret;
                    };

                    $scope.pagePlus = function (count) {
                        return +$scope.page + count;
                    }

                }]
        }
    }

})(angular.module('shop2.common'));