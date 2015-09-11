using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ###PROYECTO###.Models.BO{
    public interface IBO<T> where T: new() {

        T setData(DataRow dr);

    }
}