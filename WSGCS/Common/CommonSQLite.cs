using System;
using System.Data;
using System.Globalization;
using System.IO;
using WSGCS.SQLite;

namespace WSGCS.Common
{
    public class CommonSQLite
    { // class phuc vu lien quan den SQLite
        public string CheckExistDbSqlite(string dBSqlite, bool recreate)
        {
            var fi = new FileInfo(dBSqlite);
            return CheckExistDbSqlite(fi, recreate);
        }
        public string CheckExistDbSqlite(FileInfo dbSqlite, bool recreate)
        {
            try
            {
                using (var sqliteDao = new SQLiteDAO(dbSqlite.FullName))
                {
                    string result;
                    if (!Directory.Exists(dbSqlite.DirectoryName))
                        Directory.CreateDirectory(dbSqlite.DirectoryName);

                    if (recreate)
                        dbSqlite.Delete();

                    var resultOpen = sqliteDao.OpenConn();
                    if (resultOpen != null && resultOpen.Contains("database disk image is malformed"))
                    {
                        throw new Exception(resultOpen);
                    }

                    if (!dbSqlite.Exists)
                    {
                        // tạo db
                        result = sqliteDao.CreateDBSQLite(dbSqlite.FullName);
                        if (result != null)
                            return result;
                    }

                    // tạo table GCS_CHISO_HHU
                    result = sqliteDao.CreateTableSqlite_GCS_CHISO_HHU(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // tạo table GCS_SO_NVGCS
                    result = sqliteDao.CreateTableSqlite_GCS_SO_NVGCS(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // tạo table gcsindex
                    result = sqliteDao.CreateTableSqlite_gcsindex(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // tạo table GCS_LO_TRINH
                    result = sqliteDao.CreateTableSqlite_GCS_LO_TRINH(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // tạo table GCS_CUSTOMER
                    result = sqliteDao.CreateTableSqlite_GCS_CUSTOMER(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // tạo table GCS_LOG_DELETE
                    result = sqliteDao.CreateTableSqlite_GCS_LOG_DELETE(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;

                    // Tạo bảng tạm nếu chưa có
                    result = sqliteDao.CreateTableSqlite_GCS_CHISO_HHUTemp(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;
                    // Tạo bảng của Thanh Hóa
                    result = sqliteDao.CreateTableSqlite_GCS_TH(recreate);
                    if (result != "0" && result != "exist")//fail
                        return result;
                    return (result == "0" || result == "exist") ? "true" : result;
                }
            }
            catch (Exception ex)
            {
                var message = "Lỗi create DB :" + ex.Message;
                return message;
            }
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