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

    public abstract class EntityController : AbstractController
    {
        public override string GenerateSelectActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult Select" + Tbl.Name + "()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectActionResultPaging()
        {
            string Result = "";
            if (GlobalVariables.g_colPaging.Contains(Tbl.Name))
            {
                Result += TAB2 + "public ActionResult Select" + Tbl.Name + "Paging(int page = 1)" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;

                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return PartialView(\"Templates/TH_List" + Tbl.Name + "\", CreateViewModel(data));" + END;
                Result += TAB2 + "}" + END;
            }
            return Result;
        }

        public virtual string GenerateDetailActionResult()
        {
            string Result = "";
            if (Tbl == null)
            {
                return Result;
            }

            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            Result += TAB2 + "public ActionResult DetailOf" + Tbl.Name + "(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + Tbl.Name.ToUpper() + "," + END;

            if (lst[0].Type == DataType.STRING)
                Result += TAB4 + "StrID = " + Utils.BuildPKParams2(lst) + END;
            else if (lst[0].Type == DataType.INT)
                Result += TAB4 + "IntID = " + Utils.BuildPKParams2(lst) + END;

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;
            return Result;
        }

        public string GenerateSelectByFK(List<Attribute> FK)
        {
            string Result = "";

            Result += TAB2 + "public ActionResult Select" + Tbl.Name + "By" + FK[0].ReferTo + "(" + Utils.BuildFKParams(FK) + "int page = 1)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            for (int i = 0; i < FK.Count; i++)
            {
                Result += TAB4 + FK[i].Name + " = " + FK[i].Name.ToLower() + "," + END;
            }
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = page" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFK(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += TAB2 + "public ActionResult Select" + Tbl.Name + "By" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page = 1)" + END;
            }
            else
            {
                Result += TAB2 + "public ActionResult Select" + Tbl.Name + "By" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ")" + END;
            }

            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + FK.Name + " = " + FK.Name.ToLower() + "," + END;

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += TAB4 + "CurrentPage = page" + END;
            }

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFKPaging(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += TAB2 + "public ActionResult Select" + Tbl.Name + "By" + FK.Name + "Paging(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page = 1)" + END;
            }
            else
            {
                Result += TAB2 + "public ActionResult Select" + Tbl.Name + "By" + FK.Name + "Paging(" + Utils.GetDataType(FK.Type) + " " + idFK + ")" + END;
            }

            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + FK.Name + " = " + FK.Name.ToLower() + "," + END;

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += TAB4 + "CurrentPage = page" + END;
            }

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFKs()
        {
            string Result = "";

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    //generate select by foreign key
                    Result += GenerateSelectByFK(Tbl.Attributes[i]) + END;
                    Result += GenerateSelectByFKPaging(Tbl.Attributes[i]) + END;
                }
            }
            return Result;
        }

        public override string GenerateDeleteActionResult()
        {
            string Result = "";
            if (GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
            {
                var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
                Result += TAB2 + "[AcceptVerbs(HttpVerbs.Delete)]" + END;
                Result += TAB2 + "public JsonResult Delete" + Tbl.Name + "(" + Utils.BuildPKParams(lst) + ")" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "return Json(new{" + END;
                Result += TAB4 + "Success = _rep" + Tbl.Name + ".Delete(" + Utils.BuildPKParams2(lst) + ")," + END;
                Result += TAB4 + "RecordCount = _rep" + Tbl.Name + ".SelectAll().Count," + END;
                Result += TAB4 + "DeleteId = " + Utils.BuildPKParams2(lst) + END;
                Result += TAB3 + "});" + END;

                Result += TAB2 + "}" + END;
            }
            else
            {
                var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
                Result += TAB2 + "public ActionResult Delete" + Tbl.Name + "(" + Utils.BuildPKParams(lst) + ", int page = 1)" + END;
                Result += TAB2 + "{" + END;

                Result += TAB3 + "var result = _rep" + Tbl.Name + ".Delete(id);" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "InfoText = result ? \"Item has been deleted\" : \"Cannot delete item!\"," + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;

                Result += TAB2 + "}" + END;
            }
            return Result;
        }
    }
}
