using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Common
{
    public class colData
    {
        public string col_name;
        public Type col_type;
        public DbType db_type;
        public string sqlite_type;
        public bool allow_null;

        public colData(string col_name, Type col_type, DbType db_type, string sqlite_type, bool allow_null)
        {
            this.col_name = col_name;
            this.col_type = col_type;
            this.db_type = db_type;
            this.sqlite_type = sqlite_type;
            this.allow_null = allow_null;
        }
    }
}