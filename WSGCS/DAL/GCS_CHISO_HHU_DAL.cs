using ES_WEBKYSO.DataContext;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;
using ES_WEBKYSO.ServiceKetNoiMTB.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WSGCS.DAL
{
    public class GCS_CHISO_HHU_DAL
    {
        private UnitOfWork UnitOfWork;
        private DbContext dbContext;
        public GCS_CHISO_HHU_DAL()
        {
            dbContext = new DataContext();
            UnitOfWork = new UnitOfWork(dbContext);
        }
        public bool CapNhatChiSoHHU(string MA_DVIQLY, string MA_NVGCS, string TMUC_ANH, string TMUC_EXTRACT, DataSet dsData)
        {
            var img_name = "";
            var img_fullname = "";
            var sub_folder = "";
            //khai báo biến sử dụng tiện ích
            CommonExtend commonExtend = new CommonExtend();
            try
            {
                // Tạo folder nếu chưa có
                Directory.CreateDirectory(TMUC_ANH);

                if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                {
                    dsData.Tables[0].Columns.Add("ANH_GCS", typeof(string));
                    
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        // nếu cto chưa ghi chỉ số thì bỏ qua
                        if ((dr["CS_MOI"] == null || dr["CS_MOI"].ToString().Trim().Length == 0 || dr["CS_MOI"].ToString().Trim() == "0")
                            && (dr["TTR_MOI"] == null || dr["TTR_MOI"].ToString().Trim().Length == 0))
                        {
                            continue;
                        }

                        if (TMUC_EXTRACT != null && TMUC_EXTRACT.Trim().Length > 0)
                        {
                            // Lấy ảnh tương ứng với công tơ chèn vào thư mục ảnh trên server
                            int nam = int.Parse(dr["NAM"].ToString().Trim());
                            int thang = int.Parse(dr["THANG"].ToString().Trim());
                            int ky = int.Parse(dr["KY"].ToString().Trim());

                            //folder_image = path_img + "\\" + nam + "_" + thang + "_" + ky;

                            img_name = "";
                            img_fullname = "";
                            img_name = commonExtend.ConvertDrToImageName(dr);
                            img_name = Regex.Replace(img_name, ".jpg", "", RegexOptions.IgnoreCase);
                            
                            string[] files = Directory.GetFiles(TMUC_EXTRACT, img_name + "*.jpg");
                            if (files.Length > 0)
                            {
                                img_fullname = files[0]; // Lấy đường dẫn của file, nếu có nhiều file trùng tên thì lấy file cuối

                                // nếu ko có ảnh thì bỏ qua
                                if (!File.Exists(img_fullname))
                                    continue;

                                //C:\inetpub\wwwroot\API_WebKySo\WSGCS\GCSImages\2018_11_1\PD25918-2018-11-1\PD25918_365200909290546_2018_11_1_PD25007500951005_KT.jpg

                                sub_folder = TMUC_ANH + "\\" + nam + "_" + thang + "_" + ky + "\\" + Regex.Replace(dr["TEN_FILE"].ToString(), ".XML", "", RegexOptions.IgnoreCase);
                                Directory.CreateDirectory(sub_folder);
                                // chuyển ảnh vào thư mục lưu trữ
                                File.Copy(img_fullname, sub_folder + "\\" + img_name + ".jpg", true);
                                dr["ANH_GCS"] = sub_folder + "\\" + img_name + ".jpg";
                            }
                        }
                        else
                        {
                            dr["ANH_GCS"] = null;
                        }
                        var nGayPmax = dr["NGAY_PMAX"].ToString();
                        nGayPmax = string.IsNullOrEmpty(nGayPmax) ? null : Convert.ToDateTime(nGayPmax) >= DateTime.Parse("1753-1-1") && Convert.ToDateTime(nGayPmax) <= DateTime.Parse("9999-12-12") ? nGayPmax : "1753-01-01";

                        string ma_DDO = dr["MA_DDO"].ToString();
                        string ma_COT = dr["MA_CTO"].ToString();
                        string ma_KHANG = dr["MA_KHANG"].ToString();
                        string ma_QUYEN = dr["MA_QUYEN"].ToString();
                        string ten_FILE = dr["TEN_FILE"].ToString();
                        string loai_BCS = dr["LOAI_BCS"].ToString();
                        var ChiSoHHU = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetOne(o => o.MA_DVIQLY == MA_DVIQLY && o.MA_DDO == ma_DDO && o.MA_CTO == ma_COT
                                                && o.MA_KHANG == ma_KHANG & o.MA_QUYEN == ma_QUYEN && o.TEN_FILE == ten_FILE && o.LOAI_BCS == loai_BCS);
                        if (ChiSoHHU == null) continue; //bỏ qua chi tiết Mã điểm đo nếu không tồn tại

                        #region gán dữ liệu table vào model, chuẩn bị insert vào database
                        ChiSoHHU.MA_NVGCS = dr["MA_NVGCS"].ToString();
                        ChiSoHHU.MA_KHANG = dr["MA_KHANG"].ToString();
                        ChiSoHHU.MA_DDO = dr["MA_DDO"].ToString(); 
                        ChiSoHHU.MA_DVIQLY = dr["MA_DVIQLY"].ToString(); ;
                        ChiSoHHU.MA_GC = dr["MA_GC"].ToString();
                        ChiSoHHU.MA_QUYEN = dr["MA_QUYEN"].ToString();
                        ChiSoHHU.MA_TRAM = dr["MA_TRAM"].ToString();
                        ChiSoHHU.BOCSO_ID = Convert.ToInt64(dr["BOCSO_ID"].ToString());
                        ChiSoHHU.LOAI_BCS = dr["LOAI_BCS"].ToString();
                        ChiSoHHU.LOAI_CS = dr["LOAI_CS"].ToString();
                        ChiSoHHU.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        ChiSoHHU.DIA_CHI = dr["DIA_CHI"].ToString();
                        ChiSoHHU.MA_NN = dr["MA_NN"].ToString();
                        ChiSoHHU.SO_HO = Convert.ToDecimal(dr["SO_HO"].ToString());
                        ChiSoHHU.MA_CTO = dr["MA_CTO"].ToString();
                        ChiSoHHU.SERY_CTO = dr["SERY_CTO"].ToString();
                        ChiSoHHU.HSN = Convert.ToDecimal(dr["HSN"].ToString());
                        ChiSoHHU.CS_CU = Convert.ToDecimal(dr["CS_CU"].ToString());
                        ChiSoHHU.TTR_CU = dr["TTR_CU"].ToString();
                        if (dr["SL_CU"].ToString() != "") ChiSoHHU.SL_CU = Convert.ToDecimal(dr["SL_CU"].ToString()); else ChiSoHHU.SL_CU = 1245;
                        ChiSoHHU.SL_TTIEP = Convert.ToInt32(dr["SL_TTIEP"].ToString());
                        ChiSoHHU.NGAY_CU = Convert.ToDateTime(dr["NGAY_CU"].ToString());
                        ChiSoHHU.CS_MOI = Convert.ToDecimal(dr["CS_MOI"].ToString());
                        ChiSoHHU.TTR_MOI = dr["TTR_MOI"].ToString();
                        ChiSoHHU.SL_MOI = Convert.ToDecimal(dr["SL_MOI"].ToString());
                        ChiSoHHU.CHUOI_GIA = dr["CHUOI_GIA"].ToString();
                        ChiSoHHU.KY = Convert.ToInt32(dr["KY"].ToString());
                        ChiSoHHU.THANG = Convert.ToInt32(dr["THANG"].ToString());
                        ChiSoHHU.NAM = Convert.ToInt32(dr["NAM"].ToString());
                        ChiSoHHU.NGAY_MOI = Convert.ToDateTime(dr["NGAY_MOI"].ToString());
                        ChiSoHHU.NGUOI_GCS = dr["NGUOI_GCS"].ToString();
                        ChiSoHHU.SL_THAO = Convert.ToDecimal(dr["SL_THAO"].ToString());
                        ChiSoHHU.KIMUA_CSPK = Convert.ToInt16(dr["KIMUA_CSPK"].ToString());
                        ChiSoHHU.MA_COT = dr["MA_COT"].ToString();
                        ChiSoHHU.CGPVTHD = dr["CGPVTHD"].ToString();
                        ChiSoHHU.HTHUC_TBAO_DK = dr["HTHUC_TBAO_DK"].ToString();
                        ChiSoHHU.DTHOAI_SMS = dr["DTHOAI_SMS"].ToString();
                        ChiSoHHU.EMAIL = dr["EMAIL"].ToString();
                        ChiSoHHU.THOI_GIAN = dr["THOI_GIAN"].ToString();
                        ChiSoHHU.X = Convert.ToDecimal(dr["X"].ToString());
                        ChiSoHHU.Y = Convert.ToDecimal(dr["Y"].ToString());
                        ChiSoHHU.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"].ToString());
                        ChiSoHHU.HTHUC_TBAO_TH = dr["HTHUC_TBAO_TH"].ToString();
                        ChiSoHHU.TENKHANG_RUTGON = dr["TENKHANG_RUTGON"].ToString();
                        ChiSoHHU.TTHAI_DBO = Convert.ToByte(dr["TTHAI_DBO"].ToString());
                        ChiSoHHU.DU_PHONG = dr["DU_PHONG"].ToString();
                        ChiSoHHU.TEN_FILE = dr["TEN_FILE"].ToString();
                        ChiSoHHU.GHICHU = dr["GHICHU"].ToString();
                        ChiSoHHU.SLUONG_1 = Convert.ToDecimal(dr["SLUONG_1"].ToString());
                        ChiSoHHU.SLUONG_2 = Convert.ToDecimal(dr["SLUONG_2"].ToString());
                        ChiSoHHU.SLUONG_3 = Convert.ToDecimal(dr["SLUONG_3"].ToString());
                        if (dr["SO_HOM"].ToString() != "") ChiSoHHU.SO_HOM = dr["SO_HOM"].ToString();
                        ChiSoHHU.TT_KHAC = dr["TT_KHAC"].ToString();
                        ChiSoHHU.ANH_GCS = dr["ANH_GCS"].ToString();
                        ChiSoHHU.PMAX = Convert.ToDecimal(dr["PMAX"].ToString());
                        ChiSoHHU.NGAY_PMAX = Convert.ToDateTime(dr["NGAY_PMAX"].ToString());
                        ChiSoHHU.STR_CHECK_DSOAT = dr["STR_CHECK_DSOAT"].ToString();
                        //ChiSoHHU.ID = Convert.ToInt32(dr["ID"].ToString());
                        #endregion

                        int rs = UnitOfWork.RepoBase<GCS_CHISO_HHU>().Update(ChiSoHHU);
                    }
                    //UnitOfWork.Commit();
                }

                return true;
            }
            catch (Exception e)
            {
                UnitOfWork.RollBack();
                return false;
            }
        }
    }
}