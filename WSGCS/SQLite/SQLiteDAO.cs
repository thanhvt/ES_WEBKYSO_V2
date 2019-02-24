using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Web;
using WSGCS.Common;

namespace WSGCS.SQLite
{
    public class SQLiteDAO : SQLiteDatabase
    {
        const string sql_create_table_GCS_CHISO_HHU = "CREATE TABLE [GCS_CHISO_HHU] ("
                            + "[ID] INTEGER  NULL,"
                            + "[MA_NVGCS] TEXT  NULL,"
                            + "[MA_KHANG] TEXT  NULL,"
                            + "[MA_DDO] TEXT  NULL,"
                            + "[MA_DVIQLY] TEXT  NULL,"
                            + "[MA_GC] TEXT  NULL,"
                            + "[MA_QUYEN] TEXT  NULL,"
                            + "[MA_TRAM] TEXT  NULL,"
                            + "[BOCSO_ID] TEXT  NULL,"
                            + "[LOAI_BCS] TEXT  NULL,"
                            + "[LOAI_CS] TEXT  NULL,"
                            + "[TEN_KHANG] TEXT  NULL,"
                            + "[DIA_CHI] TEXT  NULL,"
                            + "[MA_NN] TEXT  NULL,"
                            + "[SO_HO] TEXT  NULL,"
                            + "[MA_CTO] TEXT  NULL,"
                            + "[SERY_CTO] TEXT  NULL,"
                            + "[HSN] TEXT  NULL,"
                            + "[CS_CU] TEXT  NULL,"
                            + "[TTR_CU] TEXT  NULL,"
                            + "[SL_CU] TEXT  NULL,"
                            + "[SL_TTIEP] TEXT  NULL,"
                            + "[NGAY_CU] TEXT  NULL,"
                            + "[CS_MOI] TEXT  NULL,"
                            + "[TTR_MOI] TEXT  NULL,"
                            + "[SL_MOI] TEXT  NULL,"
                            + "[CHUOI_GIA] TEXT  NULL,"
                            + "[KY] TEXT  NULL,"
                            + "[THANG] TEXT  NULL,"
                            + "[NAM] TEXT  NULL,"
                            + "[NGAY_MOI] TEXT  NULL,"
                            + "[NGUOI_GCS] TEXT  NULL,"
                            + "[SL_THAO] TEXT  NULL,"
                            + "[KIMUA_CSPK] TEXT  NULL,"
                            + "[MA_COT] TEXT  NULL,"
                            + "[CGPVTHD] TEXT  NULL,"
                            + "[HTHUC_TBAO_DK] TEXT  NULL,"
                            + "[DTHOAI_SMS] TEXT  NULL,"
                            + "[EMAIL] TEXT  NULL,"
                            + "[THOI_GIAN] TEXT  NULL,"
                            + "[X] TEXT  NULL,"
                            + "[Y] TEXT  NULL,"
                            + "[SO_TIEN] TEXT  NULL,"
                            + "[HTHUC_TBAO_TH] TEXT  NULL,"
                            + "[TENKHANG_RUTGON] TEXT  NULL,"
                            + "[TTHAI_DBO] TEXT  NULL,"
                            + "[DU_PHONG] TEXT  NULL,"
                            + "[TEN_FILE] TEXT  NULL,"
                            + "[GHICHU] TEXT  NULL,"
                            + "[TT_KHAC] TEXT  NULL,"
                            + "[ID_SQLITE] INTEGER PRIMARY KEY AUTOINCREMENT,"
                            + "[SLUONG_1] TEXT  NULL,"
                            + "[SLUONG_2] TEXT  NULL,"
                            + "[SLUONG_3] TEXT  NULL,"
                            + "[SO_HOM] TEXT  NULL,"
                            + "[CHU_KY] BLOB  NULL,"
                            + "[HINH_ANH] BLOB  NULL,"
                            + "[PMAX] TEXT  NULL,"
                            + "[NGAY_PMAX] TEXT  NULL, "
                            + "[STR_CHECK_DSOAT] TEXT NULL, "
                            + "UNIQUE (MA_DVIQLY, LOAI_BCS, MA_DDO,MA_CTO, KY,THANG, NAM, TEN_FILE))";

