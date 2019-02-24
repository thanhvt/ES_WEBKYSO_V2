using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace ES_WEBKYSO.Common
{
    public class SendMail
    {

        #region gửi mail
        public void Send_Email(string dsMailNhan, string Noidung, string TieuDe, string strFileName)
        {
            try
            {
                string kq = "";
                string dchiMailGui = "";

                var UserEmailFrom = WebConfigurationManager.AppSettings["WebMail.UserName"]; //địa chỉ email gửi
                var PassEmailFrom = WebConfigurationManager.AppSettings["WebMail.Pass"]; //pass email gửi


                if (dsMailNhan == "") return;
                kq = RemoveSignString(RemoveEmailError(dsMailNhan));

                if (!string.IsNullOrEmpty(kq))
                {
                    //MailMessage em = new MailMessage(UserEmailFrom, kq, TieuDe, Noidung);
                    //if (!string.IsNullOrEmpty(strFileName))
                    //{
                    //    em.Attachments.Add(new Attachment(strFileName));
                    //}
                    //em.Bcc.Add(UserEmailFrom);
                    //em.BodyEncoding = Encoding.UTF8;
                    //em.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    NetworkCredential auth = new NetworkCredential(UserEmailFrom, PassEmailFrom);
                    smtp.Host = "smtp.gmail.com";
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = auth;

                    Thread T1 = new Thread(delegate ()
                    {
                        MailMessage em = new MailMessage(UserEmailFrom, kq, TieuDe, Noidung);
                        if (!string.IsNullOrEmpty(strFileName))
                        {
                            em.Attachments.Add(new Attachment(strFileName));
                        }
                        em.BodyEncoding = Encoding.UTF8;
                        em.IsBodyHtml = true;
                        smtp.Send(em);
                    });

                    T1.Start();
                    //Thread th = new Thread(new ThreadStart())
                    //smtp.Send(em);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void Send_Email_Cc(string dsMailNhan, string ccDoiTruong, string Noidung, string TieuDe, string strFileName)
        {
            try
            {
                string kq = "";
                string dchiMailGui = "";
                
                var UserEmailFrom = WebConfigurationManager.AppSettings["WebMail.UserName"]; //địa chỉ email gửi
                var PassEmailFrom = WebConfigurationManager.AppSettings["WebMail.Pass"]; //pass email gửi


                if (dsMailNhan == "") return;
                kq = RemoveSignString(RemoveEmailError(dsMailNhan));

                if (!string.IsNullOrEmpty(kq))
                {
                    //MailMessage em = new MailMessage(UserEmailFrom, kq, TieuDe, Noidung);
                    //if (!string.IsNullOrEmpty(strFileName))
                    //{
                    //    em.Attachments.Add(new Attachment(strFileName));
                    //}
                    //em.Bcc.Add(UserEmailFrom);
                    //em.BodyEncoding = Encoding.UTF8;
                    //em.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    NetworkCredential auth = new NetworkCredential(UserEmailFrom, PassEmailFrom);
                    smtp.Host = "smtp.gmail.com";
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = auth;

                    Thread T1 = new Thread(delegate ()
                    {
                        MailMessage em = new MailMessage(UserEmailFrom, kq, TieuDe, Noidung);
                        if (!string.IsNullOrEmpty(strFileName))
                        {
                            em.Attachments.Add(new Attachment(strFileName));
                        }
                        em.CC.Add(ccDoiTruong); //tự động CC cho doi truong
                        em.BodyEncoding = Encoding.UTF8;
                        em.IsBodyHtml = true;
                        smtp.Send(em);
                    });

                    T1.Start();
                    //Thread th = new Thread(new ThreadStart())
                    //smtp.Send(em);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        //Xây dựng hàm loại bỏ thay thế dấu ";" thừa
        private string RemoveSignString(string str)
        {
            str = str.Replace(";", ",");
            while (str.ToString().IndexOf(" ") != -1)
            {
                str = str.Replace(" ", "");
            }
            while (str.ToString().IndexOf(",,") != -1)
            {
                str = str.Replace(",,", ",");
            }

            //Loại bỏ 2 dấu "," đầu và cuối.
            if (str.Substring(0, 1) == ",")
            {
                str = str.Substring(1, str.Length - 1).ToString();
            }

            if (str.Length > 1 && str.Substring(str.Length - 1, 1) == ",")
            {
                str = str.Substring(0, str.Length - 1).ToString();
            }
            return str.Trim();
        }

        // hàm bỏ những email không đúng định dạng
        private string RemoveEmailError(string dsMail)
        {
            string[] _arrMail = dsMail.Split(';');
            string strMail = "";
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (string mail in _arrMail)
            {
                Match match = regex.Match(mail.Trim());
                if (match.Success)
                {
                    strMail += mail + ",";
                }
            }
            return strMail;
        }

        #endregion
    }
}