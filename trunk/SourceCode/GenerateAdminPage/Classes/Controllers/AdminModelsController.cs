using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public abstract class AdminModelsController : AdminController, IAdminController
    {
        public virtual string GenerateSelectActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            
            //if (GlobalVariables.g_ModelName != "aspnet_Users")
            //{
            //    for (int i = 0; i < _table.Attributes.Count; i++)
            //    {
            //        if (_table.Attributes[i].IsForeignKey && !_table.Attributes[i].IsPrimaryKey)
            //            Result += TAB4 + _table.Attributes[i].Name + " = " + _table.Attributes[i].Name.ToLower() + "," + END;
            //    }
            //}

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateSelectActionResultPaging()
        {
            string Result = "";
            if (GlobalVariables.g_colPaging.Contains(GlobalVariables.g_ModelName))
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "Paging(int page = 1)" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;

                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return PartialView(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
                Result += TAB2 + "}" + END;
            }
            return Result;
        }

        public virtual string GenerateDetailActionResult()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);

            Result += TAB2 + "public ActionResult DetailOf" + _table.Name + "(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + _table.Name.ToUpper() + "," + END;

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

            Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "By" + FK[0].ReferTo + "(" + Utils.BuildFKParams(FK) + "int page = 1)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            for (int i = 0; i < FK.Count; i++)
            {
                Result += TAB4 + FK[i].Name + " = " + FK[i].Name.ToLower() + "," + END;
            }
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = page" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"Select" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFK(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), GlobalVariables.g_ModelName))
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "By" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page = 1)" + END;
            }
            else
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "By" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ")" + END;
            }

            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + FK.Name + " = " + FK.Name.ToLower() + "," + END;

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), GlobalVariables.g_ModelName))
            {
                Result += TAB4 + "CurrentPage = page" + END;
            }

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"Select" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFKPaging(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), GlobalVariables.g_ModelName))
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "By" + FK.Name + "Paging(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page = 1)" + END;
            }
            else
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "By" + FK.Name + "Paging(" + Utils.GetDataType(FK.Type) + " " + idFK + ")" + END;
            }

            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + FK.Name + " = " + FK.Name.ToLower() + "," + END;

            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), GlobalVariables.g_ModelName))
            {
                Result += TAB4 + "CurrentPage = page" + END;
            }

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByFKs()
        {
            string Result = "";

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    //generate select by foreign key
                    Result += GenerateSelectByFK(_table.Attributes[i]) + END;
                    Result += GenerateSelectByFKPaging(_table.Attributes[i]) + END;
                }
            }
            return Result;
        }

        public virtual string GenerateDeleteActionResult()
        {
            string Result = "";
            if (GlobalVariables.g_colUsingAjax.Contains(GlobalVariables.g_ModelName))
            {
                var lst = Utils.PKHaveMoreThan1Attribute(_table);
                Result += TAB2 + "[AcceptVerbs(HttpVerbs.Delete)]" + END;
                Result += TAB2 + "public JsonResult Delete" + GlobalVariables.g_ModelName + "(" + Utils.BuildPKParams(lst) + ")" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "return Json(new{" + END;
                Result += TAB4 + "Success = _rep" + GlobalVariables.g_ModelName + ".Delete(" + Utils.BuildPKParams2(lst) + ")," + END;
                Result += TAB4 + "RecordCount = _rep" + GlobalVariables.g_ModelName + ".SelectAll().Count," + END;
                Result += TAB4 + "DeleteId = " + Utils.BuildPKParams2(lst) + END;
                Result += TAB3 + "});" + END;

                Result += TAB2 + "}" + END;
            }
            else
            {
                var lst = Utils.PKHaveMoreThan1Attribute(_table);
                Result += TAB2 + "public ActionResult Delete" + GlobalVariables.g_ModelName + "(" + Utils.BuildPKParams(lst) + ", int page = 1)" + END;
                Result += TAB2 + "{" + END;

                Result += TAB3 + "var result = _rep" + GlobalVariables.g_ModelName + ".Delete(id);" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "InfoText = result ? \"Item has been deleted\" : \"Cannot delete item!\"," + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return View(\"Select" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;

                Result += TAB2 + "}" + END;
            }
            return Result;
        }

        public virtual string GenerateUpdateActionResult()
        {
            string Result = "";
            if (TblHaveImgAttr)
                Result += GenerateUpdateActionResultWithImage() + END;
            else
                Result += GenerateUpdateActionResultWithouImage() + END;
            return Result;
        }

        public virtual string GenerateInsertActionResult()
        {
            string Result = "";
            if (TblHaveImgAttr)
                Result += GenerateInsertActionResultWithImage() + END;
            else
                Result += GenerateInsertActionResultWithoutImage() + END;
            return Result;
        }

        public string GenerateInsertActionResultWithoutImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + _table.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsPrimaryKey)
                {
                    if (!_table.Attributes[i].IsIdentify)
                    {
                        Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\")[0];" + END;
                    }
                }
                else
                {
                    Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\")[0];" + END;
                }                  
            }

            Result += TAB3 + "var newItem = new " + _table.Name + END;
            Result += TAB3 + "{" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsPrimaryKey && _table.Attributes[i].IsIdentify)
                    continue;
                var attr = "";
                if (Utils.GetDataType(_table.Attributes[i].Type) != "string")
                    attr = Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ")";
                else
                    attr = _table.Attributes[i].Name.ToLower();

                if (i < _table.Attributes.Count - 1)
                    Result += TAB4 + _table.Attributes[i].Name + " = " + attr + "," + END;
                else
                    Result += TAB4 + _table.Attributes[i].Name + " = " + attr + END;
            }
            Result += TAB3 + "};" + END;
            Result += TAB3 + "var result = _rep" + _table.Name + ".Insert(newItem);" + END;

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            
            var lst = Utils.GetForeighKeyList(_table);
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
                    Result += TAB4 + lst[i].Name + " = DateTime.Parse(" + lst[i].Name.ToLower() + ")," + END;
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
            
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + _table.Name.ToUpper() + "," + END;
            Result += TAB4 + "InfoText = result ? \"New item has been added\" : \"Cannot insert new item\"" + END;
            Result += TAB3 + "};" + END;
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(_table.Name))
            {
                if (GlobalVariables.g_colUsingAjax.Contains(_table.Name))
                {
                    Result += TAB3 + "return Json(new{" + END;
                    Result += TAB4 + "Success = result," + END;
                    Result += TAB4 + "Message = \"A new item has been added!\"," + END;
                    Result += TAB4 + "PartialViewHtml = RenderPartialViewToString(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", CreateViewModel(data))" + END;
                    Result += TAB3 + "});" + END;
                }
                else
                {
                    Result += TAB3 + "return View(\"Select" + _table.Name + "\", CreateViewModel(data));" + END;
                }
            }
            else
            {
                Result += TAB3 + "return View(\"Select" + _table.Name + "\", CreateViewModel(data));" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateInsertActionResultWithImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + _table.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsPrimaryKey && _table.Attributes[i].IsIdentify)
                    continue;
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(_table.Attributes[i].Name))
                    continue;
                Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\")[0];" + END;
            }
            Result += TAB3 + "var file = Request.Files[\"" + _table.Name + "_" + Utils.GetImageAttrName(_table) + "\"];" + END;
            Result += TAB3 + "string refName = \"\";" + END;

            Result += TAB3 + "var newItem = new " + _table.Name + END;
            Result += TAB3 + "{" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsPrimaryKey && _table.Attributes[i].IsIdentify)
                    continue;
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(_table.Attributes[i].Name))
                    continue;
                var attr = "";
                if (Utils.GetDataType(_table.Attributes[i].Type) != "string")
                    attr = Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ")";
                else
                    attr = _table.Attributes[i].Name.ToLower();

                Result += TAB4 + _table.Attributes[i].Name + " = " + attr + "," + END;
            }
            Result += TAB4 + Utils.GetImageAttrName(_table) + " = \"" + GlobalVariables.g_sNoImages + "\"" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "var result = _rep" + _table.Name + ".Insert(newItem);" + END;

            Result += TAB3 + "if (result)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "if (file.ContentLength != 0)" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "result = UploadFile(file, \"/Content/Images/Items/\", newItem." + Utils.GetPKWith1Attr(_table) + ".ToString(), ref refName);" + END;
            Result += TAB5 + "if (result)" + END;
            Result += TAB5 + "{" + END;
            Result += TAB6 + "var addedItem = _rep" + _table.Name + ".SelectByID(newItem." + Utils.GetPKWith1Attr(_table) + ");" + END;
            Result += TAB6 + "addedItem." + Utils.GetImageAttrName(_table) + " = refName;" + END;
            Result += TAB6 + "_rep" + _table.Name + ".Save();" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "}" + END;

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;

            var lst = Utils.GetForeighKeyList(_table);
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
                    Result += TAB4 + lst[i].Name + " = DateTime.Parse(" + lst[i].Name.ToLower() + ")," + END;
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
            
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + _table.Name.ToUpper() + "," + END;
            Result += TAB4 + "InfoText = result ? \"New item has been added\" : \"Cannot insert new item\"" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(\"Select" + _table.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateUpdateActionResultWithImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Update" + _table.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(_table.Attributes[i].Name))
                    continue;
                Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\")[0];" + END;
            }

            Result += TAB3 + "var file = Request.Files[\"" + _table.Name + "_" + Utils.GetImageAttrName(_table) + "\"];" + END;

            Result += TAB3 + "string refName = \"\";" + END;

            Result += TAB3 + "var editItem = _rep" + _table.Name + ".SelectByID(int.Parse(" + Utils.GetPKWith1Attr(_table).ToLower() + "));" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (GlobalVariables.g_colTableHaveImage.Values.Contains(_table.Attributes[i].Name))
                    continue;
                if (_table.Attributes[i].Name.ToLower() != "id")
                {
                    if (_table.Attributes[i].Type != DataType.STRING)
                        Result += TAB3 + "editItem." + _table.Attributes[i].Name + " = " + Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ");" + END;
                    else
                        Result += TAB3 + "editItem." + _table.Attributes[i].Name + " = " + _table.Attributes[i].Name.ToLower() + ";" + END;
                }
            }
            Result += TAB3 + "var result = false;" + END;
            Result += TAB3 + "if (file.ContentLength != 0)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "result = UploadFile(file, \"/Content/Images/Items/\", " + Utils.GetPKWith1Attr(_table).ToLower() + ", ref refName);" + END;
            Result += TAB4 + "editItem." + Utils.GetImageAttrName(_table) + " = refName;" + END;
            Result += TAB3 + "}" + END;

            Result += TAB3 + "result = _rep" + _table.Name + ".Save();" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + _table.Name.ToUpper() + "," + END;

            var lst = Utils.PKHaveMoreThan1Attribute(_table);

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
            Result += TAB3 + "return View(\"DetailOf" + _table.Name + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateUpdateActionResultWithouImage()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Update" + _table.Name + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\")[0];" + END;
            }
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(_table.Name))
            {
                if (GlobalVariables.g_colPaging.Contains(_table.Name))
                {
                    Result += TAB3 + "string currentpage = forms.GetValues(\"" + _table.Name + "_CurrentPage\")[0];" + END;
                }
            }
            var lst = Utils.PKHaveMoreThan1Attribute(_table);
            //need editing
            Result += TAB3 + "var " + _table.Name.ToLower() + " = _rep" + _table.Name + ".SelectByID(" + Utils.BuildCastingPKParams(lst) + ");" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].Name.ToLower() != "id")
                {
                    if (Utils.GetDataType(_table.Attributes[i].Type) == "DateTime")
                        Result += TAB3 + _table.Name.ToLower() + "." + _table.Attributes[i].Name + " = DateTime.Now;" + END;
                    else
                    {
                        if (_table.Attributes[i].Type != DataType.STRING)
                            Result += TAB3 + _table.Name.ToLower() + "." + _table.Attributes[i].Name + " = " + Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ");" + END;
                        else
                            Result += TAB3 + _table.Name.ToLower() + "." + _table.Attributes[i].Name + " = " + _table.Attributes[i].Name.ToLower() + ";" + END;
                    }
                }
            }
            Result += TAB3 + "var result = _rep" + _table.Name + ".Save();" + END;
            if (!GlobalVariables.g_colUsingFCK.Keys.Contains(_table.Name))
            {
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + _table.Name.ToUpper() + "," + END;

                if (GlobalVariables.g_colPaging.Contains(_table.Name))
                {
                    Result += TAB4 + "CurrentPage = int.Parse(currentpage)" + END;
                }
                
                Result += TAB3 + "};" + END;
                if (GlobalVariables.g_colUsingAjax.Contains(_table.Name))
                {
                    Result += TAB3 + "return PartialView(\"Templates/TH_List" + _table.Name + "\", CreateViewModel(data));" + END;
                }
                else
                {
                    Result += TAB3 + "return View(\"Select" + _table.Name + "\", CreateViewModel(data));" + END;
                }
            }
            else
            {
                lst = Utils.PKHaveMoreThan1Attribute(_table);

                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + _table.Name.ToUpper() + "," + END;

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
                Result += TAB3 + "return View(\"DetailOf" + _table.Name + "\", CreateViewModel(data));" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
