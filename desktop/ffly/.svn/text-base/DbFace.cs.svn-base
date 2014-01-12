/******************************************************************************\
 * DbFace - Database interface methods with delayed, persistent connection
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * This file is part of FFlight, which is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License as 
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 * 
 * FFlight is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
 * details (license.txt).
 * 
 * You should have received a copy of the GNU General Public License along with
 * FFlight.  If not, see <http://www.gnu.org/licenses/>.
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace ffly
{
    public class DbFace : IDisposable
    {
        // The database connection info
        protected const string connection = "Database=fflights;Server=mysql.existencia.org;Data Source=mysql.existencia.org;User Id=fflights_athome;Password=ourpass;Pooling=false;Connection Timeout=10;Protocol=socket;Port=3306;";
        // The last error from a db operation
        protected string lastError;
        // The saved persistent connection
        protected MySqlConnection savedconn;

        // a new connection-- but doesn't connect until use
        public DbFace() {
            lastError = "";
        }

        // ensure disposal on delete
        ~DbFace()
        {
            Dispose();
        }

        #region IDisposable Members

        // close any saved connection
        public void Dispose()
        {
            if (savedconn != null)
            {
                savedconn.Close();
                savedconn.Dispose();
                savedconn = null;
            }
        }

        #endregion

        // get the last error from any db operation
        public string LastError {
            get
            {
                return lastError;
            }
        }

        // Get a list of rows, each a hash of {column name => value}
        public List<Dictionary<string, object>> AssocEnumerate(string sql)
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();

                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                // loop through the rows
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    // loop through the columns
                    for (int ii = 0; ii < reader.FieldCount; ii++)
                        row[reader.GetName(ii)] = reader.GetValue(ii);

                    result.Add(row);
                }

                return result;
            }
            catch (Exception ex)
            {
                lastError = ex.Message + " + connection is " + conn.State;
				Log.Debug(lastError);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return null;
        }

        // Get a single value, as the result of a sql query; use for any value types
        public T? GetValue<T>(string sql)
            where T : struct
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
                
                // return the first element
                if (reader.Read())
                {
                    if (reader.IsDBNull(0))
                        return null;

                    return (T)reader.GetValue(0);
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message + " + connection is " + conn.State;
				Log.Debug(lastError);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return null;
        }

        // get a single non-value-type as the result of a sql query;
        //   note that GetClass must be used for strings
        public T GetClass<T>(string sql)
            where T : class
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader.IsDBNull(0))
                        return null;

                    return (T)reader.GetValue(0);
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message + " + connection is " + conn.State;
				Log.Debug(lastError);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return null;
        }

        // Execute a sql query
        // if lastinsert is true, the last inserted id is returned; otherwise the number of rows affected
        public uint Execute(string sql, bool lastinsert)
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand command = new MySqlCommand(sql, conn);

            try
            {
                int affected = command.ExecuteNonQuery();
                if (lastinsert)
                    return (uint)command.LastInsertedId;
                else
                    return (uint)affected;
            }
            catch (Exception ex)
            {
                lastError = ex.Message + " + connection is " + conn.State;
				Log.Debug(lastError);
            }

            return 0;
        }

        // Get the connection, or open it as needed
        public MySqlConnection GetConnection()
        {
            if (savedconn != null && savedconn.State == System.Data.ConnectionState.Open)
                return savedconn;

            try
            {
                savedconn = new MySqlConnection(connection);
                savedconn.Open();
                lastError = "";
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
				Log.Debug(lastError);
            }

            return savedconn;
        }

        // Always use single quotes in strings and then apply EscapeSingles to unprotected string values
        public static string EscapeSingles(string input)
        {
            Regex regex = new Regex("[']");
            return regex.Replace(input, "\\'");
        }
    }
}
