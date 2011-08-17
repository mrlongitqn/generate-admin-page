using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class AdminNguoiDungController : AdminModelsController
    {
        public Table _BaseInfo { get; set; }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public partial class AdminController: BaseController" + END;
            Result += TAB + "{" + END;

            Result += GenerateSelectActionResult() + END;
            Result += GenerateSelectActionResultPaging() + END;
            Result += GenerateDeleteActionResult() + END;
            Result += GenerateUpdateActionResult() + END;
            Result += GenerateInsertActionResult() + END;
            Result += GenerateDetailActionResult() + END;

            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateDetailActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult DetailOf" + GlobalVariables.g_ModelName + "(Guid id)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
            Result += TAB4 + "GuidID = id" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "(string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
            Result += TAB4 + "Role = role" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectActionResultPaging()
        {
            string Result = "";
            if (GlobalVariables.g_colPaging.Contains(GlobalVariables.g_ModelName))
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_ModelName + "Paging(string role, int page = 1)" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
                Result += TAB4 + "CurrentPage = page," + END;
                Result += TAB4 + "Role = role" + END;
                Result += TAB3 + "};" + END;
                Result += TAB3 + "return View(CreateViewModel(data));" + END;
                Result += TAB2 + "}" + END;
            }
            return Result;
        }

        public override string GenerateDeleteActionResult()
        {
            string Result = "";

            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Delete)]" + END;
            Result += TAB2 + "public JsonResult Delete" + GlobalVariables.g_ModelName + "(Guid id, string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return Json(new{" + END;
            Result += TAB4 + "Success = _rep" + GlobalVariables.g_ModelName + ".Delete(id)," + END;
            Result += TAB4 + "RecordCount = _rep" + GlobalVariables.g_ModelName + ".GetTotalPage(role)," + END;
            Result += TAB4 + "DeleteId = id" + END;
            Result += TAB3 + "});" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateUpdateActionResult()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Update" + GlobalVariables.g_ModelName + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            Result += TAB3 + "string username = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_UserName\")[0];" + END;
            Result += TAB3 + "string email = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_Email\")[0];" + END;

            if (_table != null)
            {
                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    Result += TAB3 + "string " + _table.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")[0];" + END;
                }
            }
            Result += TAB3 + "string currentpage = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_CurrentPage\")[0];" + END;

            Result += TAB3 + "var user = Membership.GetUser(username);" + END;
            Result += TAB3 + "if (email != user.Email)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "user.Email = email;" + END;
            Result += TAB4 + "Membership.UpdateUser(user);" + END;
            Result += TAB3 + "}" + END;

            if (_table != null)
            {
                Result += TAB3 + "var " + GlobalVariables.g_ModelName.ToLower() + " = _rep" + GlobalVariables.g_ModelName + ".SelectByID(Guid.Parse(" + Utils.GetPKWith1Attr(_table).ToLower() + "));" + END;
                Result += TAB3 + "if (" + GlobalVariables.g_ModelName.ToLower() + " == null)" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + GlobalVariables.g_ModelName.ToLower() + " = new " + GlobalVariables.g_ModelName + END;
                Result += TAB4 + "{" + END;
                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    var attr = "";
                    if (Utils.GetDataType(_table.Attributes[i].Type) != "string")
                        attr = Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ")";
                    else
                        attr = _table.Attributes[i].Name.ToLower();

                    if (i < _table.Attributes.Count - 1)
                        Result += TAB5 + _table.Attributes[i].Name + " = " + attr + "," + END;
                    else
                        Result += TAB5 + _table.Attributes[i].Name + " = " + attr + END;
                }
                Result += TAB4 + "};" + END;
                Result += TAB4 + "DataContext.Instance." + GlobalVariables.g_ModelName + "s.AddObject(" + GlobalVariables.g_ModelName.ToLower() + ");" + END;
                Result += TAB4 + "DataContext.Instance.SaveChanges();" + END;
                Result += TAB3 + "}" + END;
                Result += TAB3 + "else" + END;
                Result += TAB3 + "{" + END;

                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    if (_table.Attributes[i].Name.ToLower() != "id")
                    {
                        var attr = "";
                        if (Utils.GetDataType(_table.Attributes[i].Type) != "string")
                            attr = Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(" + _table.Attributes[i].Name.ToLower() + ")";
                        else
                            attr = _table.Attributes[i].Name.ToLower();

                        if (i < _table.Attributes.Count - 1)
                            Result += TAB4 + GlobalVariables.g_ModelName.ToLower() + "." + _table.Attributes[i].Name + " = " + attr + ";" + END;
                        else
                            Result += TAB4 + GlobalVariables.g_ModelName.ToLower() + "." + _table.Attributes[i].Name + " = " + attr + ";" + END;
                    }
                }

                Result += TAB3 + " _rep" + GlobalVariables.g_ModelName + ".Save();" + END;
                Result += TAB3 + "}" + END;
            }

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = int.Parse(currentpage)" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateInsertActionResult()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + GlobalVariables.g_ModelName + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "string errorText = \"\";" + END;
            Result += TAB3 + "var nguoiDungInfo = new Add" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "Info = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
            Result += TAB5 + "{" + END;
            Result += TAB6 + "UserName = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_UserName\")[0]," + END;
            Result += TAB6 + "Password = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_Password\")[0]," + END;
            Result += TAB6 + "Email = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_Email\")[0]," + END;
            Result += TAB6 + "Role = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_Role\")[0]" + END;
            Result += TAB5 + "}" + END;
            //if (_table != null)
            //{
            //    Result += TAB5 + ",ExtraInfo = new " + GlobalVariables.g_ModelName + END;
            //    Result += TAB5 + "{" + END;
            //    for (int i = 0; i < _table.Attributes.Count; i++)
            //    {
            //        if (_table.Attributes[i].Name != "ID")
            //        {
            //            if (i < _table.Attributes.Count - 1)
            //            {
            //                if (_table.Attributes[i].Type != DataType.STRING)
            //                    Result += TAB6 + _table.Attributes[i].Name + " = " + Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(forms.GetValues(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")[0])," + END;
            //                else
            //                    Result += TAB6 + _table.Attributes[i].Name + " = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")[0]," + END;
            //            }
            //            else
            //            {
            //                if (_table.Attributes[i].Type != DataType.STRING)
            //                    Result += TAB6 + _table.Attributes[i].Name + " = " + Utils.GetDataType(_table.Attributes[i].Type) + ".Parse(forms.GetValues(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")[0])" + END;
            //                else
            //                    Result += TAB6 + _table.Attributes[i].Name + " = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")[0]" + END;
            //            }
            //        }
            //    }
            //    Result += TAB5 + "}" + END;
            //}
            Result += TAB4 + "}" + END;
            Result += TAB3 + "};" + END;

            Result += TAB3 + "var result = _rep" + GlobalVariables.g_ModelName + ".Insert(nguoiDungInfo, ref errorText);" + END;

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_ModelName.ToUpper() + "," + END;
            Result += TAB4 + "Added = result," + END;
            Result += TAB4 + "Role = forms.GetValues(\"" + GlobalVariables.g_ModelName + "_OwnerRole\")[0]" + END;
            Result += TAB3 + "};" + END;
            if (TblHaveImgAttr || TblUsingFck)
                Result += TAB3 + "return View(\"Select" + GlobalVariables.g_ModelName + "\", CreateViewModel(data));" + END;
            else
            {
                Result += TAB3 + "return Json(new{" + END;
                Result += TAB4 + "Success = result," + END;
                Result += TAB4 + "Message = \"A new item has been added!\"," + END;
                Result += TAB4 + "PartialViewHtml = RenderPartialViewToString(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", CreateViewModel(data))" + END;
                Result += TAB3 + "});" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
