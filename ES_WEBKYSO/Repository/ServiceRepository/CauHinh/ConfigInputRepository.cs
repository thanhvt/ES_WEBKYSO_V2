using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class ConfigInputRepository<T> : BaseRepository<ConfigInput>
    {
        public ConfigInputRepository(DbContext context, GenericRepository repo) : base(context, repo)
        {

        }

        public override List<ConfigInput> ManagerGetAllForIndex(FindModelGcs find, string orderKey, ref Paging page)
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
                .Select(x => new ConfigInput()
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