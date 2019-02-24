using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;
using System.Web.Mvc;
using System.Data;
using System.Web.Hosting;
using System.Xml;
using ES_WEBKYSO.Common;
using System.Threading;
using System.Globalization;
using ES_WEBKYSO.ModelParameter;
using System.IO;
using System.Reflection;

namespace ES_WEBKYSO.Common
{
    public class ReportHelper : Controller
    {
        private readonly UnitOfWork _repo;
        public ReportHelper(UnitOfWork repo)
        {
            _repo = repo;
        }

        public class ListtoDataTable
        {
            //Read data from List add to Table
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties by using reflection   
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names  
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {

                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
        }
        public DataTable GetReportDsKhKhongChupAnh(string maDonVi, string[] maSo, int ky, int thang, int nam, string loaiBangKe, string nguoiDoiSoat)
        {
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            var gcsHhu = _repo.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == maDonVi
                                                                       && maSo.Any(y => y == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && (x.ANH_GCS == "" 
                                                                        || x.ANH_GCS == m + @"Images\NoImage.png")).ToList();
            if (gcsHhu.Count == 0)
            {
                return null;
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(gcsHhu);
            return dt;
        }
        public DataTable GetReportDsKhKhongDuocDs(string maDonVi, string[] maSo, int ky, int thang, int nam, string loaiBangKe, string nguoiDoiSoat)
        {
            var gcsHhu = _repo.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == maDonVi
                                                                       && maSo.Any(y => y == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && x.STR_CHECK_DSOAT == "CHUA_DOI_SOAT").ToList();
            if (gcsHhu.Count == 0)
            {
                return null;
            }
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            foreach (var lstImage in gcsHhu)
            {
                if (string.IsNullOrEmpty(lstImage.ANH_GCS))
                {
                    lstImage.ANH_GCS = m + @"Images\NoImage.png";
                    //var path = lstImage.ANH_GCS;
                    _repo.RepoBase<GCS_CHISO_HHU>().Update(lstImage);
                }
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(gcsHhu);
            return dt;
        }
        public DataTable GetReportDsKhDaDs(string maDonVi, string[] maSo, int ky, int thang, int nam, string loaiBangKe, string nguoiDoiSoat)
        {
            var gcsHhu = _repo.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == maDonVi
                                                                       && maSo.Any(y => y == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && (x.STR_CHECK_DSOAT != "CHUA_DOI_SOAT")
                                                                       ).ToList();
            if (gcsHhu.Count == 0)
            {
                return null;
            }
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            foreach (var lstImage in gcsHhu)
            {
                if (string.IsNullOrEmpty(lstImage.ANH_GCS))
                {
                    lstImage.ANH_GCS = m + @"Images\NoImage.png";
                    var path = lstImage.ANH_GCS;
                    _repo.RepoBase<GCS_CHISO_HHU>().Update(lstImage);
                }
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(gcsHhu);
            return dt;
        }
        public DataTable GetReportDsKhDsDat(string maDonVi, string[] maSo, int ky, int thang, int nam, string loaiBangKe, string nguoiDoiSoat)
        {
            var gcsHhu = _repo.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == maDonVi
                                                                       && maSo.Any(y => y == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && (x.STR_CHECK_DSOAT == "CHECK")
                                                                       ).ToList();
            if (gcsHhu.Count == 0)
            {
                return null;
            }
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            foreach (var lstImage in gcsHhu)
            {
                if (string.IsNullOrEmpty(lstImage.ANH_GCS))
                {
                    lstImage.ANH_GCS = m + @"Images\NoImage.png";
                    var path = lstImage.ANH_GCS;
                    _repo.RepoBase<GCS_CHISO_HHU>().Update(lstImage);
                }
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(gcsHhu);
            return dt;
        }
        public DataTable GetReportDsKhDsKhongDat(string maDonVi, string[] maSo, int ky, int thang, int nam, string loaiBangKe, string nguoiDoiSoat)
        {
            var gcsHhu = _repo.RepoBase<GCS_CHISO_HHU>().GetAll(x => x.MA_DVIQLY == maDonVi
                                                                       && maSo.Any(y => y == x.MA_QUYEN)
                                                                       && x.KY == ky
                                                                       && x.THANG == thang
                                                                       && x.NAM == nam
                                                                       && x.STR_CHECK_DSOAT == "UNCHECK"
                                                                       ).ToList();
            if (gcsHhu.Count == 0)
            {
                return null;
            }
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            foreach (var lstImage in gcsHhu)
            {
                if (string.IsNullOrEmpty(lstImage.ANH_GCS))
                {
                    lstImage.ANH_GCS = m + @"Images\NoImage.png";
                    var path = lstImage.ANH_GCS;
                    _repo.RepoBase<GCS_CHISO_HHU>().Update(lstImage);
                }
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(gcsHhu);
            return dt;
        }

        public DataTable getReportBkcs(int ID_LISHGCS)
        {

            string fileXML = _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).FILE_XML;

            string filePath = "";
            //filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/TemplateFile/" + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + @"/"), fileXML);
            filePath = Utility.getXMLPath() + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + @"/" + fileXML;
            var ds = new DataSet();
            // filePath = "~/TemplateFile/cochau1_161207.xml";
            ds.ReadXml(filePath, XmlReadMode.InferSchema);
            DataTable dtTemp = Convert2TemplateDataTable(ds.Tables[0], System.IO.Path.GetFileName(filePath));
            return dtTemp;
        }
        public string getNgayGcs(int ID_LISHGCS)
        {
            GCS_LICHGCS gcs = _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS);
            string Ngay = gcs.NGAY_GHI.ToString();
            string Thang = gcs.THANG.ToString();
            string Nam = gcs.NAM.ToString();
            string NgayGCS = Ngay + "/" + Thang + "/" + Nam;
            return NgayGCS;
        }
        public string getMaQuyen(int ID_LISHGCS)
        {
            GCS_LICHGCS gcs = _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS);
            return gcs.MA_SOGCS;
        }
        public DataTable get_DSCT(int ID_LISHGCS)
        {

            string fileXML = _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).FILE_XML;

            string filePath = "";
            //filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/TemplateFile/" + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + @"/"), fileXML);
            filePath = Utility.getXMLPath() + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + @"/" + fileXML;
            var ds = new DataSet();
            // filePath = "~/TemplateFile/cochau1_161207.xml";
            ds.ReadXml(filePath, XmlReadMode.InferSchema);
            DataTable dtTemp = Convert2TemplateDataTable(ds.Tables[0], System.IO.Path.GetFileName(filePath));
            return dtTemp;
        }
        public DataTable get_SLBT(int ID_LISHGCS)
        {

            string fileXML = _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).FILE_XML;

            string filePath = "";
            //filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/TemplateFile/" + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + "/"), fileXML);
            filePath = Utility.getXMLPath() + _repo.RepoBase<GCS_LICHGCS>().GetOne(o => o.ID_LICHGCS == ID_LISHGCS).MA_DVIQLY.Trim() + @"/" + fileXML;
            var ds = new DataSet();
            ds.ReadXml(filePath, XmlReadMode.InferSchema);
            DataTable dtTemp = Convert2TemplateDataTable(ds.Tables[0], System.IO.Path.GetFileName(filePath));
            return dtTemp;
        }
        public DataTable getSanLuongBatThuong(string filePath)
        {
            try
            {
                // var mapPath = Server.MapPath("~/TemplateFile/" + maso + "-" + nam + "-" + thang + "-" + ky + ".xml");
                DataTable datatable = new DataTable();
                datatable.ReadXml(filePath);
                foreach (DataRow row in datatable.Rows)
                {
                    int cs_moi = int.Parse(row["CS_MOI"].ToString());
                    int cs_cu = int.Parse(row["CS_CU"].ToString());
                    float hs_nhan = float.Parse(row["HSN"].ToString());
                    float sl_batthuong = (cs_moi - cs_cu) * hs_nhan;

                }
                return datatable;
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
                return null;
            }


        }

