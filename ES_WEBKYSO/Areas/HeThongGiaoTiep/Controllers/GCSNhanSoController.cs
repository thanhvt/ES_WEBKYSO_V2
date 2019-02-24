using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Helpers;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using System.IO;
using Ionic.Zip;
using ES_WEBKYSO.Common;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class GCSNhanSoController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/PhanCongGcs/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            //var dataSOGCS = UnitOfWork.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findModel, "").ToList();
            findModel.USERID = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId; //lấy mã đơn vị thông qua 2 bảng UserProfile và Administrator_Department

            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GETALL().ToList();
            var dataLICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GETALL().ToList();

            //var dataPHANCONG = dataSOGCS.Join(dataCAUHINH, m1  => m1.MA_SOGCS, m2 => m2.MA_SOGCS, (phancong, cauhinh) => new { phancong, cauhinh }).ToList();
            var data = (from SOGCS in dataSOGCS
                        join CAUHINH in dataCAUHINH on SOGCS.MA_SOGCS equals CAUHINH.MA_SOGCS into grouping
                        from SOGCS_NV in grouping.DefaultIfEmpty()
                        select new { SOGCS, SOGCS_NV }).ToList();
            var dataPHANCONG = (from CAUHINH in data
                                join LICH in dataLICHGCS on new { p1 = CAUHINH.SOGCS.MA_SOGCS, p2 = CAUHINH.SOGCS.MA_DVIQLY, p3 = CAUHINH.SOGCS.SO_KY } equals new { p1 = LICH.MA_SOGCS, p2 = LICH.MA_DVIQLY, p3 = LICH.KY } //into grouping
                                select new { CAUHINH, LICH });
            List<PHANCONG> listPhanCong = new List<PHANCONG>();
            foreach (var item in dataPHANCONG)
            {
                PHANCONG phancong = new PHANCONG();
                phancong.ID_LICHGCS = item.LICH.ID_LICHGCS;
                phancong.MA_DVIQLY = item.CAUHINH.SOGCS.MA_DVIQLY;
                phancong.MA_SOGCS = item.CAUHINH.SOGCS.MA_SOGCS;
                phancong.TEN_SOGCS = item.CAUHINH.SOGCS.TEN_SOGCS;
                phancong.HINHTHUC = item.CAUHINH.SOGCS.HINH_THUC;
                phancong.NGAY_GHI = Convert.ToInt32(item.CAUHINH.SOGCS.NGAY_GHI);
                phancong.KY = Convert.ToInt32(item.LICH.KY);
                phancong.THANG = Convert.ToInt32(item.LICH.THANG);
                phancong.NAM = Convert.ToInt32(item.LICH.NAM);
                phancong.MA_DOIGCS = item.LICH.MA_DOIGCS;
                phancong.USERID = item.LICH.USERID;
                phancong.STATUS = item.LICH.STATUS_PC;
                if (phancong.USERID != null)
                {
                    phancong.TEN_NVIEN_GCS = Uow.RepoBase<UserProfile>().GetOne(phancong.USERID).FullName;
                }
                listPhanCong.Add(phancong);
            }
            paging.data = listPhanCong;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(int ID_LICHGCS)
        {
            var LichGCS = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == ID_LICHGCS);
            //string path = Server.MapPath("~/") + @"TemplateFile\" + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
            string path = Utility.getXMLPath() + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
            byte[] fileBytes = null;
            string fileName = LichGCS.FILE_XML;
            if (new FileInfo(path).Exists)
            {
                fileBytes = System.IO.File.ReadAllBytes(path);
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public FileResult DownloadFiles()
        {
            List<int> ids = Request["txtLichID"].Split(',').Select(Int32.Parse).ToList<int>();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                string fileType = "application/octet-stream";

                var outputStream = new MemoryStream();
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                foreach (var item in ids)
                {
                    var LichGCS = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    //string path = Server.MapPath("~/") + @"TemplateFile\" + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
                    string path = Utility.getXMLPath() + LichGCS.MA_DVIQLY.Trim() + @"\" + LichGCS.FILE_XML;
                    try { if (new FileInfo(path).Exists) zip.AddFile(path, ""); } catch { }
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                //zip.Save(Response.OutputStream);
                zip.Save(Response.OutputStream);
                Response.End();
                outputStream.Position = 0;
                return new FileStreamResult(outputStream, fileType);
            }

        }

    }
}
