using ModelMaker.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelMaker.Utils {
    public static class TiposDeDato {

        private static List<TipoDato> tiposDeDato = new List<TipoDato>(){
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

        public static TipoDato GetByDBType(string _tipo) {
            var tipo = tiposDeDato.Find(t => t.db_name == _tipo);
            if (tipo == null) throw new MyException("Tipo de dato no soportado.");
            return tipo;
        }

    }
}
