using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Globalization;
using System.Data.SQLite;
using Shell32;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.Zip;
using System.IO.Compression;
using System.Net;
using ES_WEBKYSO.Common;

namespace GCS_LIB
{
    public class Common
    {
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
 
       
        /// <summary>
        /// set màu cho nút
        /// </summary>
        /// <param name="btnArr"></param>
        public static void SetButtonColor(Button[] btnArr)
        {
            foreach (var btn in btnArr)
            {
                btn.BackColor = btn.Enabled ? Color.DeepSkyBlue : Color.LightGray;
            }
        }

        /// <summary>
        /// Ghép đường dẫn 
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string JoinStringPath(params string[] strings)
        {
            string strJoin = "";
            foreach (string str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                string strConvert = str.Trim();
                // nếu ko phải chuỗi đầu và ko chứa dấu / thì thêm vào
                if (strJoin != ""
                    && (strConvert.Length == 0 || !("\\,/").Contains(strConvert.Substring(0, 1))))
                {
                    strConvert = "\\" + strConvert;
                }
                if (strConvert.Length > 0 && ("\\,/").Contains(strConvert.Substring(strConvert.Length - 1)))
                {
                    strConvert = strConvert.Substring(0, strConvert.Length - 1);
                }

                strJoin += strConvert;
            }
            return strJoin;
        }

        /// <summary>
        /// Lấy tên loại nhân viên
        /// </summary>
        /// <param name="loai_nv"></param>
        /// <returns></returns>
        //public string Get_Ten_Loai_NV(int loai_nv)
        //{
        //    switch (loai_nv)
        //    {
        //        case ((int)clsConstants.LOAI_NV.ADMIN):
        //            return "Quản trị";
        //        case ((int)clsConstants.LOAI_NV.NV_CHUYEN_DU_LIEU):
        //            return "Nhân viên chuyển dữ liệu";
        //        case ((int)clsConstants.LOAI_NV.NV_GCS):
        //            return "Nhân viên ghi chỉ số";
        //        case ((int)clsConstants.LOAI_NV.TRUONG_DOI):
        //            return "Đội trưởng";
        //        default:
        //            return "";
        //    }
        //}

        /// <summary>
        /// Lấy tên trạng thái sổ
        /// </summary>
        /// <param name="tt_so"></param>
        /// <returns></returns>
        //public string GetTenTrangThaiSo(int tt_so)
        //{
        //    switch (tt_so)
        //    {
        //        case ((int)clsConstants.TRANG_THAI_SOGCS.CHUA_CHUYEN_HHU):
        //            return "Chưa chuyển thiết bị";
        //        case ((int)clsConstants.TRANG_THAI_SOGCS.DA_CHUYEN_RA_HHU):
        //            return "Đã chuyển thiết bị";
        //        case ((int)clsConstants.TRANG_THAI_SOGCS.DA_NHAN_TU_HHU):
        //            return "Đủ chỉ số";
        //        case ((int)clsConstants.TRANG_THAI_SOGCS.DA_XUAT_CHO_CMIS):
        //            return "Khóa";
        //        default:
        //            return "";
        //    }
        //}

        /// <summary>
        /// Cập nhật công tơ có chỉ số nhỏ thành trạng thái U và lưu file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="dsUpdate"></param>
        //public string CapNhatCtoThanhU(ConfigInfo config, string FileName, bool do_save_file, out int updated_rows, out DataTable dtUpdated)
        //{
        //    int dinh_muc_u = config.DINH_MUC_U;
        //    updated_rows = 0;
        //    dtUpdated = null;
        //    try
        //    {
        //        DataSet dsFile = new DataSet();
        //        dsFile.ReadXml(FileName, XmlReadMode.ReadSchema);
        //        if (dsFile != null && dsFile.Tables.Count > 0)
        //            dtUpdated = dsFile.Tables[0].Clone();
        //        else
        //            return null;
        //        decimal hieu_so = 0;
        //        decimal cs_cu;
        //        string ttr_moi = "";
        //        string loai_bcs = "";
        //        if (dsFile.Tables[0].Rows.Count == 0)
        //        {
        //            return null;
        //        }

        //        foreach (DataRow dr in dsFile.Tables[0].Rows)
        //        {
        //            hieu_so = TinhHieuSo(dr);
        //            ttr_moi = dr["TTR_MOI"] == null ? "" : dr["TTR_MOI"].ToString().Trim();
        //            loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString().Trim();
        //            if (ttr_moi.Length == 0 && loai_bcs == "KT" && hieu_so < dinh_muc_u)
        //            {
        //                cs_cu = 0;
        //                decimal.TryParse(dr["CS_CU"] == null ? "0" : dr["CS_CU"].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_cu);
        //                dr["CS_MOI"] = cs_cu;
        //                dr["TTR_MOI"] = "U";
        //                dr["SL_MOI"] = 0;

        //                dtUpdated.ImportRow(dr);
        //                updated_rows++;
        //            }
        //        }

        //        if (do_save_file)
        //        {
        //            FileInfo fi = new FileInfo(FileName);
        //            DirectoryInfo dirBackup = new DirectoryInfo(config.BACKUP_FOLDER);
        //            if (!dirBackup.Exists)
        //            {
        //                dirBackup.Create();
        //            }
        //            string strDate = DateTime.Now.ToString("yyyy/MM/dd");
        //            string backup_folder = config.BACKUP_FOLDER.Trim();
        //            if (backup_folder.Substring(backup_folder.Length - 1) != "\\" && backup_folder.Substring(backup_folder.Length - 1) != "/")
        //            {
        //                backup_folder += "\\";
        //            }
        //            DirectoryInfo dirDate = new DirectoryInfo(backup_folder + strDate);
        //            if (!dirDate.Exists)
        //            {
        //                dirDate.Create();
        //            }

        //            fi.CopyTo(backup_folder + strDate + "\\" + fi.Name, true);
        //            dsFile.WriteXml(FileName, XmlWriteMode.WriteSchema);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error:" + ex.Message;
        //    }
        //}

        /// <summary>
        /// Convert dataset -> xml file cho vào folder theo đường dẫn trong cấu hính
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="config"></param>
        /// <param name="dsFile"></param>
        /// <param name="doBackup"></param>
        /// <returns></returns>
        //public string SaveDsToFile(string FileName, ConfigInfo config, DataSet dsFile, bool doBackup)
        //{
        //    try
        //    {
        //        if (doBackup)
        //        {
        //            FileInfo fi = new FileInfo(FileName);
        //            DirectoryInfo dirBackup = new DirectoryInfo(config.BACKUP_FOLDER);
        //            if (!dirBackup.Exists)
        //            {
        //                dirBackup.Create();
        //            }
        //            string strDate = DateTime.Now.ToString("yyyy/MM/dd");
        //            string backup_folder = config.BACKUP_FOLDER.Trim();
        //            backup_folder = JoinStringPath(new string[] { backup_folder, strDate });

        //            DirectoryInfo dirDate = new DirectoryInfo(backup_folder);
        //            if (!dirDate.Exists)
        //            {
        //                dirDate.Create();
        //            }

        //            fi.CopyTo(JoinStringPath(new string[] { backup_folder, fi.Name }), true);
        //        }

        //        dsFile.WriteXml(FileName, XmlWriteMode.WriteSchema);

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}



