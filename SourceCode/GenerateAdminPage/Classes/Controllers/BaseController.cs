using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class BaseController : AdminController
    {
        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using System;" + END;
            Result += TAB + "using System.Web.Mvc;" + END;
            Result += TAB + "using System.Web.Security;" + END;
            Result += TAB + "using System.Collections.Generic;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.ViewModels;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.Repositories;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models;" + END;
            Result += TAB + "using System.IO;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public abstract class BaseController : Controller" + END;
            Result += TAB + "{" + END;

            Result += GenerateRepositoryVals() + END;
            Result += GenerateRenderPartialView() + END;
            Result += GenerateCreateViewModel() + END;
            Result += GenerateUploadFile() + END;
            Result += GenerateDeleteFile() + END;
            Result += GenerateSendingEmail() + END;
            Result += GenerateListRetrieveViewModel() + END;
            Result += GenerateRetrieveNguoiDungViewModel() + END;
            Result += GenerateDetailOf() + END;

            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateDetailOf()
        {
            string Result = "";

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name == GlobalVariables.g_sTableNguoiDung ||
                    Utils.TableUsingFCK(_db.Tables[i]) || Utils.TableHaveImageAttribute(_db.Tables[i]))
                {
                    Result += TAB2 + "public " + _db.Tables[i].Name + "ViewModel RetrieveDetailOf" + _db.Tables[i].Name + "ViewModel(DataTransferViewModel dataTransfer)" + END;
                    Result += TAB2 + "{" + END;
                    Result += TAB3 + "return new " + _db.Tables[i].Name + "ViewModel" + END;
                    Result += TAB3 + "{" + END;
                    Result += TAB4 + "GetModel = new Get" + _db.Tables[i].Name + "ViewModel" + END;
                    Result += TAB4 + "{" + END;

                    var id = "";
                    var PK = Utils.GetPK(_db.Tables[i]);

                    if (PK.Type == DataType.STRING)
                    {
                        id = "StrID";
                    }
                    else if (PK.Type == DataType.GUILD)
                    {
                        id = "GuidID";
                    }
                    else
                    {
                        id = "IntID";
                    }

                    Result += TAB5 + "LstObjModel = _rep" + _db.Tables[i].Name + ".RetrieveByID(dataTransfer." + id + ")" + END;

                    Result += TAB4 + "}," + END;
                    Result += TAB4 + "EditModel = new Edit" + _db.Tables[i].Name + "ViewModel" + END;
                    Result += TAB4 + "{" + END;
                    Result += TAB5 + "ID = dataTransfer." + id + "," + END;
                    Result += TAB4 + "}," + END;
                    Result += TAB4 + "InfoText = dataTransfer.InfoText" + END;
                    Result += TAB3 + "};" + END;
                    Result += TAB2 + "}" + END;
                }
            }
            return Result;
        }

        public string GenerateRepositoryVals()
        {
            string Result = "";

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && _db.Tables[i].Name != "aspnet_Users")
                {
                    Result += TAB2 + "protected " + _db.Tables[i].Name + "Repository _rep" + _db.Tables[i].Name + " = new " + _db.Tables[i].Name + "Repository();" + END;
                }
            }
            Result += TAB2 + "protected " + GlobalVariables.g_sTableNguoiDung + "Repository _rep" + GlobalVariables.g_sTableNguoiDung + " = new " + GlobalVariables.g_sTableNguoiDung + "Repository();" + END;
            return Result;
        }

        public string GenerateRenderPartialView()
        {
            string Result = "";

            Result += TAB2 + "protected string RenderPartialViewToString()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return RenderPartialViewToString(null, null);" + END;
            Result += TAB2 + "}" + END;

            Result += END;

            Result += TAB2 + "protected string RenderPartialViewToString(string viewName)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return RenderPartialViewToString(viewName, null);" + END;
            Result += TAB2 + "}" + END;

            Result += END;

            Result += TAB2 + "protected string RenderPartialViewToString(string viewName, object model)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "if (string.IsNullOrEmpty(viewName))" + END;
            Result += TAB4 + "viewName = ControllerContext.RouteData.GetRequiredString(\"action\");" + END;
            Result += TAB3 + "ViewData.Model = model;" + END;
            Result += TAB3 + "using (StringWriter sw = new StringWriter())" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);" + END;
            Result += TAB4 + "ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);" + END;
            Result += TAB4 + "viewResult.View.Render(viewContext, sw);" + END;
            Result += TAB4 + "return sw.GetStringBuilder().ToString();" + END;
            Result += TAB3 + "}" + END;

            Result += TAB2 + "}" + END;

            Result += END;


            return Result;
        }

        public string GenerateCreateViewModel()
        {
            string Result = "";

            Result += TAB2 + "public GroupViewModel CreateViewModel(DataTransferViewModel dataTransfer)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "GroupViewModel viewModel = new GroupViewModel();" + END;

            Result += TAB3 + "if (dataTransfer.EnumViewModelType.ToString().Contains(\"ADMIN\"))" + END;
            Result += TAB3 + "{" + END;
            Result += TAB3 + "}" + END;

            Result += TAB3 + "switch (dataTransfer.EnumViewModelType)" + END;
            Result += TAB3 + "{" + END;
            
            for (int i = 0; i < GlobalVariables.g_colEnumViewModel.Count; i++)
            {
                Result += TAB4 + "case EnumViewModel." + GlobalVariables.g_colEnumViewModel[i] + ":" + END;
                Result += TAB5 + "break;" + END;
            }

            Result += TAB3 + "}" + END;
            Result += TAB3 + "return viewModel;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateUploadFile()
        {
            string Result = "";

            Result += TAB2 + "public bool UploadFile(HttpPostedFileBase file, string path, string name, ref string refName)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "if (!System.IO.File.Exists(Server.MapPath(path + file.FileName)))" + END;
            Result += TAB4 + "{" + END;

            Result += TAB5 + "FileInfo f = new FileInfo(file.FileName);" + END;
            Result += TAB5 + "long ngay = DateTime.Now.ToBinary();" + END;
            Result += TAB5 + "refName = name + ngay + f.Extension;" + END;
            Result += TAB5 + "string fullPath = path + refName;" + END;
            Result += TAB5 + "if (file == null)" + END;
            Result += TAB5 + "return false;" + END;
            Result += TAB5 + "byte[] buffer = new byte[file.ContentLength];" + END;
            Result += TAB5 + "file.InputStream.Read(buffer, 0, file.ContentLength);" + END;
            Result += TAB5 + "FileStream newFile = new FileStream(Server.MapPath(fullPath), FileMode.Create, FileAccess.Write);" + END;
            Result += TAB5 + "newFile.Write(buffer, 0, file.ContentLength);" + END;
            Result += TAB5 + "newFile.Close();" + END;

            Result += TAB4 + "}" + END;
            Result += TAB4 + "else" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "refName = file.FileName;" + END;
            Result += TAB4 + "}" + END;
            Result += TAB4 + "return true;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "catch" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "return false;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB2 + "}" + END;
            
            return Result;
        }

        public string GenerateSendingEmail()
        {
            string Result = "";

            Result += TAB2 + "public bool SendingMail(string subject, string emailContent, string to)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "bool result = false;" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + "System.Net.Mail.SmtpClient mailclient = new System.Net.Mail.SmtpClient();" + END;
            Result += TAB4 + "System.Net.NetworkCredential auth = new System.Net.NetworkCredential(WebConfiguration.EmailLienHe, WebConfiguration.PasswordEmailLienHe);" + END;
            Result += TAB4 + "mailclient.Host = WebConfiguration.MailServer;" + END;
            Result += TAB4 + "mailclient.EnableSsl = true;" + END;
            Result += TAB4 + "mailclient.UseDefaultCredentials = true;" + END;
            Result += TAB4 + "mailclient.Credentials = auth;" + END;
            Result += TAB4 + "System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage(WebConfiguration.EmailLienHe, to);" + END;
            Result += TAB4 + "mm.Subject = subject;" + END;
            Result += TAB4 + "mm.IsBodyHtml = true;" + END;
            Result += TAB4 + "mm.Body = emailContent;" + END;
            Result += TAB4 + "mailclient.Send(mm);" + END;
            Result += TAB4 + "result = true;" + END;

            Result += TAB3 + "}" + END;
            Result += TAB3 + "catch" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "result = false;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "return result;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateListRetrieveViewModel()
        {
            string Result = "";

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && _db.Tables[i].Name != "aspnet_Users")
                {
                    if (!Utils.TableHaveForeignKey(_db.Tables[i]))
                    {
                        Result += GenerateRetrieveViewModel(_db.Tables[i]) + END;
                    }
                    else
                    {
                        Result += GenerateRetrieveViewModelByFK(_db.Tables[i]) + END;
                    }
                }
            }

            return Result;
        }

        public string   GenerateRetrieveViewModelByFK(Table _tbl)
        {
            string Result = "";

            Result += TAB2 + "public " + _tbl.Name + "ViewModel Retrieve" + _tbl.Name + "ViewModel(DataTransferViewModel dataTransfer)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "List<" + _tbl.Name + "> lst = null;" + END;
            Result += TAB3 + "int totalitem = -1;" + END;

            int firstPost = -1;
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsForeignKey)
                {
                    firstPost = i;
                    break;
                }
            }

            if (firstPost != -1)
            {
                for (int i = 0; i < _tbl.Attributes.Count; i++)
                {
                    if (_tbl.Attributes[i].IsForeignKey)
                    {
                        if (i == firstPost)
                        {
                            if (_tbl.Attributes[i].Type != DataType.GUILD)
                            {
                                if (_tbl.Attributes[i].Type == DataType.STRING)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != \"\")" + END;
                                }
                                else if (_tbl.Attributes[i].Type == DataType.BOOL)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != false)" + END;
                                }
                                else if (_tbl.Attributes[i].Type == DataType.DATETIME)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != Guid.Parse(\"1/1/1\"))" + END;
                                }
                                else
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != -1)" + END;
                                }
                            }
                            else
                                Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != Guid.Parse(\"" + GlobalVariables.g_DefaultGuid + "\"))" + END;
                        }
                        else
                        {
                            if (_tbl.Attributes[i].Type != DataType.GUILD)
                            {
                                if (_tbl.Attributes[i].Type == DataType.STRING)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != \"\")" + END;
                                }
                                else if (_tbl.Attributes[i].Type == DataType.BOOL)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != false)" + END;
                                }
                                else if (_tbl.Attributes[i].Type == DataType.DATETIME)
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != Guid.Parse(\"1/1/1\"))" + END;
                                }
                                else
                                {
                                    Result += TAB3 + "if (dataTransfer." + _tbl.Attributes[i].Name + " != -1)" + END;
                                }
                            }
                            else
                                Result += TAB3 + "else if (dataTransfer." + _tbl.Attributes[i].Name + " != Guid.Parse(\"" + GlobalVariables.g_DefaultGuid + "\"))" + END;
                        }
                        Result += TAB3 + "{" + END;

                        if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), _tbl.Name))
                        {
                            Result += TAB4 + "lst = _rep" + _tbl.Name + ".SelectBy" + _tbl.Attributes[i].Name + "(dataTransfer." + _tbl.Attributes[i].Name + ", dataTransfer.CurrentPage, WebConfiguration.Num" + _tbl.Name + "PerPage);" + END;
                        }
                        else
                        {
                            Result += TAB4 + "lst = _rep" + _tbl.Name + ".SelectBy" + _tbl.Attributes[i].Name + "(dataTransfer." + _tbl.Attributes[i].Name + ");" + END;
                        }

                        Result += TAB4 + "totalitem = _rep" + _tbl.Name + ".GetTotalItemBy" + _tbl.Attributes[i].Name + "(dataTransfer." + _tbl.Attributes[i].Name + ");" + END;
                        Result += TAB3 + "}" + END;
                    }
                }
            }
            Result += TAB3 + "else" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "lst = _rep" + _tbl.Name + ".SelectPaging" + "(dataTransfer.CurrentPage, WebConfiguration.Num" + _tbl.Name + "PerPage);" + END;
            Result += TAB4 + "totalitem = _rep" + _tbl.Name + ".GetTotalItem();" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "return new " + _tbl.Name + "ViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "GetModel = new Get" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "LstObjModel = lst," + END;
            Result += TAB5 + "TotalItem = totalitem," + END;
            Result += TAB5 + "CurrentPage = dataTransfer.CurrentPage" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "AddModel = new Add" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "EditModel = new Edit" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsPrimaryKey)
                {
                    if (_tbl.Attributes[i].Type == DataType.STRING)
                    {
                        Result += TAB5 + "ID = dataTransfer.StrID," + END;
                    }
                    else if (_tbl.Attributes[i].Type == DataType.GUILD)
                    {
                        Result += TAB5 + "ID = dataTransfer.GuidID," + END;
                    }
                    else
                    {
                        Result += TAB5 + "ID = dataTransfer.IntID," + END;
                    }
                }
            }
            //Result += TAB5 + "ID = dataTransfer.EditID" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "ReferKeys = new " + _tbl.Name + "ReferKeys()" + END;
            Result += TAB4 + "{" + END;

            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsForeignKey)
                {
                    if (i < _tbl.Attributes.Count - 1)
                        Result += TAB5 + _tbl.Attributes[i].Name + " = dataTransfer." + _tbl.Attributes[i].Name + "," + END;
                    else
                        Result += TAB5 + _tbl.Attributes[i].Name + " = dataTransfer." + _tbl.Attributes[i].Name + END;
                }
            }

            Result += TAB4 + "}," + END;
            Result += TAB4 + "InfoText = dataTransfer.InfoText" + END;
            Result += TAB3 + "};" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateRetrieveViewModel(Table _tbl)
        {
            string Result = "";

            Result += TAB2 + "public " + _tbl.Name + "ViewModel Retrieve" + _tbl.Name + "ViewModel(DataTransferViewModel dataTransfer)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return new " + _tbl.Name + "ViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "GetModel = new Get" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;

            if (GlobalVariables.g_colPaging.Contains(_tbl.Name))
            {
                Result += TAB5 + "LstObjModel = _rep" + _tbl.Name + ".SelectPaging(dataTransfer.CurrentPage, WebConfiguration.Num" + _tbl.Name + "PerPage)," + END;
            }
            else
            {
                Result += TAB5 + "LstObjModel = _rep" + _tbl.Name + ".SelectAll()," + END;
            }

            Result += TAB5 + "TotalItem = _rep" + _tbl.Name + ".GetTotalItem()," + END;
            Result += TAB5 + "CurrentPage = dataTransfer.CurrentPage" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "AddModel = new Add" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "EditModel = new Edit" + _tbl.Name + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsPrimaryKey)
                {
                    if (_tbl.Attributes[i].Type == DataType.STRING)
                    {
                        Result += TAB5 + "ID = dataTransfer.StrID," + END;
                    }
                    else if (_tbl.Attributes[i].Type == DataType.GUILD)
                    {
                        Result += TAB5 + "ID = dataTransfer.GuidID," + END;
                    }
                    else
                    {
                        Result += TAB5 + "ID = dataTransfer.IntID," + END;
                    }
                }
            }
            Result += TAB4 + "}," + END;
            Result += TAB4 + "InfoText = dataTransfer.InfoText" + END;
            Result += TAB3 + "};" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateRetrieveNguoiDungViewModel()
        {
            string Result = "";

            Result += TAB2 + "public " + GlobalVariables.g_ModelName + "ViewModel Retrieve" + GlobalVariables.g_ModelName + "ViewModel(DataTransferViewModel dataTransfer)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return new " + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "GetModel = new Get" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "LstObjModel = _rep" + GlobalVariables.g_ModelName + ".SelectPaging(dataTransfer.CurrentPage, WebConfiguration.Num" + GlobalVariables.g_ModelName + "PerPage, dataTransfer.Role)," + END;
            Result += TAB5 + "TotalItem = _rep" + GlobalVariables.g_ModelName + ".GetTotalItem(dataTransfer.Role)," + END;
            Result += TAB5 + "CurrentPage = dataTransfer.CurrentPage" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "AddModel = new Add" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB4 + "}," + END;
            Result += TAB4 + "EditModel = new Edit" + GlobalVariables.g_ModelName + "ViewModel" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "ID = dataTransfer.GuidID," + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "};" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateDeleteFile()
        {
            string Result = "";

            Result += TAB2 + "public void DeleteFile(string filePath)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "FileInfo file = new FileInfo(filePath);" + END;
            Result += TAB3 + "if (file.Exists)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "file.Delete();" + END;
            Result += TAB3 + "}" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}

