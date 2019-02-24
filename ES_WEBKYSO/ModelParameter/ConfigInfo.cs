using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace ES_WEBKYSO.ModelParameter
{
    public class ConfigInfo
    {
       // private string configFileName = Application.StartupPath + "\\ESMR.cfg";
        public string MA_DVIQLY { get; set; }
        public string ADDRESS_FTP { get; set; }
        public string USER_FTP { get; set; }
        public string PASS_FTP { get; set; }
        public string TRINHKY_TRUONG_PHONG_KD { get; set; }
        public string TRINHKY_DOI_TRUONG { get; set; }
        public string TRINHKY_NGUOI_GCS { get; set; }
        public string TRINHKY_BO_PHAN_DIEU_HANH { get; set; }
        public bool ENABLE_SLBT_PERCENT { get; set; }
        public bool ENABLE_SLBT_KWH { get; set; }
        public int SLBT_VUOT_MUC { get; set; }
        public int SLBT_DUOI_MUC { get; set; }
        public int SLBT_VUOT_MUC_KWH { get; set; }
        public int SLBT_DUOI_MUC_KWH { get; set; }
        public string CMIS_EXPORTED_FOLDER { get; set; }
        public string GSHT_MOBILE_FOLDER { get; set; }
        public string GSHT_IMPORT_FOLDER { get; set; }
        public string GSHT_EXPORT_FOLDER { get; set; }
        public string HHU_EXPORTED_FOLDER { get; set; }
        public string MOBILE_UPLOAD_FOLDER { get; set; }
        public string MOBILE_DOWNLOAD_FOLDER { get; set; }
        public string DOI_SOAT_FOLDER { get; set; }
        public int DINH_MUC_U { get; set; }
        public string BACKUP_FOLDER { get; set; }
        public string NEW_CUSTOMER_FOLDER { get; set; }
        public bool SHOW_LOGIN { get; set; }
        public bool SHOW_HHU_FUNC { get; set; }
        public bool SHOW_LOC_SL { get; set; }
        public int PHIEN_BAN_DVI { get; set; }
        public string TRINHKY_NGUOIPHUCTRA { get; set; }
        public string TRINKY_PTRACH_BPHAN_PTRA { get; set; }

        public ConfigInfo()
        {
            init();
        }
        public ConfigInfo(DataTable dtConfig)
        {
            init();
            SetConfig(dtConfig);
        }

        // -------------- Method ----------------

        /// <summary>
        /// Khởi tạo biến
        /// </summary>
        private void init()
        {
            TRINHKY_TRUONG_PHONG_KD = "";
            TRINHKY_DOI_TRUONG = "";
            TRINHKY_NGUOI_GCS = "";
            TRINHKY_BO_PHAN_DIEU_HANH = "";
            TRINHKY_NGUOIPHUCTRA = "";
            TRINKY_PTRACH_BPHAN_PTRA = "";
            ENABLE_SLBT_PERCENT = true;
            ENABLE_SLBT_KWH = false;
            SLBT_VUOT_MUC = 30;
            SLBT_DUOI_MUC = 30;
            SLBT_VUOT_MUC_KWH = 100;
            SLBT_DUOI_MUC_KWH = 100;
            //CMIS_EXPORTED_FOLDER = @"C:\CMIS\";
            //HHU_EXPORTED_FOLDER = @"C:\ESMR\";
            //DOI_SOAT_FOLDER = @"C:\ES\DOI_SOAT\";
            //GSHT_IMPORT_FOLDER = @"C:\GSHT\CHUYEN\";
            //GSHT_EXPORT_FOLDER = @"C:\GSHT\NHAN\";
            //MOBILE_UPLOAD_FOLDER = @"H:\";
            //MOBILE_DOWNLOAD_FOLDER = @"H:\";
            //BACKUP_FOLDER = @"C:\ES\Backup";
            //NEW_CUSTOMER_FOLDER = @"C:\kh-moi\";
            DINH_MUC_U = 0;
            SHOW_LOGIN = true;
            SHOW_HHU_FUNC = false;
            SHOW_LOC_SL = false;
            PHIEN_BAN_DVI = 0; // mặc định là phiên bản Hà Nội
            MA_DVIQLY = "";
            ADDRESS_FTP = "";
            USER_FTP = "";
            PASS_FTP = "";
        }

        /// <summary>
        /// Thêm cột thông tin cấu hình còn thiếu
        /// </summary>
        /// <param name="dtConfig"></param>
        public void AddMissingConfigColumn(ref DataTable dtConfig)
        {
            if (!dtConfig.Columns.Contains("TRINHKY_TRUONG_PHONG_KD"))
                dtConfig.Columns.Add("TRINHKY_TRUONG_PHONG_KD", typeof(string));

            if (!dtConfig.Columns.Contains("TRINHKY_DOI_TRUONG"))
                dtConfig.Columns.Add("TRINHKY_DOI_TRUONG", typeof(string));

            if (!dtConfig.Columns.Contains("TRINHKY_NGUOI_GCS"))
                dtConfig.Columns.Add("TRINHKY_NGUOI_GCS", typeof(string));

            if (!dtConfig.Columns.Contains("TRINHKY_BO_PHAN_DIEU_HANH"))
                dtConfig.Columns.Add("TRINHKY_BO_PHAN_DIEU_HANH", typeof(string));

            if (!dtConfig.Columns.Contains("TRINHKY_NGUOIPHUCTRA"))
                dtConfig.Columns.Add("TRINHKY_NGUOIPHUCTRA", typeof(string));

            if (!dtConfig.Columns.Contains("TRINKY_PTRACH_BPHAN_PTRA"))
                dtConfig.Columns.Add("TRINKY_PTRACH_BPHAN_PTRA", typeof(string));

            if (!dtConfig.Columns.Contains("ENABLE_SLBT_PERCENT"))
                dtConfig.Columns.Add("ENABLE_SLBT_PERCENT", typeof(bool));

            if (!dtConfig.Columns.Contains("ENABLE_SLBT_KWH"))
                dtConfig.Columns.Add("ENABLE_SLBT_KWH", typeof(bool));

            if (!dtConfig.Columns.Contains("SLBT_VUOT_MUC"))
                dtConfig.Columns.Add("SLBT_VUOT_MUC", typeof(int));

            if (!dtConfig.Columns.Contains("SLBT_DUOI_MUC"))
                dtConfig.Columns.Add("SLBT_DUOI_MUC", typeof(int));

            if (!dtConfig.Columns.Contains("SLBT_VUOT_MUC_KWH"))
                dtConfig.Columns.Add("SLBT_VUOT_MUC_KWH", typeof(int));

            if (!dtConfig.Columns.Contains("SLBT_DUOI_MUC_KWH"))
                dtConfig.Columns.Add("SLBT_DUOI_MUC_KWH", typeof(int));

            if (!dtConfig.Columns.Contains("CMIS_EXPORTED_FOLDER"))
                dtConfig.Columns.Add("CMIS_EXPORTED_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("HHU_EXPORTED_FOLDER"))
                dtConfig.Columns.Add("HHU_EXPORTED_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("DOI_SOAT_FOLDER"))
                dtConfig.Columns.Add("DOI_SOAT_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("MOBILE_UPLOAD_FOLDER"))
                dtConfig.Columns.Add("MOBILE_UPLOAD_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("MOBILE_DOWNLOAD_FOLDER"))
                dtConfig.Columns.Add("MOBILE_DOWNLOAD_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("BACKUP_FOLDER"))
                dtConfig.Columns.Add("BACKUP_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("NEW_CUSTOMER_FOLDER"))
                dtConfig.Columns.Add("NEW_CUSTOMER_FOLDER", typeof(string));

            if (!dtConfig.Columns.Contains("DINH_MUC_U"))
                dtConfig.Columns.Add("DINH_MUC_U", typeof(int));

            if (!dtConfig.Columns.Contains("SHOW_LOGIN"))
                dtConfig.Columns.Add("SHOW_LOGIN", typeof(int));

            if (!dtConfig.Columns.Contains("SHOW_HHU_FUNC"))
                dtConfig.Columns.Add("SHOW_HHU_FUNC", typeof(int));

            if (!dtConfig.Columns.Contains("SHOW_LOC_SL"))
                dtConfig.Columns.Add("SHOW_LOC_SL", typeof(int));

            if (!dtConfig.Columns.Contains("PHIEN_BAN_DVI"))
                dtConfig.Columns.Add("PHIEN_BAN_DVI", typeof(int));

            if (!dtConfig.Columns.Contains("MA_DVIQLY"))
                dtConfig.Columns.Add("MA_DVIQLY", typeof(string));

            if (!dtConfig.Columns.Contains("ADDRESS_FTP"))
                dtConfig.Columns.Add("ADDRESS_FTP", typeof(string));

            if (!dtConfig.Columns.Contains("USER_FTP"))
                dtConfig.Columns.Add("USER_FTP", typeof(string));

            if (!dtConfig.Columns.Contains("PASS_FTP"))
                dtConfig.Columns.Add("PASS_FTP", typeof(string));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConfigInfo Copy()
        {
            var objCopy = new ConfigInfo();

            objCopy.TRINHKY_TRUONG_PHONG_KD = this.TRINHKY_TRUONG_PHONG_KD;
            objCopy.TRINHKY_DOI_TRUONG = this.TRINHKY_DOI_TRUONG;
            objCopy.TRINHKY_NGUOI_GCS = this.TRINHKY_NGUOI_GCS;
            objCopy.TRINHKY_BO_PHAN_DIEU_HANH = this.TRINHKY_BO_PHAN_DIEU_HANH;
            objCopy.ENABLE_SLBT_PERCENT = this.ENABLE_SLBT_PERCENT;
            objCopy.ENABLE_SLBT_KWH = this.ENABLE_SLBT_KWH;
            objCopy.SLBT_VUOT_MUC = this.SLBT_VUOT_MUC;
            objCopy.SLBT_DUOI_MUC = this.SLBT_DUOI_MUC;
            objCopy.SLBT_VUOT_MUC_KWH = this.SLBT_VUOT_MUC_KWH;
            objCopy.SLBT_DUOI_MUC_KWH = this.SLBT_DUOI_MUC_KWH;
            objCopy.CMIS_EXPORTED_FOLDER = this.CMIS_EXPORTED_FOLDER;
            objCopy.HHU_EXPORTED_FOLDER = this.HHU_EXPORTED_FOLDER;
            objCopy.DOI_SOAT_FOLDER = this.DOI_SOAT_FOLDER;
            objCopy.MOBILE_UPLOAD_FOLDER = this.MOBILE_UPLOAD_FOLDER;
            objCopy.MOBILE_DOWNLOAD_FOLDER = this.MOBILE_DOWNLOAD_FOLDER;
            objCopy.BACKUP_FOLDER = this.BACKUP_FOLDER;
            objCopy.NEW_CUSTOMER_FOLDER = this.NEW_CUSTOMER_FOLDER;
            objCopy.DINH_MUC_U = this.DINH_MUC_U;
            objCopy.SHOW_LOGIN = this.SHOW_LOGIN;
            objCopy.SHOW_HHU_FUNC = this.SHOW_HHU_FUNC;
            objCopy.SHOW_LOC_SL = this.SHOW_LOC_SL;
            objCopy.PHIEN_BAN_DVI = this.PHIEN_BAN_DVI;
            objCopy.MA_DVIQLY = this.MA_DVIQLY;
            objCopy.ADDRESS_FTP = this.ADDRESS_FTP;
            objCopy.USER_FTP = this.USER_FTP;
            objCopy.PASS_FTP = this.PASS_FTP;
            objCopy.TRINHKY_NGUOIPHUCTRA = TRINHKY_NGUOIPHUCTRA;
            objCopy.TRINKY_PTRACH_BPHAN_PTRA = TRINKY_PTRACH_BPHAN_PTRA;

            return objCopy;
        }

        /// <summary>
        /// Tạo datatable config rỗng
        /// </summary>
        /// <returns></returns>
        public DataTable CreateBlankTableConfig()
        {
            DataTable dtConfig = new DataTable();
            dtConfig.Columns.Add("TRINHKY_TRUONG_PHONG_KD", typeof(string));
            dtConfig.Columns.Add("TRINHKY_DOI_TRUONG", typeof(string));
            dtConfig.Columns.Add("TRINHKY_NGUOI_GCS", typeof(string));
            dtConfig.Columns.Add("TRINHKY_BO_PHAN_DIEU_HANH", typeof(string));
            dtConfig.Columns.Add("TRINHKY_NGUOIPHUCTRA", typeof(string));
            dtConfig.Columns.Add("TRINKY_PTRACH_BPHAN_PTRA", typeof(string));
            dtConfig.Columns.Add("ENABLE_SLBT_PERCENT", typeof(bool));
            dtConfig.Columns.Add("ENABLE_SLBT_KWH", typeof(bool));
            dtConfig.Columns.Add("SLBT_VUOT_MUC", typeof(int));
            dtConfig.Columns.Add("SLBT_DUOI_MUC", typeof(int));
            dtConfig.Columns.Add("SLBT_VUOT_MUC_KWH", typeof(int));
            dtConfig.Columns.Add("SLBT_DUOI_MUC_KWH", typeof(int));
            dtConfig.Columns.Add("CMIS_EXPORTED_FOLDER", typeof(string));
            dtConfig.Columns.Add("HHU_EXPORTED_FOLDER", typeof(string));
            dtConfig.Columns.Add("DOI_SOAT_FOLDER", typeof(string));
            dtConfig.Columns.Add("MOBILE_UPLOAD_FOLDER", typeof(string));
            dtConfig.Columns.Add("MOBILE_DOWNLOAD_FOLDER", typeof(string));
            dtConfig.Columns.Add("BACKUP_FOLDER", typeof(string));
            dtConfig.Columns.Add("NEW_CUSTOMER_FOLDER", typeof(string));
            dtConfig.Columns.Add("DINH_MUC_U", typeof(int));
            dtConfig.Columns.Add("SHOW_LOGIN", typeof(bool));
            dtConfig.Columns.Add("SHOW_HHU_FUNC", typeof(bool));
            dtConfig.Columns.Add("SHOW_LOC_SL", typeof(bool));
            dtConfig.Columns.Add("PHIEN_BAN_DVI", typeof(string));
            // Thông tin FTP
            dtConfig.Columns.Add("ADDRESS_FTP", typeof(string));
            dtConfig.Columns.Add("USER_FTP", typeof(string));
            dtConfig.Columns.Add("PASS_FTP", typeof(string));
            dtConfig.Columns.Add("MA_DVIQLY", typeof(string));
            return dtConfig;
        }

        /// <summary>
        /// Convert thông tin cấu hình -> DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable ConvertToDataTable()
        {
            DataTable dtConfig = CreateBlankTableConfig();
            DataRow dr = dtConfig.NewRow();
            dr["TRINHKY_TRUONG_PHONG_KD"] = TRINHKY_TRUONG_PHONG_KD;
            dr["TRINHKY_DOI_TRUONG"] = TRINHKY_DOI_TRUONG;
            dr["TRINHKY_NGUOI_GCS"] = TRINHKY_NGUOI_GCS;
            dr["TRINHKY_BO_PHAN_DIEU_HANH"] = TRINHKY_BO_PHAN_DIEU_HANH;
            dr["TRINHKY_NGUOIPHUCTRA"] = TRINHKY_NGUOIPHUCTRA;
            dr["TRINKY_PTRACH_BPHAN_PTRA"] = TRINKY_PTRACH_BPHAN_PTRA;
            dr["ENABLE_SLBT_PERCENT"] = ENABLE_SLBT_PERCENT;
            dr["ENABLE_SLBT_KWH"] = ENABLE_SLBT_KWH;
            dr["SLBT_VUOT_MUC"] = SLBT_VUOT_MUC;
            dr["SLBT_DUOI_MUC"] = SLBT_DUOI_MUC;
            dr["SLBT_VUOT_MUC_KWH"] = SLBT_VUOT_MUC_KWH;
            dr["SLBT_DUOI_MUC_KWH"] = SLBT_DUOI_MUC_KWH;
            dr["CMIS_EXPORTED_FOLDER"] = CMIS_EXPORTED_FOLDER;
            dr["HHU_EXPORTED_FOLDER"] = HHU_EXPORTED_FOLDER;
            dr["MOBILE_UPLOAD_FOLDER"] = MOBILE_UPLOAD_FOLDER;
            dr["MOBILE_DOWNLOAD_FOLDER"] = MOBILE_DOWNLOAD_FOLDER;
            dr["DOI_SOAT_FOLDER"] = DOI_SOAT_FOLDER;
            dr["BACKUP_FOLDER"] = BACKUP_FOLDER;
            dr["NEW_CUSTOMER_FOLDER"] = NEW_CUSTOMER_FOLDER;
            dr["DINH_MUC_U"] = DINH_MUC_U;
            dr["SHOW_LOGIN"] = SHOW_LOGIN;
            dr["SHOW_HHU_FUNC"] = SHOW_HHU_FUNC;
            dr["SHOW_LOC_SL"] = SHOW_LOC_SL;
            dr["PHIEN_BAN_DVI"] = PHIEN_BAN_DVI;
            //Thông tin FTP
            dr["ADDRESS_FTP"] = ADDRESS_FTP;
            dr["USER_FTP"] = USER_FTP;
            dr["PASS_FTP"] = PASS_FTP;
            dr["MA_DVIQLY"] = MA_DVIQLY;

            dtConfig.Rows.Add(dr);
            return dtConfig;
        }

        /// <summary>
        /// Lấy thông tin online từ datatalbe
        /// </summary>
        /// <param name="dtConfigOnline"></param>
        public string SetConfig(DataTable dtConfig)
        {
            try
            {
                if (dtConfig == null || dtConfig.Rows.Count == 0)
                    return "NO_DATA";

                AddMissingConfigColumn(ref dtConfig);
                DataRow drConfig = dtConfig.Rows[0];

                TRINHKY_TRUONG_PHONG_KD = drConfig["TRINHKY_TRUONG_PHONG_KD"].ToString();
                TRINHKY_DOI_TRUONG = drConfig["TRINHKY_DOI_TRUONG"].ToString();
                TRINHKY_NGUOI_GCS = drConfig["TRINHKY_NGUOI_GCS"].ToString();
                TRINHKY_BO_PHAN_DIEU_HANH = drConfig["TRINHKY_BO_PHAN_DIEU_HANH"].ToString();
                TRINHKY_NGUOIPHUCTRA = drConfig["TRINHKY_NGUOIPHUCTRA"].ToString();
                TRINKY_PTRACH_BPHAN_PTRA = drConfig["TRINKY_PTRACH_BPHAN_PTRA"].ToString(); ;
                ENABLE_SLBT_PERCENT = bool.Parse(drConfig["ENABLE_SLBT_PERCENT"].ToString());
                ENABLE_SLBT_KWH = bool.Parse(drConfig["ENABLE_SLBT_KWH"].ToString());
                SLBT_VUOT_MUC = int.Parse(drConfig["SLBT_VUOT_MUC"].ToString());
                SLBT_DUOI_MUC = int.Parse(drConfig["SLBT_DUOI_MUC"].ToString());
                SLBT_VUOT_MUC_KWH = int.Parse(drConfig["SLBT_VUOT_MUC_KWH"].ToString());
                SLBT_DUOI_MUC_KWH = int.Parse(drConfig["SLBT_DUOI_MUC_KWH"].ToString());
                MOBILE_DOWNLOAD_FOLDER = drConfig["MOBILE_DOWNLOAD_FOLDER"].ToString();
                MOBILE_UPLOAD_FOLDER = drConfig["MOBILE_UPLOAD_FOLDER"].ToString();
                CMIS_EXPORTED_FOLDER = drConfig["CMIS_EXPORTED_FOLDER"].ToString();
                HHU_EXPORTED_FOLDER = drConfig["HHU_EXPORTED_FOLDER"].ToString();
                DOI_SOAT_FOLDER = drConfig["DOI_SOAT_FOLDER"].ToString();
                BACKUP_FOLDER = drConfig["BACKUP_FOLDER"].ToString();
                NEW_CUSTOMER_FOLDER = drConfig["NEW_CUSTOMER_FOLDER"].ToString();
                DINH_MUC_U = int.Parse(drConfig["DINH_MUC_U"].ToString());
                SHOW_LOGIN = bool.Parse(drConfig["SHOW_LOGIN"].ToString());
                SHOW_HHU_FUNC = bool.Parse(drConfig["SHOW_HHU_FUNC"].ToString());
                SHOW_LOC_SL = bool.Parse(drConfig["SHOW_LOC_SL"].ToString());
                PHIEN_BAN_DVI = int.Parse(drConfig["PHIEN_BAN_DVI"].ToString());
                // FTP
                ADDRESS_FTP = drConfig["ADDRESS_FTP"].ToString();
                USER_FTP = drConfig["USER_FTP"].ToString();
                PASS_FTP = drConfig["PASS_FTP"].ToString();
                MA_DVIQLY = drConfig["MA_DVIQLY"].ToString();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}