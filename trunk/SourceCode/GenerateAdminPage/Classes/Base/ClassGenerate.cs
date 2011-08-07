using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public abstract class ClassGenerate: BaseGenerate
    {
        public string GenerateNameSpace(string nameSpace)
        {
            string Result = "";

            Result += "using System;" + END;
            Result += "using System.Collections.Generic;" + END;
            Result += "using System.Linq;" + END;
            Result += "using System.Web;" + END + END;
            Result += "namespace " + GlobalVariables.g_sNameSpace + "." + nameSpace + END;
            Result += "{" + END;

            //generate #region using
            Result += GenerateUsingRegion();
            //generate class
            Result += GenerateClass();

            Result += "}";

            return Result;
        }

        public abstract string GenerateUsingRegion();
        public abstract string GenerateClass();
        public virtual string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;
            TblUsingFck = Utils.TableUsingFCK(_table);
            TblHaveImgAttr = Utils.TableHaveImageAttribute(_table);

            if (_tbl != null)
                GlobalVariables.g_ModelName = _tbl.Name;
            else
                GlobalVariables.g_ModelName = GlobalVariables.g_sTableNguoiDung;

            string Result = "";

            Result += GenerateNameSpace("Controllers");

            return Result;
        }
    }
}
