using Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using Microsoft.Ajax.Utilities;

namespace ES_WEBKYSO.Repository.ServiceRepository
{
    // Khởi tạo Generic repo cho tất cả model không có service
    public class BaseRepositorys<T> : BaseRepository<T> where T : class
    {
        public BaseRepositorys(DbContext context, GenericRepository repo) : base(context, repo)
        {

        }
    }

    // Mặt nạ cho các service
    public abstract class BaseRepository<T> : Repository<T> where T : class
    {
        // ReSharper disable once PublicConstructorInAbstractClass
        public GenericRepository _repo;

        public BaseRepository(DbContext context, GenericRepository repo) : base(context)
        {
            _repo = repo;
        }

        // Các hàm dưới đây chỉ để sử dụng override trong các service riêng
        // Các model không có service thì không dùng được



        #region For MDMS area
        public virtual List<T> ManagerGetAllForIndex(string orderKey, ref Paging page)
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }
        public virtual List<T> GETALL()
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }
        //public virtual List<T> ManagerGetAllForIndex(int plantId, string orderKey, ref Paging page)
        //{
        //    throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        //}

        public virtual List<T> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }
        public virtual List<T> ManagerGetAllForIndex(FindModelGcs findModel, string searchString)
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }

        #endregion
        public virtual List<T> ManagerGetAllForIndex(string MaSoGcs, string orderKey, ref Paging page)
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }
        public virtual List<T> ManagerGetAllForIndex(int ID_LICHGCS, string orderKey, ref Paging page)
        {
            throw new Exception("Kiểm tra/thêm " + typeof(T).Name + "Repository !");
        }
    }
}