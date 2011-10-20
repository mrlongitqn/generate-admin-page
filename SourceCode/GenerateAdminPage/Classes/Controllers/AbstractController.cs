using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Controllers
{
    #region USING
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    public abstract class AbstractController : AbstractBase
    {
        /// <summary>
        /// Method is responsible for select generate type such as: view, model or repository
        /// </summary>
        /// <returns>Correspding generate class</returns>
        public static AbstractBase SelectGenerateType(Table tbl)
        {
            if (tbl == null)
            {
                return new NguoiDungController();
            }
            else
            {
                if (tbl.Name.ToUpper() == GlobalVariables.g_sTableNguoiDung.ToUpper())
                {
                    return new NguoiDungController();
                }
                else
                {
                    if (Utils.TableHaveImageAttribute(tbl))
                    {
                        return new EntityControllerWithImage(); 
                    }
                    else
                    {
                        return new EntityControllerWithoutImage();
                    }
                }
            }
        }

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

        public abstract string GenerateSelectActionResult();
        public abstract string GenerateSelectActionResultPaging();
        public abstract string GenerateUpdateActionResult();
        public abstract string GenerateDeleteActionResult();
        public abstract string GenerateInsertActionResult();
    }
}
