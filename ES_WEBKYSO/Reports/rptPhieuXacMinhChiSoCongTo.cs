namespace ES_WEBKYSO.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptPhieuXacMinhChiSoCongTo.
    /// </summary>
    public partial class rptPhieuXacMinhChiSoCongTo : Telerik.Reporting.Report
    {
        public rptPhieuXacMinhChiSoCongTo()
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
            this.DataSource = datasource;
        }
    }
}