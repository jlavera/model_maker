using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelMaker.Utils {
    public static class SectionsHandler {

        //--Las secciones se levantan una sola vez y quedan cacheadas
        private static Dictionary<string, string> DAOCache = new Dictionary<string, string>();
        private static Dictionary<string, string> BOCache = new Dictionary<string, string>();

        public static string GetDAOSection(string section) {
            if (!DAOCache.ContainsKey(section))
                DAOCache.Add(section, GetSection(PathsMngr.SourceDAOFile, section));

            return DAOCache[section];
        }

        public static string GetBOSection(string section) {
            if (!BOCache.ContainsKey(section))
                BOCache.Add(section, GetSection(PathsMngr.SourceBOFile, section));

            return BOCache[section];
        }

        public static string GetSection(string file, string section) {
            string temp = null;
            string aux;

            StreamReader sr = new StreamReader(file);

            //--Buscar comienzo
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@" + section + "@@@")) ;

            //--Copiar hasta fin
            while (((aux = sr.ReadLine()) != null) && (aux.Trim() != "@@@END" + section + "@@@")) temp += aux + '\n';

            sr.Close(); sr.Dispose();

            return temp;
        }
    }
}
