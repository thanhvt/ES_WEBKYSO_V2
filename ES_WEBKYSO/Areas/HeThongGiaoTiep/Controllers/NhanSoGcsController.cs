using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using Microsoft.Ajax.Utilities;
using System.Xml;
using System.Xml.Serialization;
using ES_WEBKYSO.Repository;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class NhanSoGcsController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/NhanSoGcs/
        #region Declaration

        private readonly CmisRepository _cmisRepository;

        #endregion


        #region Constructor

        public NhanSoGcsController()
        {
            _cmisRepository = new CmisRepository();
        }
        #endregion
        public ActionResult Index()
        {
            ViewBag.Title = "Nhận sổ GCS";
            ViewBag.MaDviQly = new CommonUserProfile().MA_DVIQLY;
            return View();
        }

        /// <summary>
        /// Hiển thị danh sách sổ theo điều kiện tìm kiếm trên form
        /// </summary>
        /// <param name="findModel">Model lưu trữ các biến truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJson(FindModelGcs findModel, string maDvQly, string thang, string nam)
        {
            if (nam == "")
            {
                DateTime dtNow = DateTime.Now;
                thang = dtNow.Month + "";
                nam = dtNow.Year + "";
            }
            var paging = Request.Params.ToPaging("MA_DVIQLY");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            //var data = Uow.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();
            //var data2 = UnitOfWork.RepoBase<GCS_LICHGCS>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).ToList();

            //var m = data.Join(data2, d1 => d1.MA_SOGCS, d2 => d2.MA_SOGCS, (sogcs, gcs) => new { sogcs, gcs }).ToList();

            var strURL_CMISInterface = "";
            var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVTONTHAT");
            var configSVChiSoKHang = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVCHISO");
            //Service_TonThat.Service_TonThat ser = new Service_TonThat.Service_TonThat();
            if (configInput != null)
            {
                strURL_CMISInterface = configInput.Value;
            }
            else
            {
                return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
                    JsonRequestBehavior.AllowGet);
            }

            _cmisRepository.strURL_CMISInterface = strURL_CMISInterface + "getDanhMuc";
            var getSoFromCmis = _cmisRepository.GetSoFromCmis(maDvQly);
            //var getSoFromCmis = _cmisRepository.GetSoXuatHHC(maDvQly, thang, nam);
            //ThanhVT khong thay con bang do nua.
            if (getSoFromCmis != null)
            {
                #region Chỉ cho những sổ thỏa mãn điều kiện tìm kiếm được như hiện tại (bên hệ thống đó chưa nhận) và những sổ thỏa mãn đang xuất HHC (CMIS3 đang xuất)
                List<string> listSoGCSOK = new List<string>();
                try
                {
                    bool okSoGCS = false;
                    string strURL_ChiSoKhang = configSVChiSoKHang.Value + "getGcsHhcService";
                    dynamic iInput = new JObject();
                    iInput.MA_DVIQLY = maDvQly;
                    iInput.MA_SOGCS = "";
                    iInput.THANG = thang + "";
                    iInput.NAM = nam + "";
                    iInput.KY = "0";
                    iInput.isTimNgayCky = false;
                    iInput.NGAY_CKY = "01/01/2001";

                    HttpWebRequest requestSoGCS = (HttpWebRequest)WebRequest.Create(strURL_ChiSoKhang);
                    requestSoGCS.Method = "POST";

                    System.Text.UTF8Encoding encodingSoGCS = new System.Text.UTF8Encoding();
                    Byte[] byteArraySoGCS = encodingSoGCS.GetBytes(iInput.ToString());

                    requestSoGCS.ContentLength = byteArraySoGCS.Length;
                    requestSoGCS.ContentType = @"application/json";

                    using (Stream dataStream = requestSoGCS.GetRequestStream())
                    {
                        dataStream.Write(byteArraySoGCS, 0, byteArraySoGCS.Length);
                    }
                    DataTable dtSoGCSOK = new DataTable();
                    WebResponse webResponseSoGCS = requestSoGCS.GetResponse();
                    using (Stream webStream = webResponseSoGCS.GetResponseStream() ?? Stream.Null)
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        var response = responseReader.ReadToEnd();
                        if (response != null && response.ToString() != "")
                        {
                            dynamic dynObj = JsonConvert.DeserializeObject(response);
                            if (dynObj != null)
                            {
                                string arrSOGCS = dynObj.ToString();
                                if (!arrSOGCS.Contains("NODATA") && !arrSOGCS.Contains("ERROR"))
                                {
                                    dtSoGCSOK = (DataTable)JsonConvert.DeserializeObject(arrSOGCS, (typeof(DataTable)));
                                    if (dtSoGCSOK.Rows.Count > 0)
                                    {
                                        listSoGCSOK = dtSoGCSOK.AsEnumerable()
                                            .Select(r => r.Field<string>("maSogcs"))
                                            .ToList();
                                    }
                                }
                            }
                        }
                    }
                    if (!okSoGCS)
                    {
                        //return Json(new { success = false, message = "Sổ GCS " + soGcsId + " không thỏa mãn điều kiện hệ thống chưa nhận hoặc CMIS3 đang xuất HHC!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch { }

                #endregion


                DataTable dt = new DataTable();
                var id = (from DataRow dr in getSoFromCmis.Tables[0].Rows
                          select dr["maSogcs"].ToString()).ToList();
                int iThang = Convert.ToInt16(thang);
                int iNam = Convert.ToInt16(nam);

                var lstLich = Uow.RepoBase<GCS_LICHGCS>().GetAll().ToList();

                var dsSoDaXuat = lstLich.Where(o => (findModel.MaSo == null ||
                                                                o.MA_SOGCS.Contains(findModel.MaSo))
                                                            && o.THANG.Equals(iThang)
                                                            && o.NAM.Equals(iNam)).Select(i => i.MA_SOGCS).ToList();
                //var data = Uow.RepoBase<D_SOGCS>().GetAll(o => id.Any(u => u == o.MA_SOGCS)
                //                                               && listSoGCSOK.Contains(o.MA_SOGCS)
                //                                               &&
                //                                               (findModel.MaSo == null ||
                //                                                o.MA_SOGCS.Contains(findModel.MaSo))
                //                                               &&
                //                                               (findModel.TenSo == null ||
                //                                                o.TEN_SOGCS.Contains(findModel.TenSo))
                //                                               &&
                //                                               (findModel.NgayGhi == null ||
                //                                                o.NGAY_GHI == findModel.NgayGhi)
                //                                               &&
                //                                               (findModel.TrangThai == null ||
                //                                                o.TRANG_THAI.Contains(findModel.TrangThai))
                //                                               &&
                //                                               (paging.Key == null ||
                //                                                (o.MA_SOGCS.Contains(paging.Key) ||
                //                                                 o.TEN_SOGCS.Contains(paging.Key))), paging.OrderKey,
                //    ref paging).ToList();

                var data = Uow.RepoBase<D_SOGCS>().GetAll(o => id.Any(u => u == o.MA_SOGCS)
                                                               &&
                                                               (findModel.MaSo == null ||
                                                                o.MA_SOGCS.Contains(findModel.MaSo))
                                                               &&
                                                               o.TINH_TRANG == 1
                                                               &&
                                                               (findModel.TenSo == null ||
                                                                o.TEN_SOGCS.Contains(findModel.TenSo))
                                                               &&
                                                               (findModel.NgayGhi == null ||
                                                                o.NGAY_GHI == findModel.NgayGhi)
                                                               &&
                                                               (findModel.TrangThai == null ||
                                                                (findModel.TrangThai.Equals("DL") && dsSoDaXuat.Contains(o.MA_SOGCS)) ||
                                                                (findModel.TrangThai.Equals("CL") && !dsSoDaXuat.Contains(o.MA_SOGCS))
                                                               )
                                                               &&
                                                               (paging.Key == null ||
                                                                (o.MA_SOGCS.Contains(paging.Key) ||
                                                                 o.TEN_SOGCS.Contains(paging.Key))), paging.OrderKey,
                    ref paging).ToList();

                paging.data = data;
            }
            else
            {
                paging.data = 0;
            }
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JsonLayDuLieuCmis(GCS_LICHGCS model, FindModelGcs findModel, string search, List<int> ids, string MaDonVi, int Ky, int Thang, int Nam)
        {
            var lstError = new List<string>();
            var lstSuccess = new List<string>();
            List<GCS_LICHGCS> listLichNEW = new List<GCS_LICHGCS>();
            string result = "";
            try
            {
                var statusNs = "DL";
                var statusPc = "CPC";
                var statusDpc = "DPC";
                var statusCncs = "CCN";
                var strURL_CMISInterface = "";
                if (Thang == 0 && Ky == 0 || Nam == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn kỳ, tháng và năm" },
                        JsonRequestBehavior.AllowGet);
                }
                var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "CMIS");
                var configInputTonThat = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVTONTHAT");
                var configSVChiSoKHang = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "SVCHISO");
                //Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
                //Service_TonThat.Service_TonThat serTonThat = new Service_TonThat.Service_TonThat();
                if (configInput != null || configInputTonThat != null)
                {
                    strURL_CMISInterface = configInput.Value + "ReadHHCService";
                }
                else
                {
                    return Json(new { success = false, message = "Chưa khai báo tham số kết nối đến hệ thống CMIS!" },
                        JsonRequestBehavior.AllowGet);
                }

                foreach (var soId in ids)
                {
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    var soGcsId = Uow.RepoBase<D_SOGCS>().GetOne(x => x.ID_SOGCS == soId).MA_SOGCS;
                    var loaiSoGcs = Uow.RepoBase<D_SOGCS>().GetOne(x => x.ID_SOGCS == soId).LOAI_SOGCS;
                    var modelSoGcs = Uow.RepoBase<D_SOGCS>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId);
                    try
                    {
                        var modelLichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId && x.KY == Ky && x.THANG == Thang && x.NAM == Nam);
                        if (modelLichGcs != null)
                        {
                            return
                                Json(new { success = false, message = "Xảy ra lỗi: Mã đơn vị quản lý và mã sổ đã tồn tại" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch { }

                    #region Chỉ cho những sổ thỏa mãn điều kiện tìm kiếm được như hiện tại (bên hệ thống đó chưa nhận) và những sổ thỏa mãn đang xuất HHC (CMIS3 đang xuất)
                    try
                    {
                        bool okSoGCS = false;
                        string strURL_ChiSoKhang = configSVChiSoKHang.Value + "getGcsHhcService";
                        dynamic iInput = new JObject();
                        iInput.MA_DVIQLY = MaDonVi;
                        iInput.MA_SOGCS = "";
                        iInput.THANG = Thang + "";
                        iInput.NAM = Nam + "";
                        iInput.KY = "0";
                        iInput.isTimNgayCky = false;
                        iInput.NGAY_CKY = "01/01/2001";

                        HttpWebRequest requestSoGCS = (HttpWebRequest)WebRequest.Create(strURL_ChiSoKhang);
                        requestSoGCS.Method = "POST";

                        System.Text.UTF8Encoding encodingSoGCS = new System.Text.UTF8Encoding();
                        Byte[] byteArraySoGCS = encodingSoGCS.GetBytes(iInput.ToString());

                        requestSoGCS.ContentLength = byteArraySoGCS.Length;
                        requestSoGCS.ContentType = @"application/json";

                        using (Stream dataStream = requestSoGCS.GetRequestStream())
                        {
                            dataStream.Write(byteArraySoGCS, 0, byteArraySoGCS.Length);
                        }
                        DataTable dtSoGCSOK = new DataTable();
                        WebResponse webResponseSoGCS = requestSoGCS.GetResponse();
                        using (Stream webStream = webResponseSoGCS.GetResponseStream() ?? Stream.Null)
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            var response = responseReader.ReadToEnd();
                            if (response != null && response.ToString() != "")
                            {
                                dynamic dynObj = JsonConvert.DeserializeObject(response);
                                if (dynObj != null)
                                {
                                    string arrSOGCS = dynObj.ToString();
                                    dtSoGCSOK = (DataTable)JsonConvert.DeserializeObject(arrSOGCS, (typeof(DataTable)));
                                    if(dtSoGCSOK.Rows.Count > 0)
                                    {
                                        string expression = "maSogcs = " + soGcsId;
                                        DataRow[] selectedRows = dtSoGCSOK.Select(expression);
                                        if (selectedRows.Count() > 0) okSoGCS = true;
                                    }
                                }
                            }
                        }
                        if (!okSoGCS)
                        {
                            return Json(new { success = false, message = "Sổ GCS " + soGcsId + " không thỏa mãn điều kiện hệ thống chưa nhận hoặc CMIS3 đang xuất HHC!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch { }
                   
                    #endregion

                    dynamic product = new JObject();
                    product.MA_DVIQLY = MaDonVi;
                    product.MA_SOGCS = soGcsId;
                    product.KY = Ky + "";
                    product.THANG = Thang + "";
                    product.NAM = Nam + "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL_CMISInterface);
                    request.Method = "POST";

                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    Byte[] byteArray = encoding.GetBytes(product.ToString());

                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";

                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    try
                    {
                        //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        //{
                        //	length = response.ContentLength;								
                        //}

                        WebResponse webResponse = request.GetResponse();
                        using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            var response = responseReader.ReadToEnd();
                            Console.Out.WriteLine(response);
                            if (!response.Contains("<ERROR>"))
                            {
                                string fileNameTtbt = soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml";
                                //string directoryPathTtbt = CommonHelper.TemplateFile + MaDonVi;
                                //string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                                //if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                                //    Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                                //using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                                //{
                                //    using (XmlTextWriter xmlWriter = new XmlTextWriter(fs, Encoding.Unicode))
                                //    {
                                //        dataFromCmis.WriteXml(xmlWriter, XmlWriteMode.WriteSchema); 
                                //    } 
                                //}

                                var mapPath = Utility.getXMLPath() + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml"; 
                                using (StreamWriter outputFile = new StreamWriter(mapPath))
                                { 
                                    outputFile.WriteLine(response);
                                }

                                var mapDviPath = Utility.getXMLPath() + MaDonVi + @"\" + soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml";
                                using (StreamWriter outputFile = new StreamWriter(mapDviPath))
                                {
                                    outputFile.WriteLine(response);
                                }

                                var cutFile = mapPath.LastIndexOf(@"\");

                                var getTenso = Uow.RepoBase<D_SOGCS>().GetAll(x => x.ID_SOGCS == soId).FirstOrDefault();
                                var tblCauHinhGcs = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId);

                                if (tblCauHinhGcs != null)
                                {
                                    var newmodel = new GCS_LICHGCS
                                    {
                                        MA_DVIQLY = MaDonVi,
                                        MA_SOGCS = soGcsId,
                                        TEN_SOGCS = (getTenso.TEN_SOGCS == null) ? "" : getTenso.TEN_SOGCS,
                                        HINH_THUC = (getTenso.HINH_THUC == null) ? "" : getTenso.HINH_THUC,
                                        NGAY_GHI = getTenso.NGAY_GHI,
                                        KY = Ky,
                                        THANG = Thang,
                                        NAM = Nam,
                                        STATUS = statusNs,
                                        STATUS_PC = statusDpc,
                                        STATUS_CNCS = statusCncs,
                                        NHANSO_MTB = "false",
                                        MA_DOIGCS = tblCauHinhGcs.MA_DOIGCS,
                                        USERID = tblCauHinhGcs.USERID,
                                        FILE_XML = fileNameTtbt
                                    };

                                    modelSoGcs.TRANG_THAI = statusNs;

                                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().Create(newmodel);
                                    var soGcs = Uow.RepoBase<D_SOGCS>().Update(modelSoGcs);
                                    if (lichGcs != 1 && soGcs != 1)
                                    {
                                        return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công!" }, JsonRequestBehavior.AllowGet);
                                    }
                                    listLichNEW.Add(newmodel);
                                    CreateMail(tblCauHinhGcs.MA_DOIGCS, userId, listLichNEW);

                                    // Lưu Log
                                    //var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                                    var gcslichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_SOGCS == soGcsId);
                                    var logCategoryPc = "PC_TUDONG";
                                    var contentLogPc = "";
                                    DateTime logDatePc = DateTime.Now;
                                    var maBangKeLichPc = "";
                                    var logStatusPc = "DPC";
                                    var lstCategoryLog =
                                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryPc).LOG_CATEGORY_NAME;
                                    var userNamePc = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                                    var countThucHien = 1;
                                    contentLogPc = userNamePc + " " + lstCategoryLog + " sổ " + gcslichGcs.TEN_SOGCS + " thành công";
                                    WriteLog writeL = new WriteLog(Uow);
                                    writeL.WriteLogGcs(logCategoryPc, gcslichGcs.ID_LICHGCS, gcslichGcs.MA_SOGCS, gcslichGcs.KY, gcslichGcs.THANG,
                                        gcslichGcs.NAM, contentLogPc, userId, logDatePc, maBangKeLichPc,
                                        countThucHien, logStatusPc);

                                    lstSuccess.Add(soGcsId);
                                }
                                else
                                {
                                    var newmodel = new GCS_LICHGCS
                                    {
                                        MA_DVIQLY = MaDonVi,
                                        MA_SOGCS = soGcsId,
                                        TEN_SOGCS = (getTenso.TEN_SOGCS == null) ? "" : getTenso.TEN_SOGCS,
                                        HINH_THUC = (getTenso.HINH_THUC == null) ? "" : getTenso.HINH_THUC,
                                        NGAY_GHI = getTenso.NGAY_GHI,
                                        KY = Ky,
                                        THANG = Thang,
                                        NAM = Nam,
                                        STATUS = statusNs,
                                        STATUS_PC = statusPc,
                                        FILE_XML = fileNameTtbt
                                    };
                                    modelSoGcs.TRANG_THAI = statusNs;

                                    var lichGcs = Uow.RepoBase<GCS_LICHGCS>().Create(newmodel);
                                    var soGcs = Uow.RepoBase<D_SOGCS>().Update(modelSoGcs);
                                    if (lichGcs != 1 && soGcs != 1)
                                    {
                                        return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công!" }, JsonRequestBehavior.AllowGet);
                                    }
                                    lstSuccess.Add(soGcsId);
                                }
                            }
                            else
                            {
                                lstError.Add(soGcsId);
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        return Json(new { success = true, message = "Lấy dữ liệu CMIS không thành công! " + ex.Message },
                                  JsonRequestBehavior.AllowGet);
                    }

                    //DataSet dataFromCmis = new DataSet();
                    //if (loaiSoGcs == "DN")
                    //{
                    //    dataFromCmis = serTonThat.ReadXmlHHCTT(MaDonVi, soGcsId, Ky, Thang, Nam, ref result); 
                    //}
                    //else
                    //{
                    //    dataFromCmis = ser.ReadHHCService(MaDonVi, soGcsId, Ky, Thang, Nam, ref result);
                    //}


                    //if (result == "Ok")
                    //{
                    //    string fileNameTtbt = soGcsId + "-" + Nam + "-" + Thang + "-" + Ky + ".xml";
                    //    string directoryPathTtbt = CommonHelper.TemplateFile + MaDonVi;
                    //    string filePathTtbt = directoryPathTtbt + "/" + fileNameTtbt;
                    //    if (!Directory.Exists(Server.MapPath(directoryPathTtbt)))
                    //        Directory.CreateDirectory(Server.MapPath(directoryPathTtbt));
                    //    using (FileStream fs = new FileStream(Server.MapPath(filePathTtbt), FileMode.Create))
                    //    {
                    //        using (XmlTextWriter xmlWriter = new XmlTextWriter(fs, Encoding.Unicode))
                    //        {
                    //            dataFromCmis.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
                    //            Console.WriteLine("Write {0} to the File {1}.", dataFromCmis.DataSetName, fileNameTtbt);
                    //            Console.WriteLine();
                    //        }
                    //        //dataFromCmis.WriteXml(filePathTtbt);
                    //    }

                    //    //var cutFile = filePathTtbt.LastIndexOf(@"\");

                    //    var getTenso = Uow.RepoBase<D_SOGCS>().GetAll(x => x.ID_SOGCS == soId).FirstOrDefault();
                    //    var tblCauHinhGcs = Uow.RepoBase<CFG_SOGCS_NVIEN>().GetOne(x => x.MA_DVIQLY == MaDonVi && x.MA_SOGCS == soGcsId);

                    //    if (tblCauHinhGcs != null)
                    //    {
                    //        var newmodel = new GCS_LICHGCS
                    //        {
                    //            MA_DVIQLY = MaDonVi,
                    //            MA_SOGCS = soGcsId,
                    //            TEN_SOGCS = (getTenso.TEN_SOGCS == null) ? "" : getTenso.TEN_SOGCS,
                    //            HINH_THUC = (getTenso.HINH_THUC == null) ? "" : getTenso.HINH_THUC,
                    //            NGAY_GHI = getTenso.NGAY_GHI,
                    //            KY = Ky,
                    //            THANG = Thang,
                    //            NAM = Nam,
                    //            STATUS = statusNs,
                    //            STATUS_PC = statusDpc,
                    //            STATUS_CNCS = statusCncs,
                    //            NHANSO_MTB = "false",
                    //            MA_DOIGCS = tblCauHinhGcs.MA_DOIGCS,
                    //            USERID = tblCauHinhGcs.USERID,
                    //            FILE_XML = fileNameTtbt
                    //        };

                    //        modelSoGcs.TRANG_THAI = statusNs;

                    //        var lichGcs = Uow.RepoBase<GCS_LICHGCS>().Create(newmodel);
                    //        var soGcs = Uow.RepoBase<D_SOGCS>().Update(modelSoGcs);
                    //        if (lichGcs != 1 && soGcs != 1)
                    //        {
                    //            return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công!" }, JsonRequestBehavior.AllowGet);
                    //        }
                    //        listLichNEW.Add(newmodel);
                    //        CreateMail(tblCauHinhGcs.MA_DOIGCS, userId, listLichNEW);

                    //        // Lưu Log
                    //        //var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    //        var gcslichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_SOGCS == soGcsId);
                    //        var logCategoryPc = "PC_TUDONG";
                    //        var contentLogPc = "";
                    //        DateTime logDatePc = DateTime.Now;
                    //        var maBangKeLichPc = "";
                    //        var logStatusPc = "DPC";
                    //        var lstCategoryLog =
                    //            Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryPc).LOG_CATEGORY_NAME;
                    //        var userNamePc = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    //        var countThucHien = 1;
                    //        contentLogPc = userNamePc + " " + lstCategoryLog + " sổ " + gcslichGcs.TEN_SOGCS + " thành công";
                    //        WriteLog writeL = new WriteLog(Uow);
                    //        writeL.WriteLogGcs(logCategoryPc, gcslichGcs.ID_LICHGCS, gcslichGcs.MA_SOGCS, gcslichGcs.KY, gcslichGcs.THANG,
                    //            gcslichGcs.NAM, contentLogPc, userId, logDatePc, maBangKeLichPc,
                    //            countThucHien, logStatusPc);

                    //        lstSuccess.Add(soGcsId);
                    //    }
                    //    else
                    //    {
                    //        var newmodel = new GCS_LICHGCS
                    //        {
                    //            MA_DVIQLY = MaDonVi,
                    //            MA_SOGCS = soGcsId,
                    //            TEN_SOGCS = (getTenso.TEN_SOGCS == null) ? "" : getTenso.TEN_SOGCS,
                    //            HINH_THUC = (getTenso.HINH_THUC == null) ? "" : getTenso.HINH_THUC,
                    //            NGAY_GHI = getTenso.NGAY_GHI,
                    //            KY = Ky,
                    //            THANG = Thang,
                    //            NAM = Nam,
                    //            STATUS = statusNs,
                    //            STATUS_PC = statusPc,
                    //            FILE_XML = fileNameTtbt
                    //        };
                    //        modelSoGcs.TRANG_THAI = statusNs;

                    //        var lichGcs = Uow.RepoBase<GCS_LICHGCS>().Create(newmodel);
                    //        var soGcs = Uow.RepoBase<D_SOGCS>().Update(modelSoGcs);
                    //        if (lichGcs != 1 && soGcs != 1)
                    //        {
                    //            return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công!" }, JsonRequestBehavior.AllowGet);
                    //        }
                    //        lstSuccess.Add(soGcsId);
                    //    }
                    //}
                    //else
                    //{
                    //    lstError.Add(soGcsId);
                    //}
                    // Lưu Log
                    var gcslichGcsNs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_SOGCS == soGcsId);
                    var logCategoryNs = "CMIS_LAYSO";
                    var contentLogNs = "";
                    DateTime logDateNs = DateTime.Now;
                    var maBangKeLichNs = "";
                    var logStatusNs = "DL";
                    var lstCategoryLogNs =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryNs).LOG_CATEGORY_NAME;
                    var userNameNs = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    var countThucHienNs = 1;
                    contentLogNs = userNameNs + " " + lstCategoryLogNs + " " + gcslichGcsNs.TEN_SOGCS + " thành công";
                    WriteLog writeLogNs = new WriteLog(Uow);
                    writeLogNs.WriteLogGcs(logCategoryNs, gcslichGcsNs.ID_LICHGCS, gcslichGcsNs.MA_SOGCS, gcslichGcsNs.KY,
                        gcslichGcsNs.THANG, gcslichGcsNs.NAM, contentLogNs, userId, logDateNs, maBangKeLichNs, countThucHienNs,
                        logStatusNs);

                }
                if (lstSuccess.Count > 0)
                {
                    //return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + "đã lấy thành công." + " Sổ:  " + string.Join(",", lstError) + " đang ở trạng thái không phải xuất HHC!" }, JsonRequestBehavior.AllowGet);
                    //return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + " đã lấy thành công. Sổ " + string.Join(",", lstError) + " lấy không thành công. Vui lòng kiểm tra lại trạng thái xuất HHC hoặc kiểm tra lại kỳ tháng năm của sổ!" }, JsonRequestBehavior.AllowGet);
                    return Json(new { success = true, message = "Sổ " + string.Join(",", lstSuccess) + " đã lấy thành công."},JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Lấy dữ liệu CMIS không thành công." + " Mã sổ: " + string.Join(",", lstError) + " đang ở trạng thái không phải xuất HHC!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Lấy dữ liệu CMIS không thành công! " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        private void CreateMail(string MADOI, int USERID, List<GCS_LICHGCS> listLich)
        {
            if (MADOI != "" && USERID != 0)
            {//gửi mail khi phân công lại
                string HoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == USERID).FullName;
                string title = "EVN HANOI: Phân công ghi chỉ số";
                string content = "Tổ/Đội thực hiện: " + MADOI + " - " + Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == MADOI).TEN_DOI + "<br/>";
                content += "Cán bộ Ghi chỉ số thực hiện: " + HoTen + "<br/>";
                content += "Chi tiết các sổ được phân công: <br/>";
                foreach (var item in listLich)
                {
                    string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                    content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                }
                SendMail send = new SendMail();
                send.Send_Email(Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == USERID).Email, content, title, "");
            }
            else
            {//gửi mail khi cấu hình
                List<TempMail> listMail = new List<TempMail>();
                var list = listLich;
                //tạo danh sách user gửi mail
                foreach (var item in list)
                {
                    var value = listMail.Find(o => o.UserID == item.USERID);
                    if (value != null)
                    {//thêm nội vào nội dung email sẽ gửi cho user
                        listMail.Remove(value);
                        string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                        value.Content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                        listMail.Add(value);
                    }
                    else
                    {//thêm mail sẽ gửi cho user
                        TempMail temp = new TempMail();
                        temp.MaDoi = item.MA_DOIGCS;
                        temp.TenDoi = Uow.RepoBase<D_DOIGCS>().GetOne(o => o.MA_DOIGCS == temp.MaDoi).TEN_DOI;
                        temp.UserID = item.USERID;
                        temp.HoTen = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == temp.UserID).FullName;
                        temp.Email = Uow.RepoBase<UserProfile>().GetOne(o => o.UserId == temp.UserID).Email;
                        string TenSo = Uow.RepoBase<D_SOGCS>().GetOne(o => o.MA_SOGCS == item.MA_SOGCS && o.MA_DVIQLY == item.MA_DVIQLY).TEN_SOGCS;
                        temp.Content += "+ " + item.MA_SOGCS + ": " + TenSo + "<br/>";
                        listMail.Add(temp);
                    }
                }
                foreach (var item in listMail)
                {
                    string title = "EVN HANOI: Phân công ghi chỉ số";
                    string content = "Tổ/Đội thực hiện: " + item.MaDoi + " - " + item.TenDoi + "<br/>";
                    content += "Cán bộ Ghi chỉ số thực hiện: " + item.HoTen + "<br/>";
                    content += "Chi tiết các sổ được phân công: <br/>";

                    content += item.Content;
                    SendMail send = new SendMail();
                    send.Send_Email(item.Email, content, title, "");
                }

            }
        }
        public ActionResult Delete(int? ID_SOGCS)
        {
            try
            {
                if (ID_SOGCS == null)
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
                }
                // Lấy model T ra theo id
                var model = Uow.RepoBase<D_SOGCS>().GetOne(x => x.ID_SOGCS == ID_SOGCS);
                var idLichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(y => y.MA_SOGCS == model.MA_SOGCS).ID_LICHGCS;
                var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(z => z.ID_LICHGCS == idLichGcs);
                if (model != null)
                {
                    model.TRANG_THAI = "CL";
                    var ret = Uow.RepoBase<D_SOGCS>().Update(model);
                    var deleteLichgcs = Uow.RepoBase<GCS_LICHGCS>().Delete(lichGcs);

                    if (ret > 0 && deleteLichgcs > 0)
                    {
                        // Lưu Log
                        var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                        var logCategoryNs = "CMIS_HUYLAYSO";
                        var contentLogNs = "";
                        DateTime logDateNs = DateTime.Now;
                        var maBangKeLichNs = "";
                        var logStatusNs = "CL";
                        var lstCategoryLogNs =
                            Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryNs).LOG_CATEGORY_NAME;
                        var userNameNs = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                        var countThucHienNs = 1;
                        contentLogNs = userNameNs + " hủy " + lstCategoryLogNs + " " + lichGcs.TEN_SOGCS + " thành công";
                        WriteLog writeLogNs = new WriteLog(Uow);
                        writeLogNs.WriteLogGcs(logCategoryNs, lichGcs.ID_LICHGCS, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG,
                            lichGcs.NAM, contentLogNs, userId, logDateNs, maBangKeLichNs,
                            countThucHienNs, logStatusNs);
                        return Json(new { success = true, message = "Hủy nhận sổ " + model.MA_SOGCS + " thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { status = false, messenger = "Xảy ra lỗi: Không tìm thấy id" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Hủy nhận sổ không thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}
