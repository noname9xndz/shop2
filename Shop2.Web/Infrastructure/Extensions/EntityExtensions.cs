using Shop2.Model.Models;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop2.Web.Infrastructure.Extensions
{
    // tạo ra các phương thức mở rộng cho 1 đối tượng nào đó
    public static class EntityExtensions
    {
        // Khi chúng at using EntityExtensions này thì mặc định các đối tượng this  
         //sẽ có thêm phương thức mà at định nghĩa ở đây

        // bất cứ đối tượng tạo ở Model(Shop.Web) sử dụng phương thức thì giá trị 
        //của nó sẽ tự động đẩy sang Model(Shop.Model)
        public static void UpdatePostCategory(this PostCategory postCategory, PostCategoryViewModel postCategoryViewModel)
        {
            postCategory.ID = postCategoryViewModel.ID;
            postCategory.Name = postCategoryViewModel.Name;
            postCategory.Description = postCategoryViewModel.Description;
            postCategory.Alias = postCategoryViewModel.Alias;
            postCategory.ParentID = postCategoryViewModel.ParentID;
            postCategory.DisplayOrder = postCategoryViewModel.DisplayOrder;
            postCategory.Image = postCategoryViewModel.Image;
            postCategory.HomeFlag = postCategoryViewModel.HomeFlag;

            postCategory.CreatedDate = postCategoryViewModel.CreatedDate;
            postCategory.CreatedBy = postCategoryViewModel.CreatedBy;
            postCategory.UpdatedDate= postCategoryViewModel.UpdatedDate;
            postCategory.UpdatedBy = postCategoryViewModel.UpdatedBy;
            postCategory.MetaKeyword = postCategoryViewModel.MetaKeyword;
            postCategory.MetaDescription = postCategoryViewModel.MetaDescription;
            postCategory.Status = postCategoryViewModel.Status;

        }
        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryViewModel)
        {
            productCategory.ID = productCategoryViewModel.ID;
            productCategory.Name = productCategoryViewModel.Name;
            productCategory.Description = productCategoryViewModel.Description;
            productCategory.Alias = productCategoryViewModel.Alias;
            productCategory.ParentID = productCategoryViewModel.ParentID;
            productCategory.DisplayOrder = productCategoryViewModel.DisplayOrder;
            productCategory.Image = productCategoryViewModel.Image;
            productCategory.HomeFlag = productCategoryViewModel.HomeFlag;

            productCategory.CreatedDate = productCategoryViewModel.CreatedDate;
            productCategory.CreatedBy = productCategoryViewModel.CreatedBy;
            productCategory.UpdatedDate = productCategoryViewModel.UpdatedDate;
            productCategory.UpdatedBy = productCategoryViewModel.UpdatedBy;
            productCategory.MetaKeyword = productCategoryViewModel.MetaKeyword;
            productCategory.MetaDescription = productCategoryViewModel.MetaDescription;
            productCategory.Status = productCategoryViewModel.Status;

        }
        public static void UpdatePost(this Post post, PostViewModel postViewModel)
        {
            post.ID = postViewModel.ID;
            post.Name = postViewModel.Name;
            post.Description = postViewModel.Description;
            post.Alias = postViewModel.Alias;
            post.CategoryID = postViewModel.CategoryID;
            post.Content = postViewModel.Content;
            post.Image = postViewModel.Image;
            post.HomeFlag = postViewModel.HomeFlag;
            post.ViewCount = postViewModel.ViewCount;

            post.CreatedDate = postViewModel.CreatedDate;
            post.CreatedBy = postViewModel.CreatedBy;
            post.UpdatedDate = postViewModel.UpdatedDate;
            post.UpdatedBy = postViewModel.UpdatedBy;
            post.MetaKeyword = postViewModel.MetaKeyword;
            post.MetaDescription = postViewModel.MetaDescription;
            post.Status = postViewModel.Status;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productViewModel)
        {
            product.ID = productViewModel.ID;
            product.Name = productViewModel.Name;
            product.Description = productViewModel.Description;
            product.Alias = productViewModel.Alias;
            product.CategoryID = productViewModel.CategoryID;
            product.Content = productViewModel.Content;
            product.Image = productViewModel.Image;
            product.MoreImages = productViewModel.MoreImages;
            product.Price = productViewModel.Price;
            product.PromotionPrice = productViewModel.PromotionPrice;
            product.Warranty = productViewModel.Warranty;
            product.HomeFlag = productViewModel.HomeFlag;
            product.HotFlag = productViewModel.HotFlag;
            product.ViewCount = productViewModel.ViewCount;

            product.CreatedDate = productViewModel.CreatedDate;
            product.CreatedBy = productViewModel.CreatedBy;
            product.UpdatedDate = productViewModel.UpdatedDate;
            product.UpdatedBy = productViewModel.UpdatedBy;
            product.MetaKeyword = productViewModel.MetaKeyword;
            product.MetaDescription = productViewModel.MetaDescription;
            product.Status = productViewModel.Status;
            product.Tags = productViewModel.Tags;
            product.Quantity = productViewModel.Quantity;
         
        }
        public static void UpdatePage(this Page page, PageViewModel pageViewModel)
        {
            page.ID = pageViewModel.ID;
            page.Name = pageViewModel.Name;
            page.Alias = pageViewModel.Alias;
            page.Content = pageViewModel.Content;

            page.CreatedDate = pageViewModel.CreatedDate;
            page.CreatedBy = pageViewModel.CreatedBy;
            page.UpdatedDate = pageViewModel.UpdatedDate;
            page.UpdatedBy = pageViewModel.UpdatedBy;
            page.MetaKeyword = pageViewModel.MetaKeyword;
            page.MetaDescription = pageViewModel.MetaDescription;
            page.Status = pageViewModel.Status;

        }

        public static void UpdateContactDetail(this ContactDetail contactDetail, ContactDetailViewModel contactDetailViewModel)
        {
            contactDetail.ID = contactDetailViewModel.ID;
            contactDetail.Name = contactDetailViewModel.Name;
            contactDetail.Email = contactDetailViewModel.Email;
            contactDetail.Phone = contactDetailViewModel.Phone;
            contactDetail.Website = contactDetailViewModel.Website;
            contactDetail.Other = contactDetailViewModel.Other;
            contactDetail.Lat = contactDetailViewModel.Lat;
            contactDetail.Lng = contactDetailViewModel.Lng;
            contactDetail.Address = contactDetailViewModel.Address;
            

            contactDetail.CreatedDate = contactDetailViewModel.CreatedDate;
            contactDetail.CreatedBy = contactDetailViewModel.CreatedBy;
            contactDetail.UpdatedDate = contactDetailViewModel.UpdatedDate;
            contactDetail.UpdatedBy = contactDetailViewModel.UpdatedBy;
            contactDetail.MetaKeyword = contactDetailViewModel.MetaKeyword;
            contactDetail.MetaDescription = contactDetailViewModel.MetaDescription;
            contactDetail.Status = contactDetailViewModel.Status;

        }

        public static void UpdateFeedback(this Feedback feedback,FeedbackViewModel feedbackViewModel)
        {

            feedback.ID = feedbackViewModel.ID;
            feedback.Name = feedbackViewModel.Name;
            feedback.Message= feedbackViewModel.Message;
            feedback.Status= feedbackViewModel.Status;
            feedback.CreatedDate= DateTime.Now;
            feedback.Email= feedbackViewModel.Email;

        }

        //public static void UpdateOrder(this Order order, OrderViewModel orderViewModel)
        //{
        //    order.CustomerName = orderViewModel.CustomerName;
        //    order.CustomerAddress = orderViewModel.CustomerName;
        //    order.CustomerEmail = orderViewModel.CustomerName;
        //    order.CustomerMobile = orderViewModel.CustomerName;
        //    order.CustomerMessage = orderViewModel.CustomerName;
        //    order.PaymentMethod = orderViewModel.CustomerName;
        //    order.CreatedDate = DateTime.Now;
        //    order.CreatedBy = orderViewModel.CreatedBy;
        //    order.Status = orderViewModel.Status;
            
        //}

       
        
    }
}