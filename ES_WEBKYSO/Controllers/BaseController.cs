using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;

namespace ES_WEBKYSO.Controllers
{
    //[Authorize]
    public class BaseController : Controller
    {
        public UnitOfWork Uow;
        protected BangKeHelper _bangKeHelper;

        public BaseController()
        {
            DataContext.DataContext dbContext = new DataContext.DataContext();
            Uow = new UnitOfWork(dbContext);
            _bangKeHelper = new BangKeHelper(Uow);
        }

        public BaseController(UnitOfWork uow)
        {
            //DataContext.DataContext dbContext = new DataContext.DataContext();
            Uow = uow;
            
        }

    }
}