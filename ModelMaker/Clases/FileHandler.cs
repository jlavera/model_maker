using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using ModelMaker.Utils;

namespace ModelMaker.Clases {
    public class FileHandler {

        private Controles ctrs;

        public string OutPutFolder { get; set; }
        public string OutPutBOFolder { get { return OutPutFolder+ "/BO/"; } }
        public string OutPutDAOFolder { get { return OutPutFolder + "/DAO/"; } }
        public string OutPut_DAOFolder { get { return OutPutFolder + "/_DAO/"; } }
        public string OutPut_BOFolder { get { return OutPutFolder + "/_BO/"; } }
        public string OutPutUtilsFolder { get { return OutPutFolder + "/Utils/"; } }
        public string SourceFolder { get { return AppDomain.CurrentDomain.BaseDirectory + "/Sources"; } }
        public string SourceDAOBase { get { return SourceFolder + "/DAOBase.cs"; } }
        public string Source_DAOFile { get { return SourceFolder + "/_DAOEx"; } }
        public string Source_BOFile { get { return SourceFolder + "/_BOEx"; } }
        public string SourceIBO { get { return SourceFolder + "/IBO.cs"; } }
        public string SourceDBFile { get { return SourceFolder + "/DB.cs"; } }
        public string SourceEXCFile { get { return SourceFolder + "/MyException.cs"; } }

        private List<TipoDato> tiposDeDato = new List<TipoDato>(){
            new TipoDato("int", "int", "int?"),
            new TipoDato("smallint", "short", "short?"),
            new TipoDato("bigint", "long", "long?"),
            new TipoDato("text", "string", "string"),
            new TipoDato("varchar", "string", "string"),
            new TipoDato("datetime", "DateTime", "DateTime?"),
            new TipoDato("date", "DateTime", "DateTime?"),
            new TipoDato("bit", "bool", "bool?"),
            new TipoDato("char", "char", "char?"),
            new TipoDato("numeric", "int", "int?")
        };

        public string Proyecto { get; set; }
        public enum Archivos { DAOBase, _DAO, _BO, IBO, DB, EXCEPTION };
        public enum SeccionDAO { HEADER, METHODS, UQ, IX, FK, FKREF, END, PK };
        public enum SeccionBO {
            HEADER, METHODS, CONSTRUCTORES, PROPERTIES, GET, SET, FK, END, EXTERN,
            INICIALIZADORHEADER, INICIALIZADORFOOTER, INICIALIZADORITEMSTRING,
            INICIALIZADORITEMCHAR, INICIALIZADORITEMDATETIME, INICIALIZADORITEMBOOL,
            INICIALIZADORITEMSHORT, INICIALIZADORITEMINT, INICIALIZADORITEMLONG,
            INICIALIZADORITEMBYTEARRAY
        };

        public FileHandler(string _proyecto, string _folder, Controles _ctrs) {
            Proyecto = _proyecto;
            OutPutFolder = _folder;
            ctrs = _ctrs;
        }

        /// <summary>
        /// Copiar un archivo de los source
        /// </summary>
        /// <param name="_source">Archivo</param>
        /// <param name="_to">Destino</param>
        public void copyFile(Archivos _source, string _to) {
            copyFile(_source, _to, null, null);
        }

        /// <summary>
        /// Copiar un archivo de los source
        /// </summary>
        /// <param name="_source">Archivo</param>
        /// <param name="_to">Destino</param>
        public void copyFile(Archivos _source, string _to, string _singular, string _plural) {
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
                .Replace("###PROYECTO###", Proyecto)
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
        public void createClassFiles(Table _table) {
            
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
                copyFile(Archivos._DAO, OutPut_DAOFolder + "_DAO" + _table.nombreClase + ".cs", null, "DAO" + _table.nombreClase);

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
                copyFile(Archivos._BO, OutPut_BOFolder + _table.nombreClase + ".cs", _table.nombreClase, "DAO" + _table.nombreClase);
        }

        /// <summary>
        /// Devuelve una sección reemplazando los valores
        /// </summary>
        /// <param name="_seccion">Sección</param>
        /// <param name="_table">Tabla</param>
        /// <param name="_col">Columna</param>
        /// <returns></returns>
        private string getBOExSection(SeccionBO _seccion, Table _table, Column _col) {

            //--Cargar la sección
            string text = buildBOExSection(_seccion.ToString());
            TipoDato tipo;

            tipo = tiposDeDato.GetByDB(_col.tipoDeDato);

            switch (_seccion) {
                case SeccionBO.HEADER:
                    return text.Replace("###PROYECTO###", Proyecto).Replace("###SINGULAR###", _table.nombreClase);
                case SeccionBO.CONSTRUCTORES:
                    return text.Replace("###SINGULAR###", _table.nombreClase);
                case SeccionBO.PROPERTIES:
                    return text.Replace("###TIPODEDATO###", tipo.cs_name).Replace("###ATTRIBUTE###", _col.nombreAtributo);
                case SeccionBO.FK:
                    return text.Replace("###FKTABLE###", _col.foreignTable).Replace("###FKCOLUMN###", _col.foreignColumn).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###TIPODEDATO###", tipo.cs_name).Replace("###SINGULAR###", _table.nombreClase).Replace("###COLUMN###", _col.nombreAtributo);
                case SeccionBO.GET:
                    return text.Replace("###TIPODEDATO###", tipo.cs_opt_name).Replace("###ATTRIBUTE###", _col.nombreAtributo);
                case SeccionBO.SET:
                    return text.Replace("###TIPODEDATO###", tipo.cs_name).Replace("###ATTRIBUTE###", _col.nombreAtributo).Replace("###SINGULAR###", _table.nombreClase);
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
        private string buildBOExSection(string _seccion) {

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
        private string getDAOExSection(SeccionDAO _seccion, Table _table, Column _col) {

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
        private string buildDAOExSection(string _seccion) {

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
        private SeccionBO getBOSection(string _tipo, int? _largo) {

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

        public void Work(IEnumerable<Table> tables) {
            //--Carpeta raiz de salida
            //if (Directory.Exists(OutPutFolder))
            //    Directory.Delete(OutPutFolder, true);

            //--Crea las carpetas necesarias si no existen
            var create = new Action<String>(s => {
                if (!Directory.Exists(s)) Directory.CreateDirectory(s);
            });

            create.Invoke(OutPutFolder);
            create.Invoke(OutPutDAOFolder);
            create.Invoke(OutPut_DAOFolder);
            create.Invoke(OutPutBOFolder);
            create.Invoke(OutPut_BOFolder);
            create.Invoke(OutPutUtilsFolder);

            //--Crear DAOBase
            copyFile(FileHandler.Archivos.DAOBase, OutPutDAOFolder + "/DAOBase.cs");

            //--Crear IBO
            copyFile(FileHandler.Archivos.IBO, OutPutBOFolder + "/IBO.cs");

            //--Crear DB
            copyFile(FileHandler.Archivos.DB, OutPutUtilsFolder + "/DB.cs");

            //--Crear MyException
            copyFile(FileHandler.Archivos.EXCEPTION, OutPutUtilsFolder + "/MyException.cs");

            //--Crear los DAOs
            foreach (Table tlb in tables) createClassFiles(tlb);
        }

    }

}