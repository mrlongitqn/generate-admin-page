using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    /// <summary>
    /// Singleton class that is used to interact with database
    /// </summary>
    public class DataContext
    {
        private static AdminDataContext _dataContext = null;

        static DataContext()
        {
            _dataContext = new AdminDataContext();
        }

        public static AdminDataContext Instance
        {
            get { return _dataContext; }
        }
    }
}
