<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Import Namespace="ES_WEBKYSO.Common" %>
<%@ Import Namespace="ES_WEBKYSO.DataContext" %>
<%@ Import Namespace="ES_WEBKYSO.Reports" %>
<%@ Import Namespace="ES_WEBKYSO.Repository" %>
<%@ Import Namespace="Telerik.Reporting" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <link href="~/Content/themes/ClipOne/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="main" method="post" action="">
        <div class="DoiSoatReport">
            <telerik:ReportViewer ID="rptDoiSoat" CssClass="reportViewer" runat="server" Width="100%" Height="600px">
            </telerik:ReportViewer>
        </div>
    </form>
    <asp:Label Text="" runat="server" ID="lblMessage" />
</body>
<script runat="server">
    public readonly UnitOfWork UnitOfWork = new UnitOfWork(new DataContext());
    //public string maDonVi { get; set; }
    //public List<string> maSo { get; set; }
    //public int ky { get; set; }
    //public int thang { get; set; }
    //public int nam { get; set; }
    //public string loaiBangKe { get; set; }
    //public string nguoiDoiSoat { get; set; }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // to avoid the server form (<form runat="server"> requirement
    }

    protected override void OnLoad(EventArgs e)
    {

         var maSos = Request.Params["MaSo"];;
            maSos = maSos ?? "";
            var maSoSplit = maSos.Split(',');
            if (maSos == "")
            {
                maSoSplit = new string[] {};
            }


        var maDonVi = Request.Params["MaDonVi"];
        //maSo = Request.Params["MaSo"];
        string[] maSo = maSoSplit;
        var ky = int.Parse(Request.Params["Ky"]);
        var thang = int.Parse(Request.Params["Thang"]);
        var nam = int.Parse(Request.Params["Nam"]);
        var loaiBangKe = Request.Params["LoaiBangKe"];
        var nguoiDoiSoat = Request.Params["NguoiDoiSoat"];
        if (maSo == null)
        {
            lblMessage.Text = "Không tìm thấy file này";
        }


        ReportHelper reportHelper = new ReportHelper(UnitOfWork);
        //MaQuyen = reportHelper.getMaQuyen(Id);
        //NgayGcs = reportHelper.getNgayGcs(Id);
        InstanceReportSource instanceReportSource = new InstanceReportSource();
        switch (loaiBangKe)
        {
            case "01":

                rptDsKhKhongChupAnh reportDsKhKhongChupAnh = new rptDsKhKhongChupAnh();
                var rptDsKhKhongChupAnh = new rptDsKhKhongChupAnh();

                instanceReportSource.ReportDocument = rptDsKhKhongChupAnh;
                base.OnLoad(e);

                var sourceDsKhKca = reportHelper.GetReportDsKhKhongChupAnh(maDonVi, maSo, ky, thang, nam, loaiBangKe, nguoiDoiSoat);
                if (sourceDsKhKca != null)
                {
                    reportDsKhKhongChupAnh.SetSourceTable(sourceDsKhKca);
                    reportDsKhKhongChupAnh.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhKhongChupAnh.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportDsKhKhongChupAnh.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhKhongChupAnh.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                break;
            case "02":

                rptDsKhKhongDuocDs reportDsKhKhongDuocDs = new rptDsKhKhongDuocDs();
                var rptDsKhKhongDuocDs = new rptDsKhKhongDuocDs();

                instanceReportSource.ReportDocument = rptDsKhKhongDuocDs;
                base.OnLoad(e);

                var sourceDsKhKds = reportHelper.GetReportDsKhKhongDuocDs(maDonVi, maSo, ky, thang, nam, loaiBangKe, nguoiDoiSoat);
                if (sourceDsKhKds != null)
                {
                    reportDsKhKhongDuocDs.SetSourceTable(sourceDsKhKds);
                    reportDsKhKhongDuocDs.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhKhongDuocDs.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportDsKhKhongDuocDs.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhKhongDuocDs.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }

                break;
            case "03":

                rptDsKhDaDs reportDsKhDaDs = new rptDsKhDaDs();
                var rptDsKhDaDs = new rptDsKhDaDs();

                instanceReportSource.ReportDocument = rptDsKhDaDs;
                base.OnLoad(e);

                var sourceDsKhDds = reportHelper.GetReportDsKhDaDs(maDonVi, maSo, ky, thang, nam, loaiBangKe, nguoiDoiSoat);
                if (sourceDsKhDds != null)
                {
                    reportDsKhDaDs.SetSourceTable(sourceDsKhDds);
                    reportDsKhDaDs.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDaDs.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportDsKhDaDs.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDaDs.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                break;
            case "04":

                rptDsKhDsDat reportDsKhDsDat = new rptDsKhDsDat();
                var rptDsKhDsDat = new rptDsKhDaDs();

                instanceReportSource.ReportDocument = rptDsKhDsDat;
                base.OnLoad(e);

                var sourceDsKhDsDat = reportHelper.GetReportDsKhDsDat(maDonVi, maSo, ky, thang, nam, loaiBangKe, nguoiDoiSoat);
                if (sourceDsKhDsDat != null)
                {
                    reportDsKhDsDat.SetSourceTable(sourceDsKhDsDat);
                    reportDsKhDsDat.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDsDat.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportDsKhDsDat.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDsDat.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                break;
            case "05":

                rptDsKhDsKhongDat reportDsKhDsKhongDat = new rptDsKhDsKhongDat();
                var rptDsKhDsKhongDat = new rptDsKhDsKhongDat();

                instanceReportSource.ReportDocument = rptDsKhDsKhongDat;
                base.OnLoad(e);

                var sourceDsKhDsKhongDat = reportHelper.GetReportDsKhDsKhongDat(maDonVi, maSo, ky, thang, nam, loaiBangKe, nguoiDoiSoat);
                if (sourceDsKhDsKhongDat != null)
                {
                    reportDsKhDsKhongDat.SetSourceTable(sourceDsKhDsKhongDat);
                    reportDsKhDsKhongDat.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDsKhongDat.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportDsKhDsKhongDat.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportDsKhDsKhongDat.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                break;
           case "06":

                rptCongToSanLuongBatThuongDaXacNhan reportSLBT = new rptCongToSanLuongBatThuongDaXacNhan();
                var rptSLBT = new rptCongToSanLuongBatThuongDaXacNhan();

                instanceReportSource.ReportDocument = rptSLBT;
                base.OnLoad(e);

                var sourceSLBT = reportHelper.get_SLBT(ky, thang, nam, maSo[0]);
                if (sourceSLBT != null)
                {
                    reportSLBT.SetSourceTable(sourceSLBT);
                    reportSLBT.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportSLBT.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                else
                {
                    reportSLBT.SetParamater(nguoiDoiSoat);
                    instanceReportSource.ReportDocument = reportSLBT.Report;
                    rptDoiSoat.ReportSource = instanceReportSource;
                }
                break;
            default:
                break;
        }

    }
</script>
</html>
