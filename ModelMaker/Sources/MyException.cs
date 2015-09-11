using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ###PROYECTO###.Models.Utils{
    public class MyException : Exception {

        private Exception exception;

        public MyException(Exception _ex) {
            exception = _ex;
        }
		
        public MyException(string _msg) {
            exception = new Exception(_msg);
        }

        public override System.Collections.IDictionary Data {
            get {
                return exception.Data;
            }
        }

        public override string Message {
            get {
                return exception.Message;
            }
        }

        public override string Source {
            get {
                return exception.Source;
            }
            set {
                exception.Source = value;
            }
        }

        public override string StackTrace {
            get {
                return exception.StackTrace;
            }
        }

        public override string ToString() {
            return exception.ToString();
        }

    }
}