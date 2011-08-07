using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
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

        public string Generate()
        {
            string errText = "";
            string dataSource = "";
            string databaseName = "";
            try
            {
                InitPaths();

                if (DataContext.Instance.DataTextProvider.LoadDBInfo(ref dataSource, ref databaseName))
                {
                    DataContext.Instance.CreateConnection(dataSource, databaseName);
                    var DB1 = new DataBase();
                    DataContext.Instance.InitDB(ref DB1);
                    DataContext.Instance.SetForeignKey(ref DB1);

                    DB = DB1;
                    GenerateOutput(DB);
                }
            }
            catch (Exception ex)
            {
                errText = ex.Message;
            }

            return errText;
        }

        public void GenerateOutput(DataBase DB)
        {
            GenerateRepositories(DB);
            GenerateViewModels(DB);
            GenerateControllers(DB);
            GenerateDataTransferViewModel(DB);
            GenerateGroupViewModel();
            GenerateBaseController(DB);
            GenerateViews(DB);
            GenerateValidateScript(DB);
            DeleteTrashFiles();
        }

        public void GenerateValidateScript(DataBase DB)
        {
            ValidateScript _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && DB.Tables[i].Name != "aspnet_Users")
                {
                    if (!Utils.IsPKWithMoreThan1Attr(DB.Tables[i]))
                    {
                        _rep = new ValidateScript();
                        _rep.InitView(DB, DB.Tables[i]);
                        result += _rep.GenerateView(EnumView.SELECT) + Environment.NewLine;
                    }
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }
            result += _rep.GenerateValidateNguoiDung() + Environment.NewLine;
            path = DataContext.Instance.DataTextProvider.OutputValidateScriptPath + "ValidationInputs.js";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }
        public void GenerateViews(DataBase DB)
        {
            Views _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && DB.Tables[i].Name != "aspnet_Users" && DB.Tables[i].Name != "sysdiagrams")
                {
                    if (!Utils.IsPKWithMoreThan1Attr(DB.Tables[i]))
                    {
                        _rep = new ViewNotNguoiDung();
                        _rep.InitView(DB, DB.Tables[i]);
                        result = _rep.GenerateView(EnumView.SELECT);
                        path = DataContext.Instance.DataTextProvider.OutputViewsPath + "Select" + DB.Tables[i].Name + ".aspx";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);

                        result = _rep.GenerateView(EnumView.TEMPLATE_LIST);
                        path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_List" + DB.Tables[i].Name + ".ascx";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);

                        result = _rep.GenerateView(EnumView.TEMPLATE_EDIT_OR_DETAILOF);
                        if (!Utils.TableUsingFCK(DB.Tables[i]) && !Utils.TableHaveImageAttribute(DB.Tables[i]))
                            path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_Edit" + DB.Tables[i].Name + ".ascx";
                        else
                            path = DataContext.Instance.DataTextProvider.OutputViewsPath + "DetailOf" + DB.Tables[i].Name + ".aspx";
                        DataContext.Instance.DataTextProvider.WriteData(result, path);
                    }
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = new ViewNguoiDung
            {
                _BaseInfo = GetNguoiDungBaseInfo()
            };
            _rep.InitView(DB, nguoiDung);
            result = _rep.GenerateView(EnumView.SELECT);
            path = DataContext.Instance.DataTextProvider.OutputViewsPath + "Select" + GlobalVariables.g_sTableNguoiDung + ".aspx";
            DataContext.Instance.DataTextProvider.WriteData(result, path);

            result = _rep.GenerateView(EnumView.TEMPLATE_LIST);
            path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_List" + GlobalVariables.g_sTableNguoiDung + ".ascx";
            DataContext.Instance.DataTextProvider.WriteData(result, path);

            result = _rep.GenerateView(EnumView.TEMPLATE_EDIT_OR_DETAILOF);
            if (nguoiDung == null)
                path = DataContext.Instance.DataTextProvider.OutputViewsTemplatePath + "TH_Edit" + GlobalVariables.g_sTableNguoiDung + ".ascx";
            else
                path = DataContext.Instance.DataTextProvider.OutputViewsPath + "DetailOf" + GlobalVariables.g_sTableNguoiDung + ".aspx";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateDataTransferViewModel(DataBase DB)
        {
            DataTransferViewModel _rep = new DataTransferViewModel();
            var result = _rep.GenerateClasses(DB, null);
            var path = DataContext.Instance.DataTextProvider.OutputControllersPath + "DataTransferViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateRepositories(DataBase DB)
        {
            Repository _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && DB.Tables[i].Name != "aspnet_Users" && DB.Tables[i].Name != "sysdiagrams")
                {
                    _rep = new Repository();
                    result = _rep.GenerateClasses(DB, DB.Tables[i]);
                    path = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + DB.Tables[i].Name + "Repository.cs";
                    DataContext.Instance.DataTextProvider.WriteData(result, path);
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = new NguoiDungRepository
            {
                
            };
            result = _rep.GenerateClasses(DB, nguoiDung);
            path = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + GlobalVariables.g_sTableNguoiDung + "Repository.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateGroupViewModel()
        {
            string result = "";
            string path = "";

            GroupViewModel _rep = new GroupViewModel();
            result = _rep.GenerateClasses(DB, null);
            path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + "GroupViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateViewModels(DataBase DB)
        {
            ViewModel _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && DB.Tables[i].Name != "aspnet_Users" && DB.Tables[i].Name != "sysdiagrams")
                {
                    _rep = new ViewModel();
                    result = _rep.GenerateClasses(DB, DB.Tables[i]);
                    path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + DB.Tables[i].Name + "ViewModel.cs";
                    DataContext.Instance.DataTextProvider.WriteData(result, path);
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = new NguoiDungViewModel
            {
                _BaseInfo = GetNguoiDungBaseInfo()
            };
            result = _rep.GenerateClasses(DB, nguoiDung);
            path = DataContext.Instance.DataTextProvider.OutputViewModelsPath + GlobalVariables.g_sTableNguoiDung + "ViewModel.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateAdminController()
        {
            AdminController _rep = null;
            string result = "";
            string path = "";

            _rep = new AdminController();
            result = _rep.GenerateClasses(null, null);
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + "AdminController.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateBaseController(DataBase DB)
        {
            AdminController _rep = null;
            string result = "";
            string path = "";

            _rep = new BaseController();
            result = _rep.GenerateClasses(DB, null);
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + "BaseController.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void GenerateControllers(DataBase DB)
        {
            AdminController _rep = null;
            string result = "";
            string path = "";
            Table nguoiDung = null;//extra info

            GenerateAdminController();

            for (int i = 0; i < DB.Tables.Count; i++)
            {
                if (DB.Tables[i].Name != GlobalVariables.g_sTableNguoiDung && DB.Tables[i].Name != "aspnet_Users" && DB.Tables[i].Name != "sysdiagrams")
                {
                    if (Utils.TableHaveImageAttribute(DB.Tables[i]))
                        _rep = new AdminModelsControllerWithImage();
                    else
                        _rep = new AdminModelsControllerWithoutImage();

                    result = _rep.GenerateClasses(DB, DB.Tables[i]);
                    path = DataContext.Instance.DataTextProvider.OutputControllersPath + DB.Tables[i].Name + "Controller.cs";
                    DataContext.Instance.DataTextProvider.WriteData(result, path);
                }
                else if (DB.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                {
                    nguoiDung = DB.Tables[i];
                }
            }

            //Generate NguoiDung view model
            _rep = new AdminNguoiDungController
            {
                _BaseInfo = GetNguoiDungBaseInfo()
            };
            result = _rep.GenerateClasses(DB, nguoiDung);
            path = DataContext.Instance.DataTextProvider.OutputControllersPath + GlobalVariables.g_sTableNguoiDung +"Controller.cs";
            DataContext.Instance.DataTextProvider.WriteData(result, path);
        }

        public void DeleteTrashFiles()
        {
            var controllerPath = DataContext.Instance.DataTextProvider.OutputControllersPath + "\\trash.xml";
            var viewmodelPath = DataContext.Instance.DataTextProvider.OutputViewModelsPath + "\\trash.xml";
            var repPath = DataContext.Instance.DataTextProvider.OutputRepositoriesPath + "\\trash.xml";
            var scriptPath = DataContext.Instance.DataTextProvider.OutputValidateScriptPath + "\\trash.xml";

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
