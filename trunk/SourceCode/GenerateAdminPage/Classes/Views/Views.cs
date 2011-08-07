using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public enum EnumView
    {
        SELECT,
        TEMPLATE_LIST,
        TEMPLATE_EDIT_OR_DETAILOF
    }

    public abstract class Views: BaseGenerate, IView
    {
        public abstract void InitView(DataBase _database, Table _tbl);
        public abstract string GenerateView(EnumView _type);
    }
}