        /// <summary>
        /// Lấy datatable tình trạng bất thường
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_FILE"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Lấy dữ liệu chỉ số PMax
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_FILE"></param>
        /// <returns></returns>
        public DataTable GetCHISO_PMax(DataTable dtData, string TEN_FILE)
        {
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);
            dtNew.Columns.Add("NGAY_PMAX1");
            dtNew.Columns.Add("PMAX1");
            DataTable dtCSPMax = dtNew.Clone();
            DataRow dr = null;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                dr = dtNew.Rows[i];
                if (dr["LOAI_BCS"].ToString() == "KT" || dtData.Select("MA_CTO ='" + dr["MA_CTO"] + "'").Count() == 2)
                    continue;
                if ((decimal.Parse(dr["PMAX"].ToString()) <= 0) || dr["LOAI_BCS"].ToString() != "BT")
                {
                    continue;
                }
                dr.SetField("PMAX1", dr["PMAX"].ToString());
                dr.SetField("NGAY_PMAX1", dr["NGAY_PMAX"].ToString());        
                dtCSPMax.ImportRow(dr);
            }
            return dtCSPMax;
        }

        /// <summary>
        /// Lấy dữ liệu sản lượng
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_FILE"></param>
        /// <param name="time_bdau"></param>
        /// <param name="time_kthuc"></param>
        /// <returns></returns>
        public DataTable GetSanLuong(DataTable dtData, string TEN_FILE, DateTime time_bdau, DateTime time_kthuc)
        {
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);

            DataTable dtTHSL = dtNew.Clone();
            DataRow dr = null;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                dr = dtNew.Rows[i];
                if (time_bdau.Year == time_kthuc.Year)
                {

                    if (Convert.ToInt32(dr["Thang"].ToString()) >= time_bdau.Month && Convert.ToInt32(dr["Thang"].ToString()) <= time_kthuc.Month)
                    {
                        dtTHSL.ImportRow(dr);
                    }
                }
                else
                {
                    if ((Convert.ToInt32(dr["Thang"].ToString()) >= time_bdau.Month && Convert.ToInt32(dr["Nam"].ToString()) == time_bdau.Year)
                        || (Convert.ToInt32(dr["Thang"].ToString()) <= time_kthuc.Month && Convert.ToInt32(dr["Nam"].ToString()) == time_kthuc.Year))
                    {
                        dtTHSL.ImportRow(dr);
                    }
                }
            }
            return dtTHSL;
        }

        /// <summary>
        /// Lấy dữ liệu ghi chú
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_SO"></param>
        /// <returns></returns>
        public DataTable GetDtGhiChu(DataTable dtData, string TEN_SO)
        {
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_SO);
            DataTable dtGhiChu = dtNew.Clone();

            if (dtNew != null && dtNew.Columns.Contains("GHICHU") && dtNew.Rows.Count > 0)
            {
                DataRow drGhiChu = null;
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    drGhiChu = dtNew.Rows[i];
                    if (drGhiChu["GHICHU"] != null && drGhiChu["GHICHU"].ToString().Trim().Length > 0)
                    {
                        dtGhiChu.ImportRow(drGhiChu);
                    }
                }
            }
            return dtGhiChu;
        }

        /// <summary>
        /// Lấy danh sách công tơ dưới định mức U
        /// </summary>
        /// <param name="config"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        //public DataTable GetCtoDuoiMucU(ConfigInfo config, string FileName)
        //{
        //    int dinh_muc_u = config.DINH_MUC_U;
        //    DataTable dtUpdated = null;
        //    try
        //    {
        //        DataTable dtConvert = new DataTable();
        //        DataSet dsFile = new DataSet();
        //        dsFile.ReadXml(FileName, XmlReadMode.ReadSchema);
        //        decimal hieu_so = 0;
        //        decimal cs_moi = 0;
        //        string ttr_moi = "";
        //        string loai_bcs = "";
        //        FileInfo fi = new FileInfo(FileName);
        //        if (dsFile.Tables[0].Rows.Count == 0)
        //        {
        //            return dsFile.Tables[0];
        //        }
        //        dtConvert = Convert2TemplateDataTable(dsFile.Tables[0], fi.Name);
        //        dtUpdated = dtConvert.Clone();

        //        foreach (DataRow dr in dtConvert.Rows)
        //        {
        //            decimal.TryParse(dr["HIEU_SO"] == null ? "0" : dr["HIEU_SO"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out hieu_so);
        //            decimal.TryParse(dr["CS_MOI"] == null ? "0" : dr["CS_MOI"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_moi);
        //            ttr_moi = dr["TTR_MOI"] == null ? "" : dr["TTR_MOI"].ToString().Trim();
        //            loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString().Trim();

        //            if (((ttr_moi.Length == 0 && cs_moi > 0) || ttr_moi == "Q") && loai_bcs == "KT" && hieu_so < dinh_muc_u)
        //            {
        //                //dr["HIEU_SO"] = hieu_so;
        //                //dr["TEN_FILE"] = fi.Name;
        //                //dr["ISCHECKED"] = false;
        //                dtUpdated.ImportRow(dr);
        //            }
        //        }
        //        return dtUpdated;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Lấy danh sách công tơ sản lượng bất thường
        /// </summary>
        /// <param name="oprt"></param>
        /// <param name="slbt_lvl"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public DataTable GetCtoSLBT_PhanTram(string oprt, int slbt_lvl, string FileName)
        {
            DataTable dtUpdated = null;
            try
            {
                DataTable dtConvert = null;
                DataSet dsFile = new DataSet();
                dsFile.ReadXml(FileName, XmlReadMode.ReadSchema);

                decimal hieu_so = 0;
                decimal chenh_lech_sl = 0;
                decimal cs_moi = 0;
                decimal cs_cu = 0;
                decimal sl_moi = 0;
                decimal sl_cu = 0;

                string ttr_moi = "";
                string loai_bcs = "";
                FileInfo fi = new FileInfo(FileName);
                if (dsFile.Tables[0].Rows.Count == 0)
                {
                    return dsFile.Tables[0];
                }
                dtConvert = Convert2TemplateDataTable(dsFile.Tables[0], fi.Name);
                dtUpdated = dtConvert.Clone();

                foreach (DataRow dr in dtConvert.Rows)
                {
                    decimal.TryParse(dr["HIEU_SO"] == null ? "0" : dr["HIEU_SO"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out hieu_so);
                    decimal.TryParse(dr["CHENH_LECH_SL"] == null ? "0" : dr["CHENH_LECH_SL"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out chenh_lech_sl);
                    decimal.TryParse(dr["CS_MOI"] == null ? "0" : dr["CS_MOI"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_moi);
                    decimal.TryParse(dr["CS_CU"] == null ? "0" : dr["CS_CU"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_cu);
                    decimal.TryParse(dr["SL_MOI"] == null ? "0" : dr["SL_MOI"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_moi);
                    decimal.TryParse(dr["SL_CU"] == null ? "0" : dr["SL_CU"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_cu);
                    ttr_moi = dr["TTR_MOI"] == null ? "" : dr["TTR_MOI"].ToString().Trim();
                    loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString().Trim();

                    if (((ttr_moi.Length == 0 && cs_moi > 0) || ttr_moi == "Q") && loai_bcs == "KT")
                    {
                        if ((oprt.Contains("<") && chenh_lech_sl < slbt_lvl)
                            || (oprt.Contains(">") && chenh_lech_sl > slbt_lvl)
                            || (oprt.Contains("=") && chenh_lech_sl == slbt_lvl))
                        {
                            //dr["HIEU_SO"] = hieu_so.ToString("0.###", CultureInfo.CurrentCulture);
                            //dr["CHENH_LECH_SL"] = chenh_lech_sl;
                            //dr["TENFILE"] = fi.Name;
                            //dr["ISCHECKED"] = true;

                            //dr["CS_MOI"] = cs_moi.ToString("0.###", CultureInfo.CurrentCulture);
                            //dr["CS_CU"] = cs_cu.ToString("0.###", CultureInfo.CurrentCulture);
                            //dr["SL_MOI"] = sl_moi.ToString("0.###", CultureInfo.CurrentCulture);
                            //dr["SL_CU"] = sl_cu.ToString("0.###", CultureInfo.CurrentCulture);
                            dtUpdated.ImportRow(dr);
                        }
                    }
                }
                return dtUpdated;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách công tơ sản lượng bất thường
        /// </summary>
        /// <param name="oprt"></param>
        /// <param name="slbt_lvl"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public DataTable GetCtoSLBT_DonVi(string oprt, int slbt_lvl, string FileName)
        {
            DataTable dtUpdated = null;
            try
            {
                DataTable dtConvert = null;
                DataSet dsFile = new DataSet();
                dsFile.ReadXml(FileName, XmlReadMode.ReadSchema);

                decimal hieu_so = 0;
                decimal chenh_lech_sl = 0;
                decimal cs_moi = 0;
                decimal cs_cu = 0;
                decimal sl_moi = 0;
                decimal sl_cu = 0;

                string ttr_moi = "";
                string loai_bcs = "";
                FileInfo fi = new FileInfo(FileName);
                if (dsFile.Tables[0].Rows.Count == 0)
                {
                    return dsFile.Tables[0];
                }
                dtConvert = Convert2TemplateDataTable(dsFile.Tables[0], fi.Name);
                dtUpdated = dtConvert.Clone();

                foreach (DataRow dr in dtConvert.Rows)
                {
                    decimal.TryParse(dr["HIEU_SO"] == null ? "0" : dr["HIEU_SO"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out hieu_so);
                    decimal.TryParse(dr["CHENH_LECH_SL_KWH"] == null ? "0" : dr["CHENH_LECH_SL_KWH"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out chenh_lech_sl);
                    decimal.TryParse(dr["CS_MOI"] == null ? "0" : dr["CS_MOI"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_moi);
                    decimal.TryParse(dr["CS_CU"] == null ? "0" : dr["CS_CU"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out cs_cu);
                    decimal.TryParse(dr["SL_MOI"] == null ? "0" : dr["SL_MOI"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_moi);
                    decimal.TryParse(dr["SL_CU"] == null ? "0" : dr["SL_CU"].ToString().Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out sl_cu);
                    ttr_moi = dr["TTR_MOI"] == null ? "" : dr["TTR_MOI"].ToString().Trim();
                    loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString().Trim();

                    if (((ttr_moi.Length == 0 && cs_moi > 0) || ttr_moi == "Q") && loai_bcs == "KT")
                    {
                        if ((oprt.Contains("<") && chenh_lech_sl < slbt_lvl)
                            || (oprt.Contains(">") && chenh_lech_sl > slbt_lvl)
                            || (oprt.Contains("=") && chenh_lech_sl == slbt_lvl))
                        {
                            dtUpdated.ImportRow(dr);
                        }
                    }
                }
                return dtUpdated;
            }
            catch
            {
                return null;
            }
        }

        public void TizTac(ref DataSet ds)
        {

            var groupdata = from b in ds.Tables[0].AsEnumerable()
                            group b by b.Field<string>("MA_DDO")
                into g
                            select new
                            {
                                MA_DDO = g.Key,
                                List = g.ToList(),
                            }
                into g
                            select new
                            {
                                g.MA_DDO,
                                g.List.Count
                            }
            ;
            foreach (var row in ds.Tables[0].Rows.Cast<DataRow>().Where(row => groupdata.Any(x => x.MA_DDO == row["MA_DDO"].ToString() && x.Count == 2)))
            {
                if(row["LOAI_BCS"].ToString().Equals("KT"))
                    row["LOAI_BCS"] = "KTVC";
                if (row["LOAI_BCS"].ToString().Equals("VC")) row["LOAI_BCS"] = "VCKT";
            }
        }

        /// <summary>
        /// Lấy danh sách sản lượng bất thường theo cấu hình
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_FILE"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        //public DataTable GetDtSLBT(DataTable dtData, string TEN_FILE, ConfigInfo config)
        //{
        //    //DataRow[] drArr = ds.Tables[0].Select(""
        //    //    + "(SL_CU > 0 AND (SL_MOI+SL_THAO) > SL_CU AND ((SL_MOI-SL_CU+SL_THAO)/SL_CU)>=" + ((decimal)config.SLBT_VUOT_MUC / (decimal)100) + ")"
        //    //    + " OR "
        //    //    + "(SL_CU > 0 AND (SL_MOI+SL_THAO) < SL_CU AND ((SL_CU-SL_MOI-SL_THAO)/SL_CU)>=" + ((decimal)config.SLBT_DUOI_MUC / (decimal)100) + ")"
        //    //    + " OR "
        //    //    + "(SL_CU = 0 AND (SL_MOI+SL_THAO) >= " + ((decimal)config.SLBT_VUOT_MUC / (decimal)100) + ")"
        //    //);
        //    DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);
        //    DataTable dtSlbt = dtNew.Clone();
        //    DataRow dr = null;
        //    decimal sl_moi = 0;
        //    decimal sl_cu = 0;
        //    decimal sl_thao = 0;
        //    string loai_bcs = "";


        //    for (int i = 0; i < dtData.Rows.Count; i++)
        //    {
        //        dr = dtNew.Rows[i];
        //        sl_moi = decimal.Parse(dr["SL_MOI"].ToString(), CultureInfo.CurrentCulture);
        //        sl_cu = decimal.Parse(dr["SL_CU"].ToString(), CultureInfo.CurrentCulture);
        //        sl_thao = decimal.Parse(dr["SL_THAO"].ToString(), CultureInfo.CurrentCulture);
        //        loai_bcs = dr["LOAI_BCS"].ToString().Trim();

        //        //if (loai_bcs == "VC")
        //        //{
        //        //    continue;
        //        //}

        //        if (config.ENABLE_SLBT_PERCENT)
        //        {
        //            if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) / sl_cu > ((decimal)config.SLBT_VUOT_MUC / 100))
        //                || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && ((sl_cu - sl_moi - sl_thao) / sl_cu) > ((decimal)config.SLBT_DUOI_MUC / 100))
        //                || (sl_cu <= 0 && (sl_moi + sl_thao) > ((decimal)config.SLBT_VUOT_MUC / 100)))
        //            {
        //                dtSlbt.ImportRow(dr);
        //                continue;
        //            }
        //        }

        //        if (config.ENABLE_SLBT_KWH)
        //        {
        //            if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) > config.SLBT_VUOT_MUC_KWH)
        //                || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && (sl_cu - sl_moi - sl_thao) > config.SLBT_DUOI_MUC_KWH)
        //                || (sl_cu <= 0 && (sl_moi + sl_thao) > config.SLBT_VUOT_MUC_KWH))
        //            {
        //                dtSlbt.ImportRow(dr);
        //                continue;
        //            }
        //        }

        //    }
        //    return dtSlbt;
        //}

        /// <summary>
        /// Lấy danh sách sản lượng bất thường theo cấu hình
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="TEN_FILE"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable GetDtSLBT_SMS(DataTable dtData, string TEN_FILE, string p_loailoc, decimal p_vuotmuc, decimal p_vuotmucmax, decimal p_duoimuc)
        {
            DataTable dtNew = Convert2TemplateDataTable(dtData, TEN_FILE);
            DataTable dtSLBT = dtNew.Clone();
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

                if (loai_bcs == "VC")
                {
                    continue;
                }

                if (p_loailoc == "PERCENT")
                {
                    if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) / sl_cu > p_vuotmuc && (sl_moi - sl_cu + sl_thao) / sl_cu <= p_vuotmucmax)
                        || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && ((sl_cu - sl_moi - sl_thao) / sl_cu) > p_duoimuc)
                        || (sl_cu <= 0 && (sl_moi + sl_thao) > p_vuotmuc))
                    {
                        dtSLBT.ImportRow(dr);
                        continue;
                    }
                }

                if (p_loailoc == "KWH")
                {
                    if ((sl_cu > 0 && (sl_moi + sl_thao) > sl_cu && (sl_moi - sl_cu + sl_thao) > p_vuotmuc && (sl_moi - sl_cu + sl_thao) <= p_vuotmucmax)
                        || (sl_cu > 0 && (sl_moi + sl_thao) < sl_cu && (sl_cu - sl_moi - sl_thao) > p_duoimuc)
                        || (sl_cu <= 0 && (sl_moi + sl_thao) > p_vuotmuc) && (sl_moi + sl_thao) <= p_vuotmucmax)
                    {
                        dtSLBT.ImportRow(dr);
                        continue;
                    }
                }

            }
            return dtSLBT;
        }

        /// <summary>
        /// Lấy danh sách công tơ không có ảnh
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="img_folder"></param>
        /// <returns></returns>
        public DataTable GetDtCtoKhongAnh(DataTable dtSrc, string img_folder, string TEN_FILE)
        {
            DataTable dtTemp = Convert2TemplateDataTable(dtSrc, TEN_FILE);
            DataTable dtDes = dtTemp.Clone();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                string img_path = ConvertDrToImagePath(img_folder, dtTemp.Rows[i]);
                //Kiểm tra xem có ảnh hay không
                if (!File.Exists(img_path))
                {
                    //Nếu không có ảnh thì lấy ra để in
                    DtAddDr(dtDes, dtTemp.Rows[i]);
                }
            }
            return dtDes;
        }

        /// <summary>
        /// Lấy danh sách công tơ đối soát đạt
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="img_folder"></param>
        /// <returns></returns>
        public DataTable GetDtCtoDSoatDat(DataTable dtSrc, string TEN_FILE)
        {
            DataTable dtTemp = Convert2TemplateDataTable(dtSrc, TEN_FILE);
            DataTable dtDes = dtTemp.Clone();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                //Kiểm tra xem cto có đối soát đạt
                if (dtTemp.Rows[i]["STR_CHECK_DSOAT"] != null && dtTemp.Rows[i]["STR_CHECK_DSOAT"] != DBNull.Value && dtTemp.Rows[i]["STR_CHECK_DSOAT"].ToString() == "CHECK")
                {
                    DtAddDr(dtDes, dtTemp.Rows[i]);
                }
            }
            return dtDes;
        }

        /// <summary>
        /// Lấy danh sách công tơ đối soát không đạt 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetDtCtoDSoatKoDat(DataTable dtSrc, string TEN_FILE)
        {
            DataTable dtTemp = Convert2TemplateDataTable(dtSrc, TEN_FILE);
            DataTable dtDes = dtTemp.Clone();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                //Kiểm tra xem cto có đối soát ko đạt
                if (dtTemp.Rows[i]["STR_CHECK_DSOAT"] != null && dtTemp.Rows[i]["STR_CHECK_DSOAT"] != DBNull.Value && dtTemp.Rows[i]["STR_CHECK_DSOAT"].ToString() == "UNCHECK")
                {
                    DtAddDr(dtDes, dtTemp.Rows[i]);
                }
            }
            return dtDes;
        }

        /// <summary>
        /// Lấy danh sách công tơ đối soát không chụp ảnh
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="TEN_FILE"></param>
        /// <param name="folder_img"></param>
        /// <returns></returns>
        public DataTable GetDtCtoDSoatKhongAnh(DataTable dtSrc, string TEN_FILE, string folder_img)
        {
            DataTable dtTemp = Convert2TemplateDataTable(dtSrc, TEN_FILE);
            DataTable dtDes = dtTemp.Clone();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                //Lấy cto không chụp ảnh
                string img_path = ConvertDrToImagePath(folder_img, dtTemp.Rows[i]);
                if (!File.Exists(img_path))
                {
                    DtAddDr(dtDes, dtTemp.Rows[i]);
                }
            }
            return dtDes;
        }

        /// <summary>
        /// Lấy danh sách công tơ không đối soát
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="TEN_FILE"></param>
        /// <returns></returns>
        public DataTable GetDtCtoKhongDSoat(DataTable dtSrc, string TEN_FILE)
        {
            DataTable dtTemp = Convert2TemplateDataTable(dtSrc, TEN_FILE);
            DataTable dtDes = dtTemp.Clone();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                //Lấy cto không đối soát
                if (dtTemp.Rows[i]["STR_CHECK_DSOAT"] == null || dtTemp.Rows[i]["STR_CHECK_DSOAT"] == DBNull.Value || dtTemp.Rows[i]["STR_CHECK_DSOAT"].ToString().Trim().Length == 0 || dtTemp.Rows[i]["STR_CHECK_DSOAT"].ToString().Trim() == "CHUA_DOI_SOAT")
                {
                    DtAddDr(dtDes, dtTemp.Rows[i]);
                }
            }
            return dtDes;
        }

        /// <summary>
        /// Clone datarow
        /// </summary>
        /// <param name="drDes"></param>
        /// <param name="drSrc"></param>
        public void CloneDataRow(ref DataRow drDes, DataRow drSrc)
        {
            drDes.ItemArray = drSrc.ItemArray.Clone() as object[];
        }

        /// <summary>
        /// Gán 1 datarow trong datatable sang 1 datatable khác
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dt"></param>
        /// <param name="dt1"></param>
        /// <param name="i"></param>
        public void DtAddDr(DataTable dtDes, DataRow sourceRow)
        {
            var desRow = dtDes.NewRow();
            desRow.ItemArray = sourceRow.ItemArray.Clone() as object[];
            dtDes.Rows.Add(desRow);
        }

        /// <summary>
        /// Tính hiệu số công tơ qua vòng
        /// </summary>
        /// <param name="CS_Cu"></param>
        /// <param name="CS_Moi"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính hiệu số của 1 công tơ
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính sản lượng của 1 công tơ theo trạng thái
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính chênh lệch sản lượng theo %
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính chênh lệch sản lượng theo KWH
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính điện năng tiêu thụ, nếu khách hàng chưa ghi thì trả về 0
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Mở dialog chọn đường dẫn file
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            string filePath = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
            }
            return filePath;
        }

        /// <summary>
        /// Mở dialog chọn đường dẫn folder
        /// </summary>
        /// <returns></returns>
        public string GetFolderPath()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "Chọn thư mục xuất file";
            dialog.ShowNewFolderButton = true;
            //dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// Convert ảnh -> byte[]
        /// </summary>
        /// <param name="p_postedImageFileName"></param>
        /// <param name="p_fileType"></param>
        /// <returns></returns>
        public byte[] ConvertImageToByteArray(string p_postedImageFileName, string[] p_fileType)
        {
            bool isValidFileType = false;
            try
            {
                FileInfo file = new FileInfo(p_postedImageFileName);

                foreach (string strExtensionType in p_fileType)
                {
                    if (strExtensionType == file.Extension)
                    {
                        isValidFileType = true;
                        break;
                    }
                }
                if (isValidFileType)
                {
                    FileStream fs = new FileStream(p_postedImageFileName, FileMode.Open, FileAccess.Read);

                    BinaryReader br = new BinaryReader(fs);

                    byte[] image = br.ReadBytes((int)fs.Length);

                    br.Close();

                    fs.Close();

                    return image;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo tên ảnh từ datarow
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string ConvertDrToImageName(DataRow dr)
        {
            string img_name = "";

            string ma_quyen = dr["MA_QUYEN"] == null ? "" : dr["MA_QUYEN"].ToString();
            string ma_cto = dr["MA_CTO"] == null ? "" : dr["MA_CTO"].ToString();
            string loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString();
            string nam = dr["NAM"] == null ? "" : dr["NAM"].ToString();
            string thang = dr["THANG"] == null ? "" : dr["THANG"].ToString();
            string ky = dr["KY"] == null ? "" : dr["KY"].ToString();
            //string ten_file = dr["TEN_FILE"] == null ? "" : dr["TEN_FILE"].ToString().ToUpper();

            img_name = dr["MA_QUYEN"] + "_" + dr["MA_CTO"] + "_"
                        + dr["NAM"] + "_" + dr["THANG"] + "_" + dr["KY"] + "_"
                        + dr["MA_DDO"] + "_" + dr["LOAI_BCS"] + ".jpg";

            return img_name;
        }
        public static string ConvertDrToImageNameOld(DataRow dr)
        {
            string img_name = "";

            string ma_quyen = dr["MA_QUYEN"] == null ? "" : dr["MA_QUYEN"].ToString();
            string ma_cto = dr["MA_CTO"] == null ? "" : dr["MA_CTO"].ToString();
            string loai_bcs = dr["LOAI_BCS"] == null ? "" : dr["LOAI_BCS"].ToString();
            string nam = dr["NAM"] == null ? "" : dr["NAM"].ToString();
            string thang = dr["THANG"] == null ? "" : dr["THANG"].ToString();
            string ky = dr["KY"] == null ? "" : dr["KY"].ToString();
            //string ten_file = dr["TEN_FILE"] == null ? "" : dr["TEN_FILE"].ToString().ToUpper();

            img_name = dr["MA_QUYEN"] + "_" + dr["MA_CTO"] + "_" + dr["LOAI_BCS"] + "_"
                        + dr["NAM"] + "-" + dr["THANG"] + "-" + dr["KY"] + ".jpg";

            return img_name;
        }

        /// <summary>
        /// Lấy đường dẫn của ảnh từ datarow
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string ConvertDrToImagePath(string parent_path, DataRow dr)
        {
            string img_name = ConvertDrToImageName(dr);
            string ten_file = dr["TEN_FILE"] == null ? "" : dr["TEN_FILE"].ToString().ToUpper();
            string img_fullname = JoinStringPath(parent_path, Regex.Replace(ten_file, ".XML", "_PHOTO", RegexOptions.IgnoreCase), img_name);

            return img_fullname;
        }

        /// <summary>
        /// Chèn file ảnh vào cột HINH_ANH trong datatable
        /// </summary>
        /// <returns></returns>
        public bool InsertImageToDataTable(ref DataTable dt, string strFilePath)
        {
            try
            {
                if (!dt.Columns.Contains("HINH_ANH"))
                {
                    dt.Columns.Add("HINH_ANH", typeof(byte[]));
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string img_name = ConvertDrToImageName(dr);
                    string ten_file = dr["TEN_FILE"] == null ? "" : dr["TEN_FILE"].ToString().ToUpper();
                    string img_fullname;
                    string temp = Path.GetExtension(ten_file);
                    if (temp.Trim() == ".XML")
                    {
                        img_fullname = strFilePath + Regex.Replace(ten_file, ".XML", "_PHOTO", RegexOptions.IgnoreCase) + "\\" + img_name;
                    }
                    else
                    {
                        img_fullname = strFilePath + Regex.Replace(ten_file, ".XLS", "_PHOTO", RegexOptions.IgnoreCase) + "\\" + img_name;
                    }

                    if (!File.Exists(img_fullname))
                        continue;

                    byte[] img_bytes = ConvertImageToByteArray(img_fullname, new string[] { ".jpg" });

                    #region code cũ
                    //string[] files = Directory.GetFiles(strImagePath, name + "*.jpg");
                    //byte[] img_bytes = null;
                    //if (files.Length > 0)
                    //{
                    //    Array.Sort(files);
                    //    //string img_fullname = JoinStringPath(new string[] { strImagePath, name });
                    //    string img_fullname = files[files.Length - 1]; // Lấy đường dẫn của file, nếu có nhiều file trùng tên thì lấy file cuối

                    //    // nếu ko có ảnh thì bỏ qua
                    //    if (!File.Exists(img_fullname))
                    //        continue;

                    //    // Convert ảnh -> byte[]
                    //    img_bytes = ConvertImageToByteArray(img_fullname, new string[] { ".jpg" });
                    //}
                    #endregion

                    // Gán vào cột HINH_ANH
                    dr["HINH_ANH"] = img_bytes;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Convert dữ liệu bảng theo template
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        public DataTable Convert2TemplateDataTable(DataTable dtData, string TEN_SO)
        {
            try
            {
                dtData.Locale = Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN"); 
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

        /// <summary>
        /// Tạo bảng rỗng có cấu trúc bảng theo mẫu
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gán giá trị của bảng theo template
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="dtNew"></param>
        /// <returns></returns>
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

                            #endregion
                        }
                        else if (gcs_col_all[col].col_type == typeof (DateTime))
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
                                    ngay_moi = (DateTime?) Convert.ToDateTime(dtData.Rows[row]["NGAY_MOI"].ToString());
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

        /// <summary>
        /// Tạo datatable rỗng có cấu trúc giống với file xuất từ CMIS
        /// </summary>
        /// <returns></returns>
        public DataTable CreateBlankCMISTemplateDataTable()
        {
            DataTable dtTemplate = new DataTable("Table1");
            // create schema for datatable
            for (int i = 0; i < standart_col.Length; i++)
            {
                DataColumn dc = new DataColumn(standart_col[i].col_name, standart_col[i].col_type);
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

        /// <summary>
        /// Xóa cột không phải xuất ra file XML
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable RemoveColumnsBeforeExportXml(DataTable dt)
        {
            // xóa cột CHU_KY
            if (dt.Columns.Contains("CHU_KY"))
                dt.Columns.Remove("CHU_KY");
            // xóa cột HINH_ANH
            if (dt.Columns.Contains("HINH_ANH"))
                dt.Columns.Remove("HINH_ANH");

            // xóa cột ext
            foreach (colData col in ext_col)
            {
                if (dt.Columns.Contains(col.col_name))
                    dt.Columns.Remove(col.col_name);
            }
            return dt;
        }

        /// <summary>
        /// Xuất file xml theo mẫu CMIS
        /// </summary>
        /// <param name="file_des_fullname"></param>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        public string ExportXmlByTemplate(string file_des_fullname, DataTable dtXml)
        {
            try
            {
                DataSet dsXml = new DataSet();
                //dtXml.TableName = "Table1";
                dtXml = Convert2TemplateDataTable(dtXml, new FileInfo(file_des_fullname).Name);
                dsXml.Tables.Add(dtXml);
                dsXml.WriteXml(file_des_fullname, XmlWriteMode.WriteSchema);

                //tạo file xml rỗng có cấu trúc dữ liệu theo mẫu
                DataTable dtFile = CreateBlankCMISTemplateDataTable();

                dtXml = RemoveColumnsBeforeExportXml(dtXml);// xóa cột ko xuất

                //chuyển dtXml thành xml string 
                string strXml = "";
                string schema = ConvertDataTable2XmlString(dtFile, XmlWriteMode.WriteSchema);
                schema = schema.Replace("</NewDataSet>", ""); // bỏ thẻ đóng của dataset vì đã có trong phần body phía dưới
                string body = ConvertDataTable2XmlString(dtXml, XmlWriteMode.IgnoreSchema);
                body = body.Replace("<NewDataSet>", ""); // bỏ thẻ mở dataset vì đã có trong phần schema phía trên
                strXml = schema + body;

                if (strXml == null || strXml.Length == 0)
                {
                    throw new Exception("Không có dữ liệu");
                }
                // thêm xml version nếu chưa có
                if (!strXml.Contains("<?xml version=\"1.0\" standalone=\"yes\"?>"))
                {
                    strXml = "<?xml version=\"1.0\" standalone=\"yes\"?>\n" + strXml;
                }

                // tạo file từ xml string
                return ExportFileFromString(file_des_fullname, strXml);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ExportXmlByTemplate_DoiSoat(string file_des_fullname, DataTable dtXml)
        {
            try
            {
                DataSet dsXml = new DataSet();
                //dtXml.TableName = "Table1";
                dtXml = Convert2TemplateDataTable(dtXml, new FileInfo(file_des_fullname).Name);
                dsXml.Tables.Add(dtXml);
                dsXml.WriteXml(file_des_fullname, XmlWriteMode.WriteSchema);

                //tạo file xml rỗng có cấu trúc dữ liệu theo mẫu
                DataTable dtFile = CreateBlankCMISTemplateDataTable();

                // xóa cột ko xuất
                dtXml = RemoveColumnsBeforeExportXml(dtXml);

                //chuyển dtXml thành xml string 
                string strXml = "";
                string schema = ConvertDataTable2XmlString(dtFile, XmlWriteMode.WriteSchema);
                schema = schema.Replace("</NewDataSet>", ""); // bỏ thẻ đóng của dataset vì đã có trong phần body phía dưới
                string body = ConvertDataTable2XmlString(dtXml, XmlWriteMode.IgnoreSchema);
                body = body.Replace("<NewDataSet>", ""); // bỏ thẻ mở dataset vì đã có trong phần schema phía trên
                strXml = schema + body;

                if (strXml == null || strXml.Length == 0)
                {
                    throw new Exception("Không có dữ liệu");
                }
                // thêm xml version nếu chưa có
                if (!strXml.Contains("<?xml version=\"1.0\" standalone=\"yes\"?>"))
                {
                    strXml = "<?xml version=\"1.0\" standalone=\"yes\"?>\n" + strXml;
                }

                // tạo file từ xml string
                return ExportFileFromString(file_des_fullname, strXml);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Xuất dữ liệu trong bảng GCS_CUSTOMER ra xml
        /// </summary>
        /// <param name="file_des_fullname"></param>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        public string ExportDataTableToXml(string file_des_fullname, DataTable dtXml)
        {
            try
            {
                DataSet dsXml = new DataSet();
                //dtXml.TableName = "Table1";
                dsXml.Tables.Add(dtXml);
                dsXml.WriteXml(file_des_fullname, XmlWriteMode.WriteSchema);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Xuất file có dữ liệu là chuỗi nhập vào
        /// </summary>
        /// <param name="file_des_fullname"></param>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public string ExportFileFromString(string file_des_fullname, string strXml)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(file_des_fullname))
                {
                    writer.Write(strXml);
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Convert Datatable sang chuỗi Xml
        /// </summary>
        /// <param name="dtXml"></param>
        /// <param name="xml_write_mode"></param>
        /// <returns></returns>
        public string ConvertDataTable2XmlString(DataTable dtXml, XmlWriteMode xml_write_mode)
        {
            string strXml = null;
            using (StringWriter sw = new StringWriter())
            {
                dtXml.WriteXml(sw, xml_write_mode, false);// chuyển  dữ liệu ở trong datatable (ko lấy schema) thành string
                strXml = sw.ToString();
            }
            return strXml;
        }

        /// <summary>
        /// Kiểm tra giá trị có phải là số ko
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumberValue(string strNumber, out decimal number)
        {
            string value = strNumber;
            return decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out number);
        }
        /// <summary>
        /// Kiểm tra loại biến có phải là kiểu số ko
        /// </summary>
        /// <param name="typeNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Kiểm tra db sqlite và table đã có trên PC chưa, nếu chưa thì tạo mới
        /// </summary>
        /// <param name="DBSqlite"></param>
        /// <returns></returns>
        //public string CheckExistDbSqlite(string dBSqlite, bool recreate)
        //{
        //    var fi = new FileInfo(dBSqlite);
        //    return CheckExistDbSqlite(fi, recreate);
        //}
        //public string CheckExistDbSqlite(FileInfo dbSqlite, bool recreate)
        //{
          
        //    try
        //    {
        //        using (var sqliteDao = new SQLite_GCS_DAO(dbSqlite.FullName))
        //        {
        //            string result;
        //            if (!Directory.Exists(dbSqlite.DirectoryName))
        //                Directory.CreateDirectory(dbSqlite.DirectoryName);

        //            if (recreate)
        //                dbSqlite.Delete();

        //            var resultOpen = sqliteDao.OpenConn();
        //            if (resultOpen != null && resultOpen.Contains("database disk image is malformed"))
        //            {
        //                throw new Exception(resultOpen);
        //            }

        //            if (!dbSqlite.Exists)
        //            {
        //                // tạo db
        //                result = sqliteDao.CreateDBSQLite(dbSqlite.FullName);
        //                if (result != null)
        //                    return result;
        //            }

        //            // tạo table GCS_CHISO_HHU
        //            result = sqliteDao.CreateTableSqlite_GCS_CHISO_HHU(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // tạo table GCS_SO_NVGCS
        //            result = sqliteDao.CreateTableSqlite_GCS_SO_NVGCS(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // tạo table gcsindex
        //            result = sqliteDao.CreateTableSqlite_gcsindex(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // tạo table GCS_LO_TRINH
        //            result = sqliteDao.CreateTableSqlite_GCS_LO_TRINH(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // tạo table GCS_CUSTOMER
        //            result = sqliteDao.CreateTableSqlite_GCS_CUSTOMER(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // tạo table GCS_LOG_DELETE
        //            result = sqliteDao.CreateTableSqlite_GCS_LOG_DELETE(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            // Tạo bảng tạm nếu chưa có
        //            result = sqliteDao.CreateTableSqlite_GCS_CHISO_HHUTemp(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;
        //            // Tạo bảng của Thanh Hóa
        //            result = sqliteDao.CreateTableSqlite_GCS_TH(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;
        //            return (result == "0" || result == "exist") ? "true" : result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = "Lỗi create DB :" + ex.Message;
        //        return message;
        //    }
        //}

        /// <summary>
        /// Kiểm tra db tọa độ tồn tại
        /// </summary>
        /// <param name="DBSqlite"></param>
        /// <param name="recreate"></param>
        /// <returns></returns>
        //public static string CheckExistDbSqliteToaDo(string strDBToaDo, bool recreate)
        //{
        //    FileInfo DBSqlite = new FileInfo(strDBToaDo);
        //    string result = null;
        //    try
        //    {
        //        if (!Directory.Exists(DBSqlite.DirectoryName))
        //            Directory.CreateDirectory(DBSqlite.DirectoryName);

        //        if (recreate)
        //            DBSqlite.Delete();

        //        using (SQLite_GCS sqlite_dao = new SQLite_GCS(DBSqlite.FullName))
        //        {
        //            sqlite_dao.OpenConn();
        //            if (!DBSqlite.Exists)
        //            {
        //                // tạo db
        //                result = sqlite_dao.CreateDBSQLite(DBSqlite.FullName);
        //                if (result != null)
        //                    return result;
        //            }

        //            // tạo table GCS_CHISO_HHU
        //            result = sqlite_dao.CreateTableSqlite_GCS_TOADO(recreate);
        //            if (result != "0" && result != "exist")//fail
        //                return result;

        //            if (result == null || result == "0" || result == "exist")
        //                return "true";
        //            else
        //                return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        /// <summary>
        /// Hiện label có màu tùy theo trạng thái
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="msg"></param>
        /// <param name="error"></param>
        public void ShowLabel(Label lbl, String msg, string type)
        {
            lbl.Text = msg;

            if (type == "success")
            {
                lbl.ForeColor = Color.Lime;
            }
            else if (type == "error")
            {
                lbl.ForeColor = Color.Red;
            }
            else
            {
                lbl.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Xóa dữ liệu trong db sqlite trên PC
        /// </summary>
        /// <param name="list_so_gcs"></param>
        /// <param name="?"></param>
        //public static string DeleteDataGCSSqliteDb(SQLite_GCS_DAO db_sqlite, string[] arr_ten_file, bool isDeleteAll)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string list_file = null;
        //        if (db_sqlite == null)
        //            throw new Exception("Không tồn tại cơ sở dữ liệu để xóa");

        //        if (arr_ten_file != null && arr_ten_file.Length > 0) // xóa những file được chọn
        //        {
        //            list_file = "";
        //            foreach (string ten_file in arr_ten_file)
        //            {
        //                list_file += ",'" + ten_file + "'";
        //            }
        //            list_file = list_file.Substring(1); // bỏ dấu phẩy ở vị trí đầu tiên
        //            result = DeleteDataGCSSqliteDb(db_sqlite, list_file, isDeleteAll);
        //            if (!result)
        //            {
        //                throw new Exception("Xóa dữ liệu thất bại");
        //            }
        //        }
        //        else // xóa hết
        //        {
        //            result = DeleteDataGCSSqliteDb(db_sqlite, list_file, isDeleteAll);
        //            if (!result)
        //            {
        //                throw new Exception("Xóa dữ liệu thất bại");
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //public static bool DeleteDataGCSSqliteDb(SQLite_GCS_DAO db_temp, string list_ten_file, bool isDeleteAll)
        //{
        //    try
        //    {
        //        bool result = false;
        //        if (list_ten_file != null && list_ten_file.Trim().Length > 0) // xóa những file được chọn
        //        {
        //            result = db_temp.Delete("GCS_CHISO_HHU", "TEN_FILE IN (" + list_ten_file + ")");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //            result = db_temp.Delete("GCS_SO_NVGCS", "TEN_SOGCS IN (" + list_ten_file + ")");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //            result = db_temp.Delete("gcsindex", "so IN (" + list_ten_file + ")");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //        }
        //        else if (isDeleteAll) // xóa hết
        //        {
        //            result = db_temp.ClearTable("GCS_CHISO_HHU");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //            result = db_temp.ClearTable("GCS_SO_NVGCS");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //            result = db_temp.ClearTable("gcsindex");
        //            if (!result)
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Lấy danh sách mã quyển không trùng
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[] GetMaQuyen(DataTable dt)
        {
            var arrDVI = dt.AsEnumerable().Select(c => c.Field<string>("MA_QUYEN")).Distinct().ToArray();
            return arrDVI;
        }

        /// <summary>
        /// Lấy ra tên sổ nếu sổ theo mẫu dùng để giao sổ
        /// </summary>
        /// <param name="TEN_FILE"></param>
        /// <returns></returns>
        public static string ParseTenSo(string TEN_FILE)
        {
            string suffix = @"(_\d{4,5})\.[xX][mM][lL]";
            string input = TEN_FILE.ToUpper();
            string name = TEN_FILE;
            if (Regex.IsMatch(input, suffix))
            {
                // nếu theo mẫu thì bỏ phần đuôi _4311.xml
                name = Regex.Replace(input, suffix, "");
            }
            return name;
        }

        /// <summary>
        /// Xuất dữ liệu khách hàng mới ra file XML
        /// </summary>
        /// <param name="db_sqlite"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        //public static string ExportNewCustomerToXml(SQLiteDatabase db_sqlite, ConfigInfo config)
        //{
        //    string query = "SELECT * FROM GCS_CUSTOMER";
        //    DataTable dtCustomerXml = db_sqlite.ExecuteQuery(query);

        //    if (dtCustomerXml != null && dtCustomerXml.Rows.Count > 0)
        //    {
        //        DataSet dsCustomerXml = new DataSet();
        //        dsCustomerXml.Tables.Add(dtCustomerXml);
        //        string str_kh_moi_folder = JoinStringPath(new string[] { config.NEW_CUSTOMER_FOLDER, DateTime.Now.ToString("yyyy/MM/dd/", CultureInfo.CurrentCulture) });
        //        DirectoryInfo kh_moi_Folder = new DirectoryInfo(str_kh_moi_folder);
        //        if (!kh_moi_Folder.Exists)
        //            kh_moi_Folder.Create();
        //        dsCustomerXml.WriteXml(JoinStringPath(new string[] { str_kh_moi_folder, "kh-moi-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xml" }));
        //        return null;
        //    }
        //    return "HAVE_NO_NEW_CUSTOMER";
        //}

        /// <summary>
        /// Lấy dữ liệu khách hàng mới trong folder
        /// </summary>
        /// <param name="dateGetFile"></param>
        /// <param name="time"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        //public static FileInfo[] GetNewCustomerFileInFolder(DateTime dateGetFile, string time, ConfigInfo config)
        //{
        //    try
        //    {
        //        FileInfo[] customer_files = null;
        //        string str_folder = config.NEW_CUSTOMER_FOLDER;
        //        if (time == "MONTH")
        //        {
        //            str_folder = JoinStringPath(new string[] { str_folder, dateGetFile.ToString("yyyy/MM") });
        //        }
        //        else if (time == "YEAR")
        //        {
        //            str_folder = JoinStringPath(new string[] { str_folder, dateGetFile.ToString("yyyy") });
        //        }
        //        else
        //        {
        //            str_folder = JoinStringPath(new string[] { str_folder, dateGetFile.ToString("yyyy/MM/dd") });
        //        }
        //        DirectoryInfo customer_folder = new DirectoryInfo(str_folder);
        //        if (!customer_folder.Exists)
        //        {
        //            return null;
        //        }
        //        customer_files = customer_folder.GetFiles("kh-moi-" + "*" + ".xml", SearchOption.AllDirectories);
        //        return customer_files;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Copy file sqlite từ PC -> thiết bị Android
        /// </summary>
        //public static string CopySqliteDbToAndroidDevice(string mobile_folder, string db_pc)
        //{
        //    try
        //    {
        //        FileInfo file_db_pc = new FileInfo(db_pc);
        //        MTP_Connection mtp = new MTP_Connection();

        //        //Nếu chuyển bằng kiểu kết nối USB Storage
        //        if (mobile_folder.ToString().Contains(":"))
        //        {
        //            //Nếu không thấy file db sqlite trên thiết bị android
        //            if (!file_db_pc.Exists)
        //                throw new Exception("Không có file cơ sở dữ liệu");
        //            //Copy vào máy android
        //            file_db_pc.CopyTo(Common.JoinStringPath(mobile_folder, file_db_pc.Name), true);
        //        }
        //        else // nếu chuyển bằng kiểu kết nối MTP
        //        {
        //            Folder folder_mtb_ns = mtp.Convert2ShellFolder(mobile_folder);
        //            FolderItem item = null;

        //            // chuyển file vào máy android
        //            item = mtp.GetFolderItem(db_pc);
        //            folder_mtb_ns.CopyHere(item, MTP_Connection.CopyHereOptions.AutoYesToAll);
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        /// <summary>
        /// Xóa các sổ đang được chọn trên mobile
        /// </summary>
        /// <returns></returns>
        //public static string DeleteItemFromMobile(string folder_mobile, string db_sqlite_pc_temp, List<SoInfo_DTO> list_so_gcs, bool isDelAll)
        //{
        //    string result = null;
        //    string db_sqlite_mobile_folder = Common.JoinStringPath(folder_mobile, clsConstants.MOBILE_DB_FOLDER);
        //    string mobile_backup_folder = Common.JoinStringPath(folder_mobile, clsConstants.MOBILE_BACKUP_FOLDER);
        //    string mobile_db_file = Common.JoinStringPath(folder_mobile, clsConstants.MOBILE_DB_FOLDER, "ESGCS.s3db");
        //    DataSet dsXml = new DataSet();
        //    MTP_Connection mtp = new MTP_Connection();
        //    // kết nối db_temp
        //    SQLite_GCS_DAO db_temp = new SQLite_GCS_DAO(db_sqlite_pc_temp);

        //    try
        //    {
        //        string list_file = null;
        //        db_temp.OpenConn();
        //        db_temp.BeginTransaction();
        //        if (!isDelAll)
        //        {
        //            list_file = "";
        //            // lấy danh sách sổ đang chọn
        //            foreach (SoInfo_DTO so in list_so_gcs)
        //            {
        //                list_file += "'" + SQLite_GCS_DAO.Escape(so.TEN_FILE) + "',";
        //            }
        //            if (list_file.Trim().Length > 0)
        //                list_file = list_file.Substring(0, list_file.Trim().Length - 1);

        //            DeleteDataGCSSqliteDb(db_temp, list_file, false);
        //        }
        //        else
        //        {
        //            DeleteDataGCSSqliteDb(db_temp, list_file, true);
        //        }
        //        db_temp.CommitTransaction();

        //        // chuyển dữ liệu cũ trên thiết bị Android vào backup để khi copy dữ liệu mới ko hiện thông báo "Copy and replace"
        //        //mtp.MoveFile(new string[,] { { mobile_db_file, "ESGCS" + DateTime.Now.ToString("_yyyyMMdd_HHmmss", CultureInfo.CurrentCulture) + ".s3db" } }, mobile_backup_folder);

        //        // copy lại vào thiết bị Android
        //        result = Common.CopySqliteDbToAndroidDevice(db_sqlite_mobile_folder, db_sqlite_pc_temp);
        //        if (result != null)
        //        {
        //            throw new Exception("Copy cơ sở dữ liệu ra mobile thất bại.\n" + result);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        db_temp.RollbackTransaction();
        //        return ex.Message;
        //    }
        //    finally
        //    {
        //        if (db_temp != null)
        //            db_temp.Dispose();
        //    }
        //}

        /// <summary>
        /// Nén file
        /// </summary>
        public static void CompressionFile(string[] full_source_file_name, string save_name)
        {
            using (ZipFile zip = new ZipFile(save_name))
            {
                foreach (string filename in full_source_file_name)
                {
                    zip.AddFile(filename);
                }
                zip.Save();
            }
        }

        /// <summary>
        /// Nén folder
        /// </summary>
        /// <param name="full_source_file_name"></param>
        /// <param name="save_name"></param>
        public static void CompressionFolder(string[] full_source_file_name, string save_name)
        {
            using (ZipFile zip = new ZipFile(save_name))
            {
                zip.AddFiles(full_source_file_name, false, "");
                zip.Save();
            }
        }
        public static string CompressionFolder(string WrapFolderName, string ZipFullName)
        {
            try
            {
                string ext = Path.GetExtension(ZipFullName);
                if (ext.Length == 0)
                {
                    ZipFullName += ".zip";
                }
                if (ext.ToLower() != ".zip")
                {
                    Regex.Replace(ZipFullName, ext, ".zip", RegexOptions.IgnoreCase);
                }

                using (var zip = new Ionic.Zip.ZipFile())
                {
                    zip.AddDirectory(WrapFolderName);
                    zip.Save(ZipFullName);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Enable/Disable cả form khi process để tránh thao tác thêm khi chương trình đang xử lý chưa xong 
        /// </summary>
        /// <param name="frm">Form cần xử lý</param>
        /// <param name="isEnabled">Trạng thái form. True: form enable</param>
        public static void FormEnable(Form frm, bool isEnabled)
        {
            frm.Enabled = isEnabled;
            Cursor.Current = isEnabled ? null : Cursors.WaitCursor;
        }

        /// <summary>
        /// Xóa dữ liệu trong datagridview
        /// </summary>
        /// <param name="dtg"></param>
        public static void ClearDataGridViewDataSource(DataGridView dtg)
        {
            if (dtg.DataSource != null)
            {
                try
                {
                    DataSet ds = (DataSet)dtg.DataSource;
                    ds.Clear();
                }
                catch
                {
                    DataTable dt = (DataTable)dtg.DataSource;
                    dt.Rows.Clear();
                }
            }
        }

        /// <summary>
        /// Nén thư mục chứa ảnh thành file .zip
        /// </summary>
        /// <param name="full_source_file_name">Đường dẫn thư mục chứa ảnh</param>
        /// <param name="PathArchiveFile">Đường dẫn chứa file nén trên PC</param>
        /// <param name="ArchiveName">Tên thư mục bên trong file nén</param>
        public void CompressionFolderFTP(string full_source_file_name, string PathArchiveFile, string ArchiveName)
        {
            //for (int i = 0; i < full_source_file_name.Length; i++)
            //{
            //    using (var zip = new Ionic.Zip.ZipFile())
            //    {
            //        zip.AddDirectory(full_source_file_name[i], ArchiveName[i]);
            //        zip.Save(PathArchiveFile + ArchiveName[i]);
            //    }
            //}
            using (var zip = new Ionic.Zip.ZipFile())
            {
                zip.AddDirectory(full_source_file_name);
                zip.Save(PathArchiveFile + ArchiveName + ".zip");
            }
        }

        public string UploadZipFileToFTP(string source, string ArchiveName, string DiaChiFTP, string UserFTP, string PassFTP)
        {
            string rs = "";
            Stream ftpstream = null;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(DiaChiFTP + "/" + ArchiveName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(UserFTP, PassFTP); // e.g. username & e.g. password

                request.KeepAlive = false;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UsePassive = true;

                FileStream fs = File.OpenRead(source);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                ftpstream = request.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                // fix thời gian chờ 27MB ~ 10s
                Thread.Sleep(buffer.Length / 27000);
                ftpstream.Close();
                response = (FtpWebResponse)request.GetResponse();

                response.Close();
                return rs;
            }
            catch (Exception ex)
            {
                ftpstream.Close();
                response.Close();
                return "UploadZipFileToFTP " + ex.Message;
            }
        }
        public DataTable CreateDataInput(string ImagePath,DataTable dtInput,string ma_dviqly,ref string error)
        {
            try
            {
                string file_name = Path.GetFileNameWithoutExtension(ImagePath);
                //Thêm row cho dataset đầu vào service insert log ảnh
                DataTable dtResult = dtInput;
                DataRow dr = dtInput.NewRow();
                string[] ctiet_fn = file_name.Split('_');
                dr["MA_SOGCS"] = ctiet_fn[0].ToString();
                dr["NAM"] = ctiet_fn[2].ToString();
                dr["THANG"] = ctiet_fn[3].ToString();
                dr["KY"] = ctiet_fn[4].ToString();
                dr["MA_DDO"] = ctiet_fn[5].ToString();
                dr["BO_CHISO"] = ctiet_fn[6].ToString();
                dr["MA_DVIQLY"] = ma_dviqly;
                dtResult.Rows.Add(dr);
                error = null;
                return dtResult;
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return null; 
            }
        }
        public string TaoDuongDanAnh(string DuongDanAnhCu, string DuongDanAnhMoi, ref string message,ref DataTable dtInput,string maDonVi)
        {
            try
            {
                string ky = "", thang = "", nam = "", ma_ddo = "", bcs = "", rs = "";
                string file_name = Path.GetFileNameWithoutExtension(DuongDanAnhCu);

                // Cắt tên ảnh ra để tạo thư mục
                string[] ctiet_fn = file_name.Split('_');
                nam = ctiet_fn[2].ToString();
                thang = ctiet_fn[3].ToString();
                ky = ctiet_fn[4].ToString();
                ma_ddo = ctiet_fn[5].ToString();
                bcs = ctiet_fn[6].ToString();
                string DirPath = DuongDanAnhMoi + "\\" + nam + "\\" + thang + "\\" + ky + "\\" + ma_ddo + "\\";
                string image_path = DirPath + bcs + Path.GetExtension(DuongDanAnhCu);
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);
                File.Copy(DuongDanAnhCu, image_path);
                DataTable dtResult = dtInput;
                DataRow dr = dtInput.NewRow();
                dr["MA_SOGCS"] = ctiet_fn[0].ToString();
                dr["NAM"] = ctiet_fn[2].ToString();
                dr["THANG"] = ctiet_fn[3].ToString();
                dr["KY"] = ctiet_fn[4].ToString();
                dr["MA_DDO"] = ctiet_fn[5].ToString();
                dr["BO_CHISO"] = ctiet_fn[6].ToString();
                dr["MA_DVIQLY"] = maDonVi;
                dr["TTHAI_GUI"] = "1";
                dtResult.Rows.Add(dr);
                return rs;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }
        public DataTable CREATE_GCS_LOG_ANH_TABLE()
        {
            DataTable LogAnhTable = new DataTable();
            LogAnhTable.Columns.Add("MA_DVIQLY", typeof(String));
            LogAnhTable.Columns.Add("MA_DDO", typeof(String));
            LogAnhTable.Columns.Add("BO_CHISO", typeof(String));
            LogAnhTable.Columns.Add("MA_SOGCS", typeof(String));
            LogAnhTable.Columns.Add("KY", typeof(Int32));
            LogAnhTable.Columns.Add("THANG", typeof(Int32));
            LogAnhTable.Columns.Add("NAM", typeof(Int32));
            LogAnhTable.Columns.Add("TTHAI_GUI", typeof(String));
            return LogAnhTable;
        }
        public string TaoDuongDanAnhKhongSplit(string nam, string thang, string ky, string ma_ddo, string bcs, string DuongDanAnhCu, string DuongDanAnhMoi)
        {
            try
            {
                string rs = "";
                //string file_name = Path.GetFileNameWithoutExtension(DuongDanAnhCu);

                // Cắt tên ảnh ra để tạo thư mục
                //string[] ctiet_fn = file_name.Split('_');
                //nam = ctiet_fn[2].ToString();
                //thang = ctiet_fn[3].ToString();
                //ky = ctiet_fn[4].ToString();
                //ma_ddo = ctiet_fn[5].ToString();
                //bcs = ctiet_fn[6].ToString();

                string DirPath = DuongDanAnhMoi + "\\" + nam + "\\" + thang + "\\" + ky + "\\" + ma_ddo + "\\";
                string image_path = DirPath + bcs + Path.GetExtension(DuongDanAnhCu);
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);
                File.Copy(DuongDanAnhCu, image_path);
                return rs;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        public string CheckConntecion(string DiaChiFTP, string UserFTP, string PassFTP)
        {
            string rs = "";
            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(DiaChiFTP);
            requestDir.Method = WebRequestMethods.Ftp.ListDirectory;
            requestDir.Credentials = new NetworkCredential(UserFTP, PassFTP);
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse())
                {
                    response.Close();
                }
                return rs;
            }
            catch (Exception ex)
            {
                return "Kết nối server thất bại.\n" + ex.Message;
            }
        }

        private string CreateDirectoryFTP(string ky, string thang, string nam, string ma_ddo, string DiaChiFTP, string UserFTP, string PassFTP)
        {
            try
            {
                string[] subfolders = new string[] { nam, thang, ky, ma_ddo };
                string ftpfullpath = DiaChiFTP + "/";
                for (int i = 0; i < subfolders.Length; i++)
                {
                    if (i == subfolders.Length - 1)
                        ftpfullpath += subfolders[i]; // e.g. ftp://serverip/foldername/foldername
                    else ftpfullpath += subfolders[i] + "/"; // e.g. ftp://serverip/foldername/foldername
                    if (FtpDirectoryExists(ftpfullpath, UserFTP, PassFTP) ==
                        "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).") // Check xem đã tồn tại thư mục trên FTP chưa
                    {
                        FtpWebRequest create = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                        create.Method = WebRequestMethods.Ftp.MakeDirectory;
                        create.Credentials = new NetworkCredential(UserFTP, PassFTP); // e.g. username & e.g. password
                        var resp = (FtpWebResponse)create.GetResponse();
                        resp.Close();
                    }
                    else if (FtpDirectoryExists(ftpfullpath, UserFTP, PassFTP) != "")
                    {
                        return FtpDirectoryExists(ftpfullpath, UserFTP, PassFTP);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            //bool IsExists = true;
            string rs = "";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                return ex.Message;
                //IsExists = false;
            }
            return rs;
        }

        public string UploadFileToFTP(string source, string DiaChiFTP, string UserFTP, string PassFTP)
        {
            try
            {
                string ky = "", thang = "", nam = "", ma_ddo = "", bcs = "", rs = "";
                string file_name = Path.GetFileNameWithoutExtension(source);
                string[] ctiet_fn = file_name.Split('_');
                nam = ctiet_fn[2].ToString();
                thang = ctiet_fn[3].ToString();
                ky = ctiet_fn[4].ToString();
                ma_ddo = ctiet_fn[5].ToString();
                bcs = ctiet_fn[6].ToString();

                rs = CreateDirectoryFTP(ky, thang, nam, ma_ddo, DiaChiFTP, UserFTP, PassFTP); // tạo thư mục trên FTP
                if (rs != "")
                {
                    //if (rs == "Unable to connect to the remote server")
                    //{
                    //    MessageBox.Show("Mất kết nối đến Server! \r\n(Unable to connect to the remote server)", "Thông báo");
                    //}
                    //else MessageBox.Show("Lỗi: " + rs, "Thông báo");
                    return rs;
                }

                //string filename = Path.GetFileName(source);

                //string ftpfullpath = "ftp://10.9.8.205/2015/9/1/PD3000T0392031"; // e.g. ftp://serverip/foldername/foldername
                string ftpfullpath = DiaChiFTP + "/" + nam + "/" + thang + "/" + ky + "/" + ma_ddo; // e.g. ftp://serverip/foldername/foldername
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath + "/" + bcs + ".jpg");
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(UserFTP, PassFTP); // e.g. username & e.g. password

                request.KeepAlive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(source);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = request.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                return rs;
            }
            catch (Exception ex)
            {
                //if (ex.Message == "Unable to connect to the remote server")
                //    MessageBox.Show("Mất kết nối đến Server! \r\n(Unable to connect to the remote server)", "Thông báo");
                //else MessageBox.Show("Lỗi: " + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Giải nén file
        /// </summary>
        public static string ExtractFile(string file_name, string path_extract)
        {
            try
            {
                if (!System.IO.File.Exists(file_name))
                {
                    return "NOT_EXIST";
                }

                var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
                using (ZipFile zip = ZipFile.Read(file_name, options))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractAll(path_extract);
                }
                return null;
            }
            catch (System.Exception ex1)
            {
                return "EXCEPTION: " + ex1.Message;
            }
        }

        public static string ExtractFTPfile(string file_name, string path_extract)
        {
            try
            {
                if (!System.IO.File.Exists(file_name))
                {
                    return "NOT_EXIST";
                }
                //FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(DiaChiFTP + "/" + file_name);
                //request.Credentials = new NetworkCredential(UserFTP, PassFTP); // e.g. username & e.g. password
                var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
                using (ZipFile zip = ZipFile.Read(file_name, options))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractAll(path_extract);
                }
                return null;
            }
            catch (System.Exception ex1)
            {
                return "EXCEPTION: " + ex1.Message;
            }
        }

        public void AddPharseToTable(ref DataTable dt,DataSet ds)
        {
           
            foreach (DataRow row in dt.Rows)
            {
                var maCloai = row["MA_CTO"].ToString().Substring(0, 3);
                try
                {
                    foreach (var row1 in ds.Tables[0].Rows.Cast<DataRow>().Where(row1 => maCloai == row1["MA_CLOAI"].ToString().Trim()))
                    {
                        row["SERY_CTO"] += "/" + row1["SO_PHA"].ToString().Trim() + " pha";
                    }
                }
                catch
                {
                    row["SERY_CTO"] += "/" + "1 pha";
                }
               
            }
        }
        public DataSet DsChungLoaiCongTo()
        {
            var dsClct = new DataSet();
            try
            {
                var dsChungLoaiCongTo = Application.StartupPath + @"\DSChungLoaiCongTo.xml";
                if (!File.Exists(dsChungLoaiCongTo))
                {
                    var dtClct = new DataTable("DSChungLoaiCongTo");
                    dtClct.Columns.Add("MA_CLOAI", typeof (string));
                    dtClct.Columns.Add("SO_PHA", typeof (string));

                    var row1 = dtClct.NewRow();
                    var row2 = dtClct.NewRow();
                    var row3 = dtClct.NewRow();
                    var row4 = dtClct.NewRow();
                    var row5 = dtClct.NewRow();
                    var row6 = dtClct.NewRow();
                    var row7 = dtClct.NewRow();
                    var row8 = dtClct.NewRow();
                    var row9 = dtClct.NewRow();
                    var row10 = dtClct.NewRow();
                    var row11 = dtClct.NewRow();
                    var row12 = dtClct.NewRow();
                    var row13 = dtClct.NewRow();
                    var row14 = dtClct.NewRow();
                    var row15 = dtClct.NewRow();
                    var row16 = dtClct.NewRow();
                    var row17 = dtClct.NewRow();
                    var row18 = dtClct.NewRow();
                    var row19 = dtClct.NewRow();
                    var row20 = dtClct.NewRow();
                    var row21 = dtClct.NewRow();
                    var row22 = dtClct.NewRow();
                    var row23 = dtClct.NewRow();
                    var row24 = dtClct.NewRow();
                    var row25 = dtClct.NewRow();
                    var row26 = dtClct.NewRow();
                    var row27 = dtClct.NewRow();
                    var row28 = dtClct.NewRow();
                    var row29 = dtClct.NewRow();
                    var row30 = dtClct.NewRow();
                    var row31 = dtClct.NewRow();
                    var row32 = dtClct.NewRow();
                    var row33 = dtClct.NewRow();
                    var row34 = dtClct.NewRow();
                    var row35 = dtClct.NewRow();
                    var row36 = dtClct.NewRow();
                    var row37 = dtClct.NewRow();
                    var row38 = dtClct.NewRow();
                    var row39 = dtClct.NewRow();
                    var row40 = dtClct.NewRow();
                    var row41 = dtClct.NewRow();
                    var row42 = dtClct.NewRow();
                    var row43 = dtClct.NewRow();
                    var row44 = dtClct.NewRow();
                    var row45 = dtClct.NewRow();
                    var row46 = dtClct.NewRow();
                    var row47 = dtClct.NewRow();
                    var row48 = dtClct.NewRow();
                    var row49 = dtClct.NewRow();
                    var row50 = dtClct.NewRow();
                    var row51 = dtClct.NewRow();
                    var row52 = dtClct.NewRow();
                    var row53 = dtClct.NewRow();
                    var row54 = dtClct.NewRow();
                    var row55 = dtClct.NewRow();
                    var row56 = dtClct.NewRow();
                    var row57 = dtClct.NewRow();
                    var row58 = dtClct.NewRow();
                    var row59 = dtClct.NewRow();
                    var row60 = dtClct.NewRow();
                    var row61 = dtClct.NewRow();
                    var row62 = dtClct.NewRow();
                    var row63 = dtClct.NewRow();
                    var row64 = dtClct.NewRow();
                    var row65 = dtClct.NewRow();
                    var row66 = dtClct.NewRow();
                    var row67 = dtClct.NewRow();
                    var row68 = dtClct.NewRow();
                    var row69 = dtClct.NewRow();
                    var row70 = dtClct.NewRow();
                    var row71 = dtClct.NewRow();
                    var row72 = dtClct.NewRow();
                    var row73 = dtClct.NewRow();
                    var row74 = dtClct.NewRow();
                    var row75 = dtClct.NewRow();
                    var row76 = dtClct.NewRow();
                    var row77 = dtClct.NewRow();
                    var row78 = dtClct.NewRow();
                    var row79 = dtClct.NewRow();
                    var row80 = dtClct.NewRow();
                    var row81 = dtClct.NewRow();
                    var row82 = dtClct.NewRow();
                    var row83 = dtClct.NewRow();
                    var row84 = dtClct.NewRow();
                    var row85 = dtClct.NewRow();
                    var row86 = dtClct.NewRow();
                    var row87 = dtClct.NewRow();
                    var row88 = dtClct.NewRow();
                    var row89 = dtClct.NewRow();
                    var row90 = dtClct.NewRow();
                    var row91 = dtClct.NewRow();
                    var row92 = dtClct.NewRow();
                    var row93 = dtClct.NewRow();
                    var row94 = dtClct.NewRow();
                    var row95 = dtClct.NewRow();
                    var row96 = dtClct.NewRow();
                    var row97 = dtClct.NewRow();
                    var row98 = dtClct.NewRow();
                    var row99 = dtClct.NewRow();
                    var row100 = dtClct.NewRow();
                    var row101 = dtClct.NewRow();
                    var row102 = dtClct.NewRow();
                    var row103 = dtClct.NewRow();
                    var row104 = dtClct.NewRow();
                    var row105 = dtClct.NewRow();
                    var row106 = dtClct.NewRow();
                    var row107 = dtClct.NewRow();
                    var row108 = dtClct.NewRow();
                    var row109 = dtClct.NewRow();
                    var row110 = dtClct.NewRow();
                    var row111 = dtClct.NewRow();
                    var row112 = dtClct.NewRow();
                    var row113 = dtClct.NewRow();
                    var row114 = dtClct.NewRow();
                    var row115 = dtClct.NewRow();
                    var row116 = dtClct.NewRow();
                    var row117 = dtClct.NewRow();
                    var row118 = dtClct.NewRow();
                    var row119 = dtClct.NewRow();
                    var row120 = dtClct.NewRow();
                    var row121 = dtClct.NewRow();
                    var row122 = dtClct.NewRow();
                    var row123 = dtClct.NewRow();
                    var row124 = dtClct.NewRow();
                    var row125 = dtClct.NewRow();
                    var row126 = dtClct.NewRow();
                    var row127 = dtClct.NewRow();
                    var row128 = dtClct.NewRow();
                    var row129 = dtClct.NewRow();
                    var row130 = dtClct.NewRow();
                    var row131 = dtClct.NewRow();
                    var row132 = dtClct.NewRow();
                    var row133 = dtClct.NewRow();
                    var row134 = dtClct.NewRow();
                    var row135 = dtClct.NewRow();
                    var row136 = dtClct.NewRow();
                    var row137 = dtClct.NewRow();
                    var row138 = dtClct.NewRow();
                    var row139 = dtClct.NewRow();
                    var row140 = dtClct.NewRow();
                    var row141 = dtClct.NewRow();
                    var row142 = dtClct.NewRow();
                    var row143 = dtClct.NewRow();
                    var row144 = dtClct.NewRow();
                    var row145 = dtClct.NewRow();
                    var row146 = dtClct.NewRow();
                    var row147 = dtClct.NewRow();
                    var row148 = dtClct.NewRow();
                    var row149 = dtClct.NewRow();
                    var row150 = dtClct.NewRow();
                    var row151 = dtClct.NewRow();
                    var row152 = dtClct.NewRow();
                    var row153 = dtClct.NewRow();
                    var row154 = dtClct.NewRow();
                    var row155 = dtClct.NewRow();
                    var row156 = dtClct.NewRow();
                    var row157 = dtClct.NewRow();
                    var row158 = dtClct.NewRow();
                    var row159 = dtClct.NewRow();
                    var row160 = dtClct.NewRow();
                    var row161 = dtClct.NewRow();
                    var row162 = dtClct.NewRow();
                    var row163 = dtClct.NewRow();
                    var row164 = dtClct.NewRow();
                    var row165 = dtClct.NewRow();
                    var row166 = dtClct.NewRow();
                    var row167 = dtClct.NewRow();
                    var row168 = dtClct.NewRow();
                    var row169 = dtClct.NewRow();
                    var row170 = dtClct.NewRow();
                    var row171 = dtClct.NewRow();
                    var row172 = dtClct.NewRow();
                    var row173 = dtClct.NewRow();
                    var row174 = dtClct.NewRow();
                    var row175 = dtClct.NewRow();
                    var row176 = dtClct.NewRow();
                    var row177 = dtClct.NewRow();
                    var row178 = dtClct.NewRow();
                    var row179 = dtClct.NewRow();
                    var row180 = dtClct.NewRow();
                    var row181 = dtClct.NewRow();
                    var row182 = dtClct.NewRow();
                    var row183 = dtClct.NewRow();
                    var row184 = dtClct.NewRow();
                    var row185 = dtClct.NewRow();
                    var row186 = dtClct.NewRow();
                    var row187 = dtClct.NewRow();
                    var row188 = dtClct.NewRow();
                    var row189 = dtClct.NewRow();
                    var row190 = dtClct.NewRow();
                    var row191 = dtClct.NewRow();
                    var row192 = dtClct.NewRow();
                    var row193 = dtClct.NewRow();
                    var row194 = dtClct.NewRow();
                    var row195 = dtClct.NewRow();
                    var row196 = dtClct.NewRow();
                    var row197 = dtClct.NewRow();
                    var row198 = dtClct.NewRow();
                    var row199 = dtClct.NewRow();
                    var row200 = dtClct.NewRow();
                    var row201 = dtClct.NewRow();

                    row1["MA_CLOAI"] = "069";
                    row2["MA_CLOAI"] = "365";
                    row3["MA_CLOAI"] = "366";
                    row4["MA_CLOAI"] = "367";
                    row5["MA_CLOAI"] = "399";
                    row6["MA_CLOAI"] = "490";
                    row7["MA_CLOAI"] = "496";
                    row8["MA_CLOAI"] = "497";
                    row9["MA_CLOAI"] = "498";
                    row10["MA_CLOAI"] = "499";
                    row11["MA_CLOAI"] = "500";
                    row12["MA_CLOAI"] = "501";
                    row13["MA_CLOAI"] = "502";
                    row14["MA_CLOAI"] = "503";
                    row15["MA_CLOAI"] = "504";
                    row16["MA_CLOAI"] = "505";
                    row17["MA_CLOAI"] = "506";
                    row18["MA_CLOAI"] = "507";
                    row19["MA_CLOAI"] = "508";
                    row20["MA_CLOAI"] = "509";
                    row21["MA_CLOAI"] = "526";
                    row22["MA_CLOAI"] = "546";
                    row23["MA_CLOAI"] = "558";
                    row24["MA_CLOAI"] = "559";
                    row25["MA_CLOAI"] = "560";
                    row26["MA_CLOAI"] = "561";
                    row27["MA_CLOAI"] = "562";
                    row28["MA_CLOAI"] = "563";
                    row29["MA_CLOAI"] = "564";
                    row30["MA_CLOAI"] = "565";
                    row31["MA_CLOAI"] = "566";
                    row32["MA_CLOAI"] = "567";
                    row33["MA_CLOAI"] = "568";
                    row34["MA_CLOAI"] = "569";
                    row35["MA_CLOAI"] = "570";
                    row36["MA_CLOAI"] = "571";
                    row37["MA_CLOAI"] = "572";
                    row38["MA_CLOAI"] = "573";
                    row39["MA_CLOAI"] = "574";
                    row40["MA_CLOAI"] = "575";
                    row41["MA_CLOAI"] = "576";
                    row42["MA_CLOAI"] = "577";
                    row43["MA_CLOAI"] = "578";
                    row44["MA_CLOAI"] = "579";
                    row45["MA_CLOAI"] = "580";
                    row46["MA_CLOAI"] = "581";
                    row47["MA_CLOAI"] = "582";
                    row48["MA_CLOAI"] = "583";
                    row49["MA_CLOAI"] = "624";
                    row50["MA_CLOAI"] = "625";
                    row51["MA_CLOAI"] = "626";
                    row52["MA_CLOAI"] = "627";
                    row53["MA_CLOAI"] = "628";
                    row54["MA_CLOAI"] = "629";
                    row55["MA_CLOAI"] = "630";
                    row56["MA_CLOAI"] = "631";
                    row57["MA_CLOAI"] = "632";
                    row58["MA_CLOAI"] = "636";
                    row59["MA_CLOAI"] = "637";
                    row60["MA_CLOAI"] = "638";
                    row61["MA_CLOAI"] = "639";
                    row62["MA_CLOAI"] = "640";
                    row63["MA_CLOAI"] = "641";
                    row64["MA_CLOAI"] = "642";
                    row65["MA_CLOAI"] = "643";
                    row66["MA_CLOAI"] = "644";
                    row67["MA_CLOAI"] = "645";
                    row68["MA_CLOAI"] = "646";
                    row69["MA_CLOAI"] = "647";
                    row70["MA_CLOAI"] = "648";
                    row71["MA_CLOAI"] = "649";
                    row72["MA_CLOAI"] = "650";
                    row73["MA_CLOAI"] = "651";
                    row74["MA_CLOAI"] = "652";
                    row75["MA_CLOAI"] = "656";
                    row76["MA_CLOAI"] = "657";
                    row77["MA_CLOAI"] = "658";
                    row78["MA_CLOAI"] = "659";
                    row79["MA_CLOAI"] = "660";
                    row80["MA_CLOAI"] = "661";
                    row81["MA_CLOAI"] = "662";
                    row82["MA_CLOAI"] = "663";
                    row83["MA_CLOAI"] = "664";
                    row84["MA_CLOAI"] = "665";
                    row85["MA_CLOAI"] = "666";
                    row86["MA_CLOAI"] = "667";
                    row87["MA_CLOAI"] = "668";
                    row88["MA_CLOAI"] = "669";
                    row89["MA_CLOAI"] = "670";
                    row90["MA_CLOAI"] = "671";
                    row91["MA_CLOAI"] = "672";
                    row92["MA_CLOAI"] = "673";
                    row93["MA_CLOAI"] = "674";
                    row94["MA_CLOAI"] = "675";
                    row95["MA_CLOAI"] = "676";
                    row96["MA_CLOAI"] = "677";
                    row97["MA_CLOAI"] = "678";
                    row98["MA_CLOAI"] = "679";
                    row99["MA_CLOAI"] = "680";
                    row100["MA_CLOAI"] = "681";
                    row101["MA_CLOAI"] = "682";
                    row102["MA_CLOAI"] = "683";
                    row103["MA_CLOAI"] = "684";
                    row104["MA_CLOAI"] = "685";
                    row105["MA_CLOAI"] = "686";
                    row106["MA_CLOAI"] = "687";
                    row107["MA_CLOAI"] = "688";
                    row108["MA_CLOAI"] = "689";
                    row109["MA_CLOAI"] = "690";
                    row110["MA_CLOAI"] = "691";
                    row111["MA_CLOAI"] = "692";
                    row112["MA_CLOAI"] = "693";
                    row113["MA_CLOAI"] = "694";
                    row114["MA_CLOAI"] = "695";
                    row115["MA_CLOAI"] = "696";
                    row116["MA_CLOAI"] = "697";
                    row117["MA_CLOAI"] = "724";
                    row118["MA_CLOAI"] = "725";
                    row119["MA_CLOAI"] = "726";
                    row120["MA_CLOAI"] = "729";
                    row121["MA_CLOAI"] = "730";
                    row122["MA_CLOAI"] = "731";
                    row123["MA_CLOAI"] = "732";
                    row124["MA_CLOAI"] = "733";
                    row125["MA_CLOAI"] = "734";
                    row126["MA_CLOAI"] = "735";
                    row127["MA_CLOAI"] = "736";
                    row128["MA_CLOAI"] = "737";
                    row129["MA_CLOAI"] = "738";
                    row130["MA_CLOAI"] = "739";
                    row131["MA_CLOAI"] = "740";
                    row132["MA_CLOAI"] = "741";
                    row133["MA_CLOAI"] = "742";
                    row134["MA_CLOAI"] = "743";
                    row135["MA_CLOAI"] = "744";
                    row136["MA_CLOAI"] = "745";
                    row137["MA_CLOAI"] = "746";
                    row138["MA_CLOAI"] = "747";
                    row139["MA_CLOAI"] = "748";
                    row140["MA_CLOAI"] = "749";
                    row141["MA_CLOAI"] = "750";
                    row142["MA_CLOAI"] = "751";
                    row143["MA_CLOAI"] = "752";
                    row144["MA_CLOAI"] = "753";
                    row145["MA_CLOAI"] = "754";
                    row146["MA_CLOAI"] = "755";
                    row147["MA_CLOAI"] = "756";
                    row148["MA_CLOAI"] = "757";
                    row149["MA_CLOAI"] = "758";
                    row150["MA_CLOAI"] = "759";
                    row151["MA_CLOAI"] = "760";
                    row152["MA_CLOAI"] = "761";
                    row153["MA_CLOAI"] = "762";
                    row154["MA_CLOAI"] = "763";
                    row155["MA_CLOAI"] = "764";
                    row156["MA_CLOAI"] = "765";
                    row157["MA_CLOAI"] = "766";
                    row158["MA_CLOAI"] = "767";
                    row159["MA_CLOAI"] = "768";
                    row160["MA_CLOAI"] = "769";
                    row161["MA_CLOAI"] = "770";
                    row162["MA_CLOAI"] = "771";
                    row163["MA_CLOAI"] = "772";
                    row164["MA_CLOAI"] = "773";
                    row165["MA_CLOAI"] = "774";
                    row166["MA_CLOAI"] = "775";
                    row167["MA_CLOAI"] = "776";
                    row168["MA_CLOAI"] = "777";
                    row169["MA_CLOAI"] = "778";
                    row170["MA_CLOAI"] = "779";
                    row171["MA_CLOAI"] = "780";
                    row172["MA_CLOAI"] = "781";
                    row173["MA_CLOAI"] = "782";
                    row174["MA_CLOAI"] = "783";
                    row175["MA_CLOAI"] = "784";
                    row176["MA_CLOAI"] = "785";
                    row177["MA_CLOAI"] = "786";
                    row178["MA_CLOAI"] = "787";
                    row179["MA_CLOAI"] = "788";
                    row180["MA_CLOAI"] = "789";
                    row181["MA_CLOAI"] = "790";
                    row182["MA_CLOAI"] = "791";
                    row183["MA_CLOAI"] = "792";
                    row184["MA_CLOAI"] = "797";
                    row185["MA_CLOAI"] = "798";
                    row186["MA_CLOAI"] = "799";
                    row187["MA_CLOAI"] = "801";
                    row188["MA_CLOAI"] = "802";
                    row189["MA_CLOAI"] = "803";
                    row190["MA_CLOAI"] = "804";
                    row191["MA_CLOAI"] = "805";
                    row192["MA_CLOAI"] = "D11";
                    row193["MA_CLOAI"] = "D13";
                    row194["MA_CLOAI"] = "DT1";
                    row195["MA_CLOAI"] = "DT3";
                    row196["MA_CLOAI"] = "HC1";
                    row197["MA_CLOAI"] = "HC3";
                    row198["MA_CLOAI"] = "KN1";
                    row199["MA_CLOAI"] = "kn2";
                    row200["MA_CLOAI"] = "VC1";
                    row201["MA_CLOAI"] = "VC3";

                    row1["SO_PHA"] = "3";
                    row2["SO_PHA"] = "1";
                    row3["SO_PHA"] = "1";
                    row4["SO_PHA"] = "1";
                    row5["SO_PHA"] = "1";
                    row6["SO_PHA"] = "1";
                    row7["SO_PHA"] = "1";
                    row8["SO_PHA"] = "1";
                    row9["SO_PHA"] = "1";
                    row10["SO_PHA"] = "1";
                    row11["SO_PHA"] = "1";
                    row12["SO_PHA"] = "1";
                    row13["SO_PHA"] = "1";
                    row14["SO_PHA"] = "1";
                    row15["SO_PHA"] = "1";
                    row16["SO_PHA"] = "1";
                    row17["SO_PHA"] = "1";
                    row18["SO_PHA"] = "1";
                    row19["SO_PHA"] = "1";
                    row20["SO_PHA"] = "1";
                    row21["SO_PHA"] = "1";
                    row22["SO_PHA"] = "1";
                    row23["SO_PHA"] = "1";
                    row24["SO_PHA"] = "3";
                    row25["SO_PHA"] = "3";
                    row26["SO_PHA"] = "3";
                    row27["SO_PHA"] = "3";
                    row28["SO_PHA"] = "3";
                    row29["SO_PHA"] = "3";
                    row30["SO_PHA"] = "3";
                    row31["SO_PHA"] = "3";
                    row32["SO_PHA"] = "3";
                    row33["SO_PHA"] = "3";
                    row34["SO_PHA"] = "1";
                    row35["SO_PHA"] = "3";
                    row36["SO_PHA"] = "3";
                    row37["SO_PHA"] = "3";
                    row38["SO_PHA"] = "3";
                    row39["SO_PHA"] = "1";
                    row40["SO_PHA"] = "1";
                    row41["SO_PHA"] = "3";
                    row42["SO_PHA"] = "3";
                    row43["SO_PHA"] = "1";
                    row44["SO_PHA"] = "3";
                    row45["SO_PHA"] = "3";
                    row46["SO_PHA"] = "3";
                    row47["SO_PHA"] = "3";
                    row48["SO_PHA"] = "1";
                    row49["SO_PHA"] = "3";
                    row50["SO_PHA"] = "3";
                    row51["SO_PHA"] = "3";
                    row52["SO_PHA"] = "3";
                    row53["SO_PHA"] = "3";
                    row54["SO_PHA"] = "3";
                    row55["SO_PHA"] = "3";
                    row56["SO_PHA"] = "3";
                    row57["SO_PHA"] = "3";
                    row58["SO_PHA"] = "3";
                    row59["SO_PHA"] = "3";
                    row60["SO_PHA"] = "3";
                    row61["SO_PHA"] = "1";
                    row62["SO_PHA"] = "3";
                    row63["SO_PHA"] = "3";
                    row64["SO_PHA"] = "1";
                    row65["SO_PHA"] = "1";
                    row66["SO_PHA"] = "1";
                    row67["SO_PHA"] = "3";
                    row68["SO_PHA"] = "3";
                    row69["SO_PHA"] = "3";
                    row70["SO_PHA"] = "3";
                    row71["SO_PHA"] = "3";
                    row72["SO_PHA"] = "1";
                    row73["SO_PHA"] = "1";
                    row74["SO_PHA"] = "1";
                    row75["SO_PHA"] = "3";
                    row76["SO_PHA"] = "3";
                    row77["SO_PHA"] = "3";
                    row78["SO_PHA"] = "3";
                    row79["SO_PHA"] = "3";
                    row80["SO_PHA"] = "3";
                    row81["SO_PHA"] = "3";
                    row82["SO_PHA"] = "3";
                    row83["SO_PHA"] = "3";
                    row84["SO_PHA"] = "3";
                    row85["SO_PHA"] = "3";
                    row86["SO_PHA"] = "3";
                    row87["SO_PHA"] = "3";
                    row88["SO_PHA"] = "3";
                    row89["SO_PHA"] = "3";
                    row90["SO_PHA"] = "3";
                    row91["SO_PHA"] = "3";
                    row92["SO_PHA"] = "3";
                    row93["SO_PHA"] = "3";
                    row94["SO_PHA"] = "3";
                    row95["SO_PHA"] = "3";
                    row96["SO_PHA"] = "3";
                    row97["SO_PHA"] = "1";
                    row98["SO_PHA"] = "3";
                    row99["SO_PHA"] = "3";
                    row100["SO_PHA"] = "3";
                    row101["SO_PHA"] = "3";
                    row102["SO_PHA"] = "3";
                    row103["SO_PHA"] = "3";
                    row104["SO_PHA"] = "3";
                    row105["SO_PHA"] = "3";
                    row106["SO_PHA"] = "3";
                    row107["SO_PHA"] = "3";
                    row108["SO_PHA"] = "3";
                    row109["SO_PHA"] = "3";
                    row110["SO_PHA"] = "3";
                    row111["SO_PHA"] = "3";
                    row112["SO_PHA"] = "3";
                    row113["SO_PHA"] = "3";
                    row114["SO_PHA"] = "3";
                    row115["SO_PHA"] = "3";
                    row116["SO_PHA"] = "3";
                    row117["SO_PHA"] = "3";
                    row118["SO_PHA"] = "3";
                    row119["SO_PHA"] = "3";
                    row120["SO_PHA"] = "3";
                    row121["SO_PHA"] = "3";
                    row122["SO_PHA"] = "3";
                    row123["SO_PHA"] = "3";
                    row124["SO_PHA"] = "3";
                    row125["SO_PHA"] = "3";
                    row126["SO_PHA"] = "3";
                    row127["SO_PHA"] = "3";
                    row128["SO_PHA"] = "3";
                    row129["SO_PHA"] = "3";
                    row130["SO_PHA"] = "3";
                    row131["SO_PHA"] = "3";
                    row132["SO_PHA"] = "3";
                    row133["SO_PHA"] = "3";
                    row134["SO_PHA"] = "3";
                    row135["SO_PHA"] = "3";
                    row136["SO_PHA"] = "3";
                    row137["SO_PHA"] = "3";
                    row138["SO_PHA"] = "3";
                    row139["SO_PHA"] = "3";
                    row140["SO_PHA"] = "3";
                    row141["SO_PHA"] = "3";
                    row142["SO_PHA"] = "3";
                    row143["SO_PHA"] = "3";
                    row144["SO_PHA"] = "1";
                    row145["SO_PHA"] = "3";
                    row146["SO_PHA"] = "3";
                    row147["SO_PHA"] = "3";
                    row148["SO_PHA"] = "3";
                    row149["SO_PHA"] = "3";
                    row150["SO_PHA"] = "3";
                    row151["SO_PHA"] = "3";
                    row152["SO_PHA"] = "3";
                    row153["SO_PHA"] = "1";
                    row154["SO_PHA"] = "3";
                    row155["SO_PHA"] = "3";
                    row156["SO_PHA"] = "3";
                    row157["SO_PHA"] = "3";
                    row158["SO_PHA"] = "3";
                    row159["SO_PHA"] = "3";
                    row160["SO_PHA"] = "3";
                    row161["SO_PHA"] = "3";
                    row162["SO_PHA"] = "3";
                    row163["SO_PHA"] = "3";
                    row164["SO_PHA"] = "3";
                    row165["SO_PHA"] = "1";
                    row166["SO_PHA"] = "1";
                    row167["SO_PHA"] = "3";
                    row168["SO_PHA"] = "1";
                    row169["SO_PHA"] = "1";
                    row170["SO_PHA"] = "3";
                    row171["SO_PHA"] = "1";
                    row172["SO_PHA"] = "1";
                    row173["SO_PHA"] = "1";
                    row174["SO_PHA"] = "3";
                    row175["SO_PHA"] = "3";
                    row176["SO_PHA"] = "3";
                    row177["SO_PHA"] = "3";
                    row178["SO_PHA"] = "1";
                    row179["SO_PHA"] = "3";
                    row180["SO_PHA"] = "1";
                    row181["SO_PHA"] = "3";
                    row182["SO_PHA"] = "3";
                    row183["SO_PHA"] = "3";
                    row184["SO_PHA"] = "3";
                    row185["SO_PHA"] = "3";
                    row186["SO_PHA"] = "3";
                    row187["SO_PHA"] = "3";
                    row188["SO_PHA"] = "3";
                    row189["SO_PHA"] = "1";
                    row190["SO_PHA"] = "1";
                    row191["SO_PHA"] = "3";
                    row192["SO_PHA"] = "1";
                    row193["SO_PHA"] = "3";
                    row194["SO_PHA"] = "1";
                    row195["SO_PHA"] = "3";
                    row196["SO_PHA"] = "1";
                    row197["SO_PHA"] = "3";
                    row198["SO_PHA"] = "3";
                    row199["SO_PHA"] = "3";
                    row200["SO_PHA"] = "1";
                    row201["SO_PHA"] = "3";

                    dtClct.Rows.Add(row1);
                    dtClct.Rows.Add(row2);
                    dtClct.Rows.Add(row3);
                    dtClct.Rows.Add(row4);
                    dtClct.Rows.Add(row5);
                    dtClct.Rows.Add(row6);
                    dtClct.Rows.Add(row7);
                    dtClct.Rows.Add(row8);
                    dtClct.Rows.Add(row9);
                    dtClct.Rows.Add(row10);
                    dtClct.Rows.Add(row11);
                    dtClct.Rows.Add(row12);
                    dtClct.Rows.Add(row13);
                    dtClct.Rows.Add(row14);
                    dtClct.Rows.Add(row15);
                    dtClct.Rows.Add(row16);
                    dtClct.Rows.Add(row17);
                    dtClct.Rows.Add(row18);
                    dtClct.Rows.Add(row19);
                    dtClct.Rows.Add(row20);
                    dtClct.Rows.Add(row21);
                    dtClct.Rows.Add(row22);
                    dtClct.Rows.Add(row23);
                    dtClct.Rows.Add(row24);
                    dtClct.Rows.Add(row25);
                    dtClct.Rows.Add(row26);
                    dtClct.Rows.Add(row27);
                    dtClct.Rows.Add(row28);
                    dtClct.Rows.Add(row29);
                    dtClct.Rows.Add(row30);
                    dtClct.Rows.Add(row31);
                    dtClct.Rows.Add(row32);
                    dtClct.Rows.Add(row33);
                    dtClct.Rows.Add(row34);
                    dtClct.Rows.Add(row35);
                    dtClct.Rows.Add(row36);
                    dtClct.Rows.Add(row37);
                    dtClct.Rows.Add(row38);
                    dtClct.Rows.Add(row39);
                    dtClct.Rows.Add(row40);
                    dtClct.Rows.Add(row41);
                    dtClct.Rows.Add(row42);
                    dtClct.Rows.Add(row43);
                    dtClct.Rows.Add(row44);
                    dtClct.Rows.Add(row45);
                    dtClct.Rows.Add(row46);
                    dtClct.Rows.Add(row47);
                    dtClct.Rows.Add(row48);
                    dtClct.Rows.Add(row49);
                    dtClct.Rows.Add(row50);
                    dtClct.Rows.Add(row51);
                    dtClct.Rows.Add(row52);
                    dtClct.Rows.Add(row53);
                    dtClct.Rows.Add(row54);
                    dtClct.Rows.Add(row55);
                    dtClct.Rows.Add(row56);
                    dtClct.Rows.Add(row57);
                    dtClct.Rows.Add(row58);
                    dtClct.Rows.Add(row59);
                    dtClct.Rows.Add(row60);
                    dtClct.Rows.Add(row61);
                    dtClct.Rows.Add(row62);
                    dtClct.Rows.Add(row63);
                    dtClct.Rows.Add(row64);
                    dtClct.Rows.Add(row65);
                    dtClct.Rows.Add(row66);
                    dtClct.Rows.Add(row67);
                    dtClct.Rows.Add(row68);
                    dtClct.Rows.Add(row69);
                    dtClct.Rows.Add(row70);
                    dtClct.Rows.Add(row71);
                    dtClct.Rows.Add(row72);
                    dtClct.Rows.Add(row73);
                    dtClct.Rows.Add(row74);
                    dtClct.Rows.Add(row75);
                    dtClct.Rows.Add(row76);
                    dtClct.Rows.Add(row77);
                    dtClct.Rows.Add(row78);
                    dtClct.Rows.Add(row79);
                    dtClct.Rows.Add(row80);
                    dtClct.Rows.Add(row81);
                    dtClct.Rows.Add(row82);
                    dtClct.Rows.Add(row83);
                    dtClct.Rows.Add(row84);
                    dtClct.Rows.Add(row85);
                    dtClct.Rows.Add(row86);
                    dtClct.Rows.Add(row87);
                    dtClct.Rows.Add(row88);
                    dtClct.Rows.Add(row89);
                    dtClct.Rows.Add(row90);
                    dtClct.Rows.Add(row91);
                    dtClct.Rows.Add(row92);
                    dtClct.Rows.Add(row93);
                    dtClct.Rows.Add(row94);
                    dtClct.Rows.Add(row95);
                    dtClct.Rows.Add(row96);
                    dtClct.Rows.Add(row97);
                    dtClct.Rows.Add(row98);
                    dtClct.Rows.Add(row99);
                    dtClct.Rows.Add(row100);
                    dtClct.Rows.Add(row101);
                    dtClct.Rows.Add(row102);
                    dtClct.Rows.Add(row103);
                    dtClct.Rows.Add(row104);
                    dtClct.Rows.Add(row105);
                    dtClct.Rows.Add(row106);
                    dtClct.Rows.Add(row107);
                    dtClct.Rows.Add(row108);
                    dtClct.Rows.Add(row109);
                    dtClct.Rows.Add(row110);
                    dtClct.Rows.Add(row111);
                    dtClct.Rows.Add(row112);
                    dtClct.Rows.Add(row113);
                    dtClct.Rows.Add(row114);
                    dtClct.Rows.Add(row115);
                    dtClct.Rows.Add(row116);
                    dtClct.Rows.Add(row117);
                    dtClct.Rows.Add(row118);
                    dtClct.Rows.Add(row119);
                    dtClct.Rows.Add(row120);
                    dtClct.Rows.Add(row121);
                    dtClct.Rows.Add(row122);
                    dtClct.Rows.Add(row123);
                    dtClct.Rows.Add(row124);
                    dtClct.Rows.Add(row125);
                    dtClct.Rows.Add(row126);
                    dtClct.Rows.Add(row127);
                    dtClct.Rows.Add(row128);
                    dtClct.Rows.Add(row129);
                    dtClct.Rows.Add(row130);
                    dtClct.Rows.Add(row131);
                    dtClct.Rows.Add(row132);
                    dtClct.Rows.Add(row133);
                    dtClct.Rows.Add(row134);
                    dtClct.Rows.Add(row135);
                    dtClct.Rows.Add(row136);
                    dtClct.Rows.Add(row137);
                    dtClct.Rows.Add(row138);
                    dtClct.Rows.Add(row139);
                    dtClct.Rows.Add(row140);
                    dtClct.Rows.Add(row141);
                    dtClct.Rows.Add(row142);
                    dtClct.Rows.Add(row143);
                    dtClct.Rows.Add(row144);
                    dtClct.Rows.Add(row145);
                    dtClct.Rows.Add(row146);
                    dtClct.Rows.Add(row147);
                    dtClct.Rows.Add(row148);
                    dtClct.Rows.Add(row149);
                    dtClct.Rows.Add(row150);
                    dtClct.Rows.Add(row151);
                    dtClct.Rows.Add(row152);
                    dtClct.Rows.Add(row153);
                    dtClct.Rows.Add(row154);
                    dtClct.Rows.Add(row155);
                    dtClct.Rows.Add(row156);
                    dtClct.Rows.Add(row157);
                    dtClct.Rows.Add(row158);
                    dtClct.Rows.Add(row159);
                    dtClct.Rows.Add(row160);
                    dtClct.Rows.Add(row161);
                    dtClct.Rows.Add(row162);
                    dtClct.Rows.Add(row163);
                    dtClct.Rows.Add(row164);
                    dtClct.Rows.Add(row165);
                    dtClct.Rows.Add(row166);
                    dtClct.Rows.Add(row167);
                    dtClct.Rows.Add(row168);
                    dtClct.Rows.Add(row169);
                    dtClct.Rows.Add(row170);
                    dtClct.Rows.Add(row171);
                    dtClct.Rows.Add(row172);
                    dtClct.Rows.Add(row173);
                    dtClct.Rows.Add(row174);
                    dtClct.Rows.Add(row175);
                    dtClct.Rows.Add(row176);
                    dtClct.Rows.Add(row177);
                    dtClct.Rows.Add(row178);
                    dtClct.Rows.Add(row179);
                    dtClct.Rows.Add(row180);
                    dtClct.Rows.Add(row181);
                    dtClct.Rows.Add(row182);
                    dtClct.Rows.Add(row183);
                    dtClct.Rows.Add(row184);
                    dtClct.Rows.Add(row185);
                    dtClct.Rows.Add(row186);
                    dtClct.Rows.Add(row187);
                    dtClct.Rows.Add(row188);
                    dtClct.Rows.Add(row189);
                    dtClct.Rows.Add(row190);
                    dtClct.Rows.Add(row191);
                    dtClct.Rows.Add(row192);
                    dtClct.Rows.Add(row193);
                    dtClct.Rows.Add(row194);
                    dtClct.Rows.Add(row195);
                    dtClct.Rows.Add(row196);
                    dtClct.Rows.Add(row197);
                    dtClct.Rows.Add(row198);
                    dtClct.Rows.Add(row199);
                    dtClct.Rows.Add(row200);
                    dtClct.Rows.Add(row201);

                    dsClct.Tables.Add(dtClct);
                    dsClct.WriteXml(dsChungLoaiCongTo);
                    return dsClct;
                }
                else
                {
                    dsClct.ReadXml(dsChungLoaiCongTo);
                    return dsClct;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Lỗi: " + ex.Message);
                return null;
            }
        }


        public static DataSet DsDonViQl()
        {
            var dsDvql = new DataSet();
            try
            {
                var dsDviFilename = Application.StartupPath + @"\DSDonViQL.xml";
                if (!File.Exists(dsDviFilename))
                {
                    var dtDVQL = new DataTable("DSDonViQL");
                    dtDVQL.Columns.Add("MA_DVIQLY", typeof(string));
                    dtDVQL.Columns.Add("TEN_DVIQLY", typeof(string));

                    //DataRow row = dtDVQL.NewRow();
                    //row["MA_DVIQLY"] = "PH";
                    //row["TEN_DVIQLY"] = "CÔNG TY TNHH MTV ĐIỆN LỰC HẢI PHÒNG";
                    //dtDVQL.Rows.Add(row);

                    DataRow row1 = dtDVQL.NewRow();
                    row1["MA_DVIQLY"] = "PH0100";
                    row1["TEN_DVIQLY"] = "HỒNG BÀNG";
                    dtDVQL.Rows.Add(row1);

                    DataRow row2 = dtDVQL.NewRow();
                    row2["MA_DVIQLY"] = "PH0200";
                    row2["TEN_DVIQLY"] = "LÊ CHÂN";
                    dtDVQL.Rows.Add(row2);

                    DataRow row3 = dtDVQL.NewRow();
                    row3["MA_DVIQLY"] = "PH0300";
                    row3["TEN_DVIQLY"] = "NGÔ QUYỀN";
                    dtDVQL.Rows.Add(row3);

                    DataRow row4 = dtDVQL.NewRow();
                    row4["MA_DVIQLY"] = "PH0400";
                    row4["TEN_DVIQLY"] = "THỦY NGUYÊN";
                    dtDVQL.Rows.Add(row4);

                    DataRow row5 = dtDVQL.NewRow();
                    row5["MA_DVIQLY"] = "PH0500";
                    row5["TEN_DVIQLY"] = "AN DƯƠNG";
                    dtDVQL.Rows.Add(row5);

                    DataRow row6 = dtDVQL.NewRow();
                    row6["MA_DVIQLY"] = "PH0600";
                    row6["TEN_DVIQLY"] = "KIẾN AN";
                    dtDVQL.Rows.Add(row6);

                    DataRow row7 = dtDVQL.NewRow();
                    row7["MA_DVIQLY"] = "PD0700";
                    row7["TEN_DVIQLY"] = "ĐÔNG ANH";
                    dtDVQL.Rows.Add(row7);

                    DataRow row8 = dtDVQL.NewRow();
                    row8["MA_DVIQLY"] = "PH0800";
                    row8["TEN_DVIQLY"] = "KIẾN THỤY";
                    dtDVQL.Rows.Add(row8);

                    DataRow row91 = dtDVQL.NewRow();
                    row91["MA_DVIQLY"] = "PH0900";
                    row91["TEN_DVIQLY"] = "TIÊN LÃNG";
                    dtDVQL.Rows.Add(row91);

                    DataRow row9 = dtDVQL.NewRow();
                    row9["MA_DVIQLY"] = "PH1000";
                    row9["TEN_DVIQLY"] = "AN LÃO";
                    dtDVQL.Rows.Add(row9);

                    

                    DataRow row10 = dtDVQL.NewRow();
                    row10["MA_DVIQLY"] = "PH1100";
                    row10["TEN_DVIQLY"] = "VĨNH BẢO";
                    dtDVQL.Rows.Add(row10);

                    DataRow row11 = dtDVQL.NewRow();
                    row11["MA_DVIQLY"] = "PH1200";
                    row11["TEN_DVIQLY"] = "CÁT HẢI";
                    dtDVQL.Rows.Add(row11);

                    DataRow row12 = dtDVQL.NewRow();
                    row12["MA_DVIQLY"] = "PH1300";
                    row12["TEN_DVIQLY"] = "HẢI AN";
                    dtDVQL.Rows.Add(row12);

                    DataRow row13 = dtDVQL.NewRow();
                    row13["MA_DVIQLY"] = "PH1400";
                    row13["TEN_DVIQLY"] = "DƯƠNG KINH";
                    dtDVQL.Rows.Add(row13);

                    //DataRow row = dtDVQL.NewRow();
                    //row["MA_DVIQLY"] = "PD0100";
                    //row["TEN_DVIQLY"] = "HOÀN KIẾM";
                    //dtDVQL.Rows.Add(row);

                    //DataRow row1 = dtDVQL.NewRow();
                    //row1["MA_DVIQLY"] = "PD0200";
                    //row1["TEN_DVIQLY"] = "HAI BÀ TRƯNG";
                    //dtDVQL.Rows.Add(row1);

                    //DataRow row2 = dtDVQL.NewRow();
                    //row2["MA_DVIQLY"] = "PD0300";
                    //row2["TEN_DVIQLY"] = "BA ĐÌNH";
                    //dtDVQL.Rows.Add(row2);

                    //DataRow row3 = dtDVQL.NewRow();
                    //row3["MA_DVIQLY"] = "PD0400";
                    //row3["TEN_DVIQLY"] = "ĐỐNG ĐA";
                    //dtDVQL.Rows.Add(row3);

                    //DataRow row4 = dtDVQL.NewRow();
                    //row4["MA_DVIQLY"] = "PD0500";
                    //row4["TEN_DVIQLY"] = "NAM TỪ LIÊM";
                    //dtDVQL.Rows.Add(row4);

                    //DataRow row5 = dtDVQL.NewRow();
                    //row5["MA_DVIQLY"] = "PD0600";
                    //row5["TEN_DVIQLY"] = "THANH TRÌ";
                    //dtDVQL.Rows.Add(row5);

                    //DataRow row6 = dtDVQL.NewRow();
                    //row6["MA_DVIQLY"] = "PD0700";
                    //row6["TEN_DVIQLY"] = "GIA LÂM";
                    //dtDVQL.Rows.Add(row6);

                    //DataRow row7 = dtDVQL.NewRow();
                    //row7["MA_DVIQLY"] = "PD0800";
                    //row7["TEN_DVIQLY"] = "ĐÔNG ANH";
                    //dtDVQL.Rows.Add(row7);

                    //DataRow row8 = dtDVQL.NewRow();
                    //row8["MA_DVIQLY"] = "PD0900";
                    //row8["TEN_DVIQLY"] = "SÓC SƠN";
                    //dtDVQL.Rows.Add(row8);

                    //DataRow row9 = dtDVQL.NewRow();
                    //row9["MA_DVIQLY"] = "PD1000";
                    //row9["TEN_DVIQLY"] = "TÂY HỒ";
                    //dtDVQL.Rows.Add(row9);

                    //DataRow row10 = dtDVQL.NewRow();
                    //row10["MA_DVIQLY"] = "PD1100";
                    //row10["TEN_DVIQLY"] = "THANH XUÂN";
                    //dtDVQL.Rows.Add(row10);

                    //DataRow row11 = dtDVQL.NewRow();
                    //row11["MA_DVIQLY"] = "PD1200";
                    //row11["TEN_DVIQLY"] = "CẦU GIẤY";
                    //dtDVQL.Rows.Add(row11);

                    //DataRow row12 = dtDVQL.NewRow();
                    //row12["MA_DVIQLY"] = "PD1300";
                    //row12["TEN_DVIQLY"] = "HOÀNG MAI";
                    //dtDVQL.Rows.Add(row12);

                    //DataRow row13 = dtDVQL.NewRow();
                    //row13["MA_DVIQLY"] = "PD1400";
                    //row13["TEN_DVIQLY"] = "LONG BIÊN";
                    //dtDVQL.Rows.Add(row13);

                    //DataRow row14 = dtDVQL.NewRow();
                    //row14["MA_DVIQLY"] = "PD1500";
                    //row14["TEN_DVIQLY"] = "MÊ LINH";
                    //dtDVQL.Rows.Add(row14);

                    //DataRow row15 = dtDVQL.NewRow();
                    //row15["MA_DVIQLY"] = "PD1600";
                    //row15["TEN_DVIQLY"] = "HÀ ĐÔNG";
                    //dtDVQL.Rows.Add(row15);

                    //DataRow row16 = dtDVQL.NewRow();
                    //row16["MA_DVIQLY"] = "PD1700";
                    //row16["TEN_DVIQLY"] = "SƠN TÂY";
                    //dtDVQL.Rows.Add(row16);

                    //DataRow row17 = dtDVQL.NewRow();
                    //row17["MA_DVIQLY"] = "PD1800";
                    //row17["TEN_DVIQLY"] = "CHƯƠNG MỸ";
                    //dtDVQL.Rows.Add(row17);

                    //DataRow row18 = dtDVQL.NewRow();
                    //row18["MA_DVIQLY"] = "PD1900";
                    //row18["TEN_DVIQLY"] = "THẠCH THẤT";
                    //dtDVQL.Rows.Add(row18);

                    //DataRow row19 = dtDVQL.NewRow();
                    //row19["MA_DVIQLY"] = "PD2000";
                    //row19["TEN_DVIQLY"] = "THƯỜNG TÍN";
                    //dtDVQL.Rows.Add(row19);

                    //DataRow row20 = dtDVQL.NewRow();
                    //row20["MA_DVIQLY"] = "PD2100";
                    //row20["TEN_DVIQLY"] = "BA VÌ";
                    //dtDVQL.Rows.Add(row20);

                    //DataRow row21 = dtDVQL.NewRow();
                    //row21["MA_DVIQLY"] = "PD2200";
                    //row21["TEN_DVIQLY"] = "ĐAN PHƯỢNG";
                    //dtDVQL.Rows.Add(row21);

                    //DataRow row22 = dtDVQL.NewRow();
                    //row22["MA_DVIQLY"] = "PD2300";
                    //row22["TEN_DVIQLY"] = "HOÀI ĐỨC";
                    //dtDVQL.Rows.Add(row22);

                    //DataRow row23 = dtDVQL.NewRow();
                    //row23["MA_DVIQLY"] = "PD2400";
                    //row23["TEN_DVIQLY"] = "MỸ ĐỨC";
                    //dtDVQL.Rows.Add(row23);

                    //DataRow row24 = dtDVQL.NewRow();
                    //row24["MA_DVIQLY"] = "PD2500";
                    //row24["TEN_DVIQLY"] = "PHÚ XUYÊN";
                    //dtDVQL.Rows.Add(row24);

                    //DataRow row25 = dtDVQL.NewRow();
                    //row25["MA_DVIQLY"] = "PD2600";
                    //row25["TEN_DVIQLY"] = "PHÚC THỌ";
                    //dtDVQL.Rows.Add(row25);

                    //DataRow row26 = dtDVQL.NewRow();
                    //row26["MA_DVIQLY"] = "PD2700";
                    //row26["TEN_DVIQLY"] = "QUỐC OAI";
                    //dtDVQL.Rows.Add(row26);

                    //DataRow row27 = dtDVQL.NewRow();
                    //row27["MA_DVIQLY"] = "PD2800";
                    //row27["TEN_DVIQLY"] = "THANH OAI";
                    //dtDVQL.Rows.Add(row27);

                    //DataRow row28 = dtDVQL.NewRow();
                    //row28["MA_DVIQLY"] = "PD2900";
                    //row28["TEN_DVIQLY"] = "ỨNG HOÀ";
                    //dtDVQL.Rows.Add(row28);

                    //DataRow row29 = dtDVQL.NewRow();
                    //row29["MA_DVIQLY"] = "PD3000";
                    //row29["TEN_DVIQLY"] = "BẮC TỪ LIÊM";
                    //dtDVQL.Rows.Add(row29);

                    //DataRow row30 = dtDVQL.NewRow();
                    //row30["MA_DVIQLY"] = "PD6800";
                    //row30["TEN_DVIQLY"] = "LƯỚI ĐIỆN CAO THẾ HÀ NỘI";
                    //dtDVQL.Rows.Add(row30);

                    dsDvql.Tables.Add(dtDVQL);
                    dsDvql.WriteXml(dsDviFilename);
                    return dsDvql;
                }
                else
                {
                    dsDvql.ReadXml(dsDviFilename);
                    return dsDvql;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return dsDvql = null;
            }
        }

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Ghi log
        /// </summary>
        /// <param name="ten_ham"></param>
        /// <param name="gia_tri"></param>
        /// <param name="thong_bao"></param>
        /// <returns></returns>
        //public static string GhiLog(string ten_ham, string gia_tri, string thong_bao)
        //{
        //    string thoi_gian = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    try
        //    {
        //        using (SQLiteDB_LOG sqlite_log = new SQLiteDB_LOG())
        //        {
        //            sqlite_log.OpenConn();
        //            string result = sqlite_log.GCS_LOG_Insert(thoi_gian, ten_ham, gia_tri, thong_bao);
        //            sqlite_log.CloseConn();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Ghi log lỗi: " + ex.Message;
        //    }
        //}
       
    }
}
