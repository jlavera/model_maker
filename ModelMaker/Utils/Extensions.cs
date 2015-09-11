using ModelMaker.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ModelMaker.Utils {
    public static class Extensions {

        public static List<T> ToList<T>(this DataTable dt) where T : IBO<T>, new() {
            List<T> lista = new List<T>();
            foreach (DataRow dr in dt.Rows)
                lista.Add(new T().setData(dr));

            return lista;
        }

        public static TipoDato GetByDB(this List<TipoDato> list, string _db) {
            var item = list.Find(t => t.db_name == _db);
            if (item == null)
                throw new MyException("El tipo de dato " + _db + " no es soportado");

            return item;
        }

    }
}
