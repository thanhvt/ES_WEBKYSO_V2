using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using esDigitalSignature;
using esDigitalSignature.Library;
using ES_WEBKYSO.Common.CA;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;

namespace ES_WEBKYSO
{
    /// <summary>
    /// Summary description for WebKySoCA
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebKySoCA : System.Web.Services.WebService
    {
        //public readonly UnitOfWork _uow = new UnitOfWork(new DataContext.DataContext());
        public readonly UnitOfWork _uow;
        public WebKySoCA()
        {
            DataContext.DataContext context = new DataContext.DataContext();
            _uow = new UnitOfWork(context);
        }
        
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private string GetDatabaseString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        #region Đăng nhập Webservice
        [WebMethod(Description = "Đăng nhập Webservice")]
        public bool LogIn(string username, string password)
        {
            try
            {
                DateTime defaultTime = new DateTime(2012, 1, 1);
                DateTime nowTime = DateTime.Now;
                TimeSpan ts = nowTime - defaultTime;
                double iTime = ts.TotalSeconds;

                ES_Encrypt enc = new ES_Encrypt();
                double iTimePass = 0;
                if (Double.TryParse(enc.DecryptString(password), out iTimePass))
                {
                    if (Math.Abs(iTimePass - iTime) < 300)
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [WebMethod(Description = "Đăng xuất Webservice")]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public bool LogOut(string key)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();
            try
            {
                DAL_SqlConnector.ConnectionString = GetDatabaseString();
                BUSQuanTri bus = new BUSQuanTri();
                bus.CA_DataSign_CA_DataSignForDB_DeleteByKey(key);
            }
            catch
            {
                // ignored
            }
            return true;
        }
        #endregion

        [WebMethod(Description = "Hàm lấy thông tin để ký")]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public bool GetInfoToSign(string keySign, ref string strErr, out DataTable dtResult)
        {
            //Xóa dữ liệu bảng đầu vào
            dtResult = new DataTable("RESULTS");


            var cads = _uow.RepoBase<CA_DataSign>().GetAll(i => i.KeySign == keySign);

            if (!cads.Any())
            {
                strErr = "không có CA_DataSign tương ứng!";
                return false;
            }

            var files = _uow.RepoBase<FL_FILE>().GetAll().Where(i => cads.Any(o => o.FileID == i.FileID));
            if (!files.Any())
            {
                strErr = "không có FL_File tương ứng!";
                return false;
            }

            dtResult.Columns.Add("FilePath", typeof(string));
            dtResult.Columns.Add("FileData", typeof(byte[]));
            dtResult.Columns.Add("OKtoSign", typeof(bool));
            dtResult.Columns.Add("ExtensionFile", typeof(string));

            foreach (var file in files)
            {
                DataRow drResult = dtResult.NewRow();
                drResult["OKtoSign"] = true;
                drResult["FileData"] = File.ReadAllBytes(Server.MapPath(file.FilePath));
                drResult["FilePath"] = file.FilePath;
                drResult["ExtensionFile"] = Path.GetExtension(file.FilePath);
                dtResult.Rows.Add(drResult);
            }
            return true;
        }


        [WebMethod(Description = "Xác thực các văn bản ký đã có FileID trong hệ thống và cập nhật cơ sở dữ liệu")]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public bool SaveFile(DataTable dtInFo, string keySign, ref DataTable dtResult, ref string strError)
        {

            bool isSuccess = true;
            strError = "";
            //Tạo bảng Result
            dtResult = new DataTable("RESULT");
            dtResult.Columns.Add("FileID", typeof(int));
            dtResult.Columns.Add("FileName", typeof(string));
            dtResult.Columns.Add("SignResults", typeof(int));
            dtResult.Columns.Add("SignDetails", typeof(string));

            if (dtInFo.Rows.Count == 0)
            {
                strError = "dữ liệu dtInfo không hợp lệ!";
                return false;
            }
            var cads = _uow.RepoBase<CA_DataSign>().GetAll().Where(i => i.KeySign == keySign);
            if (!cads.Any())
            {
                strError = "không có CA_DataSign tương ứng!";
                return false;
            }

            var fileInfos = _uow.RepoBase<FL_FILE>().GetAll().Where(i => cads.Any(o => o.FileID == i.FileID));
            if (!fileInfos.Any())
            {
                strError = "không có FL_File tương ứng!";
                return false;
            }

            foreach (var fileInfo in fileInfos)
            {
                if (!File.Exists(Server.MapPath(fileInfo.FilePath)))
                {
                    strError = "File không tồn tại trên hệ thống";
                    return false;
                }
            }

            //var cmu =  db.CMIS_User.FirstOrDefault(i => i.UserName == cads.FirstOrDefault().UserSign);
            //if (cmu == null)
            //{
            //    strError = "không có user trong hệ thống!";
            //    return false;
            //}

            //duyệt từng file - trả kết quả theo dạng bảng
            for (int i = 0; i < dtInFo.Rows.Count; i++)
            {
                var fileInfo = fileInfos.ToArray()[i];
                DataRow drResult = dtResult.NewRow();
                drResult["FileID"] = fileInfo.FileID;
                drResult["FileName"] = Path.GetFileName(Server.MapPath(fileInfo.FilePath));
                dtResult.Rows.Add(drResult);
                var fileData = (byte[])dtInFo.Rows[i]["FileData"];
                using (ESDigitalSignatureManager dsm = new ESDigitalSignatureManager(fileData, Path.GetExtension(fileInfo.FilePath)))
                {
                    //01.validate file có bị thay đổi không. (hash)
                    if (!dsm.GetHashValue().SequenceEqual(fileInfo.FileHash))
                    {
                        dtResult.Rows[i]["SignResults"] = (int)FileSignResults.HashNotMatch;
                        dtResult.Rows[i]["SignDetails"] = "Nội dung văn bản đã bị thay đổi.";
                        isSuccess = false;
                        continue;
                    }

                    //02.verify tính toàn vẹn -thời gian ký
                    if (dsm.Signatures.Count < 1)
                    {
                        dtResult.Rows[i]["SignResults"] = (int)FileSignResults.NotSigned;
                        dtResult.Rows[i]["SignDetails"] = "Không tìm thấy chữ ký";
                        isSuccess = false;
                        continue;
                    }

                    ESignature signature = dsm.Signatures[dsm.Signatures.Count - 1];
                    if (signature.Verify != VerifyResult.Success)
                    {
                        dtResult.Rows[i]["SignResults"] = (int)FileSignResults.InvalidSignature;
                        dtResult.Rows[i]["SignDetails"] = "Chữ ký không hợp lệ: " + signature.Verify;
                        isSuccess = false;
                        continue;
                    }

                    ////03.validate usb ký có map với đúng vai trò không.
                    //if (cmu.CA_Serial != signature.Signer.SerialNumber)
                    //{
                    //    dtResult.Rows[i]["SignResults"] = (int)FileSignResults.InvalidSignature;
                    //    dtResult.Rows[i]["SignDetails"] = "Chữ ký không hợp lệ, kiểm tra cấu hình người dùng - chữ ký";
                    //    isSuccess = false;
                    //    continue;
                    //}
                }

                //04. lưu file
                using (FileStream fs = new FileStream(Server.MapPath(fileInfo.FilePath), FileMode.Create))
                {
                    fs.Write(fileData, 0, fileData.Length);
                }
                dtResult.Rows[i]["SignResults"] = (int)FileSignResults.Success;
                dtResult.Rows[i]["SignDetails"] = "Ký thành công";

                //05. call hàm sau khi lưu file thành công
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(cads.FirstOrDefault().MethodName);
                object[] arguments = { keySign, strError };
                theMethod.Invoke(this, arguments);
                //ở client sẽ gọi hàm logout để xóa CAD
            }
            return isSuccess;
        }

        //[WebMethod(Description = "Xác thực các văn bản ký được upload lên hệ thống và cập nhật cơ sở dữ liệu")]
        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public bool CreateAndSaveFileInServer(DataTable dtInFo, string keySign, ref DataTable dtResult,
            ref string strError)
        {
            return false;
        }

        public bool NhanVienKyBangKe_Oncompleted(string keySign, ref string errorMessage)
        {
            //insert bảng chit
            var cads = _uow.RepoBase<CA_DataSign>().GetAll(i => i.KeySign == keySign);
            if (!cads.Any())
            {
                errorMessage = "không có CA_DataSign tương ứng!";
                return false;
            }

            var files = _uow.RepoBase<FL_FILE>().GetAll().Where(i => cads.Any(o => o.FileID == i.FileID));
            if (!files.Any())
            {
                errorMessage = "không có FL_File tương ứng!";
                return false;
            }

            var cad = cads.FirstOrDefault();
            var user = _uow.RepoBase<UserProfile>().GetOne(i => i.UserName == cad.UserSign);

            var file = files.FirstOrDefault();
            
            var gcsChitietKy = new GCS_BANGKE_LICH_CHITIET_KY();
            gcsChitietKy.NGUOI_KY = user.UserName;
            gcsChitietKy.UserId = user.UserId;
            gcsChitietKy.NGAY_KY = DateTime.Now;
            gcsChitietKy.MA_BANGKELICH = file.MA_BANGKELICH;
            
            _uow.RepoBase<GCS_BANGKE_LICH_CHITIET_KY>().Create(gcsChitietKy);

            return true;
        }
    }
}
