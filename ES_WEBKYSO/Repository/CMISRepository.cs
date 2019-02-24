using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ES_WEBKYSO.Repository
{

    public class CmisRepository
    {
        public string strURL_CMISInterface;
        //private Service_TonThat.Service_TonThat _serviceTonThat;
        public CmisRepository()
        {
            //Service_TonThat.AuthenticateHeader authentication_td = new Service_TonThat.AuthenticateHeader();
            //authentication_td.strKeyAuthenticate = new CMIS2_Encript.cls_Encrypt_Decrypt().Encrypt("Service_TonThat");
            //_serviceTonThat = new Service_TonThat.Service_TonThat();
            //_serviceTonThat.AuthenticateHeaderValue = authentication_td;
            //_serviceTonThat.Url = Properties.Settings.Default.ES_WEBKYSO_Service_TonThat_Service_TonThat;
        }

        /// <summary>
        /// Hàm đồng bộ sổ
        /// </summary>
        /// <param name="maDvQly"></param>
        /// <returns></returns>
        /// <exception cref="Nullable"></exception>
        public DataSet GetSoFromCmis(string maDvQly)
        {
            var ds = new DataSet(); // _serviceTonThat.ExecuteQuery(query);
            dynamic product = new JObject();
            product.MA_DVIQLY = maDvQly;
            product.TEN_DANH_MUC = "D_SOGCS";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL_CMISInterface);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(product.ToString());

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            try
            {

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    var response = responseReader.ReadToEnd();
                    dynamic dynObj = JsonConvert.DeserializeObject(response);
                    string objSoGCS = dynObj.LST_OBJ.ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(objSoGCS, (typeof(DataTable)));
                    ds.Tables.Add(dt);
                    Console.Out.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
            }

            var query = "select MA_DVIQLY, MA_SOGCS, TEN_SOGCS, SO_KY, LOAI_SOGCS, NGAY_GHI,TINH_TRANG, MA_TO  from d_sogcs where ma_dviqly = '" + maDvQly + "' and tinh_trang=1 union  select MA_DVIQLY, MA_SOGCS, TEN_SOGCS, SO_KY, 'DN' LOAI_SOGCS, NGAY_GHI,TINH_TRANG, MA_TO from tt_sogcs where ma_dviqly = '" + maDvQly + "' and tinh_trang=1";

            return ds;
        }
        /// <summary>
        /// Hàm lấy sổ đang xuất dữ liệu chờ đi ghi từ CMIS
        /// </summary>
        /// <param name="maDvQly"></param>
        /// <returns></returns>
        public DataSet GetSoXuatHHC(string maDvQly, string pthang, string pnam)
        {
            try
            {
                var query = "select s.MA_DVIQLY, s.MA_SOGCS, s.KY, s.THANG, s.NAM, s.NGAY_XUAT, s.NGAY_NHAN, s.SODDO_XUAT, s.SODDO_NHAN from GCS_HHC_SERVICE s, gcs_lichgcs l where s.ma_dviqly = l.ma_dviqly and s.ma_sogcs = l.ma_sogcs and l.trang_thai = 'HHCX'and s.ky = l.ky and s.thang = l.thang and s.nam = l.nam " + "and s.ma_dviqly='" + maDvQly + "' and s.thang=" + pthang  + " and s.nam=" + pnam + " union select s.MA_DVIQLY, s.MA_SOGCS, s.KY, s.THANG, s.NAM, s.NGAY_XUAT, s.NGAY_NHAN, s.SODDO_XUAT, s.SODDO_NHAN from TT_HHC_SERVICE s where ngay_nhan is null " + "and s.ma_dviqly = '" + maDvQly + "' and s.thang=" + pthang + " and s.nam=" + pnam;
                var ds = new DataSet();
                return ds;
            }
            catch (Exception e)
            {
                 return null;
            }
        }
    }
}