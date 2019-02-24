using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ES_WEBKYSO.Areas.CauHinh.Models;
using Common.Helpers;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class CFG_BOPHAN_KYRepository<T> : BaseRepository<CFG_BOPHAN_KY>
    {
        public CFG_BOPHAN_KYRepository(DbContext context, UnitOfWork UnitOfWork) : base(context, UnitOfWork)
        {
        }
        public override List<CFG_BOPHAN_KY> ManagerGetAllForIndex(BOPHANKY model, string orderKey, ref Paging page)
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
                   && (model.MA_LOAIBANGKE == null || o.MA_LOAIBANGKE == model.MA_LOAIBANGKE)
                   && (model.MA_DVIQLY == null || o.MA_DVIQLY == model.MA_DVIQLY)
                   && (model.Ten_LoaiBangKe == null || o.HasD_LOAI_BANGKE.TEN_LOAIBANGKE == model.Ten_LoaiBangKe))
                , "MA_LOAIBANGKE", ref page)
                .ToList()
                .Select(x => new CFG_BOPHAN_KY()
                {
                    MA_BOPHAN_KY = x.MA_BOPHAN_KY,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_LOAIBANGKE = x.MA_LOAIBANGKE,
                    MO_TA = x.MO_TA,
                    THU_TUKY = x.THU_TUKY,
                    Ten_LoaiBangKe = x.HasD_LOAI_BANGKE == null ? "" : x.HasD_LOAI_BANGKE.TEN_LOAIBANGKE,
                    Ten_BoPhanKy = x.HasWebpagesRoles.RoleName == null ? "" : x.HasWebpagesRoles.RoleName
                }).OrderBy(o=>o.THU_TUKY)
                .ToList();

            return ret;
        }
    }
}