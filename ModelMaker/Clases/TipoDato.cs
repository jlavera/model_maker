using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelMaker.Clases {
    public class TipoDato {
        public string db_name { get; set; }
        public string cs_name { get; set; }
        public string cs_opt_name { get; set; }

        public TipoDato(string _db, string _cs, string _opt) {
            db_name = _db;
            cs_name = _cs;
            cs_opt_name = _opt;
        }
    }
}
