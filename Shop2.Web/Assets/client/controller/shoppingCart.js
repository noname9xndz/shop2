

// viết js dạng hướng đói tượng
var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {

        // sk nút thêm giỏ hàng
        //$("#btnAddProductToCart").off('click').on('click', function (event) {
        //    event.preventDefault(); // xóa điều hướng mặc định
        //    var productId = parseInt($(this).data('id')); // lấy ra  data-id="@Model.ID"
        //    cart.addItem(productId);
        //});

        // sử dụng jquery-validation để bắt ngoại lệ nhập form thanh toán
        $("#frmPayment").validate({
            rules: {
                Name: "required",
                Adress: "required",
                Email: {
                    required: true,
                    email: true
                },
                Phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                Name: "Yêu cầu nhập tên",
                Adress: "Yêu cầu nhập địa chỉ",
                Email: {
                    required: "Yêu cầu nhập mail",
                    email: "Sai định dạng mail"
                },
                Phone: {
                    required: "yêu cầu nhập số điện thoại",
                    number: "số điện thoại phải là số"
                }
            }
        });

        // sk nút delete
        $(".btnDeleteItem").off('click').on('click', function (event) {
            event.preventDefault(); // xóa điều hướng mặc định
            var productId = parseInt($(this).data('id')); // lấy ra  data-id="@Model.ID"
            cart.deleteItem(productId);
        });

        // sk nhập số lượng sp
        $(".txtQuantity").off('keyup').on('keyup', function () {

            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false && quantity > 0 )
            {
                // nếu số lượng là số
                var amount = quantity * price;
                $("#amount_" + productid).text(numeral(amount).format('0,0'));
            }
            else {
                $("#amount_" + productid).text(0);
            }
            $("#lblTotalOrder").text(numeral(cart.getTotalOrder()).format('0,0')); //numeral để format giá

            cart.updateCart();
        });

        // sk nút tiếp tục mua hàng
        $("#btnContinue").off('click').on('click', function (event) {
            event.preventDefault();
            window.location.href = "/"; // điều hướng về trang chủ
        });

        // sk nút xóa giỏ hàng
        $("#btnDeleteAll").off('click').on('click', function (event) {
            event.preventDefault();
            cart.deleteAllItem();
        });

        // sk nút thanh toán
        $("#btnCheckout").off('click').on('click', function (event) {
            event.preventDefault();
            $("#divCheckout").show();
        });
       // sk nút sử dụng thông tin đăng nhập để mua hàng
        $("#checkUserLoginInfo").off('click').on('click', function () {
            if ($(this).prop('checked')) {
                cart.getLoginUser();
            }
            else {
                $("#txtName").val("");
                $("#txtAdress").val("");
                $("#txtEmail").val("");
                $("#txtPhone").val("");
                $("#txtMessage").val("");
            }
        });

        // sk nút thanh toán
        $(".btnCreatOrder").off('click').on('click', function (event) {
            event.preventDefault();
            var isValid = $("#frmPayment").valid(); // sử dụng jquery-validation để bắt ngoại lệ
            if (isValid) {
                cart.createOrder();
            }
            
        });
        

    },
    //addItem: function (productId) {
    //    $.ajax(
    //        {
    //            url: '/ShoppingCart/AddProduct',
    //            data: {
    //                productId: productId
    //            },
    //            type: 'POST',
    //            dataType: 'json',
    //            success: function (res) {
    //                if (res.status == true) {
    //                    alert('Thêm vào giỏ hàng thành công');

    //                }
    //            }
    //        })
    //},
    deleteItem: function (productId) {
        $.ajax(
            {
                url: '/ShoppingCart/DeleteItem',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        cart.loadData(); // xóa xong load lại data

                    }
                }
            })
    },
    deleteAllItem: function () {
        $.ajax(
            {
                url: '/ShoppingCart/DeleteAllProduct',
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        cart.loadData(); // xóa xong load lại data

                    }
                }
            })
    },
    getTotalOrder: function () {
        var listTextBox = $(".txtQuantity");
        var total = 0;
        
        $.each(listTextBox, function (i, item) {
            //var quantity = parseInt($(item).val());
            //var price = parseFloat($(item).data('price'));
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    getLoginUser: function () {
        $.ajax(
            {
                url: '/ShoppingCart/GetUserInfo',
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        var user = res.data;

                        $("#txtName").val(user.FullName);
                        $("#txtAdress").val(user.txtAdress);
                        $("#txtEmail").val(user.txtEmail);
                        $("#txtPhone").val(user.txtPhone);
                        $("#txtMessage").val(user.txtMessage);


                    }
                }
            })
    },
    updateCart: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {

            cartList.push({
                ProductId: $(item).data('id'),
                Quantity:  $(item).val()
            });

        });
        $.ajax(
            {
                url: '/ShoppingCart/UpdateProduct',
                data: {
                    cartData: JSON.stringify(cartList)
                },
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        cart.loadData();
                        console.log("update oke");

                    }
                }
            })
    },
    createOrder: function () {
        var order = {
            CustomerName : $("#txtName").val(),
            CustomerAddress : $("#txtAdress").val(),
            CustomerEmail :  $("#txtEmail").val(),
            CustomerMobile : $("#txtPhone").val(),
            CustomerMessage: $("#txtMessage").val(),
            PaymentMethod: "Thanh Toán Tiền Mặt",
            Status: false
        }
        $.ajax(
            {
                url: '/ShoppingCart/CreatOrder',
                data: {
                    orderViewModel: JSON.stringify(order)
                },
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {

                        $("#divCheckout").hide();
                        cart.deleteAllItem();
                        // ghi đè lên cartContent
                        setTimeout(function () {
                            $('#cartContent').html('Cảm ơn bạn đã đặt hàng . Chúng tôi sẽ liên hệ lại sớm nhất');
                        }, 1000);
                       
                    }
                }
            })
    },
    loadData: function () {
        // dùng Mustache thư viện giúp load html template
        $.ajax(
            {//status ,data lấy trong ShoppingCartController->GetAllProduct
                url: '/ShoppingCart/GetAllProduct',
                type: 'GET',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        var template = $('#tplShoppingCart').html();
                        //var template = Mustache.parse(template);
                       // Mustache.parse(template);
                        var html = '';
                        var data = res.data;
                        //console.log(data);
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                // lấy ra các đối tượng json tương ứng với ShoppingCartViewModel
                                ProductID: item.ProductId,
                                ProductName: item.Product.Name,
                                Image: item.Product.Image,
                                Price: item.Product.Price,
                                PriceFormat: numeral(item.Product.Price).format('0,0'), // tính toán vẫn dùng price hiển thị dùng priceformat
                                Quantity : item.Quantity,
                                Amount : numeral(item.Quantity * item.Product.Price).format('0,0')

                            });
                        });

                        // binding ra html bên trên
                        $('#shoppingCartBody').html(html);

                        if (html == '') {
                            $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng.');
                        }
                        // load lại các sự kiện click
                        $("#lblTotalOrder").text(numeral(cart.getTotalOrder()).format('0,0')); //numeral(thư viện để format số) để format giá
                        cart.registerEvent();
                    }
                }
            })
    }
}
cart.init();