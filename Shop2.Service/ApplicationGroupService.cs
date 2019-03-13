using Shop2.Common.Exceptions;
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
    public interface IApplicationGroupService
    { 
        // lấy chi tiết user
        ApplicationGroup GetDetail(int id);
        // lấy tất cả với bộ lọc
        IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter);
        // lấy tất cả 
        IEnumerable<ApplicationGroup> GetAll();
        // thêm mới
        ApplicationGroup Add(ApplicationGroup appGroup);
        // cập nhật
        void Update(ApplicationGroup appGroup);
        // xóa
        ApplicationGroup Delete(int id);
        // add user vào group
        bool AddUserToGroups(IEnumerable<ApplicationUserGroup> groups, string userId);
        // lấy list group bằng id của user
        IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId);
        // lấy list user bằng id của group
        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId);

        void Save();
    }
    public class ApplicationGroupService : IApplicationGroupService
    {
        private IApplicationGroupRepository _appGroupRepository;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserGroupRepository _appUserGroupRepository;

        public ApplicationGroupService(IUnitOfWork unitOfWork,
            IApplicationUserGroupRepository appUserGroupRepository,
            IApplicationGroupRepository appGroupRepository)
        {
            this._appGroupRepository = appGroupRepository;
            this._appUserGroupRepository = appUserGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public ApplicationGroup Add(ApplicationGroup appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appGroupRepository.Add(appGroup);
        }

        public ApplicationGroup Delete(int id)
        {
            var appGroup = this._appGroupRepository.GetSingleById(id);
            return _appGroupRepository.Delete(appGroup);
        }

        public IEnumerable<ApplicationGroup> GetAll()
        {
            return _appGroupRepository.GetAll();
        }

        public IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appGroupRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public ApplicationGroup GetDetail(int id)
        {
            return _appGroupRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ApplicationGroup appGroup)
        {
            // kiểm tra tên nhưng id phải khác id truyền vào
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name && x.ID != appGroup.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _appGroupRepository.Update(appGroup);
        }

        public bool AddUserToGroups(IEnumerable<ApplicationUserGroup> userGroups, string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                _appUserGroupRepository.Add(userGroup);
            }
            return true;
        }
        // 2 phương thức này liên quan trực tiếp đến 2 bảng nên ta viết linq bổ sung trong repository
        public IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId)
        {
            
            return _appGroupRepository.GetListGroupByUserId(userId);
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId)
        {
            return _appGroupRepository.GetListUserByGroupId(groupId);
        }
    }
}
