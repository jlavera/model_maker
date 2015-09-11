using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ModelMaker.Clases {
    static public class DB {﻿

        static private string motor { get; set; }
        static private string direccion { get; set; }
        static private string database { get; set; }
        static private string username { get; set; }
        static private string password { get; set; }

        //static private string strCon = "Data Source=JOA-NB\\;Initial Catalog=todo_tracker;Persist Security Info=True;User ID=sa;Password=surubi";
        static private string strCon {
            get {
                return "Data Source=" + direccion + ";Initial Catalog=" +
                    database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password + ";";
            }
        }
        static private SqlConnection sqlCon;
        static public Exception exception;

        static public void setData(string _motor, string _direccion, string _db, string _username, string _password) {
            motor = _motor;
            direccion = _direccion;
            database = _db;
            username = _username;
            password = _password;

            sqlCon = new SqlConnection(strCon);
        }

        /// <summary>
        /// Ejecuta comando y lo devuelve en un datatable
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns
        static public List<T> ExecuteReader<T>(string command, params object[] parameters) where T : IBO<T>, new() {

            DataTable dt = new DataTable();
            try {

                sqlCon.Open();

                SqlCommand com = new SqlCommand(command, sqlCon);

                for (int i = 0; i < parameters.Length; i++)
                    com.Parameters.Add(new SqlParameter((i + 1).ToString(), parameters[i]));

                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                da.Dispose();

            } catch (Exception ex) {
                exception = ex;
            } finally {

                sqlCon.Close();
            }

            List<T> aux = new List<T>();
            foreach (DataRow dr in dt.Rows)
                aux.Add(new T().setData(dr));

            return aux;
        }

        /// <summary>
        /// Ejecuta comando y devuelve el primer registro
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public T ExecuteReaderSingle<T>(string command, params object[] parameters) where T : IBO<T>, new() {

            JDataTable dt = new JDataTable();
            try {

                sqlCon.Open();

                SqlDataAdapter da = new SqlDataAdapter(command, sqlCon);
                da.Fill(dt);
                da.Dispose();

            } catch (Exception ex) {
                exception = ex;
            } finally {

                sqlCon.Close();
            }
            return new T().setData(dt.Rows[0]);
        }

        /// <summary>
        /// Ejecuta comando y devuelve un número
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public int ExecuteCardinal(string command) {

            SqlDataReader reader = null;
            int temp = -1;

            try {
                sqlCon.Open();

                reader = (new SqlCommand(command, sqlCon)).ExecuteReader();

                if (reader.Read())
                    //--Es convert porque hay veces que trae Decimal y el getInt no entiende nada :)
                    temp = Convert.ToInt32(reader[0]);

            } catch (Exception ex) {
                exception = ex;
            } finally {

                sqlCon.Close();
            }
            return temp;
        }

        /// <summary>
        /// Ejecuta comando y devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string command) {
            int temp = -1;

            try {
                sqlCon.Open();
                temp = (new SqlCommand(command, sqlCon)).ExecuteNonQuery();

            } catch (Exception ex) {
                exception = ex;
            } finally {
                sqlCon.Close();
            }
            return temp;
        }
    }
}