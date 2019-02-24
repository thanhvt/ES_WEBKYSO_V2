using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ES_WEBKYSO.ModelParameter;
using Common.Helpers;

namespace ES_WEBKYSO.Repository.ServiceRepository.QuanTriHeThong
{
    public class CFG_DOIGCS_NVIENRepository<T> : BaseRepository<CFG_DOIGCS_NVIEN>
    {
        public CFG_DOIGCS_NVIENRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {
        }
        public override List<CFG_DOIGCS_NVIEN> GETALL()
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
        public override List<CFG_DOIGCS_NVIEN> ManagerGetAllForIndex(FindModelGcs findModel, string searchString)
        {
            var ret = new List<CFG_DOIGCS_NVIEN>();
            ret = GetAll(o => (findModel.MaDoi == o.MA_DOIGCS || searchString == o.MA_DOIGCS))
                .ToList()
                .Select(x => new CFG_DOIGCS_NVIEN
                {
                    ID = x.ID,
                    MA_DOIGCS = x.MA_DOIGCS,
                    USERID = x.USERID
                })
                .ToList();
            return ret;
        }
        public override List<CFG_DOIGCS_NVIEN> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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
                   (qString == null || qString == ""
                   &&(findModel.MaDoi == null || o.MA_DOIGCS == findModel.MaDoi)
                   && (findModel.USERID == null || o.USERID == findModel.USERID)
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new CFG_DOIGCS_NVIEN
                {
                    ID = x.ID,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_DOIGCS = x.MA_DOIGCS,
                    USERID = x.USERID
                })
                .ToList();

            return ret;
        }
        //public override List<DM_DOIGCS> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
        //{

        //    if (orderKey.EndsWith("String"))
        //    {
        //        orderKey = orderKey.Replace("String", "");
        //        page.OrderKey = orderKey;
        //    }


        //    var pagingOrg = page;

        //    var qString = pagingOrg.Key;

        //    int qInt;
        //    int? qIntSearch = null;
        //    if (int.TryParse(qString, out qInt))
        //    {
        //        qIntSearch = qInt;
        //    }

        //    DateTime qDateTime;
        //    DateTime? qDateTimeSearch = null;
        //    if (DateTime.TryParse(qString, out qDateTime))
        //    {
        //        qDateTimeSearch = qDateTime;
        //    }

        //    // qString, qInt, qDateTime

        //    //var ret = GetAll(o =>
        //    //       (qString == null || qString == ""
        //    //       && (findModel.MaDoi == null || o.MA_DOIGCS == findModel.MaDoi)
        //    //       && (findModel.TenDoi == null || o.TEN_DOI == findModel.TenDoi))

        //    //    , page.OrderKey, ref page)
        //    //    .ToList()
        //    //    .Select(x => new SOGCS_DOI
        //    //    {
        //    //        MA_DOIGCS = x.MA_DOIGCS,
        //    //        MA_DVIQLY = x.MA_DVIQLY,
        //    //        TEN_DOI = x.TEN_DOI,
        //    //        GHI_CHU = x.GHI_CHU
        //    //    })
        //    //    .ToList();
        //    var ret = new List<DM_DOIGCS>();
        //    return ret;
        //}
    }
}