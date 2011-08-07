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
            string errText = "";
            _adminCore.BasePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            errText = _adminCore.Generate();

            if (errText == "")
            {
                Console.WriteLine("Generated");
            }
            else
            {
                Console.WriteLine(errText);
            }
        }
    }
}
