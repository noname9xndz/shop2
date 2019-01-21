

(function (app) {
    app.controller('productAddController', productAddController);
    //$state đói tượng của ui router để điều hướng
    productAddController.$inject = ['apiService', '$scope', 'notificationService', '$state','commonService'];
    function productAddController(apiService, $scope, notificationService, $state,commonService) {

        $scope.product = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        // viết sự kiện submit
        $scope.AddProduct = AddProduct;

        $scope.editorOptions = {
            languague: 'vi',
            height: '200px'
        };

         // chuẩn hóa seo
        $scope.GetSeoTitle = GetSeoTitle;
        // nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name)
        }

        function AddProduct() {
            // chuyển đổi sang string để add vào database
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);

            apiService.post('api/product/create', $scope.product, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                $state.go('products');
            }, function (error) {
                notificationService.displaySuccess('Thêm mới không thành công');
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

        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function (error) {
                console.log('get productcategory failed')
            })
        }
        loadProductCategory();

    }
})(angular.module('shop2.products'));