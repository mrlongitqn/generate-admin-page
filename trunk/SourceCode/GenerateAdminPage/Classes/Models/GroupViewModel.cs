using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class GroupViewModel: ClassGenerate
    {
        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;

            string Result = "";

            Result += GenerateNameSpace("Models.ViewModels");

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

            Result += TAB + "public class GroupViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public LogOnModel LogOnModel { get; set; }" + END;
            Result += TAB2 + "public RegisterModel RegisterModel { get; set; }" + END;
            Result += TAB2 + "public " + GlobalVariables.g_sTableNguoiDung + "ViewModel " + GlobalVariables.g_sTableNguoiDung + "Model { get; set; }" + END;
            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && _db.Tables[i].Name != "aspnet_Users")
                {
                    Result += TAB2 + "public " + _db.Tables[i].Name + "ViewModel " + _db.Tables[i].Name + "Model { get; set; }" + END;
                }
            }
            Result += TAB + "}" + END;

            return Result;
        }
    }
}
