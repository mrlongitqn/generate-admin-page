using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Models
{
    #region USING
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    public class EntityViewModel : AbstractViewModel
    {
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

            Result += TAB + "public class " + Tbl.Name + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public Get" + Tbl.Name + "ViewModel GetModel { get; set; }" + END;
            Result += TAB2 + "public Edit" + Tbl.Name + "ViewModel EditModel { get; set; }" + END;
            Result += TAB2 + "public Add" + Tbl.Name + "ViewModel AddModel { get; set; }" + END;
            Result += TAB2 + "public " + Tbl.Name + "ReferKeys ReferKeys { get; set; }" + END;
            Result += TAB2 + "public string InfoText { get; set; }" + END;
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

            Result += TAB + "public class " + Tbl.Name + "ReferKeys" + END;
            Result += TAB + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "public " + Utils.GetDataType(Tbl.Attributes[i].Type) + " " + Tbl.Attributes[i].Name + ";" + END;
                }
            }

            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateGetViewModel()
        {
            string Result = "";

            Result += TAB + "public class Get" + Tbl.Name + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public List<" + Tbl.Name + "> LstObjModel { get; set; }" + END;
            Result += TAB2 + "public int TotalItem { get; set; }" + END;
            Result += TAB2 + "public int CurrentPage { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateEditViewModel()
        {
            string Result = "";

            Result += TAB + "public class Edit" + Tbl.Name + "ViewModel" + END;
            Result += TAB + "{" + END;
            if (Utils.PKHaveMoreThan1Attribute(Tbl).Count > 0)
            {
                Result += TAB2 + "public " + Utils.GetDataType(Utils.PKHaveMoreThan1Attribute(Tbl)[0].Type) + " ID { get; set; }" + END;
            }
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateAddViewModel()
        {
            string Result = "";

            Result += TAB + "public class Add" + Tbl.Name + "ViewModel" + END;
            Result += TAB + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey)
                {
                    Result += TAB2 + "public " + Utils.GetDataType(Tbl.Attributes[i].Type) + " " + Tbl.Attributes[i].Name + " { get; set; }" + END;
                }
            }

            Result += TAB + "}" + END;

            return Result;
        }
    }
}
