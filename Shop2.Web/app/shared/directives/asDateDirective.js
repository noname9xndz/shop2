(function (app) {
    'use strict';

    app.directive('asDate', asDate);

    function asDate() {
        return {
            require: '^ngModel', // phải đặt trong phần tử ngModel
            restrict: 'A',  // chỉ để làm thuộc tính(attribute) không làm element

            // thao tác format date -> datetime
            link: function (scope, element, attrs, ctrl) {
                ctrl.$formatters.splice(0, ctrl.$formatters.length);
                ctrl.$parsers.splice(0, ctrl.$parsers.length);
                ctrl.$formatters.push(function (modelValue) {
                    if (!modelValue) {
                        return;
                    }
                    return new Date(modelValue);
                });
                ctrl.$parsers.push(function (modelValue) {
                    return modelValue;
                });
            }
        };
    }
})(angular.module('shop2.common'));