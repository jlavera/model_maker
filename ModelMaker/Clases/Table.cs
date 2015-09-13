using ModelMaker.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMaker.Clases {
    public partial class Table {

        private string tabla;
        private string NombreClase { get; set; }
        public List<Column> columnas { get; set; }
        //public List<Column> pk { get { return columnas.FindAll(c => c.primaryKey); } }
        public Column pk { get { return columnas.Find(c => c.primaryKey); } }
        public List<Column> fk { get { return columnas.FindAll(c => c.foreignKey); } }
        public List<Column> uk { get { return columnas.FindAll(c => c.uniqueKey); } }

        //--TODO podría parsear el nombre para que cambie los _ por camel case
        public string nombreClase {
            get {
                return (NombreClase ?? tabla);
            }
            set {
                NombreClase = value;
            }
        }

        public Table(string nombre, List<Column> _col) {
            tabla = nombre;
            columnas = _col;
        }

        public void addColumn(Column _col) {
            columnas.Add(_col);
        }

        public void Generate() { //--TODO tipar las secciones

            #region armar DAO
            //--Primero armar todo el archivo, y al final reemplazar los valores
            string text = "";
            text += SectionsHandler.GetDAOSection("HEADER") + SectionsHandler.GetDAOSection("METHODS");

            text += replaceColumnValues(SectionsHandler.GetDAOSection("PK"), pk);
            foreach (Column _col in uk)
                text += replaceColumnValues(SectionsHandler.GetDAOSection("UQ"), _col);
            foreach (Column _col in columnas.FindAll(c => !c.uniqueKey && !c.primaryKey && !c.foreignKeyReferenced && c.indexKey))
                text += replaceColumnValues(SectionsHandler.GetDAOSection("IX"), _col);
            foreach (Column _col in columnas.FindAll(c => c.foreignKey && !c.indexKey && !c.primaryKey))
                text += replaceColumnValues(SectionsHandler.GetDAOSection("FK"), _col);
            foreach (Column _col in columnas.FindAll(c => c.foreignKeyReferenced && !c.indexKey && !c.primaryKey))
                text += replaceColumnValues(SectionsHandler.GetDAOSection("FKREF"), _col);

            text += SectionsHandler.GetDAOSection("END");

            text = replaceTableValues(text);

            FilesHandler.SaveFile(PathsMngr.OutPutDAOFolder + "DAO" + nombreClase + ".cs", text);

            //--Crear los _DAO si no existen
            if (!File.Exists(PathsMngr.OutPut_DAOFolder + nombreClase + ".cs"))
                FilesHandler.SaveFile(
                    PathsMngr.OutPut_DAOFolder + "_DAO" + nombreClase + ".cs",
                    replaceTableValues(FilesHandler.LoadFile(PathsMngr.Source_DAOFile)));
            #endregion

            #region armar BO
            //--Primero armar todo el archivo, y al final reemplazar los valores
            text = "";
            text += SectionsHandler.GetBOSection("HEADER") + SectionsHandler.GetBOSection("CONSTRUCTORES");

            foreach (Column _col in columnas.FindAll(c => !c.foreignKey))
                text += replaceColumnValues(SectionsHandler.GetBOSection("PROPERTIES"), _col);
            foreach (Column _col in columnas.FindAll(c => c.foreignKey))
                text += replaceColumnValues(SectionsHandler.GetBOSection("FK"), _col);

            text += replaceTableValues(SectionsHandler.GetBOSection("INICIALIZADORHEADER"));
            foreach (Column _col in columnas)
                text += replaceColumnValues(SectionsHandler.GetBOSection("INICIALIZADORITEM" + _col.TipoDato.cs_name.ToUpper()), _col);
            text += replaceTableValues(SectionsHandler.GetBOSection("INICIALIZADORFOOTER"));

            foreach (Column _col in columnas.FindAll(c => !c.primaryKey && !c.uniqueKey && c.foreignKeyReferenced))
                text += replaceColumnValues(SectionsHandler.GetBOSection("EXTERN"), _col);

            text += SectionsHandler.GetBOSection("METHODS");
            text += SectionsHandler.GetBOSection("END");

            text = replaceTableValues(text);

            FilesHandler.SaveFile(PathsMngr.OutPutBOFolder + nombreClase + ".cs", text);

            //--Crear los _BO si no existen
            if (!File.Exists(PathsMngr.OutPut_BOFolder + nombreClase + ".cs"))
                FilesHandler.SaveFile(
                    PathsMngr.OutPut_BOFolder + "_BO" + nombreClase + ".cs",
                    replaceTableValues(FilesHandler.LoadFile(PathsMngr.Source_BOFile)));
            #endregion
        }

        private string replaceTableValues(string text) {
            return text
                .Replace("###SINGULAR###", nombreClase)
                .Replace("###PLURAL###", "DAO" + nombreClase)
                .Replace("###PK###", pk.nombreAtributo);
        }

        private string replaceColumnValues(string text, Column col) {
            return text
                .Replace("###TIPODEDATO###", col.TipoDato.cs_opt_name)
                .Replace("###ATTRIBUTE###", col.nombreAtributo)
                .Replace("###FKTABLE###", col.foreignTable)
                .Replace("###FKCOLUMN###", col.foreignColumn)
                .Replace("###COLUMN###", col.nombreAtributo)
                .Replace("###FIELD###", col.nombreAtributo)
                .Replace("###FKTABLE###", col.foreignTable)
                .Replace("###FKTABLEPLURAL###", "DAO" + col.foreignTable);
        }

        public override string ToString() {
            return tabla;
        }
    }
}