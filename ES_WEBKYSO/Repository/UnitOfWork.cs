using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository.ServiceRepository;

namespace ES_WEBKYSO.Repository
{

    /// <summary>
    /// chưa sử dụng chuẩn Unit Of Work, sau khi thực hiện các hàm insert, update, delete sẽ cập nhật database
    /// không sử dụng savechange cho mỗi transactions
    /// </summary>
    public sealed class UnitOfWork : IDisposable
    {
        private DbContext _context;
        private TransactionScope _transaction;

        public UnitOfWork(DbContext dbContext)
        {
            _context = dbContext;
        }

        public BaseRepository<T> RepoBase<T>() where T : class
        {
            var nameOfEntity = typeof(T).Name;
            ConstructorInfo constructorMethod = null;
            nameOfEntity = StringHelpers.FirstLetterUperString(nameOfEntity);
            nameOfEntity += "Repository";
            
            var serviceRepo = Assembly.GetAssembly(typeof(BaseRepository<>))
                .GetTypes()
                .FirstOrDefault(
                    x =>
                        x.Namespace != null &&
                        (x.Namespace == "ES_WEBKYSO.Repository.ServiceRepository" ||
                         x.Namespace.Contains("ES_WEBKYSO.Repository.ServiceRepository.")) &&
                        !x.IsAbstract && x.Name == nameOfEntity + "`1");
            // Lấy ra type service repo cho từng entity nếu có
            // Nếu không có service riêng thì dùng repo chung đóng tại BaseRepository
            //var serviceRepo = Type.GetType("ES_BIDDING_GENCO3.Repository.ServiceRepository." + nameOfEntity + "`1");
            if (serviceRepo == null)
            {
                serviceRepo = Type.GetType("ES_WEBKYSO.Repository.ServiceRepository.BaseRepositorys`1");
                if (serviceRepo == null)
                {
                    throw new Exception(
                        "Kiểm tra lại các service trong đường dẫn /Repository/ServiceRepository. Đảm bảo sự tồn tại của BaseRepositorys !");
                }
                nameOfEntity = "BaseRepositorys";
            }
            var makeMeNow = serviceRepo.MakeGenericType(typeof(T));
            constructorMethod = makeMeNow.GetConstructor(new[] { typeof(DbContext), typeof(UnitOfWork) });
            if (constructorMethod == null)
            {
                throw new Exception("Kiểm tra lại sự đúng đắn của service: " + nameOfEntity + "  xem nào. Không thấy hàm khởi tạo (constructor).");
            }
            try
            {
                var instance = constructorMethod.Invoke(new object[] { _context, this });
                return (BaseRepository<T>)instance;
            }
            catch
            {
                throw new Exception("Chịu luôn đó, kiểm tra lại service: " + nameOfEntity + "  đi. Hàm khởi tạo (constructor) không run được.");
            }
        }

        public void BeginTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _transaction.Complete();
            _transaction.Dispose();
        }

        public void RollBack()
        {
            _transaction.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}