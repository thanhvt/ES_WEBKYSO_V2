using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Administrator.Library.Models;
using ES_WEBKYSO.Controllers;
using ES_WEBKYSO.Models;

namespace ES_WEBKYSO.Common
{
    public class WriteLog
    {
        private Repository.UnitOfWork _uow; 
        public WriteLog(Repository.UnitOfWork uow)
        {
            _uow = uow;
        }
        public void WriteLogGcs(string logcategoryid, int? idlichgcs, string masogcs, int? ky, int? thang, int? nam, string content, int? userid, DateTime logdate, string mabangKe, int? countthuchien, string logstatus)
        {
            try
            {
                //var lstLog1 = Uow.RepoBase<LOG>().GetAll().ToList();
                var lstLog = _uow.RepoBase<LOG>().GetOne(x => x.LOG_CATEGORY_ID == logcategoryid && x.MA_SOGCS == masogcs);
                if (lstLog != null)
                {
                    //if (lstLog.LOG_CATEGORY_ID == "CMIS_LAYSO" || lstLog.LOG_CATEGORY_ID == "CMIS_HUYLAYSO")
                    //{
                    //    var lstLogLhs = _uow.RepoBase<LOG>().GetOne(x => x.LOG_CATEGORY_ID == logcategoryid
                    //                                                     && x.MA_SOGCS == masogcs);
                    //    lstLogLhs.LOG_CATEGORY_ID = logcategoryid;
                    //    lstLogLhs.ID_LICHGCS = idlichgcs;
                    //    lstLogLhs.MA_SOGCS = masogcs;
                    //    lstLogLhs.KY = ky;
                    //    lstLogLhs.THANG = thang;
                    //    lstLogLhs.NAM = nam;
                    //    lstLogLhs.CONTENTS = content;
                    //    lstLogLhs.UserId = userid;
                    //    lstLogLhs.LOG_DATE = logdate;
                    //    lstLogLhs.MA_LOAIBANGKE = mabangKe;
                    //    lstLogLhs.COUNT_THUCHIEN = countthuchien + 1;
                    //    lstLogLhs.LOG_STATUS_ID = logstatus;
                    //    _uow.RepoBase<LOG>().Update(lstLogLhs);

                    //}
                    //else
                    //{

                        lstLog.LOG_CATEGORY_ID = logcategoryid;
                        lstLog.ID_LICHGCS = idlichgcs;
                        lstLog.MA_SOGCS = masogcs;
                        lstLog.KY = ky;
                        lstLog.THANG = thang;
                        lstLog.NAM = nam;
                        lstLog.CONTENTS = content;
                        lstLog.UserId = userid;
                        lstLog.LOG_DATE = logdate;
                        lstLog.MA_LOAIBANGKE = mabangKe;
                        lstLog.COUNT_THUCHIEN = lstLog.COUNT_THUCHIEN + 1;
                        lstLog.LOG_STATUS_ID = logstatus;
                        _uow.RepoBase<LOG>().Update(lstLog);
                    //}
                }
                else
                {
                        lstLog = new LOG();
                        lstLog.LOG_CATEGORY_ID = logcategoryid;
                        lstLog.ID_LICHGCS = idlichgcs;
                        lstLog.MA_SOGCS = masogcs;
                        lstLog.KY = ky;
                        lstLog.THANG = thang;
                        lstLog.NAM = nam;
                        lstLog.CONTENTS = content;
                        lstLog.UserId = userid;
                        lstLog.LOG_DATE = logdate;
                        lstLog.MA_LOAIBANGKE = mabangKe;
                        lstLog.COUNT_THUCHIEN = countthuchien;
                        lstLog.LOG_STATUS_ID = logstatus;
                        _uow.RepoBase<LOG>().Create(lstLog);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}