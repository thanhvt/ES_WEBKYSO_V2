using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.DanhMucHeThong
{
    public class D_IMEIRepository<T> : BaseRepository<D_IMEI>
    {
        public D_IMEIRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }

        public override List<D_IMEI> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
        {

            if (orderKey.EndsWith("String"))
            {
                orderKey = orderKey.Replace("String", "");
                page.OrderKey = orderKey;
            }


            var pagingOrg = page;

            var qString = pagingOrg.Key;

            int qInt;
            int? qIntSearch = null;
            if (int.TryParse(qString, out qInt))
            {
                qIntSearch = qInt;
            }

            DateTime qDateTime;
            DateTime? qDateTimeSearch = null;
            if (DateTime.TryParse(qString, out qDateTime))
            {
                qDateTimeSearch = qDateTime;
            }

            // qString, qInt, qDateTime

            var ret = GetAll(o =>
                   (qString == null || qString == ""
                   && (findModel.LoaiMay == null || o.LOAI_MAY == findModel.LoaiMay)
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
                   && (findModel.NguoiCap == null || o.NGUOI_CAP == findModel.NguoiCap)
                   && (findModel.NguoiDung == null || o.NGUOI_DUNG == findModel.NguoiDung))
                   &&
                    (qDateTimeSearch == null
                   && (findModel.NgayCapString == null || o.NGAY_CAP == findModel.NgayCapString))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new D_IMEI
                {
                    ID = x.ID,
                    IMEI = x.IMEI,
                    MA_DVIQLY = x.MA_DVIQLY,
                    LOAI_MAY = x.LOAI_MAY,
                    NGAY_CAP = x.NGAY_CAP,
                    NgayCapString = x.NGAY_CAP?.ToString("dd'/'MM'/'yyyy") ?? "",
                    NGUOI_CAP = x.NGUOI_CAP,
                    NGUOI_DUNG = x.NGUOI_DUNG
                })
                .ToList();

            return ret;
        }

        //private Dictionary<string, string> _plantKind;
        //public Dictionary<string, string> GetPlantKind()
        //{
        //    return _plantKind ?? (_plantKind = new Dictionary<string, string>
        //           {
        //               {"ccgt", "Nhà máy tua bin khí chu trình hỗn hợp"},
        //               {"thermal", "Nhà máy nhiệt điện"},
        //               {"hydro", "Nhà máy thuỷ điện"}
        //           });
        //}
    }
}