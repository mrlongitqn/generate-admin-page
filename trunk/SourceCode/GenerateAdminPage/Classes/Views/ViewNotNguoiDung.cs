using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class ViewNotNguoiDung: Views
    {
        public override void InitView(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;
            GlobalVariables.g_ModelName = _tbl.Name;
            TblUsingFck = Utils.TableUsingFCK(_table);
            TblHaveImgAttr = Utils.TableHaveImageAttribute(_table);
        }

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
            Result += TAB + "Select" + GlobalVariables.g_ModelName + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + GlobalVariables.g_ModelName + " Management</span>" + END;
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
                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    if (GlobalVariables.g_colUsingFCK.Values.Contains(_table.Attributes[i].Name))
                    {
                        Result += TAB3 + "var objFckEditor = new FCKeditor('" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "');" + END;
                    }
                }
                Result += TAB3 + "objFckEditor.BasePath = '<%= ResolveUrl(\"~/Scripts/fckeditor/\")%>';" + END;
                Result += TAB3 + "objFckEditor.Height = 480;" + END;
                Result += TAB3 + "objFckEditor.Width = 600;" + END;
                Result += TAB3 + "objFckEditor.ToolbarSet = 'My" + GlobalVariables.g_sNameSpace + "Toolbar';" + END;
                Result += TAB3 + "objFckEditor.ReplaceTextarea();" + END;
                Result += TAB3 + "<% if (Model." + GlobalVariables.g_ModelName + "Model.EditModel.Edited){ %>" + END;
                Result += TAB4 + "alert(\"An item has been updated!\");" + END;
                Result += TAB3 + "<% } %>" + END;
                Result += TAB2 + "}" + END;
                Result += TAB + "</script>" + END;
            }
            else
            {
                Result += TAB + "<script type=\"text/javascript\">" + END;
                Result += TAB2 + "<% if (Model." + GlobalVariables.g_ModelName + "Model.EditModel.Edited){ %>" + END;
                Result += TAB3 + "alert(\"An item has been updated!\");" + END;
                Result += TAB2 + "<% } %>" + END;
                Result += TAB + "</script>" + END;
            }

            if (TblHaveImgAttr || TblUsingFck)
            {
                Result += TAB + "<% using (Html.BeginForm(\"Update" + GlobalVariables.g_ModelName + "\", \"Admin\", FormMethod.Post, new { enctype = \"multipart/form-data\", onsubmit = \"return Validate" + GlobalVariables.g_ModelName + "();\" }))" + END;
            }
            else
            {
                Result += TAB + "<% using (Ajax.BeginForm(\"Update" + GlobalVariables.g_ModelName + "\", \"Admin\", new AjaxOptions { OnComplete = \"jsonAdd_OnComplete\" }))" + END;
            }
            Result += TAB + "{ %>" + END;

            Result += TAB2 + "<input type=\"hidden\" name=\"" + GlobalVariables.g_ModelName + "_" + Utils.GetPKWith1Attr(_table) + "\" value=\"<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + Utils.GetPKWith1Attr(_table) + " %>\" />" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "<input type=\"hidden\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" value=\"<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + _table.Attributes[i].Name + " %>\" />" + END;
                }
            }
            Result += TAB2 + "<div style=\"padding-top: 40px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">DetailOf " + GlobalVariables.g_ModelName + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                {
                    if (_table.Attributes[i].Name == Utils.GetImageAttrName(_table))
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (_table.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : _table.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<input type=\"file\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" />" + END;
                        Result += TAB8 + "<br />" + END;
                        Result += TAB8 + "<br />" + END;
                        Result += TAB8 + "<img src=\"../../Content/Images/Items/<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + Utils.GetImageAttrName(_table) + " %>\" width=\"120px\" height=\"120px\" />" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
                    }
                    else
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (_table.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : _table.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        if (haveUsingFCK)
                        {
                            if(GlobalVariables.g_colUsingFCK.Values.Contains(_table.Attributes[i].Name))
                                Result += TAB8 + "<%= Html.TextArea(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + _table.Attributes[i].Name + ")%>" + END;
                            else if (_table.Attributes[i].Name.ToUpper().Contains("RUTGON"))
                                Result += TAB8 + "<textarea name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" style=\"width: 590px; height: 80px;\"><%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + _table.Attributes[i].Name + "%></textarea>" + END;
                            else
                                Result += TAB8 + "<%= Html.TextBox(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + _table.Attributes[i].Name + ")%>" + END;
                        }
                        else
                            Result += TAB8 + "<input type=\"text\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" value=\"<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[0]." + _table.Attributes[i].Name + " %>\" />" + END;
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
            Result += "List Of " + GlobalVariables.g_ModelName + END;
            Result += "</div>" + END;

            if (!GlobalVariables.g_colUsingAjax.Contains(GlobalVariables.g_ModelName))
                Result += "<% using (Html.BeginForm(\"Update" + GlobalVariables.g_ModelName + "\", \"Admin\", FormMethod.Post, new { id = \"Update\" })){ %>" + END;
            else
                Result += "<% using (Ajax.BeginForm(\"Update" + GlobalVariables.g_ModelName + "\", \"Admin\", new AjaxOptions { HttpMethod = \"Post\", UpdateTargetId = \"PartialDiv\" })){ %>" + END;

            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                    Result += TAB3 + "<th>" + _table.Attributes[i].Name + "</th>" + END;
            }
            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "</tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " %>\">" + END;
            Result += TAB2 + "<% } %>" + END;
            Result += TAB2 + "<% if (Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " == Model." + GlobalVariables.g_ModelName + "Model.EditModel.ID){ %>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB3 + "<td>" + END;
                    Result += TAB4 + "<%= Html.TextBox(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + _table.Attributes[i].Name + ")%>" + END;
                    Result += TAB3 + "</td>" + END;
                }
                //else if (!_table.Attributes[i].IsForeignKey)
                //{
                //    Result += TAB3 + "<td>" + END;
                //    Result += TAB4 + "<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + _table.Attributes[i].Name + "%>" + END;
                //    Result += TAB3 + "</td>" + END;
                //}
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<a href=\"#\" onclick=\"submitForm(event);\">" + END;
            Result += TAB5 + "<span style=\"color: Blue; text-decoration: underline\">Update</span>" + END;
            Result += TAB4 + "</a>" + END;
            Result += TAB4 + "&nbsp;" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + GlobalVariables.g_ModelName + "_CurrentPage\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage) %>" + END;
            Result += TAB4 + "<%= Html.Hidden(\"" + GlobalVariables.g_ModelName + "_" + Utils.GetPKWith1Attr(_table) + "\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + ")%>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB4 + "<%= Html.Hidden(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\", Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + _table.Attributes[i].Name + ") %>" + END;
                }
            }

            Result += TAB4 + "<%= Ajax.ActionLink(\"Cancel\", \"CancelEditing" + GlobalVariables.g_ModelName + "\", \"Admin\", new { page = Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_ModelName + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(_table).ToLower() + " = Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% }else{ %>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                    Result += TAB3 + "<td><%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + _table.Attributes[i].Name + " %></td>" + END;
            }

            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + GlobalVariables.g_ModelName + "\", \"Admin\", new { page = Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage, " + Utils.GetPKWith1Attr(_table).ToLower() + " = Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_ModelName + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(_table).ToLower() + " = Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;

            Result += TAB2 + "<% } %>" + END;

            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            Result += "<% } %>" + END;

            Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "PageSize = WebConfiguration.ProductsPerPage," + END;
            Result += TAB2 + "RecordCount = Model." + GlobalVariables.g_ModelName + "Model.GetModel.TotalItem," + END;
            Result += TAB2 + "CurrentPageIndex = Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage," + END;
            Result += TAB2 + "Controller = \"Admin\"," + END;
            Result += TAB2 + "ActionName = \"Select" + GlobalVariables.g_ModelName + "\"," + END;
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
            Result += "List Of " + GlobalVariables.g_ModelName + END;
            Result += "</div>" + END;

            Result += "<table class=\"Grid\" cellpadding=\"0\" cellspacing=\"0\">" + END;
            Result += TAB + "<thead>" + END;
            Result += TAB2 + "<tr>" + END;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                    Result += TAB3 + "<th>" + _table.Attributes[i].Name + "</th>" + END;
            }
            Result += TAB3 + "<th></th>" + END;
            Result += TAB3 + "<th></th>" + END;
            Result += TAB2 + "<tr>" + END;
            Result += TAB + "</thead>" + END;

            Result += TAB + "<% for (int i=0; i<Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel.Count; i++){" + END;
            Result += TAB2 + "if (i % 2 != 0) {%>" + END;
            Result += TAB2 + "<tr class=\"Row\" id=\"row-<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " %>\">" + END;
            Result += TAB2 + "<% }" + END;
            Result += TAB2 + "else { %>" + END;
            Result += TAB2 + "<tr class=\"AlternatingRow\" id=\"row-<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " %>\">" + END;
            Result += TAB2 + "<% } %>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                {
                    if (_table.Attributes[i].Name != Utils.GetImageAttrName(_table))
                        Result += TAB3 + "<td><%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + _table.Attributes[i].Name + " %></td>" + END;
                    else
                    {
                        Result += TAB3 + "<td><img src=\"../../../Content/Images/Items/<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetImageAttrName(_table) + " %>\" width=\"90px\" height=\"90px\" /></td>" + END;
                    }
                }

            }

            Result += TAB3 + "<td>" + END;
            if (!TblHaveImgAttr && !TblUsingFck)
                Result += TAB4 + "<%= Ajax.ActionLink(\"Edit\", \"Edit" + GlobalVariables.g_ModelName + "\", \"Admin\", new { page = Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage, " + Utils.GetPKWith1Attr(_table).ToLower() + " = Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " }, new AjaxOptions { UpdateTargetId = \"PartialDiv\" })%>" + END;
            else
            {
                Result += TAB4 + "<a href=\"../../Admin/DetailOf" + GlobalVariables.g_ModelName + "?" + Utils.GetPKWith1Attr(_table).ToLower() + "=<%= Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " %>\">" + END;
                Result += TAB5 + "<span style=\"color: Blue; text-decoration: underline\">Select</span>" + END;
                Result += TAB4 + "</a>" + END;
            }
            Result += TAB3 + "</td>" + END;
            Result += TAB3 + "<td>" + END;
            Result += TAB4 + "<%= Ajax.ActionLink(\"Delete\", \"Delete" + GlobalVariables.g_ModelName + "\", \"Admin\", new { " + Utils.GetPKWith1Attr(_table).ToLower() + " = Model." + GlobalVariables.g_ModelName + "Model.GetModel.LstObjModel[i]." + Utils.GetPKWith1Attr(_table) + " }, new AjaxOptions{Confirm = \"Are you sure you want to Delete it? This action cannot be undone.\", HttpMethod = \"Delete\", OnComplete = \"jsonDelete_OnComplete\"})%>" + END;
            Result += TAB3 + "</td>" + END;
            Result += TAB2 + "</tr>" + END;
            Result += TAB + "<% } %>  " + END;
            Result += "</table>" + END;
            if (GlobalVariables.g_colPaging.Contains(GlobalVariables.g_ModelName))
            {
                Result += "<% GridPagerProperties properties = new GridPagerProperties()" + END;
                Result += TAB + "{" + END;
                Result += TAB2 + "PageSize = WebConfiguration.ProductsPerPage," + END;
                Result += TAB2 + "RecordCount = Model." + GlobalVariables.g_ModelName + "Model.GetModel.TotalItem," + END;
                Result += TAB2 + "CurrentPageIndex = Model." + GlobalVariables.g_ModelName + "Model.GetModel.CurrentPage," + END;
                Result += TAB2 + "Controller = \"Admin\"," + END;
                Result += TAB2 + "ActionName = \"Select" + GlobalVariables.g_ModelName + "Paging\"," + END;
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
            Result += TAB + "Select" + GlobalVariables.g_ModelName + END;
            Result += "</asp:Content>" + END;

            Result += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"MainContent1\" runat=\"server\">" + END;
            Result += TAB + "<span class=\"AdminTitle\" style=\"color:#cc0000;\">" + GlobalVariables.g_ModelName + " Management</span>" + END;
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

                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    if (GlobalVariables.g_colUsingFCK.Values.Contains(_table.Attributes[i].Name))
                    {
                        Result += TAB3 + "var objFckEditor = new FCKeditor('" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "');" + END;
                    }
                }

                Result += TAB3 + "objFckEditor.BasePath = '<%= ResolveUrl(\"~/Scripts/fckeditor/\")%>';" + END;
                Result += TAB3 + "objFckEditor.Height = 480;" + END;
                Result += TAB3 + "objFckEditor.Width = 600;" + END;
                Result += TAB3 + "objFckEditor.ToolbarSet = 'My" + GlobalVariables.g_sNameSpace + "Toolbar';" + END;
                Result += TAB3 + "objFckEditor.ReplaceTextarea();" + END;
                Result += TAB3 + "<% if (Model." + GlobalVariables.g_ModelName + "Model.AddModel.Added){ %>" + END;
                Result += TAB4 + "alert(\"A new item has been added!\");" + END;
                Result += TAB3 + "<% } %>" + END;
                Result += TAB2 + "}" + END;
                Result += TAB + "</script>" + END;
            }
            else
            {
                if (!GlobalVariables.g_colUsingAjax.Contains(GlobalVariables.g_ModelName))
                {
                    Result += TAB + "<script type=\"text/javascript\">" + END;
                    Result += TAB2 + "<% if (Model." + GlobalVariables.g_ModelName + "Model.AddModel.Added){ %>" + END;
                    Result += TAB3 + "alert(\"A new item has been added!\");" + END;
                    Result += TAB2 + "<% } %>" + END;
                    Result += TAB + "</script>" + END;
                }
            }

            Result += TAB + "<div id=\"PartialDiv\">" + END;
            Result += TAB2 + "<% Html.RenderPartial(\"Templates/TH_List" + GlobalVariables.g_ModelName + "\", Model); %>" + END;
            Result += TAB + "</div>" + END;

            if (TblHaveImgAttr || TblUsingFck)
            {
                Result += TAB + "<% using (Html.BeginForm(\"Insert" + GlobalVariables.g_ModelName + "\", \"Admin\", FormMethod.Post, new { enctype = \"multipart/form-data\", onsubmit = \"return Validate" + GlobalVariables.g_ModelName + "();\" }))" + END;
            }
            else
            {
                if (!GlobalVariables.g_colUsingAjax.Contains(GlobalVariables.g_ModelName))
                    Result += TAB + "<% using (Html.BeginForm(\"Insert" + GlobalVariables.g_ModelName + "\", \"Admin\", FormMethod.Post, new { onsubmit = \"return Validate" + GlobalVariables.g_ModelName + "();\" }))" + END;
                else
                    Result += TAB + "<% using (Ajax.BeginForm(\"Insert" + GlobalVariables.g_ModelName + "\", \"Admin\", new AjaxOptions { OnBegin = \"Validate" + GlobalVariables.g_ModelName + "\", OnComplete = \"jsonAdd_OnComplete\" }))" + END;
            }
            Result += TAB + "{ %>" + END;
            
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += TAB2 + "<input type=\"hidden\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" value=\"<%= Model." + GlobalVariables.g_ModelName + "Model.ReferKeys." + _table.Attributes[i].Name + " %>\" />" + END;
                }
            }

            Result += TAB2 + "<div style=\"padding-top: 40px\">" + END;
            Result += TAB3 + "<span style=\"font-size:18px; font-weight:bold;\">Add New " + GlobalVariables.g_ModelName + "</span>" + END;
            Result += TAB3 + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"AdminPageText\">" + END;
            Result += TAB4 + "<tbody>" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                {
                    if (_table.Attributes[i].Name == Utils.GetImageAttrName(_table))
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (_table.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : _table.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<input type=\"file\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" />" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB5 + "</tr>" + END;
                    }
                    else
                    {
                        Result += TAB5 + "<tr>" + END;
                        Result += TAB6 + "<td>" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;
                        Result += TAB8 + "<font class=\"normal8\">" + (_table.Attributes[i].Name.ToUpper().Contains("RUTGON") ? "Nội dung rút gọn hiển thị trên trang chủ" : _table.Attributes[i].Name) + ": </font>" + END;
                        Result += TAB7 + "</div>" + END;
                        Result += TAB6 + "</td>" + END;
                        Result += TAB6 + "<td height=\"20\">" + END;
                        Result += TAB7 + "<div align=\"left\" style=\"padding-top:15px\">" + END;

                        if (haveUsingFCK)
                        {
                            if(GlobalVariables.g_colUsingFCK.Values.Contains(_table.Attributes[i].Name))
                                Result += TAB8 + "<%= Html.TextArea(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")%>" + END;
                            else if(_table.Attributes[i].Name.ToUpper().Contains("RUTGON"))
                                Result += TAB8 + "<textarea name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" style=\"width: 590px; height: 80px;\"></textarea>" + END;
                            else
                                Result += TAB8 + "<%= Html.TextBox(\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\")%>" + END;

                        }
                        else
                            Result += TAB8 + "<input type=\"text\" name=\"" + GlobalVariables.g_ModelName + "_" + _table.Attributes[i].Name + "\" />" + END;

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
    }
}
