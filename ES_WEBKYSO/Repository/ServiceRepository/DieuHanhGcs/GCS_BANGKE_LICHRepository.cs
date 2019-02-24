using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.DieuHanhGcs
{
    public class GCS_BANGKE_LICHRepository<T> : BaseRepository<GCS_BANGKE_LICH>
    {
        public GCS_BANGKE_LICHRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }
        public override List<GCS_BANGKE_LICH> GETALL()
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
        public List<GCS_BANGKE_LICH> ManagerGetAllForIndex(int ID_LICHGCS, string orderKey, ref Paging page)
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

            DateTime qDateTime;
            DateTime? qDateTimeSearch = null;
            if (DateTime.TryParse(qString, out qDateTime))
            {
                qDateTimeSearch = qDateTime;
            }

            // qString, qInt, qDateTime
            var ret = GetAll(o =>
                   (qString == null || qString == "" || o.MA_LOAIBANGKE.Contains(qString))

                , "MA_BANGKELICH", ref page)
                .ToList()
                .Select(x => new GCS_BANGKE_LICH
                {
                    MA_BANGKELICH = x.MA_BANGKELICH,
                    ID_LICHGCS = x.ID_LICHGCS,
                    MA_LOAIBANGKE = x.MA_LOAIBANGKE,
                    Ten_LoaiBangKe = x.HasD_LOAI_BANGKE == null ? "" : x.HasD_LOAI_BANGKE.TEN_LOAIBANGKE
                })
                .ToList();

            return ret;
        }
        public override List<GCS_BANGKE_LICH> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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
                   (qString == null || qString == "" || qString.Equals(qString))
                   && (findModel.MaLoaiBangKe == null || o.MA_LOAIBANGKE == findModel.MaLoaiBangKe)
                , "MA_BANGKELICH", ref page)
                .ToList()
                .Select(x => new GCS_BANGKE_LICH
                {
                    MA_BANGKELICH = x.MA_BANGKELICH,
                    ID_LICHGCS = x.ID_LICHGCS,
                    MA_LOAIBANGKE = x.MA_LOAIBANGKE,
                    Ten_LoaiBangKe = x.HasD_LOAI_BANGKE == null ? "" : x.HasD_LOAI_BANGKE.TEN_LOAIBANGKE
                })
                .ToList();

            return ret;
        }
    }
}