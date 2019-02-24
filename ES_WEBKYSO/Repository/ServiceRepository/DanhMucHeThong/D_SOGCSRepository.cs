using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository
{
    public class D_SOGCSRepository<T> : BaseRepository<D_SOGCS>
    {
        public D_SOGCSRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }
        public override List<D_SOGCS> GETALL()
        {
            try
            {
                var ret = GetAll().ToList();
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override List<D_SOGCS> ManagerGetAllForIndex(string orderKey, ref Paging page)
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
                    (qIntSearch == null || qIntSearch.Value == o.SO_KY || qIntSearch.Value == o.NGAY_GHI)
                        &&
                    (qString == null || qString == "" || o.MA_SOGCS.Contains(qString) || o.TEN_SOGCS.Contains(qString))
                , page.OrderKey, ref page)
                .ToList()
                .Select(x => new D_SOGCS()
                {
                    ID_SOGCS = x.ID_SOGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    TEN_SOGCS = x.TEN_SOGCS,
                    SO_KY = x.SO_KY,
                    LOAI_SOGCS = x.LOAI_SOGCS,
                    NGAY_GHI = x.NGAY_GHI,
                    HINH_THUC = x.HINH_THUC,
                    TINH_TRANG = x.TINH_TRANG
                })
                .ToList();

            return ret;
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
                        (findModel.Ky == null || o.SO_KY == findModel.Ky)
                        && (findModel.NgayGhi == null || o.NGAY_GHI == findModel.NgayGhi)
                        && (findModel.LoaiSo == null || o.LOAI_SOGCS == findModel.LoaiSo)
                        && (findModel.HinhThuc == null || o.HINH_THUC == findModel.HinhThuc)
                        && (findModel.TrangThai == null || o.TRANG_THAI == findModel.TrangThai)
                        && (findModel.MaSo == null || o.MA_SOGCS == findModel.MaSo)
                        && (findModel.TenSo == null || o.TEN_SOGCS == findModel.TenSo)
                        && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
                        && (qIntSearch == null || qIntSearch.Value == o.SO_KY || qIntSearch.Value == o.NGAY_GHI || qIntSearch.Value == o.TINH_TRANG
                       || qString == null || qString == "" || o.MA_DVIQLY.Contains(qString) || o.MA_SOGCS.Contains(qString) || o.TEN_SOGCS.Contains(qString) || o.HINH_THUC.Contains(qString))
                , "ID_SOGCS", ref page)
                .ToList()
                .Select(x => new D_SOGCS
                {
                    ID_SOGCS = x.ID_SOGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    TEN_SOGCS = x.TEN_SOGCS,
                    SO_KY = x.SO_KY,
                    TINH_TRANG = x.TINH_TRANG,
                    LOAI_SOGCS = x.LOAI_SOGCS,
                    NGAY_GHI = x.NGAY_GHI,
                    HINH_THUC = x.HINH_THUC,
                    TRANG_THAI = x.TRANG_THAI
                    //STATUS_NVK = x.STATUS_NVK,
                    //STATUS_DTK = x.STATUS_DTK,
                    //STATUS_DHK = x.STATUS_DHK
                })
                .ToList();

            return ret;
        }

        public override List<D_SOGCS> ManagerGetAllForIndex(FindModelGcs findModel, string qString)
        {

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
                            && (findModel.Ky == null || o.SO_KY == findModel.Ky)
                            && (findModel.NgayGhi == null || o.NGAY_GHI == findModel.NgayGhi)
                            && (findModel.LoaiSo == null || o.LOAI_SOGCS == findModel.LoaiSo)
                            && (findModel.HinhThuc == null || o.HINH_THUC == findModel.HinhThuc)
                            && (findModel.TrangThai == null || o.TRANG_THAI == findModel.TrangThai)

                            &&(qString == null || qString == ""
                            && (findModel.MaSo == null || o.MA_SOGCS == findModel.MaSo)
                            && (findModel.TenSo == null || o.TEN_SOGCS == findModel.TenSo)
                            && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)))
                .ToList()
                .Select(x => new D_SOGCS
                {
                    ID_SOGCS = x.ID_SOGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    TEN_SOGCS = x.TEN_SOGCS,
                    SO_KY = x.SO_KY,
                    TINH_TRANG = x.TINH_TRANG,
                    LOAI_SOGCS = x.LOAI_SOGCS,
                    NGAY_GHI = x.NGAY_GHI,
                    HINH_THUC = x.HINH_THUC,
                    TRANG_THAI = x.TRANG_THAI
                })
                .ToList();

            return ret;
        }
    }
}