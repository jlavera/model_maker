using ModelMaker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelMaker.Clases {
    public class Controles {
        public string Direccion { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public void Vaidate() {
            if (Direccion == "" || Database == "" || User == "" || Password == "")
                throw new MyException("Falta completar campos");
        }
    }
}