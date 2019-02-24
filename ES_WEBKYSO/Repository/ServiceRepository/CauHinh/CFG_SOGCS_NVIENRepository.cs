using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class CFG_SOGCS_NVIENRepository<T> : BaseRepository<CFG_SOGCS_NVIEN>
    {
        public CFG_SOGCS_NVIENRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {
        }
        public override List<CFG_SOGCS_NVIEN> GETALL()
        {
            try
            {
                var ret = GetAll().ToList();
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override List<CFG_SOGCS_NVIEN> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
        {
            if (orderKey.EndsWith("String"))
            {
                orderKey = orderKey.Replace("String", "");
                page.OrderKey = orderKey;
            }

            var pagingOrg = page;

            var qString = pagingOrg.Key;

            int qInt;
            int? qIntSearch = null;
            if (int.TryParse(qString, out qInt))
            {
                qIntSearch = qInt;
            }
            
            var ret = GetAll(o =>
                   (qString == null || qString == "" || o.MA_SOGCS.Equals(qString) || o.MA_DOIGCS.Equals(qString)
                   && (findModel.MaSo == null || o.MA_SOGCS == findModel.MaSo)
                   && (findModel.USERID == null || o.USERID == findModel.USERID)
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new CFG_SOGCS_NVIEN
                {
                    MA_SOGCS_NVIEN = x.MA_SOGCS_NVIEN,
                    MA_DOIGCS = x.MA_DOIGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    USERID = x.USERID
                })
                .ToList();

            return ret;
        }
        public override List<CFG_SOGCS_NVIEN> ManagerGetAllForIndex(string orderKey, ref Paging page)
        {
            if (orderKey.EndsWith("String"))
            {
                orderKey = orderKey.Replace("String", "");
                page.OrderKey = orderKey;
            }


            var pagingOrg = page;

            var qString = pagingOrg.Key;

            //string qstring;
            string qStringSearch = null;
            //if (int.TryParse(qString, out qstring))
            //{
            qStringSearch = qString;
            //}

            DateTime qDateTime;
            DateTime? qDateTimeSearch = null;
            if (DateTime.TryParse(qString, out qDateTime))
            {
                qDateTimeSearch = qDateTime;
            }
            var ret = GetAll(o =>
                    (qStringSearch == null || qStringSearch == o.MA_DOIGCS || qStringSearch == o.MA_SOGCS)
                        &&
                    (qString == null || qString == "" || o.MA_SOGCS.Contains(qString) || o.MA_SOGCS.Contains(qString))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new CFG_SOGCS_NVIEN()
                {
                    MA_SOGCS_NVIEN = x.MA_SOGCS_NVIEN,
                    MA_DOIGCS = x.MA_DOIGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    USERID = x.USERID
                })
                .ToList();
            //var ret = new List<SOGCS_NVIEN>();
            return ret;
        }
    }
}