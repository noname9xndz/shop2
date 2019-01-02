using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.UnitTest.RepositoryTest
{
    
    [TestClass]
    public class PostCategoryRepositoryTest
    {
        // khai báo đối tượng cần test
        IDbFactory dbFactory;
        IPostCategoryRepository objRepostitory;
        IUnitOfWork unitOfWork;

        // viết phương thức thiết lập khởi tạo các đối tượng test
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            objRepostitory = new PostCategoryRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
        }
        // viết phương thức test 


        [TestMethod]
        // lấy về tất cả postcategory
        public void PostCategory_Repository_GetAll()
        {
            var list = objRepostitory.GetAll().ToList();
            Assert.AreEqual(3, list.Count);
        }

        // test tạo mới Postcategory
        [TestMethod]
        public void PostCategory_Repository_Create()
        {
            PostCategory category = new PostCategory();
            category.Name = "Test category";
            category.Alias = "Test-category";
            category.Status = true;

            var result = objRepostitory.Add(category);
            unitOfWork.Commit();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.ID);
            
        }

    }

}
