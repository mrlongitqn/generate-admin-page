using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage
{
    #region Using
    using GenerateAdminPage.Classes;
    using System.Reflection;
    #endregion

    class Program
    {
        static AdminCore _adminCore = new AdminCore();
        static void Main(string[] args)
        {
            _adminCore.BasePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _adminCore.Generate();
        }
    }
}
