using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.Models;
using Ionic.Zip;

namespace ES_WEBKYSO.Areas.HeThongGiaoTiep.Controllers
{
    public class ChuyenDuLieuController : BaseController
    {
        //
        // GET: /HeThongGiaoTiep/ChuyenDuLieu/
        [HttpGet]
        public ActionResult Index()
        {


            //var ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"];
            //var ftpUserID = ConfigurationManager.AppSettings["ftpUserID"];
            //var ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            //UploadFileToFtp();
            return View();
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

                using (var zip = new ZipFile())
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
        public List<string> DirectoryListing(string folder, string user, string pass)
        {
            var _remoteHost = "ftp://117.0.37.184/";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + folder);

            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(user, pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            List<string> result = new List<string>();

            while (!reader.EndOfStream)

            {

                result.Add(reader.ReadLine());

            }

            reader.Close();

            response.Close();

            return result;

        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            string maDviQly = new CommonUserProfile().MA_DVIQLY;
            var mapPath = System.Web.HttpContext.Current.Server.MapPath("~/");
            int idx = mapPath.LastIndexOf('\\');
            var directoryFolder = mapPath.Substring(0, idx - 11);
            //var directoryFolder1 = mapPath.Substring(1, idx);
            if (username == "" && password == "")
            {
                TempData["MessageStatus"] = false;
                TempData["Error"] = "Vui lòng nhập tên tài khoản, mật khẩu FTP";
            }
            else
            {
                string wsgcsFolder1 = directoryFolder + "\\WSGCS\\GCSImages";

                try
                {
                    foreach (string d in Directory.GetDirectories(wsgcsFolder1))
                    {
                        foreach (string e in Directory.GetDirectories(d))
                        {
                            string zip = e + ".zip";
                            CompressionFolder(e, zip);

                            var configInput = Uow.RepoBase<ConfigInput>().GetOne(x => x.TypeInput == "FTP");
                            var ftpServerIp = configInput.Value;

                            string filePath = zip;
                            //int z = filePath.LastIndexOf(@"\") + 1;
                            //int y = filePath.LastIndexOf(@".zip");
                            //string folderName = filePath.Substring(z, y - z);

                            string filename = Path.GetFileName(filePath);
                            string ftpfullpath = ftpServerIp;

                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpfullpath + "/" + maDviQly);
                            request.Method = WebRequestMethods.Ftp.MakeDirectory;

                            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath + "/" + maDviQly + "_" + filename);
                            ftp.Credentials = new NetworkCredential(username, password);

                            ftp.KeepAlive = true;
                            ftp.UseBinary = true;
                            ftp.Method = WebRequestMethods.Ftp.UploadFile;

                            FileStream fs = System.IO.File.OpenRead(filePath);
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            fs.Close();

                            Stream ftpstream = ftp.GetRequestStream();
                            ftpstream.Write(buffer, 0, buffer.Length);
                            ftpstream.Close();

                            TempData["MessageStatus"] = true;
                            TempData["Success"] = "Chuyển dữ liệu thành công";
                        }
                    }
                }
                catch (Exception excpt)
                {
                    TempData["MessageStatus"] = false;
                    TempData["Error"] = "Chuyển dữ liệu không thành công " + excpt.Message;
                }
            }
           
            //return RedirectToAction("Index", "CauHinhNvGcsMacDinh");
            return View();
        }

    }
}
