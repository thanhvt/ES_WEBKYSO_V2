using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ES_WEBKYSO.Models;
using ES_WEBKYSO.ModelParameter;
using Common.Helpers;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class CFG_BANGKE_DONVIRepository<T> : BaseRepository<CFG_BANGKE_DONVI>
    {
        public CFG_BANGKE_DONVIRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {
        }
        //public override List<LOAI_BANGKE_DONVI> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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

        //    var ret = GetAll(o =>
        //           ((qString == null || qString == "" || o.MA_LOAIBANGKE.Contains(qString) || o.MA_DVIQLY.Contains(qString) || o.HasD_LOAI_BANGKE.TEN_LOAIBANGKE.Contains(qString))
        //           && (findModel.MaBangKe == null || o.MA_LOAIBANGKE == findModel.MaBangKe)
        //           && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
        //           && (findModel.TenBangKe == null || o.HasD_LOAI_BANGKE.TEN_LOAIBANGKE == findModel.TenBangKe))
        //        , "MA_LOAIBANGKE", ref page)
        //        .ToList()
        //        .Select(x => new LOAI_BANGKE_DONVI()
        //        {
        //            ID = x.ID,
        //            MA_LOAIBANGKE = x.MA_LOAIBANGKE,
        //            MA_DVIQLY = x.MA_DVIQLY,
        //            GHI_CHU = x.GHI_CHU,
        //            Ten_LoaiBangKe = x.HasD_LOAI_BANGKE == null ? "" : x.HasD_LOAI_BANGKE.TEN_LOAIBANGKE
        //        })
        //        .ToList();

        //    return ret;
        //}

        public override List<CFG_BANGKE_DONVI> ManagerGetAllForIndex(FindModelGcs findModelGcs, string orderKey, ref Paging page)
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
                   ((qString == null || qString == "" || o.MA_LOAIBANGKE.Contains(qString) || o.MA_DVIQLY.Contains(qString) || o.HasD_LOAI_BANGKE.TEN_LOAIBANGKE.Contains(qString))
                   && (findModelGcs.MaBangKe == null || o.MA_LOAIBANGKE == findModelGcs.MaBangKe)
                   && (findModelGcs.MaDonVi == null || o.MA_DVIQLY == findModelGcs.MaDonVi)
                   && (findModelGcs.TenBangKe == null || o.HasD_LOAI_BANGKE.TEN_LOAIBANGKE == findModelGcs.TenBangKe))
                , "MA_LOAIBANGKE", ref page)
                .ToList()
                .Select(x => new CFG_BANGKE_DONVI()
                {
                    ID = x.ID,
                    MA_LOAIBANGKE = x.MA_LOAIBANGKE,
                    MA_DVIQLY = x.MA_DVIQLY,
                    GHI_CHU = x.GHI_CHU,
                    Ten_LoaiBangKe = x.HasD_LOAI_BANGKE == null ? "" : x.HasD_LOAI_BANGKE.TEN_LOAIBANGKE
                })
                .ToList();

            return ret;
        }
    }
}