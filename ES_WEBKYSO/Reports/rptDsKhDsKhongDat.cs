namespace ES_WEBKYSO.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptDsKhDsKhongDat.
    /// </summary>
    public partial class rptDsKhDsKhongDat : Telerik.Reporting.Report
    {
        public rptDsKhDsKhongDat()
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
            //Table dt = new Table();
            //dt.DataSource = datasource;
            this.DataSource = datasource;
        }
        public void SetParamater(string nguoidoisoat)
        {
            ReportParameter[] parameter = new ReportParameter[1];
            parameter[0] = new ReportParameter("nguoiDoiSoat", ReportParameterType.String, nguoidoisoat);
            this.ReportParameters.Add(parameter[0]);
        }
    }
}