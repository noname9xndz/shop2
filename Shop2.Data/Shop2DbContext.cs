using Microsoft.AspNet.Identity.EntityFramework;
using Shop2.Model.Models;
using System.Data.Entity;


/// <summary>
/// 3 cách sử dụng entity framework
/// Database first : Database => Model => Code (dùng khi có sẵn csdl ,EF Wizard sẽ tạo ra model và code cho ta)
/// Model first : Model => Code => Database (dùng khi ta bắt đầu thiết kế csdl từ đầu, ta sẽ thiết kế csdl(Model) EF sẽ tự tạo ra code cho ta, sau đó nhờ EF Wizard tạo CSDL)
/// Code First : Code => Database (nên dùng khi đã có mô hình CSDL, ta sẽ chỉ viết code từ đó tạo ra databaseb)
/// </summary>

namespace Shop2.Data
{

    // public class Shop2DbContext : DbContext
    // thay vì add DbConText chúng ta sẽ cho kế thừa từ IdentityDbContext<ApplicationUser> để tích hợp 
    // ASP.NET Identity (1 công nghệ giúp chúng ta trong bài toán xác thực và phân quyền người dùng trên website)
    public class Shop2DbContext : IdentityDbContext<ApplicationUser>
    {  // objectContext đại diện cho 1 database có chức năng quản lý các kết nối, định nghĩa mô hình dữ liệu với metadata và các thao tác với database
        
        //internal object ApplicationUser;

        public Shop2DbContext() : base("Shop2Connection")
        {
            // true thì khi select nó sẽ hoãn lại việc load ra các dữ liệu ở các enttiy liên quan cho đến khi bạn yêu cầu nó
            // false ngược lại : https://tedu.com.vn/lap-trinh-aspnet/tim-hieu-ve-lazyloading-va-earger-loading-trong-entity-framework-120.html
            this.Configuration.LazyLoadingEnabled = false;

            /*
             * 1 số chiến lược khởi tạo Db với code first
            Database.SetInitializer<Shop2DbContext>(new CreateDatabaseIfNotExists<Shop2DbContext>()); // mặc định tạo database nếu chưa tồn tại
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges());// xóa database hiện tại và tạo mới database nếu mô hình của bạn thay đổi
            Database.SetInitializer(new DropCreateDatabaseAlways());// xóa database và tạo mới database bất kể mô hình của bạn có thay đổi hay không
            Database.SetInitializer(new CustomDBInitializer());// 3 tùy chọn trên ko đáp ứng yc thì có thể tự viết database cho mình
              */



        }
        // ObjectSet<TEntity> là 1 tập hợp các entity mỗi đối tượng ứng với 1 table,có thể lấy các đối tượng này thông qua property tương ứng của objectContext
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<MenuGroup> MenuGroups { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }

        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }

        // phương thức để tạo mới Identity 
        public static Shop2DbContext Create()
        {
            return new Shop2DbContext();
        }

        // ghi đè phương thức có sẵn OnModelCreating(DbModelBuilder modelBuilder) của DbContext,sk chạy khi context chạy lần đầu tiên
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // câu hình key cho Identity,ToTable("Tên bảng") : đính nghĩa tên bảng trong sql thay vì dùng [Table("tên bảng")] trong mỗi class
            builder.Entity<IdentityUserRole>().HasKey(x => new { x.UserId, x.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(x => x.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");


        }
    }

}
