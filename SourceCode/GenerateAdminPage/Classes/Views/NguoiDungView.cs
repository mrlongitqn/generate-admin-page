using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    #region USING
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    public class NguoiDungView : AbstractView
    {
        public override string GenerateView(EnumView _type)
        {
            string Result = "";

            switch (_type)
            {
                case EnumView.SELECT:
                    Result += GenerateSelectView(TblUsingFck, TblHaveImgAttr);
                    break;

                case EnumView.TEMPLATE_LIST:
                    Result += GenerateTemplateList();
                    break;

                case EnumView.TEMPLATE_EDIT_OR_DETAILOF:
                    if (Tbl == null)
                        Result += GenerateTemplateEdit();
                    else
                        Result += GenerateDetailOf(TblUsingFck, TblHaveImgAttr);
                    break;
            }

            return Result;
        }

        public string GenerateDetailOf(bool haveUsingFCK, bool haveUploadFiles)
        {
            string Result = "";

            Result += "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/Views/Shared/Admin.Master\"Inherits=\"System.Web.Mvc.ViewPage<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"TitleContent\" runat=\"server\">" + END;
            Result += TAB + "Select" + GlobalVariables.g_sTableNguoiDung + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + GlobalVariables.g_sTableNguoiDung + " Management</span>" + END;
            Result += TAB + "(<a href=\"<%= Url.Action(\"Index\", \"Home\") %>\" class=\"AdminPageText\">" + END;
            Result += TAB + "<%= WebConfiguration.SiteName  %>" + END;
            Result += TAB + "</a>)" + END;
            Result += TAB + "<br />" + END;
            Result += "</asp:Content>" + END;
            Result += "<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"MainContent2\" runat=\"server\">" + END;

            Result += TAB2 + "<div style=\"padding-top: 20px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">Detail Of " + GlobalVariables.g_sTableNguoiDung + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            // UserName
            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td>" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<font class=\"normal8\">UserName: </font>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[0].BaseInfo.UserName %>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB5 + "</tr>" + END;

            // Email
            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td>" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<font class=\"normal8\">Email: </font>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[0].BaseInfo.Email %>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB5 + "</tr>" + END;

            // Role
            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td>" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<font class=\"normal8\">Role: </font>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            Result += TAB8 + "<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[0].BaseInfo.Role %>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB5 + "</tr>" + END;

            if (Tbl != null)
            {
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
                    {
                        if (haveUploadFiles)
                        {
                            if (Tbl.Attributes[i].Name == Utils.GetImageAttrName(Tbl))
                            {
                                Result += TAB5 + "<tr>" + END;
                                Result += TAB6 + "<td>" + END;
                                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                                Result += TAB8 + "<font class=\"normal8\">" + Tbl.Attributes[i].Name + ": </font>" + END;
                                Result += TAB7 + "</div>" + END;
                                Result += TAB6 + "</td>" + END;
                                Result += TAB6 + "<td height=\"20\">" + END;
                                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                                Result += TAB8 + "<input type=\"file\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\" />" + END;
                                Result += TAB8 + "<br />" + END;
                                Result += TAB8 + "<br />" + END;
                                Result += TAB8 + "<img src=\"../../Content/Images/Items/<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[0].ObjModel." + Utils.GetImageAttrName(Tbl) + " %>\" width=\"120px\" height=\"120px\" />" + END;
                                Result += TAB7 + "</div>" + END;
                                Result += TAB6 + "</td>" + END;
                                Result += TAB5 + "</tr>" + END;
                            }
                        }
                        else
                        {
                            Result += TAB5 + "<tr>" + END;
                            Result += TAB6 + "<td>" + END;
                            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                            Result += TAB8 + "<font class=\"normal8\">" + Tbl.Attributes[i].Name + ": </font>" + END;
                            Result += TAB7 + "</div>" + END;
                            Result += TAB6 + "</td>" + END;
                            Result += TAB6 + "<td height=\"20\">" + END;
                            Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                            Result += TAB8 + "<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[0].ExtraInfo." + Tbl.Attributes[i].Name +  "%>" + END;
                            Result += TAB7 + "</div>" + END;
                            Result += TAB6 + "</td>" + END;
                            Result += TAB5 + "</tr>" + END;
                        }
                    }
                }
            }
            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td height=\"20\" style=\"width: 130px\">" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\">" + END;
            Result += TAB8 + "<br/>" + END;
            Result += TAB8 + "<p>" + END;
            Result += TAB8 + "</p>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB5 + "</tr>" + END;

            Result += TAB4 + "</tbody>" + END;
            Result += TAB3 + "</table>" + END;
            Result += TAB2 + "</div>" + END;

            Result += "</asp:Content>" + END;

            return Result;
        }

        public string GenerateTemplateEdit()
        {
            string Result = "";

            Result += "<%@ Control Language=\"C#\" Inherits=\"System.Web.Mvc.ViewUserControl<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<%@ Import Namespace=\"" + GlobalVariables.g_sNameSpace + ".Helpers\" %>" + END;

            Result += "<div class=\"AdminPageText\">" + END;
            Result += "List Of " + GlobalVariables.g_sTableNguoiDung + END;
            Result += "</div>" + END;

            Result += "<% using (Ajax.BeginForm(\"UpdateDiaDiem\", \"Admin\", new AjaxOptions { HttpMethod = \"Post\", UpdateTargetId = \"PartialDiv\" })){ %>" + END;
            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;

            if (Tbl != null)
            {
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
                        Result += TAB3 + "<th>" + Tbl.Attributes[i].Name + "</th>" + END;
                }
            }
            else
            {
                Result += TAB3 + "<th>UserName</th>" + END;
                Result += TAB3 + "<th>Role</th>" + END;
                Result += TAB3 + "<th>Email</th>" + END;
            }

            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "<tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID %>\">" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB2 + "<% if (Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID == Model." + GlobalVariables.g_sTableNguoiDung + "Model.EditModel.ID){ %>" + END;

            if (Tbl != null)
            {
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
                        Result += TAB3 + "<%= Html.TextBox(\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\", Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel." + Tbl.Attributes[i].Name + ")%>" + END;
                }
            }
            else
            {
                //Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.UserName %></td>" + END;
                //Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.Role %></td>" + END;
                //Result += TAB3 + "<%= Html.TextBox(\"" + GlobalVariables.g_sTableNguoiDung + "_Email" + "\", Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel." + Tbl.Attributes[i].Name + ")%>" + END;
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<a href=\"#\" onclick=\"submitForm(event)\">" + END;
            Result += TAB5 + "<span style=\"color: Blue; text-decoration: underline\">Update</span>" + END;
            Result += TAB4 + "</a>" + END;
            Result += TAB4 + "&nbsp;" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + GlobalVariables.g_sTableNguoiDung + "_CurrentPage\", Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage) %>" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + GlobalVariables.g_sTableNguoiDung + "_ID\", Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID)%>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Cancel\", \"CancelEditing" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { page = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { id = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% }else{ %>" + END;

            if (Tbl != null)
            {
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
                        Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel." + Tbl.Attributes[i].Name + " %></td>" + END;
                }
            }
            else
            {
                //Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.UserName %></td>" + END;
                //Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.Role %></td>" + END;
                //Result += TAB3 + "<td><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.Email %></td>" + END;
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { page = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage, id = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { id = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].ObjModel.ID }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% } %>" + END;

            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            Result += "<% } %>" + END;

            Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "PageSize = WebConfiguration.Num" + GlobalVariables.g_sTableNguoiDung + "PerPage," + END;
            Result += TAB2 + "RecordCount = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.TotalItem," + END;
            Result += TAB2 + "CurrentPageIndex = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage," + END;
            Result += TAB2 + "Controller = \"Admin\"," + END;
            Result += TAB2 + "ActionName = \"Select" + GlobalVariables.g_sTableNguoiDung + "\"," + END;
            Result += TAB2 + "UpdateTargetId = \"PartialDiv\"" + END;
            Result += TAB + "};" + END;
            Result += "%>" + END;
            Result += "<%= Ajax.GridPager(properties)%>" + END;

            return Result;
        }

        public string GenerateTemplateList()
        {
            string Result = "";

            Result += "<%@ Control Language=\"C#\" Inherits=\"System.Web.Mvc.ViewUserControl<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<div class=\"AdminPageText\">" + END;
            Result += "List Of " + GlobalVariables.g_sTableNguoiDung + END;
            Result += "</div>" + END;

            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;
            Result += TAB3 + "<th>UserName</th>" + END;
            Result += TAB3 + "<th>Email</th>" + END;
            Result += TAB3 + "<th>Role</th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "<tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.ID %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.ID %>\">" + END;
            Result += TAB2 + "<% } %>" + END;

            Result += TAB3 + "<td><span style=\"\"><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.UserName %></span></td>" + END;
            Result += TAB3 + "<td><span style=\"\"><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.Email %></span></td>" + END;
            Result += TAB3 + "<td><span style=\"\"><%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.Role %></span></td>" + END;
 
            Result += TAB3 + "<td>" + END;
            if (Tbl != null)
            {
                Result += TAB4 + "<a href=\"../../Admin/DetailOf" + GlobalVariables.g_sTableNguoiDung + "?id=<%= Model." +  GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.ID %>\">" + END;
                Result += TAB4 + "<span style=\"color: Blue; text-decoration: underline\">Select</span>" + END;
                Result += TAB4 + "</a>" + END;
            }
            else
                Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { page = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage, id = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.ID }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new { id = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.LstObjModel[i].BaseInfo.ID, role = Roles.GetRolesForUser(Page.User.Identity.Name)[0] }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            if (GlobalVariables.g_colPaging.Contains(GlobalVariables.g_sTableNguoiDung))
            {
                Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
                Result += TAB + "{" + END;
                Result += TAB2 + "PageSize = WebConfiguration.Num" + GlobalVariables.g_sTableNguoiDung + "PerPage," + END;
                Result += TAB2 + "RecordCount = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.TotalItem," + END;
                Result += TAB2 + "CurrentPageIndex = Model." + GlobalVariables.g_sTableNguoiDung + "Model.GetModel.CurrentPage," + END;
                Result += TAB2 + "TypeOfParam = EnumTypeOfParam." + GlobalVariables.g_sTableNguoiDung.ToUpper() + "," + END;
                Result += TAB2 + "StrTransferValue = Roles.GetRolesForUser(Page.User.Identity.Name)[0]," + END;
                Result += TAB2 + "Controller = \"Admin\"," + END;
                Result += TAB2 + "ActionName = \"Select" + GlobalVariables.g_sTableNguoiDung + "Paging\"," + END;
                Result += TAB2 + "UpdateTargetId = \"PartialDiv\"" + END;
                Result += TAB + "};" + END;
                Result += "%>" + END;
                Result += "<%= Ajax.GridPager(properties)%>" + END;
            }
            return Result;
        }

        public string GenerateSelectView(bool haveUsingFCK, bool haveUploadFiles)
        {
            string Result = "";

            Result += "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/Views/Shared/Admin.Master\"Inherits=\"System.Web.Mvc.ViewPage<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"TitleContent\" runat=\"server\">" + END;
            Result += TAB + "Select" + GlobalVariables.g_sTableNguoiDung + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + GlobalVariables.g_sTableNguoiDung + " Management</span>" + END;
            Result += TAB + "(<a href=\"<%= Url.Action(\"Index\", \"Home\") %>\" class=\"AdminPageText\">" + END;
            Result += TAB + "<%= WebConfiguration.SiteName  %>" + END;
            Result += TAB + "</a>)" + END;
            Result += TAB + "<br />" + END;
            Result += "</asp:Content>" + END;

            if (haveUsingFCK)
            {
                Result += TAB + "<script src=\"../../Scripts/fckeditor/fckeditor.js\" type=\"text/javascript\"></script>" + END;
                Result += TAB + "<script type=\"text/javascript\">" + END;
                Result += TAB2 + "window.onload = function () {" + END;
                Result += TAB3 + "var objFckEditor = new FCKeditor('" + GlobalVariables.g_sTableNguoiDung + "_NoiDung');" + END;
                Result += TAB3 + "objFckEditor.BasePath = '<%= ResolveUrl(\"~/Scripts/fckeditor/\")%>';" + END;
                Result += TAB3 + "objFckEditor.Height = 480;" + END;
                Result += TAB3 + "objFckEditor.Width = 600;" + END;
                Result += TAB3 + "objFckEditor.ToolbarSet = '" + GlobalVariables.g_sNameSpace + ".MyToolbar';" + END;
                Result += TAB3 + "objFckEditor.ReplaceTextarea();" + END;
                Result += TAB2 + "}" + END;
                Result += TAB + "</script>" + END;
            }

            Result += "<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"MainContent2\" runat=\"server\">" + END;

            Result += TAB + "<script type=\"text/javascript\">" + END;
            Result += TAB2 + "<% if (Model." + GlobalVariables.g_sTableNguoiDung + "Model.InfoText != null && Model." + GlobalVariables.g_sTableNguoiDung + "Model.InfoText != \"\"){ %>" + END;
            Result += TAB3 + "alert('<%= Model." + GlobalVariables.g_sTableNguoiDung + "Model.InfoText %>');" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB + "</script>" + END;

            Result += TAB + "<div id=\"PartialDiv\">" + END;
            Result += TAB2 + "<% Html.RenderPartial(\"Templates/TH_List" + GlobalVariables.g_sTableNguoiDung + "\", Model); %>" + END;
            Result += TAB + "</div>" + END;

            if (TblHaveImgAttr || TblUsingFck)
            {
                Result += TAB + "<% using (Html.BeginForm(\"Insert" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", FormMethod.Post, new { enctype = \"multipart/form-data\", onsubmit = \"return Validate" + GlobalVariables.g_sTableNguoiDung + "();\" }))" + END;
            }
            else
            {
                Result += TAB + "<% using (Ajax.BeginForm(\"Insert" + GlobalVariables.g_sTableNguoiDung + "\", \"Admin\", new AjaxOptions { OnBegin = \"Validate" + GlobalVariables.g_sTableNguoiDung + "\", OnComplete = \"jsonAdd_OnComplete\" }))" + END;
            }
            Result += TAB + "{ %>" + END;
            Result += TAB2 + "<input type=\"hidden\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_OwnerRole\" value=\"<%= Roles.GetRolesForUser(Page.User.Identity.Name)[0] %>\" />" + END;
            Result += TAB2 + "<div style=\"padding-top: 40px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">Add New " + GlobalVariables.g_sTableNguoiDung + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            //show base info
            for (int i = 0; i < 4; i++)
            {
                var name = "";
                switch (i)
                {
                    case 0:
                        name = "UserName";
                        break;
                    case 1:
                        name = "Password";
                        break;
                    case 2:
                        name = "Email";
                        break;
                    case 3:
                        name = "Role";
                        break;
                }
                Result += TAB5 + "<tr>" + END;
                Result += TAB6 + "<td>" + END;
                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                Result += TAB8 + "<font class=\"normal8\">" + name + ": </font>" + END;
                Result += TAB7 + "</div>" + END;
                Result += TAB6 + "</td>" + END;
                Result += TAB6 + "<td height=\"20\">" + END;
                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                if(i == 3)
                    Result += TAB8 + "<%= Html.DropDownList(\"" + GlobalVariables.g_sTableNguoiDung + "_Role\", new SelectList(Roles.GetAllRoles())) %>" + END;
                else if(i == 1)
                    Result += TAB8 + "<input type=\"password\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + name + "\" />" + END;
                else
                    Result += TAB8 + "<input type=\"text\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + name + "\" />" + END;
                Result += TAB7 + "</div>" + END;
                Result += TAB6 + "</td>" + END;
                Result += TAB5 + "</tr>" + END;
            }

            //if (Tbl != null)
            //{
            //    for (int i = 0; i < Tbl.Attributes.Count; i++)
            //    {
            //        if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
            //        {
            //            if (Tbl.Attributes[i].Name == Utils.GetImageAttrName(Tbl))
            //            {
            //                Result += TAB5 + "<tr>" + END;
            //                Result += TAB6 + "<td>" + END;
            //                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            //                Result += TAB8 + "<font class=\"normal8\">" + Tbl.Attributes[i].Name + ": </font>" + END;
            //                Result += TAB7 + "</div>" + END;
            //                Result += TAB6 + "</td>" + END;
            //                Result += TAB6 + "<td height=\"20\">" + END;
            //                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            //                Result += TAB8 + "<input type=\"file\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\" />" + END;
            //                Result += TAB7 + "</div>" + END;
            //                Result += TAB6 + "</td>" + END;
            //                Result += TAB5 + "</tr>" + END;
            //            }
            //            else
            //            {
            //                Result += TAB5 + "<tr>" + END;
            //                Result += TAB6 + "<td>" + END;
            //                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
            //                Result += TAB8 + "<font class=\"normal8\">" + Tbl.Attributes[i].Name + ": </font>" + END;
            //                Result += TAB7 + "</div>" + END;
            //                Result += TAB6 + "</td>" + END;
            //                Result += TAB6 + "<td height=\"20\">" + END;
            //                Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;

            //                if (haveUsingFCK)
            //                    Result += TAB8 + "<textarea name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\" />" + END;
            //                else
            //                    Result += TAB8 + "<input type=\"text\" name=\"" + GlobalVariables.g_sTableNguoiDung + "_" + Tbl.Attributes[i].Name + "\" />" + END;

            //                Result += TAB7 + "</div>" + END;
            //                Result += TAB6 + "</td>" + END;
            //                Result += TAB5 + "</tr>" + END;
            //            }
            //        }
            //    }
            //}
            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td height=\"20\" style=\"width: 130px\">" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\">" + END;
            Result += TAB8 + "<br/>" + END;
            Result += TAB9 + "<input type=\"submit\" value=\"Add New\" />" + END;
            Result += TAB8 + "<p>" + END;
            Result += TAB8 + "</p>" + END;
            Result += TAB7 + "</div>" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB5 + "</tr>" + END;

            Result += TAB4 + "</tbody>" + END;
            Result += TAB3 + "</table>" + END;
            Result += TAB2 + "</div>" + END;
            Result += TAB + "<% } %>" + END;

            Result += "</asp:Content>" + END;

            return Result;
        }
        
    }
}
