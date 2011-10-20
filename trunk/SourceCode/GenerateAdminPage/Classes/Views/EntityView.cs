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

    public class EntityView : AbstractView
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
                    if (!TblHaveImgAttr && !TblUsingFck)
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
            Result += TAB + "Select" + Tbl.Name + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + Tbl.Name + " Management</span>" + END;
            Result += TAB + "(<a href=\"<%= Url.Action(\"Index\", \"Home\") %>\" class=\"AdminPageText\">" + END;
            Result += TAB + "<%= WebConfiguration.SiteName  %>" + END;
            Result += TAB + "</a>)" + END;
            Result += TAB + "<br />" + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"MainContent2\" runat=\"server\">" + END;
            if (haveUsingFCK)
            {
                Result += TAB + "<script src=\"../../Scripts/fckeditor/fckeditor.js\" type=\"text/javascript\"></script>" + END;
                Result += TAB + "<script type=\"text/javascript\">" + END;
                Result += TAB2 + "window.onload = function () {" + END;
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                    {
                        Result += TAB3 + "var objFckEditor = new FCKeditor('" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "');" + END;
                    }
                }
                Result += TAB3 + "objFckEditor.BasePath = '<%= ResolveUrl(\"~/Scripts/fckeditor/\")%>';" + END;
                Result += TAB3 + "objFckEditor.Height = 480;" + END;
                Result += TAB3 + "objFckEditor.Width = 600;" + END;
                Result += TAB3 + "objFckEditor.ToolbarSet = '" + GlobalVariables.g_sNameSpace + ".MyToolbar';" + END;
                Result += TAB3 + "objFckEditor.ReplaceTextarea();" + END;
                Result += TAB2 + "}" + END;
                Result += TAB + "</script>" + END;
            }

            Result += TAB + "<script type=\"text/javascript\">" + END;
            Result += GenerateDatePickerJS();
            Result += TAB2 + "<% if (Model." + Tbl.Name + "Model.InfoText != null && Model." + Tbl.Name + "Model.InfoText != \"\"){ %>" + END;
            Result += TAB3 + "alert('<%= Model." + Tbl.Name + "Model.InfoText %>');" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB + "</script>" + END;

            if (TblHaveImgAttr || TblUsingFck)
            {
                Result += TAB + "<% using (Html.BeginForm(\"Update" + Tbl.Name + "\", \"Admin\", FormMethod.Post, new { enctype = \"multipart/form-data\", onsubmit = \"return Validate" + Tbl.Name + "();\" }))" + END;
            }
            else
            {
                Result += TAB + "<% using (Ajax.BeginForm(\"Update" + Tbl.Name + "\", \"Admin\", new AjaxOptions { OnComplete = \"jsonAdd_OnComplete\" }))" + END;
            }
            Result += TAB + "{ %>" + END;

            Result += TAB2 + "<input type=\"hidden\" name=\"" + Tbl.Name + "_" + Utils.GetPKWith1Attr(Tbl) + "\" value=\"<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Utils.GetPKWith1Attr(Tbl) + " %>\" />" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "<input type=\"hidden\" name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" value=\"<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + " %>\" />" + END;
                }
            }
            Result += TAB2 + "<div style=\"padding-top: 40px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">DetailOf " + Tbl.Name + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                {
                    if (Tbl.Attributes[i].Name == Utils.GetImageAttrName(Tbl))
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (Tbl.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : Tbl.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<input type=\"file\" name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" />" + END;
                        Result += TAB8 + "<br />" + END;
                        Result += TAB8 + "<br />" + END;
                        Result += TAB8 + "<img src=\"../../Content/Images/Items/<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Utils.GetImageAttrName(Tbl) + " %>\" width=\"120px\" height=\"120px\" />" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
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
                        if (haveUsingFCK)
                        {
                            if (GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                            {
                                Result += TAB8 + "<%= Html.TextArea(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + ")%>" + END;
                            }
                            else if (Tbl.Attributes[i].Name.ToUpper().Contains("RUTGON"))
                            {
                                Result += TAB8 + "<textarea name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" style=\"width: 590px; height: 80px;\"><%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + "%></textarea>" + END;
                            }
                            else
                            {
                                if (Tbl.Attributes[i].Type == DataType.DATETIME)
                                {
                                    Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", String.Format(\"{0:dd/MM/yyyy}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + "), new { @style = \"width: 290px;\" })%>" + END;
                                }
                                else if (Tbl.Attributes[i].Type == DataType.LONG)
                                {
                                    Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", String.Format(\"{0:#,##0;Nothing}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + "), new { @style = \"width: 290px;\" })%>" + END;
                                }
                                else
                                {
                                    Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + ", new { @style = \"width: 590px;\" })%>" + END;
                                }
                            }
                        }
                        else
                        {
                            if (Tbl.Attributes[i].Type == DataType.DATETIME)
                            {
                                Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", String.Format(\"{0:dd/MM/yyyy}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + "), new { @style = \"width: 290px;\" })%>" + END;
                            }
                            else if (Tbl.Attributes[i].Type == DataType.LONG)
                            {
                                Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", String.Format(\"{0:#,##0;Nothing}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + "), new { @style = \"width: 290px;\" })%>" + END;
                            }
                            else
                            {
                                Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[0]." + Tbl.Attributes[i].Name + ", new { @style = \"width: 290px;\" })%>" + END;
                            }
                        }
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
                    }
                }
            }

            Result += TAB5 + "<tr>" + END;
            Result += TAB6 + "<td height=\"20\" style=\"width: 130px\">" + END;
            Result += TAB6 + "</td>" + END;
            Result += TAB6 + "<td height=\"20\">" + END;
            Result += TAB7 + "<div align=\"left\">" + END;
            Result += TAB8 + "<br/>" + END;
            Result += TAB8 + "<input type=\"submit\" value=\"Update\" />" + END;
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

        public string GenerateTemplateEdit()
        {
            string Result = "";

            Result += "<%@ Control Language=\"C#\" Inherits=\"System.Web.Mvc.ViewUserControl<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<%@ Import Namespace=\"" + GlobalVariables.g_sNameSpace + ".Helpers\" %>" + END;

            Result += "<div class=\"AdminPageText\">" + END;
            Result += "List Of " + Tbl.Name + END;
            Result += "</div>" + END;

            if (!GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
                Result += "<% using (Html.BeginForm(\"Update" + Tbl.Name + "\", \"Admin\", FormMethod.Post, new { id = \"Update\" })){ %>" + END;
            else
                Result += "<% using (Ajax.BeginForm(\"Update" + Tbl.Name + "\", \"Admin\", new AjaxOptions { HttpMethod = \"Post\", UpdateTargetId = \"PartialDiv\" })){ %>" + END;

            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                    Result += TAB3 + "<th>" + Tbl.Attributes[i].Name + "</th>" + END;
            }
            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "</tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + Tbl.Name + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>\">" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB2 + "<% if (Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " == Model." + Tbl.Name + "Model.EditModel.ID){ %>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB3 + "<td>" + END;
                    Result += TAB4 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + ", new { @Style = \"\"})%>" + END;
                    Result += TAB3 + "</td>" + END;
                }
                //else if (!Tbl.Attributes[i].IsForeignKey)
                //{
                //    Result += TAB3 + "<td>" + END;
                //    Result += TAB4 + "<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + "%>" + END;
                //    Result += TAB3 + "</td>" + END;
                //}
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<a href=\"#\" onclick=\"submitForm(event);\">" + END;
            Result += TAB5 + "<span style=\"color: Blue; text-decoration: underline\">Update</span>" + END;
            Result += TAB4 + "</a>" + END;
            Result += TAB4 + "&nbsp;" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + Tbl.Name + "_CurrentPage\", Model." + Tbl.Name + "Model.GetModel.CurrentPage) %>" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + Tbl.Name + "_" + Utils.GetPKWith1Attr(Tbl) + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + ")%>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB4 + "<%= Html.Hidden(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + ") %>" + END;
                }
            }

            Result += TAB4 + "<%= Ajax.ActionLink(\"Cancel\", \"CancelEditing" + Tbl.Name + "\", \"Admin\", new { page = Model." + Tbl.Name + "Model.GetModel.CurrentPage }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + Tbl.Name + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(Tbl).ToLower() + " = Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% }else{ %>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                    Result += TAB3 + "<td><%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + " %></td>" + END;
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + Tbl.Name + "\", \"Admin\", new { page = Model." + Tbl.Name + "Model.GetModel.CurrentPage, " + Utils.GetPKWith1Attr(Tbl).ToLower() + " = Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + Tbl.Name + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(Tbl).ToLower() + " = Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% } %>" + END;

            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            Result += "<% } %>" + END;
            if (GlobalVariables.g_colPaging.Contains(Tbl.Name))
            {
                Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
                Result += TAB + "{" + END;
                Result += TAB2 + "PageSize = WebConfiguration.Num" + Tbl.Name + "PerPage," + END;
                Result += TAB2 + "RecordCount = Model." + Tbl.Name + "Model.GetModel.TotalItem," + END;
                Result += TAB2 + "CurrentPageIndex = Model." + Tbl.Name + "Model.GetModel.CurrentPage," + END;

                if (Utils.GetForeighKeyList(Tbl).Count > 0)
                {
                    Result += TAB2 + "TypeOfParam = EnumTypeOfParam." + Tbl.Name.ToUpper() + "," + END;
                    Result += TAB2 + "TransferValue = Model." + Tbl.Name + "Model.ReferKeys." + Utils.GetForeighKeyList(Tbl)[0].Name + "," + END;
                    Result += TAB2 + "ActionName = \"Select" + Tbl.Name + "By" + Utils.GetForeighKeyList(Tbl)[0].Name + "Paging\"," + END;
                }
                else
                {
                    Result += TAB2 + "ActionName = \"Select" + Tbl.Name + "Paging\"," + END;
                }

                Result += TAB2 + "Controller = \"Admin\"," + END;
                Result += TAB2 + "UpdateTargetId = \"PartialDiv\"" + END;
                Result += TAB + "};" + END;
                Result += "%>" + END;
                Result += "<%= Ajax.GridPager(properties)%>" + END;
            }
            return Result;
        }

        public string GenerateTemplateList()
        {
            string Result = "";

            Result += "<%@ Control Language=\"C#\" Inherits=\"System.Web.Mvc.ViewUserControl<" + GlobalVariables.g_sNameSpace + ".Models.ViewModels.GroupViewModel>\" %>" + END;
            Result += "<div class=\"AdminPageText\">" + END;
            Result += "List Of " + Tbl.Name + END;
            Result += "</div>" + END;

            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                {
                    if (!GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name) || !GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                    {
                        Result += TAB3 + "<th>" + Tbl.Attributes[i].Name + "</th>" + END;
                    }
                }
            }
            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "<tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + Tbl.Name + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>\">" + END;
            Result += TAB2 + "<% } %>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                {
                    if (!GlobalVariables.g_colUsingFCK.Keys.Contains(Tbl.Name) || !GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                    {
                        if (Tbl.Attributes[i].Name != Utils.GetImageAttrName(Tbl))
                        {
                            if (Tbl.Attributes[i].Name.ToUpper().Contains("RUTGON"))
                            {
                                Result += TAB3 + "<td><span style=\"\"><%= Html.Truncate(Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + ", WebConfiguration." + Tbl.Name + "SortDescription) %></span></td>" + END;
                            }
                            else
                            {
                                if (Tbl.Attributes[i].Type == DataType.DATETIME)
                                {
                                    Result += TAB3 + "<td><span style=\"\"><%= String.Format(\"{0:dd/MM/yyyy}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + ") %></span></td>" + END;
                                }
                                else if (Tbl.Attributes[i].Type == DataType.LONG)
                                {
                                    Result += TAB3 + "<td><span style=\"\"><%= String.Format(\"{0:#,##0;Nothing}\", Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + ") %></span></td>" + END;
                                }
                                else
                                {
                                    Result += TAB3 + "<td><span style=\"\"><%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Tbl.Attributes[i].Name + " %></span></td>" + END;
                                }
                            }
                        }
                        else
                        {
                            Result += TAB3 + "<td><img src=\"../../../Content/Images/Items/<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetImageAttrName(Tbl) + " %>\" width=\"60px\" height=\"60px\" /></td>" + END;
                        }
                    }
                }
            }

            Result += TAB3 + "<td>" + END;
            if (!TblHaveImgAttr && !TblUsingFck)
                Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + Tbl.Name + "\", \"Admin\", new { page = Model." + Tbl.Name + "Model.GetModel.CurrentPage, " + Utils.GetPKWith1Attr(Tbl).ToLower() + " = Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            else
            {
                Result += TAB4 + "<a href=\"../../Admin/DetailOf" + Tbl.Name + "?" + Utils.GetPKWith1Attr(Tbl).ToLower() + "=<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>\">" + END;
                Result += TAB5 + "<span style=\"color: Blue; text-decoration: underline\">Select</span>" + END;
                Result += TAB4 + "</a>" + END;
            }
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;

            if (GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
            {
                Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + Tbl.Name + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(Tbl).ToLower() + " = Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            }
            else
            {
                Result += TAB4 + "<a href=\"../../Admin/Delete" + Tbl.Name + "?id=<%= Model." + Tbl.Name + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(Tbl) + " %>&page=<%= Model." + Tbl.Name + "Model.GetModel.CurrentPage %>\" onclick=\"return confirm('Are you sure you want to Delete it? This action cannot be undone.');\">Delete</a>" + END;
            }
            Result += TAB3 + "</td>" + END;
            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            if (GlobalVariables.g_colPaging.Contains(Tbl.Name))
            {
                Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
                Result += TAB + "{" + END;
                Result += TAB2 + "PageSize = WebConfiguration.Num" + Tbl.Name + "PerPage," + END;
                Result += TAB2 + "RecordCount = Model." + Tbl.Name + "Model.GetModel.TotalItem," + END;
                Result += TAB2 + "CurrentPageIndex = Model." + Tbl.Name + "Model.GetModel.CurrentPage," + END;

                if (Utils.GetForeighKeyList(Tbl).Count > 0)
                {
                    Result += TAB2 + "TypeOfParam = EnumTypeOfParam." + Tbl.Name.ToUpper() + "," + END;
                    Result += TAB2 + "TransferValue = Model." + Tbl.Name + "Model.ReferKeys." + Utils.GetForeighKeyList(Tbl)[0].Name + "," + END;
                    Result += TAB2 + "ActionName = \"Select" + Tbl.Name + "By" + Utils.GetForeighKeyList(Tbl)[0].Name + "Paging\"," + END;
                }
                else
                {
                    Result += TAB2 + "ActionName = \"Select" + Tbl.Name + "Paging\"," + END;
                }

                Result += TAB2 + "Controller = \"Admin\"," + END;
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
            Result += TAB + "Select" + Tbl.Name + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + Tbl.Name + " Management</span>" + END;
            Result += TAB + "(<a href=\"<%= Url.Action(\"Index\", \"Home\") %>\" class=\"AdminPageText\">" + END;
            Result += TAB + "<%= WebConfiguration.SiteName  %>" + END;
            Result += TAB + "</a>)" + END;
            Result += TAB + "<br />" + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content3\" ContentPlaceHolderID=\"MainContent2\" runat=\"server\">" + END;

            if (haveUsingFCK)
            {
                Result += TAB + "<script src=\"../../Scripts/fckeditor/fckeditor.js\" type=\"text/javascript\"></script>" + END;
                Result += TAB + "<script type=\"text/javascript\">" + END;
                Result += TAB2 + "window.onload = function () {" + END;

                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                    {
                        Result += TAB3 + "var objFckEditor = new FCKeditor('" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "');" + END;
                    }
                }

                Result += TAB3 + "objFckEditor.BasePath = '<%= ResolveUrl(\"~/Scripts/fckeditor/\")%>';" + END;
                Result += TAB3 + "objFckEditor.Height = 480;" + END;
                Result += TAB3 + "objFckEditor.Width = 600;" + END;
                Result += TAB3 + "objFckEditor.ToolbarSet = '" + GlobalVariables.g_sNameSpace + ".MyToolbar';" + END;
                Result += TAB3 + "objFckEditor.ReplaceTextarea();" + END;
                Result += TAB2 + "}" + END;
                Result += TAB + "</script>" + END;
            }

            Result += TAB + "<script type=\"text/javascript\">" + END;
            Result += GenerateDatePickerJS();
            Result += TAB2 + "<% if (Model." + Tbl.Name + "Model.InfoText != null && Model." + Tbl.Name + "Model.InfoText != \"\"){ %>" + END;
            Result += TAB3 + "alert('<%= Model." + Tbl.Name + "Model.InfoText %>');" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB + "</script>" + END;

            Result += TAB + "<div id=\"PartialDiv\">" + END;
            Result += TAB2 + "<% Html.RenderPartial(\"Templates/TH_List" + Tbl.Name + "\", Model); %>" + END;
            Result += TAB + "</div>" + END;

            if (TblHaveImgAttr || TblUsingFck)
            {
                Result += TAB + "<% using (Html.BeginForm(\"Insert" + Tbl.Name + "\", \"Admin\", FormMethod.Post, new { enctype = \"multipart/form-data\", onsubmit = \"return Validate" + Tbl.Name + "();\" }))" + END;
            }
            else
            {
                if (!GlobalVariables.g_colUsingAjax.Contains(Tbl.Name))
                    Result += TAB + "<% using (Html.BeginForm(\"Insert" + Tbl.Name + "\", \"Admin\", FormMethod.Post, new { onsubmit = \"return Validate" + Tbl.Name + "();\" }))" + END;
                else
                    Result += TAB + "<% using (Ajax.BeginForm(\"Insert" + Tbl.Name + "\", \"Admin\", new AjaxOptions { OnBegin = \"Validate" + Tbl.Name + "\", OnComplete = \"jsonAdd_OnComplete\" }))" + END;
            }
            Result += TAB + "{ %>" + END;
            
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "<input type=\"hidden\" name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" value=\"<%= Model." + Tbl.Name + "Model.ReferKeys." + Tbl.Attributes[i].Name + " %>\" />" + END;
                }
            }

            Result += TAB2 + "<div style=\"padding-top: 40px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">Add New " + Tbl.Name + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                {
                    if (Tbl.Attributes[i].Name == Utils.GetImageAttrName(Tbl))
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (Tbl.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : Tbl.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<input type=\"file\" name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" />" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
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

                        if (haveUsingFCK)
                        {
                            if (GlobalVariables.g_colUsingFCK.Values.Contains(Tbl.Attributes[i].Name))
                                Result += TAB8 + "<%= Html.TextArea(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\")%>" + END;
                            else if (Tbl.Attributes[i].Name.ToUpper().Contains("RUTGON"))
                                Result += TAB8 + "<textarea name=\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\" style=\"width: 590px; height: 80px;\"></textarea>" + END;
                            else
                            {
                                Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", \"\", new { @style = \"width: 590px;\" })%>" + END;
                            }
                        }
                        else
                            Result += TAB8 + "<%= Html.TextBox(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\", \"\", new { @style = \"width: 290px;\" })%>" + END;

                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
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
            Result += TAB9 + "<input type=\"submit\" value=\"Add New\" />" + END;
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

        public string GenerateDatePickerJS()
        {
            string Result = "";
            var exist = false;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].Type == DataType.DATETIME)
                {
                    exist = true;
                    break;
                }
            }

            if (exist)
            {
                Result += TAB2 + "$(document).ready(function () {" + END;
                for (int i = 0; i < Tbl.Attributes.Count; i++)
                {
                    if (Tbl.Attributes[i].Type == DataType.DATETIME)
                    {
                        Result += TAB3 + "$(\"#" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\").datepicker();" + END;
                    }
                }
                Result += TAB2 + "});" + END;
            }
            return Result;
        }
    }
}
