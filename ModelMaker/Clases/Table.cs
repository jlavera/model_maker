using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMaker.Clases {
    public partial class Table {

        private string tabla;
        private string NombreClase { get; set; }
        public List<Column> columnas { get; set; }
        public Column pk { get { return columnas.Find(c => c.primaryKey); } }
        public List<Column> fk { get { return columnas.Where<Column>(c => c.foreignKey).ToList<Column>(); } }
        public List<Column> uk { get { return columnas.Where<Column>(c => c.uniqueKey).ToList<Column>(); } }

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

        public void addColumn(DataRow _dr) {
            columnas.Add(new Column(_dr));
        }

        public void addColumn(Column _col) {
            columnas.Add(_col);
        }

        public override string ToString() {
            return tabla;
        }
    }
}