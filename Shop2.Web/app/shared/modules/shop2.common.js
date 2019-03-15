
/*
Khi tạo ra 1 SPA, bộ định tuyến(Route) là cực kỳ quan trọng, bạn muốn trang của mình chỉ cẩn load những dữ liệu tại 1 view 
và chỉ thực hiệncontroller trong view đó mà không cần tải lại cả trang web

--UI - router là 1 “routing framework”  khác biệt hơn so với ngRoute của Angular.
  UI - router tổ chức dữ liệu thành từng phần theo từng $state(đặc biệt xây dựng 1 multi views) theo từng 
   truy vấn URL.UI - router cho phép bạn xử lý nhiều thao tác với route 1 cách cực kỳ dễ dàng,
   và đôi khi nó không quan tâm đến URL của bạn mà bạn vẫn có thể thực hiện xử lý ở các view con.
*/

// dependency  ui.router vào common, bất cứ function nào truyền vào common có thể dùng ui router
(function () {
    //angular.module('shop2.common', ['ui.router'])
    angular.module('shop2.common', ['ui.router', 'ngBootbox', 'ngCkeditor', 'checklist-model','chart.js'])
})(); // chỉ ra module này thuộc tp nào không có bỏ trống