using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ModelMaker.Clases {
    public interface IItem<T> where T: new() {

        T setData(DataRow dr);

    }
}