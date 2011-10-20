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

    public class EntityControllerWithoutImage : EntityController
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

            if (GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name) ||
                     GlobalVariables.g_colViewDetail.Contains(Tbl.Name))
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
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            if (lst.Count > 0)
            {
                Result += TAB2 + "public ActionResult Edit" + Tbl.Name + "(int page, " + Utils.BuildPKParams(lst) + ")" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page," + END;
                if (lst[0].Type == DataType.STRING)
                {
                    Result += TAB4 + "StrID = " + Utils.BuildPKParams2(lst) + END;
                }
                else if (lst[0].Type == DataType.GUILD)
                {
                    Result += TAB4 + "GuidID= " + Utils.BuildPKParams2(lst) + END;
                }
                else
                {
                    Result += TAB4 + "IntID = " + Utils.BuildPKParams2(lst) + END;
                }
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return PartialView(\"Templates/TH_Edit" + Tbl.Name + "\", CreateViewModel(data));" + END;
                Result += TAB2 + "}" + END;
            }
            return Result;
        }

        public string GenerateCancelActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult CancelEditing" + Tbl.Name + "(int page)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = page" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateUpdateActionResult()
        {
            string Result = "";
            Result += GenerateUpdateActionResultWithouImage() + END;
            return Result;
        }

        public override string GenerateInsertActionResult()
        {
            string Result = "";
            Result += GenerateInsertActionResultWithoutImage() + END;
            return Result;
        }

        public string GenerateInsertActionResultWithoutImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + Tbl.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsPrimaryKey)
                {
                    if (!Tbl.Attributes[i].IsIdentify)
                    {
                        Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
                    }
                }
                else
                {
                    Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
                }
            }

            Result += TAB3 + "var newItem = new " + Tbl.Name + END;
            Result += TAB3 + "{" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsPrimaryKey && Tbl.Attributes[i].IsIdentify)
                    continue;
                var attr = "";
                if (Utils.GetDataType(Tbl.Attributes[i].Type) != "string")
                {
                    if (Tbl.Attributes[i].Type == DataType.DATETIME)
                    {
                        attr = Tbl.Attributes[i].Name.ToLower() + " == \"\" ? DateTime.Now : " + Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ")";
                    }
                    else
                    {
                        attr = Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ")";
                    }
                }
                else
                    attr = Tbl.Attributes[i].Name.ToLower();

                if (i < Tbl.Attributes.Count - 1)
                    Result += TAB4 + Tbl.Attributes[i].Name + " = " + attr + "," + END;
                else
                    Result += TAB4 + Tbl.Attributes[i].Name + " = " + attr + END;
            }
            Result += TAB3 + "};" + END;
            Result += TAB3 + "var result = _rep" + Tbl.Name + ".Insert(newItem);" + END;

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            var lst = Utils.GetForeighKeyList(Tbl);
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].Type == DataType.STRING)
                {
                    Result += TAB4 + lst[i].Name + " = " + lst[i].Name.ToLower() + "," + END;
                }
                else if (lst[i].Type == DataType.BOOL)
                {
                    Result += TAB4 + lst[i].Name + " = bool.Parse(" + lst[i].Name.ToLower() + ")," + END;
                }
                else if (lst[i].Type == DataType.DATETIME)
                {
                    Result += TAB4 + lst[i].Name + " = " + lst[i].Name.ToLower() + " == \"\" ? DateTime.Now : DateTime.Parse(" + lst[i].Name.ToLower() + ")," + END;
                }
                else if (lst[i].Type == DataType.INT)
                {
                    Result += TAB4 + lst[i].Name + " = int.Parse(" + lst[i].Name.ToLower() + ")," + END;
                }
                else if (lst[i].Type == DataType.DOUBLE)
                {
                    Result += TAB4 + lst[i].Name + " = double.Parse(" + lst[i].Name.ToLower() + ")," + END;
                }
            }

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;
            Result += TAB4 + "InfoText = result ? \"New item has been added\" : \"Cannot insert new item\"" + END;
            Result += TAB3 + "};" + END;
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name))
            {
                if (GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
                {
                    Result += TAB3 + "return Json(new{" + END;
                    Result += TAB4 + "Success = result," + END;
                    Result += TAB4 + "Message = \"A new item has been added!\"," + END;
                    Result += TAB4 + "PartialViewHtml = RenderPartialViewToString(\"Templates/TH_List" + Tbl.Name + "\", CreateViewModel(data))" + END;
                    Result += TAB3 + "});" + END;
                }
                else
                {
                    Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
                }
            }
            else
            {
                Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateUpdateActionResultWithouImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Update" + Tbl.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
            }
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name))
            {
                if (GlobalVariables.g_colPaging.Contains(Tbl.Name))
                {
                    Result += TAB3 + "string currentpage = forms.GetValues(\"" + Tbl.Name + "_CurrentPage\")[0];" + END;
                }
            }
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            //need editing
            Result += TAB3 + "var " + Tbl.Name.ToLower() + " = _rep" + Tbl.Name + ".SelectByID(" + Utils.BuildCastingPKParams(lst) + ");" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].Name.ToLower() != "id")
                {
                    if (Utils.GetDataType(Tbl.Attributes[i].Type) == "DateTime")
                        Result += TAB3 + Tbl.Name.ToLower() + "." + Tbl.Attributes[i].Name + " = DateTime.Now;" + END;
                    else
                    {
                        if (Tbl.Attributes[i].Type != DataType.STRING)
                        {
                            if (Tbl.Attributes[i].Type == DataType.DATETIME)
                            {
                                Result += TAB3 + "editItem." + Tbl.Attributes[i].Name + " = " + Tbl.Attributes[i].Name.ToLower() + " == \"\" ? DateTime.Now :" + Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ");" + END;
                            }
                            else
                            {
                                Result += TAB3 + Tbl.Name.ToLower() + "." + Tbl.Attributes[i].Name + " = " + Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ".Replace(\".\", \"\").Replace(\",\", \"\"));" + END;
                            }
                        }
                        else
                            Result += TAB3 + Tbl.Name.ToLower() + "." + Tbl.Attributes[i].Name + " = " + Tbl.Attributes[i].Name.ToLower() + ";" + END;
                    }
                }
            }
            Result += TAB3 + "var result = _rep" + Tbl.Name + ".Save();" + END;
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name))
            {
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + Tbl.Name.ToUpper() + "," + END;

                if (GlobalVariables.g_colPaging.Contains(Tbl.Name))
                {
                    Result += TAB4 + "CurrentPage = int.Parse(currentpage)" + END;
                }

                Result += TAB3 + "};" + END;
                if (GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
                {
                    Result += TAB3 + "return PartialView(\"Templates/TH_List" + Tbl.Name + "\", CreateViewModel(data));" + END;
                }
                else
                {
                    Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
                }
            }
            else
            {
                lst = Utils.PKHaveMoreThan1Attribute(Tbl);

                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + Tbl.Name.ToUpper() + "," + END;

                if (lst[0].Type == DataType.STRING)
                {
                    Result += TAB4 + "StrID = " + Utils.BuildPKParams2(lst) + "," + END;
                }
                else
                {
                    Result += TAB4 + "IntID = " + Utils.GetDataType(lst[0].Type) + ".Parse(" + Utils.BuildPKParams2(lst) + ")," + END;
                }

                Result += TAB4 + "InfoText = result ? \"Item has been updated\" : \"Cannot update this item\"" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return View(\"DetailOf" + Tbl.Name + "\", CreateViewModel(data));" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
