using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IView
    {
        void InitView(DataBase _database, Table _tbl);
        string GenerateView(EnumView _type);
    }
}
