<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="ES_WEBKYSO.Common" %>
<%@ Import Namespace="ES_WEBKYSO.Models" %>
<%@ Import Namespace="ES_WEBKYSO.DataContext" %>
<%@ Import Namespace="ES_WEBKYSO.Reports" %>
<%@ Import Namespace="ES_WEBKYSO.Repository" %>
<%--<%@ Import Namespace="ES_WEBKYSO.Service_GCS" %>--%>
<%@ Import Namespace="Telerik.Reporting" %>
<%@ Import Namespace="Telerik.Reporting.Processing" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #rptViewBangKeChiSo {
            width: 75vw !important;
            height: 60vh !important;
        }
    </style>
</head>
<body>
    <form id="main" method="post" action="">
        <div class="ReportBangKeChiSo">
            <telerik:ReportViewer ID="rptViewBangKeChiSo" CssClass="reportViewer" runat="server">
            </telerik:ReportViewer>
        </div>
    </form>
</body>

<script runat="server">
    public readonly UnitOfWork UnitOfWork = new UnitOfWork(new DataContext());


    public override void VerifyRenderingInServerForm(Control control)
    {
        // to avoid the server form (<form runat="server"> requirement
    }

    protected override void OnLoad(EventArgs e)
    {
        int idLichGcs = string.IsNullOrEmpty(Request["idLichgcs"]) ? 0 : Convert.ToInt32(Request["idLichgcs"]);
        InstanceReportSource instanceReportSource = new InstanceReportSource();
        rptBangKeChiSo report1 = new rptBangKeChiSo();
        var report = new rptBangKeChiSo();

        instanceReportSource.ReportDocument = report;
        base.OnLoad(e);
        BangKeHelper bangKeHelper = new BangKeHelper(UnitOfWork);
        var source = bangKeHelper.GetBangKeChiSo(idLichGcs);
        if (source != null)
        {
            report1.SetSourceTable(source);
        }
        instanceReportSource.ReportDocument = report1.Report;
        rptViewBangKeChiSo.ReportSource = instanceReportSource;
    }
</script>
</html>
