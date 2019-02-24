using Administrator.Library.Models;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.ServiceKetNoiMTB.Model
{
    public class GCS_CHISO_HHU_ett
    {
        public readonly Repository.UnitOfWork _uow;
        GCS_CHISO_HHU_ett()
        {
            DataContext.DataContext context = new DataContext.DataContext();
            _uow = new Repository.UnitOfWork(context);
        }
        private Repository.UnitOfWork UnitOfWork;// = new Repository.UnitOfWork(_uo);
        public void Insert()
        {

        }

        public bool InsertByDataSet(string MA_DVIQLY, string NV_GCS, DataSet ds)
        {
            bool result = false;

            #region định nghĩa datatable là cấu trúc dữ liệu bảng GCS_CHISO_HHU_ett có tên Table
            DataTable Table = new DataTable();
            Table.TableName = "Table";
            Table.Columns.Add("ID");
            Table.Columns.Add("MA_NVGCS");
            Table.Columns.Add("MA_KHANG");
            Table.Columns.Add("MA_DDO");
            Table.Columns.Add("MA_DVIQLY");
            Table.Columns.Add("MA_GC");
            Table.Columns.Add("MA_QUYEN");
            Table.Columns.Add("MA_TRAM");
            Table.Columns.Add("BOCSO_ID");
            Table.Columns.Add("LOAI_BCS");
            Table.Columns.Add("LOAI_CS");
            Table.Columns.Add("TEN_KHANG");
            Table.Columns.Add("DIA_CHI");
            Table.Columns.Add("MA_NN");
            Table.Columns.Add("SO_HO");
            Table.Columns.Add("MA_CTO");
            Table.Columns.Add("SERY_CTO");
            Table.Columns.Add("HSN");
            Table.Columns.Add("CS_CU");
            Table.Columns.Add("TTR_CU");
            Table.Columns.Add("SL_CU");
            Table.Columns.Add("SL_TTIEP");
            Table.Columns.Add("NGAY_CU");
            Table.Columns.Add("CS_MOI");
            Table.Columns.Add("SL_MOI");
            Table.Columns.Add("CHUOI_GIA");
            Table.Columns.Add("KY");
            Table.Columns.Add("THANG");
            Table.Columns.Add("NAM");
            Table.Columns.Add("NGAY_MOI");
            Table.Columns.Add("NGUOI_GCS");
            Table.Columns.Add("SL_THAO");
            Table.Columns.Add("KIMUA_CSPK");
            Table.Columns.Add("MA_COT");
            Table.Columns.Add("CGPVTHD");
            Table.Columns.Add("HTHUC_TBAO_DK");
            Table.Columns.Add("DIENTHOAI_SMS");
            Table.Columns.Add("EMAIL");
            Table.Columns.Add("THOI_GIAN");
            Table.Columns.Add("X");
            Table.Columns.Add("Y");
            Table.Columns.Add("SO_TIEN");
            Table.Columns.Add("HTHUC_TBAO_TH");
            Table.Columns.Add("TEN_KHANG_RUTGON");
            Table.Columns.Add("TTHAI_DBO");
            Table.Columns.Add("DU_PHONG");
            Table.Columns.Add("TEN_FILE");
            Table.Columns.Add("GHICHU");
            Table.Columns.Add("SLUONG_1");
            Table.Columns.Add("SLUONG_2");
            Table.Columns.Add("SLUONG_3");
            Table.Columns.Add("SO_HOM");
            Table.Columns.Add("TT_KHAC");
            Table.Columns.Add("ANH_GCS");
            Table.Columns.Add("PMAX");
            Table.Columns.Add("NGAY_PMAX");
            Table.Columns.Add("STR_CHECK_DSOAT");
            #endregion

            try
            {
                foreach (DataRow row in ds.Tables[0].Rows) //do dataset chỉ có 1 bảng
                {
                    GCS_CHISO_HHU gcs = new GCS_CHISO_HHU();
                    gcs.MA_NVGCS = NV_GCS;
                    gcs.MA_KHANG = row["MA_KHANG"].ToString();
                    gcs.MA_DDO = row["MA_DDO"].ToString();
                    gcs.MA_DVIQLY = MA_DVIQLY;
                    gcs.MA_GC = row["MA_GC"].ToString();
                    gcs.MA_QUYEN = row["MA_QUYEN"].ToString();
                    gcs.MA_TRAM = row["MA_TRAM"].ToString();
                    gcs.BOCSO_ID = Convert.ToInt64(row["BOCSO_ID"].ToString());
                    gcs.LOAI_BCS = row["LOAI_BCS"].ToString();
                    gcs.LOAI_CS = row["LOAI_CS"].ToString();
                    gcs.TEN_KHANG = row["TEN_KHANG"].ToString();
                    gcs.DIA_CHI = row["DIA_CHI"].ToString();
                    gcs.MA_NN = row["MA_NN"].ToString();
                    gcs.SO_HO = Convert.ToDecimal(row["SO_HO"].ToString());
                    gcs.MA_CTO = row["MA_CTO"].ToString();
                    gcs.SERY_CTO = row["SERY_CTO"].ToString();
                    gcs.HSN = Convert.ToDecimal(row["HSN"].ToString());
                    gcs.CS_CU = Convert.ToDecimal(row["CS_CU"].ToString());
                    gcs.TTR_CU = row["TTR_CU"].ToString();
                    gcs.SL_CU = Convert.ToDecimal(row["SL_CU"].ToString());
                    gcs.SL_TTIEP = Convert.ToInt32(row["SL_TTIEP"].ToString());
                    gcs.NGAY_CU = Convert.ToDateTime(row["NGAY_CU"].ToString());
                    gcs.CS_MOI = Convert.ToDecimal(row["CS_MOI"].ToString());
                    gcs.TTR_MOI = row["TTR_MOI"].ToString();
                    gcs.SL_MOI = Convert.ToDecimal(row["SL_MOI"].ToString());
                    gcs.CHUOI_GIA = row["CHUOI_GIA"].ToString();
                    gcs.KY = Convert.ToInt32(row["KY"].ToString());
                    gcs.THANG = Convert.ToInt32(row["THANG"].ToString());
                    gcs.NAM = Convert.ToInt32(row["NAM"].ToString());
                    gcs.NGAY_MOI = Convert.ToDateTime(row["NGAY_MOI"].ToString());
                    gcs.NGUOI_GCS = row["NGUOI_GCS"].ToString();
                    gcs.SL_THAO = Convert.ToInt32(row["SL_THAO"].ToString());
                    gcs.KIMUA_CSPK = Convert.ToInt16(row["KIMUA_CSPK"].ToString());
                    gcs.MA_COT = row["MA_COT"].ToString();
                    gcs.CGPVTHD = row["CGPVTHD"].ToString();
                    gcs.HTHUC_TBAO_DK = row["HTHUC_TBAO_DK"].ToString();
                    gcs.DTHOAI_SMS = row["DTHOAI_SMS"].ToString();
                    gcs.THOI_GIAN = row["THOI_GIAN"].ToString();
                    gcs.X = Convert.ToDecimal(row["X"].ToString());
                    gcs.Y = Convert.ToDecimal(row["Y"].ToString());
                    gcs.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"].ToString());
                    gcs.HTHUC_TBAO_TH = row["HTHUC_TBAO_TH"].ToString();
                    gcs.TENKHANG_RUTGON = row["TENKHANG_RUTGON"].ToString();
                    gcs.TTHAI_DBO = Convert.ToByte(row["TTHAI_DBO"].ToString());
                    gcs.DU_PHONG = row["DU_PHONG"].ToString();
                    gcs.TEN_FILE = row["TEN_FILE"].ToString();
                    gcs.GHICHU = row["GHICHU"].ToString();
                    gcs.SLUONG_1 = Convert.ToDecimal(row["SLUONG_1"].ToString());
                    gcs.SLUONG_2 = Convert.ToDecimal(row["SLUONG_2"].ToString());
                    gcs.SLUONG_3 = Convert.ToDecimal(row["SLUONG_3"].ToString());
                    gcs.SO_HOM = row["SO_HOM"].ToString();
                    gcs.TT_KHAC = row["TT_KHAC"].ToString();
                    gcs.ANH_GCS = row["ANH_GCS"].ToString();
                    gcs.PMAX = Convert.ToDecimal(row["PMAX"].ToString());
                    gcs.NGAY_PMAX = Convert.ToDateTime(row["NGAY_PMAX"].ToString());
                    gcs.STR_CHECK_DSOAT = row["STR_CHECK_DSOAT"].ToString();

                    UnitOfWork.RepoBase<GCS_CHISO_HHU>().Create(gcs);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }
    }
}