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

       public BaseController()
        {
            _repo = new GenericRepository();
            _bangKeHelper = new BangKeHelper(_repo);
             
        }

        public  GenericRepository _repo;
        protected BangKeHelper _bangKeHelper;
        
    }
}