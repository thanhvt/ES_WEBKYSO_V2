using ES_WEBKYSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ES_WEBKYSO.ModelParameter;
using Common.Helpers;
using ES_WEBKYSO.Areas.DoiSoatDuLieu.Models;
using ES_WEBKYSO.Common;

namespace ES_WEBKYSO.Repository.ServiceRepository.DoiSoatDuLieu
{
    public class GCS_CHISO_HHURepository<T> : BaseRepository<GCS_CHISO_HHU>
    {
        public GCS_CHISO_HHURepository(DbContext context, UnitOfWork repo) : base(context, repo)
        {
        }
        public override List<GCS_CHISO_HHU> GETALL()
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
        public override List<GCS_CHISO_HHU> ManagerGetAllForIndex(DoiSoatModel findModel, string orderKey, ref Paging page)
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

            string strAPIPath = Utility.getAPI_IMG();
            var m = System.Web.HttpContext.Current.Server.MapPath("~/");
            var fix = Utility.getAPI_PATH();
            //@"C:/inetpub/wwwroot/API_WebKySo/";
            //C:\inetpub\wwwroot\WebKySo\
            //C:\inetpub\wwwroot\API_WebKySo\WSGCS\GCSImages\2018_12_1\D25854412-2018-12-1\D25854412_57920161632002902_2018_12_1_PD25007654504001_VC.jpg
            //var a = UnitOfWork.RepoBase<GCS_CHISO_HHU>().GetOne(x => x.ID == 758);

            //var b = a.Replace(m, "").Replace("\\", "/");

            var lstMaSo = findModel.MaSos;

            var ret = GetAll(o =>
                   (qString == null || qString == ""
                   && (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
                && (lstMaSo.Count == 0 || lstMaSo.Any(x => x == o.MA_QUYEN))
                && (findModel.Ky == null || o.KY == findModel.Ky)
                && (findModel.Thang == null || o.THANG == findModel.Thang)
                && (findModel.Nam == null || o.NAM == findModel.Nam)
                && (findModel.LocCongTo == null || o.STR_CHECK_DSOAT == findModel.LocCongTo))
                , "MA_DVIQLY", ref page)
                .ToList()
                .Select(x => new GCS_CHISO_HHU
                {
                    ID = x.ID,
                    MA_DVIQLY = x.MA_DVIQLY,
                    TEN_KHANG = x.TEN_KHANG,
                    DIA_CHI = x.DIA_CHI,
                    TTR_MOI = x.TTR_MOI,
                    LOAI_BCS = x.LOAI_BCS,
                    MA_TRAM = x.MA_TRAM,
                    MA_KHANG = x.MA_KHANG,
                    MA_GC = x.MA_GC,
                    SERY_CTO = x.SERY_CTO,
                    CS_CU = x.CS_CU,
                    CS_MOI = x.CS_MOI,
                    SL_MOI = x.SL_MOI,
                    SLUONG_1 = x.SLUONG_1,
                    SLUONG_2 = x.SLUONG_2,
                    SLUONG_3 = x.SLUONG_3,
                    ANH_GCS = strAPIPath + x.ANH_GCS.Replace(m, "").Replace("\\","/").Replace(fix, ""),
                    STR_CHECK_DSOAT = x.STR_CHECK_DSOAT
                })
                .ToList();

            return ret;
        }

        public override List<GCS_CHISO_HHU> ManagerGetAllForIndex(DoiSoatModel findModel)
        {
            var lstMaSo = findModel.MaSos;
            var ret = GetAll(o =>
                   (findModel.MaDonVi == null || o.MA_DVIQLY == findModel.MaDonVi)
                && (lstMaSo.Count == 0 || lstMaSo.Any(x => x == o.MA_QUYEN))
                && (findModel.Ky == null || o.KY == findModel.Ky)
                && (findModel.Thang == null || o.THANG == findModel.Thang)
                && (findModel.Nam == null || o.NAM == findModel.Nam)
                && (findModel.LocCongTo == null || o.STR_CHECK_DSOAT == findModel.LocCongTo)).ToList()
                .Select(x => new GCS_CHISO_HHU
                {
                    ID = x.ID,
                    MA_DVIQLY = x.MA_DVIQLY,
                    TEN_KHANG = x.TEN_KHANG,
                    DIA_CHI = x.DIA_CHI,
                    TTR_MOI = x.TTR_MOI,
                    LOAI_BCS = x.LOAI_BCS,
                    MA_TRAM = x.MA_TRAM,
                    MA_KHANG = x.MA_KHANG,
                    MA_GC = x.MA_GC,
                    SERY_CTO = x.SERY_CTO,
                    CS_CU = x.CS_CU,
                    CS_MOI = x.CS_MOI,
                    SL_MOI = x.SL_MOI,
                    SLUONG_1 = x.SLUONG_1,
                    SLUONG_2 = x.SLUONG_2,
                    SLUONG_3 = x.SLUONG_3,
                    STR_CHECK_DSOAT = x.STR_CHECK_DSOAT
                })
                .ToList();

            return ret;
        }
    }
}