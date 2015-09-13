using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using ModelMaker.Utils;

namespace ModelMaker.Clases {
    public class FilesHandler {

        public static Controles ctrs;

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

        public FilesHandler(string _proyecto, string _folder, Controles _ctrs) {
            Proyecto = _proyecto;
            PathsMngr.OutPutFolder = _folder;
            ctrs = _ctrs;
        }

        public void Generate(List<Table> tables) {
            //--Generar las carpetas necesarias si no existen
            new List<string>(){ 
                PathsMngr.OutPutFolder,
                PathsMngr.OutPutDAOFolder,
                PathsMngr.OutPut_DAOFolder,
                PathsMngr.OutPutBOFolder,
                PathsMngr.OutPut_BOFolder,
                PathsMngr.OutPutUtilsFolder
            }.ForEach(s => { if (!Directory.Exists(s)) Directory.CreateDirectory(s); });

            //--Generar los archivos básicos
            new List<Tuple<string, string>>() {
                //--Crear DAOBase
                new Tuple<string, string>(PathsMngr.SourceDAOBase, PathsMngr.OutPutDAOBaseFile),
                //--Crear IBO
                new Tuple<string, string>(PathsMngr.SourceIBO, PathsMngr.OutPutIBOFile),
                //--Crear DB
                new Tuple<string, string>(PathsMngr.SourceDBFile, PathsMngr.OutPutDBFile),
                //--Crear MyException
                new Tuple<string, string>(PathsMngr.SourceEXCFile, PathsMngr.OutPutExcFile)
            }.ForEach(t => FilesHandler.CopyFile(t.Item1, t.Item2));

            //--Generar el código de cada tabla
            tables.ForEach(t => t.Generate());
        }

        public static void SaveFile(string to, string text) {
            StreamWriter sw = new StreamWriter(to, false);
            sw.Write(replaceGenericValues(text)); sw.Close(); sw.Dispose();
        }

        public static string LoadFile(string from) {
            StreamReader sr = new StreamReader(from);
            var text = sr.ReadToEnd();
            sr.Close(); sr.Dispose();

            return text;
        }

        public static void CopyFile(string from, string to) {
            SaveFile(to, LoadFile(from));
        }

        //--TODO escapear esto
        private static string replaceGenericValues(string text) {
            return text
                .Replace("###PROYECTO###", FilesHandler.Proyecto)
                .Replace("###DIRECCION###", FilesHandler.ctrs.Direccion)
                .Replace("###DATABASE###", FilesHandler.ctrs.Database)
                .Replace("###USER###", FilesHandler.ctrs.User)
                .Replace("###SCHEMA###", "dbo")
                .Replace("###PASSWORD###", FilesHandler.ctrs.Password);
        }

        ///// <summary>
        ///// Devuelve el tipo de sección, según el tipo de dato
        ///// </summary>
        ///// <param name="_tipo">Tipo de dato</param>
        ///// <returns></returns>
        //private SeccionBO getBOSection(string _tipo, int? _largo) {

        //    if (_tipo == "char" && _largo == 1)
        //        return SeccionBO.INICIALIZADORITEMCHAR;

        //    if (_tipo == "text" || _tipo.IndexOf("char") != -1)
        //        return SeccionBO.INICIALIZADORITEMSTRING;

        //    if (_tipo.IndexOf("date") != -1)
        //        return SeccionBO.INICIALIZADORITEMDATETIME;

        //    if (_tipo == "bit")
        //        return SeccionBO.INICIALIZADORITEMBOOL;

        //    if (_tipo == "smallint")
        //        return SeccionBO.INICIALIZADORITEMSHORT;

        //    if (_tipo == "int" || _tipo == "numeric")
        //        return SeccionBO.INICIALIZADORITEMINT;

        //    if (_tipo == "bigint")
        //        return SeccionBO.INICIALIZADORITEMLONG;

        //    if (_tipo == "varbinary")
        //        return SeccionBO.INICIALIZADORITEMBYTEARRAY;

        //    throw new Exception("Tipo de dato: " + _tipo + " no soportado.");
        //}

    }

}