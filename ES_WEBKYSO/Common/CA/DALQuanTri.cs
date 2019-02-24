using System;
using System.Data;
using System.Data.SqlClient;

namespace ES_WEBKYSO.Common.CA
{
    public partial class DALQuanTri
    {
        #region Private Members & Constructors
        /// <summary>
        /// SQL Connection
        /// </summary>
        private DAL_SqlConnector sc;

        /// <summary>
        /// Constructs new SqlDataProvider instance use default connection
        /// </summary>
        public DALQuanTri()
        {
            sc = new DAL_SqlConnector();
        }

        /// <summary>
        /// Constructs new SqlDataProvider instance with specific connection
        /// </summary>
        public DALQuanTri(string strConn)
        {
            sc = new DAL_SqlConnector(strConn);
        }

        /// <summary>
        /// Constructs new SqlDataProvider instance with specific connection
        /// </summary>
        public DALQuanTri(string sComputerName, string sDBName, string sUserName, string sPassword)
        {
            sc = new DAL_SqlConnector(sComputerName, sDBName, sUserName, sPassword);
        }
        #endregion

        #region FL_File
        public bool FL_File_SelectForAllowSign_Array_New(string arrFileID, string programName, string userName,
            ref DataTable dtFile)
        {
            //dtFile = new DataTable("FL_File_SelectForAllowSign_Array");

            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_SelectForAllowSign_Array_New";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@arrFileID", arrFileID);
                cmd.Parameters.AddWithValue("@OKtoSign", 0).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@ProgramName", programName);
                cmd.Parameters.AddWithValue("@UserProgName", userName);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtFile);
                bool bOK = Convert.ToBoolean(cmd.Parameters["@OKtoSign"].Value);

                sqlcon.Close();

                return bOK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public bool FL_File_SelectForSaveSign(int id_StatusLog)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_SelectForSaveSign";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@ID_StatusLog", id_StatusLog);
                cmd.Parameters.AddWithValue("@OKtoSave", 0).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                bool bOK = Convert.ToBoolean(cmd.Parameters["@OKtoSave"].Value);

                sqlcon.Close();

                return bOK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public void FL_File_UpdateForLogSign(int fileID, string certSerial, DateTime signTime, int verify, int signatureType,
            int action, string backupPath, string reason, string programName, string userName)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_UpdateForLogSign";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.Add(new SqlParameter("@FileID", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileID));
                cmd.Parameters.Add(new SqlParameter("@CertSerial", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, certSerial));
                cmd.Parameters.Add(new SqlParameter("@SignTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, signTime));
                cmd.Parameters.Add(new SqlParameter("@Verify", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, verify));
                cmd.Parameters.Add(new SqlParameter("@SignatureTypeID", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, signatureType));
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.TinyInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, action));
                cmd.Parameters.Add(new SqlParameter("@BackupPath", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, backupPath));
                cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, reason));
                cmd.Parameters.Add(new SqlParameter("@ProgramName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, programName));
                cmd.Parameters.Add(new SqlParameter("@UserProgName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, userName));

                cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public void FL_File_UpdateStatus(int fileID, int status, string reason, string programName, string userName)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_UpdateStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.Add(new SqlParameter("@FileID", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileID));
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, status));
                cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, reason));
                cmd.Parameters.Add(new SqlParameter("@ProgramName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, programName));
                cmd.Parameters.Add(new SqlParameter("@UserProgName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, userName));

                cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public DataTable FL_File_InsertSelectNewFile(string programName, string userName, int fileTypeID, string fileMaDV,
            DateTime fileDate, string fileName, string description)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_InsertSelectNewFile";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.Add(new SqlParameter("@ProgramName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, programName));
                cmd.Parameters.Add(new SqlParameter("@UserProgName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, userName));
                cmd.Parameters.Add(new SqlParameter("@FileTypeID", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileTypeID));
                cmd.Parameters.Add(new SqlParameter("@FileMaDV", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileMaDV));
                cmd.Parameters.Add(new SqlParameter("@FileDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileDate));
                cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileName));
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, description));

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public DataTable FL_File_SelectByFileID(int fileID)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_SelectByFileID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@FileID", fileID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public void FL_File_UpdateStatus_WithHash(int fileID, int status, byte[] fileHash, string reason,
            string programName, string userName)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_File_UpdateStatus_WithHash";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.Add(new SqlParameter("@FileID", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileID));
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, status));
                cmd.Parameters.Add(new SqlParameter("@FileHash", SqlDbType.VarBinary, fileHash.Length, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, fileHash));
                cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, reason));
                cmd.Parameters.Add(new SqlParameter("@ProgramName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, programName));
                cmd.Parameters.Add(new SqlParameter("@UserProgName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, userName));

                cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }
        #endregion

        #region FL_FileType
        public DataTable FL_FileType_QuyenXacNhan_CheckByFileID_CertID(int fileID, string certSerial)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FL_FileType_QuyenXacNhan_CheckByFileID_CertID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@FileID", fileID);
                cmd.Parameters.AddWithValue("@CertSerial", certSerial);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }
        #endregion

        #region CA_Certificate
        public DataTable CA_Certificate_SelectChainByCertProg(string programName, string userName, string certSerial)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "CA_Certificate_SelectChainByCertProg";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@ProgramName", programName);
                cmd.Parameters.AddWithValue("@UserProgName", userName);
                cmd.Parameters.AddWithValue("@Serial", certSerial);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }
        #endregion

        #region CA_DataSign

        public DataTable CA_DataSign_SelectByKey(string key)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "CA_DataSign_SelectByKey";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@Key", key);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public DataTable CA_DataSign_FL_File_SelectByKey(string key)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "CA_DataSign_FL_File_SelectByKey";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@Key", key);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public DataTable CA_DataSignForDB_SelectByKeyObj(string key, int columnType, string obj, int typeSign)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "CA_DataSignForDB_SelectByKeyObj";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@Key", key);
                cmd.Parameters.AddWithValue("@ColumnType", columnType);
                cmd.Parameters.AddWithValue("@Obj", obj);
                cmd.Parameters.AddWithValue("@TypeSign", typeSign);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(cmd.CommandText);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

        }

        public void CA_DataSign_CA_DataSignForDB_DeleteByKey(string key)
        {
            SqlConnection sqlcon = sc.GetConnection();
            sqlcon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "CA_DataSign_CA_DataSignForDB_DeleteByKey";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlcon;

                cmd.Parameters.AddWithValue("@Key", key);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }
        #endregion
    }
}