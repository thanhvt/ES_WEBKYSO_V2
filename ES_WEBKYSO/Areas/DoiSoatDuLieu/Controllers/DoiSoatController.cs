using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
using ES_WEBKYSO.Areas.DoiSoatDuLieu.Models;
using ES_WEBKYSO.Areas.HeThongGiaoTiep.Models;

namespace ES_WEBKYSO.Areas.DoiSoatDuLieu.Controllers
{
    public class DoiSoatController : BaseController
    {
        //
        // GET: /DoiSoatDuLieu/DoiSoat/
        /// <summary>
        /// Compare form interface
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(DoiSoatModel model)
        {
            //Load data to dropdownlist, textbox
            var maDonviQuanly = new CommonUserProfile().MA_DVIQLY;
            ViewBag.MaDviQly = maDonviQuanly;

            var ky = Request.Params["Ky"];

            //ViewData["gcsLich"] = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => x.MA_DVIQLY == maDonviQuanly && x.STATUS_CNCS == "DCN").ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ID_LICHGCS.ToString(),
            //    Text = x.MA_SOGCS
            //}).ToList();

            //var lich = Uow.RepoBase<GCS_LICHGCS>().GetAll().ToList();
            //var bangKeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll().ToList();

            //var xxx = (from table1 in lich
            //           join table2 in bangKeLich
            //           on table1.ID_LICHGCS equals table2.ID_LICHGCS
            //           where table1.MA_DVIQLY.Equals(maDonviQuanly)
            //           select new
            //           {
            //               MASOGCS = table1.MA_SOGCS
            //           }).ToList();
            //ViewBag.ID = xxx.Select(
            //    x => new
            //        SelectListItem
            //    {
            //        Value = x.MASOGCS,
            //        Text = x.MASOGCS
            //    }
            //).ToList();
       //     ViewBag.ID = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => x.MA_DVIQLY == maDonviQuanly && x.ID_LICHGCS == bangKeLich.ID_LICHGCS).ToList().Select(x => new
       //SelectListItem
       //     {
       //         Value = x.MA_SOGCS,
       //         Text = x.MA_SOGCS
       //     }).ToList();

            ViewBag.ID = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => x.MA_DVIQLY == maDonviQuanly && x.STATUS_CNCS=="DCN").ToList().Select(x => new
            SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS
            }).ToList();

            //List<SelectListItem> CountryList = new List<SelectListItem>
            //    {
            //        new SelectListItem{Text="India",Value="1"},
            //        new SelectListItem{Text="United States",Value="2"},
            //        new SelectListItem{Text="Australia",Value="3"},
            //        new SelectListItem{Text="South Africa",Value="4"},
            //        new SelectListItem{Text="China",Value="5"}
            //    };

