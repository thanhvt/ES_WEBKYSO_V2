using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ionic.Zip;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading;
using System.Globalization;
using WSGCS.Common;

namespace ES_WEBKYSO.ServiceKetNoiMTB.Common
{
    public class CommonExtend
    { // class tien ich them
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
        /// Tạo tên ảnh từ datarow
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string ConvertDrToImageName(DataRow dr)
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
                    for (int col = 0; col < CommonSQLite.gcs_col_all.Length; col++)
                    {
                        if (CommonSQLite.gcs_col_all[col].col_name == "STR_CHECK_DSOAT") // else if
                        {
                            i = 2;

                            #region Gán dữ liệu đối soát

                            try
                            {
                                if (dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "CHECK" ||
                                    dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "UNCHECK" ||
                                    dtData.Rows[row]["STR_CHECK_DSOAT"].ToString().Trim() == "CTO_DTU")
                                {
                                    drNew[CommonSQLite.gcs_col_all[col].col_name] = dtData.Rows[row]["STR_CHECK_DSOAT"].ToString();
                                }
                                else
                                {
                                    drNew[CommonSQLite.gcs_col_all[col].col_name] = "CHUA_DOI_SOAT";
                                }
                            }
                            catch
                            {
                                drNew[CommonSQLite.gcs_col_all[col].col_name] = "CHUA_DOI_SOAT";
                            }

                            #endregion
                        }
                        else if (CommonSQLite.IsNumber(CommonSQLite.gcs_col_all[col].col_type))
                        {
                            i = 3;

                            #region kiểu cột là number

                            // nếu giá trị cột đó != null và kiểu là number
                            if (dtData.Columns.Contains(CommonSQLite.gcs_col_all[col].col_name)
                                && dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name] != null
                                && CommonSQLite.IsNumberValue(dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name].ToString(), out number))
                            {
                                if (CommonSQLite.gcs_col_all[col].col_name == "X" || CommonSQLite.gcs_col_all[col].col_name == "Y")
                                {
                                    drNew[CommonSQLite.gcs_col_all[col].col_name] = number.ToString("0.####################",
                                        CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    drNew[CommonSQLite.gcs_col_all[col].col_name] = number.ToString("0.###",
                                        CultureInfo.CurrentCulture);
                                }
                            }
                            else
                            {
                                drNew[CommonSQLite.gcs_col_all[col].col_name] = 0;
                            }

                            #endregion
                        }
                        else if (CommonSQLite.gcs_col_all[col].col_type == typeof(DateTime))
                        {
                            i = 4;

                            #region kiểu cột là Datetime

                            if (dtData.Columns.Contains(CommonSQLite.gcs_col_all[col].col_name) &&
                                dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name] != null)
                            {
                                //Kiểm tra tháng gcs và tháng trong cột ngay_moi
                                THANG = Convert.ToInt32(dtData.Rows[row]["THANG"].ToString());
                                DateTime? ngay_moi = null;
                                try
                                {
                                    ngay_moi = (DateTime?)Convert.ToDateTime(dtData.Rows[row]["NGAY_MOI"].ToString());
                                    if (dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name] != null &&
                                        dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name].ToString().Trim().Length > 0)
                                    {
                                        drNew[CommonSQLite.gcs_col_all[col].col_name] = dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name];
                                    }
                                    else if (THANG == ngay_moi.Value.Month &&
                                             (CommonSQLite.gcs_col_all[col].col_name == "NGAY_CU" ||
                                              CommonSQLite.gcs_col_all[col].col_name == "NGAY_MOI")) // giá trị ngày đúng định dạng
                                    {
                                        drNew[CommonSQLite.gcs_col_all[col].col_name] =
                                            dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name];
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
                                        if (CommonSQLite.gcs_col_all[col].col_name == "NGAY_CU" ||
                                            CommonSQLite.gcs_col_all[col].col_name == "NGAY_MOI")
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
                                                drNew[CommonSQLite.gcs_col_all[col].col_name] =
                                                    DateTime.ParseExact(
                                                        dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name].ToString(),
                                                        "dd/MM/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                            catch
                                            {
                                                drNew[CommonSQLite.gcs_col_all[col].col_name] =
                                                    DateTime.ParseExact(
                                                        dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name].ToString(),
                                                        "d/M/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        drNew[CommonSQLite.gcs_col_all[col].col_name] = DateTime.Parse("1753-01-01");
                                        // nếu giá trị cột = null thì gán giá trị thời gian nhỏ nhất
                                    }

                                }
                            }
                            else
                                drNew[CommonSQLite.gcs_col_all[col].col_name] = DBNull.Value;

                            #endregion
                        }
                        else if (dtData.Columns.Contains(CommonSQLite.gcs_col_all[col].col_name))
                        {
                            i = 5;

                            #region cột có trong bảng dtData

                            try
                            {
                                drNew[CommonSQLite.gcs_col_all[col].col_name] = dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name];
                            }
                            catch
                            {
                                try
                                {
                                    #region kiểu cột là System.Byte[]

                                    drNew[CommonSQLite.gcs_col_all[col].col_name] =
                                        Convert.FromBase64String(dtData.Rows[row][CommonSQLite.gcs_col_all[col].col_name].ToString());

                                    #endregion
                                }
                                catch
                                {
                                    drNew[CommonSQLite.gcs_col_all[col].col_name] = null;
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            // cột ko có trong bảng dtData
                            drNew[CommonSQLite.gcs_col_all[col].col_name] = null;
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
        /// Tạo bảng rỗng có cấu trúc bảng theo mẫu
        /// </summary>
        /// <returns></returns>
        public DataTable CreateBlankTemplateDataTable()
        {
            DataTable dtTemplate = new DataTable("Table1");

            // create schema for datatable
            for (int i = 0; i < CommonSQLite.gcs_col_all.Length; i++)
            {
                DataColumn dc = new DataColumn(CommonSQLite.gcs_col_all[i].col_name, CommonSQLite.gcs_col_all[i].col_type);
                if (dc.DataType == System.Type.GetType("System.DateTime"))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified; //set msdata:DateTimeMode="Unspecified" trong XML
                }

                dc.AllowDBNull = true; // set minOccurs="0" trong XML
                dc.MaxLength = -1; // bỏ maxLength trong XML
                dtTemplate.Columns.Add(dc);
            }
            for (int i = 0; i < CommonSQLite.ext_col.Length; i++)
            {
                DataColumn dc = new DataColumn(CommonSQLite.ext_col[i].col_name, CommonSQLite.ext_col[i].col_type);
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
    }
}