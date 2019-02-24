using Common.Helpers;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class NhanFileController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/NhanFile/

        public ActionResult Index()
        {
            ViewBag.MA_DVIQLY = new CommonUserProfile().MA_DVIQLY;
            return View();
        }
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            //var dataSOGCS = UnitOfWork.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findModel, "").ToList();
            var dataSOGCS = Uow.RepoBase<D_SOGCS>().GETALL();  //lấy hết sổ
            if (findModel.TrangThai == "" || findModel.TrangThai == null) findModel.TrangThai = "CCN"; //gán trạng thái đã phân công
            findModel.USERID = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId; //lấy mã đơn vị thông qua 2 bảng UserProfile và Administrator_Department

            var dataLICHGCS = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            var dataCAUHINH = Uow.RepoBase<CFG_SOGCS_NVIEN>().GETALL().ToList();

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
                phancong.STATUS = item.LICH.STATUS_CNCS;
                if (phancong.USERID != null)
                {
                    phancong.TEN_NVIEN_GCS = Uow.RepoBase<UserProfile>().GetOne(phancong.USERID).FullName;
                }
                phancong.TINHTRANGSO = item.LICH.STATUS_DHK;
                listPhanCong.Add(phancong);
            }
            paging.data = listPhanCong;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDOI_GCS(string MA_DVIQLY)
        {
            try
            {
                var listDOIGCS = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY).ToList();
                List<DANHMUC> listDOI = new List<DANHMUC>();
                foreach (var item in listDOIGCS)
                {
                    DANHMUC dm = new DANHMUC();
                    dm.MAChar = item.MA_DOIGCS;
                    dm.TEN = item.TEN_DOI;
                    listDOI.Add(dm);
                }
                return Json(listDOI, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        [HttpPost]
        public ActionResult UploadFile(List<HttpPostedFileBase> files)
        {
            Common.CommonJsonResult result = new Common.CommonJsonResult();
            try
            {
                List<int> ids = Request["txtLichID"].Split(',').Select(Int32.Parse).ToList<int>();
                bool check = false;
                foreach (var item in ids)
                {
                    var LichGCS = Uow.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == item);
                    foreach (var f in files)
                    {
                        if (f.FileName.Trim() == LichGCS.FILE_XML.Trim())
                        {
                            //var pathFile = Path.Combine(Server.MapPath("~/" + @"TemplateFile/" + LichGCS.MA_DVIQLY.Trim() + @"/"), f.FileName);
                            var pathFile = Utility.getXMLPath() + LichGCS.MA_DVIQLY.Trim() + @"\" + f.FileName;
                            try
                            {
                                if (!new FileInfo(pathFile).Exists) { result.Message += "\nKhông tìm thấy sổ " + LichGCS.MA_SOGCS; continue; }

                                

                                f.SaveAs(pathFile);

                                var lst = new List<string>();
                                DataSet dsSo = new DataSet();
                                dsSo.ReadXml(pathFile);
                                var numberRow = dsSo.Tables[0].Select("CS_MOI is not null").Length;
                                if (dsSo.Tables.Count > 0)
                                {
                                    foreach (DataTable table in dsSo.Tables)
                                    {
                                        foreach (DataRow row in table.Rows)
                                        {
                                            var x = row["CS_MOI"].ToString();
                                            if (x == "0")
                                            {
                                                lst.Add(x);
                                            }
                                        }
                                    }
                                }
                                var countRows = lst.Count();

                                LichGCS.STATUS_CNCS = "DCN";
                                LichGCS.STATUS_NVK = "NVCK";
                                LichGCS.STATUS_DTK = numberRow.ToString();
                                LichGCS.STATUS_DHK = countRows.ToString();
                                var ret = Uow.RepoBase<GCS_LICHGCS>().Update(LichGCS);
                                if (ret <= 0)
                                {
                                    result.Message += "\nCập nhật sổ " + LichGCS.MA_SOGCS + " không thành công";
                                }
                                check = true;
                                break;
                            }
                            catch (Exception ex) { result.Message += "\n" + ex.Message; }

                        }
                    }
                }
                if (check)
                {
                    TempData["MessageStatus"] = true;
                    TempData["Success"] = "Cập nhật dữ liệu chỉ số thành công!";
                    //result.Result = true;
                    //result.Data = "Cập nhật dữ liệu chỉ số thành công!";
                }
                else
                {
                    result.Result = false;
                    result.Message = "Không tìm thấy sổ nào!";
                    TempData["MessageStatus"] = result.Result;
                    TempData["Error"] = result.Message;

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                //result.Message += "\n" + ex.Message;
                result.Message = "Vui lòng chọn File cần tải lên!";
                TempData["MessageStatus"] = result.Result;
                TempData["Error"] = result.Message;                
            }
            return RedirectToAction("Index", "NhanFile");
        }
    }
}
