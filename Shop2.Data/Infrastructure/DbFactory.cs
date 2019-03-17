namespace Shop2.Data.Infrastructure
{

    //DbFactory được kế thừa từ IDbFactory  và class DIsposable để khởi tạo 
    //các đối tượng DbContext, thay vi lưu trực tiếp chúng ta sẽ thông qua 
    //lớp DbFactory này để khởi tạo các đối tượng
    public class DbFactory : Disposable, IDbFactory
    {
                private Shop2DbContext dbContext;

                public Shop2DbContext Init()
                {
                    return dbContext ?? (dbContext = new Shop2DbContext());
                }

                protected override void DisposeCore()
                {
                    if (dbContext != null)
                        dbContext.Dispose();
                }
    }
}