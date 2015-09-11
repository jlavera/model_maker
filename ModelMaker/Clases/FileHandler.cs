using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ModelMaker.Clases {
    public static class FileHandler {

        public static string OutPutFolder { get; set; }
        public static string OutPutBOFolder { get { return OutPutFolder+ "/BO/"; } }
        public static string OutPutDAOFolder { get { return OutPutFolder + "/DAO/"; } }
        public static string OutPut_DAOFolder { get { return OutPutFolder + "/_DAO/"; } }
        public static string OutPut_BOFolder { get { return OutPutFolder + "/_BO/"; } }
        public static string OutPutUtilsFolder { get { return OutPutFolder + "/Utils/"; } }
        public static string SourceFolder { get { return AppDomain.CurrentDomain.BaseDirectory + "/Sources"; } }
        public static string SourceDAOBase { get { return SourceFolder + "/DAOBase.cs"; } }
        public static string Source_DAOFile { get { return SourceFolder + "/_DAOEx"; } }
        public static string Source_BOFile { get { return SourceFolder + "/_BOEx"; } }
        public static string SourceIBO { get { return SourceFolder + "/IBO.cs"; } }
        public static string SourceDBFile { get { return SourceFolder + "/DB.cs"; } }
        public static string SourceEXCFile { get { return SourceFolder + "/MyException.cs"; } }

        private static Dictionary<string, string> tiposDeDato = new Dictionary<string, string>() {
            {"int", "int?"},
            {"smallint", "short?"},
            {"bigint", "long?"},
            {"text", "string"},
            {"varchar", "string"},
            {"datetime", "DateTime?"},
            {"date", "DateTime?"},
            {"bit", "bool?"},
            {"char", "char?"},
            {"varbinary", "byte[]"},
            {"numeric", "int?"}
        };

        private static Dictionary<string, string> tiposDeDato2 = new Dictionary<string, string>() {
            {"int", "int"},
            {"smallint", "short"},
            {"bigint", "long"},
            {"text", "string"},
            {"varchar", "string"},
            {"datetime", "DateTime"},
            {"date", "DateTime"},
            {"bit", "bool"},
            {"char", "char"},
            {"numeric", "int"}
        };

        public static string Proyecto { get; set; }
        public enum Archivos { DAOBase, _DAO, _BO, IBO, DB, EXCEPTION };
        public enum SeccionDAO { HEADER, METHODS, UQ, IX, FK, FKREF, END, PK };
        public enum SeccionBO {
            HEADER, METHODS, CONSTRUCTORES, PROPERTIES, GET, SET, FK, END, EXTERN,
            INICIALIZADORHEADER, INICIALIZADORFOOTER, INICIALIZADORITEMSTRING,
            INICIALIZADORITEMCHAR, INICIALIZADORITEMDATETIME, INICIALIZADORITEMBOOL,
            INICIALIZADORITEMSHORT, INICIALIZADORITEMINT, INICIALIZADORITEMLONG,
            INICIALIZADORITEMBYTEARRAY
        };
        /// <summary>
        /// Copiar un archivo de los source
        /// </summary>
        /// <param name="_source">Archivo</param>
        /// <param name="_to">Destino</param>
        public static void copyFile(Archivos _source, string _to, string _proyecto, Controles ctrs) {
            copyFile(_source, _to, _proyecto, null, null, ctrs);
        }

        public static void copyFile(Archivos _source, string _to, string _proyecto, string _singular, string _plural) {
            copyFile(_source, _to, _proyecto, _singular, _plural, null);
        }

        /// <summary>
        /// Copiar un archivo de los source
        /// </summary>
        /// <param name="_source">Archivo</param>
        /// <param name="_to">Destino</param>
        public static void copyFile(Archivos _source, string _to, string _proyecto, string _singular, string _plural, Controles ctrs) {
            string _from = null;

            switch (_source) {
                case Archivos.DAOBase:
                    _from = SourceDAOBase;
                    break;
                case Archivos.IBO:
                    _from = SourceIBO;
                    break;
                case Archivos.DB:
                    _from = SourceDBFile;
                    break;
                case Archivos.EXCEPTION:
                    _from = SourceEXCFile;
                    break;
                case Archivos._DAO:
                    _from = Source_DAOFile;
                    break;
                case Archivos._BO:
                    _from = Source_BOFile;
                    break;
                default:
                    throw new Exception("Not Found");
            }

            StreamReader sr = new StreamReader(_from);
            StreamWriter sw = new StreamWriter(_to, false);
            sw.Write(sr.ReadToEnd()
                .Replace("###PROYECTO###", _proyecto)
                .Replace("###SINGULAR###", _singular)
                .Replace("###PLURAL###", _plural)
                .Replace("###DIRECCION###", ctrs != null? ctrs.Direccion : "")
                .Replace("###DATABASE###", ctrs != null? ctrs.Database : "")
                .Replace("###USER###", ctrs != null? ctrs.User : "")
                .Replace("###PASSWORD###", ctrs != null? ctrs.Password : "")
                );
            sr.Close(); sr.Dispose();
            sw.Close(); sw.Dispose();
        }

        /// <summary>
        /// Crear la clase para una tabla
        /// </summary>
        /// <param name="_table">Tabla</param>
        public static void createClassFiles(Table _table) {
            
            //--Crear DAO
            StreamWriter swDAO = new StreamWriter(OutPutDAOFolder + "DAO" + _table.nombreClase + ".cs", false);
            swDAO.Write(getDAOExSection(SeccionDAO.HEADER, _table, null));
            swDAO.Write(getDAOExSection(SeccionDAO.METHODS, _table, null));
            swDAO.Write(getDAOExSection(SeccionDAO.PK, _table, _table.columnas.Find(c => c.primaryKey)));
            foreach(Column _col in _table.uk)
                swDAO.Write(getDAOExSection(SeccionDAO.UQ, _table, _col));
            foreach (Column _col in _table.columnas.FindAll(c => !c.uniqueKey && !c.primaryKey && !c.foreignKeyReferenced && c.indexKey))
                swDAO.Write(getDAOExSection(SeccionDAO.IX, _table, _col));
            foreach (Column _col in _table.columnas.FindAll(c => c.foreignKey && !c.indexKey && !c.primaryKey))
                swDAO.Write(getDAOExSection(SeccionDAO.FK, _table, _col));
            //foreach (Column _col in _table.columnas.FindAll(c => c.foreignKeyReferenced && !c.indexKey && !c.primaryKey))
            //    swDAO.Write(getDAOExSection(SeccionDAO.FKREF, _table, _col));
            swDAO.Write(getDAOExSection(SeccionDAO.END, _table, null));

            swDAO.Close();
            swDAO.Dispose();

            //--Crear los _DAO si no existen
            if (!File.Exists(OutPut_DAOFolder + "_DAO" + _table.nombreClase + ".cs"))
                copyFile(Archivos._DAO, OutPut_DAOFolder + "_DAO" + _table.nombreClase + ".cs", Proyecto, null, "DAO" + _table.nombreClase);

            //--Crear BO
            StreamWriter swBO = new StreamWriter(OutPutBOFolder + _table.nombreClase + ".cs", false);
            swBO.Write(getBOExSection(SeccionBO.HEADER, _table, null));
            swBO.Write(getBOExSection(SeccionBO.CONSTRUCTORES, _table, null));

            foreach (Column _col in _table.columnas.FindAll(c=>!c.foreignKey))
                swBO.Write(getBOExSection(SeccionBO.PROPERTIES, _table, _col));

            foreach (Column _col in _table.columnas.FindAll(c => c.foreignKey))
                swBO.Write(getBOExSection(SeccionBO.FK, _table, _col));

            swBO.Write(getBOExSection(SeccionBO.INICIALIZADORHEADER, _table, null));

            foreach (Column _col in _table.columnas)
                swBO.Write(getBOExSection(getBOSection(_col.tipoDeDato, _col.largoMaximo), _table, _col));

            swBO.Write(getBOExSection(SeccionBO.INICIALIZADORFOOTER, _table, null));

            foreach (Column _col in _table.columnas.FindAll(c => !c.primaryKey && !c.uniqueKey && c.foreignKeyReferenced))
                swBO.Write(getBOExSection(SeccionBO.EXTERN, _table, _col));

            swBO.Write(getBOExSection(SeccionBO.METHODS, _table, null));
            swBO.Write(getBOExSection(SeccionBO.END, _table, null));

            swBO.Close();
            swBO.Dispose();

            //--Crear los _BO si no existen
            if (!File.Exists(OutPut_BOFolder + _table.nombreClase + ".cs"))
                copyFile(Archivos._BO, OutPut_BOFolder + _table.nombreClase + ".cs", Proyecto, _table.nombreClase, "DAO" + _table.nombreClase);
        }

        /// <summary>
        /// Devuelve una sección reemplazando los valores
        /// </summary>
        /// <param name="_seccion">Sección</param>
        /// <param name="_table">Tabla</param>
        /// <param name="_col">Columna</param>
        /// <returns></returns>
        private static string getBOExSection(SeccionBO _seccion, Table _table, Column _col) {

            //--Cargar la sección
            string text = buildBOExSection(_seccion.ToString());

            string t1 = null;
            string t2 = null;
            try{
                if (_col != null) {
                    t1 = tiposDeDato[_col.tipoDeDato];
                    t2 = tiposDeDato2[_col.tipoDeDato];
                }
            }catch(KeyNotFoundException){
                throw new Exception("El tipo de dato " + _col.tipoDeDato + " no es soportado");
            }

            switch (_seccion) {
                case SeccionBO.HEADER:
                    return text.Replace("###PROYECTO###", Proyecto).Replace("###SINGULAR###", _table.nombreClase);
                case SeccionBO.CONSTRUCTORES:
                    return text.Replace("###SINGULAR###", _table.nombreClase);
                case SeccionBO.PROPERTIES:
                    return text.Replace("###TIPODEDATO###", t1).Replace("###ATTRIBUTE###", _col.nombreAtributo);
                case SeccionBO.FK:
                    return text.Replace("###FKTABLE###", _col.foreignTable).Replace("###FKCOLUMN###", _col.foreignColumn).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###TIPODEDATO###", t1).Replace("###SINGULAR###", _table.nombreClase).Replace("###COLUMN###", _col.nombreAtributo);
                case SeccionBO.GET:
                    return text.Replace("###TIPODEDATO###", t2).Replace("###ATTRIBUTE###", _col.nombreAtributo);
                case SeccionBO.SET:
                    return text.Replace("###TIPODEDATO###", t2).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###SINGULAR###", _table.nombreClase);
                case SeccionBO.METHODS:
                    return text.Replace("###PLURAL###", "DAO" + _table.nombreClase).Replace("###SINGULAR###", _table.nombreClase).Replace("###PK###", _table.pk.columna);
                case SeccionBO.INICIALIZADORHEADER:
                    return text.Replace("###PLURAL###", "DAO" + _table.nombreClase).Replace("###SINGULAR###", _table.nombreClase).Replace("###PK###", _table.pk.columna);
                case SeccionBO.INICIALIZADORITEMSTRING:
                case SeccionBO.INICIALIZADORITEMCHAR:
                case SeccionBO.INICIALIZADORITEMDATETIME:
                case SeccionBO.INICIALIZADORITEMBOOL:
                case SeccionBO.INICIALIZADORITEMSHORT:
                case SeccionBO.INICIALIZADORITEMINT:
                case SeccionBO.INICIALIZADORITEMLONG:
                case SeccionBO.INICIALIZADORITEMBYTEARRAY:
                    return text.Replace("###COLUMN###", _col.nombreAtributo).Replace("###FIELD###", _col.nombreAtributo);
                case SeccionBO.INICIALIZADORFOOTER:
                    return text.Replace("###PLURAL###", "DAO" + _table.nombreClase).Replace("###SINGULAR###", _table.nombreClase).Replace("###PK###", _table.pk.columna);
                case SeccionBO.EXTERN:
                    return text.Replace("###FKTABLE###", _col.foreignTable).Replace("###FKTABLEPLURAL###", "DAO" + _col.foreignTable).Replace("###FKCOLUMN###", _col.foreignColumn);
                case SeccionBO.END:
                    return text;
                default:
                    throw new Exception("Not found");
            }
        }

        /// <summary>
        /// Guarda en el diccionario una sección
        /// </summary>
        /// <param name="_seccion">Sección</param>
        private static string buildBOExSection(string _seccion) {

            string temp = null;
            string aux;

            StreamReader sr = new StreamReader(SourceFolder + "/BOEx");

            //--Buscar comienzo
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@BOEX" + _seccion + "@@@")) ;

            //--Copiar hasta fin
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@ENDBOEX" + _seccion + "@@@"))
                temp += aux + '\n';

            sr.Close();
            sr.Dispose();

            //--Guardar en diccionario
            return temp;

        }

        /// <summary>
        /// Devuelve una sección reemplazando los valores
        /// </summary>
        /// <param name="_seccion">Sección</param>
        /// <param name="_table">Tabla</param>
        /// <returns></returns>
        private static string getDAOExSection(SeccionDAO _seccion, Table _table, Column _col) {

            //--Cargar la sección
            string text = buildDAOExSection(_seccion.ToString());

            if (_table.pk == null) throw new Exception("Debe haber una columna PK en la tabla " + _table.nombreClase + ".");

            switch (_seccion) {
                case SeccionDAO.HEADER:
                    return text.Replace("###PROYECTO###", Proyecto).Replace("###PLURAL###", "DAO" + _table.nombreClase).Replace("###SINGULAR###", _table.nombreClase);
                case SeccionDAO.METHODS:
                    return text.Replace("###PLURAL###", "DAO" + _table.nombreClase).Replace("###SINGULAR###", _table.nombreClase).Replace("###PK###", _table.pk.columna).Replace("###SCHEMA###", "dbo");
                case SeccionDAO.PK:
                    return text.Replace("###SINGULAR###", _table.nombreClase).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###COLUMN###", _col.columna);
                case SeccionDAO.UQ:
                    return text.Replace("###SINGULAR###", _table.nombreClase).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###COLUMN###", _col.columna);
                case SeccionDAO.IX:
                    return text.Replace("###SINGULAR###", _table.nombreClase).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###COLUMN###", _col.columna);
                case SeccionDAO.FK:
                    return text.Replace("###SINGULAR###", _table.nombreClase).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###COLUMN###", _col.columna);
                case SeccionDAO.END:
                    return text;
                default:
                    throw new Exception("Not found");
            }
        }

        /// <summary>
        /// Guarda en el diccionario una sección
        /// </summary>
        /// <param name="_seccion">Sección</param>
        private static string buildDAOExSection(string _seccion) {

            string temp = null;
            string aux;

            StreamReader sr = new StreamReader(SourceFolder + "/DAOEx");

            //--Buscar comienzo
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@DAOEX" + _seccion + "@@@")) ;

            //--Copiar hasta fin
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@ENDDAOEX" + _seccion + "@@@"))
                temp += aux + '\n';

            sr.Close();
            sr.Dispose();

            //--Guardar en diccionario
            return temp;
        }

        /// <summary>
        /// Devuelve el tipo de sección, según el tipo de dato
        /// </summary>
        /// <param name="_tipo">Tipo de dato</param>
        /// <returns></returns>
        private static SeccionBO getBOSection(string _tipo, int? _largo) {

            if (_tipo == "char" && _largo == 1)
                return SeccionBO.INICIALIZADORITEMCHAR;

            if (_tipo == "text" || _tipo.IndexOf("char") != -1)
                return SeccionBO.INICIALIZADORITEMSTRING;

            if (_tipo.IndexOf("date") != -1)
                return SeccionBO.INICIALIZADORITEMDATETIME;

            if (_tipo == "bit")
                return SeccionBO.INICIALIZADORITEMBOOL;

            if (_tipo == "smallint")
                return SeccionBO.INICIALIZADORITEMSHORT;

            if (_tipo == "int" || _tipo == "numeric")
                return SeccionBO.INICIALIZADORITEMINT;

            if (_tipo == "bigint")
                return SeccionBO.INICIALIZADORITEMLONG;

            if (_tipo == "varbinary")
                return SeccionBO.INICIALIZADORITEMBYTEARRAY;

            throw new Exception("Tipo de dato: " + _tipo + " no soportado.");
        }

    }
}