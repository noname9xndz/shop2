




(function () {
    //cấu hình router
    angular.module('shop2.pages', ['shop2.common']).config(config);
    // dependency 2 service được định nghĩa sẵn trong ui-route $stateProvider,$urlRouterProvider
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('pages', {
                url: "/pages",
                parent: 'base',
                templateUrl: "/app/components/page/pageListView.html",
                controller: "pageListController"
            })
            .state('pageAdd', {
                url: "/pageAdd",
                parent: 'base',
                templateUrl: "/app/components/page/pageAddView.html",
                controller: "pageAddController"
            })
            .state('pageEdit', {
                url: "/pageEdit/:id",
                parent: 'base',
                templateUrl: "/app/components/page/pageEditView.html",
                controller: "pageEditController"
            });;

    }
})();// chỉ ra module này thuộc tp nào không có bỏ trống