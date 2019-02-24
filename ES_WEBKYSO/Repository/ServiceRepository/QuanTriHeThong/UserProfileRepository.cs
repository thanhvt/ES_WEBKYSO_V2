using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Common.Helpers;

namespace ES_WEBKYSO.Repository.ServiceRepository.CauHinh
{
    public class UserProfileRepository<T> : BaseRepository<UserProfile>
    {
        public UserProfileRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {
        }

        public override List<UserProfile> GETALL()
        {
            try
            {
                var ret = GetAll().ToList();
                foreach (var item in ret)
                {
                    if (item.UserName.ToLower() == "administrator") { ret.Remove(item); break; }
                }         
                return ret;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}