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

    public class GroupViewModel: AbstractBase
    {
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
            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                    !DB.Tables[i].Name.StartsWith("SLM_") && !DB.Tables[i].Name.StartsWith("aspnet_") &&
                    DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                {
                    Result += TAB2 + "public " + DB.Tables[i].Name + "ViewModel " + DB.Tables[i].Name + "Model { get; set; }" + END;
                }
            }
            Result += TAB + "}" + END;

            return Result;
        }
    }
}
