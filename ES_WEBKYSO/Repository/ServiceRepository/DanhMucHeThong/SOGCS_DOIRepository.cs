using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.DanhMucHeThong
{
    public class SOGCS_DOIRepository<T> : BaseRepository<SOGCS_DOI>
    {
        public SOGCS_DOIRepository(DbContext context, GenericRepository repo) : base(context, repo)
        {

        }

        public override List<SOGCS_DOI> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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
                   ((qString == null || qString == "" || o.MA_DVIQLY.Equals(qString) || o.MA_DOIGCS.Equals(qString) || o.TEN_DOI.Equals(qString)) 
                   && (findModel.MaDoi == null || o.MA_DOIGCS == findModel.MaDoi)
                   && (findModel.TenDoi == null || o.TEN_DOI == findModel.TenDoi))
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)

                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new SOGCS_DOI
                {
                    MA_DOIGCS = x.MA_DOIGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    TEN_DOI = x.TEN_DOI,
                    GHI_CHU = x.GHI_CHU
                })
                .ToList();

            return ret;
        }
    }
}