﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Shop2.Common;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Extensions;
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
            
            if(cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
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
                // lấy sp ra
                var product = _productService.GetById(productId);
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
        [HttpPost]
        public JsonResult DeleteAllProduct()
        {
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
        public JsonResult CreatOrder(string orderViewModel)
        {
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

            foreach(var item in cartSession)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                orderDetails.Add(detail);
            }
            _orderService.Create(orderNew,orderDetails);

            return Json(new
            {
                status = true
            });

        }
    }
}