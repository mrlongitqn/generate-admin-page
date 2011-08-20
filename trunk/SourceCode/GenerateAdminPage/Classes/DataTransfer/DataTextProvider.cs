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
        public bool LoadDBInfo()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(InputPath);
                XmlElement root = doc.DocumentElement;

                GlobalVariables.g_sDataSource = root.SelectSingleNode("datasource").InnerText;
                GlobalVariables.g_sDatabaseName = root.SelectSingleNode("databasename").InnerText;
                GlobalVariables.g_sNameSpace = root.SelectSingleNode("namespace").InnerText;
                GlobalVariables.g_sTableNguoiDung = root.SelectSingleNode("usertablename").InnerText;
                GlobalVariables.g_sSuperAdmin = root.SelectSingleNode("superadmin").InnerText.ToUpper();
                GlobalVariables.g_sAdmin = root.SelectSingleNode("admin").InnerText.ToUpper();
                GlobalVariables.g_DefaultGuid = Guid.Parse(root.SelectSingleNode("defaultguid").InnerText);
                GlobalVariables.g_sNoImages = root.SelectSingleNode("noimages").InnerText;

                var lst = root.SelectSingleNode("fck").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    GlobalVariables.g_colUsingFCK.Add(lst[i].Attributes["id"].Value, lst[i].Attributes["attr"].Value);
                }

                lst = root.SelectSingleNode("image").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    GlobalVariables.g_colTableHaveImage.Add(lst[i].Attributes["id"].Value, lst[i].Attributes["attr"].Value);
                }

                lst = root.SelectSingleNode("pkpaging").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    GlobalVariables.g_colPaging.Add(lst[i].InnerText);
                }

                lst = root.SelectSingleNode("ajax").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    GlobalVariables.g_colUsingAjax.Add(lst[i].InnerText);
                }

                lst = root.SelectSingleNode("detail").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    GlobalVariables.g_colViewDetail.Add(lst[i].InnerText);
                }

                lst = root.SelectSingleNode("fkpaging").SelectNodes("item");
                for (int i = 0; i < lst.Count; i++)
                {
                    var childFcks = lst[i].SelectNodes("fk");
                    for (int j = 0; j < childFcks.Count; j++)
                    {
                        GlobalVariables.g_colFKPaging.Add(lst[i].Attributes["id"].Value, childFcks[j].InnerText);
                    }
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