        public string strColumns = "ID,MA_NVGCS,MA_KHANG,MA_DDO,MA_DVIQLY,MA_GC,MA_QUYEN,MA_TRAM,BOCSO_ID,LOAI_BCS,LOAI_CS,TEN_KHANG,DIA_CHI,MA_NN,SO_HO,MA_CTO,SERY_CTO,HSN,CS_CU,TTR_CU,SL_CU,SL_TTIEP,NGAY_CU,CS_MOI,TTR_MOI,SL_MOI,CHUOI_GIA,KY,THANG,NAM,NGAY_MOI,NGUOI_GCS,SL_THAO,KIMUA_CSPK,MA_COT,CGPVTHD,HTHUC_TBAO_DK,DTHOAI_SMS,EMAIL,THOI_GIAN,X,Y,SO_TIEN,HTHUC_TBAO_TH,TENKHANG_RUTGON,TTHAI_DBO,DU_PHONG,TEN_FILE,GHICHU,TT_KHAC,ID_SQLITE,SLUONG_1,SLUONG_2,SLUONG_3,SO_HOM,CHU_KY,HINH_ANH,PMAX,NGAY_PMAX,STR_CHECK_DSOAT";

        public object[,] gcs_cols = new object[,] {
            {"ID","INTEGER","NULL", typeof(int) },  //tên cột, loại cột sql, thuộc tính sql khác của cột, loại biến
            {"MA_NVGCS","TEXT","NULL", typeof(string) },
            {"MA_KHANG","TEXT","NULL", typeof(string) },
            {"MA_DDO","TEXT","NULL", typeof(string) },
            {"MA_DVIQLY","TEXT","NULL", typeof(string) },
            {"MA_GC","TEXT","NULL", typeof(string) },
            {"MA_QUYEN","TEXT","NULL", typeof(string) },
            {"MA_TRAM","TEXT","NULL", typeof(string) },
            {"BOCSO_ID","TEXT","NULL", typeof(string) },
            {"LOAI_BCS","TEXT","NULL", typeof(string) },
            {"LOAI_CS","TEXT","NULL", typeof(string) },
            {"TEN_KHANG","TEXT","NULL", typeof(string) },
            {"DIA_CHI","TEXT","NULL", typeof(string) },
            {"MA_NN","TEXT","NULL", typeof(string) },
            {"SO_HO","TEXT","NULL", typeof(string) },
            {"MA_CTO","TEXT","NULL", typeof(string) },
            {"SERY_CTO","TEXT","NULL", typeof(string) },
            {"HSN","TEXT","NULL", typeof(string) },
            {"CS_CU","TEXT","NULL", typeof(string) },
            {"TTR_CU","TEXT","NULL", typeof(string) },
            {"SL_CU","TEXT","NULL", typeof(string) },
            {"SL_TTIEP","TEXT","NULL", typeof(string) },
            {"NGAY_CU","TEXT","NULL", typeof(string) },
            {"CS_MOI","TEXT","NULL", typeof(string) },
            {"TTR_MOI","TEXT","NULL", typeof(string) },
            {"SL_MOI","TEXT","NULL", typeof(string) },
            {"CHUOI_GIA","TEXT","NULL", typeof(string) },
            {"KY","TEXT","NULL", typeof(string) },
            {"THANG","TEXT","NULL", typeof(string) },
            {"NAM","TEXT","NULL", typeof(string) },
            {"NGAY_MOI","TEXT","NULL", typeof(string) },
            {"NGUOI_GCS","TEXT","NULL", typeof(string) },
            {"SL_THAO","TEXT","NULL", typeof(string) },
            {"KIMUA_CSPK","TEXT","NULL", typeof(string) },
            {"MA_COT","TEXT","NULL", typeof(string) },
            {"CGPVTHD","TEXT","NULL", typeof(string) },
            {"HTHUC_TBAO_DK","TEXT","NULL", typeof(string) },
            {"DTHOAI_SMS","TEXT","NULL", typeof(string) },
            {"EMAIL","TEXT","NULL", typeof(string) },
            {"THOI_GIAN","TEXT","NULL", typeof(string) },
            {"X","TEXT","NULL", typeof(string) },
            {"Y","TEXT","NULL", typeof(string) },
            {"SO_TIEN","TEXT","NULL", typeof(string) },
            {"HTHUC_TBAO_TH","TEXT","NULL", typeof(string) },
            {"TENKHANG_RUTGON","TEXT","NULL", typeof(string) },
            {"TTHAI_DBO","TEXT","NULL", typeof(string) },
            {"DU_PHONG","TEXT","NULL", typeof(string) },
            {"TEN_FILE","TEXT","NULL", typeof(string) },
            {"GHICHU","TEXT","NULL", typeof(string) },
            {"TT_KHAC","TEXT","NULL", typeof(string) },
            {"ID_SQLITE","INTEGER","PRIMARY KEY AUTOINCREMENT", typeof(int) },
            {"SLUONG_1","TEXT","NULL", typeof(string) },
            {"SLUONG_2","TEXT","NULL", typeof(string) },
            {"SLUONG_3","TEXT","NULL", typeof(string) },
            {"SO_HOM","TEXT","NULL", typeof(string) },
            {"CHU_KY","TEXT","NULL", typeof(string) },
            {"HINH_ANH","TEXT","NULL", typeof(string) },
            {"PMAX","TEXT","NULL", typeof(string) },
            {"NGAY_PMAX","TEXT","NULL", typeof(string) },
            {"STR_CHECK_DSOAT","TEXT","NULL", typeof(string) }
        };
        const string sql_create_table_Customer_Parameter = "CREATE TABLE [GCS_CHISO_TH] ("
                           + "[STT] INTEGER  NULL,"
                           + "[MA_KHACH_HANG] TEXT  NULL,"
                           + "[TEN_KHACH_HANG] TEXT  NULL,"
                           + "[DIA_CHI] TEXT  NULL,"
                           + "[MA_SO_THUE] TEXT  NULL,"
                           + "[CAN_BO_THEO_DOI] TEXT  NULL,"
                           + "[MA_CONG_TO] TEXT  NULL,"
                           + "[SO_HO] TEXT  NULL,"
                           + "[HE_SO] TEXT  NULL,"
                           + "[NOI_DUNG] TEXT  NULL,"
                           + "[NGAY_SU_DUNG] TEXT  NULL,"
                           + "[THANG] TEXT  NULL,"
                           + "[NAM] TEXT  NULL,"
                           + "[CHI_SO] TEXT  NULL,"
                           + "[CHI_SO_CU] TEXT  NULL,"
                           + "[SAN_LUONG_CU] TEXT  NULL,"
                           + "[SAN_LUONG] TEXT  NULL,"
                           + "[MDK] TEXT  NULL,"
                           + "[DON_GIA] TEXT  NULL,"
                           + "[TRAM_DIEN] TEXT  NULL,"
                           + "[CHI_SO_1] TEXT  NULL,"
                           + "[CHI_SO_CU_1] TEXT  NULL,"
                           + "[CHI_SO_2] TEXT  NULL,"
                           + "[CHI_SO_CU_2] TEXT  NULL,"
                           + "[CHI_SO_3] TEXT  NULL,"
                           + "[CHI_SO_CU_3] TEXT  NULL, "
                           + "[TEN_FILE] TEXT NULL)";


