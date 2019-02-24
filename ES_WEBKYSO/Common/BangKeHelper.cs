using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.Repository;

namespace ES_WEBKYSO.Common
{
    public class BangKeHelper : Controller
    {
        #region private properties

        private readonly UnitOfWork UnitOfWork;

        #endregion

        public BangKeHelper(UnitOfWork repo)
        {
            UnitOfWork = repo;
        }

        public DataSet ReadXmLso(int idLichGcs)
        {
            //1.getXML

            //lấy thang ky nam, maso theo idlich
            var gcsLichgcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll(i => i.ID_LICHGCS == idLichGcs).FirstOrDefault();
            if (gcsLichgcs == null)
                return null;

            //var mapPath = HostingEnvironment.MapPath("~/TemplateFile/" + gcsLichgcs.MA_DVIQLY.Trim() + @"/" + gcsLichgcs.FILE_XML);
            var mapPath = Utility.getXMLPath() + gcsLichgcs.MA_DVIQLY.Trim() + @"/" + gcsLichgcs.FILE_XML;
            DataSet dsSo = new DataSet();
            try
            {
                dsSo.ReadXml(mapPath);
            }
            catch (Exception ex)
            {
                //todo: log ex
                return null;
            }
            return dsSo;
        }
        
        public object GetBangKeChiSo(int idLichGcs)
        {
            DataSet so = ReadXmLso(idLichGcs);
            //todo: xử lý lấy dữ liệu bảng kê
            return so;
        }

        public object GetBangKeTonThat(int idLichGcs)
        {
            DataSet so = ReadXmLso(idLichGcs);
            //todo: xử lý lấy dữ liệu bảng kê
            return (object)so;
        }
    }
}
