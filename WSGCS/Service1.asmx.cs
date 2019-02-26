using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;
using ES_WEBKYSO.Controllers;
using WebMatrix.WebData;
using System.Web.Security;
using ES_WEBKYSO.ServiceKetNoiMTB.Common;
using WSGCS.SQLite;
using System.Text.RegularExpressions;
using WSGCS.Common;
using FastMember;
using System.Reflection;
using ES_WEBKYSO.Common;
using WSGCS.DAL;

namespace ES_WEBKYSO.ServiceKetNoiMTB
{
    /// <summary>
    /// Summary description for ServiceKetNoiMTB
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceKetNoiMTB : WebService
    {
        CommonSQLite commonSQLite;
        //private string path_img; //vị trí thư mục lưu ảnh khi MTB đẩy ảnh lên Server
        private string GcsImages; //vị trí thư mục lưu ảnh GCS khi MTB đẩy lên SERVER
        private string GcsUpload; //vị trí file upload từ HHU

        public ServiceKetNoiMTB()
        {
            commonSQLite = new CommonSQLite();
            GcsImages = Server.MapPath("GCSImages"); //lấy thư mục lưu ảnh
            CheckPath(GcsImages); //kiểm tra thư mục, tạo mới nếu chưa có
            GcsUpload = Server.MapPath("GCSUploads"); //lấy thư mục lưu file đẩy lên
            CheckPath(GcsUpload); //kiểm tra thư mục, tạo mới nếu chưa có
        }

        [WebMethod]
        public DataSet ReadXMLToHHC(string FileName, int KY, int THANG, int NAM, string MA_DVIQLY, string MA_DOIGCS, int USERID)
        {
            DataSet ds = new DataSet();
            #region định nghĩa bảng ERROR
            DataTable dtERROR = new DataTable();
            dtERROR.Columns.Add("ERROR");
            #endregion
            try
            {
                //lấy thông tin phân công
                //DataContext.DataContext context = new DataContext.DataContext();
                UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
                var lich = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML.Trim() == FileName.Trim() && o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_DOIGCS == MA_DOIGCS && o.USERID == USERID);
                if (lich == null)
                {
                    return null;
                }
                if (lich.FILE_XML != FileName) //không cho phép lấy sổ nếu USERID ko được phân công
                {
                    DataRow row = dtERROR.NewRow();
                    row["ERROR"] = "LỖI: Không có quyền thao tác với sổ này.";
                    dtERROR.Rows.Add(row);
                    ds.Tables.Add(dtERROR);
                    return ds;
                }

                #region định nghĩa datatable dữ liệu cấu trúc file XML Table
                DataTable Table = new DataTable();
                Table.TableName = "Table";
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
                Table.Columns.Add("TTR_MOI");
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
                Table.Columns.Add("SLUONG_1");
                Table.Columns.Add("SLUONG_2");
                Table.Columns.Add("SLUONG_3");
                Table.Columns.Add("SO_HOM");
                Table.Columns.Add("PMAX");
                Table.Columns.Add("NGAY_PMAX");
                Table.Columns.Add("ID");
                Table.Columns.Add("GHICHU");
                Table.Columns.Add("TT_KHAC");
                Table.Columns.Add("CGPVTHD");
                Table.Columns.Add("HTHUC_TBAO_DK");
                Table.Columns.Add("DTHOAI_SMS");
                Table.Columns.Add("EMAIL");
                Table.Columns.Add("THOI_GIAN");
                Table.Columns.Add("X");
                Table.Columns.Add("Y");
                Table.Columns.Add("SO_TIEN");
                Table.Columns.Add("HTHUC_TBAO_TH");
                Table.Columns.Add("TENKHANG_RUTGON");
                Table.Columns.Add("TTHAI_DBO");
                Table.Columns.Add("DU_PHONG");
                Table.Columns.Add("TEN_FILE");
                Table.Columns.Add("STR_CHECK_DSOAT");
                #endregion

                //string path = Server.MapPath("~/TemplateFile/" + lich.MA_DVIQLY.Trim() + "/" + FileName);
                string path = Utility.getXMLPath() + lich.MA_DVIQLY.Trim() + @"\" + lich.FILE_XML;
                if (File.Exists(path) == false)
                {
                    DataRow row = dtERROR.NewRow();
                    row["ERROR"] = "LỖI: Không tìm thấy tệp.";
                    dtERROR.Rows.Add(row);
                    ds.Tables.Add(dtERROR);
                    return ds;
                }
                #region đọc file xml và lưu vào dataset
                XmlDocument fileXML = new XmlDocument();
                fileXML.Load(path);
                XmlNodeList nodeList = fileXML.DocumentElement.SelectNodes("/NewDataSet/Table1");
                var tableXML = Table.Clone();
                foreach (XmlNode node in nodeList)
                {
                    DataRow row = tableXML.NewRow();
                    #region gán giá trị của xml sang datatable
                    row["MA_NVGCS"] = node.SelectSingleNode("MA_NVGCS").InnerText;
                    row["MA_KHANG"] = node.SelectSingleNode("MA_KHANG").InnerText;
                    row["MA_DDO"] = node.SelectSingleNode("MA_DDO").InnerText;
                    row["MA_DVIQLY"] = node.SelectSingleNode("MA_DVIQLY").InnerText;
                    row["MA_GC"] = node.SelectSingleNode("MA_GC").InnerText;
                    row["MA_QUYEN"] = node.SelectSingleNode("MA_QUYEN").InnerText;
                    row["MA_TRAM"] = node.SelectSingleNode("MA_TRAM").InnerText;
                    row["BOCSO_ID"] = node.SelectSingleNode("BOCSO_ID").InnerText;
                    row["LOAI_BCS"] = node.SelectSingleNode("LOAI_BCS").InnerText;
                    row["LOAI_CS"] = node.SelectSingleNode("LOAI_CS").InnerText;
                    row["TEN_KHANG"] = node.SelectSingleNode("TEN_KHANG").InnerText;
                    row["DIA_CHI"] = node.SelectSingleNode("DIA_CHI").InnerText;
                    row["MA_NN"] = node.SelectSingleNode("MA_NN").InnerText;
                    row["SO_HO"] = node.SelectSingleNode("SO_HO").InnerText;
                    row["MA_CTO"] = node.SelectSingleNode("MA_CTO").InnerText;
                    row["SERY_CTO"] = node.SelectSingleNode("SERY_CTO").InnerText;
                    row["HSN"] = node.SelectSingleNode("HSN").InnerText;
                    row["CS_CU"] = node.SelectSingleNode("CS_CU").InnerText;
                    row["TTR_CU"] = node.SelectSingleNode("SL_CU").InnerText;
                    row["SL_TTIEP"] = node.SelectSingleNode("SL_TTIEP").InnerText;
                    row["NGAY_CU"] = node.SelectSingleNode("NGAY_CU").InnerText;
                    row["CS_MOI"] = node.SelectSingleNode("CS_MOI").InnerText;
                    row["TTR_MOI"] = node.SelectSingleNode("TTR_MOI").InnerText;
                    row["SL_MOI"] = node.SelectSingleNode("SL_MOI").InnerText;
                    row["CHUOI_GIA"] = node.SelectSingleNode("CHUOI_GIA").InnerText;
                    row["KY"] = node.SelectSingleNode("KY").InnerText;
                    row["THANG"] = node.SelectSingleNode("THANG").InnerText;
                    row["NAM"] = node.SelectSingleNode("NAM").InnerText;
                    row["NGAY_MOI"] = node.SelectSingleNode("NGAY_MOI").InnerText;
                    row["NGUOI_GCS"] = node.SelectSingleNode("NGUOI_GCS").InnerText;
                    row["SL_THAO"] = node.SelectSingleNode("SL_THAO").InnerText;
                    row["KIMUA_CSPK"] = node.SelectSingleNode("KIMUA_CSPK").InnerText;
                    row["MA_COT"] = node.SelectSingleNode("MA_COT").InnerText;
                    row["SLUONG_1"] = node.SelectSingleNode("SLUONG_1").InnerText;
                    row["SLUONG_2"] = node.SelectSingleNode("SLUONG_2").InnerText;
                    row["SLUONG_3"] = node.SelectSingleNode("SLUONG_3").InnerText;
                    if (node.SelectSingleNode("SO_HOM") != null) row["SO_HOM"] = node.SelectSingleNode("SO_HOM").InnerText; else row["SO_HOM"] = 0;
                    row["PMAX"] = node.SelectSingleNode("PMAX").InnerText;
                    row["NGAY_PMAX"] = node.SelectSingleNode("NGAY_PMAX").InnerText;
                    row["ID"] = node.SelectSingleNode("ID").InnerText;
                    row["GHICHU"] = node.SelectSingleNode("GHICHU").InnerText;
                    row["TT_KHAC"] = node.SelectSingleNode("TT_KHAC").InnerText;
                    row["CGPVTHD"] = node.SelectSingleNode("CGPVTHD").InnerText;
                    row["HTHUC_TBAO_DK"] = node.SelectSingleNode("HTHUC_TBAO_DK").InnerText;
                    row["DTHOAI_SMS"] = node.SelectSingleNode("DTHOAI_SMS").InnerText;
                    row["EMAIL"] = node.SelectSingleNode("EMAIL").InnerText;
                    row["THOI_GIAN"] = node.SelectSingleNode("THOI_GIAN").InnerText;
                    row["X"] = node.SelectSingleNode("X").InnerText;
                    row["Y"] = node.SelectSingleNode("Y").InnerText;
                    row["SO_TIEN"] = node.SelectSingleNode("SO_TIEN").InnerText;
                    row["HTHUC_TBAO_TH"] = node.SelectSingleNode("HTHUC_TBAO_TH").InnerText;
                    row["TENKHANG_RUTGON"] = node.SelectSingleNode("TENKHANG_RUTGON").InnerText;
                    row["TTHAI_DBO"] = node.SelectSingleNode("TTHAI_DBO").InnerText;
                    row["DU_PHONG"] = node.SelectSingleNode("DU_PHONG").InnerText;
                    row["TEN_FILE"] = node.SelectSingleNode("TEN_FILE").InnerText;
                    row["STR_CHECK_DSOAT"] = node.SelectSingleNode("STR_CHECK_DSOAT").InnerText;
                    #endregion
                    tableXML.Rows.Add(row);
                }
                ds.Tables.Add(tableXML);
            }
            catch (Exception ex)
            {
                DataRow row = dtERROR.NewRow();
                row["ERROR"] = "LỖI: " + ex.Message;
                dtERROR.Rows.Add(row);
                ds.Tables.Add(dtERROR);
                return ds;
            }
            #endregion
            return ds;
        }

