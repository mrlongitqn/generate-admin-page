using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class NguoiDungViewModel : ViewModel
    {
        public Table _BaseInfo { get; set; }

        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _table = _tbl;
            _db = _database;

            if (_tbl != null)
            {
                GlobalVariables.g_ModelName = _tbl.Name;
            }
            else
            {
                GlobalVariables.g_ModelName = GlobalVariables.g_sTableNguoiDung;
            }

            string Result = "";

            Result += GenerateNameSpace("Models.ViewModels");

            return Result;
        }

        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using System.ComponentModel;" + END;
            Result += TAB + "using System.ComponentModel.DataAnnotations;" + END;
            Result += TAB + "using System.Web.Mvc;" + END;
            Result += TAB + "using System.Web.Security;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public Get" + GlobalVariables.g_ModelName + "ViewModel GetModel { get; set; }" + END;
            Result += TAB2 + "public Edit" + GlobalVariables.g_ModelName + "ViewModel EditModel { get; set; }" + END;
            Result += TAB2 + "public Add" + GlobalVariables.g_ModelName + "ViewModel AddModel { get; set; }" + END;
            Result += TAB2 + "public string InfoText { get; set; }" + END;
            Result += TAB + "}" + END;
            Result += GenerateGetViewModel() + END;
            Result += GenerateEditViewModel() + END;
            Result += GenerateAddViewModel() + END;
            Result += GenerateObjectInfo() + END;
            Result += GenerateObjectBaseInfo() + END;
            return Result;
        }

        public override string GenerateGetViewModel()
        {
            string Result = "";

            Result += TAB + "public class Get" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "Info> LstObjModel { get; set; }" + END;
            Result += TAB2 + "public int TotalItem { get; set; }" + END;
            Result += TAB2 + "public int CurrentPage { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateEditViewModel()
        {
            string Result = "";

            Result += TAB + "public class Edit" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public Guid ID { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateAddViewModel()
        {
            string Result = "";

            Result += TAB + "public class Add" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public " + GlobalVariables.g_ModelName + "Info Info { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateObjectInfo()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public " + GlobalVariables.g_ModelName + "BaseInfo BaseInfo { get; set; }" + END;
            if(_table != null)
                Result += TAB2 + "public " + GlobalVariables.g_ModelName + " ExtraInfo { get; set; }" + END;

            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateObjectBaseInfo()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "BaseInfo" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public Guid ID { get; set; }" + END;
            Result += TAB2 + "public string UserName { get; set; }" + END;
            Result += TAB2 + "public string Email { get; set; }" + END;
            Result += TAB2 + "public string Password { get; set; }" + END;
            Result += TAB2 + "public string Role { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }
    }
}
