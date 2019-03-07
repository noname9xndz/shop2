

var common = {
    init: function () {
        common.registerEvents();
     
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.label + "</a>")
                .appendTo(ul);
            };
         $(".btnAddProductToCart").off('click').on('click', function (event) {
            event.preventDefault(); // xóa điều hướng mặc định
            var productId = parseInt($(this).data('id')); // lấy ra  data-id="@Model.ID"
             $.ajax(
                 {
                     url: '/ShoppingCart/AddProduct',
                     data: {
                         productId: productId
                     },
                     type: 'POST',
                     dataType: 'json',
                     success: function (res) {
                         if (res.status) {
                             alert('Thêm sản phẩm thành công.');
                         }
                         else {
                             alert(response.message);
                         }
                     }
                 });
        });
        
    }
}
common.init();