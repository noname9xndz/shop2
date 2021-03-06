﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Shop2.Common;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Extensions;
using Shop2.Web.Infrastructure.NganLuongAPI;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Shop2.Web.App_Start.IdentityConfig;

namespace Shop2.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        // tiêm đối tượng vào controller
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;
        // dùng để thanh toán ngân lượng
        private string merchantId = ConfigHelper.GetByKey("MerchantId");
        private string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");

        public ShoppingCartController(IProductService productService, IOrderService orderService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
        }
    

        public ActionResult Index()
        {
            // khởi tạo giá trị cho session
            if(Session[CommonConstants.SessionCart]==null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }
          
            return View();
        }
        //chúng ta xử lý bằng ajax nên trả về json
        // thêm sp vào giỏ hàng 
        [HttpPost]
        public JsonResult AddProduct(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if(product.Quantity==0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản Phẩm không đủ hàng"
                });
            }
            if(product.Price <= 0 || product.PromotionPrice <= 0 || product.PromotionPrice > product.Price)
            {
                return Json(new
                {
                    status = false,
                    message = "Liên Hệ Người Bán Hàng Để Mua"
                });
            }
            //kiểm tra thông tin sp
            if (cart.Any(x => x.ProductId == productId))
            {// có tăng số lượng
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {//không add 1 sp mới vào giỏ hàng
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            // trả về  session và chuỗi json
            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
            
        }

        // lấy toàn bộ sp trong giỏ hàng
        public JsonResult GetAllProduct()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            },JsonRequestBehavior.AllowGet);  
        }

        //update giỏ hàng
        [HttpPost]
        public JsonResult UpdateProduct(string cartData)
        {
            // thư viện JavaScriptSerializer giúp chuyển đối một đối tượng .NET (hay CLR) bất kì thành một chuỗi JSON và ngược lại. 
            //ở đây là lấy về list sản phẩm
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            
            // update cart nếu có sự thành đổi của người dùng
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {// cập nhật số lượng
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity += jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });

        }
       // xóa 1 sp trong giỏ hàng
        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        // xóa tất cả sp trong giỏ hàng
        [HttpPost]
        public JsonResult DeleteAllProduct()
        {
            //gán ngược session lại bằng cách tạo mới
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }
        public JsonResult GetUserInfo()
        {
            if(Request.IsAuthenticated) // đã đăng nhập
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
           
        }
        // tạo order khi click thanh toán
        public ActionResult CreatOrder(string orderViewModel)
        {
            // order(chứa thông tin người mua hàng) 
            // orderDetail(chứ số lượng sp,sp) 
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            

            var orderNew = new Order();
            orderNew.UpdateOrder(order);

            if(Request.IsAuthenticated)
            {
                orderNew.CustomerID = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>(); 
            bool isEnough = true;

            foreach(var item in cartSession)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                orderDetails.Add(detail);
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity); // tính số sp còn lại
                break;
            }
            if(isEnough==true)
            {
              //_orderService.Create(orderNew, orderDetails);
                var orderReturn = _orderService.Create(ref orderNew, orderDetails);
                _productService.Save();

                // thanh toán tiền mặt
                if (order.PaymentMethod == "CASH")
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {  // thanh toán bằng ATM,Ngân Lượng
                    var currentLink = ConfigHelper.GetByKey("CurrentLink");

                    RequestInfo info = new RequestInfo();
                    info.Merchant_id = merchantId;
                    info.Merchant_password = merchantPassword;
                    info.Receiver_email = merchantEmail;



                    info.cur_code = "vnd";
                    info.bank_code = order.BankCode;

                    info.Order_code = orderReturn.ID.ToString();
                    info.Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString();
                    info.fee_shipping = "0";
                    info.Discount_amount = "0";
                    info.order_description = "Thanh toán đơn hàng tại NonameShop";
                    info.return_url = currentLink + "xac-nhan-don-hang.html";
                    info.cancel_url = currentLink + "huy-don-hang.html";

                    info.Buyer_fullname = order.CustomerName;
                    info.Buyer_email = order.CustomerEmail;
                    info.Buyer_mobile = order.CustomerMobile;

                    APICheckoutV3 objNLChecout = new APICheckoutV3();
                    ResponseInfo result = objNLChecout.GetUrlCheckout(info, order.PaymentMethod);
                    if (result.Error_code == "00")
                    {
                        return Json(new
                        {
                            status = true,
                            urlCheckout = result.Checkout_url,// chuyển hướng qua ngân lượng
                            message = result.Description
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            message = result.Description
                        });
                    }
                }
                  
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng"
                });
            }
            
        }
        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];

            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Token = token;

            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);

            if (result.errorCode == "00")
            {
                //update status order
                _orderService.UpdateStatus(int.Parse(result.order_code));
                _orderService.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            return View();
        }
        public ActionResult CancelOrder()
        {
            return View();
        }
    }
}