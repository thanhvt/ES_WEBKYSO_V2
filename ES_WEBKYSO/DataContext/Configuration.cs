using System.Data.Entity.Migrations;

namespace ES_WEBKYSO.DataContext
{
    class Configuration : DbMigrationsConfiguration<DataContext>
    {
        // Hàm này để cấu hình EntityFramework Code First
        public Configuration()
        {
            // Cho phép framework tự động chỉnh sửa database nếu có thay đổi từ Entity = không
            AutomaticMigrationsEnabled = false;
            // Khi tự động chỉnh sửa database, cho phép chỉnh sửa ngay cả khi mất dữ liệu = không
            // Trường hợp này thường xảy ra với CÁC trường hợp như 1 cột trong bảng bị đổi tên
            // Framework sẽ tẩy đi dữ liệu trên cột đó để xóa cột rồi tạo cột mới với tên mới và KHÔNG có dữ liệu
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}