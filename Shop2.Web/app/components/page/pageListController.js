

// khai báo controller
(function (app) {
    app.controller('pageListController', pageListController);
    // truyền đối tượng apiService  để get dữ liệu từ webapi
    pageListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function pageListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        // lấy tạo mảng pages
        $scope.pages = [];
        // phân trang
        $scope.page = 0;
        $scope.pageCount = 0;

        // lấy dữ liệu từ webapi
        $scope.getpages = getpages;

        $scope.keyword = '';
        $scope.search = search;

        $scope.deletePage = deletePage;

        $scope.selectAll = selectAll;

        $scope.deleteMulti = deleteMulti;
        // xóa nhiều 
        function deleteMulti() {
            var listId = [];
            // gọi lại selected bên dưới,lấy ra id trong listProductCategory trong api
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                // đưa list id vào
                params: {
                    // biến đổi kiểu json trả về thành string
                    checkedPages: JSON.stringify(listId)
                }
            }
            apiService.del('api/page/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                // gọi lại hàm hiển thị
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }
        // sự kiện cho nút select All
        // kiểm tra sự kiện trên selectAll, sự kiện này được binding vào  ng-model="item.checked"
        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.pages, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.pages, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }
        // sự kiện cho nút xóa
        // dùng watch + filter lắng nghe sự thay đổi(check) của list này 2 tham số : tên của list,hàm call back(giá trị mới , giá trị cũ)
        $scope.$watch("pages", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);


        function deletePage(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/page/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }



        function search() {
            getpages();
        }

        function getpages(page) {
            page = page || 0;
            var cofig = {
                //params 1 định dạng để truyền params vào uri
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }

            apiService.get('/api/page/getall', cofig, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('không có bản ghi nào được tìm thấy');
                }
                //else {
                //    notificationService.displaySuccess('tìm thấy ' + result.data.TotalCount + ' bản ghi');
                //}
                // lấy về data 
                $scope.pages = result.data.Items;
                // lấy về giá trị dùng để phân trang
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;

            }, function () {
                console.log('load productcategory failed');
            });
        }
        $scope.getpages();

    }
})(angular.module('shop2.pages')); // chỉ ra controller này thuộc module nào