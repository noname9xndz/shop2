



(function (app) {
    app.controller('pageEditController', pageEditController);
    //$state đói tượng của ui router để điều hướng
    // $stateParams đói tượng của ui router giúp lấy ra các tham số trong uri
    pageEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];
    function pageEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {

        $scope.page = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }
        $scope.editorOptions = {
            languague: 'vi',
            height: '200px'
        };

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                })
            }
            finder.popup();
        }

        // viết sự kiện submit
        $scope.UpdatePage = UpdatePage;
       
        // chuẩn hóa seo
        $scope.GetSeoTitle = GetSeoTitle;
        // nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.page.Alias = commonService.getSeoTitle($scope.page.Name)
        }
        //  lấy về id
        function loadPageDetail() {
            apiService.get('api/page/getbyid/' + $stateParams.id, null, function (result) {
                $scope.page = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdatePage() {
            apiService.put('api/page/update', $scope.page,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('pages');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

   
        loadPageDetail();

    }
})(angular.module('shop2.pages'));