using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Common.Helpers;
using ES_WEBKYSO.Common;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;
using Microsoft.Ajax.Utilities;
using System.Xml;
using System.Xml.Serialization;

namespace ES_WEBKYSO.Areas.MDMS.Controllers
{
    public class SoGcsController : BaseController
    {
        // GET: /MDMS/d_sogcs/
        public ActionResult Index()
        {
            //var result = _repo.RepoBase<GcsLichGcs>().Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));


            ViewBag.Title = "Danh sách sổ GCS";
            ViewBag.MaDonVi = "PD0100";
            return View();
        }

        [HttpPost]
        public ActionResult GetJson(FindModelGcs findPar)
        {
            var paging = Request.Params.ToPaging("Year");
            // Lấy dữ liệu từ CSDL sử dụng Paging để phân trang
            var data = _repo.RepoBase<D_SOGCS>().ManagerGetAllForIndex(findPar, paging.OrderKey, ref paging).ToList();
            paging.data = data;
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JsonLayDuLieuCmis(FindModelGcs findPar, string search, string maDvQly)
        {
            int nam = Convert.ToInt32(findPar.Nam);
            int thang = Convert.ToInt32(findPar.Thang);
            int ky = Convert.ToInt32(findPar.Ky);
            var status = "Đang chờ";

            if (thang == 0 && ky == 0 || nam == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn kỳ, tháng và năm" }, JsonRequestBehavior.AllowGet);
            }

            var data = _repo.RepoBase<D_SOGCS>()
                .ManagerGetAllForIndex(findPar, search)
                .ToList()
                .Select(x => x.MA_SOGCS).ToList();

            Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
            var lstLoi = new List<string>();

            foreach (var item in data)
            {
                string result = "";
                var dataFromCmis = ser.ReadHHCService(maDvQly, item, ky, thang, nam, ref result);

                if (result == "Ok")
                {

                    //dataFromCmis.WriteXml(@"D:\" + item + "-" + nam + "-" + thang + "-" + ky + ".xml");
                    var mapPath = Server.MapPath("~/TemplateFile/" + item + "-" + nam + "-" + thang + "-" + ky + ".xml");

                    dataFromCmis.WriteXml(mapPath);

                    var modelOrig = _repo.RepoBase<D_SOGCS>().GetOne(x => x.MA_DVIQLY == maDvQly && x.MA_SOGCS == item && x.NGAY_GHI == findPar.NgayGhi);

                    var model = _repo.RepoBase<GCS_LICHGCS>().GetOne(x => x.MA_DVIQLY == maDvQly && x.MA_SOGCS == item && x.KY == ky && x.THANG == thang && x.NAM == nam);

                    if (modelOrig == null || model == null)
                    {
                        return Json(new { success = false, message = "Không có dữ liệu đầu vào!" }, JsonRequestBehavior.AllowGet);
                    }



                    modelOrig.TRANG_THAI = status;

                    model.FILE_XML = mapPath;

                    int lichqkq = _repo.RepoBase<GCS_LICHGCS>().Update(model);

                    int kq = _repo.RepoBase<D_SOGCS>().Update(modelOrig);
                    //return Json(new { success = true, message = "Lấy dữ liệu CMIS thành công!"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    lstLoi.Add(item);
                }
            }
            return Json(new { success = true, message = "Lấy dữ liệu CMIS sổ thành công!" + " Mã sổ: " + string.Join(",", lstLoi) + " đang ở trạng thái không phải xuất HHC!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JsonCapNhatDuLieuCmis(FindModelGcs findPar, string search, string maDvQly)
        {
            int nam = Convert.ToInt32(findPar.Nam);
            int thang = Convert.ToInt32(findPar.Thang);
            int ky = Convert.ToInt32(findPar.Ky);
            var status = "Đang chờ";

            var data = _repo.RepoBase<D_SOGCS>()
                .ManagerGetAllForIndex(findPar, search)
                .ToList();

            Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
            var lstLoi = new List<string>();

            if (thang == 0)
            {
                return Json(new { success = false, message = "Vui lòng nhập chọn tháng!"}, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in data)
            {
                string result = "";
                var dataFromCmis = ser.ReadHHCService(maDvQly, item.MA_SOGCS, ky, thang, nam, ref result);

                if (result == "Ok")
                {
                    var mapPath = Server.MapPath("~/TemplateFile/" + item + "-" + nam + "-" + thang + "-" + ky + ".xml");

                    dataFromCmis.WriteXml(mapPath);

                    var modelOrig = _repo.RepoBase<D_SOGCS>().GetOne(x => x.MA_DVIQLY == maDvQly && x.MA_SOGCS == item.MA_SOGCS && x.NGAY_GHI == findPar.NgayGhi);

                    if (modelOrig == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy bản ghi!" }, JsonRequestBehavior.AllowGet);
                    }
                    modelOrig.TRANG_THAI = status;
                    int kq = _repo.RepoBase<D_SOGCS>().Update(modelOrig);
                    return Json(new { success = true, message = "Lấy dữ liệu CMIS thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    lstLoi.Add(result);
                }
            }
            return Json(new { success = true, message = "Lấy dữ liệu CMIS không thành công!" + " Mã sổ: " + string.Join(",", lstLoi) + " đang ở trạng thái không phải xuất HHC!" }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult JsonLuuDuLieuCmis(FindModelGcs findPar, string search, string maDvQly)
        {
            int nam = Convert.ToInt32(findPar.Nam);
            int thang = Convert.ToInt32(findPar.Thang);
            int ky = Convert.ToInt32(findPar.Ky);

            var data = _repo.RepoBase<D_SOGCS>()
                .ManagerGetAllForIndex(findPar, search)
                .ToList();

            Service_GCS.Service_GCS ser = new Service_GCS.Service_GCS();
            var lstLoi = new List<string>();
            foreach (var items in data)
            {
                var mapPath = Server.MapPath("~/TemplateFile/" + items.MA_SOGCS + "-" + nam + "-" + thang + "-" + ky + ".xml");
                DataSet ds = new DataSet();
                ds.ReadXml(mapPath);

                var dataFromCmis = ser.WriteHHCService(maDvQly, items.MA_SOGCS, ky, thang, nam, ds);
                if (dataFromCmis == "Ok")
                {
                    return Json(new { success = true, message = "Đẩy dữ liệu về CMIS thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, message = "Đẩy dữ liệu về CMIS không thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}