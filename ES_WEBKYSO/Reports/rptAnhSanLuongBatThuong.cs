namespace ES_WEBKYSO.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptAnhSanLuongBatThuong.
    /// </summary>
    public partial class rptAnhSanLuongBatThuong : Telerik.Reporting.Report
    {
        public rptAnhSanLuongBatThuong()
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
            tblanhsanluongbatthuong.DataSource = datasource;
        }
        public void SetParamater(string nguoidoisoat)
        {
            ReportParameter[] parameter = new ReportParameter[1];
            parameter[0] = new ReportParameter("nguoiDoiSoat", ReportParameterType.String, nguoidoisoat);
            this.ReportParameters.Add(parameter[0]);
        }
    }
}