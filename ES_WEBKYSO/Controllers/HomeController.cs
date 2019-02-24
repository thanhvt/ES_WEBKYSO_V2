using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ES_WEBKYSO.Models;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;


namespace ES_WEBKYSO.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //var model = new SoGcs
            //{
            //    SoGcsName = "123",
            //    Description= "adadasd",
            //    Id = 1
            //};

            //try
            //{
            //    _repo.RepoBase<SoGcs>().Create(model);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            return View();
        }

    }
}
