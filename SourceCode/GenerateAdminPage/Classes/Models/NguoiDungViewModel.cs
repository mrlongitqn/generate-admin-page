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

    public class NguoiDungViewModel : AbstractViewModel
    {
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

            Result += TAB + "public class " + GlobalVariables.g_sTableNguoiDung + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public Get" + GlobalVariables.g_sTableNguoiDung + "ViewModel GetModel { get; set; }" + END;
            Result += TAB2 + "public Edit" + GlobalVariables.g_sTableNguoiDung + "ViewModel EditModel { get; set; }" + END;
            Result += TAB2 + "public Add" + GlobalVariables.g_sTableNguoiDung + "ViewModel AddModel { get; set; }" + END;
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

            Result += TAB + "public class Get" + GlobalVariables.g_sTableNguoiDung + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public List<" + GlobalVariables.g_sTableNguoiDung + "Info> LstObjModel { get; set; }" + END;
            Result += TAB2 + "public int TotalItem { get; set; }" + END;
            Result += TAB2 + "public int CurrentPage { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateEditViewModel()
        {
            string Result = "";

            Result += TAB + "public class Edit" + GlobalVariables.g_sTableNguoiDung + "ViewModel" + END;
            Result += TAB + "{" + END;
            if (Tbl != null)
            {
                Result += TAB2 + "public " + Utils.GetDataType(Utils.GetPK(Tbl).Type) + " ID { get; set; }" + END;
            }
            else
            {
                Result += TAB2 + "public Guid ID { get; set; }" + END;
            }
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateAddViewModel()
        {
            string Result = "";

            Result += TAB + "public class Add" + GlobalVariables.g_sTableNguoiDung + "ViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public " + GlobalVariables.g_sTableNguoiDung + "Info Info { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateObjectInfo()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_sTableNguoiDung + "Info" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public " + GlobalVariables.g_sTableNguoiDung + "BaseInfo BaseInfo { get; set; }" + END;
            if (Tbl != null)
            {
                Result += TAB2 + "public " + GlobalVariables.g_sTableNguoiDung + " ExtraInfo { get; set; }" + END;
            }
            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateObjectBaseInfo()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_sTableNguoiDung + "BaseInfo" + END;
            Result += TAB + "{" + END;
            if (Tbl != null)
            {
                Result += TAB2 + "public " + Utils.GetDataType(Utils.GetPK(Tbl).Type) + " ID { get; set; }" + END;
            }
            else
            {
                Result += TAB2 + "public Guid ID { get; set; }" + END;
            }
            Result += TAB2 + "public string UserName { get; set; }" + END;
            Result += TAB2 + "public string Email { get; set; }" + END;
            Result += TAB2 + "public string Password { get; set; }" + END;
            Result += TAB2 + "public string Role { get; set; }" + END;
            Result += TAB + "}" + END;

            return Result;
        }
    }
}
