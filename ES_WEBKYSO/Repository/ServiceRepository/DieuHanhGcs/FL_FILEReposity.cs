using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.DieuHanhGcs
{
    public class FL_FILERepository<T> : BaseRepository<FL_FILE>
    {
        public FL_FILERepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }
        public override List<FL_FILE> ManagerGetAllForIndex(string orderKey, ref Paging page)
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
                   (qIntSearch == null || qIntSearch.Value == o.MA_BANGKELICH || qIntSearch.Value == o.MA_BANGKELICH)

                , "FileID", ref page)
                .ToList()
                .Select(x => new FL_FILE
                {
                    FileID = x.FileID,
                    MA_BANGKELICH = x.MA_BANGKELICH,
                    FilePath = x.FilePath,
                    FileHash = x.FileHash
                })
                .ToList();

            return ret;
        }
    }
}