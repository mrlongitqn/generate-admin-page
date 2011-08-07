using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class Controller : BaseGenerate
    {
        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;
            GlobalVariables.g_ModelName = _table.Name;

            string Result = "";

            Result += GenerateNameSpace("ViewModels");

            return Result;
        }

        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using System.Web.Mvc;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;

            Result += TAB + "}" + END;

            return Result;
        }
    }
}
