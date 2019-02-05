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
    public interface IPageService
    {
        Page Add(Page Page);

        void Update(Page Page);

        Page Delete(int id);

        IEnumerable<Page> GetAll();

        IEnumerable<Page> GetAllByKeyWord(string keyword);

        Page GetById(int id);

        Page GetPageByAlias(string Alias);

        void Save();
    }

    public class PageService : IPageService
    {
        private IPageRepository _pageRepository;
        private IUnitOfWork _unitOfWork;

        public PageService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
        }

        public Page Add(Page Page)
        {
            return _pageRepository.Add(Page);
        }

        public Page Delete(int id)
        {
            return _pageRepository.Delete(id);
        }

        public IEnumerable<Page> GetAll()
        {
            return _pageRepository.GetAll();
        }

        public IEnumerable<Page> GetAllByKeyWord(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _pageRepository.GetMulti(x => x.Name.Contains(keyword));
            }
            else
            {
                return _pageRepository.GetAll();
            }
        }

        public Page GetById(int id)
        {
            return _pageRepository.GetSingleById(id);
        }

        public Page GetPageByAlias(string Alias)
        {
            return _pageRepository.GetSingleByCondition(x => x.Alias == Alias);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Page Page)
        {
            _pageRepository.Update(Page);
        }
    }
}
