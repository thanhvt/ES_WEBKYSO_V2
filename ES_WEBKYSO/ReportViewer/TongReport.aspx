﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

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
             ReportHelper reportHelper = new ReportHelper(UnitOfWork);
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
             MaBangKe = Request.Params["LoaiBangKe"];
             var nguoiDoiSoat = Request.Params["NguoiDoiSoat"];
             if (maSo == null)
             {
                 lblMessage.Text = "Không tìm thấy file này";
             }

             //IdLich = int.Parse(Request["IdLich"]);
             //MaBangKe = Request["MaBangKe"];
             //if(MaBangKe == null || IdLich == 0)
             //{
             //    lblMessage.Text = "Không tìm thấy file này";
             //}



             //MaQuyen = reportHelper.getMaQuyen(IdLich);
             //NgayGcs = reportHelper.getNgayGcs(IdLich);
             InstanceReportSource instanceReportSource = new InstanceReportSource();
             switch (MaBangKe)
             {
                 case "08":

                     rptBangKeChiSo reportBkcs = new rptBangKeChiSo();
                     var rptBkcs = new rptBangKeChiSo();

                     instanceReportSource.ReportDocument = rptBkcs;
                     base.OnLoad(e);

                     var sourceBkcs = reportHelper.getReportBkcsKTN(ky, thang, nam, maSo[0]);
                     //if (sourceBkcs != null)
                     //{
                     //    reportBkcs.SetSourceTable(sourceBkcs);
                     //    reportBkcs.SetParamater(NgayGcs,MaQuyen);
                     //}
                     //var sourceBkcs = reportHelper.getReportBkcs(IdLich);
                     //if (sourceBkcs != null)
                     //{
                     //    reportBkcs.SetSourceTable(sourceBkcs);
                     //    reportBkcs.SetParamater(NgayGcs,MaQuyen);
                     //}
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
                 case "06":
                     rptSanLuongBatThuong report_SLBT = new rptSanLuongBatThuong();
                     var rpt_Slbt = new rptSanLuongBatThuong();

                     instanceReportSource.ReportDocument = rpt_Slbt;
                     base.OnLoad(e);

                     var source_Slbt = reportHelper.get_SLBT(ky, thang, nam, maSo[0]);
                     if (source_Slbt != null)
                     {
                         report_SLBT.SetSourceTable(source_Slbt);
                     }
                     instanceReportSource.ReportDocument = report_SLBT.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 case "07":
                     rptAnhSanLuongBatThuong report_ASLBT = new rptAnhSanLuongBatThuong();
                     var rpt_Aslbt = new rptAnhSanLuongBatThuong();

                     instanceReportSource.ReportDocument = rpt_Aslbt;
                     base.OnLoad(e);

                     var source_Aslbt = reportHelper.get_SLBT(ky, thang, nam, maSo[0]);
                     if (source_Aslbt != null)
                     {
                         report_ASLBT.SetSourceTable(source_Aslbt);
                     }
                     instanceReportSource.ReportDocument = report_ASLBT.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 case "09":
                     rptCongToCoChiSoPMax report_ChiSoPMax = new rptCongToCoChiSoPMax();
                     var rpt_ChiSoPMax = new rptCongToCoChiSoPMax();

                     instanceReportSource.ReportDocument = rpt_ChiSoPMax;
                     base.OnLoad(e);

                     var source_ChiSoPMax = reportHelper.GetCHISO_PMax(ky, thang, nam, maSo[0]);
                     if (source_ChiSoPMax != null)
                     {
                         report_ChiSoPMax.SetSourceTable(source_ChiSoPMax);
                     }
                     instanceReportSource.ReportDocument = report_ChiSoPMax.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                  case "10":
                     rptPhucTraChiSoCongTo report_PhucTraChiSoCongTo = new rptPhucTraChiSoCongTo();
                     var rpt_PhucTraChiSoCongTo = new rptCongToCoChiSoPMax();

                     instanceReportSource.ReportDocument = rpt_PhucTraChiSoCongTo;
                     base.OnLoad(e);

                     var source_PhucTraChiSoCongTo = reportHelper.get_SLBT_PhucTraChiSo(ky, thang, nam, maSo[0]);
                     if (source_PhucTraChiSoCongTo != null)
                     {
                         report_PhucTraChiSoCongTo.SetSourceTable(source_PhucTraChiSoCongTo);
                     }
                     instanceReportSource.ReportDocument = report_PhucTraChiSoCongTo.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                  case "12":
                     rptPhieuXacMinhChiSoCongTo report_PhieuXacMinhChiSoCongTo = new rptPhieuXacMinhChiSoCongTo();
                     var rpt_PhieuXacMinhChiSoCongTo = new rptPhieuXacMinhChiSoCongTo();

                     instanceReportSource.ReportDocument = rpt_PhieuXacMinhChiSoCongTo;
                     base.OnLoad(e);

                     var source_PhieuXacMinhChiSoCongTo = reportHelper.get_SLBT(ky, thang, nam, maSo[0]);
                     if (source_PhieuXacMinhChiSoCongTo != null)
                     {
                         report_PhieuXacMinhChiSoCongTo.SetSourceTable(source_PhieuXacMinhChiSoCongTo);
                     }
                     instanceReportSource.ReportDocument = report_PhieuXacMinhChiSoCongTo.Report;
                     rptTong.ReportSource = instanceReportSource;
                     break;
                 default:
                     break;
             }

         }
</script>
</html>
