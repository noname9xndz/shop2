



(function (app) {
    app.controller('pageAddController', pageAddController);
    //$state đói tượng của ui router để điều hướng
    pageAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function pageAddController(apiService, $scope, notificationService, $state, commonService) {

        $scope.page = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        $scope.editorOptions = {
            languague: 'vi',
            height: '200px'
        };

        // viết sự kiện submit
        $scope.AddPage = AddPage;

        $scope.GetSeoTitle = GetSeoTitle;
        // nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.page.Alias = commonService.getSeoTitle($scope.page.Name)
        }

        // up load ảnh sp
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                })
            }
            finder.popup();
        }

        function AddPage() {
            apiService.post('api/page/create', $scope.page, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                $state.go('pages');
            }, function (error) {
                notificationService.displaySuccess('Thêm mới không thành công');
            });
        }



    }
})(angular.module('shop2.pages'));