        [WebMethod]
        public DataSet ReadXMLToHHCByUserAll(int KY, int THANG, int NAM, string MA_DVIQLY, string MA_DOIGCS, int USERID)
        { //lấy toàn bộ các sổ đc phân công cho nhân viên có USERID và trả về dataset
            #region định nghĩa bảng ERROR
            DataTable dtERROR = new DataTable();
            dtERROR.Columns.Add("ERROR");
            #endregion
            DataSet ds = new DataSet();
            try
            {
                //lấy thông tin các sổ được phân công
                UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
                var listLICH = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll(o => o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_DOIGCS == MA_DOIGCS && o.USERID == USERID && o.STATUS_PC == "DPC" && o.STATUS_CNCS == "CCN");
                if (listLICH != null)
                {
                    foreach (var lstSo in listLICH)
                    {
                        lstSo.HINH_THUC = "MTB";
                        UnitOfWork.RepoBase<GCS_LICHGCS>().Update(lstSo);
                    }
                }
                else
                {
                    return null;
                }

                #region định nghĩa datatable dữ liệu cấu trúc file XML Table
                DataTable Table = new DataTable();
                Table.TableName = "Table";
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
                Table.Columns.Add("SLUONG_1");
                Table.Columns.Add("SLUONG_2");
                Table.Columns.Add("SLUONG_3");
                Table.Columns.Add("SO_HOM");
                Table.Columns.Add("PMAX");
                Table.Columns.Add("NGAY_PMAX");
                #endregion

                #region đọc dữ liệu file .xml và lưu vào dataset
                foreach (var item in listLICH) //đọc lần lượt các sổ GCS trong file .xml
                {
                    //string path = Server.MapPath("~/TemplateFile/" + item.MA_DVIQLY.Trim() + "/" + item.FILE_XML);
                    string path = Utility.getXMLPath() + item.MA_DVIQLY.Trim() + @"\" + item.FILE_XML;
                    if (File.Exists(path))
                    {//tồn tại file xml thì đọc và add vào dataset
                        XmlDocument fileXML = new XmlDocument();
                        fileXML.Load(path);
                        XmlNodeList nodeList = fileXML.DocumentElement.SelectNodes("/NewDataSet/Table1");
                        var tableXML = Table.Clone();
                        tableXML.TableName = item.MA_SOGCS;
                        foreach (XmlNode node in nodeList)
                        {
                            DataRow row = tableXML.NewRow();
                            #region gán giá trị của xml sang datatable
                            row["MA_NVGCS"] = node.SelectSingleNode("MA_NVGCS").InnerText;
                            row["MA_KHANG"] = node.SelectSingleNode("MA_KHANG").InnerText;
                            row["MA_DDO"] = node.SelectSingleNode("MA_DDO").InnerText;
                            row["MA_DVIQLY"] = node.SelectSingleNode("MA_DVIQLY").InnerText;
                            row["MA_GC"] = node.SelectSingleNode("MA_GC").InnerText;
                            row["MA_QUYEN"] = node.SelectSingleNode("MA_QUYEN").InnerText;
                            row["MA_TRAM"] = node.SelectSingleNode("MA_TRAM").InnerText;
                            row["BOCSO_ID"] = node.SelectSingleNode("BOCSO_ID").InnerText;
                            row["LOAI_BCS"] = node.SelectSingleNode("LOAI_BCS").InnerText;
                            row["LOAI_CS"] = node.SelectSingleNode("LOAI_CS").InnerText;
                            row["TEN_KHANG"] = node.SelectSingleNode("TEN_KHANG").InnerText;
                            row["DIA_CHI"] = node.SelectSingleNode("DIA_CHI").InnerText;
                            row["MA_NN"] = node.SelectSingleNode("MA_NN").InnerText;
                            row["SO_HO"] = node.SelectSingleNode("SO_HO").InnerText;
                            row["MA_CTO"] = node.SelectSingleNode("MA_CTO").InnerText;
                            row["SERY_CTO"] = node.SelectSingleNode("SERY_CTO").InnerText;
                            row["HSN"] = node.SelectSingleNode("HSN").InnerText;
                            row["CS_CU"] = node.SelectSingleNode("CS_CU").InnerText;
                            row["TTR_CU"] = node.SelectSingleNode("SL_CU").InnerText;
                            row["SL_TTIEP"] = node.SelectSingleNode("SL_TTIEP").InnerText;
                            row["NGAY_CU"] = node.SelectSingleNode("NGAY_CU").InnerText;
                            row["CS_MOI"] = node.SelectSingleNode("CS_MOI").InnerText;
                            row["SL_MOI"] = node.SelectSingleNode("SL_MOI").InnerText;
                            row["CHUOI_GIA"] = node.SelectSingleNode("CHUOI_GIA").InnerText;
                            row["KY"] = node.SelectSingleNode("KY").InnerText;
                            row["THANG"] = node.SelectSingleNode("THANG").InnerText;
                            row["NAM"] = node.SelectSingleNode("NAM").InnerText;
                            row["NGAY_MOI"] = node.SelectSingleNode("NGAY_MOI").InnerText;
                            row["NGUOI_GCS"] = node.SelectSingleNode("NGUOI_GCS").InnerText;
                            row["SL_THAO"] = node.SelectSingleNode("SL_THAO").InnerText;
                            row["KIMUA_CSPK"] = node.SelectSingleNode("KIMUA_CSPK").InnerText;
                            row["SLUONG_1"] = node.SelectSingleNode("SLUONG_1").InnerText;
                            row["SLUONG_2"] = node.SelectSingleNode("SLUONG_2").InnerText;
                            row["SLUONG_3"] = node.SelectSingleNode("SLUONG_3").InnerText;
                            if (node.SelectSingleNode("SO_HOM") != null) row["SO_HOM"] = node.SelectSingleNode("SO_HOM").InnerText; else row["SO_HOM"] = 0;
                            row["PMAX"] = node.SelectSingleNode("PMAX").InnerText;
                            row["NGAY_PMAX"] = node.SelectSingleNode("NGAY_PMAX").InnerText;
                            #endregion
                            tableXML.Rows.Add(row);
                        }
                        ds.Tables.Add(tableXML);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                DataRow row = dtERROR.NewRow();
                row["ERROR"] = "LỖI: " + ex.Message;
                dtERROR.Rows.Add(row);
                ds.Tables.Add(dtERROR);
                return ds;
            }
            return ds;

        }

        [WebMethod]
        public byte[] ReadXMLToS3DB(int KY, int THANG, int NAM, string MA_DVIQLY, string MA_DOIGCS, int USERID, string IMEI)
        { //lấy toàn bộ các sổ đc phân công cho nhân viên có USERID và chuyển về file .s3db
            byte[] s3db = null; //file .s3db sẽ truyền về nếu hợp lệ
            try
            {
                string FileNameS3DB = "ESGCS_" + IMEI + ".s3db"; //tên file .s3db và chuyển cho MTB

                DataSet ds = new DataSet();
                #region định nghĩa bảng ERROR
                DataTable dtERROR = new DataTable();
                dtERROR.Columns.Add("ERROR");
                #endregion

                //lấy thông tin các sổ được phân công
                UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
                var listLICH = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll(o => o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_DOIGCS == MA_DOIGCS && o.USERID == USERID && o.STATUS_PC == "DPC" && o.STATUS_CNCS == "CCN");
                if (listLICH == null)
                {
                    DataRow row = dtERROR.NewRow();
                    row["ERROR"] = "LỖI: Không tìm thấy sổ được phân công.";
                    dtERROR.Rows.Add(row);
                    ds.Tables.Add(dtERROR);
                    return null;
                }

                #region định nghĩa datatable dữ liệu cấu trúc file XML Table
                DataTable Table = new DataTable();
                Table.TableName = "Table";
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
                Table.Columns.Add("SLUONG_1");
                Table.Columns.Add("SLUONG_2");
                Table.Columns.Add("SLUONG_3");
                Table.Columns.Add("SO_HOM");
                Table.Columns.Add("PMAX");
                Table.Columns.Add("NGAY_PMAX");
                #endregion
                //đường dẫn tới thư mục chứ file xml
                string path = Server.MapPath("~/");
                int str_length = path.Length - @"\WSGCS".Length;
                path = path.Substring(0, str_length);
                //path += @"TemplateFile\" + MA_DVIQLY.Trim() + @"\";
                path += Utility.getXMLPath() + MA_DVIQLY.Trim() + @"\";
                #region đọc dữ liệu file .xml và lưu vào dataset
                foreach (var item in listLICH) //đọc lần lượt các sổ GCS trong file .xml
                {

                    string pathXML = path + item.FILE_XML;
                    if (File.Exists(path))
                    {//tồn tại file xml thì đọc và add vào dataset
                        XmlDocument fileXML = new XmlDocument();
                        fileXML.Load(pathXML);
                        XmlNodeList nodeList = fileXML.DocumentElement.SelectNodes("/NewDataSet/Table1");
                        var tableXML = Table.Clone();
                        tableXML.TableName = item.MA_SOGCS;
                        foreach (XmlNode node in nodeList)
                        {
                            DataRow row = tableXML.NewRow();
                            #region gán giá trị của xml sang datatable
                            row["MA_NVGCS"] = node.SelectSingleNode("MA_NVGCS").InnerText;
                            row["MA_KHANG"] = node.SelectSingleNode("MA_KHANG").InnerText;
                            row["MA_DDO"] = node.SelectSingleNode("MA_DDO").InnerText;
                            row["MA_DVIQLY"] = node.SelectSingleNode("MA_DVIQLY").InnerText;
                            row["MA_GC"] = node.SelectSingleNode("MA_GC").InnerText;
                            row["MA_QUYEN"] = node.SelectSingleNode("MA_QUYEN").InnerText;
                            row["MA_TRAM"] = node.SelectSingleNode("MA_TRAM").InnerText;
                            row["BOCSO_ID"] = node.SelectSingleNode("BOCSO_ID").InnerText;
                            row["LOAI_BCS"] = node.SelectSingleNode("LOAI_BCS").InnerText;
                            row["LOAI_CS"] = node.SelectSingleNode("LOAI_CS").InnerText;
                            row["TEN_KHANG"] = node.SelectSingleNode("TEN_KHANG").InnerText;
                            row["DIA_CHI"] = node.SelectSingleNode("DIA_CHI").InnerText;
                            row["MA_NN"] = node.SelectSingleNode("MA_NN").InnerText;
                            row["SO_HO"] = node.SelectSingleNode("SO_HO").InnerText;
                            row["MA_CTO"] = node.SelectSingleNode("MA_CTO").InnerText;
                            row["SERY_CTO"] = node.SelectSingleNode("SERY_CTO").InnerText;
                            row["HSN"] = node.SelectSingleNode("HSN").InnerText;
                            row["CS_CU"] = node.SelectSingleNode("CS_CU").InnerText;
                            row["TTR_CU"] = node.SelectSingleNode("SL_CU").InnerText;
                            row["SL_TTIEP"] = node.SelectSingleNode("SL_TTIEP").InnerText;
                            row["NGAY_CU"] = node.SelectSingleNode("NGAY_CU").InnerText;
                            row["CS_MOI"] = node.SelectSingleNode("CS_MOI").InnerText;
                            row["SL_MOI"] = node.SelectSingleNode("SL_MOI").InnerText;
                            row["CHUOI_GIA"] = node.SelectSingleNode("CHUOI_GIA").InnerText;
                            row["KY"] = node.SelectSingleNode("KY").InnerText;
                            row["THANG"] = node.SelectSingleNode("THANG").InnerText;
                            row["NAM"] = node.SelectSingleNode("NAM").InnerText;
                            row["NGAY_MOI"] = node.SelectSingleNode("NGAY_MOI").InnerText;
                            row["NGUOI_GCS"] = node.SelectSingleNode("NGUOI_GCS").InnerText;
                            row["SL_THAO"] = node.SelectSingleNode("SL_THAO").InnerText;
                            row["KIMUA_CSPK"] = node.SelectSingleNode("KIMUA_CSPK").InnerText;
                            row["SLUONG_1"] = node.SelectSingleNode("SLUONG_1").InnerText;
                            row["SLUONG_2"] = node.SelectSingleNode("SLUONG_2").InnerText;
                            row["SLUONG_3"] = node.SelectSingleNode("SLUONG_3").InnerText;
                            if (node.SelectSingleNode("SO_HOM") != null) row["SO_HOM"] = node.SelectSingleNode("SO_HOM").InnerText; else row["SO_HOM"] = 0;
                            row["PMAX"] = node.SelectSingleNode("PMAX").InnerText;
                            row["NGAY_PMAX"] = node.SelectSingleNode("NGAY_PMAX").InnerText;
                            #endregion
                            tableXML.Rows.Add(row);
                        }
                        ds.Tables.Add(tableXML);
                    }
                }
                #endregion
                string pathTemp = path + @"temp\";
                if (!Directory.Exists(pathTemp)) //kiểm tra tồn tại thư mục tạm temp ko?
                {
                    Directory.CreateDirectory(pathTemp); //tạo mới thư mục tạm temp nếu không tồn tại
                }

                #region chuyển sang file .s3db
                string pathSQLite = pathTemp + FileNameS3DB;
                SQLite sqlite = new SQLite();
                string TableNameSQLite = "GCS_" + IMEI;
                sqlite.ConvertToSQLite(pathSQLite, TableNameSQLite, ds);
                #endregion


                if (File.Exists(pathSQLite))
                {
                    s3db = File.ReadAllBytes(pathSQLite);
                    File.Delete(pathSQLite);
                }
            }
            catch (Exception ex) { return null; }
            return s3db;

        }

        [WebMethod]
        public string WriteXMLFromHHC(string FileName, byte[] buffer, long Offset, int KY, int THANG, int NAM, string MA_DVIQLY, string MA_DOIGCS, int USERID)
        {
            UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
            string retVal = null;
            try
            {
                //lấy thông tin phân công
                var lich = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML == FileName && o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_SOGCS == MA_DOIGCS && o.USERID == USERID);
                if (lich == null) return "LỖI: Không tìm thấy sổ được phân công.";
                if (lich.FILE_XML == FileName) return "LỖI: Không có quyền thao tác với sổ này."; //không cho phép đẩy sổ lên nếu USERID ko được phân công
                                                                                                  // cấu hình save file trên server.
                                                                                                  //string FilePath = Path.Combine(Server.MapPath("~/TemplateFile/" + lich.MA_DVIQLY.Trim() + "/" + FileName));
                string FilePath = Utility.getXMLPath() + lich.MA_DVIQLY.Trim() + @"\" + FileName;
                if (Offset == 0) // tạo flie mới, khởi tạo một file trống
                    File.Create(FilePath).Close();
                // mở 1 file stream và ghi các the buffer.
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }
                retVal = "Thành công.";
            }
            catch (Exception ex)
            {
                retVal = "ERROR: " + ex.Message;
            }
            return retVal;
        }

