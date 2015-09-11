using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ModelMaker.Clases {
    /// <summary>
    /// Representa una columna de una tabla
    /// </summary>
    public class Column : IBO<Column> {

        public string tabla { get; set; }
        public int posicion { get; set; }
        public string columna { get; set; }
        private string NombreAtributo { get; set; }
        public string tipoDeDato { get; set; }
        public int? largoMaximo { get; set; }
        public int? precisionNumerica { get; set; }
        public string defaultValue { get; set; }
        public bool nullable { get; set; }
        public bool primaryKey { get; set; }
        public bool uniqueKey { get; set; }
        public bool indexKey { get; set; }
        public bool foreignKey { get; set; }
        public string foreignTable { get; set; }
        public string foreignColumn { get; set; }
        public bool foreignKeyReferenced { get; set; }

        public string nombreAtributo {
            get {
                return (NombreAtributo ?? columna);
            }
            set {
                NombreAtributo = value;
            }
        }

        public Column() { }

        public Column(DataRow dr) {
            setData(dr);
        }

        public Column setData(DataRow dr) {

            tabla = dr["TABLE_NAME"].ToString();
            posicion = Convert.ToInt32(dr["ORDINAL_POSITION"]);
            columna = dr["COLUMN_NAME"].ToString();
            tipoDeDato = dr["DATA_TYPE"].ToString();
            largoMaximo = (dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]);
            precisionNumerica = (dr["NUMERIC_PRECISION"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["NUMERIC_PRECISION"]);
            defaultValue = dr["COLUMN_DEFAULT"].ToString();
            nullable = Convert.ToBoolean(dr["IS_NULLABLE"]);
            primaryKey = Convert.ToBoolean(dr["PRIMARY KEY"]);
            uniqueKey = Convert.ToBoolean(dr["UNIQUE KEY"]);
            indexKey= Convert.ToBoolean(dr["INDEX KEY"]);
            foreignKey = Convert.ToBoolean(dr["FOREIGN KEY"]);
            foreignTable = dr["FKTABLE"].ToString();
            foreignColumn = dr["FKCOLUMN"].ToString();
            foreignKeyReferenced = Convert.ToBoolean(dr["FKREFERENCED"]);

            return this;
        }

        public object[] ToArray() {
            return new Object[] { posicion, columna, tipoDeDato, largoMaximo, precisionNumerica,
                                    defaultValue, nullable, primaryKey, uniqueKey, indexKey,
                                    foreignKey, foreignTable, foreignColumn, foreignKeyReferenced};
        }

        public override string ToString() {
            return columna;
        }
    }
}