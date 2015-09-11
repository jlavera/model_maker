using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ModelMaker.Utils {
    public interface IBO<T> where T: new() {

        T setData(DataRow dr);

    }
}