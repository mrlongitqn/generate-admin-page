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

    public class EntityControllerWithImage : EntityController
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
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Delete)]" + END;
            Result += TAB2 + "public JsonResult Delete" + Tbl.Name + "(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;

            Result += TAB3 + "var deleteItem = _rep" + Tbl.Name + ".SelectByID(" + Utils.GetPKWith1Attr(Tbl).ToLower() + ");" + END;
            Result += TAB3 + "var fileName = deleteItem." + Utils.GetImageAttrName(Tbl) + ";" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB3 + "var " + Tbl.Attributes[i].Name.ToLower() + " = deleteItem." + Tbl.Attributes[i].Name + ";" + END;
                }
            }

            Result += TAB3 + "var result = _rep" + Tbl.Name + ".Delete(" + Utils.GetPKWith1Attr(Tbl).ToLower() + ");" + END;
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
            Result += TAB4 + "RecordCount = _rep" + Tbl.Name + ".SelectAll().Count," + END;
            Result += TAB4 + "DeleteId = " + Utils.BuildPKParams2(lst) + END;
            Result += TAB3 + "});" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateUpdateActionResult()
        {
            string Result = "";
            Result += GenerateUpdateActionResultWithImage() + END;
            return Result;
        }

        public override string GenerateInsertActionResult()
        {
            string Result = "";
            Result += GenerateInsertActionResultWithImage() + END;
            return Result;
        }

        public string GenerateUpdateActionResultWithImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Update" + Tbl.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(Tbl.Attributes[i].Name))
                    continue;
                Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
            }

            Result += TAB3 + "var file = Request.Files[\"" + Tbl.Name + "_" + Utils.GetImageAttrName(Tbl) + "\"];" + END;

            Result += TAB3 + "string refName = \"\";" + END;

            Result += TAB3 + "var editItem = _rep" + Tbl.Name + ".SelectByID(int.Parse(" + Utils.GetPKWith1Attr(Tbl).ToLower() + "));" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(Tbl.Attributes[i].Name))
                    continue;
                if (Tbl.Attributes[i].Name.ToLower() != "id")
                {
                    if (Tbl.Attributes[i].Type != DataType.STRING)
                    {
                        if (Tbl.Attributes[i].Type == DataType.DATETIME)
                        {
                            Result += TAB3 + "editItem." + Tbl.Attributes[i].Name + " = " + Tbl.Attributes[i].Name.ToLower() + " == \"\" ? DateTime.Now :" + Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ");" + END;
                        }
                        else
                        {
                            Result += TAB3 + "editItem." + Tbl.Attributes[i].Name + " = " + Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ".Replace(\".\", \"\").Replace(\",\", \"\"));" + END;
                        }
                    }
                    else
                    {
                        Result += TAB3 + "editItem." + Tbl.Attributes[i].Name + " = " + Tbl.Attributes[i].Name.ToLower() + ";" + END;
                    }
                }
            }
            Result += TAB3 + "var result = false;" + END;
            Result += TAB3 + "if (file.ContentLength != 0)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "result = UploadFile(file, \"/Content/Images/Items/\", " + Utils.GetPKWith1Attr(Tbl).ToLower() + ", ref refName);" + END;
            Result += TAB4 + "editItem." + Utils.GetImageAttrName(Tbl) + " = refName;" + END;
            Result += TAB3 + "}" + END;

            Result += TAB3 + "result = _rep" + Tbl.Name + ".Save();" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + Tbl.Name.ToUpper() + "," + END;

            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);

            if (lst[0].Type == DataType.STRING)
            {
                Result += TAB4 + "StrID = " + Utils.BuildPKParams2(lst) + "," + END;
            }
            else
            {
                Result += TAB4 + "IntID = " + Utils.GetDataType(lst[0].Type) + ".Parse(" + Utils.BuildPKParams2(lst) + ".Replace(\".\", \"\").Replace(\",\", \"\"))," + END;
            }

            Result += TAB4 + "InfoText = result ? \"Item has been updated\" : \"Cannot update this item\"" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"DetailOf" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateInsertActionResultWithImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + Tbl.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsPrimaryKey && Tbl.Attributes[i].IsIdentify)
                    continue;
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(Tbl.Attributes[i].Name))
                    continue;
                Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
            }
            Result += TAB3 + "var file = Request.Files[\"" + Tbl.Name + "_" + Utils.GetImageAttrName(Tbl) + "\"];" + END;
            Result += TAB3 + "string refName = \"\";" + END;

            Result += TAB3 + "var newItem = new " + Tbl.Name + END;
            Result += TAB3 + "{" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsPrimaryKey && Tbl.Attributes[i].IsIdentify)
                    continue;
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(Tbl.Attributes[i].Name))
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

                Result += TAB4 + Tbl.Attributes[i].Name + " = " + attr + "," + END;
            }
            Result += TAB4 + Utils.GetImageAttrName(Tbl) + " = \"" + GlobalVariables.g_sNoImages + "\"" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "var result = _rep" + Tbl.Name + ".Insert(newItem);" + END;

            Result += TAB3 + "if (result)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "if (file.ContentLength != 0)" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "result = UploadFile(file, \"/Content/Images/Items/\", newItem." + Utils.GetPKWith1Attr(Tbl) + ".ToString(), ref refName);" + END;
            Result += TAB5 + "if (result)" + END;
            Result += TAB5 + "{" + END;
            Result += TAB6 + "var addedItem = _rep" + Tbl.Name + ".SelectByID(newItem." + Utils.GetPKWith1Attr(Tbl) + ");" + END;
            Result += TAB6 + "addedItem." + Utils.GetImageAttrName(Tbl) + " = refName;" + END;
            Result += TAB6 + "_rep" + Tbl.Name + ".Save();" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "}" + END;

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
            Result += TAB3 + "return View(\"Select" + Tbl.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
