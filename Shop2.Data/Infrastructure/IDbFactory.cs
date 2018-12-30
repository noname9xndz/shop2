using System;

namespace Shop2.Data.Infrastructure
{
    //IDbFactory  là class giao tiếp khởi tạo gián tiếp các đối tượng entity 
    //thông qua factory, kế thừa lớp IDisposable
    public interface IDbFactory : IDisposable
    {
        Shop2DbContext Init();
    }
}