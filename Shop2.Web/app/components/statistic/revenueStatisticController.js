

// sử dụng char + chart js để vẽ biểu đồ
(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];
    
    function revenueStatisticController($scope, apiService, notificationService, $filter) {

        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.chartdata = [];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/01/2018',
                    toDate: '01/01/2022'
                }
            }
       apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
           $.each(response.data, function (i, item) {
                 // đẩy số liệu vào các biến  
               labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
           });
            // add số liệu ra ngoài view
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
           $scope.labels = labels;

            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }

        getStatistic();
    }

})(angular.module('shop2.statistics'));