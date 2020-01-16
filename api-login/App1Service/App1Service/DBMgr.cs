using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App1Service
{
    public class DBMgr
    {
        private static string constr = "Server=127.0.0.1;Port=3306;Database=service;Uid=root;Pwd=";

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <returns>User IDX </returns>
        public static int Login (string id, string pw)
        {
            int userIdx = -1;
            // Check Parms
            if (id == default || pw == default)
                return userIdx;
            // Connect DB
            MySqlConnection con = new MySqlConnection(constr);
            MySqlCommand cmd = con.CreateCommand();
            string sql = $"SELECT idx FROM users WHERE id='{id}' AND pw='{pw}';";
            cmd.CommandText = sql;
            try
            {
                con.Open();
            }catch (InvalidOperationException)
            {
                return userIdx;
            }
            catch (MySqlException)
            {
                return userIdx;
            }

            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                int.TryParse(reader["idx"].ToString(), out userIdx);

            return userIdx;
        }
    }
}