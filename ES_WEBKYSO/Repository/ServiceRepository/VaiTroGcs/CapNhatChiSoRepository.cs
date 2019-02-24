using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.VaiTroGcs
{
    public class CapNhatChiSoRepository<T> : BaseRepository<D_SOGCS>
    {
        public CapNhatChiSoRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }
        public override List<D_SOGCS> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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
                    (qIntSearch == null)
                        && (findModel.MaSo == null || o.MA_SOGCS == findModel.MaSo)
                        && (findModel.NgayGhi == null || o.NGAY_GHI == findModel.NgayGhi)
                        && (findModel.LoaiSo == null || o.LOAI_SOGCS == findModel.LoaiSo)
                        && (findModel.HinhThuc == null || o.HINH_THUC == findModel.HinhThuc)
                        &&
                   (qString == null || qString == ""
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new D_SOGCS
                {
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    TEN_SOGCS = x.TEN_SOGCS,
                    SO_KY = x.SO_KY,
                    TINH_TRANG = x.TINH_TRANG,
                    LOAI_SOGCS = x.LOAI_SOGCS,
                    NGAY_GHI = x.NGAY_GHI,
                    HINH_THUC = x.HINH_THUC
                })
                .ToList();

            return ret;
        }
    }
}