
//IUnitOfWork thiết kế 1 phương thức Commit 
namespace Shop2.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}