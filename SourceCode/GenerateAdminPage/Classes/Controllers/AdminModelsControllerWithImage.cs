using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class AdminModelsControllerWithImage : AdminModelsController, IAdminControllerWithImage
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
            Result += GenerateDetailActionResult() + END;
            Result += GenerateUpdateActionResult() + END;
            Result += GenerateInsertActionResult() + END;

            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateDeleteActionResult()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Delete)]" + END;
            Result += TAB2 + "public JsonResult Delete" + GlobalVariables.g_ModelName + "(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;

            Result += TAB3 + "var deleteItem = _rep" + GlobalVariables.g_ModelName + ".SelectByID(" + Utils.GetPKWith1Attr(_table).ToLower() + ");" + END;
            Result += TAB3 + "var fileName = deleteItem." + Utils.GetImageAttrName(_table) + ";" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB3 + "var " + _table.Attributes[i].Name.ToLower() + " = deleteItem." + _table.Attributes[i].Name + ";" + END;
                }
            }

            Result += TAB3 + "var result = _rep" + GlobalVariables.g_ModelName + ".Delete(" + Utils.GetPKWith1Attr(_table).ToLower() + ");" + END;
            Result += TAB3 + "if (result)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "if (fileName != \"" + GlobalVariables.g_sNoImages + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "var fullPath = Server.MapPath(\"/Content/Images/Items/\" + fileName);" + END;
            Result += TAB5 + "DeleteFile(fullPath);" + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "}" + END;

            Result += TAB3 + "return Json(new{" + END;
            Result += TAB4 + "Success = result," + END;
            Result += TAB4 + "RecordCount = _rep" + GlobalVariables.g_ModelName + ".SelectAll().Count," + END;
            Result += TAB4 + "DeleteId = " + Utils.BuildPKParams2(lst) + END;
            Result += TAB3 + "});" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
