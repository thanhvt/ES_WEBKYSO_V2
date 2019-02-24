using System;
using System.Data;

namespace ES_WEBKYSO.Common.CA
{
    public partial class BUSQuanTri
    {
        private DALQuanTri _dal;

        #region Constructor
        /// <summary>
        /// Khởi tạo dùng kết nối mặc định
        /// </summary>
        public BUSQuanTri()
        {
            _dal = new DALQuanTri();
        }

        /// <summary>
        /// Khởi tạo dùng kết nối tạm (đến database khác)
        /// </summary>
        /// <param name="strConn"></param>
        public BUSQuanTri(string strConn)
        {
            _dal = new DALQuanTri(strConn);
        }

        /// <summary>
        /// Khởi tạo dùng kết nối tạm (đến database khác)
        /// </summary>
        /// <param name="sComputerName"></param>
        /// <param name="sDBName"></param>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        public BUSQuanTri(string sComputerName, string sDBName, string sUserName, string sPassword)
        {
            _dal = new DALQuanTri(sComputerName, sDBName, sUserName, sPassword);
        }
        #endregion

        #region FL_File
        /// <summary>
        /// Kiểm tra trạng thái nhiều file có cho phép ký hay không và ghi log. Trả về thông tin file và ID_Log phiên ký.
        /// Bảng kết quả bao gồm: FilePath, FileData, OKtoSign, QuyenUnit_Type, ID_StatusLog
        /// </summary>
        public bool FL_File_SelectForAllowSign_Array_New(string arrFileID, string programName, string userName,
            ref DataTable dtFile)
        {
            bool bOK = _dal.FL_File_SelectForAllowSign_Array_New(arrFileID, programName, userName, ref dtFile);
            if (dtFile.Rows.Count < 1)
                throw new Exception("WS_Không tìm thấy file.");

            return bOK;
        }

        /// <summary>
        /// Xét phiên log file xem có cho phép lưu file hay không.
        /// </summary>
        public bool FL_File_SelectForSaveSign(int id_StatusLog)
        {
            return _dal.FL_File_SelectForSaveSign(id_StatusLog);
        }

        /// <summary>
        /// Cập nhật log ký/xóa chữ ký và thiết lập trạng thái về LuuFile
        /// </summary>
        public void FL_File_UpdateForLogSign(int fileID, string certSerial, DateTime signTime, int verify, int signatureType,
            int action, string backupPath, string reason, string programName, string userName)
        {
            _dal.FL_File_UpdateForLogSign(fileID, certSerial, signTime, verify, signatureType, action, backupPath, reason, programName, userName);
        }

        /// <summary>
        /// Update trường Status của bảng FL_File
        /// </summary>
        public void FL_File_UpdateStatus(int fileID, int status, string reason, string programName, string userName)
        {
            _dal.FL_File_UpdateStatus(fileID, status, reason, programName, userName);
        }

        /// <summary>
        /// Lấy thông tin file theo FileID
        /// </summary>
        public DataTable FL_File_SelectByFileID(int fileID)
        {
            return _dal.FL_File_SelectByFileID(fileID);
        }

        /// <summary>
        /// Lưu chuỗi hash và cập nhật trạng thái sau khi tạo file.
        /// </summary>
        public void FL_File_UpdateStatus_WithHash(int fileID, int status, byte[] fileHash, string reason,
            string programName, string userName)
        {
            _dal.FL_File_UpdateStatus_WithHash(fileID, status, fileHash, reason, programName, userName);
        }

        /// <summary>
        /// Tạo mới file trong hệ thống
        /// </summary>
        public DataTable FL_File_InsertNewFile(string programName, string userName, int fileTypeID, string fileMaDV,
            DateTime fileDate, string fileName, string description)
        {
            return _dal.FL_File_InsertSelectNewFile(programName, userName, fileTypeID, fileMaDV, fileDate, fileName, description);
        }
        #endregion

        #region FL_FileType
        /// <summary>
        /// Toantk 21/10/2015: Kiểm tra xem chứng thư có quyền Xác nhận hay không
        /// </summary>
        public bool FL_FileType_QuyenXacNhan_CheckByFileID_CertID(int fileID, string certSerial)
        {
            DataTable dt = _dal.FL_FileType_QuyenXacNhan_CheckByFileID_CertID(fileID, certSerial);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region CA_Certificate
        /// <summary>
        /// Lấy chuỗi liên kết Chứng thư số - Người dùng - Hệ thống 
        /// </summary>
        public DataTable CA_Certificate_SelectChainByCertProg(string programName, string userName, string certSerial)
        {
            return _dal.CA_Certificate_SelectChainByCertProg(programName, userName, certSerial);
        }
        #endregion

        #region CA_DataSign
        /// <summary>
        /// Lấy thông tin của liên kết key - FileID hoặc FilePath theo key
        /// </summary>
        public DataTable CA_DataSign_SelectByKey(string key)
        {
            return _dal.CA_DataSign_SelectByKey(key);
        }

        /// <summary>
        /// Lấy thông tin File từ dbo.CA trong bảng FL_File
        /// </summary>
        public DataTable CA_DataSign_FL_File_SelectByKey(string key)
        {
            return _dal.CA_DataSign_FL_File_SelectByKey(key);
        }

        /// <summary>
        /// Lấy thông tin của liên kết key - FileID hoặc FilePath theo key
        /// </summary>
        public DataTable CA_DataSignForDB_SelectByKeyObj(string key, int columnType, string obj, int typeSign)
        {
            return _dal.CA_DataSignForDB_SelectByKeyObj(key, columnType, obj, typeSign);
        }

        /// <summary>
        /// Xóa thông tin phục vụ ký
        /// </summary>
        public void CA_DataSign_CA_DataSignForDB_DeleteByKey(string key)
        {
            _dal.CA_DataSign_CA_DataSignForDB_DeleteByKey(key);
        }
        #endregion
    }
}