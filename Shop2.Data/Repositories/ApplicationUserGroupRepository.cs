using Shop2.Data.Infrastructure;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Data.Repositories
{
    public interface IApplicationUserGroupRepository:IRepository<ApplicationUserGroup>
    {
       
    }
    public class ApplicationUserGroupRepository:RepositoryBase<ApplicationUserGroup>, IApplicationUserGroupRepository
    {
        public ApplicationUserGroupRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
       
    }
}