            //ViewBag.CountryList = CountryList;
            return View();
        }

        public ActionResult DoiTruongDoiSoat()
        {
            var maDonviQuanly = new CommonUserProfile().MA_DVIQLY;
            ViewBag.MaDviQly = maDonviQuanly;

            var ky = Request.Params["Ky"];

            ViewBag.ID = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => x.MA_DVIQLY == maDonviQuanly && x.STATUS_CNCS == "DCN").ToList().Select(x => new
             SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS
            }).ToList();

            //var lich = Uow.RepoBase<GCS_LICHGCS>().GetAll().ToList();
            //var bangKeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll().ToList();

            //var xxx = (from table1 in lich
            //           join table2 in bangKeLich
            //           on table1.ID_LICHGCS equals table2.ID_LICHGCS
            //           where table1.MA_DVIQLY.Equals(maDonviQuanly)
            //           select new
            //           {
            //               MASOGCS = table1.MA_SOGCS
            //           }).ToList();
            //ViewBag.ID = xxx.Select(
            //    x => new
            //        SelectListItem
            //    {
            //        Value = x.MASOGCS,
            //        Text = x.MASOGCS
            //    }
            //).ToList();
            return View();
        }
        public ActionResult NhanVienDoiSoat()
        {
            var maDonviQuanly = new CommonUserProfile().MA_DVIQLY;
            ViewBag.MaDviQly = maDonviQuanly;
            var lstBkl = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll().ToList();
            var lstLich = Uow.RepoBase<GCS_LICHGCS>().GetAll().ToList();

            ViewBag.ID = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => x.MA_DVIQLY == maDonviQuanly && x.STATUS_CNCS == "DCN"
                        && x.STATUS_NVK == "NVCK").ToList().Select(x => new
            SelectListItem
            {
                Value = x.MA_SOGCS,
                Text = x.MA_SOGCS
            }).ToList();

            ////lstMaSo.Any(x => x == o.MA_QUYEN)

            //var query = (from emp2 in lstBkl
            //             join emp1 in lstLich
            //             on emp2.ID_LICHGCS equals emp1.ID_LICHGCS into allEmployees
            //             from result in allEmployees.DefaultIfEmpty()
            //             select new
            //             {
            //                 ID = result.MA_SOGCS
            //             }).ToList();
            //var questionGroups = query.Select(x => x.ID)
            //                  .Where(x => x != "")
            //                  .DefaultIfEmpty()
            //                  .Distinct().ToList();
            //var aaa = Uow.RepoBase<GCS_LICHGCS>().GetAll(x => !questionGroups.Any(y => y == x.MA_SOGCS));


            //var query = from city in lstBkl
            //            where !lstLich.Any(country => country.ID_LICHGCS == city.ID_LICHGCS)
            //            select city;

            //            var progy = (
            //        from u in lstBkl
            //        join b in lstLich
            //on u.ID_LICHGCS equals b.ID_LICHGCS into yG
            //        from y1 in yG.DefaultIfEmpty()
            //        where y1 != null
            //        select u.ID_LICHGCS
            //       ).ToList();

            //var query1 = from p in lstBkl
            //            join pl in lstLich
            //                on p.ID_LICHGCS equals pl.ID_LICHGCS into pp
            //            from pl in pp.DefaultIfEmpty()
            //            where pl != null
            //            select pl;
            //var vv = query1.Select(x => x.MA_SOGCS)
            //                  .Where(x => x != "")
            //                  .DefaultIfEmpty()
            //                  .Distinct().ToList();

            //ViewBag.ID = aaa.Select(x => new
            //  SelectListItem
            //{
            //    Value = x.MA_SOGCS,
            //    Text = x.MA_SOGCS
            //}).ToList();
            return View();
        }

        /// <summary>
        /// Get data to Datatable
        /// </summary>
        /// <param name="findModel">Data param from Compare form</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJson(DoiSoatModel findModel)
        {
            var maSos = Request.Params["MaSos[]"];
            maSos = maSos ?? "";
            var maSoSplit = maSos.Split(',');
            if (maSos == "")
            {
                maSoSplit = new string[] { };
            }
            findModel.MaSos = maSoSplit.ToList();
            var paging = Request.Params.ToPaging("MA_DVIQLY");
            var data =
                Uow.RepoBase<GCS_CHISO_HHU>().ManagerGetAllForIndex(findModel, paging.OrderKey, ref paging).OrderBy(c => c.MA_KHANG).ThenBy(c => c.LOAI_BCS).ToList();
            paging.data = data;
            var lstChuaDs = Uow.RepoBase<GCS_CHISO_HHU>().ManagerGetAllForIndex(findModel).Count(x => x.STR_CHECK_DSOAT != "CHUA_DOI_SOAT");
            var lstDsDat = Uow.RepoBase<GCS_CHISO_HHU>().ManagerGetAllForIndex(findModel).Count(x => x.STR_CHECK_DSOAT == "CHECK");
            var lstDsKd = Uow.RepoBase<GCS_CHISO_HHU>().ManagerGetAllForIndex(findModel).Count(x => x.STR_CHECK_DSOAT == "UNCHECK");
            var lstCtDt = Uow.RepoBase<GCS_CHISO_HHU>().ManagerGetAllForIndex(findModel).Count(x => x.STR_CHECK_DSOAT == "CTO_DTU");
            //Paging in Datatable
            return Json(new
            {
                data = paging.data,
                draw = paging.draw,
                Key = paging.Key,
                OrderDirection = paging.OrderDirection,
                OrderKey = paging.OrderKey,
                recordsFiltered = paging.recordsFiltered,
                recordsTotal = paging.recordsTotal,
                Skip = paging.Skip,
                Take = paging.Take,

                //Count Total data after searching
                Total = data.Count(),
                DaDoiSoat = lstChuaDs,
                DoiSoatDat = lstDsDat,
                DoiSoatKhongDat = lstDsKd,
                CongToDienTu = lstCtDt

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save function Compare
        /// </summary>
        /// <param name="idr">id rows table Compare form</param>
        /// <param name="idc">ids checkbox in rows</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SaveDoiSoat(int idr, string idc)
        {
            try
            {
                if (idr != 0)
                {
                    var gcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().GetOne(x => x.ID == idr);
                    if (gcsHhu == null)
                    {
                        return Json(new { success = false, message = "Mã điểm đo không tồn tại trong cơ sở dữ liệu!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (gcsHhu.ANH_GCS == "")
                    {
                        return Json(new { success = false, message = "Mã điểm đo này chưa có ảnh chỉ số, chưa thể đối soát!" },
                           JsonRequestBehavior.AllowGet);
                    }
                    var bcsKt = gcsHhu.LOAI_BCS;

                    var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_SOGCS == gcsHhu.MA_QUYEN).ID_LICHGCS;
                    var gcsBangKeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll(o => o.ID_LICHGCS == gcsLichgcs && (o.MA_LOAIBANGKE == "BKCS" || o.MA_LOAIBANGKE == "SLBT" || o.MA_LOAIBANGKE == "TTBT"));
                    if (gcsBangKeLich.Count() != 0)
                    {
                        return Json(new { success = false, message = "Không thể thực hiện được chức năng đối soát. Sổ này đã được ký!" },
                            JsonRequestBehavior.AllowGet);
                    }

                    //If checked cell "Đạt". Save to STR_CHECK_DSOAT = 'CHECK'
                    if (idc == "a") gcsHhu.STR_CHECK_DSOAT = "CHECK";
                    //If checked cell "Không đạt". Save to STR_CHECK_DSOAT = 'UNCHECK'
                    if (idc == "b") gcsHhu.STR_CHECK_DSOAT = "UNCHECK";
                    //If checked cell "Công tơ điện tử". Save to STR_CHECK_DSOAT = 'CTO_DTU'
                    if (idc == "c" && bcsKt == "KT")
                    {
                        gcsHhu.STR_CHECK_DSOAT = "CHUA_DOI_SOAT";
                        return Json(new { success = false, message = "Đây không phải là công tơ điện tử" },
                            JsonRequestBehavior.AllowGet);
                    }
                    else if( idc == "c")
                    {
                        gcsHhu.STR_CHECK_DSOAT = "CTO_DTU";
                    }
                    //If no check. Save to STR_CHECK_DSOAT = 'CHUA_DOI_SOAT'
                    if (idc == "cbk") gcsHhu.STR_CHECK_DSOAT = "CHUA_DOI_SOAT";

                    var checkGcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().Update(gcsHhu);
                    if (checkGcsHhu != 1)
                    {
                        return Json(new { success = false, message = "Đối soát không thành công!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đối soát không thành công! " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Đối soát thành công!" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function Compare Automatic
        /// </summary>
        /// <param name="madv">Mã đơn vị</param>
        /// <param name="maso">Mã sổ, mã quyển</param>
        /// <param name="ky">Kỳ</param>
        /// <param name="thang">Tháng</param>
        /// <param name="nam">Năm</param>
        /// <returns></returns>
        public ActionResult DoiSoatTuDong(string madv, List<string> maso, int ky, int thang, int nam)
        {
            var maSos = Request.Params["MaSo[]"];
            maSos = maSos ?? "";
            var maSoSplit = maSos.Split(',');
            if (maSos == "")
            {
                maSoSplit = new string[] { };
            }
            var lstx = maSoSplit.ToList();
            try
            {
                var gcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == madv && lstx.Any(o => o == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && x.STR_CHECK_DSOAT == "CHUA_DOI_SOAT").ToList();

               
                //Browse listings
                foreach (var lst in gcsHhu)
                {
                    var gcsLichgcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_SOGCS == lst.MA_QUYEN).ID_LICHGCS;
                    var gcsBangKeLich = Uow.RepoBase<GCS_BANGKE_LICH>().GetAll(o => o.ID_LICHGCS == gcsLichgcs && (o.MA_LOAIBANGKE == "BKCS" || o.MA_LOAIBANGKE == "SLBT" || o.MA_LOAIBANGKE == "TTBT"));
                    if (gcsBangKeLich.Count() != 0)
                    {
                        return Json(new { success = false, message = "Không thể thực hiện được chức năng tự động đối soát. Sổ này đã được ký!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    //If is electronic meter then save to STR_CHECK_DSOAT = 'CTO_DTU'
                    if (lst.LOAI_BCS != "KT")
                    {
                        lst.STR_CHECK_DSOAT = "CTO_DTU";
                        var checkGcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().Update(lst);    
                        if (checkGcsHhu != 1)
                        {
                            return Json(new { success = false, message = "Tự động đối soát không thành công!" },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var m = System.Web.HttpContext.Current.Server.MapPath("~/");
                        //If is not electronic meter and no image then save to STR_CHECK_DSOAT = 'UNCHECK'. If have image is do nothing
                        if (lst.ANH_GCS == "" || lst.ANH_GCS == (m + "Images\\NoImage.png"))
                        {
                            lst.STR_CHECK_DSOAT = "UNCHECK";
                            var checkGcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().Update(lst);
                            if (checkGcsHhu != 1)
                            {
                                return Json(new { success = false, message = "Tự động đối soát không thành công!" },
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    // Lưu Log
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    //var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    var logCategoryId = "DOISOAT_TUDONG";
                    var contentLog = "";
                    DateTime logDate = DateTime.Now;
                    var maBangKeLich = "";
                    var logStatus = "DDS";
                    var lstCategoryLog =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                    var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    var countThucHien = 1;
                    contentLog = userName + " " + lstCategoryLog + " sổ " + lst.MA_QUYEN + " thành công";
                    WriteLog writeL = new WriteLog(Uow);
                    writeL.WriteLogGcs(logCategoryId, gcsLichgcs, lst.MA_QUYEN, ky, thang, nam, contentLog, userId, logDate, maBangKeLich,
                        countThucHien, logStatus);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Đối soát không thành công! " + e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Đối soát thành công!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XuatGhiLai(string madv, string maSo, int ky, int thang, int nam)
        {
            try
            {
                var maSos = Request.Params["maSo[]"];
                maSos = maSos ?? "";
                var maSoSplit = maSos.Split(',');
                if (maSos == "")
                {
                    maSoSplit = new string[] { };
                }
                var lstMaso = maSoSplit.ToList();
                var lstGcsHhu = Uow.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == madv
                                                                          && lstMaso.Any(y => y == x.MA_QUYEN)
                                                                          && x.KY == ky
                                                                          && x.THANG == thang
                                                                          && x.NAM == nam
                                                                          && (x.STR_CHECK_DSOAT == "UNCHECK" 
                                                                          || x.STR_CHECK_DSOAT == "CHUA_DOI_SOAT")).ToList();
                if (lstGcsHhu.Count == 0)
                {
                    return Json(new { success = false,
                                    message = "Sổ này đã được đối soát, không có khách hàng nào đối soát không đạt hoặc chưa đối soát!" },
                                    JsonRequestBehavior.AllowGet);
                }
                foreach (var lstTraLai in lstGcsHhu)
                {
                    lstTraLai.STR_CHECK_DSOAT = "CHUA_DOI_SOAT";
                    Uow.RepoBase<GCS_CHISO_HHU>().Update(lstTraLai);
                }
                var lstLichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(y => lstMaso.Any(z => z == y.MA_SOGCS));
                if (lstLichGcs != null)
                {
                    lstLichGcs.STATUS_CNCS = "CCN";
                    Uow.RepoBase<GCS_LICHGCS>().Update(lstLichGcs);

                    // Lưu Log
                    var userId = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserId;
                    //var lichGcs = Uow.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                    var logCategoryId = "XGL";
                    var contentLog = "";
                    DateTime logDate = DateTime.Now;
                    var maBangKeLich = "";
                    var logStatus = "DX";
                    var lstCategoryLog =
                        Uow.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                    var userName = Uow.RepoBase<UserProfile>().GetOne(x => x.UserName == User.Identity.Name).UserName;
                    var countThucHien = 1;
                    contentLog = userName + " " + lstCategoryLog + " sổ " + lstLichGcs.TEN_SOGCS + " thành công";
                    WriteLog writeL = new WriteLog(Uow);
                    writeL.WriteLogGcs(logCategoryId, lstLichGcs.ID_LICHGCS, maSo, ky, thang, nam, contentLog, userId, logDate, maBangKeLich,
                        countThucHien, logStatus);
                }
                return Json(new { success = true, message = "Xuất ghi lại thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Xuất ghi lại không thành công! " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="madv"></param>
        /// <param name="maSo"></param>
        /// <param name="ky"></param>
        /// <param name="thang"></param>
        /// <param name="nam"></param>
        /// <param name="loaibk"></param>
        /// <param name="nguoids"></param>
        /// <returns></returns>
        public ActionResult PrintBangKe(string madv, int ky, int thang, int nam, string loaibk, string nguoids)
        {
            ViewBag.Title = "In bảng kê";
            try
            {
                var maSos = Request.Params["maSo[]"];
                maSos = maSos ?? "";
                var maSoSplit = maSos.Split(',');
                if (maSos == "")
                {
                    maSoSplit = new string[] { };
                }
                string[] maSo = maSoSplit;

                ReportHelper reportHelper = new ReportHelper(Uow);
                switch (loaibk)
                {
                    case "01":
                        var sourceDsKhKca = reportHelper.GetReportDsKhKhongChupAnh(madv, maSo, ky, thang, nam, loaibk,
                            nguoids);
                        return sourceDsKhKca == null
                            ? Json(new {success = false, message = "Không có dữ liệu đối soát!"},
                                JsonRequestBehavior.AllowGet)
                            : Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    case "02":
                        var sourceDsKhKds = reportHelper.GetReportDsKhKhongDuocDs(madv, maSo, ky, thang, nam, loaibk,
                            nguoids);
                        return sourceDsKhKds == null
                            ? Json(new {success = false, message = "Không có dữ liệu đối soát!"},
                                JsonRequestBehavior.AllowGet)
                            : Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    case "03":
                        var sourceDsKhDds = reportHelper.GetReportDsKhDaDs(madv, maSo, ky, thang, nam, loaibk, nguoids);
                        return sourceDsKhDds == null
                            ? Json(new {success = false, message = "Không có dữ liệu đối soát!"},
                                JsonRequestBehavior.AllowGet)
                            : Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    case "04":
                        var sourceDsKhDsDat = reportHelper.GetReportDsKhDsDat(madv, maSo, ky, thang, nam, loaibk, nguoids);
                        return sourceDsKhDsDat == null
                            ? Json(new {success = false, message = "Không có dữ liệu đối soát!"},
                                JsonRequestBehavior.AllowGet)
                            : Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    case "05":
                        var sourceDsKhDsKhongDat = reportHelper.GetReportDsKhDsKhongDat(madv, maSo, ky, thang, nam, loaibk,
                            nguoids);
                        return sourceDsKhDsKhongDat == null
                            ? Json(new {success = false, message = "Không có dữ liệu đối soát!"},
                                JsonRequestBehavior.AllowGet)
                            : Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    case "06":
                    case "07":
                    case "08":
                    case "10":
                    case "12":
                        var sourceGet_SLBT = reportHelper.get_SLBT(ky, thang, nam, maSo[0]);
                        return sourceGet_SLBT == null
                            ? Json(new { success = false, message = "Không có dữ liệu đối soát!" },
                                JsonRequestBehavior.AllowGet)
                            : Json(new { success = true, message = "OK" }, JsonRequestBehavior.AllowGet);
                    //case "07":
                    //    return null;
                    //case "08":
                    //    return null;
                    case "09":
                        var sourceGetCHISO_PMax = reportHelper.GetCHISO_PMax(ky, thang, nam, maSo[0]);
                        return sourceGetCHISO_PMax == null
                            ? Json(new { success = false, message = "Không có dữ liệu đối soát!" },
                                JsonRequestBehavior.AllowGet)
                            : Json(new { success = true, message = "OK" }, JsonRequestBehavior.AllowGet);
                        
                    //case "10":
                    //    return null;
                    //case "11":
                    //    return null;
                    //case "12":
                    //    return null;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "In bảng kê không thành công! " + ex.Message}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "In bảng kê thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}
