using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelMaker.Utils {
    public static class PathsMngr {

        public static string OutPutFolder { get; set; }
        public static string OutPutBOFolder { get { return OutPutFolder + "/BO/"; } }
        public static string OutPutIBOFile { get { return OutPutBOFolder + "IBO.cs"; } }
        public static string OutPutDAOFolder { get { return OutPutFolder + "/DAO/"; } }
        public static string OutPutDAOBaseFile { get { return OutPutDAOFolder + "DAOBase.cs"; } }
        public static string OutPut_DAOFolder { get { return OutPutFolder + "/_DAO/"; } }
        public static string OutPut_BOFolder { get { return OutPutFolder + "/_BO/"; } }
        public static string OutPutUtilsFolder { get { return OutPutFolder + "/Utils/"; } }
        public static string OutPutDBFile { get { return OutPutUtilsFolder + "DB.cs"; } }
        public static string OutPutExcFile { get { return OutPutUtilsFolder + "MyException.cs"; } }

        public static string SourceFolder { get { return AppDomain.CurrentDomain.BaseDirectory + "/Sources/"; } }
        public static string SourceDAOBase { get { return SourceFolder + "DAOBase.cs"; } }
        public static string SourceDAOFile { get { return SourceFolder + "DAOEx"; } }
        public static string Source_DAOFile { get { return SourceFolder + "_DAOEx"; } }
        public static string SourceBOFile { get { return SourceFolder + "BOEx"; } }
        public static string Source_BOFile { get { return SourceFolder + "_BOEx"; } }
        public static string SourceIBO { get { return SourceFolder + "IBO.cs"; } }
        public static string SourceDBFile { get { return SourceFolder + "DB.cs"; } }
        public static string SourceEXCFile { get { return SourceFolder + "MyException.cs"; } }
    }
}