        public DataTable get_SLBT(string xmlFile)
        {
            var ds = new DataSet();

            ds.ReadXml(xmlFile, XmlReadMode.InferSchema);
            var dtPrint = CreateBlankTemplateDataTable();
            DataTable dtTemp = null;
            ConfigInfo config = new ConfigInfo();
            dtTemp = GetDtSLBT(ds.Tables[0], xmlFile, config);
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                //Thêm vào dtPrint
                dtPrint.Merge(dtTemp.Copy());
            }
            return dtTemp;
        }


        public DataTable GetDtSLBT(DataTable dtData, string TEN_FILE, ConfigInfo config)
        {
            //DataRow[] drArr = ds.Tables[0].Select(""
            //    + "(SL_CU > 0 AND (SL_MOI+SL_THAO) > SL_CU AND ((SL_MOI-SL_CU+SL_THAO)/SL_CU)>=" + ((decimal)config.SLBT_VUOT_MUC / (decimal)100) + ")"
            //    + " OR "
            //    + "(SL_CU > 0 AND (SL_MOI+SL_THAO) < SL_CU AND ((SL_CU-SL_MOI-SL_THAO)/SL_CU)>=" + ((decimal)config.SLBT_DUOI_MUC / (decimal)100) + ")"
            //    + " OR "
            //    + "(SL_CU = 0 AND (SL_MOI+SL_THAO) >= " + ((decimal)config.SLBT_VUOT_MUC / (decimal)100) + ")"
            //);
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);
            DataTable dtSlbt = dtNew.Clone();
            DataRow dr = null;
            decimal sl_moi = 0;
            decimal sl_cu = 0;
            decimal sl_thao = 0;
            string loai_bcs = "";

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                dr = dtNew.Rows[i];
                sl_moi = decimal.Parse(dr["SL_MOI"].ToString(), CultureInfo.CurrentCulture);
                sl_cu = decimal.Parse(dr["SL_CU"].ToString(), CultureInfo.CurrentCulture);
                sl_thao = decimal.Parse(dr["SL_THAO"].ToString(), CultureInfo.CurrentCulture);
                loai_bcs = dr["LOAI_BCS"].ToString().Trim();

                //if (loai_bcs == "VC")
                //{
                //    continue;
                //}

