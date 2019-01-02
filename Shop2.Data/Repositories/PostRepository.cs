using System;
using System.Collections;
using System.Collections.Generic;
using Shop2.Data.Infrastructure;
using Shop2.Model.Models;
using System.Linq;

namespace Shop2.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        // lấy về bài viết theo tag ()
        IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
        
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // join giữa bảng tag và posttag

        public IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags
                        on p.ID equals pt.PostID
                        where pt.TagID == tag && p.Status
                        orderby p.CreatedDate descending
                        select p;

            totalRow = query.Count();// trả về số lượng bản ghi
            // phân trang
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            return query;
        }
    }
}