using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelMaker.Utils {
    public class MyException : Exception {
        public MyException(string msg): base(msg) {
        }
    }
}
