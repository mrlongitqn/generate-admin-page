using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class AdminController : ClassGenerate
    {
        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using System.Web.Mvc;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.ViewModels;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.Repositories;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "using System.Web.Security;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "[Authorize(Roles= \"SuperAdmin, Admin\")]" + END;
            Result += TAB + "public partial class AdminController: BaseController" + END;
            Result += TAB + "{" + END;

            Result += TAB2 + "public ActionResult Index()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return null;" + END;
            Result += TAB2 + "}" + END;

            Result += TAB + "}" + END;

            return Result;
        }
    }
}
