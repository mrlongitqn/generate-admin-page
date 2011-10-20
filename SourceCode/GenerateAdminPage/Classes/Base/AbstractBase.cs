using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Base
{
    #region USING
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    /// <summary>
    /// Abstract base class: implement some inheritance methods and some mothod to child classes overritance
    /// </summary>
    public abstract class AbstractBase: IBase
    {
        #region CONSTANTS
        public string END = Environment.NewLine;
        public const string TAB = "\t";
        public const string TAB2 = "\t\t";
        public const string TAB3 = "\t\t\t";
        public const string TAB4 = "\t\t\t\t";
        public const string TAB5 = "\t\t\t\t\t";
        public const string TAB6 = "\t\t\t\t\t\t";
        public const string TAB7 = "\t\t\t\t\t\t\t";
        public const string TAB8 = "\t\t\t\t\t\t\t\t";
        public const string TAB9 = "\t\t\t\t\t\t\t\t\t";
        #endregion

        #region PROPERTIES
        public DataBase     DB { get; set; }
        public Table        Tbl { get; set; }
        public bool         TblHaveImgAttr { get; set; }
        public bool         TblUsingFck { get; set; }
        #endregion

        #region METHODS
        public void InitClassGenerate(DataBase db, Table tbl)
        {
            DB = db;
            Tbl = tbl;
            TblUsingFck = Utils.TableUsingFCK(Tbl);
            TblHaveImgAttr = Utils.TableHaveImageAttribute(Tbl);
        }

        public virtual string GenerateNameSpace(string nameSpace)
        {
            string Result = "";

            Result += "using System;" + END;
            Result += "using System.Collections.Generic;" + END;
            Result += "using System.Linq;" + END;
            Result += "namespace " + GlobalVariables.g_sNameSpace + "." + nameSpace + END;
            Result += "{" + END;
            Result += GenerateUsingRegion();
            Result += GenerateClass();
            Result += "}";

            return Result;
        }

        /// <summary>
        /// Method is overrided by child classes
        /// </summary>
        /// <returns>Return a string that present using region</returns>
        public abstract string GenerateUsingRegion();

        /// <summary>
        /// Method is overrided by child classes
        /// </summary>
        /// <returns>Return a string that present body of class</returns>
        public abstract string GenerateClass();

        public virtual string GenerateView(EnumView _type)
        {
            return string.Empty;
        }
        #endregion
    }
}
