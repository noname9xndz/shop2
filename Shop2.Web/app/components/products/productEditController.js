

(function (app) {
    app.controller('productEditController', productEditController);
    //$state đói tượng của ui router để điều hướng
    // $stateParams đói tượng của ui router giúp lấy ra các tham số trong uri
    productEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];
    function productEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {

        $scope.product = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        // viết sự kiện submit
        $scope.Updateproduct = Updateproduct;

        // chuẩn hóa seo
        $scope.GetSeoTitle = GetSeoTitle;
        // nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name)
        }
        //  lấy về id
        function loadproductDetail() {
           
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);
            apiService.get('api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;
                // load moreImages chuyển đổi từ json sang mảng để angular js đọc
                $scope.moreImages = JSON.parse ($scope.product.MoreImages);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function Updateproduct() {
            // chuyển đổi sang string để add vào database
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);

            apiService.put('api/product/update', $scope.product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
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

        // up ảnh mô tả sp
        $scope.moreImages = [];

        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }

        function loadProduct() {
            apiService.get('api/product/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('cannot get list parent');
            });
        }
        loadProduct();
        loadproductDetail();

    }
})(angular.module('shop2.products'));