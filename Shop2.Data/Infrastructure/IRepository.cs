using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shop2.Data.Infrastructure
{
    //IRepository class chứa các phương thức thao tác với csdl , các phương thức này sẽ được 
    // triển khai trong RepositoryBase
    //có thể sử dụng trong tất cả các lớp trong dự án
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        T Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        T Delete(T entity);

        T Delete(int id);

        // xóa nhiều đối tượng
        void DeleteMulti(Expression<Func<T, bool>> where);

        // tìm kiếm  bằng id
        T GetSingleById(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        // lấy về các đối tượng và lấy thêm được cả đối tượng được nó chứa
        // ví dụ select ra bài viết và có thể lấy ra được cả danh danh mục
        IEnumerable<T> GetAll(string[] includes = null);

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
    }
}