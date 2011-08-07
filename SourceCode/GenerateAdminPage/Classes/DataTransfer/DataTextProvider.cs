using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    #region Using
    using System.Xml;
    using System.IO;
    #endregion

    public class DataTextProvider
    {
        #region Variables
        public string InputPath { get; set; }
        public string OutputRepositoriesPath { get; set; }
        public string OutputViewModelsPath { get; set; }
        public string OutputControllersPath { get; set; }
        public string OutputViewsPath { get; set; }
        public string OutputViewsTemplatePath { get; set; }
        public string OutputValidateScriptPath { get; set; }
        #endregion

        #region Methods
        public bool LoadDBInfo(ref string dataSoure, ref string dataBaseName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(InputPath);

                dataSoure = doc.GetElementsByTagName("datasource")[0].InnerText;
                dataBaseName = doc.GetElementsByTagName("databasename")[0].InnerText;
                GlobalVariables.g_sDatabaseName = dataBaseName;
                GlobalVariables.g_sNameSpace = doc.GetElementsByTagName("namespace")[0].InnerText;
                GlobalVariables.g_sTableNguoiDung = doc.GetElementsByTagName("usertablename")[0].InnerText;
                GlobalVariables.g_sSuperAdmin = doc.GetElementsByTagName("superadmin")[0].InnerText;
                GlobalVariables.g_sAdmin = doc.GetElementsByTagName("admin")[0].InnerText;

                var lst = doc.GetElementsByTagName("usingfck");
                for (int i = 0; i < lst[0].ChildNodes.Count; i++)
                {
                    GlobalVariables.g_colUsingFCK.Add(lst[0].ChildNodes[i].Attributes["id"].Value, lst[0].ChildNodes[i].Attributes["attr"].Value);
                }

                lst = doc.GetElementsByTagName("tablehaveimage");
                for (int i = 0; i < lst[0].ChildNodes.Count; i++)
                {
                    GlobalVariables.g_colTableHaveImage.Add(lst[0].ChildNodes[i].Attributes["id"].Value, lst[0].ChildNodes[i].Attributes["attr"].Value);
                }
                GlobalVariables.g_DefaultGuid = Guid.Parse(doc.GetElementsByTagName("defaultguid")[0].InnerText);
                GlobalVariables.g_sNoImages = doc.GetElementsByTagName("noimages")[0].InnerText;

                lst = doc.GetElementsByTagName("paging");
                for (int i = 0; i < lst[0].ChildNodes.Count; i++)
                {
                    GlobalVariables.g_colPaging.Add(lst[0].ChildNodes[i].InnerText);
                }

                lst = doc.GetElementsByTagName("usingajax");
                for (int i = 0; i < lst[0].ChildNodes.Count; i++)
                {
                    GlobalVariables.g_colUsingAjax.Add(lst[0].ChildNodes[i].InnerText);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WriteData(string content, string path)
        {
            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.Write(content);
            }
        }
        #endregion
    }
}
