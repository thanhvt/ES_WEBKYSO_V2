using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Data;
using System.Web;
using WSGCS.SQLite;

namespace WSGCS.SQLite
{
    public class SQLite
    {
        SQLiteConnection SQLite_Connect;
        SQLiteCommand SQLite_Command;
        public void ConvertToSQLite(string DataSoure, string TableName, DataSet ds)
        {
            try
            {
                //tạo database và tạo bảng
                SQLite_Connect = new SQLiteConnection("Data Source=" + DataSoure);
                SQLite_Connect.Open();
                SQLite_Command = new SQLiteCommand(SQLite_Connect);
                SQLite_Command.CommandText = "CREATE TABLE " + TableName + " ("
                    + "MA_NVGCS integer, "
                    + "MA_KHANG varchar(25), "
                    + "MA_DDO varchar(25), "
                    + "MA_DVIQLY varchar(10), "
                    + "MA_GC varchar(25), "
                    + "MA_QUYEN varchar(25), "
                    + "MA_TRAM varchar(25), "
                    + "BOCSO_ID varchar(25), "
                    + "LOAI_BCS varchar(25), "
                    + "LOAI_CS varchar(25), "
                    + "TEN_KHANG varchar(50), "
                    + "DIA_CHI varchar(250), "
                    + "MA_NN varchar(25), "
                    + "SO_HO integer, "
                    + "MA_CTO varchar(25), "
                    + "SERY_CTO varchar(30), "
                    + "HSN real, "
                    + "CS_CU integer, "
                    + "TTR_CU varchar(25), "
                    + "SL_CU varchar(25), "
                    + "SL_TTIEP varchar(25), "
                    + "NGAY_CU datetime, "
                    + "CS_MOI integer, "
                    + "TTR_MOI varchar(25), "
                    + "SL_MOI varchar(25), "
                    + "CHUOI_GIA varchar(25), "
                    + "KY integer, "
                    + "THANG integer, "
                    + "NAM integer, "
                    + "NGAY_MOI datetime, "
                    + "NGUOI_GCS varchar(25), "
                    + "SL_THAO varchar(25), "
                    + "KIMUA_CSPK varchar(25), "
                    + "MA_COT varchar(25), "
                    + "SLUONG_1 integer, "
                    + "SLUONG_2 integer, "
                    + "SLUONG_3 integer, "
                    + "SO_HOM varchar(25), "
                    + "PMAX varchar(25), "
                    + "NGAY_PMAX datetime" + ")";
                SQLite_Command.ExecuteNonQuery();

                //insert dữ liệu vào bảng
                foreach (DataTable dt in ds.Tables)
                {
                    //Get field names
                    string sqlString = "INSERT into " + TableName + " (";
                    string valString = "";
                    var sqlParams = new string[dt.Rows[0].ItemArray.Count()];
                    int count = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sqlString += dc.ColumnName + ", ";
                        valString += "@" + dc.ColumnName + ", ";
                        sqlParams[count] = "@" + dc.ColumnName;
                        count++;
                    }
                    valString = valString.Substring(0, valString.Length - 2);
                    sqlString = sqlString.Substring(0, sqlString.Length - 2) + ") VALUES (" + valString + ")";

                    SQLite_Command.CommandText = sqlString;
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dr.ItemArray.Count(); i++)
                        {
                            SQLite_Command.Parameters.AddWithValue(sqlParams[i], dr.ItemArray[i] ?? DBNull.Value);
                        }

                        SQLite_Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                SQLite_Command.Dispose();
                SQLite_Connect.Close();
                SQLite_Connect.Dispose();
            }
        }
    }
}