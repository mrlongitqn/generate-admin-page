using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BK.Util;

namespace GenerateAdminPage.Classes
{
    public static class GlobalVariables
    {
        public static string g_sDataSource = "";
        public static string g_sDatabaseName = "";
        public static string g_sNameSpace = "";
        public static string g_sTableNguoiDung = "NguoiDung";
        public static string g_sSuperAdmin = "SuperAdmin";
        public static string g_sAdmin = "Admin";
        public static string g_ModelName = "";
        public static List<string> g_colEnumViewModel = new List<string>();
        public static IDictionary<string, string> g_colUsingFCK = new Dictionary<string, string>();
        public static IDictionary<string, string> g_colTableHaveImage = new Dictionary<string, string>();
        public static List<string> g_colPaging = new List<string>();
        public static List<string> g_colUsingAjax = new List<string>();
        public static List<string> g_colViewDetail = new List<string>();
        public static MultimapBK<string, string> g_colFKPaging = new MultimapBK<string, string>();
        public static Guid g_DefaultGuid;
        public static string g_sNoImages = "noimages.jpg";
    }
}
