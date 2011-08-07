using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IBaseModel
    {
        string GenerateNameSpace(string nameSpace);
        string GenerateClasses(DataBase _database, Table _tbl);
        string GenerateUsingRegion();
        string GenerateClass();
    }
}