                if (config.ENABLE_SLBT_PERCENT)
                {
                    if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) / sl_cu > ((decimal)config.SLBT_VUOT_MUC / 100))
                        || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && ((sl_cu - sl_moi - sl_thao) / sl_cu) > ((decimal)config.SLBT_DUOI_MUC / 100))
                        || (sl_cu <= 0 && (sl_moi + sl_thao) > ((decimal)config.SLBT_VUOT_MUC / 100)))
                    {
                        dtSlbt.ImportRow(dr);
                        continue;
                    }
                }

                if (config.ENABLE_SLBT_KWH)
                {
                    if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) > config.SLBT_VUOT_MUC_KWH)
                        || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && (sl_cu - sl_moi - sl_thao) > config.SLBT_DUOI_MUC_KWH)
                        || (sl_cu <= 0 && (sl_moi + sl_thao) > config.SLBT_VUOT_MUC_KWH))
                    {
                        dtSlbt.ImportRow(dr);
                        continue;
                    }
                }

            }
            return dtSlbt;
        }
        public DataTable GetDtTinhTrangBatThuong(DataTable dtData, string TEN_FILE)
        {
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);
            DataTable dtSLBT = dtNew.Clone();
            DataRow dr = null;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                dr = dtNew.Rows[i];
                if (dr["TTR_MOI"] != null && dr["TTR_MOI"].ToString().Trim().Length > 0)
                {
                    dtSLBT.ImportRow(dr);
                }
            }
            return dtSLBT;
        }
        public DataTable Convert2TemplateDataTable(DataTable dtData, string TEN_SO)
        {
            try
            {
                //dtData.Locale = Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
                DataTable dtTemplate = CreateBlankTemplateDataTable();
                DataTable dtNew = SetCellValue(dtData, dtTemplate, TEN_SO);
                return dtNew;
            }
            catch (Exception ex)
            {
                string result = ex.Message;
                return null;
            }

        }
        public DataTable SetCellValue(DataTable dtData, DataTable dtNew, string TEN_SO)
        {
            DataRow drNew = null;
            decimal number = 0;
            int THANG = 0;
            int i = 1;
            try
            {
                for (int row = 0; row < dtData.Rows.Count; row++)
                {
                    drNew = dtNew.NewRow();
                    // gán giá trị của các cột trong dtData cho dtNew
                    for (int col = 0; col < gcs_col_all.Length; col++)
                    {
                        #region code cũ

                        //if (gcs_col_all[col].col_name == "CHU_KY" && dtData.Columns.Contains("CHU_KY_BASE64") && dtData.Rows[row]["CHU_KY_BASE64"] != null && dtData.Rows[row]["CHU_KY_BASE64"].ToString().Trim().Length > 0)
                        //{
                        //    #region Chuyển dữ liệu cột CHU_KY_BASE64 sang kiểu byte[] rồi gán cho cột CHU_KY
                        //    try
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = Convert.FromBase64String(dtData.Rows[row]["CHU_KY_BASE64"].ToString());
                        //    }
                        //    catch
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = null;
                        //    }
                        //    #endregion
                        //}
                        //else if (gcs_col_all[col].col_name == "CHU_KY_BASE64" && dtData.Columns.Contains("CHU_KY") && dtData.Rows[row]["CHU_KY"] != null)
                        //{
                        //    #region Chuyển dữ liệu kiểu byte[] của cột CHU_KY sang kiểu base64 rồi gán cho cột CHU_KY_BASE64
                        //    try
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = Convert.ToBase64String((byte[])dtData.Rows[row]["CHU_KY"]);
                        //    }
                        //    catch
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = null;
                        //    }
                        //    #endregion
                        //}
                        //else if (gcs_col_all[col].col_name == "HINH_ANH" && dtData.Columns.Contains("HINH_ANH_BASE64") && dtData.Rows[row]["HINH_ANH_BASE64"] != null && dtData.Rows[row]["HINH_ANH_BASE64"].ToString().Trim().Length > 0)
                        //{
                        //    #region Chuyển dữ liệu cột HINH_ANH_BASE64 sang kiểu byte[] rồi gán cho cột HINH_ANH
                        //    try
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = Convert.FromBase64String(dtData.Rows[row]["HINH_ANH_BASE64"].ToString());
                        //    }
                        //    catch
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = null;
                        //    }
                        //    #endregion
                        //}
                        //else if (gcs_col_all[col].col_name == "HINH_ANH_BASE64" && dtData.Columns.Contains("HINH_ANH") && dtData.Rows[row]["HINH_ANH"] != null)
                        //{
                        //    #region Chuyển dữ liệu kiểu byte[] của cột HINH_ANH sang kiểu base64 rồi gán cho cột HINH_ANH_BASE64
                        //    try
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = Convert.ToBase64String((byte[])dtData.Rows[row]["HINH_ANH"]);
                        //    }
                        //    catch
                        //    {
                        //        drNew[gcs_col_all[col].col_name] = null;
                        //    }
                        //    #endregion
                        //}

                        #endregion

                        if (gcs_col_all[col].col_name == "STR_CHECK_DSOAT") // else if
                        {
                            i = 2;

                            #region Gán dữ liệu đối soát

                            try
                            {
                                if (dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "CHECK" ||
                                    dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "UNCHECK" ||
                                    dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "CTO_DTU")
                                {
                                    drNew[gcs_col_all[col].col_name] = dtData.Rows[row]["STR_CHECK_DSOAT"].ToString();
                                }
                                else
                                {
                                    drNew[gcs_col_all[col].col_name] = "CHUA_DOI_SOAT";
                                }
                            }
                            catch
                            {
                                drNew[gcs_col_all[col].col_name] = "CHUA_DOI_SOAT";
                            }

                            #endregion
                        }
                        else if (IsNumber(gcs_col_all[col].col_type))
                        {
                            i = 3;

                            #region kiểu cột là number

                            if (gcs_col_all[col].col_name == "CS_MOI" || gcs_col_all[col].col_name == "PMAX" || gcs_col_all[col].col_name == "SL_MOI")
                            {
                                if (dtData.Columns.Contains(gcs_col_all[col].col_name)
                                    && dtData.Rows[row][gcs_col_all[col].col_name] != null
                                    && IsNumberValueNew(dtData.Rows[row][gcs_col_all[col].col_name].ToString(), out number))
                                {
                                    drNew[gcs_col_all[col].col_name] = number.ToString("0.##",
                                        CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    drNew[gcs_col_all[col].col_name] = 0;
                                }
                            }
                            else
                            {
                                // nếu giá trị cột đó != null và kiểu là number
                                if (dtData.Columns.Contains(gcs_col_all[col].col_name)
                                    && dtData.Rows[row][gcs_col_all[col].col_name] != null
                                    && IsNumberValue(dtData.Rows[row][gcs_col_all[col].col_name].ToString(), out number))
                                {
                                    if (gcs_col_all[col].col_name == "X" || gcs_col_all[col].col_name == "Y")
                                    {
                                        drNew[gcs_col_all[col].col_name] = number.ToString("0.####################",
                                            CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        drNew[gcs_col_all[col].col_name] = number.ToString("0.###",
                                            CultureInfo.CurrentCulture);
                                    }
                                }
                                else
                                {
                                    drNew[gcs_col_all[col].col_name] = 0;
                                }
                            }



                            #endregion
                        }
                        else if (gcs_col_all[col].col_type == typeof(DateTime))
                        {
                            i = 4;

                            #region kiểu cột là Datetime

                            if (dtData.Columns.Contains(gcs_col_all[col].col_name) &&
                                dtData.Rows[row][gcs_col_all[col].col_name] != null)
                            {
                                //Kiểm tra tháng gcs và tháng trong cột ngay_moi
                                THANG = Convert.ToInt32(dtData.Rows[row]["THANG"].ToString());
                                DateTime? ngay_moi = null;
                                try
                                {
                                    ngay_moi = (DateTime?)Convert.ToDateTime(dtData.Rows[row]["NGAY_MOI"].ToString());
                                    if (dtData.Rows[row][gcs_col_all[col].col_name] != null &&
                                        dtData.Rows[row][gcs_col_all[col].col_name].ToString().Trim().Length > 0)
                                    {
                                        drNew[gcs_col_all[col].col_name] = dtData.Rows[row][gcs_col_all[col].col_name];
                                    }
                                    else if (THANG == ngay_moi.Value.Month &&
                                             (gcs_col_all[col].col_name == "NGAY_CU" ||
                                              gcs_col_all[col].col_name == "NGAY_MOI")) // giá trị ngày đúng định dạng
                                    {
                                        drNew[gcs_col_all[col].col_name] =
                                            dtData.Rows[row][gcs_col_all[col].col_name];
                                    }
                                    else // giá trị ngày sai định dạng
                                    {
                                        throw new Exception();
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        if (gcs_col_all[col].col_name == "NGAY_CU" ||
                                            gcs_col_all[col].col_name == "NGAY_MOI")
                                        {
                                            // sửa lại định dạng ngày và cập nhật vào drNew
                                            try
                                            {
                                                drNew["NGAY_CU"] =
                                                    DateTime.ParseExact(dtData.Rows[row]["NGAY_CU"].ToString(),
                                                        "dd/MM/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                                drNew["NGAY_MOI"] =
                                                    DateTime.ParseExact(dtData.Rows[row]["NGAY_MOI"].ToString(),
                                                        "dd/MM/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                            catch
                                            {
                                                drNew["NGAY_CU"] =
                                                    DateTime.ParseExact(dtData.Rows[row]["NGAY_CU"].ToString(),
                                                        "d/M/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                                drNew["NGAY_MOI"] =
                                                    DateTime.ParseExact(dtData.Rows[row]["NGAY_MOI"].ToString(),
                                                        "d/M/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                drNew[gcs_col_all[col].col_name] =
                                                    DateTime.ParseExact(
                                                        dtData.Rows[row][gcs_col_all[col].col_name].ToString(),
                                                        "dd/MM/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                            catch
                                            {
                                                drNew[gcs_col_all[col].col_name] =
                                                    DateTime.ParseExact(
                                                        dtData.Rows[row][gcs_col_all[col].col_name].ToString(),
                                                        "d/M/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        drNew[gcs_col_all[col].col_name] = DateTime.Parse("1753-01-01");
                                        // nếu giá trị cột = null thì gán giá trị thời gian nhỏ nhất
                                    }

                                }
                            }
                            else
                                drNew[gcs_col_all[col].col_name] = DBNull.Value;

                            #endregion
                        }
                        else if (dtData.Columns.Contains(gcs_col_all[col].col_name))
                        {
                            i = 5;

                            #region cột có trong bảng dtData

                            try
                            {
                                drNew[gcs_col_all[col].col_name] = dtData.Rows[row][gcs_col_all[col].col_name];
                            }
                            catch
                            {
                                try
                                {
                                    #region kiểu cột là System.Byte[]

                                    drNew[gcs_col_all[col].col_name] =
                                        Convert.FromBase64String(dtData.Rows[row][gcs_col_all[col].col_name].ToString());

                                    #endregion
                                }
                                catch
                                {
                                    drNew[gcs_col_all[col].col_name] = null;
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            // cột ko có trong bảng dtData
                            drNew[gcs_col_all[col].col_name] = null;
                        }
                    }
                    i = 6;
                    if (drNew["TEN_FILE"] == null || drNew["TEN_FILE"].ToString().Trim().Length == 0)
                        drNew["TEN_FILE"] = TEN_SO;

                    drNew["SL_MOI"] = TinhSanLuong(drNew); // Tính lại sản lượng của công tơ
                    // gán giá trị cho các cột ext
                    drNew["HIEU_SO"] = TinhHieuSo(drNew);
                    drNew["CHENH_LECH_SL"] = TinhChenhLechSL_PhanTram(drNew);
                    drNew["CHENH_LECH_SL_KWH"] = TinhChenhLechSL_KWH(drNew);
                    drNew["ISCHECKED"] = false;
                    drNew["DNTT"] = TinhDNTT(drNew);

                    dtNew.Rows.Add(drNew);
                }
                return dtNew;
            }
            catch (Exception ex)
            {
                int result = i;
                return null;
            }
        }
        public decimal TinhDNTT(DataRow dr)
        {
            string TTR_MOI = dr["TTR_MOI"] != null ? dr["TTR_MOI"].ToString().Trim() : "";
            decimal hsn = 0;
            decimal CS_Cu = 0;
            decimal CS_Moi = 0;
            decimal hieu_so = TinhHieuSo(dr);
            decimal sl_moi = 0;
            decimal sl_thao = 0;
            decimal sl_ttiep = 0;
            decimal dntt = 0;

            if (!decimal.TryParse(dr["HSN"] != null ? dr["HSN"].ToString().Trim() : "1", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out hsn))
            {
                hsn = 0;
            }

            if (!decimal.TryParse(dr["SL_TTIEP"] != null ? dr["SL_TTIEP"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_ttiep))
            {
                sl_ttiep = 0;
            }
            else
            {
                sl_moi = 0;
            }
            if (!decimal.TryParse(dr["SL_THAO"] != null ? dr["SL_THAO"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_thao))
            {
                sl_thao = 0;
            }

            sl_moi = hieu_so * hsn;

            if (decimal.TryParse(dr["CS_CU"] != null ? dr["CS_CU"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out CS_Cu)
                && decimal.TryParse(dr["CS_MOI"] != null ? dr["CS_MOI"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out CS_Moi))
            {
                //Nếu khách hàng chưa ghi
                if (TTR_MOI.Length == 0 && CS_Moi == 0)
                {
                    dntt = 0; // trả về dntt = 0
                }
                else
                {
                    dntt = sl_moi + sl_thao + sl_ttiep;
                }
            }
            else
            {
                dntt = 0; // trả về dntt = 0
            }


            return decimal.Round(dntt);
        }
        public decimal TinhChenhLechSL_PhanTram(DataRow dr)
        {
            decimal hieu_so = TinhHieuSo(dr);
            decimal hsn = 0;
            decimal sl_cu = 0;
            decimal sl_moi = 0;
            decimal sl_thao = 0;
            decimal phan_tram_chenh_lech = 0;

            if (!decimal.TryParse(dr["SL_CU"] != null ? dr["SL_CU"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_cu))
            {
                sl_cu = 0;
            }
            if (!decimal.TryParse(dr["HSN"] != null ? dr["HSN"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out hsn))
            {
                hsn = 0;
            }
            if (!decimal.TryParse(dr["SL_THAO"] != null ? dr["SL_THAO"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_thao))
            {
                sl_thao = 0;
            }
            sl_moi = hieu_so * hsn;

            if (sl_cu > 0)
            {
                if ((sl_moi + sl_thao) > sl_cu)
                {
                    phan_tram_chenh_lech = (sl_moi - sl_cu + sl_thao) / sl_cu * 100;
                }
                else if ((sl_moi + sl_thao) < sl_cu)
                {
                    phan_tram_chenh_lech = (sl_cu - sl_moi - sl_thao) / sl_cu * 100;
                }
                else
                {
                    phan_tram_chenh_lech = 0;
                }
            }
            else
            {
                phan_tram_chenh_lech = 999999;
            }

            return decimal.Round(phan_tram_chenh_lech);
        }
        public decimal TinhChenhLechSL_KWH(DataRow dr)
        {
            decimal sl_cu = 0;
            decimal sl_moi = 0;
            decimal sl_thao = 0;
            decimal kwh_chenh_lech = 0;

            if (!decimal.TryParse(dr["SL_CU"] != null ? dr["SL_CU"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_cu))
            {
                sl_cu = 0;
            }
            if (!decimal.TryParse(dr["SL_MOI"] != null ? dr["SL_MOI"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_moi))
            {
                sl_moi = 0;
            }
            if (!decimal.TryParse(dr["SL_THAO"] != null ? dr["SL_THAO"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_thao))
            {
                sl_thao = 0;
            }

            if (sl_cu > 0)
            {
                if ((sl_moi + sl_thao) > sl_cu)
                {
                    kwh_chenh_lech = sl_moi - sl_cu + sl_thao;
                }
                else if ((sl_moi + sl_thao) < sl_cu)
                {
                    kwh_chenh_lech = sl_cu - sl_moi - sl_thao;
                }
                else
                {
                    kwh_chenh_lech = 0;
                }
            }
            else
            {
                kwh_chenh_lech = sl_moi + sl_thao;
            }

            return decimal.Round(kwh_chenh_lech);
        }
        public decimal TinhSanLuong(DataRow dr)
        {
            if (dr == null)
                return 0;

            decimal san_luong = 0;
            decimal hieu_so = TinhHieuSo(dr);
            decimal he_so_nhan = Convert.ToDecimal(dr["HSN"].ToString());
            string TinhTrangMoi = null;
            if (dr["TTR_MOI"] != null)
                TinhTrangMoi = dr["TTR_MOI"].ToString();

            if (TinhTrangMoi.Equals("K") || TinhTrangMoi.Equals("C")
                || TinhTrangMoi.Equals("H") || TinhTrangMoi.Equals("G") || TinhTrangMoi.Equals("T") || TinhTrangMoi.Length == 0)
            {
                san_luong = hieu_so * he_so_nhan;
            }
            else if (TinhTrangMoi.Equals("Q"))
            {
                san_luong = hieu_so * he_so_nhan;
            }
            else if (TinhTrangMoi.Equals("U") || TinhTrangMoi.Equals("L") || TinhTrangMoi.Equals("D") || TinhTrangMoi.Equals("X")
                        || TinhTrangMoi.Equals("M") || TinhTrangMoi.Equals("V") || TinhTrangMoi.Equals("Y"))
            {
                san_luong = 0;
            }

            return san_luong;
        }
        public decimal TinhHieuSo(DataRow dr)
        {
            decimal CS_Cu = 0;
            decimal CS_Moi = 0;
            string TTR_MOI = dr["TTR_MOI"] != null ? dr["TTR_MOI"].ToString().Trim() : "";

            if (decimal.TryParse(dr["CS_CU"] != null ? dr["CS_CU"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out CS_Cu)
                && decimal.TryParse(dr["CS_MOI"] != null ? dr["CS_MOI"].ToString() : "0", NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out CS_Moi))
            {
                //Nếu khách hàng chưa ghi
                if (TTR_MOI.Length == 0 && CS_Moi == 0)
                {
                    return 0; // trả về hiệu số = 0
                }

                if (TTR_MOI == "Q")//công tơ qua vòng
                {
                    return TinhHieuSoCtoQuaVong(CS_Cu, CS_Moi);
                }
                else
                {
                    return CS_Moi - CS_Cu;
                }

            }
            else
            {
                return 0;
            }

        }
        public decimal TinhHieuSoCtoQuaVong(decimal CS_Cu, decimal CS_Moi)
        {
            decimal hieu_so = 0;
            if (CS_Moi <= CS_Cu)
            {
                hieu_so = Convert.ToDecimal(decimal.Parse(Math.Pow(10, Double.Parse(decimal.ToInt32(CS_Cu).ToString().Length.ToString())).ToString()) - CS_Cu + CS_Moi, CultureInfo.CurrentCulture);
            }
            else
            {
                hieu_so = Convert.ToDecimal(decimal.Parse(Math.Pow(10, Double.Parse(decimal.ToInt32(CS_Moi).ToString().Length.ToString())).ToString()) - CS_Cu + CS_Moi, CultureInfo.CurrentCulture);
            }

            return hieu_so;
        }
        public static bool IsNumberValue(string strNumber, out decimal number)
        {
            string value = strNumber;
            return decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out number);
        }
        public static bool IsNumberValueNew(string strNumber, out decimal number)
        {
            string value = strNumber;
            return decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number);
        }
        public static bool IsNumber(Type typeNumber)
        {
            return (typeNumber == typeof(sbyte)
                    || typeNumber == typeof(byte)
                    || typeNumber == typeof(short)
                    || typeNumber == typeof(ushort)
                    || typeNumber == typeof(int)
                    || typeNumber == typeof(uint)
                    || typeNumber == typeof(long)
                    || typeNumber == typeof(ulong)
                    || typeNumber == typeof(float)
                    || typeNumber == typeof(double)
                    || typeNumber == typeof(decimal));
        }
        public DataTable CreateBlankTemplateDataTable()
        {
            DataTable dtTemplate = new DataTable("Table1");


            // create schema for datatable
            for (int i = 0; i < gcs_col_all.Length; i++)
            {
                DataColumn dc = new DataColumn(gcs_col_all[i].col_name, gcs_col_all[i].col_type);
                if (dc.DataType == System.Type.GetType("System.DateTime"))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified; //set msdata:DateTimeMode="Unspecified" trong XML
                }

                dc.AllowDBNull = true; // set minOccurs="0" trong XML
                dc.MaxLength = -1; // bỏ maxLength trong XML
                dtTemplate.Columns.Add(dc);
            }
            for (int i = 0; i < ext_col.Length; i++)
            {
                DataColumn dc = new DataColumn(ext_col[i].col_name, ext_col[i].col_type);
                if (dc.DataType == System.Type.GetType("System.DateTime"))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified; //set msdata:DateTimeMode="Unspecified" trong XML
                }

                dc.AllowDBNull = true; // set minOccurs="0" trong XML
                dc.MaxLength = -1; // bỏ maxLength trong XML
                dtTemplate.Columns.Add(dc);
            }
            return dtTemplate;
        }
        public static readonly colData[] standart_col = new colData[] {
            new colData("MA_NVGCS", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_KHANG", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_DDO", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_DVIQLY", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_GC", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_QUYEN", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_TRAM", typeof(string), DbType.String, "TEXT", true),
            new colData("BOCSO_ID", typeof(long), DbType.String, "TEXT", true),
            new colData("LOAI_BCS", typeof(string), DbType.String, "TEXT", true),
            new colData("LOAI_CS", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_KHANG", typeof(string), DbType.String, "TEXT", true),
            new colData("DIA_CHI", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_NN", typeof(string), DbType.String, "TEXT", true),
            new colData("SO_HO", typeof(decimal), DbType.String, "TEXT", true),
            new colData("MA_CTO", typeof(string), DbType.String, "TEXT", true),
            new colData("SERY_CTO", typeof(string), DbType.String, "TEXT", true),
            new colData("HSN", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CS_CU", typeof(decimal), DbType.String, "TEXT", true),
            new colData("TTR_CU", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_CU",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SL_TTIEP", typeof(int), DbType.String, "TEXT", true),
            new colData("NGAY_CU", typeof(DateTime), DbType.String, "TEXT", true),
            new colData("CS_MOI", typeof(decimal), DbType.String, "TEXT", true),
            new colData("TTR_MOI", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_MOI", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CHUOI_GIA", typeof(string), DbType.String, "TEXT", true),
            new colData("KY", typeof(int), DbType.String, "TEXT", true),
            new colData("THANG", typeof(int), DbType.String, "TEXT", true),
            new colData("NAM", typeof(int), DbType.String, "TEXT", true),
            new colData("NGAY_MOI",typeof(DateTime), DbType.String, "TEXT", true),
            new colData("NGUOI_GCS", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_THAO", typeof(decimal), DbType.String, "TEXT", true),
            new colData("KIMUA_CSPK", typeof(short), DbType.String, "TEXT", true) ,
            new colData("MA_COT",typeof(string), DbType.String, "TEXT", true),
            new colData("SLUONG_1",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SLUONG_2",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SLUONG_3",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SO_HOM",typeof(string), DbType.String, "TEXT", true),
            new colData("PMAX",typeof(decimal), DbType.String, "TEXT", true),
            new colData("NGAY_PMAX",typeof(DateTime), DbType.String, "TEXT", true)

        };

        public static readonly colData[] wifi_col = new colData[] {
            new colData("ID", typeof(int), DbType.String, "TEXT", true),
            new colData("GHICHU", typeof(string), DbType.String, "TEXT", true),
            new colData("TT_KHAC", typeof(string), DbType.String, "TEXT", true),
            new colData("CGPVTHD",typeof(string), DbType.String, "TEXT", true),
            new colData("HTHUC_TBAO_DK", typeof(string), DbType.String, "TEXT", true),
            new colData("DTHOAI_SMS", typeof(string), DbType.String, "TEXT", true),
            new colData("EMAIL", typeof(string), DbType.String, "TEXT", true),
            new colData("THOI_GIAN", typeof(string), DbType.String, "TEXT", true),
            new colData("X", typeof(double), DbType.String, "TEXT", true),
            new colData("Y", typeof(double), DbType.String, "TEXT", true),
            new colData("SO_TIEN", typeof(decimal), DbType.String, "TEXT", true),
            new colData("HTHUC_TBAO_TH", typeof(string), DbType.String, "TEXT", true),
            new colData("TENKHANG_RUTGON", typeof(string), DbType.String, "TEXT", true),
            new colData("TTHAI_DBO", typeof(short), DbType.String, "TEXT", true),
            new colData("DU_PHONG", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_FILE", typeof(string), DbType.String, "TEXT", true),
            new colData("CHU_KY", typeof(byte[]), DbType.Binary, "BLOB", true),
            new colData("CHU_KY_BASE64", typeof(string), DbType.String, "TEXT", true),
            new colData("HINH_ANH", typeof(byte[]), DbType.Binary, "BLOB", true),
            new colData("HINH_ANH_BASE64", typeof(string), DbType.String, "TEXT", true)

        };

        public static readonly colData[] ext_col = new colData[]{
            new colData("HIEU_SO", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CHENH_LECH_SL", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CHENH_LECH_SL_KWH", typeof(decimal), DbType.String, "TEXT", true), //chứa chênh lệch sản lượng theo đơn vị kwh
            new colData("ISCHECKED", typeof(bool), DbType.String, "TEXT", true),
            new colData("DNTT", typeof(decimal), DbType.String, "TEXT", true)
        };

        public static readonly colData[] gcs_col_all = new colData[]{
            //standart
            new colData("MA_NVGCS", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_KHANG", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_DDO", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_DVIQLY", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_GC", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_QUYEN", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_TRAM", typeof(string), DbType.String, "TEXT", true),
            new colData("BOCSO_ID", typeof(long), DbType.String, "TEXT", true),
            new colData("LOAI_BCS", typeof(string), DbType.String, "TEXT", true),
            new colData("LOAI_CS", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_KHANG", typeof(string), DbType.String, "TEXT", true),
            new colData("DIA_CHI", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_NN", typeof(string), DbType.String, "TEXT", true),
            new colData("SO_HO", typeof(decimal), DbType.String, "TEXT", true),
            new colData("MA_CTO", typeof(string), DbType.String, "TEXT", true),
            new colData("SERY_CTO", typeof(string), DbType.String, "TEXT", true),
            new colData("HSN", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CS_CU", typeof(decimal), DbType.String, "TEXT", true),
            new colData("TTR_CU", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_CU",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SL_TTIEP", typeof(int), DbType.String, "TEXT", true),
            new colData("NGAY_CU", typeof(DateTime), DbType.String, "TEXT", true),
            new colData("CS_MOI", typeof(decimal), DbType.String, "TEXT", true),
            new colData("TTR_MOI", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_MOI", typeof(decimal), DbType.String, "TEXT", true),
            new colData("CHUOI_GIA", typeof(string), DbType.String, "TEXT", true),
            new colData("KY", typeof(int), DbType.String, "TEXT", true),
            new colData("THANG", typeof(int), DbType.String, "TEXT", true),
            new colData("NAM", typeof(int), DbType.String, "TEXT", true),
            new colData("NGAY_MOI",typeof(DateTime), DbType.String, "TEXT", true),
            new colData("NGUOI_GCS", typeof(string), DbType.String, "TEXT", true),
            new colData("SL_THAO", typeof(decimal), DbType.String, "TEXT", true),
            new colData("KIMUA_CSPK", typeof(short), DbType.String, "TEXT", true) ,
            new colData("MA_COT",typeof(string), DbType.String, "TEXT", true),
            new colData("SLUONG_1",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SLUONG_2",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SLUONG_3",typeof(decimal), DbType.String, "TEXT", true),
            new colData("SO_HOM",typeof(string), DbType.String, "TEXT", true),
            new colData("PMAX",typeof(decimal), DbType.String, "TEXT", true),
            new colData("NGAY_PMAX",typeof(DateTime), DbType.String, "TEXT", true),

            // wifi_col
            new colData("ID", typeof(int), DbType.String, "TEXT", true),
            new colData("GHICHU", typeof(string), DbType.String, "TEXT", true),
            new colData("TT_KHAC", typeof(string), DbType.String, "TEXT", true),
            new colData("CGPVTHD",typeof(string), DbType.String, "TEXT", true),
            new colData("HTHUC_TBAO_DK", typeof(string), DbType.String, "TEXT", true),
            new colData("DTHOAI_SMS", typeof(string), DbType.String, "TEXT", true),
            new colData("EMAIL", typeof(string), DbType.String, "TEXT", true),
            new colData("THOI_GIAN", typeof(string), DbType.String, "TEXT", true),
            new colData("X", typeof(double), DbType.String, "TEXT", true),
            new colData("Y", typeof(double), DbType.String, "TEXT", true),
            new colData("SO_TIEN", typeof(decimal), DbType.String, "TEXT", true),
            new colData("HTHUC_TBAO_TH", typeof(string), DbType.String, "TEXT", true),
            new colData("TENKHANG_RUTGON", typeof(string), DbType.String, "TEXT", true),
            new colData("TTHAI_DBO", typeof(short), DbType.String, "TEXT", true),
            new colData("DU_PHONG", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_FILE", typeof(string), DbType.String, "TEXT", true),
            new colData("CHU_KY", typeof(byte[]), DbType.Binary, "BLOB", true),
            new colData("CHU_KY_BASE64", typeof(string), DbType.String, "TEXT", true),
            new colData("HINH_ANH", typeof(byte[]), DbType.Binary, "BLOB", true),
            new colData("HINH_ANH_BASE64", typeof(string), DbType.String, "TEXT", true),
            new colData("ID_SQLITE", typeof(string), DbType.String, "TEXT", true),

            // Đối soát
            new colData("STR_CHECK_DSOAT", typeof(string), DbType.String, "TEXT", true)

        };

        public static readonly colData[] gcs_TH_col_all = new colData[]{
            new colData("STT",typeof(string),DbType.String,"TEXT",true),
            new colData("MA_KHACH_HANG", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_KHACH_HANG", typeof(string), DbType.String, "TEXT", true),
            new colData("DIA_CHI", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_SO_THUE", typeof(string), DbType.String, "TEXT", true),
            new colData("CAN_BO_THEO_DOI", typeof(string), DbType.String, "TEXT", true),
            new colData("MA_CONG_TO", typeof(string), DbType.String, "TEXT", true),
            new colData("SO_HO", typeof(string), DbType.String, "TEXT", true),
            new colData("HE_SO", typeof(string), DbType.String, "TEXT", true),
            new colData("NOI_DUNG", typeof(string), DbType.String, "TEXT", true),
            new colData("NGAY_SU_DUNG", typeof(string), DbType.String, "TEXT", true),
            new colData("THANG", typeof(string), DbType.String, "TEXT", true),
            new colData("NAM", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_CU", typeof(string), DbType.String, "TEXT", true),
            new colData("SAN_LUONG", typeof(string), DbType.String, "TEXT", true),
            new colData("SAN_LUONG_CU", typeof(string), DbType.String, "TEXT", true),
            new colData("MDK", typeof(string), DbType.String, "TEXT", true),
            new colData("DON_GIA", typeof(string), DbType.String, "TEXT", true),
            new colData("TRAM_DIEN", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_1", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_CU_1", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_2", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_CU_2", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_3", typeof(string), DbType.String, "TEXT", true),
            new colData("CHI_SO_CU_3", typeof(string), DbType.String, "TEXT", true),
            new colData("TEN_FILE", typeof(string), DbType.String, "TEXT", true),
         };
    }
}