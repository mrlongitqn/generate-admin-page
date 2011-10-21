using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Helpers
{
    #region USING
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.Controllers;
    using GenerateAdminPage.Classes.Models;
    using System.IO;
    #endregion

    public class AdminCore
    {
        public string BasePath { get; set; }
        public DataBase DB { get; set; }

        public void InitPaths()
        {
            DataContext.Instance.DataTextProvider.InputPath = BasePath + @"\\Input\\DB.xml";
            DataContext.Instance.DataTextProvider.OutputRepositoriesPath = BasePath + @"\\Output\\Models\\Repositories\\";
            DataContext.Instance.DataTextProvider.OutputViewModelsPath = BasePath + @"\\Output\\Models\\ViewModels\\";
            DataContext.Instance.DataTextProvider.OutputControllersPath = BasePath + @"\\Output\\Controllers\\";
            DataContext.Instance.DataTextProvider.OutputViewsPath = BasePath + @"\\Output\\Views\\Admin\\";
            DataContext.Instance.DataTextProvider.OutputViewsTemplatePath = BasePath + @"\\Output\\Views\\Admin\\Templates\\";
            DataContext.Instance.DataTextProvider.OutputValidateScriptPath = BasePath + @"\\Output\\Scripts\\";
        }

        public void Generate()
        {
            try
            {
                InitPaths();

                if (DataContext.Instance.DataTextProvider.LoadDBInfo())
                {
                    Console.WriteLine("Load DB info successfully");
                    DataContext.Instance.CreateConnection();
                    DB = new DataBase();
                    DataContext.Instance.InitDB(DB);
                    Console.WriteLine("Init DB successfully");
                    DataContext.Instance.SetForeignKey(DB);
                    Console.WriteLine("Set ForeignKeys successfully");
                    GenerateOutput(DB);
                    Console.WriteLine("Generated");
                }
                else
                {
                    throw new Exception("Cannot load DB info");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GenerateOutput(DataBase DB)
        {
            DeleteOldFiles();
            GenerateRepositories(DB);
            GenerateViewModels(DB);
            GenerateGroupViewModel();
            GenerateDataTransferViewModel(DB);
            GenerateBaseController(DB);
            GenerateControllers(DB);
            GenerateViews(DB);
            GenerateValidateScript(DB);
            DeleteTrashFiles();
        }

        public void GenerateValidateScript(DataBase DB)
        {
            IBase _rep = null;
            string result = "";
            string path = "";

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                         !DB.Tables[i].Name.StartsWith("aspnet_") &&
                         DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                {
                    if (!Utils.IsPKWithMoreThan1Attr(DB.Tables[i]))
                    {
                        _rep = new ValidateScript();
                        _rep.InitClassGenerate(DB, DB.Tables[i]);
                        result += _rep.GenerateView(EnumView.SELECT) + Environment.NewLine;
                    }
                }
            }
            result += GenerateValidateNguoiDung();
            path = DataContext.Instance.DataTextProvider.OutputValidateScriptPath + "ValidationInputs.js";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated ValidationInputs");
        }

        public string GenerateValidateNguoiDung()
        {
            string END = Environment.NewLine;
            const string TAB = "\t";
            const string TAB2 = "\t\t";
            string Result = "";

            Result += "function Validate" + GlobalVariables.g_sTableNguoiDung + "() {" + END;

            Result += TAB + "var username = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_UserName" + "\").item(0);" + END;
            Result += TAB + "if (username.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input UserName\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "var password = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_Password" + "\").item(0);" + END;
            Result += TAB + "if (password.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input Password\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "var email = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_Email" + "\").item(0);" + END;
            Result += TAB + "if (email.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input Email\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "return true;" + END;
            Result += "}" + END;

            return Result;
        }

        public void GenerateViews(DataBase DB)
        {
            IBase _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                         !DB.Tables[i].Name.StartsWith("aspnet_") &&
                         DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                {
                    if (!Utils.IsPKWithMoreThan1Attr(DB.Tables[i]))
                    {
                        _rep = AbstractView.SelectGenerateType(DB.Tables[i]);
                        _rep.InitClassGenerate(DB, DB.Tables[i]);

                        result = _rep.GenerateView(EnumView.SELECT);
                        path = DataContext.Instance.DataTextProvider.OutputViewsPath + "Select" + DB.Tables[i].Name + ".aspx";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);

                        result = _rep.GenerateView(EnumView.TEMPLATE_LIST);
                        path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_List" + DB.Tables[i].Name + ".ascx";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);

                        result = _rep.GenerateView(EnumView.TEMPLATE_EDIT_OR_DETAILOF);
                        if (!Utils.TableUsingFCK(DB.Tables[i]) && !Utils.TableHaveImageAttribute(DB.Tables[i]))
                        {
                            path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_Edit" + DB.Tables[i].Name + ".ascx";
                        }
                        else
                        {
                            path = DataContext.Instance.DataTextProvider.OutputViewsPath + "DetailOf" + DB.Tables[i].Name + ".aspx";
                        }
                        DataContext.Instance.DataTextProvider.WriteData(result, path);
                        Console.WriteLine("Generated view of table " + DB.Tables[i].Name);
                    }
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = AbstractView.SelectGenerateType(nguoiDung);
            _rep.InitClassGenerate(DB, nguoiDung);
            result = _rep.GenerateView(EnumView.SELECT);
            path = DataContext.Instance.DataTextProvider.OutputViewsPath + "Select" + GlobalVariables.g_sTableNguoiDung + ".aspx";
            DataContext.Instance.DataTextProvider.WriteData(result, path);

            result = _rep.GenerateView(EnumView.TEMPLATE_LIST);
            path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_List" + GlobalVariables.g_sTableNguoiDung + ".ascx";
            DataContext.Instance.DataTextProvider.WriteData(result, path);

            result = _rep.GenerateView(EnumView.TEMPLATE_EDIT_OR_DETAILOF);
            if (nguoiDung == null)
            {
                path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_Edit" + GlobalVariables.g_sTableNguoiDung + ".ascx";
            }
            else
            {
                path = DataContext.Instance.DataTextProvider.OutputViewsPath + "DetailOf" + GlobalVariables.g_sTableNguoiDung + ".aspx";
            }
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated view of table " + GlobalVariables.g_sTableNguoiDung);
        }

        public void GenerateDataTransferViewModel(DataBase DB)
        {
            IBase _rep = new DataTransferViewModel();
            _rep.InitClassGenerate(DB, null);
            var result = _rep.GenerateNameSpace("Controllers");
            var path = DataContext.Instance.DataTextProvider.OutputControllersPath + "DataTransferViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated DataTransferViewModel");
        }

        public void GenerateRepositories(DataBase DB)
        {
            try
            {
                IBase _rep = null;
                string result = "";
                string path = "";
                Table nguoiDung = null;//extra info

                for (int i = 0; i < DB.Tables.Count; i++)
                {
                    if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                        !DB.Tables[i].Name.StartsWith("aspnet_") &&
                        DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                    {
                        _rep = AbstractRepository.SelectGenerateType(DB.Tables[i]);
                        _rep.InitClassGenerate(DB, DB.Tables[i]);
                        result = _rep.GenerateNameSpace("Models.Repositories");
                        path = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + DB.Tables[i].Name + "Repository.cs";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);
                        Console.WriteLine("Generated Repository of table " + DB.Tables[i].Name);
                    }
                    else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                    {
                        nguoiDung = DB.Tables[i];
                    }
                }

                //Generate NguoiDung view model
                _rep = AbstractRepository.SelectGenerateType(nguoiDung);
                _rep.InitClassGenerate(DB, nguoiDung);
                result = _rep.GenerateNameSpace("Models.Repositories");
                path = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + GlobalVariables.g_sTableNguoiDung + "Repository.cs";
                DataContext.Instance.DataTextProvider.WriteData(result, path);
                Console.WriteLine("Generated Repository of table " + GlobalVariables.g_sTableNguoiDung);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GenerateGroupViewModel()
        {
            string result = "";
            string path = "";

            IBase _rep = new GroupViewModel();
            _rep.InitClassGenerate(DB, null);
            result = _rep.GenerateNameSpace("Models.ViewModels");
            path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + "GroupViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated GroupViewModel");
        }

        public void GenerateViewModels(DataBase DB)
        {
            IBase _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                        !DB.Tables[i].Name.StartsWith("aspnet_") &&
                        DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                {
                    _rep = AbstractViewModel.SelectGenerateType(DB.Tables[i]);
                    _rep.InitClassGenerate(DB, DB.Tables[i]);
                    result = _rep.GenerateNameSpace("Models.ViewModels");
                    path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + DB.Tables[i].Name + "ViewModel.cs";
                    DataContext.Instance.DataTextProvider.WriteData(result, path);
                    Console.WriteLine("Generated ViewModel of table " + DB.Tables[i].Name);
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = AbstractViewModel.SelectGenerateType(nguoiDung);
            _rep.InitClassGenerate(DB, nguoiDung);
            result = _rep.GenerateNameSpace("Models.ViewModels");
            path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + GlobalVariables.g_sTableNguoiDung + "ViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated ViewModel of table " + GlobalVariables.g_sTableNguoiDung);
        }

        public void GenerateAdminController()
        {
            IBase _rep = null;
            string result = "";
            string path = "";

            _rep = new AdminController();
            _rep.InitClassGenerate(DB, null);
            result = _rep.GenerateNameSpace("Controllers");
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + "AdminController.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated AdminController");
        }

        public void GenerateBaseController(DataBase DB)
        {
            IBase _rep = null;
            string result = "";
            string path = "";

            _rep = new BaseController();
            _rep.InitClassGenerate(DB, null);
            result = _rep.GenerateNameSpace("Controllers");
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + "BaseController.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated BaseController");
        }

        public void GenerateControllers(DataBase DB)
        {
            IBase _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            GenerateAdminController();

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung &&
                        !DB.Tables[i].Name.StartsWith("aspnet_") &&
                        DB.Tables[i].Name != "dtproperties" && DB.Tables[i].Name != "sysdiagrams")
                {
                    _rep = AbstractController.SelectGenerateType(DB.Tables[i]);
                    _rep.InitClassGenerate(DB, DB.Tables[i]);
                    result = _rep.GenerateNameSpace("Controllers");
                    path = DataContext.Instance.DataTextProvider.OutputControllersPath + DB.Tables[i].Name + "Controller.cs";
                    DataContext.Instance.DataTextProvider.WriteData(result, path);
                    Console.WriteLine("Generated Controller " + DB.Tables[i].Name);
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = AbstractController.SelectGenerateType(nguoiDung);
            _rep.InitClassGenerate(DB, nguoiDung);
            result = _rep.GenerateNameSpace("Controllers");
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + GlobalVariables.g_sTableNguoiDung +"Controller.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
            Console.WriteLine("Generated Controller " + GlobalVariables.g_sTableNguoiDung);
        }

        public void DeleteOldFiles()
        {
            try
            {
                var viewmodelPath = DataContext.Instance.DataTextProvider.OutputViewModelsPath;
                var repPath = DataContext.Instance.DataTextProvider.OutputRepositoriesPath;
                var viewPath = DataContext.Instance.DataTextProvider.OutputViewsPath;
                var controllerPath = DataContext.Instance.DataTextProvider.OutputControllersPath;
                var templatePath = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath;
                var scriptPath = DataContext.Instance.DataTextProvider.OutputValidateScriptPath;

                if (System.IO.Directory.Exists(controllerPath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(controllerPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (System.IO.Directory.Exists(templatePath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(templatePath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (System.IO.Directory.Exists(scriptPath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(scriptPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (System.IO.Directory.Exists(viewmodelPath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(viewmodelPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (System.IO.Directory.Exists(repPath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(repPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (System.IO.Directory.Exists(viewPath))
                {
                    try
                    {
                        string[] filePaths = Directory.GetFiles(viewPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
                Console.WriteLine("Delete old files");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteTrashFiles()
        {
            var controllerPath = DataContext.Instance.DataTextProvider.OutputControllersPath + "\\trash.xml";
            var viewmodelPath = DataContext.Instance.DataTextProvider.OutputViewModelsPath + "\\trash.xml";
            var repPath = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + "\\trash.xml";
            var scriptPath = DataContext.Instance.DataTextProvider.OutputValidateScriptPath + "\\trash.xml";
            var viewPath = DataContext.Instance.DataTextProvider.OutputViewsPath + "\\Templates\\trash.xml";

            if (System.IO.File.Exists(controllerPath))
            {
                try
                {
                    System.IO.File.Delete(controllerPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            if (System.IO.File.Exists(viewmodelPath))
            {
                try
                {
                    System.IO.File.Delete(viewmodelPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            if (System.IO.File.Exists(repPath))
            {
                try
                {
                    System.IO.File.Delete(repPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            if (System.IO.File.Exists(scriptPath))
            {
                try
                {
                    System.IO.File.Delete(scriptPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            if (System.IO.File.Exists(viewPath))
            {
                try
                {
                    System.IO.File.Delete(viewPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            Console.WriteLine("Deleted all trash files");
        }

        public Table GetNguoiDungBaseInfo()
        {
            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name == "aspnet_Users")
                    return DB.Tables[i];
            }

            return null;
        }
    }
}
