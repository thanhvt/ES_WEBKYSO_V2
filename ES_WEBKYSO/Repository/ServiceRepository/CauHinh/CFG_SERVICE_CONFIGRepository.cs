using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class CFG_SERVICE_CONFIGRepository<T> : BaseRepository<CFG_SERVICE_CONFIG>
    {
        public CFG_SERVICE_CONFIGRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }

        public override List<CFG_SERVICE_CONFIG> ManagerGetAllForIndex(FindModelGcs find, string orderKey, ref Paging page)
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
                        &&
                        (qString == null || qString == "" || o.TypeInput.Contains(qString) ||
                         o.Desctiption.Contains(qString))
                    , "ConfigId", ref page)
                .ToList()
                .Select(x => new CFG_SERVICE_CONFIG()
                {
                    ConfigId = x.ConfigId,
                    TypeInput = x.TypeInput,
                    Desctiption = x.Desctiption,
                    Value = x.Value
                })
                .ToList();

            return ret;
        }
    }
}