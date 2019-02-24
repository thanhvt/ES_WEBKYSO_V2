namespace ES_WEBKYSO.Reports
{
    using Reports;
    using System;
    using System.ComponentModel;
    using System.Drawing;
  
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rpDanhSachCongTo.
    /// </summary>
    public partial class rptDanhSachCongTo : Telerik.Reporting.Report
    {
        public rptDanhSachCongTo()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        public void SetSourceTable(object datasource)
        {
            tbdanhsachcongto.DataSource = datasource;
        }
    }
}