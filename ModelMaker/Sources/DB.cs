using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using ###PROYECTO###.Models.BO;

namespace ###PROYECTO###.Models.Utils{
    static public class DB {﻿
 
        static private string motor { get; set; }
        static private string direccion { get; set; }
        static private string database { get; set; }
        static private string username { get; set; }
        static private string password { get; set; }
 
        static private string strCon = "Data Source=###DIRECCION###;Initial Catalog=###DATABASE###;Persist Security Info=True;User ID=###USER###;Password=###PASSWORD###";
 
        static private SqlConnection sqlCon = new SqlConnection(strCon);
        static public Exception exception;
 
        static private Dictionary<Transaccion, SqlTransaction> transacciones = new Dictionary<Transaccion, SqlTransaction>();
 
        static private void setData() {
            sqlCon = new SqlConnection(strCon);
        }
 
        static public void setData(string _motor, string _direccion, string _db, string _username, string _password) {
            motor = _motor;
            direccion = _direccion;
            database = _db;
            username = _username;
            password = _password;
 
            setData();
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Crea una transacción
        /// </summary>
        static public void CreateTransaction(Transaccion tran) {
            CreateTransaction(tran.Nombre);
        }
 
        /// <summary>
        /// Crea una transacción
        /// </summary>
        /// <param name="nombre">Nombre único</param>
        static public void CreateTransaction(string nombre) {
 
            Transaccion tran = transacciones.Keys.FirstOrDefault<Transaccion>(t => t.Nombre == nombre);
 
            if (tran != null)
                throw new MyException("Ya existe la transacción.");
           
            sqlCon.Open();
 
            transacciones.Add(tran, sqlCon.BeginTransaction(tran.Nombre));
        }
 
        /// <summary>
        /// Commitea una transacción
        /// </summary>
        static public void CommitTransaction(Transaccion tran) {
            CommitTransaction(tran.Nombre);
        }
 
        /// <summary>
        /// Commitea una transacción
        /// </summary>
        /// <param name="nombre">Nombre</param>
        static public void CommitTransaction(string nombre) {
 
            Transaccion tran = transacciones.Keys.FirstOrDefault<Transaccion>(t => t.Nombre == nombre);
 
            if (tran == null)
                throw new MyException("No se encuentra la transacción.");
 
            transacciones[tran].Commit();
 
            transacciones.Remove(tran);
 
            sqlCon.Close();
        }
 
        /// <summary>
        /// Rollbackea una transacción
        /// </summary>
        static public void RollBackTransaction(Transaccion tran) {
            RollBackTransaction(tran.Nombre);
        }
 
        /// <summary>
        /// Rollbackea una transacción
        /// </summary>
        /// <param name="nombre">Nombre</param>
        static public void RollBackTransaction(string nombre) {
 
            Transaccion tran = transacciones.Keys.FirstOrDefault<Transaccion>(t => t.Nombre == nombre);
 
            if (tran == null)
                throw new MyException("No se encuentra la transacción.");
 
            transacciones[tran].Rollback();
 
            transacciones.Remove(tran);
 
            sqlCon.Close();
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Ejecuta comando y lo devuelve en un datatable
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns
        static public List<T> ExecuteReader<T>(string command) where T : IBO<T>, new() {
            return ExecuteReader<T>(command, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y lo devuelve en un datatable
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns
        static public List<T> ExecuteReader<T>(string command, params object[] parameters) where T : IBO<T>, new() {
            return ExecuteReader<T>(command, null, parameters);
        }
 
        /// <summary>
        /// Ejecuta comando y lo devuelve en un datatable
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns
        static public List<T> ExecuteReader<T>(string command, Transaccion transaccion) where T : IBO<T>, new() {
            return ExecuteReader<T>(command, transaccion, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y lo devuelve en un datatable
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns
        static public List<T> ExecuteReader<T>(string command, Transaccion transaccion, params object[] parameters) where T : IBO<T>, new() {
 
            DataTable dt = new DataTable();
            try {
 
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();
               
                SqlCommand com = CompleteCommand(command, transaccion, parameters);
 
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                da.Dispose();
 
            } catch (Exception ex) {
                if (transaccion != null)
                    RollBackTransaction(transaccion);
                throw new MyException(ex);
            } finally {
 
                sqlCon.Close();
            }
 
            List<T> aux = new List<T>();
            foreach (DataRow dr in dt.Rows)
                aux.Add(new T().setData(dr));
 
            return aux;
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Ejecuta comando y devuelve el primer registro
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public T ExecuteReaderSingle<T>(string command) where T : IBO<T>, new() {
            return ExecuteReaderSingle<T>(command, null, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer registro
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public T ExecuteReaderSingle<T>(string command, params object[] parameters) where T : IBO<T>, new() {
            return ExecuteReaderSingle<T>(command, null, parameters);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer registro
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns>
        static public T ExecuteReaderSingle<T>(string command, Transaccion transaccion) where T : IBO<T>, new() {
            return ExecuteReaderSingle<T>(command, transaccion, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer registro
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <param name="parameters">Parámetros de la transacción</param>
        /// <returns></returns>
        static public T ExecuteReaderSingle<T>(string command, Transaccion transaccion, params object[] parameters) where T : IBO<T>, new() {
 
            DataTable dt = new DataTable();
            try {
 
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();
 
                SqlCommand com = CompleteCommand(command, transaccion, parameters);
 
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                da.Dispose();
 
            } catch (Exception ex) {
                if (transaccion != null)
                    RollBackTransaction(transaccion);
                throw new MyException(ex);
            } finally {
 
                sqlCon.Close();
            }
 
            return new T().setData(dt.Rows[0]);
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Ejecuta comando y devuelve un número
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public int ExecuteCardinal(string command) {
            return ExecuteCardinal(command, null, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve un número
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public int ExecuteCardinal(string command, params object[] parameters) {
            return ExecuteCardinal(command, null, parameters);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve un número
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns>
        static public int ExecuteCardinal(string command, Transaccion transaccion) {
            return ExecuteCardinal(command, transaccion, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve un número
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public int ExecuteCardinal(string command, Transaccion transaccion, params object[] parameters) {
            SqlDataReader reader = null;
            int temp = -1;
 
            try {
 
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();
 
                SqlCommand com = CompleteCommand(command, transaccion, parameters);
 
                reader = com.ExecuteReader();
 
                if (reader.Read())
                    //--Es convert porque hay veces que trae Decimal y el getInt no entiende nada :)
                    temp = Convert.ToInt32(reader[0]);
 
            } catch (Exception ex) {
                if (transaccion != null)
                    RollBackTransaction(transaccion);
                throw new MyException(ex);
            } finally {
 
                sqlCon.Close();
            }
            return temp;
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Ejecuta comando y devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string command) {
            return ExecuteNonQuery(command, null, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string command, params object[] parameters) {
            return ExecuteNonQuery(command, null, parameters);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string command, Transaccion transaccion) {
            return ExecuteNonQuery(command, transaccion, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string command, Transaccion transaccion, params object[] parameters) {
            int temp = -1;
 
            try {
 
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();
 
                SqlCommand com = CompleteCommand(command, transaccion, parameters);
 
                temp = com.ExecuteNonQuery();
 
            } catch (Exception ex) {
                if (transaccion != null)
                    RollBackTransaction(transaccion);
                throw new MyException(ex);
            } finally {
                if (transaccion == null)
                    sqlCon.Close();
            }
            return temp;
        }
 
        //---------------------------------------------------------------------
        /// <summary>
        /// Ejecuta comando y devuelve el primer dato casteado
        /// </summary>
        /// <param name="command">Comando</param>
        /// <returns></returns>
        static public T ExecuteCastable<T>(string command) {
            return ExecuteCastable<T>(command, null, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer dato casteado
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public T ExecuteCastable<T>(string command, params object[] parameters) {
            return ExecuteCastable<T>(command, null, parameters);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer dato casteado
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <returns></returns>
        static public T ExecuteCastable<T>(string command, Transaccion transaccion) {
            return ExecuteCastable<T>(command, transaccion, new object[0]);
        }
 
        /// <summary>
        /// Ejecuta comando y devuelve el primer dato casteado
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="transaccion">Transacción a utilizar</param>
        /// <param name="parameters">Parámetros del comando</param>
        /// <returns></returns>
        static public T ExecuteCastable<T>(string command, Transaccion transaccion, params object[] parameters) {
            SqlDataReader reader = null;
            T temp = default(T);
 
            try {
 
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();
 
                SqlCommand com = CompleteCommand(command, transaccion, parameters);
 
                reader = com.ExecuteReader();
 
                if (reader.Read()) {
                    if (reader[0] == DBNull.Value)
                        throw new MyException("Error en la lectura (resultado vacío).");
                    temp = (T)Convert.ChangeType(reader[0], typeof(T));
                }
 
            } catch (Exception ex) {
                if (transaccion != null)
                    RollBackTransaction(transaccion);
                throw new MyException(ex);
            } finally {
 
                sqlCon.Close();
            }
 
            return temp;
        }
 
        //---------------------------------------------------------------------
        static private SqlCommand CompleteCommand(string command, Transaccion transaccion, params object[] parameters) {
 
            //--Validar parámetros
            if (parameters.Any<object>(p => p == null))
                throw new MyException("No pueden haber parámetros nulos.");
 
            Console.Out.WriteLine("Command: " + command + ";");
            SqlCommand com = new SqlCommand(command, sqlCon);
 
            if (parameters.Length > 0) {
                Console.Out.Write("Parameters: ");
                for (int i = 0; i < parameters.Length; i++)
                    com.Parameters.Add(new SqlParameter((i + 1).ToString(), parameters[i]));
 
                foreach (SqlParameter param in com.Parameters)
                    Console.Out.Write(" [@" + param.ParameterName + " = " + param.Value.ToString() + "] ");
 
                Console.Out.WriteLine("");
            }
 
            //--Si vino la transacción, utilizarla
            if (transaccion != null)
                com.Transaction = transacciones[transaccion];
 
            return com;
        }
    }
	
    public class Transaccion {

        public string Nombre { get; set; }

    }
}