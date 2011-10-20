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

    public class NguoiDungController : AbstractController
    {
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

        public string GenerateDetailActionResult()
        {
            string Result = "";
            var tblNguoiDung = Utils.GetTableByName(DB, GlobalVariables.g_sTableNguoiDung);
            if (tblNguoiDung == null)
            {
                return Result;
            }

            Result += TAB2 + "public ActionResult DetailOf" + GlobalVariables.g_sTableNguoiDung + "(" + Utils.GetDataType(Utils.GetPK(tblNguoiDung).Type) + " id)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_DETAILOF_" + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;

            if (Utils.GetPK(tblNguoiDung).Type == DataType.INT)
            {
                Result += TAB4 + "IntID = id" + END;
            }
            else if (Utils.GetPK(tblNguoiDung).Type == DataType.GUILD)
            {
                Result += TAB4 + "GuidID =id" + END;
            }

            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectActionResult()
        {
            string Result = "";

            Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_sTableNguoiDung + "(string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;
            Result += TAB4 + "Role = role" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return View(CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectActionResultPaging()
        {
            string Result = "";
            if (GlobalVariables.g_colPaging.Contains(GlobalVariables.g_sTableNguoiDung))
            {
                Result += TAB2 + "public ActionResult Select" + GlobalVariables.g_sTableNguoiDung + "Paging(string role, int page = 1)" + END;
                Result += TAB2 + "{" + END;
                Result += TAB3 + "var data = new DataTransferViewModel" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;
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
            if (Tbl != null)
            {
                Result += TAB2 + "public JsonResult Delete" + GlobalVariables.g_sTableNguoiDung + "(" + Utils.GetDataType(Utils.GetPK(Tbl).Type) + " id, string role)" + END;
            }
            else
            {
                Result += TAB2 + "public JsonResult Delete" + GlobalVariables.g_sTableNguoiDung + "(Guid id, string role)" + END;
            }
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return Json(new{" + END;
            Result += TAB4 + "Success = _rep" + GlobalVariables.g_sTableNguoiDung + ".Delete(id)," + END;
            Result += TAB4 + "RecordCount = _rep" + GlobalVariables.g_sTableNguoiDung + ".GetTotalPage(role)," + END;
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
            Result += TAB2 + "public ActionResult Update" + GlobalVariables.g_sTableNguoiDung + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;

            Result += TAB3 + "string username = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_UserName\")[0];" + END;
            Result += TAB3 + "string email = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_Email\")[0];" + END;

            if (Tbl != null)
            {
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    Result += TAB3 + "string " + Tbl.Attributes[i].Name.ToLower() + " = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\")[0];" + END;
                }
            }
            Result += TAB3 + "string currentpage = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_CurrentPage\")[0];" + END;

            Result += TAB3 + "var user = Membership.GetUser(username);" + END;
            Result += TAB3 + "if (email != user.Email)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "user.Email = email;" + END;
            Result += TAB4 + "Membership.UpdateUser(user);" + END;
            Result += TAB3 + "}" + END;

            if (Tbl != null)
            {
                Result += TAB3 + "var " + GlobalVariables.g_sTableNguoiDung.ToLower() + " = _rep" + GlobalVariables.g_sTableNguoiDung + ".SelectByID(" + Utils.GetDataType(Utils.GetPK(Tbl).Type) + ".Parse(" + Utils.GetPKWith1Attr(Tbl).ToLower() + "));" + END;
                Result += TAB3 + "if (" + GlobalVariables.g_sTableNguoiDung.ToLower() + " == null)" + END;
                Result += TAB3 + "{" + END;
                Result += TAB4 + GlobalVariables.g_sTableNguoiDung.ToLower() + " = new " + GlobalVariables.g_sTableNguoiDung + END;
                Result += TAB4 + "{" + END;
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    var attr = "";
                    if (Utils.GetDataType(Tbl.Attributes[i].Type) != "string")
                        attr = Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ")";
                    else
                        attr = Tbl.Attributes[i].Name.ToLower();

                    if (i < Tbl.Attributes.Count - 1)
                        Result += TAB5 + Tbl.Attributes[i].Name + " = " + attr + "," + END;
                    else
                        Result += TAB5 + Tbl.Attributes[i].Name + " = " + attr + END;
                }
                Result += TAB4 + "};" + END;
                Result += TAB4 + "DataContext.Instance." + GlobalVariables.g_sTableNguoiDung + ".AddObject(" + GlobalVariables.g_sTableNguoiDung.ToLower() + ");" + END;
                Result += TAB4 + "DataContext.Instance.SaveChanges();" + END;
                Result += TAB3 + "}" + END;
                Result += TAB3 + "else" + END;
                Result += TAB3 + "{" + END;

                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (Tbl.Attributes[i].Name.ToLower() != "id")
                    {
                        var attr = "";
                        if (Utils.GetDataType(Tbl.Attributes[i].Type) != "string")
                            attr = Utils.GetDataType(Tbl.Attributes[i].Type) + ".Parse(" + Tbl.Attributes[i].Name.ToLower() + ")";
                        else
                            attr = Tbl.Attributes[i].Name.ToLower();

                        if (i < Tbl.Attributes.Count - 1)
                            Result += TAB4 + GlobalVariables.g_sTableNguoiDung.ToLower() + "." + Tbl.Attributes[i].Name + " = " + attr + ";" + END;
                        else
                            Result += TAB4 + GlobalVariables.g_sTableNguoiDung.ToLower() + "." + Tbl.Attributes[i].Name + " = " + attr + ";" + END;
                    }
                }

                Result += TAB3 + " _rep" + GlobalVariables.g_sTableNguoiDung + ".Save();" + END;
                Result += TAB3 + "}" + END;
            }

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;
            Result += TAB4 + "CurrentPage = int.Parse(currentpage)" + END;
            Result += TAB3 + "};" + END;
            Result += TAB3 + "return PartialView(\"Templates/TH_List" + GlobalVariables.g_sTableNguoiDung + "\", CreateViewModel(data));" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateInsertActionResult()
        {
            string Result = "";

            Result += TAB2 + "[ValidateInput(false)]" + END;
            Result += TAB2 + "[AcceptVerbs(HttpVerbs.Post)]" + END;
            Result += TAB2 + "public ActionResult Insert" + GlobalVariables.g_sTableNguoiDung + "(FormCollection forms)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "string errorText = \"\";" + END;
            Result += TAB3 + "var nguoiDungInfo = new Add" + GlobalVariables.g_sTableNguoiDung + "ViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "Info = new " + GlobalVariables.g_sTableNguoiDung + "Info" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "BaseInfo = new " + GlobalVariables.g_sTableNguoiDung + "BaseInfo" + END;
            Result += TAB5 + "{" + END;
            Result += TAB6 + "UserName = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_UserName\")[0]," + END;
            Result += TAB6 + "Password = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_Password\")[0]," + END;
            Result += TAB6 + "Email = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_Email\")[0]," + END;
            Result += TAB6 + "Role = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_Role\")[0]" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "};" + END;

            Result += TAB3 + "var result = _rep" + GlobalVariables.g_sTableNguoiDung + ".Insert(nguoiDungInfo, ref errorText);" + END;

            Result += TAB3 + "var data = new DataTransferViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "EnumViewModelType = EnumViewModel.ADMIN_" + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;
            Result += TAB4 + "Role = forms.GetValues(\"" + GlobalVariables.g_sTableNguoiDung + "_OwnerRole\")[0]" + END;
            Result += TAB3 + "};" + END;
            if (TblHaveImgAttr || TblUsingFck)
                Result += TAB3 + "return View(\"Select" + GlobalVariables.g_sTableNguoiDung + "\", CreateViewModel(data));" + END;
            else
            {
                Result += TAB3 + "return Json(new{" + END;
                Result += TAB4 + "Success = result," + END;
                Result += TAB4 + "Message = \"A new item has been added!\"," + END;
                Result += TAB4 + "PartialViewHtml = RenderPartialViewToString(\"Templates/TH_List" + GlobalVariables.g_sTableNguoiDung + "\", CreateViewModel(data))" + END;
                Result += TAB3 + "});" + END;
            }
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
