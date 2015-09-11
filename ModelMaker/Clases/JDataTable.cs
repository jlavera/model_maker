using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ModelMaker.Clases {
    public class JDataTable : DataTable{

        public List<T> ToList<T>() where T: IBO<T>, new(){
            List<T> lista = new List<T>();
            foreach (DataRow dr in Rows)
                lista.Add(new T().setData(dr));

            return lista;
        }

    }
}
