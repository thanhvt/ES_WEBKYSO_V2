using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace WSGCS.SQLite
{
    public class SQLiteDatabase : IDisposable
    {
        protected String dbConnection;
        private SQLiteConnection cnn;
        private SQLiteTransaction trans;

        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SQLiteDatabase(String inputFile)
        {
            dbConnection = String.Format(@"Data Source={0};Version=3;Synchronous=Normal;UseUTF8Encoding=True;", inputFile);//PRAGMA auto_vacuum = 1;

            cnn = new SQLiteConnection(dbConnection);
            //cnn.ChangePassword("nghind");
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SQLiteDatabase(Dictionary<String, String> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            dbConnection = str + "PRAGMA auto_vacuum = 1;";
        }



        /// <summary>
        /// Escape string sequence for text value to avoid SQL injection or invalid SQL syntax to be constructed.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Escape(string data)
        {
            data = data.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            return data;
        }

        /// <summary>
        /// Create new sqlite database
        /// </summary>
        /// <param name="strPathDB"></param>
        /// <returns></returns>
        public string CreateDBSQLite(string strPathDB)
        {
            try
            {
                SQLiteConnection.CreateFile(strPathDB);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CheckColumnExist(string table_name, string column)
        {
            string sql_table_info = string.Format("PRAGMA table_info({0})", table_name);
            try
            {
                DataTable dtInfo = ExecuteQuery(sql_table_info);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    foreach (DataRow drInfo in dtInfo.Rows)
                    {
                        if (drInfo["name"].ToString() == column)
                        {
                            return "exist";
                        }
                    }
                }
                return "not_exist";
            }
            catch (Exception ex)
            {
                return "error: " + ex.Message;
            }
        }

        public string AddColIfNotExist(string table_name, string column, string define_col)
        {
            string check_col_exist = CheckColumnExist(table_name, column);
            if (check_col_exist != "exist")
            {
                // Tạo cột
                string sql_insert_col = string.Format("ALTER TABLE {0} ADD COLUMN {1} {2}", table_name, column, define_col);
                int result = ExecuteNonQuery(sql_insert_col);

                if (result == 0) //success
                {
                    return "exist";
                }
                else
                {
                    return "create column fail";
                }
            }
            return "exist";
        }

        /// <summary>
        /// Kiểm tra bảng tồn tại
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public string CheckTableExist(string table_name)
        {
            string sql_check = "SELECT name FROM sqlite_master WHERE name='" + table_name + "'";
            try
            {
                string table_exist = ExecuteScalar(sql_check, null);
                if (table_exist != null)
                {
                    return "exist";
                }
                return "not_exist";
            }
            catch (Exception ex)
            {
                return "error: " + ex.Message;
            }
        }

        public DataTable ExecuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            using (SQLiteCommand cmd = new SQLiteCommand(cnn))
            {
                cmd.CommandText = sql;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            using (var cmd = new SQLiteCommand(cnn))
            {
                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandText = sql;
                var rowsUpdated = cmd.ExecuteNonQuery();

                return rowsUpdated;
            }
        }
        public int ExecuteNonQuery(string sql, SQLiteCommand cmd)
        {
            try
            {
                if (cmd == null)
                    cmd = new SQLiteCommand(cnn);
                else if (cmd.Connection == null)
                    cmd.Connection = cnn;

                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandText = sql;
                int rowsUpdated = cmd.ExecuteNonQuery();

                return rowsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public string ExecuteScalar(string sql)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = new SQLiteCommand(cnn);
                cmd.CommandText = sql;
                object value = cmd.ExecuteScalar();

                if (value != null)
                {
                    return value.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }

        }
        public string ExecuteScalar(string sql, SQLiteCommand cmd)
        {
            try
            {
                if (cmd == null)
                    cmd = new SQLiteCommand(cnn);
                else
                    cmd.Connection = cnn;

                cmd.CommandText = sql;
                object value = cmd.ExecuteScalar();

                if (value != null)
                {
                    return value.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
        }

        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where), null);
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }
        
        public string Insert(string tableName, List<KeyValuePair<String, SQLiteParameter>> list_param)
        {
            string columns = "";
            String values = "";
            SQLiteCommand command = null;

            try
            {
                command = new SQLiteCommand();
                foreach (KeyValuePair<String, SQLiteParameter> param in list_param)
                {
                    columns += String.Format(",{0}", param.Key.ToString());
                    values += String.Format(",{0}", "@" + param.Key.ToString());
                    param.Value.ParameterName = "@" + param.Key.ToString();
                    command.Parameters.Add(param.Value);
                }
                if (columns.Length == 0 || values.Length == 0)
                {
                    throw new Exception("Không có dữ liệu để chèn");
                }
                columns = columns.Substring(1);
                values = values.Substring(1);
                //command.CommandText = String.Format("insert into {0}({1}) values({2});", tableName, columns, values);


                return this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values), command) + "";
            }
            catch (Exception fail)
            {
                return fail.Message;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
        }

        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.ExecuteQuery("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearTable(String table)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sqlite Connection
        public string OpenConn()
        {
            try
            {
                if (dbConnection == null || dbConnection.ToString().Length == 0)
                {
                    throw new Exception("NO CONNECTION STRING");
                }
                if (cnn == null)
                {
                    cnn = new SQLiteConnection(dbConnection);
                }
                if (cnn.State == ConnectionState.Broken)
                {
                    cnn.Close();
                }
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                return null;
            }
            catch (Exception ex)
            {
                if (cnn != null)
                {
                    cnn.Dispose();
                }
                return ex.Message;
            }
        }
        public void CloseConn()
        {
            try
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
            catch
            {
                if (cnn != null)
                {
                    cnn.Dispose();
                }
            }
        }
        public void Dispose()
        {
            CloseConn();
            if (cnn != null)
            {
                cnn.Dispose();
            }
            //common.ClearMemory();
        }

        // Sqlite Transaction
        public SQLiteTransaction GetTransaction()
        {
            return trans;
        }
        public void BeginTransaction()
        {
            trans = null;
            trans = cnn.BeginTransaction();
        }
        public void CommitTransaction()
        {
            try
            {
                trans.Commit();

            }
            catch
            {

            }
            finally
            {
                trans.Dispose();
            }

        }
        public void RollbackTransaction()
        {
            try
            {
                trans.Rollback();

            }
            catch //(SQLiteException ex2)
            {

            }
            finally
            {
                trans.Dispose();
            }
        }





    }
}