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
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlides();
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerResponsitory;
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;



        public CommonService(IFooterRepository footerResponsitory,IUnitOfWork unitOfWork,ISlideRepository slideRepository)
        {
            _footerResponsitory = footerResponsitory;
            _unitOfWork = unitOfWork;
            _slideRepository = slideRepository;
        }
        public Footer GetFooter()
        {
            return _footerResponsitory.GetSingleByCondition(x=>x.ID==CommonConstants.DefaultFooterID);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status);
            //return _slideRepository.GetMulti(x => x.Status == true);
        }
    }
}
