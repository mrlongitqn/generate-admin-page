using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class ViewModel: ClassGenerate, IViewModel
    {
        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;
            GlobalVariables.g_ModelName = _table.Name;

            string Result = "";

            Result += GenerateNameSpace("Models.ViewModels");

            return Result;
        }

        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using System.Web.Mvc;" + END;
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
            Result += TAB2 + "public " + _table.Name + "ReferKeys ReferKeys { get; set; }" + END;
            Result += TAB + "}" + END;
            Result += GenerateGetViewModel() + END;
            Result += GenerateEditViewModel() + END;
            Result += GenerateAddViewModel() + END;
            Result += GenerateReferKeys() + END;

            return Result;
        }

        public string GenerateReferKeys()
        {
            string Result = "";

            Result += TAB + "public class " + _table.Name + "ReferKeys" + END;
            Result += TAB + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "public " + Utils.GetDataType(_table.Attributes[i].Type) + " " + _table.Attributes[i].Name + ";" + END;
                }
            }

            Result += TAB + "}" + END;

            return Result;
        }

        public virtual string GenerateGetViewModel()
        {
            string Result = "";

            Result += TAB + "public class Get" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "> LstObjModel { get; set; }" + END;
            Result += TAB2 + "public int TotalItem { get; set; }" + END;
            Result += TAB2 + "public int CurrentPage { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public virtual string GenerateEditViewModel()
        {
            string Result = "";

            Result += TAB + "public class Edit" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public " + Utils.GetDataType(Utils.PKHaveMoreThan1Attribute(_table)[0].Type) + " ID { get; set; }" + END;
            Result += TAB2 + "public bool Edited { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public virtual string GenerateAddViewModel()
        {
            string Result = "";

            Result += TAB + "public class Add" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey)
                {
                    Result += TAB2 + "public " + Utils.GetDataType(_table.Attributes[i].Type) + " " + _table.Attributes[i].Name + " { get; set; }" + END;
                }
            }

            Result += TAB2 + "public bool Added { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public virtual string GenerateObjectInfo()
        {
            string Result = "";
            return Result;
        }

    }
}
