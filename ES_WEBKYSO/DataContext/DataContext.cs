using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace ES_WEBKYSO.DataContext
{
    // Lớp chứa cấu hình EntityFramework
    public class DataContext : DbContext
    {
        // Chỉ định connection string cần làm việc, nằm trong Web.config
        public DataContext() : base("Name=DefaultConnection")
        {
        }

        // Cấu hình bước đệm cho context trước khi setup model cho context
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Không khởi tạo lại CSDL
            Database.SetInitializer(new NullDatabaseInitializer<DataContext>());
            // Loại bỏ quy ước đặt tên bảng và tự đặt tên bảng qua attribute
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // 
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // Setup model cho context
            ConfigureModel(modelBuilder);
            // Cho phép Lazy load, gọi các bảng liên kết từ bảng khác
            Configuration.LazyLoadingEnabled = true;
            base.OnModelCreating(modelBuilder);
        }

        // Cấu hình các entity model sử dụng trong context
        private void ConfigureModel(DbModelBuilder modelBuilder)
        {
            // Lấy hàm build các entity model
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");
            // Lấy các đối tượng có Namespace = Entities.Models để đưa vào context
            var entityTypes = Assembly.GetAssembly(typeof(IEntity))
                .GetTypes()
                .Where(
                x =>
                x.Namespace != null &&
                (x.Namespace == "ES_WEBKYSO.Models" || x.Namespace.Contains("ES_WEBKYSO.Models.")) &&
                !x.IsAbstract)
                .ToList();
            // Lặp qua các Entity class tìm được
            foreach (var type in entityTypes)
            {
                // Chạy hàm setup context với các Entity class tìn được
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
            }
        }
    }
}