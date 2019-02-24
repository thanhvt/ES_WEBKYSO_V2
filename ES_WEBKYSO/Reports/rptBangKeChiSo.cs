namespace ES_WEBKYSO.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptBangKeChiSo.
    /// </summary>
    public partial class rptBangKeChiSo : Telerik.Reporting.Report
    {
        public rptBangKeChiSo()
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
            tbBangKeChiSo.DataSource = datasource;
        }
        public void SetParamater(string ngaygcs, string maquyen)
        {
            ReportParameter[] parameter = new ReportParameter[2];
            //parameter[0] = new ReportParameter("NgayGCS", ReportParameterType.DateTime, Convert.ToDateTime(ngaygcs));
            parameter[0] = new ReportParameter("NgayGCS", ReportParameterType.String, ngaygcs.ToString());
            parameter[1] = new ReportParameter("MaQuyen", ReportParameterType.String, maquyen.ToString());
            this.ReportParameters.Add(parameter[0]);
            this.ReportParameters.Add(parameter[1]);
        }
    }
}