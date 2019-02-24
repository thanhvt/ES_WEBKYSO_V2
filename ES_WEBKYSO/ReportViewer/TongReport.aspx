<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="ES_WEBKYSO.Common" %>
<%@ Import Namespace="ES_WEBKYSO.DataContext" %>
<%@ Import Namespace="ES_WEBKYSO.Models" %>
<%@ Import Namespace="ES_WEBKYSO.Reports" %>
<%@ Import Namespace="ES_WEBKYSO.Repository" %>
<%@ Import Namespace="ES_WEBKYSO.ModelParameter" %>
<%--<%@ Import Namespace="ES_WEBKYSO.Service_GCS" %>--%>
<%@ Import Namespace="Telerik.Reporting" %>
<%@ Import Namespace="Telerik.Reporting.Processing" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <link href="~/Content/themes/ClipOne/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="main" method="post" action="">
        <div class="ReportTong">
            <telerik:ReportViewer ID="rptTong" CssClass="reportViewer" runat="server" Width ="100%" Height="600px">
            </telerik:ReportViewer>
        </div>
    </form>
    <asp:Label Text="" runat="server" Id="lblMessage"/>
</body>
     <script runat="server">
         public readonly UnitOfWork UnitOfWork = new UnitOfWork(new DataContext());
         public int IdLich { get; set; }
         public string MaBangKe { get; set; }
         public string MaQuyen { get; set; }
         public string NgayGcs { get; set; }
         public override void VerifyRenderingInServerForm(Control control)
         {
             // to avoid the server form (<form runat="server"> requirement
         }

         protected override void OnLoad(EventArgs e)
         {

             IdLich = int.Parse(Request["IdLich"]);
             MaBangKe = Request["MaBangKe"];
             if(MaBangKe == null || IdLich == 0)
             {
                 lblMessage.Text = "Không tìm thấy file này";
             }
            

             ReportHelper reportHelper = new ReportHelper(UnitOfWork);
             MaQuyen = reportHelper.getMaQuyen(IdLich);
             NgayGcs = reportHelper.getNgayGcs(IdLich);
              InstanceReportSource instanceReportSource = new InstanceReportSource();
             switch (MaBangKe)
             {
                 case "BKCS":
                    
                     rptBangKeChiSo reportBkcs = new rptBangKeChiSo();
                     var rptBkcs = new rptBangKeChiSo();

                     instanceReportSource.ReportDocument = rptBkcs;
                     base.OnLoad(e);


                     var sourceBkcs = reportHelper.getReportBkcs(IdLich);
                     if (sourceBkcs != null)
                     {
                         reportBkcs.SetSourceTable(sourceBkcs);
                         reportBkcs.SetParamater(NgayGcs,MaQuyen);
                     }
                     instanceReportSource.ReportDocument = reportBkcs.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 case "TTBT":
                    
                     rptTinhTrangBatThuong reportTtbt = new rptTinhTrangBatThuong();
                     var rptTtbt = new rptTinhTrangBatThuong();

                     instanceReportSource.ReportDocument = rptTtbt;
                     base.OnLoad(e);


                     var source = reportHelper.getReportBkcs(IdLich);
                     if (source != null)
                     {
                         reportTtbt.SetSourceTable(source);
                     }
                     instanceReportSource.ReportDocument = reportTtbt.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 case "SLBT":
                     rptSanLuongBatThuong report_SLBT = new rptSanLuongBatThuong();
                     var rpt_Slbt = new rptSanLuongBatThuong();

                     instanceReportSource.ReportDocument = rpt_Slbt;
                     base.OnLoad(e);

                     var source_Slbt = reportHelper.get_SLBT(IdLich);
                     if (source_Slbt != null)
                     {
                         report_SLBT.SetSourceTable(source_Slbt);
                     }
                     instanceReportSource.ReportDocument = report_SLBT.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 case "ASLBT":
                     rptSanLuongBatThuong report_ASLBT = new rptSanLuongBatThuong();
                     var rpt_Aslbt = new rptSanLuongBatThuong();

                     instanceReportSource.ReportDocument = rpt_Aslbt;
                     base.OnLoad(e);

                     var source_Aslbt = reportHelper.get_SLBT(IdLich);
                     if (source_Aslbt != null)
                     {
                         report_ASLBT.SetSourceTable(source_Aslbt);
                     }
                     instanceReportSource.ReportDocument = report_ASLBT.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 default:
                     break;
             }

         }
</script>
</html>
