namespace ES_WEBKYSO.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptSoLuongBatThuong.
    /// </summary>
    public partial class rptSanLuongBatThuong : Telerik.Reporting.Report
    {
        public rptSanLuongBatThuong()
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
            tblsanluongbatthuong.DataSource = datasource;
        }
    }
}