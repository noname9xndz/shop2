﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Data.Infrastructure
{

    /*1 mảng IEnumerable có những thuộc tính
                -Là một mảng read-only, chỉ có thể đọc, không thể thêm hay bớt phần tử.
                -Chỉ duyệt theo một chiều, từ đầu tới cuối mảng
*/

    //RepositoryBase là 1 lớp abstract class  kế thừa IRepository và 
    //triển khai các phương thức trong IRepository(Thêm sửa xóa get,....)
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties
        private Shop2DbContext dataContext;
        private readonly IDbSet<T> dbSet; // đại điện cho lớp DbSet của bảng nhất định

        
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected Shop2DbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region 

        // thêm
        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }
        // update
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        // xóa 1 đối tượng trong entity
        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }
        // xóa bằng id
        public virtual T Delete(int id)
        {
            var entity = dbSet.Find(id);
            return dbSet.Remove(entity);
        }
        // xóa nhiều đối tượng
        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }
        // tìm kiếm  bằng id
        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }
        // lấy về nhiều đối tượng với điều kiện
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, string includes)
        {
            return dbSet.Where(where).ToList();
        }

        // tính tổng số bảng ghi hay đối tượng
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }
        // lấy về các đối tượng và lấy thêm được cả đối tượng được nó chứa
        // ví dụ select ra bài viết và có thể lấy ra được cả danh danh mục
        public IEnumerable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return dataContext.Set<T>().AsQueryable();
        }
        // lấy về 1 đối tượng với điều kiện 
        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return dataContext.Set<T>().FirstOrDefault(expression);
        }

        public virtual IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }
        // lấy về nhiều đối tượng sau đó phân trang
        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 20, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Count<T>(predicate) > 0;
        }
        #endregion
    }
}