        public SQLiteDAO(String inputFile)
            : base(inputFile)
        { }


        //------------------------ Create table --------------------------

        /// <summary>
        /// Tạo bảng log FTP lưu lại thông tin những lần chuyển ảnh chụp công tơ lên server
        /// </summary>
        /// <param name="recreate"></param>
        /// <returns></returns>
        public string CreateTableSqlite_FTP_Log(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='FTP_LOG'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }

                string sql_create_table_ftp_Log = "CREATE TABLE [FTP_LOG] ("
                            + "[THUMUC_ID] INTEGER PRIMARY KEY AUTOINCREMENT,"
                            + "[TEN_THUMUC] TEXT  NULL,"
                            + "[SO_LUONG_ANH] TEXT  NULL,"
                            + "[TEN_FILEZIP] TEXT  NULL"
                            + ")";

                result = ExecuteNonQuery(sql_create_table_ftp_Log) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng FTP_LOG trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_CHISO_HHU(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_CHISO_HHU'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }

                result = ExecuteNonQuery(sql_create_table_GCS_CHISO_HHU) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng chi tiết khách hàng trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_TH(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_CHISO_TH'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }

                result = ExecuteNonQuery(sql_create_table_Customer_Parameter) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng chi tiết khách hàng của Thanh Hóa trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }

        }