        [WebMethod]
        public string WriteXMLFromS3DB(string FileName, byte[] buffer, long Offset, int KY, int THANG, int NAM, string MA_DVIQLY, string MA_DOIGCS, int USERID)
        {
            UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
            string retVal = null;
            try
            {
                //lấy thông tin phân công
                var lich = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML == FileName && o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_SOGCS == MA_DOIGCS && o.USERID == USERID);
                if (lich == null) return "LỖI: Không tìm thấy sổ được phân công.";
                if (lich.FILE_XML == FileName) return "LỖI: Không có quyền thao tác với sổ này."; //không cho phép đẩy sổ lên nếu USERID ko được phân công
                                                                                                  // cấu hình save file trên server.
                                                                                                  //string FilePath = Path.Combine(Server.MapPath("~/TemplateFile/" + lich.MA_DVIQLY.Trim() + "/" + FileName));
                string FilePath = Utility.getXMLPath() + lich.MA_DVIQLY.Trim() + @"\" + FileName;
                if (Offset == 0) // tạo flie mới, khởi tạo một file trống
                    File.Create(FilePath).Close();
                // mở 1 file stream và ghi các the buffer.
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }
                retVal = "Thành công.";
            }
            catch (Exception ex)
            {
                retVal = "ERROR: " + ex.Message;
            }
            return retVal;
        }

        #region các method phục vụ MTB
        //biến lấy dữ liệu từ database
        private UnitOfWork UnitOfWork = new BaseController().Uow;

        [WebMethod]
        public DataSet UserLogin(string userName, string pass)
        {
            try
            {
                //thực hiện kiểm tra user đã mã hóa (tham khảo anh NinhVQ)
                if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                var membership = (WebMatrix.WebData.SimpleMembershipProvider)Membership.Provider;
                var checkLogin = membership.ValidateUser(userName, pass);
                if (checkLogin == false) return null; //return null nếu đăng nhập sai

                //lấy thông tin người dùng khi đăng nhập đúng
                var loginInfo = UnitOfWork.RepoBase<UserProfile>().GetOne(o => o.UserName == userName);
                DataTable table = new DataTable();
                table.TableName = "UserInfo";
                #region định nghĩa bảng table giống với cấu trúc Service phiên bản Datastranfer
                table.Columns.Add("MA_DVIQLY");
                table.Columns.Add("MA_NV");
                table.Columns.Add("LOAI_NV");
                table.Columns.Add("TEN_NV");
                table.Columns.Add("MA_DQL"); //mã đội quản lý
                table.Columns.Add("GROUP_ID");
                table.Columns.Add("MAT_KHAU");
                #endregion

                DataRow row = table.NewRow();
                var id = UnitOfWork.RepoBase<UserProfile>().GetOne(x => x.UserName == userName).DepartmentId;
                row["MA_DVIQLY"] = UnitOfWork.RepoBase<AdministratorDepartment>().GetOne(o => o.DepartmentId == id).DepartmentCode;
                row["MA_NV"] = userName;
                row["LOAI_NV"] = 0;
                row["TEN_NV"] = loginInfo.FullName;
                row["MA_DQL"] = ""; // UnitOfWork.RepoBase<CFG_DOIGCS_NVIEN>().GetOne(o => o.USERID == loginInfo.UserId).MA_DOIGCS;
                row["GROUP_ID"] = "";
                row["MAT_KHAU"] = "";
                table.Rows.Add(row);

                if (table != null && table.Rows.Count > 0)
                {
                    DataSet dsUser = new DataSet();
                    dsUser.Tables.Add(table);
                    return dsUser;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public bool CheckIMEI(string IMEI)
        {
            try
            {
                return true;
                //var result = UnitOfWork.RepoBase<D_IMEI>().GetOne(o => o.IMEI == IMEI);
                //if (result != null)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public int CheckChotSo(string MA_DVIQLY, string TEN_FILE)
        {//trả về 1 nếu đã chốt sổ, trả về 0 nếu chưa chốt sổ (chốt sổ khi đã đối soát thành công)
            try
            {
                var listALL = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY && o.TEN_FILE == TEN_FILE).ToList(); //số lượng công tơ cần đối soát
                var listDoiSoat = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY && o.TEN_FILE == TEN_FILE && o.STR_CHECK_DSOAT == "CHECK").ToList(); //số lượng công tơ đã đối soát
                if (listALL.Count == listDoiSoat.Count) return 1;
                else return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private bool KiemTraSoDoiSoatBYMaSo(string MA_DVIQLY, string MA_SOGCS) //sổ đã chốt khi đối soát xong tất cả các công tơ
        {
            var listDoiSoat = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.MA_QUYEN == MA_SOGCS && o.MA_DVIQLY == MA_DVIQLY).ToList();
            if (listDoiSoat.Count == 0) return false;
            foreach (var item in listDoiSoat)
            {
                if (item.STR_CHECK_DSOAT != "CHUA_DOI_SOAT") continue;
                else return false; //có công tơ chưa đối soát thì trả về false
            }
            return true;
        }
        private bool KiemTraSoDoiSoatBYFileName(string MA_DVIQLY, string FileName) //sổ đã chốt khi đối soát xong tất cả các công tơ
        {
            var listDoiSoat = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.TEN_FILE == FileName && o.MA_DVIQLY == MA_DVIQLY).ToList();
            if (listDoiSoat.Count == 0) return false;
            foreach (var item in listDoiSoat)
            {
                if (item.STR_CHECK_DSOAT == "CHECK") continue;
                else return false; //có công tơ chưa đối soát thì trả về false
            }
            return true;
        }
        [WebMethod]
        public List<string> GetMaSoGCSOfNV(string MA_DVIQLY, string MA_NVGCS)
        {//Mã NVGCS là username đăng nhập hệ thống
            try
            {
                List<string> lst = new List<string>();

                var userID = UnitOfWork.RepoBase<UserProfile>().GetOne(o => o.UserName == MA_NVGCS).UserId; //mã nhân viên GCS

                var listSoGCSNV = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY && o.USERID == userID).ToList(); //danh sách các sổ đc phân công cho NV GCS

                List<GCS_LICHGCS> listChuaDoiSoat = new List<GCS_LICHGCS>();

                //loại bỏ các sổ đã được đối soát thành công
                foreach (var item in listSoGCSNV)
                {
                    if (item.NHANSO_MTB == "false")
                    {
                        if (KiemTraSoDoiSoatBYMaSo(item.MA_DVIQLY, item.MA_SOGCS) == false)
                            listChuaDoiSoat.Add(item); //thêm vào danh sách sổ chưa đối soát
                    }
                    else if (item.NHANSO_MTB == "true" && item.STATUS_CNCS == "CCN")
                    {
                        if (KiemTraSoDoiSoatBYMaSo(item.MA_DVIQLY, item.MA_SOGCS) == false)
                            listChuaDoiSoat.Add(item); //thêm vào danh sách sổ chưa đối soát
                    }
                }
                //lấy danh sách các tên file .xml của sổ chưa đối soát
                foreach (var item in listChuaDoiSoat)
                {
                    lst.Add(item.FILE_XML);
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string UploadRows(string MA_DVIQLY, string MA_NVGCS, string IMEI, string xmlData, string TEN_FILE)
        {
            try
            {
                //kiểm tra IMEI đã đăng ký chưa
                if (UnitOfWork.RepoBase<D_IMEI>().GetOne(o => o.IMEI == IMEI.Trim()) != null) // nếu ko có IMEI
                {
                    return "IMEI_INVALID";
                }

                //kiểm tra sổ đã chốt chưa
                if (KiemTraSoDoiSoatBYFileName(MA_DVIQLY, TEN_FILE))
                {
                    return "SO_DA_CHOT";
                }

                int result;
                string rs = null;
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlTextReader(new StringReader(xmlData)));




                //cập nhật db
                //if (gcsDAL.p_GCS_CHISO_HHU_Update_Rows_Mobile(MA_NVGCS, ds))
                //{
                //    rs = null;
                //}
                //else
                //{
                //    rs = "FAIL";
                //}

                return rs;
            }
            catch
            {
                return "FAIL";
            }
            finally
            {
                //gcsDAL.CloseConnection();

            }
        }
        [WebMethod]
        public long GetFileSize(string FileName)
        {
            try
            {
                string path = Server.MapPath("MobileUploads");
                string FilePath = Path.Combine(Server.MapPath(path), FileName);

                // kiểm tra file có tồn tại hay không
                if (!File.Exists(FilePath))
                    CustomSoapException("File not found", "The file " + path + " does not exist");

                return new FileInfo(FilePath).Length;
            }
            catch
            {
                return -1;
            }
        }
        [WebMethod]
        public string UploadChunk_Mobile(string FileName, byte[] buffer, long Offset, string MA_DVIQLY, string IMEI)
        {
            string retVal = null;
            try
            {
                //kiểm tra IMEI
                if (CheckIMEI(IMEI) == false)
                {
                    return "IMEI_INVALID";
                }

                string path = "MobileUploads";
                if (Directory.Exists(Server.MapPath("~/") + path) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("~/") + path);
                }

                // cấu hình save file trên server.
                string FilePath = Path.Combine(Server.MapPath(path), FileName);

                if (Offset == 0) // tạo flie mới, khởi tạo một file trống
                    File.Create(FilePath).Close();
                // mở 1 file stream và ghi các the buffer.
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }

                retVal = null;
            }
            catch (Exception ex)
            {
                retVal = "ERROR";
                //common.Log("ERROR: UploadChunk_Mobile (" + FileName + "," + MA_DVIQLY + "," + IMEI + ") -- DETAIL: " + ex.Message);
            }
            return retVal;
        }
        [WebMethod]
        public string UpdateGCS_CHISO_HHU(string MA_DVIQLY, string MA_NVGCS, string IMEI, string lstFileName)
        {
            int update_file_success = 0;
            string[] lst = lstFileName.Split(',');
            string path = Server.MapPath("MobileUploads");
            string path_extract = Path.Combine(path, IMEI);
            string zip_name = Path.Combine(path, "ESGCS_" + IMEI + ".zip"); // lấy ra db của máy đang đẩy dữ liệu lên
            string db_name = Path.Combine(path_extract, "ESGCS.s3db");

            // xóa dữ liệu cũ
            if (Directory.Exists(path_extract))
            {
                Directory.Delete(path_extract, true);
            }
            Directory.CreateDirectory(path_extract);
            // Giải nén file zip
            CommonExtend.ExtractFile(zip_name, path_extract);
            // Giải nén file zip ảnh
            foreach (string fileName in lst)
            {
                string zipImg = Path.Combine(path_extract, Path.ChangeExtension(fileName, ".zip"));
                CommonExtend.ExtractFile(zipImg, path_extract);
            }

            string list_ten_file = "";
            try
            {
                foreach (string fileName in lst)
                {
                    list_ten_file += ",'" + fileName + "'";
                    update_file_success++;
                }
                list_ten_file = list_ten_file.Substring(1);

                //kiểm tra IMEI đã đăng ký chưa
                if (CheckIMEI(IMEI) == false) // nếu ko có
                {
                    //common.Log("IMEI máy truy cập chưa đăng ký trên server");
                    return "IMEI_INVALID";
                }

                UpdateGCS_CHISO_HHU_Thread(list_ten_file, db_name, MA_DVIQLY, MA_NVGCS, path_extract, IMEI);

            }
            catch (Exception ex)
            {
                WriteLog("ERROR: UpdateGCS_CHISO_HHU(" + MA_DVIQLY + "," + MA_NVGCS + "," + IMEI + "," + lstFileName + ") -- DETAIL : " + ex.Message);
                return "ERROR";
            }
            //common.Log("END: UpdateGCS_CHISO_HHU(" + MA_DVIQLY + "," + MA_NVGCS + "," + IMEI + "," + lstFileName + ")");
            return update_file_success + "";
        }

        private void UpdateGCS_CHISO_HHU_Thread(string list_ten_file, string db_name, string MA_DVIQLY, string MA_NVGCS, string TMUC_EXTRACT, string IMEI)
        {
            UnitOfWork Uow = new UnitOfWork(new DataContext.DataContext());

            string query = null;
            string path = Server.MapPath("MobileUploads");
            if (File.Exists(db_name) == false)
            {
                string pathTemp = Server.MapPath("Temp");
                //string path_extract = Path.Combine(path, IMEI);
                string zip_name = Path.Combine(pathTemp, "ESGCS_" + IMEI); // lấy ra db của máy đang đẩy dữ liệu lên
                string db_copy = Path.Combine(zip_name, "ESGCS_" + IMEI + ".s3db");
                if (File.Exists(db_copy) == false)
                {
                    return;
                }
                System.IO.File.Copy(db_copy, db_name);

            }
            try
            {
                using (SQLiteDAO db_temp = new SQLiteDAO(db_name))
                {
                    try
                    {
                        DataTable dt = null;
                        if (list_ten_file.Trim().Length > 0)
                        {
                            query = "SELECT * FROM GCS_CHISO_HHU where TEN_FILE IN(" + list_ten_file + ")";
                            db_temp.OpenConn();
                            dt = db_temp.ExecuteQuery(query);
                            db_temp.CloseConn();

                            if (dt == null || dt.Rows.Count == 0)
                            {
                                return;
                            }

                            // lấy tất cả sổ trong db sqlite chèn vào sql server
                            DataSet ds = new DataSet();
                            dt = new CommonExtend().Convert2TemplateDataTable(dt, null);
                            ds.Tables.Add(dt);
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {   //cập nhật chỉ số thông qua file được đẩy lên từ thiết bị HHU
                                GCS_CHISO_HHU_DAL chisoHHU_DAL = new GCS_CHISO_HHU_DAL();
                                bool kq = chisoHHU_DAL.CapNhatChiSoHHU(MA_DVIQLY, MA_NVGCS, GcsImages, TMUC_EXTRACT, ds);
                                if (kq == true)
                                {//xuất trả file xml để cập nhật nếu cập nhật thành công
                                    string listFile = list_ten_file.Substring(1, list_ten_file.Length - 2);//loại bỏ 2 dấu nháu đơn
                                    var replace = listFile.Replace("'", "");
                                    string[] lstFile = replace.Split(','); //lấy từng file để cập nhật

                                    foreach (var tenFile in lstFile)
                                    {
                                        string pathServer = Server.MapPath("~/");
                                        int str_length = pathServer.Length - @"\WSGCS".Length;
                                        string pathXML = pathServer.Substring(0, str_length);
                                        //pathXML += @"TemplateFile\" + MA_DVIQLY.Trim() + @"\" + tenFile.Trim();
                                        //ThanhVT sửa, chú ý xem có chỗ khác dùng cái này ko
                                        pathXML = Utility.getXMLPath() + MA_DVIQLY.Trim() + @"\" + tenFile.Trim();
                                        path = Utility.getXMLPath() + MA_DVIQLY.Trim() + @"\" + tenFile.Trim();
                                        //lấy về chi tiết sổ GCS
                                        IEnumerable<GCS_CHISO_HHU> lstCapNhatChiSo = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.TEN_FILE == tenFile).ToList();
                                        //chuyển đổi về dataset và lưu XML
                                        DataTable dtCapNhatChiSo = new DataTable();
                                        dtCapNhatChiSo = ToDataTable(lstCapNhatChiSo);
                                        dtCapNhatChiSo.TableName = "Table1";//đổi tên bảng giống với file ban đầu
                                        //bỏ các cột của datatable để giống tệp ban đầu
                                        dtCapNhatChiSo.Columns.Remove("ID");
                                        dtCapNhatChiSo.Columns.Remove("MA_COT");
                                        dtCapNhatChiSo.Columns.Remove("CGPVTHD");
                                        dtCapNhatChiSo.Columns.Remove("HTHUC_TBAO_DK");
                                        dtCapNhatChiSo.Columns.Remove("DTHOAI_SMS");
                                        dtCapNhatChiSo.Columns.Remove("EMAIL");
                                        dtCapNhatChiSo.Columns.Remove("THOI_GIAN");
                                        dtCapNhatChiSo.Columns.Remove("X");
                                        dtCapNhatChiSo.Columns.Remove("Y");
                                        dtCapNhatChiSo.Columns.Remove("SO_TIEN");
                                        dtCapNhatChiSo.Columns.Remove("HTHUC_TBAO_TH");
                                        dtCapNhatChiSo.Columns.Remove("TENKHANG_RUTGON");
                                        dtCapNhatChiSo.Columns.Remove("TTHAI_DBO");
                                        dtCapNhatChiSo.Columns.Remove("DU_PHONG");
                                        dtCapNhatChiSo.Columns.Remove("GHICHU");
                                        dtCapNhatChiSo.Columns.Remove("TT_KHAC");
                                        dtCapNhatChiSo.Columns.Remove("ANH_GCS");
                                        //dtCapNhatChiSo.Columns.Remove("STR_CHECK_DSOAT");
                                        DataSet dsCapNhatChiSo = new DataSet();
                                        dsCapNhatChiSo.Tables.Add(dtCapNhatChiSo);//thêm  datatable vào dataset để tiến hành lưu XML
                                        dsCapNhatChiSo.WriteXml(pathXML); //lưu xml

                                        var lst = new List<string>();
                                        var numberRow = dsCapNhatChiSo.Tables[0].Select("CS_MOI is not null").Length;
                                        if (dsCapNhatChiSo.Tables.Count > 0)
                                        {
                                            foreach (DataTable table in dsCapNhatChiSo.Tables)
                                            {
                                                foreach (DataRow row in table.Rows)
                                                {
                                                    var y = row["CS_MOI"].ToString();
                                                    var z = row["STR_CHECK_DSOAT"].ToString();
                                                    long bcsId = Int64.Parse(row["BOCSO_ID"].ToString());

                                                    if (z == "CTO_DTU")
                                                    {
                                                        var lichGcsHhu =
                                                            UnitOfWork.RepoBase<GCS_CHISO_HHU>()
                                                                .GetOne(o => o.BOCSO_ID == bcsId);
                                                        if (lichGcsHhu != null)
                                                        {
                                                            lichGcsHhu.STR_CHECK_DSOAT = "CTO_DTU";
                                                            UnitOfWork.RepoBase<GCS_CHISO_HHU>().Update(lichGcsHhu);
                                                        }
                                                    }
                                                    var csMoi = Convert.ToInt32(Math.Round(Convert.ToDouble(y)));
                                                    if (csMoi > 0)
                                                    {
                                                        lst.Add(y);
                                                    }
                                                }
                                            }
                                        }
                                        var countRows = lst.Count();

                                        //Cập nhật trạng thái sổ trong bảng Lịch
                                        var LichGCS = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML.Trim() == tenFile.Trim());
                                        if (LichGCS != null)
                                        {
                                            LichGCS.STATUS_CNCS = "DCN";
                                            LichGCS.STATUS_NVK = "NVCK";
                                            LichGCS.NHANSO_MTB = "true";
                                            LichGCS.STATUS_DTK = numberRow.ToString();
                                            LichGCS.STATUS_DHK = countRows.ToString();
                                            UnitOfWork.RepoBase<GCS_LICHGCS>().Update(LichGCS);

                                            // Lưu Log
                                            var userId = UnitOfWork.RepoBase<UserProfile>().GetOne(x => x.UserName == MA_NVGCS).UserId;
                                            //var lichGcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                                            var logCategoryId = "CNCS_MTB";
                                            var contentLog = "";
                                            DateTime logDate = DateTime.Now;
                                            var maBangKeLich = "";
                                            var logStatus = "DCN_MTB";
                                            var lstCategoryLog =
                                                UnitOfWork.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                                            var userName = UnitOfWork.RepoBase<UserProfile>().GetOne(x => x.UserName == MA_NVGCS).UserName;
                                            var countThucHien = 1;
                                            contentLog = userName + " " + lstCategoryLog + " sổ " + LichGCS.TEN_SOGCS + " thành công";
                                            WriteLog writeL = new WriteLog(UnitOfWork);
                                            writeL.WriteLogGcs(logCategoryId, LichGCS.ID_LICHGCS, LichGCS.MA_SOGCS, LichGCS.KY, LichGCS.THANG, LichGCS.NAM, contentLog, userId, logDate, maBangKeLich,
                                                countThucHien, logStatus);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Service1.WriteLog("ERROR: [1]UpdateGCS_CHISO_HHU_Thread(" + list_ten_file + "," + db_name + "," + MA_DVIQLY + "," + MA_NVGCS + "," + TMUC_EXTRACT + ") -- DETAIL : " + e.Message);
                    }
                }
            }
            catch (Exception e2)
            {
                WriteLog("ERROR: [2]UpdateGCS_CHISO_HHU_Thread(" + list_ten_file + "," + db_name + "," + MA_DVIQLY + "," + MA_NVGCS + "," + TMUC_EXTRACT + ") -- DETAIL : " + e2.Message);
            }
            finally
            {
                //gcs_dal.CloseConnection();
                GC.Collect();
            }
        }

        [WebMethod]
        public byte[] DownloadSoGCS_MOBILE(string MA_DVIQLY, string MA_NVGCS, string IMEI, string TEN_FILE)
        { //MA_NVGCS là username đăng nhập hệ thống
            string[] TEN_FILES = TEN_FILE.Split(',');
            // thêm IMEI vào tên db để tránh bị trùng db nếu nhiều máy cùng lấy dữ liệu 1 lúc
            string str_db_file = HttpContext.Current.Server.MapPath("~/") + "Temp\\ESGCS_" + IMEI + "\\ESGCS_" + IMEI + ".s3db";

            string sql_result = null;
            //kiểm tra folder temp đã có chưa
            string temp_folder = Server.MapPath("~/Temp");
            string temp_folder_mtb = temp_folder + "\\ESGCS_" + IMEI;
            // xóa folder temp của mtb đang kết nối
            if (Directory.Exists(temp_folder_mtb))
            {
                Directory.Delete(temp_folder_mtb, true);
            }
            Directory.CreateDirectory(temp_folder_mtb);
            // tạo mới db
            sql_result = commonSQLite.CheckExistDbSqlite(str_db_file, true);
            if (sql_result != "true")
                return null;

            using (SQLiteDAO db_sqlite = new SQLiteDAO(str_db_file))
            {
                try
                {
                    //kiểm tra IMEI đã đăng ký chưa
                    if (CheckIMEI(IMEI) == false) // nếu IMEI chua dang ki thi thoat
                    {
                        return null;
                    }

                    //khai báo biến sử dụng tiện ích
                    CommonExtend commonExtend = new CommonExtend();

                    db_sqlite.OpenConn();
                    db_sqlite.BeginTransaction();

                    DataSet ds = new DataSet();
                    byte[] bytes = null;

                    // lấy hết sổ được giao cho nhân viên
                    DataTable dt = new DataTable();
                    foreach (string file_name in TEN_FILES)
                    {
                        //  Sổ GCS chỉ được xuất hiện ở một nơi, VD khi nhận về MTB rồi không được nhận lại lần nữa (hiện đang lấy thoải mái) 
                        //  có hỗ trợ reset trạng thái để tránh người dùng lấy lại dữ liệu không để ý > mất dữ liệu
                        var lichGcsCheck = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML.Trim() == file_name.Trim() && o.NHANSO_MTB == "true");
                        if (lichGcsCheck != null)
                        {
                            continue;
                        }
                        dt = null;
                        //cập nhật chi tiết sổ GCS vào GCS_CHISO_HHU
                        DataSet dsXML = new DataSet();
                        dsXML = ReadXML(file_name); //đọc DL file xml và lưu vào dataset
                        foreach (DataRow row in dsXML.Tables[0].Rows)
                        {
                            string madonvi = row["MA_DVIQLY"].ToString();
                            string ma_DDO = row["MA_DDO"].ToString();
                            string ma_khang = row["MA_KHANG"].ToString();
                            string ma_cto = row["MA_CTO"].ToString();
                            string ma_SOGCS = row["MA_QUYEN"].ToString();
                            string loaiBCS = row["LOAI_BCS"].ToString();
                            var lstchiso = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.MA_DVIQLY == madonvi && o.MA_DDO == ma_DDO && o.MA_CTO == ma_cto
                                                && o.MA_KHANG == ma_khang & o.MA_QUYEN == ma_SOGCS && o.TEN_FILE == file_name && o.LOAI_BCS == loaiBCS).OrderBy(c => c.MA_KHANG).ThenBy(c => c.LOAI_BCS).ToList();
                            if (lstchiso.Count == 0) //thêm mới chi tiết điểm đo vào bảng GCS_CHISO_HHU nếu chưa có
                            {
                                GCS_CHISO_HHU chiso = new GCS_CHISO_HHU();
                                #region gán dữ liệu table vào model, chuẩn bị insert vào database
                                chiso.MA_NVGCS = MA_NVGCS;
                                chiso.MA_KHANG = ma_khang;
                                chiso.MA_DDO = ma_DDO;
                                chiso.MA_DVIQLY = madonvi;
                                chiso.MA_GC = row["MA_GC"].ToString();
                                chiso.MA_QUYEN = ma_SOGCS;
                                chiso.MA_TRAM = row["MA_TRAM"].ToString();
                                chiso.BOCSO_ID = Convert.ToInt64(row["BOCSO_ID"].ToString());
                                chiso.LOAI_BCS = loaiBCS;
                                chiso.LOAI_CS = row["LOAI_CS"].ToString();
                                chiso.TEN_KHANG = row["TEN_KHANG"].ToString();
                                chiso.DIA_CHI = row["DIA_CHI"].ToString();
                                chiso.MA_NN = row["MA_NN"].ToString();
                                chiso.SO_HO = Convert.ToDecimal(row["SO_HO"].ToString());
                                chiso.MA_CTO = row["MA_CTO"].ToString();
                                chiso.SERY_CTO = row["SERY_CTO"].ToString();
                                chiso.HSN = Convert.ToDecimal(row["HSN"].ToString());
                                chiso.CS_CU = Convert.ToDecimal(row["CS_CU"].ToString());
                                chiso.TTR_CU = row["TTR_CU"].ToString();
                                if (row["SL_CU"].ToString() != "") chiso.SL_CU = Convert.ToDecimal(row["SL_CU"].ToString()); else chiso.SL_CU = 1245;
                                chiso.SL_TTIEP = Convert.ToInt32(row["SL_TTIEP"].ToString());
                                chiso.NGAY_CU = Convert.ToDateTime(row["NGAY_CU"].ToString());
                                chiso.CS_MOI = Convert.ToDecimal(row["CS_MOI"].ToString());
                                chiso.SL_MOI = Convert.ToDecimal(row["SL_MOI"].ToString());
                                chiso.CHUOI_GIA = row["CHUOI_GIA"].ToString();
                                chiso.KY = Convert.ToInt32(row["KY"].ToString());
                                chiso.THANG = Convert.ToInt32(row["THANG"].ToString());
                                chiso.NAM = Convert.ToInt32(row["NAM"].ToString());
                                chiso.NGAY_MOI = Convert.ToDateTime(row["NGAY_MOI"].ToString());
                                chiso.NGUOI_GCS = row["NGUOI_GCS"].ToString();
                                chiso.SL_THAO = Convert.ToDecimal(row["SL_THAO"].ToString());
                                chiso.KIMUA_CSPK = Convert.ToInt16(row["KIMUA_CSPK"].ToString());
                                chiso.CGPVTHD = row["CGPVTHD"].ToString();
                                chiso.TEN_FILE = file_name;
                                chiso.SLUONG_1 = Convert.ToDecimal(row["SLUONG_1"].ToString());
                                chiso.SLUONG_2 = Convert.ToDecimal(row["SLUONG_2"].ToString());
                                chiso.SLUONG_3 = Convert.ToDecimal(row["SLUONG_3"].ToString());
                                if (row["SO_HOM"].ToString() != "") chiso.SO_HOM = row["SO_HOM"].ToString();
                                chiso.ANH_GCS = "";
                                chiso.PMAX = Convert.ToDecimal(row["PMAX"].ToString());
                                chiso.NGAY_PMAX = Convert.ToDateTime(row["NGAY_PMAX"].ToString());
                                if (chiso.NGAY_PMAX < new DateTime(1753, 1, 1)) chiso.NGAY_PMAX = new DateTime(1753, 1, 1);
                                chiso.STR_CHECK_DSOAT = "CHUA_DOI_SOAT"; // row["STR_CHECK_DSOAT"].ToString();
                                //chiso.ID = Convert.ToInt32(row["ID"].ToString());
                                //chiso.TTR_MOI = row["TTR_MOI"].ToString();
                                //chiso.TT_KHAC = row["TT_KHAC"].ToString();
                                //chiso.MA_COT = row["MA_COT"].ToString();
                                //chiso.HTHUC_TBAO_DK = row["HTHUC_TBAO_DK"].ToString();
                                //chiso.DTHOAI_SMS = row["DTHOAI_SMS"].ToString();
                                //chiso.EMAIL = row["EMAIL"].ToString();
                                //chiso.THOI_GIAN = row["THOI_GIAN"].ToString();
                                //chiso.X = Convert.ToDecimal(row["X"].ToString());
                                //chiso.Y = Convert.ToDecimal(row["Y"].ToString());
                                //chiso.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"].ToString());
                                //chiso.HTHUC_TBAO_TH = row["HTHUC_TBAO_TH"].ToString();
                                //chiso.TENKHANG_RUTGON = row["TENKHANG_RUTGON"].ToString();
                                //chiso.TTHAI_DBO = Convert.ToByte(row["TTHAI_DBO"].ToString());
                                //chiso.DU_PHONG = row["DU_PHONG"].ToString();
                                //chiso.GHICHU = row["GHICHU"].ToString();
                                #endregion
                                UnitOfWork.RepoBase<GCS_CHISO_HHU>().Create(chiso); //insert DL chi tiết điểm đo vào bảng GCS_CHISO_HHU
                            }
                        }

                        //lấy về chi tiết các sổ GCS
                        IEnumerable<GCS_CHISO_HHU> lstChiTietSo = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetAll(o => o.MA_DVIQLY == MA_DVIQLY && o.TEN_FILE == file_name).OrderBy(c => c.MA_KHANG).ThenBy(c => c.LOAI_BCS).ToList();
                        dt = ToDataTable(lstChiTietSo);
                        //dt = gcsDAL.p_GCS_CHISO_HHU_GetDataSoGCS(MA_DVIQLY, file_name.Trim(), "SO_CHUA_CHOT", 0, 0, 0);
                        dt.Columns.Add("HINH_ANH", typeof(byte[]));
                        //insert datatable to sqlite
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.Columns.Remove("ANH_GCS");
                            db_sqlite.InsertDataTableToDbSqlite(dt, dt.Rows[0]["TEN_FILE"].ToString());
                            //string rs = gcsDAL.p_GCS_LOG_HHC_Update_TRANG_THAI(MA_DVIQLY
                            //                                    , dt.Rows[0]["TEN_FILE"].ToString()
                            //                                    , int.Parse(dt.Rows[0]["KY"].ToString())
                            //                                    , int.Parse(dt.Rows[0]["THANG"].ToString())
                            //                                    , int.Parse(dt.Rows[0]["NAM"].ToString())
                            //                                    , 1, false);
                        }
                        var lichGcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML.Trim() == file_name.Trim() && o.NHANSO_MTB == "false");
                        if (lichGcs != null)
                        {
                            lichGcs.NHANSO_MTB = "true";
                            UnitOfWork.RepoBase<GCS_LICHGCS>().Update(lichGcs);

                            // Lưu Log
                            var userId = UnitOfWork.RepoBase<UserProfile>().GetOne(x => x.UserName == MA_NVGCS).UserId;
                            //var lichGcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(x => x.ID_LICHGCS == item);
                            var logCategoryId = "NS_MTB";
                            var contentLog = "";
                            DateTime logDate = DateTime.Now;
                            var maBangKeLich = "";
                            var logStatus = "DN_MTB";
                            var lstCategoryLog =
                                UnitOfWork.RepoBase<LOG_CATEGORY>().GetOne(x => x.LOG_CATEGORY_ID == logCategoryId).LOG_CATEGORY_NAME;
                            var userName = UnitOfWork.RepoBase<UserProfile>().GetOne(x => x.UserName == MA_NVGCS).UserName;
                            var countThucHien = 1;
                            contentLog = userName + " " + lstCategoryLog + " sổ " + lichGcs.TEN_SOGCS + " thành công";
                            WriteLog writeL = new WriteLog(UnitOfWork);
                            writeL.WriteLogGcs(logCategoryId, lichGcs.ID_LICHGCS, lichGcs.MA_SOGCS, lichGcs.KY, lichGcs.THANG, lichGcs.NAM, contentLog, userId, logDate, maBangKeLich,
                                countThucHien, logStatus);
                        }
                    }

                    db_sqlite.CommitTransaction();

                    DataTable dtKoDat = db_sqlite.ExecuteQuery("SELECT * FROM GCS_CHISO_HHU WHERE STR_CHECK_DSOAT = 'UNCHECK' order by MA_KHANG, LOAI_BCS");

                    db_sqlite.CloseConn();

                    // Lấy ảnh không đạt của sổ
                    foreach (DataRow dr in dtKoDat.Rows)
                    {
                        int nam = int.Parse(dr["NAM"].ToString().Trim());
                        int thang = int.Parse(dr["THANG"].ToString().Trim());
                        int ky = int.Parse(dr["KY"].ToString().Trim());
                        string ten_file = Regex.Replace(dr["TEN_FILE"].ToString(), ".XML", "", RegexOptions.IgnoreCase);
                        string img_name = commonExtend.ConvertDrToImageName(dr);
                        string sub_folder = GcsImages + "\\" + nam + "_" + thang + "_" + ky + "\\" + ten_file;
                        string img_fullname = sub_folder + "\\" + img_name;
                        string PathImage = Server.MapPath("~/GCSImages") + img_fullname;
                        if (!File.Exists(PathImage))
                        {
                            continue;
                        }

                        // chuyển ảnh vào thư mục tạm để nén
                        string folder_des = temp_folder_mtb + "\\" + ten_file;
                        Directory.CreateDirectory(folder_des);
                        File.Copy(PathImage, folder_des + "\\" + img_name, true);
                    }

                    // nén folder chứa ảnh và file sqlite chỉ số

                    string file_zip = temp_folder_mtb + "\\ESGCS_" + IMEI + ".zip";
                    string compress_result = CommonExtend.CompressionFolder(temp_folder_mtb, file_zip);
                    //WriteLog(compress_result);

                    // convert sqlite to byte[]
                    //bytes = System.IO.File.ReadAllBytes(str_db_file);
                    bytes = System.IO.File.ReadAllBytes(file_zip);

                    //gcsDAL.CommitTransaction();
                    return bytes;
                }
                catch (Exception ex)
                {
                    db_sqlite.RollbackTransaction();
                    return null;
                }
                finally
                {
                    db_sqlite.CloseConn();
                }
            }
        }


        #endregion

        #region Các api giao tiếp HHU
        [WebMethod]
        public DataSet ReadXMLToHHU(int KY, int THANG, int NAM, string MA_DVIQLY, string MA_SOGCS)
        {
            DataSet ds = new DataSet();
            #region định nghĩa bảng ERROR
            DataTable dtERROR = new DataTable();
            dtERROR.Columns.Add("ERROR");
            #endregion
            try
            {
                //lấy thông tin phân công
                //DataContext.DataContext context = new DataContext.DataContext();
                UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
                var lich = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.KY == KY && o.THANG == THANG
                    && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_SOGCS == MA_SOGCS && o.STATUS == "DL");
                if (lich == null)
                {
                    return null;
                }
                //if (lich.FILE_XML != FileName) //không cho phép lấy sổ nếu USERID ko được phân công
                //{
                //    DataRow row = dtERROR.NewRow();
                //    row["ERROR"] = "LỖI: Không có quyền thao tác với sổ này.";
                //    dtERROR.Rows.Add(row);
                //    ds.Tables.Add(dtERROR);
                //    return ds;
                //}

                #region định nghĩa datatable dữ liệu cấu trúc file XML Table
                DataTable Table = new DataTable();
                Table.TableName = "Table";
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
                Table.Columns.Add("TTR_MOI");
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
                Table.Columns.Add("SLUONG_1");
                Table.Columns.Add("SLUONG_2");
                Table.Columns.Add("SLUONG_3");
                Table.Columns.Add("SO_HOM");
                Table.Columns.Add("PMAX");
                Table.Columns.Add("NGAY_PMAX");
                Table.Columns.Add("ID");
                Table.Columns.Add("GHICHU");
                Table.Columns.Add("TT_KHAC");
                Table.Columns.Add("CGPVTHD");
                Table.Columns.Add("HTHUC_TBAO_DK");
                Table.Columns.Add("DTHOAI_SMS");
                Table.Columns.Add("EMAIL");
                Table.Columns.Add("THOI_GIAN");
                Table.Columns.Add("X");
                Table.Columns.Add("Y");
                Table.Columns.Add("SO_TIEN");
                Table.Columns.Add("HTHUC_TBAO_TH");
                Table.Columns.Add("TENKHANG_RUTGON");
                Table.Columns.Add("TTHAI_DBO");
                Table.Columns.Add("DU_PHONG");
                Table.Columns.Add("TEN_FILE");
                Table.Columns.Add("STR_CHECK_DSOAT");
                #endregion

                //string path = Server.MapPath("~/TemplateFile/" + lich.MA_DVIQLY.Trim() + "/" + FileName);
                string path = Utility.getXMLPath() + lich.MA_DVIQLY.Trim() + @"\" + lich.FILE_XML;
                if (File.Exists(path) == false)
                {
                    DataRow row = dtERROR.NewRow();
                    row["ERROR"] = "LỖI: Không tìm thấy tệp.";
                    dtERROR.Rows.Add(row);
                    ds.Tables.Add(dtERROR);
                    return ds;
                }
                #region đọc file xml và lưu vào dataset
                XmlDocument fileXML = new XmlDocument();
                fileXML.Load(path);
                XmlNodeList nodeList = fileXML.DocumentElement.SelectNodes("/NewDataSet/Table1");
                var tableXML = Table.Clone();
                foreach (XmlNode node in nodeList)
                {
                    DataRow row = tableXML.NewRow();
                    #region gán giá trị của xml sang datatable
                    row["MA_NVGCS"] = node.SelectSingleNode("MA_NVGCS").InnerText;
                    row["MA_KHANG"] = node.SelectSingleNode("MA_KHANG").InnerText;
                    row["MA_DDO"] = node.SelectSingleNode("MA_DDO").InnerText;
                    row["MA_DVIQLY"] = node.SelectSingleNode("MA_DVIQLY").InnerText;
                    row["MA_GC"] = node.SelectSingleNode("MA_GC").InnerText;
                    row["MA_QUYEN"] = node.SelectSingleNode("MA_QUYEN").InnerText;
                    row["MA_TRAM"] = node.SelectSingleNode("MA_TRAM").InnerText;
                    row["BOCSO_ID"] = node.SelectSingleNode("BOCSO_ID").InnerText;
                    row["LOAI_BCS"] = node.SelectSingleNode("LOAI_BCS").InnerText;
                    row["LOAI_CS"] = node.SelectSingleNode("LOAI_CS").InnerText;
                    row["TEN_KHANG"] = node.SelectSingleNode("TEN_KHANG").InnerText;
                    row["DIA_CHI"] = node.SelectSingleNode("DIA_CHI").InnerText;
                    row["MA_NN"] = node.SelectSingleNode("MA_NN").InnerText;
                    row["SO_HO"] = node.SelectSingleNode("SO_HO").InnerText;
                    row["MA_CTO"] = node.SelectSingleNode("MA_CTO").InnerText;
                    row["SERY_CTO"] = node.SelectSingleNode("SERY_CTO").InnerText;
                    row["HSN"] = node.SelectSingleNode("HSN").InnerText;
                    row["CS_CU"] = node.SelectSingleNode("CS_CU").InnerText;
                    row["TTR_CU"] = node.SelectSingleNode("SL_CU").InnerText;
                    row["SL_TTIEP"] = node.SelectSingleNode("SL_TTIEP").InnerText;
                    row["NGAY_CU"] = node.SelectSingleNode("NGAY_CU").InnerText;
                    row["CS_MOI"] = node.SelectSingleNode("CS_MOI").InnerText;
                    row["TTR_MOI"] = node.SelectSingleNode("TTR_MOI").InnerText;
                    row["SL_MOI"] = node.SelectSingleNode("SL_MOI").InnerText;
                    row["CHUOI_GIA"] = node.SelectSingleNode("CHUOI_GIA").InnerText;
                    row["KY"] = node.SelectSingleNode("KY").InnerText;
                    row["THANG"] = node.SelectSingleNode("THANG").InnerText;
                    row["NAM"] = node.SelectSingleNode("NAM").InnerText;
                    row["NGAY_MOI"] = node.SelectSingleNode("NGAY_MOI").InnerText;
                    row["NGUOI_GCS"] = node.SelectSingleNode("NGUOI_GCS").InnerText;
                    row["SL_THAO"] = node.SelectSingleNode("SL_THAO").InnerText;
                    row["KIMUA_CSPK"] = node.SelectSingleNode("KIMUA_CSPK").InnerText;
                    row["MA_COT"] = node.SelectSingleNode("MA_COT").InnerText;
                    row["SLUONG_1"] = node.SelectSingleNode("SLUONG_1").InnerText;
                    row["SLUONG_2"] = node.SelectSingleNode("SLUONG_2").InnerText;
                    row["SLUONG_3"] = node.SelectSingleNode("SLUONG_3").InnerText;
                    if (node.SelectSingleNode("SO_HOM") != null) row["SO_HOM"] = node.SelectSingleNode("SO_HOM").InnerText; else row["SO_HOM"] = 0;
                    row["PMAX"] = node.SelectSingleNode("PMAX").InnerText;
                    row["NGAY_PMAX"] = node.SelectSingleNode("NGAY_PMAX").InnerText;
                    //row["ID"] = node.SelectSingleNode("ID").InnerText;
                    //row["GHICHU"] = node.SelectSingleNode("GHICHU").InnerText;
                    //row["TT_KHAC"] = node.SelectSingleNode("TT_KHAC").InnerText;
                    //row["CGPVTHD"] = node.SelectSingleNode("CGPVTHD").InnerText;
                    //row["HTHUC_TBAO_DK"] = node.SelectSingleNode("HTHUC_TBAO_DK").InnerText;
                    //row["DTHOAI_SMS"] = node.SelectSingleNode("DTHOAI_SMS").InnerText;
                    //row["EMAIL"] = node.SelectSingleNode("EMAIL").InnerText;
                    //row["THOI_GIAN"] = node.SelectSingleNode("THOI_GIAN").InnerText;
                    //row["X"] = node.SelectSingleNode("X").InnerText;
                    //row["Y"] = node.SelectSingleNode("Y").InnerText;
                    //row["SO_TIEN"] = node.SelectSingleNode("SO_TIEN").InnerText;
                    //row["HTHUC_TBAO_TH"] = node.SelectSingleNode("HTHUC_TBAO_TH").InnerText;
                    //row["TENKHANG_RUTGON"] = node.SelectSingleNode("TENKHANG_RUTGON").InnerText;
                    //row["TTHAI_DBO"] = node.SelectSingleNode("TTHAI_DBO").InnerText;
                    //row["DU_PHONG"] = node.SelectSingleNode("DU_PHONG").InnerText;
                    //row["TEN_FILE"] = node.SelectSingleNode("TEN_FILE").InnerText;
                    //row["STR_CHECK_DSOAT"] = node.SelectSingleNode("STR_CHECK_DSOAT").InnerText;
                    #endregion
                    tableXML.Rows.Add(row);
                }
                ds.Tables.Add(tableXML);
            }
            catch (Exception ex)
            {
                DataRow row = dtERROR.NewRow();
                row["ERROR"] = "LỖI: " + ex.Message;
                dtERROR.Rows.Add(row);
                ds.Tables.Add(dtERROR);
                return ds;
            }
            #endregion
            return ds;
        }

        [WebMethod]
        public string WriteXMLFromHHU(DataSet ds, int KY, int THANG, int NAM, string MA_DVIQLY, string MA_SOGCS)
        {
            UnitOfWork UnitOfWork = new UnitOfWork(new DataContext.DataContext());
            string retVal = null;
            try
            {
                //lấy thông tin phân công
                var lich = UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.KY == KY && o.THANG == THANG && o.NAM == NAM && o.MA_DVIQLY == MA_DVIQLY && o.MA_SOGCS == MA_SOGCS);
                if (lich == null) return "LỖI: Không tìm thấy sổ được phân công.";
                // Thừa quá ??? if (lich.FILE_XML == FileName) return "LỖI: Không có quyền thao tác với sổ này."; //không cho phép đẩy sổ lên nếu USERID ko được phân công
                // cấu hình save file trên server.
                //string FilePath = Path.Combine(Server.MapPath("~/TemplateFile/" + lich.MA_DVIQLY.Trim() + "/" + lich.FILE_XML));

                string FilePath = Utility.getXMLPath() + lich.MA_DVIQLY.Trim() + @"\" + lich.FILE_XML;
                ds.WriteXml(FilePath);
                //if (Offset == 0) // tạo flie mới, khởi tạo một file trống
                //File.Create(FilePath).Close();
                //// mở 1 file stream và ghi các the buffer.
                //using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                //{
                //    fs.Seek(0, SeekOrigin.Begin);
                //    fs.Write(buffer, 0, buffer.Length);
                //}
                try
                {
                    //DataSet ds = new DataSet();
                    //ds.ReadXml(FilePath);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in ds.Tables[0].Rows)
                        {
                            string maquyen = dataRow["MA_QUYEN"].ToString();
                            int ky = Convert.ToInt32(dataRow["KY"].ToString());
                            int thang = Convert.ToInt32(dataRow["THANG"].ToString());
                            int nam = Convert.ToInt32(dataRow["NAM"].ToString());

                            var gcsHhu = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetOne(x => x.MA_DVIQLY == MA_DVIQLY
                                                                    && x.MA_QUYEN == maquyen
                                                                    && x.KY == ky
                                                                    && x.THANG == thang
                                                                    && x.NAM == nam
                                                                    && x.STR_CHECK_DSOAT == "UNCHECK"
                                                                    );
                            if (gcsHhu != null)
                            {
                                /*
                                 *  <CS_CU>762</CS_CU>
                                    <TTR_CU/>
                                    <SL_CU>20</SL_CU>
                                    -- <SL_TTIEP>0</SL_TTIEP>
                                    <NGAY_CU>2018-11-01T00:00:00</NGAY_CU>
                                    -- <CS_MOI>0</CS_MOI>
                                    <TTR_MOI> </TTR_MOI>
                                    -- <SL_MOI>0</SL_MOI>
                                    <CHUOI_GIA> KT: 100%*1572-SXBT-A</CHUOI_GIA>
                                    <KY>1</KY>
                                    <THANG>11</THANG>
                                    <NAM>2018</NAM>
                                    <NGAY_MOI>2018-11-30T00:00:00</NGAY_MOI>
                                    <NGUOI_GCS>002 - Vũ Thị Vân</NGUOI_GCS>
                                    <SL_THAO>0</SL_THAO>
                                    <KIMUA_CSPK>0</KIMUA_CSPK>
                                    <MA_COT/>
                                    <SLUONG_1>20</SLUONG_1>
                                    <SLUONG_2>29</SLUONG_2>
                                    <SLUONG_3>17</SLUONG_3>
                                    <SO_HOM>1</SO_HOM>
                                    <GIA_TRI_1/>
                                    <GIA_TRI_2/>
                                    <GIA_TRI_3/>
                                    <PMAX>0</PMAX>
                                    <NGAY_PMAX>1900-01-01T00:00:00</NGAY_PMAX>
                                 */
                                if (dataRow["SL_TTIEP"] != null)
                                    gcsHhu.SL_TTIEP = Convert.ToInt32(dataRow["SL_TTIEP"]);
                                if (dataRow["CS_MOI"] != null)
                                    gcsHhu.CS_MOI = Convert.ToDecimal(dataRow["CS_MOI"]);
                                if (dataRow["SL_MOI"] != null)
                                    gcsHhu.SL_MOI = Convert.ToDecimal(dataRow["SL_MOI"]);


                                UnitOfWork.RepoBase<GCS_CHISO_HHU>().Update(gcsHhu);
                            }
                            //var m = System.Web.HttpContext.Current.Server.MapPath("~/");
                            //foreach (var lstImage in gcsHhu)
                            //{
                            //    if (string.IsNullOrEmpty(lstImage.ANH_GCS))
                            //    {
                            //        lstImage.ANH_GCS = m + @"Images\NoImage.png";
                            //        var path = lstImage.ANH_GCS;

                            //    }
                            //}
                        }
                    }
                }
                catch (Exception e)
                {

                }
                retVal = "Thành công.";
            }
            catch (Exception ex)
            {
                retVal = "ERROR: " + ex.Message;
            }
            return retVal;
        }
        #endregion

        #region các hàm hỗ trợ mở rộng service
        private DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (PropertyInfo prop in props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                tb.Columns.Add(prop.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
        private void WriteLog(string strLog)
        {
            string PathLogFile = Server.MapPath(@"MobileUploads\ServiceKetNoiMTB\\GCS-LOG.TXT");
            string date_time = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");

            using (StreamWriter file_log = new StreamWriter(PathLogFile, true))
            {
                try
                {
                    file_log.WriteLine(date_time + ": " + strLog);
                }
                catch
                {

                }
            }
        }
        //private DataSet ReadXML(string FileName)
        [WebMethod]
        public DataSet ReadXML(string FileName)
        {
            DataSet ds = new DataSet();
            #region định nghĩa bảng ERROR
            DataTable dtERROR = new DataTable();
            dtERROR.Columns.Add("ERROR");
            #endregion
            try
            {
                #region định nghĩa datatable dữ liệu cấu trúc file XML Table
                DataTable Table = new DataTable();
                Table.TableName = "Table";
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
                Table.Columns.Add("TTR_MOI");
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
                Table.Columns.Add("SLUONG_1");
                Table.Columns.Add("SLUONG_2");
                Table.Columns.Add("SLUONG_3");
                Table.Columns.Add("SO_HOM");
                Table.Columns.Add("PMAX");
                Table.Columns.Add("NGAY_PMAX");
                Table.Columns.Add("ID");
                Table.Columns.Add("GHICHU");
                Table.Columns.Add("TT_KHAC");
                Table.Columns.Add("CGPVTHD");
                Table.Columns.Add("HTHUC_TBAO_DK");
                Table.Columns.Add("DTHOAI_SMS");
                Table.Columns.Add("EMAIL");
                Table.Columns.Add("THOI_GIAN");
                Table.Columns.Add("X");
                Table.Columns.Add("Y");
                Table.Columns.Add("SO_TIEN");
                Table.Columns.Add("HTHUC_TBAO_TH");
                Table.Columns.Add("TENKHANG_RUTGON");
                Table.Columns.Add("TTHAI_DBO");
                Table.Columns.Add("DU_PHONG");
                Table.Columns.Add("TEN_FILE");
                Table.Columns.Add("STR_CHECK_DSOAT");
                #endregion

                string pathServer = Server.MapPath("~/");
                int str_length = pathServer.Length - @"\WSGCS".Length;
                string path = pathServer.Substring(0, str_length);
                //path += @"TemplateFile\" + UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o=>o.FILE_XML == FileName).MA_DVIQLY.Trim() +@"\" + FileName;
                path = Utility.getXMLPath() + UnitOfWork.RepoBase<GCS_LICHGCS>().GetOne(o => o.FILE_XML.Contains(FileName)).MA_DVIQLY.Trim() + @"\" + FileName;
                if (File.Exists(path) == false)
                {
                    DataRow row = dtERROR.NewRow();
                    row["ERROR"] = "LỖI: Không tìm thấy tệp.";
                    dtERROR.Rows.Add(row);
                    ds.Tables.Add(dtERROR);
                    return ds;
                }
                #region đọc file xml và lưu vào dataset
                XmlDocument fileXML = new XmlDocument();
                fileXML.Load(path);
                XmlNodeList nodeList = fileXML.DocumentElement.SelectNodes("/NewDataSet/Table1");
                var tableXML = Table.Clone();
                foreach (XmlNode node in nodeList)
                {
                    DataRow row = tableXML.NewRow();
                    #region gán giá trị của xml sang datatable
                    row["MA_NVGCS"] = node.SelectSingleNode("MA_NVGCS").InnerText;
                    row["MA_KHANG"] = node.SelectSingleNode("MA_KHANG").InnerText;
                    row["MA_DDO"] = node.SelectSingleNode("MA_DDO").InnerText;
                    row["MA_DVIQLY"] = node.SelectSingleNode("MA_DVIQLY").InnerText;
                    row["MA_GC"] = node.SelectSingleNode("MA_GC").InnerText;
                    row["MA_QUYEN"] = node.SelectSingleNode("MA_QUYEN").InnerText;
                    row["MA_TRAM"] = node.SelectSingleNode("MA_TRAM").InnerText;
                    row["BOCSO_ID"] = node.SelectSingleNode("BOCSO_ID").InnerText;
                    row["LOAI_BCS"] = node.SelectSingleNode("LOAI_BCS").InnerText;
                    row["LOAI_CS"] = node.SelectSingleNode("LOAI_CS").InnerText;
                    row["TEN_KHANG"] = node.SelectSingleNode("TEN_KHANG").InnerText;
                    row["DIA_CHI"] = node.SelectSingleNode("DIA_CHI").InnerText;
                    row["MA_NN"] = node.SelectSingleNode("MA_NN").InnerText;
                    row["SO_HO"] = node.SelectSingleNode("SO_HO").InnerText;
                    row["MA_CTO"] = node.SelectSingleNode("MA_CTO").InnerText;
                    row["SERY_CTO"] = node.SelectSingleNode("SERY_CTO").InnerText;
                    row["HSN"] = node.SelectSingleNode("HSN").InnerText;
                    row["CS_CU"] = node.SelectSingleNode("CS_CU").InnerText;
                    row["TTR_CU"] = node.SelectSingleNode("TTR_CU").InnerText;
                    row["SL_CU"] = node.SelectSingleNode("SL_CU").InnerText;
                    row["SL_TTIEP"] = node.SelectSingleNode("SL_TTIEP").InnerText;
                    row["NGAY_CU"] = node.SelectSingleNode("NGAY_CU").InnerText;
                    row["CS_MOI"] = node.SelectSingleNode("CS_MOI").InnerText;
                    //row["TTR_MOI"] = node.SelectSingleNode("TTR_MOI").InnerText;
                    row["SL_MOI"] = node.SelectSingleNode("SL_MOI").InnerText;
                    row["CHUOI_GIA"] = node.SelectSingleNode("CHUOI_GIA").InnerText;
                    row["KY"] = node.SelectSingleNode("KY").InnerText;
                    row["THANG"] = node.SelectSingleNode("THANG").InnerText;
                    row["NAM"] = node.SelectSingleNode("NAM").InnerText;
                    row["NGAY_MOI"] = node.SelectSingleNode("NGAY_MOI").InnerText;
                    row["NGUOI_GCS"] = node.SelectSingleNode("NGUOI_GCS").InnerText;
                    row["SL_THAO"] = node.SelectSingleNode("SL_THAO").InnerText;
                    row["KIMUA_CSPK"] = node.SelectSingleNode("KIMUA_CSPK").InnerText;
                    //row["MA_COT"] = node.SelectSingleNode("MA_COT").InnerText;
                    row["SLUONG_1"] = node.SelectSingleNode("SLUONG_1").InnerText;
                    row["SLUONG_2"] = node.SelectSingleNode("SLUONG_2").InnerText;
                    row["SLUONG_3"] = node.SelectSingleNode("SLUONG_3").InnerText;
                    if (node.SelectSingleNode("SO_HOM") != null) row["SO_HOM"] = node.SelectSingleNode("SO_HOM").InnerText; else row["SO_HOM"] = 0;
                    row["PMAX"] = node.SelectSingleNode("PMAX").InnerText;
                    row["NGAY_PMAX"] = node.SelectSingleNode("NGAY_PMAX").InnerText;
                    //row["ID"] = node.SelectSingleNode("ID").InnerText;
                    //row["GHICHU"] = node.SelectSingleNode("GHICHU").InnerText;
                    //row["TT_KHAC"] = node.SelectSingleNode("TT_KHAC").InnerText;
                    //row["CGPVTHD"] = node.SelectSingleNode("CGPVTHD").InnerText;
                    //row["HTHUC_TBAO_DK"] = node.SelectSingleNode("HTHUC_TBAO_DK").InnerText;
                    //row["DTHOAI_SMS"] = node.SelectSingleNode("DTHOAI_SMS").InnerText;
                    //row["EMAIL"] = node.SelectSingleNode("EMAIL").InnerText;
                    //row["THOI_GIAN"] = node.SelectSingleNode("THOI_GIAN").InnerText;
                    //row["X"] = node.SelectSingleNode("X").InnerText;
                    //row["Y"] = node.SelectSingleNode("Y").InnerText;
                    //row["SO_TIEN"] = node.SelectSingleNode("SO_TIEN").InnerText;
                    //row["HTHUC_TBAO_TH"] = node.SelectSingleNode("HTHUC_TBAO_TH").InnerText;
                    //row["TENKHANG_RUTGON"] = node.SelectSingleNode("TENKHANG_RUTGON").InnerText;
                    //row["TTHAI_DBO"] = node.SelectSingleNode("TTHAI_DBO").InnerText;
                    //row["DU_PHONG"] = node.SelectSingleNode("DU_PHONG").InnerText;
                    //row["TEN_FILE"] = node.SelectSingleNode("TEN_FILE").InnerText;
                    //row["STR_CHECK_DSOAT"] = node.SelectSingleNode("STR_CHECK_DSOAT").InnerText;
                    #endregion
                    tableXML.Rows.Add(row);
                }
                ds.Tables.Add(tableXML);
            }
            catch (Exception ex)
            {
                DataRow row = dtERROR.NewRow();
                row["ERROR"] = "LỖI: " + ex.Message;
                dtERROR.Rows.Add(row);
                ds.Tables.Add(dtERROR);
                return ds;
            }
            #endregion
            return ds;
        }

        private void CheckPath(string path) //kiểm tra thư mục, chưa có thì tạo
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private static void CustomSoapException(string exceptionName, string message)
        {
            throw new System.Web.Services.Protocols.SoapException(exceptionName + ": " + message, new System.Xml.XmlQualifiedName("BufferedUpload"));
        }
        #endregion
    }
}
