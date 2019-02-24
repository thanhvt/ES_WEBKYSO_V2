using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;

namespace ES_WEBKYSO.Common
{
    #region Markup for Import Area

    public class ImportIncludeAttribute : Attribute
    {
        public int Order { get; set; }
        /// <summary>
        /// 0 not allow
        /// 1 allow
        /// 2 allow and hidden
        /// 3 allow and readonly
        /// 4 allow and disable
        /// </summary>
        public int ForUpdateForm { get; set; } // cho phép trên update
        /// <summary>
        /// 0 not allow
        /// 1 allow and sort
        /// 2 allow and nsort
        /// </summary>
        public int ForDataTable { get; set; } // nếu hiển thị trên datatable
        public int AllowFormatNumber { get; set; }
        public int ForExcel { get; set; } // nếu excel cần 
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool AllowNull { get; set; }
        public string CssName { get; set; }
        public string Alias { get; set; }

        #region Construct style
        public ImportIncludeAttribute()
        {
            ForDataTable = 1;
            ForExcel = 1;
            ForUpdateForm = 1;
            AllowFormatNumber = 0;
        }

        #endregion

        #region Rewirte for DataTable Js

        //public string header => DisplayName;
        //public string data => Name;
        //public string className => CssName;

        #endregion

    }

    #endregion
}