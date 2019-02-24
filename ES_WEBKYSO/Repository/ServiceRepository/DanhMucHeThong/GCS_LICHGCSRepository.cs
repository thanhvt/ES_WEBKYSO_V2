using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Helpers;
using ES_WEBKYSO.ModelParameter;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Repository.ServiceRepository.DanhMucHeThong
{
    public class GCS_LICHGCSRepository<T> : BaseRepository<GCS_LICHGCS>
    {
        public GCS_LICHGCSRepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {

        }
        public override List<GCS_LICHGCS> GETALL()
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
        public override List<GCS_LICHGCS> ManagerGetAllForIndex(FindModelGcs findModel, string orderKey, ref Paging page)
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
            //(qIntSearch == null || qIntSearch.Value == o.NGAY_GHI)
            //|| (qIntSearch.Value == o.KY)
            //|| qIntSearch.Value == o.THANG || qIntSearch.Value == o.NAM
            //&&
            //(qString == null || qString == "" || qString.Equals(o.MA_SOGCS)
            //                 || qString.Equals(o.TEN_SOGCS) || qString.Equals(o.HINH_THUC)
            //                 || qString.Equals(o.MA_DOIGCS) || qString.Equals(o.HasUserProfile.FullName))
            (((qString == null || qString == "" || o.MA_SOGCS.Equals(qString) || o.MA_DVIQLY.Equals(qString)
                              || o.TEN_SOGCS.Equals(qString) || o.HINH_THUC.Equals(qString) || o.HasUserProfile.FullName.Equals(qString))
                              && ((qIntSearch == null || qIntSearch.Value == o.NGAY_GHI || qIntSearch.Value == o.KY || qIntSearch.Value == o.THANG
                              || qIntSearch.Value == o.NAM)))
            &&
                  (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
                  && (findModel.NgayGhi == null || o.NGAY_GHI == findModel.NgayGhi)
                  && (findModel.MaSo == null || o.MA_SOGCS == findModel.MaSo)
                  && (findModel.Ky == null || o.KY == findModel.Ky)
                  && (findModel.Thang == null || o.THANG == findModel.Thang)
                  && (findModel.Nam == null || o.NAM == findModel.Nam)
                  && (findModel.MaDoi == null || o.MA_DOIGCS == findModel.MaDoi))
                ////&& (findModel.TrangThai == null || o.STATUS == findModel.TrangThai || o.STATUS_PC == findModel.TrangThai || o.STATUS_CNCS == findModel.TrangThai || o.STATUS_DVCM == findModel.TrangThai)
                //
                //&& (findModel.USERID == null || o.USERID == findModel.USERID)

                //&& (findModel.TenSo == null || o.TEN_SOGCS == findModel.TenSo))
                , "ID_LICHGCS", ref page)
                .ToList()
                .Select(x => new GCS_LICHGCS
                {
                    ID_LICHGCS = x.ID_LICHGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    TEN_SOGCS = x.TEN_SOGCS,
                    HINH_THUC = x.HINH_THUC,
                    NGAY_GHI = x.NGAY_GHI,
                    KY = x.KY,
                    THANG = x.THANG,
                    NAM = x.NAM,
                    MA_DOIGCS = x.MA_DOIGCS,
                    Ten_Doi = x.HasD_DOIGCS == null ? "" : x.HasD_DOIGCS.TEN_DOI,
                    STATUS = x.STATUS,
                    STATUS_PC = x.STATUS_PC,
                    STATUS_CNCS = x.STATUS_CNCS,
                    STATUS_NVK = x.STATUS_NVK,
                    STATUS_DTK = x.STATUS_DTK,
                    STATUS_DHK = x.STATUS_DHK,
                    STATUS_DVCM = x.STATUS_DVCM,
                    USERID = x.USERID,
                    FullName = x.HasUserProfile == null ? "" : x.HasUserProfile.FullName,
                    NGAY_TAO = x.NGAY_TAO,
                    NGAY_SUA = x.NGAY_SUA,
                    NGUOI_SUA = x.NGUOI_SUA,
                })
                .ToList();

            return ret;
        }
        public List<GCS_LICHGCS> ManagerGetAllForIndex(string MaSoGcs, string orderKey, ref Paging page)
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
                (MaSoGcs == null || o.MA_SOGCS == MaSoGcs)

                , "ID_LICHGCS", ref page)
                .ToList()
                .Select(x => new GCS_LICHGCS
                {
                    ID_LICHGCS = x.ID_LICHGCS,
                    MA_DVIQLY = x.MA_DVIQLY,
                    MA_SOGCS = x.MA_SOGCS,
                    KY = x.KY,
                    THANG = x.THANG,
                    NAM = x.NAM,
                    MA_DOIGCS = x.MA_DOIGCS,
                    Ten_Doi = x.HasD_DOIGCS == null ? "" : x.HasD_DOIGCS.TEN_DOI,
                    STATUS = x.STATUS,
                    STATUS_PC = x.STATUS_PC,
                    STATUS_CNCS = x.STATUS_CNCS,
                    STATUS_DTK = x.STATUS_DTK,
                    STATUS_DHK = x.STATUS_DHK,
                    USERID = x.USERID,
                    NGAY_TAO = x.NGAY_TAO,
                    NGAY_SUA = x.NGAY_SUA,
                    NGUOI_SUA = x.NGUOI_SUA,
                })
                .ToList();

            return ret;
        }
    }
}