

// service thông báo dùng chung cho project (toastr  là 1 thư viện của js để tạo ra các thông báo cho người dùng)
(function (app) {
    app.factory('notificationService', notificationService);

    function notificationService() {
        toastr.options = {
            "debug": false,
            // chỗ hiển thị thông báo
            "positionClass": "toast-top-right",
            "onclick": null, 
            "fadeIn": 300, // tắt 0.3s
            "fadeOut": 1000, // hiên 1s
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        function displaySuccess(message) {
            toastr.success(message);
        }

        function displayError(error) {

            if (Array.isArray(error)) {
                error.each(function (err) {
                    toastr.error(err);
                });
            }
            else {
                toastr.error(error);
            }
        }

        function displayWarning(message) {
            toastr.warning(message);
        }
        function displayInfo(message) {
            toastr.info(message);
        }

        // trả phương thức các thông báo 
        return {
            //tên public : tên nội bộ
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        }
     

    }
})(angular.module('shop2.common'));