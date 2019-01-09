using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;

using System.Linq;
namespace Shop2.Service
{
    // tạo 1 interface chứa các phương thức sử dụng cho bảng Post
    // bất cứ tầng là 1 đối tượng ta đều cho đi qua 1 interface
    public interface IPostService
    {
        /*1 mảng IEnumerable có những thuộc tính
                -Là một mảng read-only, chỉ có thể đọc, không thể thêm hay bớt phần tử.
                -Chỉ duyệt theo một chiều, từ đầu tới cuối mảng
*/
        void Add(Post post);

        void Update(Post post);

        void Delete(int id);

        IEnumerable<Post> GetAll(); // lấy về tất cả bài viết

        // lấy về tất cả bài viết và phân trang
        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);

        // lấy bài viết bằng danh mục và phân trang
        IEnumerable<Post> GetAllByCategoryPaging(int categoryID, int page, int pageSize, out int totalRow);

        Post GetById(int id); // lấy ra 1 ghi(tìm kiếm)

        //lấy về tất cả bài viết bằng tag sau đó phân trang
        IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow);

        void SaveChanges();
    }
    // class chứa các phương thức cho postservice và kế thừa interface trên
    public class PostService : IPostService
    {
        // khai báo biến thông qua interface của nó mà không thông qua biến 
        IPostRepository _postRepository; // chứa các phương thức mà chúng ta định nghĩa trong tầng data
        IUnitOfWork _unitOfWork;  // giúp chúng ta commit vào database
        
        // khai báo contructor truyền vào 2 biến bên trên (lưu cách gián tiếp các đối tượng)
        public PostService(IPostRepository postRepository,IUnitOfWork unitOfWork)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
        }

        // triển khai các phương thức có trên interface
        public void Add(Post post)
        {
            _postRepository.Add(post);
        }

        public void Delete(int id)
        {
            _postRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            // lấy về các đối tượng và lấy thêm được cả đối tượng được nó chứa
            // ví dụ select ra bài viết và có thể lấy ra được cả danh danh mục
            return _postRepository.GetAll(new string[] {"PostCategory" });
        }

        public IEnumerable<Post> GetAllByCategoryPaging(int categoryID, int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x => x.Status && x.CategoryID == categoryID, out totalRow,page, 
                                            pageSize,new string[] { "PostCategory" });
        }

        // lấy về nhiều đối tượng bằng tag sau đó phân trang
        public IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow)
        {
            // lấy tất cả bài viết bằng tag được định nghĩa ở IPostRepository
            return _postRepository.GetAllByTag(tag,page,pageSize, out totalRow);
            
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            // lấy về nhiều đối tượng sau đó phân trang
            return _postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Post GetById(int id)
        {// lấy ra 1 ghi(tìm kiếm)
            return _postRepository.GetSingleById(id);
        }
        // commit để select dữ liệu vào db
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
        // Update để select dữ liệu vào db
        public void Update(Post post)
        {
            _postRepository.Update(post);
        }
    }
}