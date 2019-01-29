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

        Product GetById(int id);

        
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

        public void Save()
        {
            _unitOfWork.Commit();
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
