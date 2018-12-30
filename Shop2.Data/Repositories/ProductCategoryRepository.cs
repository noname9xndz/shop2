using System.Collections.Generic;
using System.Linq;
using Shop2.Data.Infrastructure;
using Shop2.Model.Models;

namespace Shop2.Data.Repositories
{
    //tạo 1 interface chứa các phương thức mà không nằm trong RepositoryBase mà chúng ta cần định nghĩa lại
    //nếu không cần có thì thôi
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        // khai báo phương thức cần thêm
        IEnumerable<ProductCategory> GetByAlias(string alias);
    }
    //khởi tạo class chứa các phương thức trong bảng ProductCategory kế thừa RepositoryBase và lớp interface trên
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        //khởi tạo Contructor kế thừa IDbFactory
        //úc khởi tạo  Repository  của Product ta sẽ truyền vào DbFactory , đồng thời nó sẽ
        //lấy giá trị đó truyền vào Contructor của RepositoryBase để khởi tạo RepositoryBase 
        //=> ta có thể sử dụngđược các phương thức của RepositoryBase trên ProductCategoryRepository
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
        // định nghĩa lại phương thức cần thêm này
        public IEnumerable<ProductCategory> GetByAlias(string alias)
        {
            return this.DbContext.ProductCategories.Where(x => x.Alias == alias);
        }
    }
}