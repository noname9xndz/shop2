using Shop2.Common;
using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Service
{
    public interface IProductService
    {
        // với dữ liệu lớn thì nên sử dung IQueryable để giảm  việc query vào database
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        // tìm kiếm theo điều kiện gì đó
        IEnumerable<Product> GetAllByKeyWord(string keyword);

        // sp mới nhât
        IEnumerable<Product> GetLastest(int top);
        // sp bán chạy
        IEnumerable<Product> GetHotProduct(int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,out int totalRow, string sort);

        IEnumerable<string> GetListProductByName(string name);

        IEnumerable<Product> SearchProductByName(string keyword, int page, int pageSize, out int totalRow, string sort);
        // sp liên quan
        IEnumerable<Product> GetReatedProducts(int id, int top);

        IEnumerable<Tag> GetListTagByProductId(int id);

        // tăng view khi xem chi tiết sp
        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByTag(string tagID, int page,int pageSize, out int totalRow, string sort);

        //Tag GetTag(string tagID);
        Tag GetTag(string tagID);

        Product GetById(int id);

        bool SellProduct(int productId, int quantity); // trừ sp khi đặt hàng thành công

        

        void Save();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _ProductRepository;
        private IUnitOfWork _unitOfWork;


        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
      

        public ProductService(IProductRepository ProductRepository, IUnitOfWork unitOfWork,
                             IProductTagRepository productTagRepository,ITagRepository tagRepository)
        {
            this._ProductRepository = ProductRepository;
            this._unitOfWork = unitOfWork;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
        }

        public Product Add(Product Product)
        {
            //thêm mới 1 sản phẩm
            var product = _ProductRepository.Add(Product);
            _unitOfWork.Commit();
            // lấy trường Tags bên sản phẩm đẩy quá bảng Tags
            if (!string.IsNullOrEmpty(Product.Tags)) 
            {
                
                string[] tags = Product.Tags.Split(',');

                for(var i=0;i<tags.Length;i++)
                {
                    // chuyển đổi chuỗi ký tự bằng  static method  trong common
                    var tagID = StringHelper.ToUnsignString(tags[i]);
                    if(_tagRepository.Count(x=>x.ID==tagID)==0)
                    {
                        // tạo ra 1 đối tượng tag mới add vô bảng Tags
                        Tag tag = new Tag
                        {
                            ID = tagID,
                            Name = tags[i],
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    // add tag vừa tạo add vào bảng productTags
                    ProductTag productTag = new ProductTag
                    {
                        ProductID = Product.ID,
                        TagID = tagID
                    };
                    _productTagRepository.Add(productTag);
                }
              
            }
            return product;
        }

       

        public Product Delete(int id)
        {
            return _ProductRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _ProductRepository.GetAll();
        }
        // tìm kiếm theo điều kiện người dùng nhập vào
        public IEnumerable<Product> GetAllByKeyWord(string keyword)
        {
            if(!string.IsNullOrEmpty(keyword))
            { 
              return _ProductRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }
            else
            {
                return _ProductRepository.GetAll();
            }
        }


        public Product GetById(int id)
        {
            return _ProductRepository.GetSingleById(id);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status==true && x.HotFlag==true).OrderByDescending(x => x.CreatedDate).Take(top);
        }
        
        public IEnumerable<Product> GetLastest(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status==true).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,out int totalRow, string sort)
        {
            var query =  _ProductRepository.GetMulti(x => x.Status==true && x.CategoryID == categoryId);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _ProductRepository.GetMulti(x => x.Status == true && x.Name.Contains(name)).Select(y=>y.Name);
        }

        public IEnumerable<Product> GetListProductByTag(string tagID,int page,int pageSize,out int totalRow, string sort)
        {
           // get mutil không hỗ trợ phương thức
           // trong trường hợp respository không hỗ hỗ thì quay lại viết linq thuần 
           return _ProductRepository.GetListProductByTag(tagID, page, pageSize, out totalRow, sort);
            
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y=>y.Tag);
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = _ProductRepository.GetSingleById(id);
            return _ProductRepository.GetMulti(x => x.Status == true && x.ID != id && x.CategoryID == product.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);

        }

        public Tag GetTag(string tagID)
        {
            return _tagRepository.GetSingleByCondition(x=>x.ID==tagID);
        }

        public void IncreaseView(int id)
        { // trên thực tế chúng ta phải tính view theo ip 
           var product = _ProductRepository.GetSingleById(id);
            if(product.ViewCount.HasValue) //.HasValue có giá trị nào đó ko??
            {
                product.ViewCount += 1;
            }
            else
            {
                product.ViewCount = 1;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        } 

        public IEnumerable<Product> SearchProductByName(string keyword, int page, int pageSize, out int totalRow, string sort)
        {
            var query = _ProductRepository.GetMulti(x => x.Status == true && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public bool SellProduct(int productId, int quantity)
        {
            // nếu sp ddc đặt thành công sẽ trừ đi số lượng
            var product = _ProductRepository.GetSingleById(productId);
            if (product.Quantity < quantity)
                return false;
            product.Quantity -= quantity;
            return true;
        }

        public void Update(Product Product)
        {
            // tương tự như add
             _ProductRepository.Update(Product) ;
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagID = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagID) == 0)
                    {
                        Tag tag = new Tag
                        {
                            ID = tagID,
                            Name = tags[i],
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    // xóa tag cũ sau đó add lại tag mới vứ tạo thành tag mới
                    _productTagRepository.DeleteMulti(x => x.ProductID == Product.ID);

                    ProductTag productTag = new ProductTag
                    {
                        ProductID = Product.ID,
                        TagID = tagID
                    };
                    _productTagRepository.Add(productTag);
                }
               
            }
            
        }
    }

}
