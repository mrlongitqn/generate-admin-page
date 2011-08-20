using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class AdminModelsControllerWithoutImage : AdminModelsController, IAdminControllerWithoutImage
    {
        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public partial class AdminController: BaseController" + END;
            Result += TAB + "{" + END;

            Result += GenerateSelectActionResult() + END;
            Result += GenerateSelectActionResultPaging() + END;
            Result += GenerateSelectByFKs() + END;
            Result += GenerateDeleteActionResult() + END;

            if (GlobalVariables.g_colUsingFCK.Keys.Contains(_table.Name) ||
                     GlobalVariables.g_colViewDetail.Contains(_table.Name))
            {
                Result += GenerateDetailActionResult() + END;
            }
            else
            {
                Result += GenerateEditActionResult() + END;
                Result += GenerateCancelActionResult() + END;
            }

            Result += GenerateUpdateActionResult() + END;
            Result += GenerateInsertActionResult() + END;             

            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateEditActionResult()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);

            Result += TAB2 + "public ActionResult Edit" + _table.Name + "(int page, " + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + _table.Name.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = page," + END;
            if (lst[0].Type == DataType.STRING)
                Result += TAB4 + "StrID = " + Utils.BuildPKParams2(lst) + END;
            else if(lst[0].Type == DataType.GUILD)
                Result += TAB4 + "GuidID= " + Utils.BuildPKParams2(lst) + END;
            else
                Result += TAB4 + "IntID = " + Utils.BuildPKParams2(lst) + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_Edit" + _table.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateCancelActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult CancelEditing" + _table.Name + "(int page)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + _table.Name.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = page" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + _table.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

    }
}
