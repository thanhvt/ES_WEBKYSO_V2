using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Common.Helpers;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Common;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class CapNhatChiSoController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/CapNhatChiSo/

        public ActionResult Index()
        {
            ViewBag.Title = "Cập nhật dữ liệu chỉ số MDMS";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = Uow.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //var data2 = UnitOfWork.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

            //var m = data.Join(data2, d1 => d1.MA_SOGCS, d2 => d2.MA_SOGCS, (sogcs, gcs) => new { sogcs, gcs }).ToList();

            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        //#region Cập nhật dữ liệu chỉ số MDMS
        //public ActionResult JsonCapNhatDuLieuCmis(FindModelGcs findModel, string search, string maDvQly)
        //{
        //    try
        //    {
        //        int nam = Convert.ToInt32(findModel.Nam);
        //        int thang = Convert.ToInt32(findModel.Thang);
        //        int ky = Convert.ToInt32(findModel.Ky);
        //        var status = "DTH";

        //        if (thang == 0 && ky == 0 || nam == 0)
        //        {
        //            return Json(new { success = false, message = "Vui lòng chọn kỳ, tháng và năm" }, JsonRequestBehavior.AllowGet);
        //        }

        //        var data = UnitOfWork.RepoBase<D_SOGCS>()
        //            .ManagerGetAllForIndex(findModel, search)
        //            .ToList()
        //            .Select(x => x.MA_SOGCS).ToList();

        //        var configInput = UnitOfWork.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "MDMS");
        //        Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
        //        if (configInput != null)
        //        {
        //            ser.Url = configInput.Value;
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" }, JsonRequestBehavior.AllowGet);
        //        }

        //        MdmsService.mdms_serviceClient mdmsSv = new MdmsService.mdms_serviceClient();

        //        //var lstLoi = new List<string>();

        //        foreach (var items in data)
        //        {
        //            var mapPath = Server.MapPath("~/TemplateFile/" + items + "-" + nam + "-" + thang + "-" + ky + ".xml");

        //            DataSet dsSo = new DataSet();
        //            dsSo.ReadXml(mapPath);

        //            if (dsSo.Tables.Count > 0)
        //            {
        //                List<MdmsService.getBillingDataFromMDMSTable1> lstBDataMdms = new List<MdmsService.getBillingDataFromMDMSTable1>();
        //                foreach (DataTable dtDiemDo in dsSo.Tables)
        //                {
        //                    if (dtDiemDo.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow Row in dtDiemDo.Rows)
        //                        {
        //                            MdmsService.getBillingDataFromMDMSTable1 getMdms = new MdmsService.getBillingDataFromMDMSTable1();
        //                            getMdms.MA_DDO = Row["MA_DDO"].ToString();
        //                            getMdms.LOAI_BCS = Row["LOAI_BCS"].ToString();
        //                            getMdms.MA_CTO = Row["MA_CTO"].ToString();
        //                            getMdms.NGAY_MOI = Row["NGAY_MOI"].ToString();
        //                            getMdms.HSN = Row["HSN"].ToString();
        //                            getMdms.CS_CU = Row["CS_CU"].ToString();

        //                            //getMdms.MA_DDO = "PD040091007001";
        //                            //getMdms.LOAI_BCS = "BT";
        //                            //getMdms.MA_CTO = "772201212174368";
        //                            //getMdms.NGAY_MOI = "2017-07-01T00:00:00";
        //                            //getMdms.HSN = "200";
        //                            //getMdms.CS_CU = "1590";
        //                            lstBDataMdms.Add(getMdms);
        //                        }
        //                    }
        //                }
        //                var updateDataMdms = mdmsSv.getBillingDataFromMDMS(lstBDataMdms.ToArray(), findModel.Gio);
        //                int index = 0;
        //                foreach (ES_WEBKYSO.MdmsService.getBillingDataFromMDMSResponseTable1 dt in updateDataMdms)
        //                {
        //                    dsSo.Tables[0].Rows[index]["CS_MOI"] = dt.CS_MOI;
        //                    dsSo.Tables[0].Rows[index]["PMAX"] = dt.PMAX;
        //                    dsSo.Tables[0].Rows[index]["SL_MOI"] = dt.SL_MOI;
        //                    index++;
        //                }
        //                var kqSoGcs = UnitOfWork.RepoBase<D_SOGCS>().GetOne(x => x.MA_DVIQLY == maDvQly && x.MA_SOGCS == items);
        //                var kqLichGcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_DVIQLY == maDvQly && x.MA_SOGCS == items && x.KY == ky && x.THANG == thang && x.NAM == nam);
        //                if (kqSoGcs != null && kqLichGcs != null)
        //                {
        //                    kqSoGcs.TRANG_THAI = status;
        //                    kqLichGcs.STATUS = status;
        //                    int sokq = UnitOfWork.RepoBase<GCS_LICHGCS>().Update(kqLichGcs);
        //                    int lichqkq = UnitOfWork.RepoBase<GCS_LICHGCS>().Update(kqLichGcs);
        //                }
        //                else
        //                {
        //                    return Json(new { success = true, message = "Không tồn tại mã sổ " + items + "!" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            dsSo.WriteXml(mapPath);
        //        }
        //        return Json(new { success = true, message = "Cập nhật dữ liệu chỉ số thành công!" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(new { success = true, message = "Cập nhật dữ liệu chỉ số không thành công!" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //#endregion

        public ActionResult JsonCapNhatDuLieuCmis(FindModelGcs findModel, string search, List<int> ids, string MaDonVi, int Ky, int Thang, int Nam, int Gio)
        {
            var lstError = new List<string>();
            var lstSuccess = new List<string>();
            try
            {
                var status = "DCN";
                //var statusNvKy = "NVCK";

                if (Thang == 0 && Ky == 0 || Nam == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn kỳ, tháng và năm" },
                        JsonRequestBehavior.AllowGet);
                }

                var data = Uow.RepoBase<D_SOGCS>()
                    .ManagerGetAllForIndex(findModel, search)
                    .ToList()
                    .Select(x => x.MA_SOGCS).ToList();

                var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "MDMS");
                //Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
                //if (configInput != null)
                //{
                //    ser.Url = configInput.Value;
                //}
                //else
                //{
                //    return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
                //        JsonRequestBehavior.AllowGet);
                //}
                //Khởi tạo client
                MdmsService.mdms_serviceClient mdmsSv = new MdmsService.mdms_serviceClient();
                string user = ConfigurationManager.AppSettings["MdmsUsername"];
                string pass = ConfigurationManager.AppSettings["MdmsPassword"];
                //Sử dụng OperationContex set thông tin user pw
                using (new OperationContextScope(mdmsSv.InnerChannel))
                {
                    //Set thông tin user pw trên HTTP header để xác thực, thông tin user pw có thể lưu ở file config
                    HttpRequestMessageProperty userHeader = new HttpRequestMessageProperty();
                    userHeader.Headers.Add("Username", user);
                    userHeader.Headers.Add("Password", pass);
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = userHeader;
                    //Gọi function như bình thường để lấy dữ liệu
                    //Console.WriteLine(mdmsSv.hello());


                    //var lstLoi = new List<string>();

                    foreach (var soId in ids)
                    {
                        var soGcsId = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_SOGCS;
                        //var mapPath = Server.MapPath("~/TemplateFile/" + Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_DVIQLY.Trim() + @"/" + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml");
                        var mapPath = Utility.getXMLPath() + Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId).MA_DVIQLY.Trim() + @"/" + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml";
                        if (System.IO.File.Exists(mapPath))
                        {
                            //Thực hiện cập nhật dữ liệu chỉ số MDMS   
                            #region MDMS

                            var lst = new List<string>();
                            DataSet dsSo = new DataSet();
                            dsSo.ReadXml(mapPath);
                            var numberRow = dsSo.Tables[0].Select("CS_MOI is not null").Length;

                            if (dsSo.Tables.Count > 0)
                            {
                                List<MdmsService.getBillingDataFromMDMSTable1> lstBDataMdms =
                                    new List<MdmsService.getBillingDataFromMDMSTable1>();

                                foreach (DataTable dtDiemDo in dsSo.Tables)
                                {
                                    if (dtDiemDo.Rows.Count > 0)
                                    {
                                        foreach (DataRow Row in dtDiemDo.Rows)
                                        {
                                            MdmsService.getBillingDataFromMDMSTable1 getMdms =
                                                new MdmsService.getBillingDataFromMDMSTable1();
                                            getMdms.MA_DDO = Row["MA_DDO"].ToString();
                                            getMdms.LOAI_BCS = Row["LOAI_BCS"].ToString();
                                            getMdms.MA_CTO = Row["MA_CTO"].ToString();
                                            getMdms.NGAY_MOI = Row["NGAY_MOI"].ToString();
                                            getMdms.HSN = Row["HSN"].ToString();
                                            getMdms.CS_CU = Row["CS_CU"].ToString();

                                            //getMdms.MA_DDO = "PD040091007001";
                                            //getMdms.LOAI_BCS = "BT";
                                            //getMdms.MA_CTO = "772201212174368";
                                            //getMdms.NGAY_MOI = "2017-07-01T00:00:00";
                                            //getMdms.HSN = "200";
                                            //getMdms.CS_CU = "1590";
                                            lstBDataMdms.Add(getMdms);
                                        }
                                    }
                                }
                                var updateDataMdms = mdmsSv.getBillingDataFromMDMS(lstBDataMdms.ToArray(), findModel.Gio);
                                int index = 0;
                                foreach (ES_WEBKYSO.MdmsService.getBillingDataFromMDMSResponseTable1 dt in updateDataMdms)
                                {
                                    var countCsoMoi = dsSo.Tables[0].Rows[index]["CS_MOI"].ToString();
                                    var csMoi = dt.CS_MOI;
                                    var pMax = dt.PMAX;
                                    var slMoi = dt.SL_MOI;
                                    if (csMoi != null)
                                    {
                                        dsSo.Tables[0].Rows[index]["CS_MOI"] = dt.CS_MOI;
                                    }
                                    else
                                    {
                                        dsSo.Tables[0].Rows[index]["CS_MOI"] = 0;
                                    }
                                    if (pMax != null)
                                    {
                                        dsSo.Tables[0].Rows[index]["PMAX"] = dt.PMAX;
                                    }
                                    else
                                    {
                                        dsSo.Tables[0].Rows[index]["PMAX"] = 0;
                                    }
                                    if (slMoi != null)
                                    {
                                        dsSo.Tables[0].Rows[index]["SL_MOI"] = dt.SL_MOI;
                                    }
                                    else
                                    {
                                        dsSo.Tables[0].Rows[index]["SL_MOI"] = 0;
                                    }


                                    if (countCsoMoi != "0")
                                    {
                                        lst.Add(countCsoMoi);
                                    }

                                    index++;
                                }
                                var countLst = lst.Count();
                                //var kqSoGcs =
                                //    UnitOfWork.RepoBase<D_SOGCS>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId);
                                var kqLichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_DVIQLY == MaDonVi
                                                    && x.MA_SOGCS == soGcsId
                                                    && x.KY == Ky
                                                    && x.THANG == Thang
                                                    && x.NAM == Nam);
                                if (kqLichGcs != null)
                                {
                                    kqLichGcs.STATUS_CNCS = status;
                                    kqLichGcs.STATUS_DTK = numberRow.ToString();
                                    kqLichGcs.STATUS_DHK = countLst.ToString();
                                    int lichqkq = Uow.RepoBase<GCS_LICHGCS>().Update(kqLichGcs);
                                }
                                else
                                {
                                    return Json(new { success = true, message = "Không tồn tại mã sổ " + soGcsId + "trong bảng GCS_LICHGCS ho!" }, JsonRequestBehavior.AllowGet);
                                }
                                lstSuccess.Add(soGcsId);
                                dsSo.WriteXml(mapPath);
                            }
                            else
                            {
                                return Json(new { success = true, message = "Dữ liệu không tồn tại!" },
                                    JsonRequestBehavior.AllowGet);
                            }


                            #endregion
                        }
                        else
                        {
                            lstError.Add(soGcsId);
                        }
                        // Lưu Log
                        var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                        var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == soId);
                        var logCategoryId = "CNCS_MDMS";
                        var contentLog = "";
                        DateTime logDate = DateTime.Now;
                        var maBangKeLich = "";
                        var logStatus = "DCN";
                        var lstCategoryLog =
                            Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                        var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                        var countThucHien = 1;
                        contentLog = userName + " " + lstCategoryLog + " sổ " + lichGcs.TEN_SOGCS + " thành công";
                        WriteLog writeL = new WriteLog(Uow);
                        writeL.WriteLogGcs(logCategoryId, soId, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maBangKeLich,
                            countThucHien, logStatus);
                    }
                    if (lstSuccess.Count > 0)
                    {
                        return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + " đã được cập nhật thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, message = "Sổ " + string.Join(",", lstError) + " cập nhật không thành công." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Cập nhật dữ liệu chỉ số không thành công! " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDOI_GCS()
        {
            string username = User.Identity.Name;
            var listDOIGCS = new List<D_DOIGCS>();
            string MaDonVi = new CommonUserProfile().MA_DVIQLY;
            try
            {
                if (username == "administrator")
                {
                    listDOIGCS = Uow.RepoBase<D_DOIGCS>().GetAll().ToList();
                }
                else
                {
                    listDOIGCS = Uow.RepoBase<D_DOIGCS>().GetAll(o => o.MA_DVIQLY == MaDonVi).ToList();
                }
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
    }
}