        public string CreateTableSqlite_GCS_Customer(bool recreate)
        {
            string result = null;
            try
            {
                string table_exist = CheckTableExist("GCS_CHISO_TH");
                if (table_exist == "exist" && !recreate)
                {
                    return "exist";
                }

                result = ExecuteNonQuery(sql_create_table_Customer_Parameter) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng dữ liệu khách hàng mới trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_CHISO_HHUTemp(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_CHISO_HHU_TEMP'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }

                string sql_create = sql_create_table_GCS_CHISO_HHU.Replace("CREATE TABLE [GCS_CHISO_HHU]", "CREATE TABLE [GCS_CHISO_HHU_TEMP]");

                result = ExecuteNonQuery(sql_create) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng chi tiết khách hàng trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_SO_NVGCS(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_SO_NVGCS'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }
                string sql = "CREATE TABLE [GCS_SO_NVGCS] ("
                            + "[_id] integer  PRIMARY KEY AUTOINCREMENT NULL,"
                            + "[MA_DVIQLY] text  NULL,"
                            + "[MA_NVGCS] text  NULL,"
                            + "[TEN_SOGCS] text  NULL,"
                            + "[MA_DQL] TEXT  NULL"
                            + ")";
                result = ExecuteNonQuery(sql) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng danh sách sổ gcs trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_gcsindex(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='gcsindex'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }
                string sql = "CREATE TABLE [gcsindex] ("
                            + "[_id] integer  PRIMARY KEY AUTOINCREMENT NULL,"
                            + "[so] text  NULL,"
                            + "[vitri] text  NULL"
                            + ")";
                result = ExecuteNonQuery(sql) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng danh sách sổ gcs trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_LO_TRINH(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_LO_TRINH'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }
                string sql = "CREATE TABLE [GCS_LO_TRINH] ("
                            + "[ID_ROUTE] INTEGER  PRIMARY KEY AUTOINCREMENT NULL,"
                            + "[SERY_CTO] VARCHAR(50)  NULL,"
                            + "[STT_ORG] INTEGER  NULL,"
                            + "[STT_ROUTE] INTEGER  NULL,"
                            + "[LOAI_BCS] VARCHAR(50)  NULL,"
                            + "[TEN_FILE] VARCHAR(200)  NULL"
                            + ")";
                result = ExecuteNonQuery(sql) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng danh sách sổ gcs trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_CUSTOMER(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_CUSTOMER'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }
                string sql = "CREATE TABLE [GCS_CUSTOMER] ("
                            + "[ID] INTEGER  PRIMARY KEY AUTOINCREMENT,"
                            + "[NOI_DUNG] TEXT  NULL"
                            + ")";
                result = ExecuteNonQuery(sql) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng danh sách khách hàng mới trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }

        public string CreateTableSqlite_GCS_LOG_DELETE(bool recreate)
        {
            string result = null;
            string sql_check = "SELECT name FROM sqlite_master WHERE name='GCS_LOG_DELETE'";
            try
            {
                string table_exist = ExecuteScalar(sql_check);
                if (table_exist != null && !recreate)
                {
                    return "exist";
                }
                string sql = "CREATE TABLE [GCS_LOG_DELETE] ("
                            + "[ID] INTEGER  PRIMARY KEY AUTOINCREMENT,"
                            + "[MA_QUYEN] TEXT,"
                            + "[SERY_CTO] TEXT,"
                            + "[CS_XOA] TEXT,"
                            + "[NGAY_XOA] TEXT,"
                            + "[LY_DO] TEXT"
                            + ")";

                result = ExecuteNonQuery(sql) + "";
                return result;
            }
            catch (Exception ex)
            {
                return "Tạo bảng danh sách khách hàng mới trong cơ sở dữ liệu thất bại\n" + ex.Message;
            }
        }


        //-------------------------  --------------------------------

        public DataTable GetAllSo()
        {
            string query = "SELECT t2.MA_DVIQLY, t2.TEN_FILE, t2.KY, t2.THANG, t2.NAM "
                                + ",IFNULL ( DA_GHI , 0 ) AS DA_GHI,IFNULL ( SO_BANGHI , 0 ) AS SO_BANGHI "
                                + ",(IFNULL ( SO_BANGHI , 0 ) - IFNULL ( DA_GHI , 0 ))AS CHUA_GHI "
                                + ",CASE SO_BANGHI - DA_GHI "
                                    + "WHEN 0 THEN 'true' "
                                    + "ELSE 'false' "
                                    + "END as DA_GHI_XONG "
                            + "FROM "
                                + "(SELECT MA_DVIQLY,TEN_FILE, COUNT(*) AS SO_BANGHI ,KY,THANG,NAM "
                                    + "FROM GCS_CHISO_HHU "
                                    + "GROUP BY MA_DVIQLY, TEN_FILE,KY,THANG,NAM) t2 "
                                + "LEFT JOIN "
                                + "(SELECT MA_DVIQLY,TEN_FILE, COUNT(*) AS DA_GHI ,KY,THANG,NAM "
                                    + "FROM GCS_CHISO_HHU "
                                    + "WHERE CS_MOI > 0 OR (TTR_MOI IS NOT NULL AND length(RTRIM(LTRIM(TTR_MOI))) > 0 ) "
                                    + "GROUP BY MA_DVIQLY, TEN_FILE,KY,THANG,NAM ) t1 "
                                + "ON t1.TEN_FILE=t2.TEN_FILE AND t1.KY=t2.KY AND t1.THANG=t2.THANG AND t1.NAM=t2.NAM "
                            + "ORDER BY t1.TEN_FILE ";
            DataTable dtSqlite = ExecuteQuery(query);
            return dtSqlite;
        }

        /// <summary>
        /// Insert datatable vào db sqlite
        /// </summary>
        /// <param name="dtSqlite"></param>
        /// <param name="db"></param>
        /// <param name="TEN_FILE">Giá trị để insert vào cột TEN_FILE trong bảng GCS_CHISO_HHU</param>
        /// <returns></returns>
        public string InsertDataTableToDbSqlite(DataTable dtSqlite, string TEN_FILE)
        {
            List<KeyValuePair<String, SQLiteParameter>> data = new List<KeyValuePair<String, SQLiteParameter>>();
            decimal number = 0;
            string sql_result = null;

            if (dtSqlite != null && dtSqlite.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSqlite.Rows)
                {
                    data = new List<KeyValuePair<String, SQLiteParameter>>();

                    for (int i = 0; i < CommonSQLite.gcs_col_all.Length; i++)
                    {
                        // bỏ cột khi chèn vào db
                        if (CommonSQLite.gcs_col_all[i].col_name == "CHU_KY_BASE64"
                            || CommonSQLite.gcs_col_all[i].col_name == "HINH_ANH_BASE64"
                            || CommonSQLite.gcs_col_all[i].col_name == "ID_SQLITE")
                        {
                            continue;
                        }
                        SQLiteParameter cmd_param = null;

                        //nếu bảng có chứa cột đang duyệt
                        if (dtSqlite.Columns.Contains(CommonSQLite.gcs_col_all[i].col_name))
                        {
                            #region nếu cell value != null và type = number và value != number
                            if (dr[CommonSQLite.gcs_col_all[i].col_name] != null && CommonSQLite.IsNumber(CommonSQLite.gcs_col_all[i].col_type))
                            {
                                if (CommonSQLite.IsNumberValue(dr[CommonSQLite.gcs_col_all[i].col_name].ToString(), out number))
                                {
                                    cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = number.ToString("0.###", CultureInfo.CurrentCulture) };
                                }
                                else
                                {
                                    cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = "0" };
                                }
                            }
                            # endregion
                            #region nếu cột đó là Datetime
                            //else if (CommonSQLite.gcs_col_all[i].col_type == typeof(DateTime))
                            //{
                            //    if (dr[CommonSQLite.gcs_col_all[i].col_name] != null)
                            //    {
                            //        try
                            //        {
                            //            data.Add(CommonSQLite.gcs_col_all[i].col_name, Convert.ToDateTime(dr[CommonSQLite.gcs_col_all[i].col_name].ToString(),CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture));
                            //        }
                            //        catch (Exception e1)
                            //        {
                            //            try
                            //            {
                            //                data.Add(CommonSQLite.gcs_col_all[i].col_name, DateTime.ParseExact(dr[CommonSQLite.gcs_col_all[i].col_name].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture));
                            //            }
                            //            catch (Exception ex)
                            //            {
                            //                data.Add(CommonSQLite.gcs_col_all[i].col_name, DateTime.ParseExact(dr[CommonSQLite.gcs_col_all[i].col_name].ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture));
                            //            }
                            //        }
                            //    }
                            //    else
                            //        data.Add(CommonSQLite.gcs_col_all[i].col_name, null);
                            //}
                            #endregion
                            #region nếu là cột chữ ký, cột hình ảnh
                            else if (CommonSQLite.gcs_col_all[i].col_name == "CHU_KY" || CommonSQLite.gcs_col_all[i].col_name == "HINH_ANH")
                            {
                                if (dr[CommonSQLite.gcs_col_all[i].col_name] != DBNull.Value && dr[CommonSQLite.gcs_col_all[i].col_name] != null)
                                {
                                    cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = (byte[])dr[CommonSQLite.gcs_col_all[i].col_name] };
                                }
                                else
                                {
                                    cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = null };
                                }
                            }
                            #endregion
                            else
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = SQLiteDAO.Escape(dr[CommonSQLite.gcs_col_all[i].col_name].ToString()) };
                            }
                        }
                        //nếu bảng không chứa cột đang duyệt
                        else
                        {
                            // nếu giá trị cell của cột đó là số
                            if (CommonSQLite.IsNumber(CommonSQLite.gcs_col_all[i].col_type))
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = "0" };
                            }
                            // nếu cột đang duyệt là "TEN_FILE"
                            else if (CommonSQLite.gcs_col_all[i].col_name == "TEN_FILE")
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = SQLiteDAO.Escape(TEN_FILE) };
                            }
                            else
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_col_all[i].db_type) { Value = null };
                            }
                        }

                        data.Add(new KeyValuePair<String, SQLiteParameter>(CommonSQLite.gcs_col_all[i].col_name, cmd_param));
                    }

                    //insert into table GCS_CHISO_HHU 
                    sql_result = Insert("GCS_CHISO_HHU", data);
                    if (sql_result != "1")
                    {
                        if (sql_result.Contains("constraint failed: GCS_CHISO_HHU.MA_DVIQLY, GCS_CHISO_HHU.LOAI_BCS, GCS_CHISO_HHU.MA_DDO, GCS_CHISO_HHU.MA_CTO, GCS_CHISO_HHU.KY, GCS_CHISO_HHU.THANG, GCS_CHISO_HHU.NAM, GCS_CHISO_HHU.TEN_FILE"))
                        {
                            throw new Exception("File bị trùng mã công tơ: " + dr["MA_CTO"].ToString());
                        }
                        else
                        {
                            throw new Exception(sql_result + "Lỗi HHU");
                        }
                    }
                }

                //chèn vào bảng GCS_SO_NVGCS 
                data = new List<KeyValuePair<String, SQLiteParameter>>();
                SQLiteParameter cmd_param1 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(dtSqlite.Rows[0]["MA_DVIQLY"].ToString()) };
                data.Add(new KeyValuePair<String, SQLiteParameter>("MA_DVIQLY", cmd_param1));
                SQLiteParameter cmd_param2 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(dtSqlite.Rows[0]["MA_NVGCS"].ToString()) };
                data.Add(new KeyValuePair<String, SQLiteParameter>("MA_NVGCS", cmd_param2));
                SQLiteParameter cmd_param3 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(dtSqlite.Rows[0]["TEN_FILE"].ToString()) };
                data.Add(new KeyValuePair<String, SQLiteParameter>("TEN_SOGCS", cmd_param3));
                SQLiteParameter cmd_param4 = new SQLiteParameter(DbType.String) { Value = null };
                data.Add(new KeyValuePair<String, SQLiteParameter>("MA_DQL", cmd_param4));

                sql_result = Insert("GCS_SO_NVGCS", data);
                if (sql_result != "1")
                    throw new Exception("Chèn thông tin sổ gcs thất bại");

                //insert into table gcsindex
                data = new List<KeyValuePair<String, SQLiteParameter>>();
                SQLiteParameter cmd_param5 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(TEN_FILE) };
                data.Add(new KeyValuePair<String, SQLiteParameter>("so", cmd_param5));
                SQLiteParameter cmd_param6 = new SQLiteParameter(DbType.String) { Value = "0" };
                data.Add(new KeyValuePair<String, SQLiteParameter>("vitri", cmd_param6));

                sql_result = Insert("gcsindex", data);
                if (sql_result != "1")
                    throw new Exception("Chèn bảng gcsindex thất bại");
            }

            return null;
        }
        /// <summary>
        /// insert vao bang Thanh hoa
        /// </summary>
        /// <param name="dtTest"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string InsertDataTableFromExcelFileToDbSqlite(DataTable dtTest)
        {
            List<KeyValuePair<String, SQLiteParameter>> data = new List<KeyValuePair<String, SQLiteParameter>>();
            decimal number = 0;
            string sql_result = null;

            if (dtTest != null && dtTest.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTest.Rows)
                {
                    if (String.IsNullOrEmpty(dr["STT"].ToString())) break;
                    else
                    {
                        data = new List<KeyValuePair<String, SQLiteParameter>>();
                        for (int i = 0; i < CommonSQLite.gcs_TH_col_all.Length; i++)
                        {
                            SQLiteParameter cmd_param = null;
                            if (dtTest.Columns.Contains(CommonSQLite.gcs_TH_col_all[i].col_name))
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_TH_col_all[i].db_type) { Value = SQLiteDAO.Escape(dr[CommonSQLite.gcs_TH_col_all[i].col_name].ToString()) };
                            }
                            else
                            {
                                cmd_param = new SQLiteParameter(CommonSQLite.gcs_TH_col_all[i].db_type) { Value = null };
                            }

                            data.Add(new KeyValuePair<String, SQLiteParameter>(CommonSQLite.gcs_TH_col_all[i].col_name, cmd_param));
                        }

                        //insert into table GCS_CHISO_HHU 
                        sql_result = Insert("GCS_CHISO_TH", data);
                        if (sql_result != "1")
                        {
                            throw new Exception(sql_result + "Lỗi HHU_TH");
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Chèn thông tin thư mục, số lượng ảnh, tên file zip vào trong file s3db
        /// </summary>
        /// <param name="ten_thumuc"></param>
        /// <param name="so_luong_anh"></param>
        /// <param name="zip_filename"></param>
        /// <returns></returns>
        public string InsertIntoFTPLog(string[] ten_thumuc, string[] so_luong_anh, string zip_filename)
        {
            List<KeyValuePair<String, SQLiteParameter>> data;
            string sql_result = null;
            try
            {
                for (int i = 0; i < ten_thumuc.Length; i++)
                {
                    data = new List<KeyValuePair<String, SQLiteParameter>>();
                    SQLiteParameter cmd_param1 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(ten_thumuc[i].ToString()) };
                    data.Add(new KeyValuePair<String, SQLiteParameter>("TEN_THUMUC", cmd_param1));
                    SQLiteParameter cmd_param2 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(so_luong_anh[i].ToString()) };
                    data.Add(new KeyValuePair<String, SQLiteParameter>("SO_LUONG_ANH", cmd_param2));
                    SQLiteParameter cmd_param3 = new SQLiteParameter(DbType.String) { Value = SQLiteDAO.Escape(zip_filename) };
                    data.Add(new KeyValuePair<String, SQLiteParameter>("TEN_FILEZIP", cmd_param3));

                    sql_result = Insert("FTP_LOG", data);
                    if (sql_result != "1")
                        throw new Exception("Chèn thông tin chuyển ảnh công tơ thất bại! (" + sql_result + ")");
                }
                return sql_result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Thêm cột vào db nếu chưa có
        /// </summary>
        public void AddColumnsIfNotExists_GCS_CHISO_HHU()
        {
            // Lấy danh sách cột của bảng GCS_CHISO_HHU
            var dt = ExecuteQuery("PRAGMA table_info('GCS_CHISO_HHU');");
            foreach (colData col in CommonSQLite.gcs_col_all)
            {
                if (col.col_name == "CHU_KY_BASE64" || col.col_name == "HINH_ANH_BASE64")
                {
                    continue;
                }
                int idx = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Thêm cột vào db nếu chưa có
                    if (dt.Rows[i]["name"] != null && col.col_name == (dt.Rows[i]["name"].ToString()))
                    {
                        idx = i;
                        break;
                    }
                }

                if (idx == -1)
                {
                    ExecuteNonQuery("ALTER TABLE GCS_CHISO_HHU ADD COLUMN " + col.col_name + " " + col.sqlite_type + ";");
                }
                else
                {
                    dt.Rows.RemoveAt(idx);
                }
            }
        }

        /// <summary>
        /// Sắp xếp cột trong bảng GCS_CHISO_HHU theo đúng thứ tự
        /// </summary>
        /// <returns></returns>
        public string ReorderColumns()
        {
            try
            {
                string col_order = "";
                for (int i = 0; i < CommonSQLite.gcs_col_all.Length; i++)
                {
                    if (CommonSQLite.gcs_col_all[i].col_name == "CHU_KY_BASE64" || CommonSQLite.gcs_col_all[i].col_name == "HINH_ANH_BASE64")
                        continue;

                    col_order += "," + CommonSQLite.gcs_col_all[i].col_name;
                }
                col_order = "ID_SQLITE" + col_order;

                string sql_create_table_reorder = sql_create_table_GCS_CHISO_HHU.Replace("GCS_CHISO_HHU", "tbl_reorder");

                string result = ExecuteNonQuery(sql_create_table_reorder) + "";

                string sql_insert_to_new_table = "insert into tbl_reorder "
                                                    + " SELECT  " + col_order
                                                    + " FROM GCS_CHISO_HHU ; ";
                result = ExecuteNonQuery(sql_insert_to_new_table) + "";

                string sql_drop_old_table = "DROP table GCS_CHISO_HHU; "
                                            + "ALTER TABLE tbl_reorder RENAME TO GCS_CHISO_HHU; ";
                result = ExecuteNonQuery(sql_drop_old_table) + "";

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đối soát vào bảng chỉ số tạm
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GCS_CHISO_HHU_UpdateDSoat(DataRow dr)
        {
            string result = null;

            return result;
        }

        /// <summary>
        /// Insert data từ table của db1 sang db2
        /// </summary>
        /// <param name="strDBSrc"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string ImportData(string strDBSrc, string condition)
        {
            condition = condition == null ? "" : condition;

            // Attach db
            string SQL = "ATTACH '" + strDBSrc + "' AS DB_SRC";
            int retval = 0;
            try
            {
                retval = ExecuteNonQuery(SQL);
            }
            catch (Exception ex)
            {
                return "Import table thất bại. \nAttach db thất bại" + ex.Message;
                //return "Import table thất bại. \nAttach db thất bại";
            }

            // Delete duplicate data
            int total_row = 0;
            DataTable dt = ExecuteQuery("SELECT COUNT(*) AS total_row FROM GCS_CHISO_HHU WHERE 1=1 and " + condition);
            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["total_row"] == null || dt.Rows[0]["total_row"] == DBNull.Value)
            {
                return "Import table thất bại. \nSelect dữ liệu trùng thất bại.";
            }
            if (int.TryParse(dt.Rows[0]["total_row"].ToString(), out total_row) && total_row > 0)
            {
                SQL = "DELETE FROM GCS_CHISO_HHU WHERE 1=1 and " + condition;
                try
                {
                    //backup 

                    retval = ExecuteNonQuery(SQL);
                }
                catch
                {
                    return "Import table thất bại. \nXóa dữ liệu trùng thất bại.";
                }
            }

            // insert data
            SQL = "INSERT INTO GCS_CHISO_HHU (" + strColumns.Replace("ID_SQLITE,", "") + ") "
                + "SELECT " + strColumns.Replace("ID_SQLITE,", "") + " FROM DB_SRC.GCS_CHISO_HHU WHERE 1=1 and " + condition;
            try
            {
                retval = ExecuteNonQuery(SQL);
            }
            catch (Exception ex)
            {
                return "Import table thất bại. " + ex.Message;
            }

            return "OK";
        }

        /// <summary>
        /// Cập nhật gộp sổ
        /// </summary>
        /// <param name="strDBSrc"></param>
        /// <param name="list_file"></param>
        /// <returns></returns>
        public string UpdateData(string strDBSrc, string list_file)
        {
            int rs = -1;
            DataTable dtSrc = null;

            try
            {
                // Lấy dt công tơ đã chốt chỉ số để gộp
                using (SQLiteDAO db_Src = new SQLiteDAO(strDBSrc))
                {
                    db_Src.OpenConn();
                    dtSrc = db_Src.ExecuteQuery("SELECT * FROM GCS_CHISO_HHU WHERE TEN_FILE in (" + list_file + ") AND (CS_MOI > 0 || LENGTH(TTR_MOI)  > 0 )");
                    db_Src.CloseConn();
                }

                // backup 

                // cập nhật vào db 
                foreach (DataRow drSrc in dtSrc.Rows)
                {
                    rs = ExecuteNonQuery("UPDATE GCS_CHISO_HHU "
                                    + "SET CS_MOI=" + drSrc["CS_MOI"].ToString()
                                        + ",SL_MOI=" + drSrc["SL_MOI"].ToString()
                                        + ",TTR_MOI='" + drSrc["TTR_MOI"].ToString() + "'"
                                        + ",PMAX=" + (drSrc["PMAX"] == null || drSrc["PMAX"].ToString() == "" ? "0" : drSrc["PMAX"].ToString())
                                        + ",X=" + drSrc["X"].ToString()
                                        + ",Y= " + drSrc["Y"].ToString()
                                    + " WHERE MA_DDO = '" + drSrc["MA_DDO"].ToString() + "'"
                                        + " AND LOAI_BCS='" + drSrc["LOAI_BCS"].ToString() + "'"
                                        + " AND KY=" + drSrc["KY"].ToString()
                                        + " AND THANG=" + drSrc["THANG"].ToString()
                                        + " AND NAM=" + drSrc["NAM"].ToString()
                                        + " AND TEN_FILE = '" + drSrc["TEN_FILE"].ToString() + "' "
                    );
                }
            }
            catch
            {
                return "Update table thất bại. \nInsert table thất bại";
            }

            return "OK";
        }


        public void GetListExistFile(string list_file, out string strNotExist, out string strExist)
        {
            try
            {
                strExist = "";
                strNotExist = list_file;
                string sql = "select TEN_FILE from gcs_chiso_hhu where ten_file in (" + list_file + ") group by ten_file";
                DataTable dtExistFile = ExecuteQuery(sql);

                foreach (DataRow dr in dtExistFile.Rows)
                {
                    if (list_file.Contains(dr["TEN_FILE"].ToString()))
                    {
                        strExist += ",'" + dr["TEN_FILE"].ToString() + "'"; // thêm tên file vào list đã có
                        strNotExist = strNotExist.Replace(dr["TEN_FILE"].ToString(), ""); // bỏ tên file ở list chưa có
                    }
                }
                strNotExist = strNotExist.Replace("'',", "");
                strNotExist = strNotExist.Replace(",''", "");
                strNotExist = strNotExist.Replace("''", "");

                strExist = (strExist.Length > 0 ? strExist.Substring(1) : "");

            }
            catch
            {
                strNotExist = "ERROR";
                strExist = "ERROR";
            }
        }



        public DataTable Select_tenthumuc()
        {
            DataTable dt = new DataTable();
            string query = "SELECT TEN_THUMUC FROM FTP_LOG";

            try
            {
                dt = ExecuteQuery(query);
                return dt;
            }
            catch
            {
                return null;
            }
        }


    }
}