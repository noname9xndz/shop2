using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using Shop2.Service;
using System.Collections.Generic;

namespace Shop2.UnitTest.ServiceTest
{
    [TestClass]
    public class PostCategoryServiceTest
    {
        // khai báo các đối tượng bằng cách dùng mock để giả lập đối tượng
        private Mock<IPostCategoryRepository> _mockRepository;

        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPostCategoryService _categoryService;
        private List<PostCategory> _listCategory;

        [TestInitialize]
        public void Initialize()
        {
            // khởi tạo đối tượng giả lập
            _mockRepository = new Mock<IPostCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            
            _categoryService = new PostCategoryService(_mockRepository.Object, _mockUnitOfWork.Object);
            _listCategory = new List<PostCategory>()
            {
                new PostCategory(){ID=1,Name="danh mục 1",Status=true},
                new PostCategory(){ID=1,Name="danh mục 2",Status=true},
                new PostCategory(){ID=1,Name="danh mục 3",Status=true}
              };
        }


        [TestMethod]
        public void PostCategory_Service_GetAll()
        {
            //setup method
            _mockRepository.Setup(x => x.GetAll(null)).Returns(_listCategory);

            // call action

            var result = _categoryService.GetAll() as List<PostCategory>;

            // compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void PostCategory_Service_Create()
        {
            PostCategory category = new PostCategory();
            category.Name = "test";
            category.Alias = "test";
            category.Status = true;

            _mockRepository.Setup(x=>x.Add(category)).Returns(
                (PostCategory p)=>{
                                      p.ID = 1;
                                      return p;
                                    });
            var result =_categoryService.Add(category);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }
    }
}