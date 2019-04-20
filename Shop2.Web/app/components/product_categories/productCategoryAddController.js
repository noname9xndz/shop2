

(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);
    //$state đói tượng của ui router để điều hướng
    productCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function productCategoryAddController(apiService, $scope, notificationService, $state,commonService) {

        $scope.productCategory = {
            // khởi tạo mặc định các giá trị
            CreateDate: new Date(),
            Status: true
        }

        // viết sự kiện submit
        $scope.AddProductCategory = AddProductCategory;
        $scope.flatFolders = []; // tạo 1 danh mục tổng để chứa parentID
        $scope.GetSeoTitle = GetSeoTitle;
        // nhớ gọi hàm này bằng ng-change trên trang html
        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name)
        }
    

        function AddProductCategory() {
            apiService.post('api/productcategory/create', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                $state.go('product_categories');
            }, function(error){
                notificationService.displaySuccess('Thêm mới không thành công');
            });
        }
        //function loadParentCategories() {
        //    apiService.get('api/productcategory/getallparents', null, function (result) {
        //        //  $scope.parentCategories = result.data;
                
        //    }, function () {
        //        console.log('cannot get list parent');
        //    });
        //}
        function loadParentCategories() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                //  $scope.parentCategories = result.data;
                console.log(result);
                $scope.parentCategories = commonService.getTree(result.data, "ID", "ParentID");// hiện thị parentID dạng tree
                $scope.parentCategories.forEach(function (item) {
                    recur(item,0, $scope.flatFolders);   // flatFolders 1 danh mục tổng để chứa parentID
                });
            }, function () {
                console.log('cannot get list parent');
                });
        }
        function times(n, str) {
            var result = '';
            for (var i = 0; i < n; i++) {
                result += str;
            }
            return result;
        };
        function recur(item, level, arr) {
            arr.push({
                Name: times(level, '–') + ' ' + item.Name,
                ID: item.ID,
                Level: level,
                Indent: times(level, '–')
            });
            if (item.children) {
                item.children.forEach(function (item) {
                    recur(item, level + 1, arr);
                });
            }
        };
        loadParentCategories();
       // loadProductCategory();

    }
})(angular.module('shop2.productCategories'));