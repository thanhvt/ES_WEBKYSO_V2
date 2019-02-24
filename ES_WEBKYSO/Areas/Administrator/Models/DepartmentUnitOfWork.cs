using System;
using System.Linq;
using System.Transactions;

namespace Administrator.Department.Models
{
    public class DepartmentUnitOfWork : IDisposable
    {
        private DepartmentContext context = new DepartmentContext();
        private TransactionScope transaction;
        private DepartmentGenericRepository<Administrator_Department> administrator_DepartmentRepository;
        private DepartmentGenericRepository<UserProfile> userProfileRepository;

        public DepartmentGenericRepository<Administrator_Department> Administrator_DepartmentRepository
        {
            get
            {
                if (this.administrator_DepartmentRepository == null)
                {
                    this.administrator_DepartmentRepository = new DepartmentGenericRepository<Administrator_Department>(context);
                }
                return administrator_DepartmentRepository;
            }
        }

        public DepartmentGenericRepository<UserProfile> UserProfileRepository
        {
            get
            {
                if (this.userProfileRepository == null)
                {
                    this.userProfileRepository = new DepartmentGenericRepository<UserProfile>(context);
                }
                return userProfileRepository;
            }
        }

        public void BeginTransaction()
        {
            transaction = new TransactionScope();
        }

        public void Commit()
        {
            transaction.Complete();
            transaction.Dispose();
        }

        public void RollBack()
        {
            transaction.Dispose();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}