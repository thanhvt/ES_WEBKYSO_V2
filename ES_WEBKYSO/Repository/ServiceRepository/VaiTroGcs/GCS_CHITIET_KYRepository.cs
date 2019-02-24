using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.VaiTroGcs
{
    public class GCS_CHITIET_KYRepository<T> : BaseRepository<GCS_CHITIET_KY>
    {
        public GCS_CHITIET_KYRepository(DbContext context, GenericRepository repo) : base(context, repo)
        {

        }
        public override List<GCS_CHITIET_KY> ManagerGetAllForIndex(int ID_LICHGCS, string orderKey, ref Paging page)
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
                       (qIntSearch == null)
                       &&(qString == null || qString == "" || o.NGUOI_KY.Contains(qString))
                       && (qDateTimeSearch == null || qDateTimeSearch == o.NGAY_KY)
                   , "ID", ref page)
               .ToList()
               .Select(x => new GCS_CHITIET_KY
               {
                   ID = x.ID,
                   MA_BANGKELICH = x.MA_BANGKELICH,
                   UserId = x.UserId,
                   NGUOI_KY = x.NGUOI_KY,
                   NGAY_KY = x.NGAY_KY,
                   NGAY_KYString = x.NGAY_KY.ToString("dd'/'MM'/'yyyy") ?? "",
                   GHI_CHU = x.GHI_CHU
               })
                .ToList();

            return ret;
        }
    }
}