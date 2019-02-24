<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="ES_WEBKYSO.Models" %>
<%@ Import Namespace="ES_WEBKYSO.Reports" %>
<%@ Import Namespace="ES_WEBKYSO.DataContext" %>
<%@ Import Namespace="ES_WEBKYSO.Repository" %>
<%@ Import Namespace="ES_WEBKYSO.Common" %>
<%--<%@ Import Namespace="ES_WEBKYSO.Service_GCS" %>--%>
<%@ Import Namespace="Telerik.Reporting" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .reportViewer {
            margin: auto;
            max-width: 1600px;
        }
    </style>
</head>
<body>
    <form id="main" method="post" action="">
        <div class="reportViewer">
            <telerik:ReportViewer ID="ReportViewer1" Width="100%" Height="900px" runat="server">
            </telerik:ReportViewer>
        </div>
    </form>
</body>

<script runat="server">
    public override void VerifyRenderingInServerForm(Control control)
    {
        // to avoid the server form (<form runat="server"> requirement
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        var reportName = Request["maloaibangke"];

        var instanceReportSource = new InstanceReportSource();
        Report report;
        switch (reportName)
        {
            case "TTBT":
                rptTinhTrangBatThuong report1 = new rptTinhTrangBatThuong();
                //report1.DataSource = GetTinhTrangBatThuongSource();
                var source = GetTinhTrangBatThuongSource();
                if (source!=null)
                {
                    report1.SetSourceTable(source);
                }
                instanceReportSource.ReportDocument = report1.Report;
                ReportViewer1.ReportSource = instanceReportSource;
                break;
            case "SLBT":
                //var instanceReportSource2123 = new InstanceReportSource();
                //EsReport report2342 = new EsReport( new rptTinhTrangBatThuong());
                //report2342.DataSource = report.GetSource();
                //instanceReportSource.ReportDocument = report.Report;
                //ReportViewer1.ReportSource = instanceReportSource;
                break;
            case "DSCT":
                //var instanceReportSource = new InstanceReportSource();
                ////var report = new rptABC();
                //report.DataSource = dtField;
                //instanceReportSource.ReportDocument = report;
                //ReportViewer1.ReportSource = instanceReportSource;
                break;
        }
    }

    public readonly UnitOfWork UnitOfWork = new UnitOfWork(new DataContext());

    private object GetTinhTrangBatThuongSource()
    {
        int idLichGcs = string.IsNullOrEmpty(Request["ID_LICHGCS"]) ? 0 : Convert.ToInt32(Request["ID_LICHGCS"]);
        if (idLichGcs==0)
        {
            return null;
        }

        //1.getXML

        //lấy thang ky nam, maso theo idlich
        var gcsLichgcs = UnitOfWork.RepoBase<GCS_LICHGCS>().GetAll(i => i.ID_LICHGCS == idLichGcs).FirstOrDefault();

        var maso = gcsLichgcs.MA_SOGCS;
        var nam = gcsLichgcs.NAM;
        var thang = gcsLichgcs.THANG;
        var ky = gcsLichgcs.KY;
        //var mapPath = Server.MapPath("~/TemplateFile/" + gcsLichgcs.MA_DVIQLY + "/" + maso + "-" + nam + "-" + thang + "-" + ky + ".xml");
        var mapPath = Utility.getXMLPath() + gcsLichgcs.MA_DVIQLY + "/" + maso + "-" + nam + "-" + thang + "-" + ky + ".xml";
        DataSet dsSo = new DataSet();
        dsSo.ReadXml(mapPath);
        //2.gán XML => Source

        return dsSo;
    }


</script>
</html>
